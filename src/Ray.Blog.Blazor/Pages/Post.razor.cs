using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ray.Blog.Blazor.Pages
{
    public partial class Post
    {
        [Parameter]
        public Guid Id { get; set; }

        string markdownValue = "# EasyMDE \n Go ahead, play around with the editor! Be sure to check out **bold**, *italic*, [links](https://google.com) and all the other features. You can type the Markdown syntax, use the toolbar, or use shortcuts like `ctrl-b` or `cmd-b`.";

        string markdownHtml;

        protected override void OnInitialized()
        {
            markdownHtml = Markdig.Markdown.ToHtml(markdownValue ?? string.Empty);

            base.OnInitialized();
        }

        Task OnMarkdownValueChanged(string value)
        {
            markdownValue = value;

            markdownHtml = Markdig.Markdown.ToHtml(markdownValue ?? string.Empty);

            return Task.CompletedTask;
        }

    }
}
