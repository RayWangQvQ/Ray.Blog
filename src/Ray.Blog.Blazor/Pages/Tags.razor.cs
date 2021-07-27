using Microsoft.AspNetCore.Components;
using Ray.Blog.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Blazor.Pages
{
    public partial class Tags
    {
        [Inject]
        ITagAppService TagAppService { get; set; }

        private IReadOnlyList<TagDto> TagList { get; set; } = new List<TagDto>();

        private int PageSize { get; } = 1000;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetTagsAsync();
        }

        private async Task GetTagsAsync()
        {
            var result = await TagAppService.GetListAsync(
                new PagedAndSortedResultRequestDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            TagList = result.Items;
            TotalCount = (int)result.TotalCount;
        }
    }
}
