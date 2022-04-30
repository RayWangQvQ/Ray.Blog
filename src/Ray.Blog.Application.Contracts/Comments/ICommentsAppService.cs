using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Comments
{
    public interface ICommentsAppService : ICrudAppService<CommentDto, Guid,
        GetCommentListDto, CreateCommentDto>
    {
        Task<CommentDto> ThumbUpAsync(Guid commentId);
        Task<CommentDto> CancelThumbUpAsync(Guid commentId);
    }
}
