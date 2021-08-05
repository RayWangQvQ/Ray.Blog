using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Posts
{
    public class TagLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
