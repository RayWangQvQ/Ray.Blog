using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Ray.Blog.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog
{
    //[Authorize("Blog_Tag")]
    public class TagAppService : CrudAppService<Tag, TagDto, Guid, PagedAndSortedResultRequestDto>, ITagAppService
    {
        public TagAppService(IRepository<Tag, Guid> repository) : base(repository)
        {
        }

        [AllowAnonymous]
        public override async Task<PagedResultDto<TagDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }

        [Authorize("Blog_Tag_Create")]
        public override async Task<TagDto> CreateAsync(TagDto input)
        {
            return await base.CreateAsync(input);
        }
    }
}
