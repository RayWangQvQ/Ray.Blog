using Blazorise;
using Ray.Blog.Permissions;
using Ray.Blog.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ray.Blog.Blazor.Pages.Admin
{
    public partial class AdminPosts
    {
        IReadOnlyList<CategoryLookupDto> categoryAllList = Array.Empty<CategoryLookupDto>();
        IReadOnlyList<TagLookupDto> tagAllList = Array.Empty<TagLookupDto>();

        public AdminPosts()
        {
            CreatePolicyName = BlogPermissions.Posts.Create;
            UpdatePolicyName = BlogPermissions.Posts.Edit;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //获取类别下拉列表
            categoryAllList = (await AppService.GetCategoryLookupAsync()).Items;

            //获取标签列表
            tagAllList = (await AppService.GetTagLookupAsync()).Items;
        }

        private TagLookupDto SelectedTag { get; set; }

        protected TagLookupDto GetTagById(Guid tagId)
        {
            return tagAllList.FirstOrDefault(x => x.Id == tagId);
        }

        protected Color GetRandomColor()
        {
            Color[] enums = Enum.GetValues(typeof(Color)) as Color[];
            Random random = new();
            return enums[random.Next(0, enums.Length)];
        }
    }
}
