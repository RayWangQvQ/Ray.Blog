using System;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog.Blog.Repositories
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
    }
}