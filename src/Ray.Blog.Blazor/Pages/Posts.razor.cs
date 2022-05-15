using AutoMapper;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Ray.Blog.Permissions;
using Ray.Blog.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ray.Blog.Categories;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Blazor.Pages
{
    public partial class Posts : BlogComponentBase
    {
        [Inject]
        IPostsAppService PostAppService { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public Guid? CategoryId { get; set; }

        [Parameter]
        public Guid? TagId { get; set; }

        private IReadOnlyList<PostDto> PostList { get; set; } = new List<PostDto>();

        IReadOnlyList<CategoryLookupDto> _categoryAllList = new List<CategoryLookupDto>();

        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }

        private bool CanCreateAuthor { get; set; }
        private bool CanEditAuthor { get; set; }
        private bool CanDeleteAuthor { get; set; }

        private CreatePostDto NewAuthor { get; set; } = new CreatePostDto();

        private Guid EditingAuthorId { get; set; }
        private CreatePostDto EditingAuthor { get; set; } = new CreatePostDto();

        private Modal CreateAuthorModal { get; set; }
        private Modal EditAuthorModal { get; set; }

        private Validations CreateValidationsRef;

        private Validations EditValidationsRef;

        protected override async Task OnInitializedAsync()
        {
            //获取类别下拉列表
            _categoryAllList = (await PostAppService.GetCategoryLookupAsync()).Items;

            await SetPermissionsAsync();
            await GetPostsAsync();
        }

        Task OnSelectedValueChanged(Guid? categoryId)
        {
            CategoryId = categoryId == Guid.Empty ? null : categoryId;

            return Task.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateAuthor = await AuthorizationService
                .IsGrantedAsync(BlogPermissions.Posts.Create);

            CanEditAuthor = await AuthorizationService
                .IsGrantedAsync(BlogPermissions.Posts.Edit);

            CanDeleteAuthor = await AuthorizationService
                .IsGrantedAsync(BlogPermissions.Posts.Delete);
        }

        private async Task GetPostsAsync()
        {
            GetPostListDto request = new GetPostListDto();
            request.MaxResultCount = PageSize;
            request.SkipCount = CurrentPage * PageSize;
            request.Sorting = CurrentSorting;

            if (CategoryId.HasValue)
                request.CategoryIds = new Guid[] { CategoryId.Value };

            var result = await PostAppService.GetListAsync(request);

            PostList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<PostDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetPostsAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async Task OnSearchButtonClickAsync()
        {
            await GetPostsAsync();
        }

        private void OpenCreateAuthorModal()
        {
            CreateValidationsRef.ClearAll();

            NewAuthor = new CreatePostDto();
            CreateAuthorModal.Show();
        }

        private void CloseCreateAuthorModal()
        {
            CreateAuthorModal.Hide();
        }

        private void OpenEditAuthorModal(PostDto author)
        {
            EditValidationsRef.ClearAll();

            EditingAuthorId = author.Id;
            EditingAuthor = ObjectMapper.Map<PostDto, CreatePostDto>(author);
            EditAuthorModal.Show();
        }

        private async Task DeleteAuthorAsync(PostDto author)
        {
            var confirmMessage = L["AuthorDeletionConfirmationMessage", author.Title];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await PostAppService.DeleteAsync(author.Id);
            await GetPostsAsync();
        }

        private void CloseEditAuthorModal()
        {
            EditAuthorModal.Hide();
        }

        private async Task CreateAuthorAsync()
        {
            if (await CreateValidationsRef.ValidateAll())
            {
                await PostAppService.CreateAsync(NewAuthor);
                await GetPostsAsync();
                await CreateAuthorModal.Hide();
            }
        }

        private async Task UpdateAuthorAsync()
        {
            if (await EditValidationsRef.ValidateAll())
            {
                await PostAppService.UpdateAsync(EditingAuthorId, EditingAuthor);
                await GetPostsAsync();
                await EditAuthorModal.Hide();
            }
        }
    }
}
