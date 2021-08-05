using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ray.Blog.Posts
{
    public interface IPostsAppService : ICrudAppService<PostDto, Guid, PagedAndSortedResultRequestDto, CreatePostDto>
    {
        Task<ListResultDto<CategoryLookupDto>> GetCategoryLookupAsync();

        Task<ListResultDto<TagLookupDto>> GetTagLookupAsync();
    }
}
