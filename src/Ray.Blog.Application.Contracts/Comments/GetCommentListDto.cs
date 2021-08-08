using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Comments
{
    public class GetCommentListDto : PagedAndSortedResultRequestDto
    {
        public Guid? PostId { get; set; }
    }
}
