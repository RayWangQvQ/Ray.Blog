﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ray.Blog.Comments
{
    public class Comment : FullAuditedAggregateRoot<Guid>
    {
        protected Comment()
        {
        }

        public Comment(Guid postId, string text, Guid? repliedCommentId = null)
        {
            PostId = postId;
            Text = text;
            RepliedCommentId = repliedCommentId;
        }

        public Guid PostId { get; set; }

        public Guid? RepliedCommentId { get; set; }

        public string Text { get; set; }
    }
}
