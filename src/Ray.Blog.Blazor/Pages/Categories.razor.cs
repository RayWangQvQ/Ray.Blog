using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Ray.Blog.Categories;
using Ray.Blog.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Blazor.Pages
{
    public partial class Categories: BlogComponentBase
    {
        [Inject]
        ICategoryAppService AuthorAppService { get; set; }

        private IReadOnlyList<CategoryDto> CategoryList { get; set; } = new List<CategoryDto>();

        //private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int PageSize { get; } = 1000;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }

        private bool CanCreateAuthor { get; set; }
        private bool CanEditAuthor { get; set; }
        private bool CanDeleteAuthor { get; set; }

        private CreateCategoryDto NewAuthor { get; set; }

        private Guid EditingAuthorId { get; set; }
        private CreateCategoryDto EditingAuthor { get; set; }

        private Modal CreateAuthorModal { get; set; }
        private Modal EditAuthorModal { get; set; }

        private Validations CreateValidationsRef;

        private Validations EditValidationsRef;

        public Categories()
        {
            NewAuthor = new CreateCategoryDto();
            EditingAuthor = new CreateCategoryDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetAuthorsAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateAuthor = await AuthorizationService
                .IsGrantedAsync(BlogPermissions.Categories.Create);

            CanEditAuthor = await AuthorizationService
                .IsGrantedAsync(BlogPermissions.Categories.Edit);

            CanDeleteAuthor = await AuthorizationService
                .IsGrantedAsync(BlogPermissions.Categories.Delete);
        }

        private async Task GetAuthorsAsync()
        {
            var result = await AuthorAppService.GetListAsync(
                new PagedAndSortedResultRequestDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            CategoryList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CategoryDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetAuthorsAsync();

            await InvokeAsync(StateHasChanged);
        }

        private void OpenCreateAuthorModal()
        {
            CreateValidationsRef.ClearAll();

            NewAuthor = new CreateCategoryDto();
            CreateAuthorModal.Show();
        }

        private void CloseCreateAuthorModal()
        {
            CreateAuthorModal.Hide();
        }

        private void OpenEditAuthorModal(CategoryDto author)
        {
            EditValidationsRef.ClearAll();

            EditingAuthorId = author.Id;
            EditingAuthor = ObjectMapper.Map<CategoryDto, CreateCategoryDto>(author);
            EditAuthorModal.Show();
        }

        private async Task DeleteAuthorAsync(CategoryDto author)
        {
            var confirmMessage = L["AuthorDeletionConfirmationMessage", author.Name];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await AuthorAppService.DeleteAsync(author.Id);
            await GetAuthorsAsync();
        }

        private void CloseEditAuthorModal()
        {
            EditAuthorModal.Hide();
        }

        private async Task CreateAuthorAsync()
        {
            if (await CreateValidationsRef.ValidateAll())
            {
                await AuthorAppService.CreateAsync(NewAuthor);
                await GetAuthorsAsync();
                await CreateAuthorModal.Hide();
            }
        }

        private async Task UpdateAuthorAsync()
        {
            if (await EditValidationsRef.ValidateAll())
            {
                await AuthorAppService.UpdateAsync(EditingAuthorId, EditingAuthor);
                await GetAuthorsAsync();
                await EditAuthorModal.Hide();
            }
        }
    }
}
