using Ray.Blog.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace Ray.Blog
{
    public class CommentsAppService : CrudAppService<Comment, CommentDto, Guid,
        GetCommentListDto, CreateCommentDto>,
        ICommentsAppService
    {
        private readonly ICurrentUser _currentUser;

        public CommentsAppService(IRepository<Comment, Guid> repository, ICurrentUser currentUser) : base(repository)
        {
            _currentUser = currentUser;
        }

        public override async Task<PagedResultDto<CommentDto>> GetListAsync(GetCommentListDto input)
        {
            /*
            await CheckGetListPolicyAsync();

            var query = await CreateFilteredQueryAsync(input);

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncExecuter.ToListAsync(query);
            var entityDtos = await MapToGetListOutputDtosAsync(entities);
            return new PagedResultDto<CommentDto>(
                totalCount,
                entityDtos
            );
            */

            return await base.GetListAsync(input);
        }

        protected override async Task<IQueryable<Comment>> CreateFilteredQueryAsync(GetCommentListDto input)
        {
            IQueryable<Comment> query = await Repository.WithDetailsAsync();
            if (input.PostId.HasValue)
            {
                query = query.Where(x => x.PostId == input.PostId);
            }

            return query;
        }

        protected override IQueryable<Comment> ApplySorting(IQueryable<Comment> query, GetCommentListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Comment.CreationTime);
            }
            return base.ApplySorting(query, input);
        }

        [Authorize]
        public async Task<CommentDto> ThumbUpAsync(Guid commentId)
        {
            var comment = await Repository.GetAsync(commentId);
            comment.ThumbUp(_currentUser.Id.Value);
            await Repository.UpdateAsync(comment, true);
            return await MapToGetOutputDtoAsync(comment);
        }

        [Authorize]
        public async Task<CommentDto> CancelThumbUpAsync(Guid commentId)
        {
            var comment = await Repository.GetAsync(commentId);
            comment.CancelThumbUp(_currentUser.Id.Value);
            comment = await Repository.UpdateAsync(comment, true);
            return await MapToGetOutputDtoAsync(comment);
        }
    }
}
