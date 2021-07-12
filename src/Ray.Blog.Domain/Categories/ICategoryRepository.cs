using System;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog.Categories
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
    }
}