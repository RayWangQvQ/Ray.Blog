using Ray.Blog.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog
{
    public class CommentAppService : CrudAppService<Comment, CommentDto, Guid,
        PagedAndSortedResultRequestDto, CreateCommentDto>,
        ICommentAppService
    {
        public CommentAppService(IRepository<Comment, Guid> repository) : base(repository)
        {
        }
    }
}
