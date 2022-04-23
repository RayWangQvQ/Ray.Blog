using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog.ThumbUps
{
    public interface IThumbUpRepository : IRepository<ThumbUp, Guid>
    {
        /// <summary>
        /// Get tag list by post id
        /// </summary>
        /// <returns></returns>
        Task<List<ThumbUp>> GetListAsync(Guid postId);
    }
}