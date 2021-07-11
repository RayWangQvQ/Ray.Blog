using Ray.Blog.Blog;
using Ray.Blog.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog
{
    public class PostAppService : CrudAppService<Post, PostDto, Guid, PagedAndSortedResultRequestDto>, IPostAppService
    {
        public PostAppService(IRepository<Post, Guid> repository) : base(repository)
        {
        }
    }
}
