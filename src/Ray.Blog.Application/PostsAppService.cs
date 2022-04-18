using Microsoft.AspNetCore.Authorization;
using Ray.Blog.Categories;
using Ray.Blog.Posts;
using Ray.Blog.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog
{
    public class PostsAppService : CrudAppService<Post, PostDto, Guid,
        GetPostListDto, CreatePostDto>,
        IPostsAppService
    {
        private readonly IRepository<Category, Guid> _categoryepository;
        private readonly IRepository<Tag, Guid> _tagRepository;

        public PostsAppService(
            IRepository<Post, Guid> repository,
            IRepository<Category, Guid> categoryepository,
            IRepository<Tag, Guid> tagRepository
            ) : base(repository)
        {
            _categoryepository = categoryepository;
            _tagRepository = tagRepository;
        }

        [AllowAnonymous]
        public override async Task<PostDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            //var entity = await GetEntityByIdAsync(id);
            //Get the IQueryable<Book> from the repository
            IQueryable<Post> queryable = await Repository.WithDetailsAsync(x => x.RelatePostTags);
            //Prepare a query to join books and authors
            var query = from post in queryable
                        join category in await _categoryepository.GetQueryableAsync() on post.CategoryId equals category.Id
                        where post.Id == id
                        select new { post, category };

            //Execute the query and get the book with author
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);

            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Post), id);
            }

            var dto = await MapToGetOutputDtoAsync(queryResult.post);
            dto.CategoryName = queryResult.category.Name;

            await SetTagsOfPost(dto, queryResult.post.RelatePostTags.Select(x => x.TagId));

            return dto;
        }

        [AllowAnonymous]
        public override async Task<PagedResultDto<PostDto>> GetListAsync(GetPostListDto input)
        {
            //Get the IQueryable<Book> from the repository
            IQueryable<Post> queryable = await Repository.WithDetailsAsync(x => x.RelatePostTags);

            //Filter
            if (!string.IsNullOrWhiteSpace(input.Title))
            {
                queryable = queryable.Where(x => x.Title.Contains(input.Title));
            }
            if (input.CategoryIds != null && input.CategoryIds.Any())
            {
                queryable = queryable.Where(x => input.CategoryIds.Contains(x.CategoryId));
            }
            if (input.TagIds != null && input.TagIds.Any())
            {
                foreach (var tagId in input.TagIds)
                {
                    queryable = queryable.Where(x => x.RelatePostTags.Any(r => r.TagId == tagId));
                }
            }

            //Prepare a query to join books and authors
            var query = from post in queryable
                        join category in await _categoryepository.GetQueryableAsync() on post.CategoryId equals category.Id
                        select new { post, category };

            //Paging
            query = query
                .OrderBy(NormalizeSorting(input.Sorting))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            //Execute the query and get a list
            var queryResult = await AsyncExecuter.ToListAsync(query);

            //Convert the query result to a list of BookDto objects
            var dtos = queryResult.Select(x =>
            {
                var bookDto = ObjectMapper.Map<Post, PostDto>(x.post);
                bookDto.CategoryName = x.category.Name;
                SetTagsOfPost(bookDto, x.post.RelatePostTags.Select(x => x.TagId)).Wait();
                return bookDto;
            }).ToList();

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<PostDto>(
                totalCount,
                dtos
            );
        }

        public override async Task<PostDto> CreateAsync(CreatePostDto input)
        {
            //return base.CreateAsync(input);

            await CheckCreatePolicyAsync();

            var entity = await MapToEntityAsync(input);

            TryToSetTenantId(entity);

            input.TagIds.ForEach(x => entity.AddTag(x));

            await Repository.InsertAsync(entity, autoSave: true);

            return await MapToGetOutputDtoAsync(entity);
        }

        public override async Task<PostDto> UpdateAsync(Guid id, CreatePostDto input)
        {
            //return base.UpdateAsync(id, input);

            await CheckUpdatePolicyAsync();

            var entity = await GetEntityByIdAsync(id);
            //TODO: Check if input has id different than given id and normalize if it's default value, throw ex otherwise
            await MapToEntityAsync(input, entity);

            entity.RelatePostTags.Clear();
            input.TagIds.ForEach(x => entity.AddTag(x));

            await Repository.UpdateAsync(entity, autoSave: true);

            return await MapToGetOutputDtoAsync(entity);
        }

        public async Task<ListResultDto<CategoryLookupDto>> GetCategoryLookupAsync()
        {
            var categories = await _categoryepository.GetListAsync();
            return new ListResultDto<CategoryLookupDto>(
                ObjectMapper.Map<List<Category>, List<CategoryLookupDto>>(categories)
            );
        }

        public async Task<ListResultDto<TagLookupDto>> GetTagLookupAsync()
        {
            var tags = await _tagRepository.GetListAsync();
            return new ListResultDto<TagLookupDto>(
                ObjectMapper.Map<List<Tag>, List<TagLookupDto>>(tags)
            );
        }

        protected override IQueryable<Post> ApplySorting(IQueryable<Post> query, GetPostListDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                input.Sorting = $"{nameof(Post.CreationTime)} desc";
            }
            return base.ApplySorting(query, input);
        }

        protected override async Task<IQueryable<Post>> CreateFilteredQueryAsync(GetPostListDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            if (string.IsNullOrWhiteSpace(input.Title))
            {
                query = query.Where(x => x.Title.Contains(input.Title));
            }

            return query;
        }

        private async Task SetTagsOfPost(PostDto postDto, IEnumerable<Guid> tagIds)
        {
            var tags = (await _tagRepository.GetQueryableAsync())
                .Where(t => tagIds.Contains(t.Id))
                .ToList();

            postDto.Tags = ObjectMapper.Map<List<Tag>, List<TagLookupDto>>(tags);
        }

        private static string NormalizeSorting(string sorting)
        {
            if (sorting.IsNullOrEmpty())
            {
                return $"post.{nameof(Post.Title)}";
            }

            if (sorting.Contains("categoryName", StringComparison.OrdinalIgnoreCase))
            {
                return sorting.Replace(
                    "categoryName",
                    "category.Name",
                    StringComparison.OrdinalIgnoreCase
                );
            }

            return $"book.{sorting}";
        }
    }
}
