using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Ray.Blog.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ray.Blog.ThumbUps;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog
{
    [Authorize]
    public class ThumbUpAppService : CrudAppService<ThumbUp, ThumbUpDto, Guid, PagedAndSortedResultRequestDto>,
    IThumbUpAppService
    {
        public ThumbUpAppService(IRepository<ThumbUp, Guid> repository) : base(repository)
        {
        }

        [AllowAnonymous]
        public override async Task<PagedResultDto<ThumbUpDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }
    }
}
