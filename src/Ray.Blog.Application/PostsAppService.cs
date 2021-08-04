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
        PagedAndSortedResultRequestDto, CreatePostDto>,
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
            var queryable = await Repository.GetQueryableAsync();

            //Prepare a query to join books and authors
            var query = from post in queryable
                        join category in _categoryepository on post.CategoryId equals category.Id
                        where post.Id == id
                        select new { post, category };

            //Execute the query and get the book with author
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);

            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Post), id);
            }

            var dto = await MapToGetOutputDtoAsync(queryResult.post);

            await SetTagsOfPost(dto);

            dto.CategoryName = queryResult.category.Name;

            return dto;
        }

        public async Task<ListResultDto<CategoryLookupDto>> GetCategoryLookupDtoAsync()
        {
            var categories = await _categoryepository.GetListAsync();
            return new ListResultDto<CategoryLookupDto>(
                ObjectMapper.Map<List<Category>, List<CategoryLookupDto>>(categories)
            );
        }

        [AllowAnonymous]
        public override async Task<PagedResultDto<PostDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            //Get the IQueryable<Book> from the repository
            var queryable = await Repository.GetQueryableAsync();

            //Prepare a query to join books and authors
            var query = from post in queryable
                        join category in _categoryepository on post.CategoryId equals category.Id
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
                return bookDto;
            }).ToList();

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<PostDto>(
                totalCount,
                dtos
            );
        }

        private async Task SetTagsOfPost(PostDto postDto)
        {
            var tagIds = postDto.RelatePostTags.Select(x => x.TagId);

            var tags = (await _tagRepository.GetQueryableAsync()).Where(t => tagIds.Contains(t.Id)).ToList();

            postDto.Tags = ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);
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
