using Ray.Blog.Tags;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Posts
{
    public class RelatePostTagDto : CreationAuditedEntityDto
    {
        public Guid TagId { get; set; }
    }
}
