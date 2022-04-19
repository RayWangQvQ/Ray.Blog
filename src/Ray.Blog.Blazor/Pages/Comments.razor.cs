using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Ray.Blog.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Blazor.Pages;

public partial class Comments: BlogComponentBase
{
    [Parameter]
    public Guid PostId { get; set; }

    [Inject]
    ICommentsAppService CommentsAppService { get; set; }

    CreateCommentDto NewComment { get; set; } = new CreateCommentDto() { Text = "" };

    private IReadOnlyList<CommentDto> CommentList { get; set; } = new List<CommentDto>();

    private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
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
            new GetCommentListDto
            {
                MaxResultCount = PageSize,
                SkipCount = CurrentPage * PageSize,
                Sorting = CurrentSorting,

                PostId = this.PostId
            }
        );

        CommentList = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CommentDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page - 1;

        await GetCommentsAsync();

        await InvokeAsync(StateHasChanged);
    }

    private async Task OnAddCommentButtonClickedAsync()
    {
        //新增评论
        NewComment.PostId = this.PostId;
        await CommentsAppService.CreateAsync(NewComment);

        //弹提示框
        await this.Notify.Success("评论发送成功");

        //刷新评论列表
        NewComment = new CreateCommentDto() { Text = "" };
        await GetCommentsAsync();
    }
}