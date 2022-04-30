using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Comments
{
    public class CommentDto : FullAuditedEntityDto<Guid>
    {
        public Guid PostId { get; set; }

        public Guid? RepliedCommentId { get; set; }

        public string Text { get; set; }

        public List<CommentThumbUpHistoryDto> ThumbUpHistories { get; set; } = new();
    }
}
