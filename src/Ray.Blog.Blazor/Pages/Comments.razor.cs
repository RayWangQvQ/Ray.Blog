using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Ray.Blog.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Blazor.Pages;

public partial class Comments : BlogComponentBase
{
    [Parameter]
    public Guid PostId { get; set; }

    [Inject]
    ICommentsAppService CommentsAppService { get; set; }

    CreateCommentDto NewComment { get; set; } = new CreateCommentDto() { Text = "" };

    private IReadOnlyList<CommentDto> CommentList { get; set; } = new List<CommentDto>();

    private int PageSize { get; set; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; } = 1;
    private string CurrentSorting { get; set; }
    private int TotalCount { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetCommentsAsync();

        await base.OnInitializedAsync();
    }

    private async Task GetCommentsAsync()
    {
        var result = await CommentsAppService.GetListAsync(
            new GetCommentListDto
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting,

                PostId = this.PostId
            }
        );

        CommentList = result.Items;
        TotalCount = (int)result.TotalCount;
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

    private async Task OnShowSizeChange(PaginationEventArgs args)
    {
        var (current, pageSize) = args;

        this.PageSize = pageSize;
        this.CurrentPage=current;

        await GetCommentsAsync();
    }

    async Task OnChange(PaginationEventArgs args)
    {
        Console.WriteLine(args.Page);
        CurrentPage = args.Page;

        await GetCommentsAsync();
    }
}