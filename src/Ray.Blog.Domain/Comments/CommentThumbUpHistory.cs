using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Comments
{
    public class CommentThumbUpHistory : CreationAuditedEntity
    {
        public CommentThumbUpHistory(Guid commentId, Guid userId)
        {
            CommentId = commentId;
            UserId = userId;
        }

        public virtual Guid CommentId { get; set; }

        public virtual Guid UserId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { CommentId, UserId };
        }
    }
}
