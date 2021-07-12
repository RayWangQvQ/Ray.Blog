using System;
using System.Collections.Generic;
using Ray.Blog.Posts;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Tags
{
    public class Tag : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        public List<RelatePostTag> RelatePostTags { get; set; }
    }
}