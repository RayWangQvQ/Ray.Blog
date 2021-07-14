﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace Ray.Blog.Comments
{
    public interface ICommentAppService : ICrudAppService<CommentDto, Guid,
        PagedAndSortedResultRequestDto, CreateCommentDto>
    {
    }
}
