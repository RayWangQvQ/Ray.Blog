using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Ray.Blog.Posts;
using Volo.Abp.Users;

namespace Ray.Blog.Blazor.Pages
{
    public partial class PostThumbUp : BlogComponentBase
    {
        [Parameter]
        public PostDto Post { get; set; } = new PostDto();

        [Parameter]
        public List<PostThumbUpHistoryDto> PostThumbUpHistories { get; set; }

        [Inject]
        public IPostsAppService PostsAppService { get; set; }

        [Inject]
        public ICurrentUser CurrentUser { get; set; }

        private bool IsCurrentUserThumbUped => Post.ThumbUpHistories.Any(x => x.UserId == CurrentUser.Id);

        protected IFluentBorder ThumbButtonBorder => IsCurrentUserThumbUped ? Border.Is1.Rounded.Success : Border.Is1.Rounded.Secondary;

        protected TextColor ThumbColor => IsCurrentUserThumbUped ? TextColor.Success : TextColor.Secondary;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        public async Task OnThumbUpClicked()
        {
            if (!IsCurrentUserThumbUped)
            {
                Post = await PostsAppService.ThumbUpAsync(Post.Id);
                return;
            }

            var id = Post.ThumbUpHistories.FirstOrDefault(x => x.PostId == this.Post.Id && x.UserId == CurrentUser.Id)?.Id;
            if (id.HasValue)
            {
                Post = await PostsAppService.CancelThumbUpAsync(Post.Id);
            }
        }
    }
}
