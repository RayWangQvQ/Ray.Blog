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
using Ray.Blog.Comments;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace Ray.Blog
{
    public class PostsAppService : CrudAppService<Post, PostDto, Guid,
        GetPostListDto, CreatePostDto>,
        IPostsAppService
    {
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<Tag, Guid> _tagRepository;
        private readonly IRepository<Comment, Guid> _commentRepository;

        public PostsAppService(
            IRepository<Post, Guid> repository,
            IRepository<Category, Guid> categoryRepository,
            IRepository<Tag, Guid> tagRepository, 
            IRepository<Comment, Guid> commentRepository) : base(repository)
        {
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _commentRepository = commentRepository;
        }

        [AllowAnonymous]
        public override async Task<PostDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            //var entity = await GetEntityByIdAsync(id);
            //Get the IQueryable<Book> from the repository
            IQueryable<Post> queryable = await Repository.WithDetailsAsync();

            //Execute the query and get the book with author
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(queryable);

            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Post), id);
            }

            var dto = await MapToGetOutputDtoAsync(queryResult);

            await SetTagsOfPost(dto, queryResult.RelatePostTags.Select(x => x.TagId));
            await SetCommentCountOfPost(dto);

            return dto;
        }

        [AllowAnonymous]
        public override async Task<PagedResultDto<PostDto>> GetListAsync(GetPostListDto input)
        {
            //Get the IQueryable<Book> from the repository
            IQueryable<Post> queryable = await Repository.WithDetailsAsync();

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

            //Paging
            var query = queryable
                .OrderBy(NormalizeSorting(input.Sorting))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            //Execute the query and get a list
            var queryResult = await AsyncExecuter.ToListAsync(query);


            //Convert the query result to a list of BookDto objects
            var dtoTasks = queryResult.Select(async x =>
            {
                var postDto = ObjectMapper.Map<Post, PostDto>(x);
                await SetTagsOfPost(postDto, x.RelatePostTags.Select(r => r.TagId));
                await SetCommentCountOfPost(postDto);
                return postDto;
            }).ToList();
            var dtos = await Task.WhenAll(dtoTasks);

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
            var categories = await _categoryRepository.GetListAsync();
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

        [Authorize]
        public async Task<PostDto> ThumbUpAsync(Guid postId)
        {
            var post = await Repository.GetAsync(postId);
            post.ThumbUp(CurrentUser.Id.Value);
            await Repository.UpdateAsync(post, true);
            return await MapToGetOutputDtoAsync(post);
        }

        [Authorize]
        public async Task<PostDto> CancelThumbUpAsync(Guid postId)
        {
            var post = await Repository.GetAsync(postId);
            post.CancelThumbUp(CurrentUser.Id.Value);
            post = await Repository.UpdateAsync(post, true);
            return await MapToGetOutputDtoAsync(post);
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

        private async Task SetCommentCountOfPost(PostDto postDto)
        {
            var count=await _commentRepository.CountAsync(x=>x.PostId==postDto.Id);
            postDto.CommentCount = count;
        }

        private static string NormalizeSorting(string sorting)
        {
            if (sorting.IsNullOrEmpty())
            {
                //return $"{nameof(Post.Title)}";
                return $"{nameof(Post.Title)}";
            }
            /*
            if (sorting.Contains("categoryName", StringComparison.OrdinalIgnoreCase))
            {
                return sorting.Replace(
                    "categoryName",
                    "category.Name",
                    StringComparison.OrdinalIgnoreCase
                );
            }
            */

            return sorting;
        }
    }
}
