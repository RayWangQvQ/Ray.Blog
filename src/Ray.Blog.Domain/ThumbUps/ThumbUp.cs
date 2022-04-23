using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ray.Blog.Posts;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.ThumbUps
{
    public class ThumbUp : FullAuditedAggregateRoot<Guid>
    {
        public virtual Guid PostId { get; set; }

        public Post Post { get; set; }
    }
}
