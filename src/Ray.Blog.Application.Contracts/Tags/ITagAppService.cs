using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ray.Blog.Tags
{
    public interface ITagAppService : ICrudAppService<TagDto, Guid,
        PagedAndSortedResultRequestDto, CreateTagDto>
    {
    }
}
