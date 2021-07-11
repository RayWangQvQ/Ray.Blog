using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog.Messages.Repositories
{
    public interface IMessageRepository : IRepository<Message, Guid>
    {
        /// <summary>
        /// Get the list of messages by paging.
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        Task<Tuple<int, List<Message>>> GetPagedListAsync(int skipCount, int maxResultCount);
    }
}