using Ray.Blog.Posts;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Categories
{
    public class Category : FullAuditedAggregateRoot<Guid>
    {
        protected Category()
        {
        }

        public Category(string name, string alias, string picUrl)
        {
            Name = name;
            Alias = alias;
            PicUrl = picUrl;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string PicUrl { get; set; }
    }
}