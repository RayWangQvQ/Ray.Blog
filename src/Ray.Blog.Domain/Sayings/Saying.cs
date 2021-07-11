using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Sayings
{
    public class Saying : FullAuditedAggregateRoot<Guid>
    {
        public string Content { get; set; }
    }
}