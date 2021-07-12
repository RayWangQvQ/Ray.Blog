using System;
using System.Collections.Generic;
using Ray.Blog.Categories;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Posts
{
    public class Post : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public virtual string Author { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// Markdown
        /// </summary>
        public virtual string Markdown { get; set; }

        public virtual Guid CategoryId { get; set; }

        /// <summary>
        /// 标签列表
        /// </summary>
        public virtual List<RelatePostTag> RelatePostTags { get; protected set; }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="tagId"></param>
        public virtual void AddTag(Guid tagId)
        {
            RelatePostTags.Add(new RelatePostTag(Id, tagId));
        }

        /// <summary>
        /// 移除所有标签
        /// </summary>
        /// <param name="tagId"></param>
        public virtual void RemoveTag(Guid tagId)
        {
            RelatePostTags.RemoveAll(t => t.TagId == tagId);
        }
    }
}