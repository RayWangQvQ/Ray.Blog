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
    public class ThumbUpAppService : CrudAppService<ThumbUp, ThumbUpDto, Guid, GetThumbUpListDto>,
    IThumbUpAppService
    {
        public ThumbUpAppService(IRepository<ThumbUp, Guid> repository) : base(repository)
        {
        }

        [AllowAnonymous]
        public override async Task<PagedResultDto<ThumbUpDto>> GetListAsync(GetThumbUpListDto input)
        {
            return await base.GetListAsync(input);
        }

        protected override async Task<IQueryable<ThumbUp>> CreateFilteredQueryAsync(GetThumbUpListDto input)
        {
            var query= await base.CreateFilteredQueryAsync(input);

            if (input.SourceId.HasValue)
            {
                query = query.Where(x => x.SourceId == input.SourceId.Value);
            }

            return query;
        }
    }
}
