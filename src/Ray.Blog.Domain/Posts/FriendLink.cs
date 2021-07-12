using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Posts
{
    public class FriendLink : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public string Url { get; set; }
    }
}