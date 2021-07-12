using Ray.Blog.Tags;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Posts
{
    public class RelatePostTagDto : AuditedEntityDto<Guid>
    {
        public Guid TagId { get; set; }
        public TagDto Tag { get; }
    }
}
