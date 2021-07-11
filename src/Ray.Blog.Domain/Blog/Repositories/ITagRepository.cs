using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog.Blog.Repositories
{
    public interface ITagRepository : IRepository<Tag, Guid>
    {
        /// <summary>
        /// Get tag list by names
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        Task<List<Tag>> GetListAsync(List<string> names);
    }
}