using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Ray.Blog.ThumbUps;

namespace Ray.Blog.ThumbUps
{
    public class ThumbUpDto : AuditedEntityDto<Guid>
    {
        public ThumbUpSourceType SourceType { get; set; }

        public Guid SourceId { get; set; }
    }
}
