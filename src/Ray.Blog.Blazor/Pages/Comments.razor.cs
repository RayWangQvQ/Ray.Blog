using Blazorise;
using Blazorise.DataGrid;
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
        [Parameter]
        public Guid PostId { get; set; }

        [Inject]
        ICommentsAppService CommentsAppService { get; set; }

        [Inject]
        public INotificationService NotificationService { get; set; }

        CreateCommentDto NewComment { get; set; } = new CreateCommentDto() { Text = "" };

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

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CommentDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.Direction != SortDirection.None)
                .Select(c => c.Field + (c.Direction == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetCommentsAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async Task OnAddCommentButtonClickedAsync()
        {
            NewComment.PostId = this.PostId;
            await CommentsAppService.CreateAsync(NewComment);
            await NotificationService.Success("This is a simple notification message!", "Hello");

            NewComment = new CreateCommentDto() { Text = "" };
        }
    }
}
