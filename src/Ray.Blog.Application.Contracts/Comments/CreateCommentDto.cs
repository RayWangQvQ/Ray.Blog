using System;
using System.Collections.Generic;
using System.Text;

namespace Ray.Blog.Comments
{
    public class CreateCommentDto
    {
        public Guid PostId { get; set; }

        public Guid? RepliedCommentId { get; set; }

        public string Text { get; set; }
    }
}
