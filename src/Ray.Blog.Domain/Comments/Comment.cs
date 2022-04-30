using System;
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

        public virtual List<CommentThumbUpHistory> ThumbUpHistories { get; protected set; } = new List<CommentThumbUpHistory>();

        public virtual void ThumbUp(Guid userId)
        {
            ThumbUpHistories.AddIfNotContains(x => x.UserId == userId,
                () => new CommentThumbUpHistory(Id, userId));
        }

        public virtual void CancelThumbUp(Guid userId)
        {
            ThumbUpHistories.RemoveAll(x => x.CommentId == Id && x.UserId == userId);
        }
    }
}
