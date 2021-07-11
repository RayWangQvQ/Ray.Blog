using System;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog.Blog.Repositories
{
    public interface IFriendLinkRepository : IRepository<FriendLink, Guid>
    {
    }
}