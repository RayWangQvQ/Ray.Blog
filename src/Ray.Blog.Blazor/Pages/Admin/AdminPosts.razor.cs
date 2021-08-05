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
            DeletePolicyName = BlogPermissions.Posts.Delete;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            categoryAllList = (await AppService.GetCategoryLookupAsync()).Items;

            tagAllList = (await AppService.GetTagLookupAsync()).Items;
        }

        private TagLookupDto Value { get; set; }

        public IList<TagLookupDto> SelectedTags { get; set; }

        void MySearchHandler(TagLookupDto newValue)
        {
            SelectedTags.Add(newValue);
        }

        protected override async Task OnCreatingEntityAsync()
        {
            this.NewEntity.TagIds = SelectedTags.Select(x => x.Id).ToList();

            await base.OnCreatingEntityAsync();
        }

        protected override Task CloseCreateModalAsync()
        {
            SelectedTags.Clear();
            return base.CloseCreateModalAsync();
        }

        protected override async Task OnUpdatingEntityAsync()
        {
            this.EditingEntity.TagIds = SelectedTags.Select(x => x.Id).ToList();

            await base.OnUpdatingEntityAsync();
        }

        protected override async Task OpenEditModalAsync(PostDto entity)
        {
            SelectedTags = entity.Tags;
            await base.OpenEditModalAsync(entity);
        }

        protected override Task CloseEditModalAsync()
        {
            SelectedTags.Clear();
            return base.CloseEditModalAsync();
        }
    }
}
