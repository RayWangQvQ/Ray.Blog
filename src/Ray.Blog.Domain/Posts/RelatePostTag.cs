using System;
using Ray.Blog.Tags;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Posts
{
    public class RelatePostTag : CreationAuditedEntity
    {
        protected RelatePostTag()
        {
        }
        public RelatePostTag(Guid postId, Guid tagId)
        {
            PostId = postId;
            TagId = tagId;
        }

        public virtual Guid PostId { get; protected set; }

        public virtual Guid TagId { get; protected set; }

        public override object[] GetKeys()
        {
            return new object[] { PostId, TagId };
        }
    }
}
