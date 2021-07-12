using System;
using Ray.Blog.Tags;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Posts
{
    public class RelatePostTag : FullAuditedEntity<Guid>
    {
        protected RelatePostTag()
        {
        }
        public RelatePostTag(Guid postId, Guid tagId)
        {
            PostId = postId;
            TagId = tagId;
        }

        public Guid PostId { get; protected set; }
        public Post Post { get; set; }

        public Guid TagId { get; protected set; }
        public Tag Tag { get; set; }
    }
}
