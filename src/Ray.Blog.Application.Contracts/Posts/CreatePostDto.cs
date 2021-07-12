using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Posts
{
    public class CreatePostDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
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
        /// 标签列表
        /// </summary>
        public List<CreateRelatePostTagDto> RelatePostTags { get; set; }
    }

    public class CreateRelatePostTagDto
    {
        public Guid TagId { get; set; }
    }
}
