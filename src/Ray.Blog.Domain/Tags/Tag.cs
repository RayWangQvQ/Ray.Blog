using System;
using System.Collections.Generic;
using Ray.Blog.Posts;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Tags
{
    public class Tag : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public virtual string Alias { get; set; }

        public virtual List<RelatePostTag> RelatePostTags { get; set; }
    }
}