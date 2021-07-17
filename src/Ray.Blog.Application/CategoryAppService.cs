using Ray.Blog.Categories;
using Ray.Blog.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog
{
    public class CategoryAppService : CrudAppService<Category, CategoryDto, Guid,
        PagedAndSortedResultRequestDto, CreateCategoryDto>,
        ICategoryAppService
    {
        public CategoryAppService(IRepository<Category, Guid> repository) : base(repository)
        {
            GetPolicyName = BlogPermissions.Categories.Delete;
            GetListPolicyName = BlogPermissions.Categories.Default;
            CreatePolicyName = BlogPermissions.Categories.Create;
            UpdatePolicyName = BlogPermissions.Categories.Edit;
            DeletePolicyName = BlogPermissions.Categories.Delete;
        }

        public override Task<CategoryDto> CreateAsync(CreateCategoryDto input)
        {
            return base.CreateAsync(input);
        }
    }
}
