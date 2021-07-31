using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ray.Blog.Posts
{
    public interface IPostsAppService : ICrudAppService<PostDto, Guid, PagedAndSortedResultRequestDto, CreatePostDto>
    {
    }
}
