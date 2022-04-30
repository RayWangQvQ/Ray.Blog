using System;
using Ray.Blog.ThumbUps;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Posts
{
    public class PostThumbUpHistoryDto : CreationAuditedEntityDto<Guid>
    {
        public virtual Guid PostId { get; set; }

        public virtual Guid UserId { get; set; }
    }
}
