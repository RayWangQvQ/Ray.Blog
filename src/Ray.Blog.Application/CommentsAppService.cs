using Ray.Blog.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Ray.Blog
{
    public class CommentsAppService : CrudAppService<Comment, CommentDto, Guid,
        GetCommentListDto, CreateCommentDto>,
        ICommentsAppService
    {
        public CommentsAppService(IRepository<Comment, Guid> repository) : base(repository)
        {
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
            IQueryable<Comment> query = await base.CreateFilteredQueryAsync(input);

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
    }
}
