using Ray.Blog.Categories;
using Ray.Blog.Tags;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Posts
{
    public class PostDto : AuditedEntityDto<Guid>
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

        /*
        public CategoryDto Category { get; set; }
        public string CategoryName => Category.Name;
        */

        public List<TagLookupDto> Tags { get; set; } = new();

        public List<PostThumbUpHistoryDto> ThumbUpHistories { get; set; } = new();
    }
}
