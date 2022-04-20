using Microsoft.AspNetCore.Components;
using Ray.Blog.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ray.Blog.Blazor.Pages
{
    public partial class Post : BlogComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        IPostsAppService PostAppService { get; set; }

        string markdownHtml;

        PostDto PostDto { get; set; } = new PostDto();

        public Post()
        {
            PostDto = new PostDto();
        }

        protected override async Task OnInitializedAsync()
        {
            //获取详情
            PostDto = await PostAppService.GetAsync(Id);

            markdownHtml = Markdig.Markdown.ToHtml(PostDto.Markdown ?? string.Empty);

            await base.OnInitializedAsync();
        }
    }
}
