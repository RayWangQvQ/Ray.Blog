using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Ray.Blog.Comments;
using Ray.Blog.Posts;
using Volo.Abp.Users;

namespace Ray.Blog.Blazor.Pages
{
    public partial class CommentThumbUp : BlogComponentBase
    {
        [Parameter]
        public CommentDto Comment { get; set; } = new();

        [Inject]
        public ICommentsAppService CommentsAppService { get; set; }

        protected int Count => Comment.ThumbUpHistories.Count;
        protected string CountStr => Count == 0 ? "  " : $"{Count}";

        private bool IsCurrentUserThumbUped => Comment.ThumbUpHistories.Any(x => x.UserId == CurrentUser.Id);

        protected TextColor ThumbColor => IsCurrentUserThumbUped ? TextColor.Success : TextColor.Secondary;

        protected bool ButtonActive { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        public async Task OnThumbUpClicked()
        {
            if (!IsCurrentUserThumbUped)
            {
                ButtonActive = true;
                Comment.ThumbUpHistories = (await CommentsAppService.ThumbUpAsync(Comment.Id)).ThumbUpHistories;
                ButtonActive = false;
                return;
            }

            var id = Comment.ThumbUpHistories.FirstOrDefault(x => x.CommentId == this.Comment.Id && x.UserId == CurrentUser.Id)?.Id;
            if (id.HasValue)
            {
                ButtonActive = true;
                Comment.ThumbUpHistories = (await CommentsAppService.CancelThumbUpAsync(Comment.Id)).ThumbUpHistories;
                ButtonActive = false;
            }
        }
    }
}