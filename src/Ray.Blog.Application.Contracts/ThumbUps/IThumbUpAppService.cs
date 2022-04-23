using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ray.Blog.ThumbUps
{
    public interface IThumbUpAppService : ICrudAppService<ThumbUpDto, Guid, PagedAndSortedResultRequestDto>
    {
    }
}
