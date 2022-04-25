using System;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.ThumbUps
{
    public class GetThumbUpListDto : PagedAndSortedResultRequestDto
    {
        public Guid? SourceId { get; set; }
    }
}
