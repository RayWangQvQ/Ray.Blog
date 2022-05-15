using Microsoft.AspNetCore.Components;
using Ray.Blog.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Blazor.Pages
{
    public partial class Tags : BlogComponentBase
    {
        [Inject] ITagAppService TagAppService { get; set; }

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

        private Blazorise.Color GetRandomBadgeColor()
        {
            var dic = new Dictionary<int, string>()
            {
                {0, "primary"},
                {1, "secondary"},
                {2, "success"},
                {3, "danger"},
                {4, "warning"},
                {5, "info"},
                {6, "dark"},
                //{7, "light"},
                //{8, "link"},
            };

            var num = new Random().Next(0, 7);
            var random = dic[num];

            return new Color(random);
        }
    }
}
