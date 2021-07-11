using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Messages
{
    public class Message : FullAuditedAggregateRoot<Guid>
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public string Content { get; set; }

        public Guid? ReplyMessageId { get; set; }
    }
}