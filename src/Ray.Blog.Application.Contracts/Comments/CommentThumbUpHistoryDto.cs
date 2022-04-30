using System;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Comments
{
    public class CommentThumbUpHistoryDto : CreationAuditedEntityDto<Guid>
    {
        public virtual Guid CommentId { get; set; }

        public virtual Guid UserId { get; set; }
    }
}
