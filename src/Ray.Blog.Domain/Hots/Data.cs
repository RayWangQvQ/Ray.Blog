using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Values;

namespace Ray.Blog.Hots
{
    public class Data : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }

        public string Url { get; set; }
    }
}
