using Microsoft.AspNetCore.Components;
using Ray.Blog.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Blazor.Pages
{
    public partial class Comments
    {
        [Inject]
        ICommentsAppService CommentsAppService { get; set; }

        private IReadOnlyList<CommentDto> CommentList { get; set; } = new List<CommentDto>();

        private int PageSize { get; } = 1000;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetCommentsAsync();
        }

        private async Task GetCommentsAsync()
        {
            var result = await CommentsAppService.GetListAsync(
                new PagedAndSortedResultRequestDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            CommentList = result.Items;
            TotalCount = (int)result.TotalCount;
        }
    }
}
