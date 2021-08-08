using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Posts
{
    public class GetPostListDto : PagedAndSortedResultRequestDto
    {
        public string Title { get; set; }
    }
}
