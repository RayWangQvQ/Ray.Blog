using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog.Hots.Repositories
{
    public interface IHotRepository : IRepository<Hot, Guid>
    {
        /// <summary>
        /// Get the list of sources.
        /// </summary>
        /// <returns></returns>
        Task<List<Hot>> GetSourcesAsync();
    }
}