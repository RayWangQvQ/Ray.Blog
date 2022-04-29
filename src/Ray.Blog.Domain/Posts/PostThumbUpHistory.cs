using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ray.Blog.ThumbUps;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Posts
{
    public class PostThumbUpHistory : CreationAuditedEntity
    {
        public PostThumbUpHistory(Guid postId, Guid userId)
        {
            PostId = postId;
            UserId = userId;
        }

        public virtual Guid PostId { get; set; }

        public virtual Guid UserId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { PostId, UserId };
        }
    }
}
