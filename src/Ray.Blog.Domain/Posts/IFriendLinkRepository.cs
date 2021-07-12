using System;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog.Posts
{
    public interface IFriendLinkRepository : IRepository<FriendLink, Guid>
    {
    }
}