using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Blog
{
    public class Post : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Markdown
        /// </summary>
        public string Markdown { get; set; }

        public Guid CategoryId { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// 标签列表
        /// </summary>
        public List<RelatePostTag> RelatePostTags { get; set; }
    }
}