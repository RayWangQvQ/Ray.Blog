using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Blog
{
    public class Category : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
    }
}