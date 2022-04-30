using System;
using System.Collections.Generic;
using Ray.Blog.Categories;
using Ray.Blog.ThumbUps;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Posts
{
    public class Post : FullAuditedAggregateRoot<Guid>
    {
        protected Post()
        {
        }

        public Post(Guid categoryId, string title)
        {
            CategoryId = categoryId;
            Title = title;
        }

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

        /// <summary>
        /// 分类Id
        /// （根据DDD，聚合根不要互相引用，但可以放射外键，所以不要加Category）
        /// </summary>
        public virtual Guid CategoryId { get; set; }

        /// <summary>
        /// 标签列表
        /// </summary>
        public virtual List<RelatePostTag> RelatePostTags { get; protected set; } = new List<RelatePostTag>();

        public virtual List<PostThumbUpHistory> ThumbUpHistories { get; protected set; } = new List<PostThumbUpHistory>();
        public int ThumbUpCount => ThumbUpHistories.Count;

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="tagId"></param>
        public virtual void AddTag(Guid tagId)
        {
            RelatePostTags.Add(new RelatePostTag(Id, tagId));
        }

        /// <summary>
        /// 移除标签
        /// </summary>
        /// <param name="tagId"></param>
        public virtual void RemoveTag(Guid tagId)
        {
            RelatePostTags.RemoveAll(t => t.TagId == tagId);
        }

        public virtual void ThumbUp(Guid userId)
        {
            ThumbUpHistories.AddIfNotContains(x => x.UserId == userId, 
                () => new PostThumbUpHistory(Id, userId));
        }

        public virtual void CancelThumbUp(Guid userId)
        {
            ThumbUpHistories.RemoveAll(x => x.PostId == Id && x.CreatorId == userId);
        }
    }
}