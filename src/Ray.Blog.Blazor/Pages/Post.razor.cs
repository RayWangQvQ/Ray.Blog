using Microsoft.AspNetCore.Components;
using Ray.Blog.Posts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Markdig;
using Volo.Abp.Users;

namespace Ray.Blog.Blazor.Pages
{
    public partial class Post : BlogComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        IPostsAppService PostAppService { get; set; }



        [Inject]
        private ICurrentUser CurrentUser { get; set; }

        string _markdownHtml;

        PostDto PostDto { get; set; } = new PostDto();

        private IFluentBorderColorWithSide _thumbButtonBorder = Border.Is1.Rounded.Secondary;
        private bool _isThumbUped => PostDto.ThumbUps.Any(x => x.CreatorId == CurrentUser.Id);

        public Post()
        {
            PostDto = new PostDto();
        }

        protected override async Task OnInitializedAsync()
        {
            //获取详情
            PostDto = await PostAppService.GetAsync(Id);
            var markdown = PostDto.Markdown ?? string.Empty;

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            _markdownHtml = Markdown.ToHtml(markdown, pipeline);

            if (_isThumbUped)
            {
                _thumbButtonBorder = Border.Is1.Rounded.Success;
            }

            await base.OnInitializedAsync();
        }
    }
}
