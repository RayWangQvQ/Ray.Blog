using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.ThumbUps
{
    public class ThumbUpDto : AuditedEntityDto<Guid>
    {
        public Guid PostId { get; set; }
    }
}
