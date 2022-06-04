using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Tags
{
    public class TagWithCountDto : TagDto
    {
        /// <summary>
        /// 文章总数
        /// </summary>
        public int PostCount { get; set; }
    }
}
