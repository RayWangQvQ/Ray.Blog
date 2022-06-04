using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Ray.Blog.Posts;
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
    public class TagAppService : CrudAppService<Tag, TagDto, Guid,
        PagedAndSortedResultRequestDto, CreateTagDto>,
        ITagAppService
    {
        private readonly IRepository<RelatePostTag> _relatePostTagRepository;

        public TagAppService(IRepository<Tag, Guid> repository,
            IRepository<RelatePostTag> relatePostTagRepository) : base(repository)
        {
            _relatePostTagRepository = relatePostTagRepository;
        }

        [AllowAnonymous]
        public override async Task<PagedResultDto<TagDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }

        public async Task<List<TagWithCountDto>> GetTagWithCountListAsync()
        {
            //https://stackoverflow.com/questions/695506/linq-left-join-group-by-and-count
            //https://github.com/dotnet/efcore/issues/12901
            var query = await ReadOnlyRepository.ToListAsync();
            var relatePostTagQuery = await _relatePostTagRepository.GetQueryableAsync();//.ToListAsync();

            var groupQuery = query
                .GroupJoin(relatePostTagQuery,
                    outerKeySelector: tag => tag.Id,
                    innerKeySelector: relatePostTag => relatePostTag.TagId,
                    resultSelector: (tag, relatePostTags) => new
                    {
                        tag,
                        Count = relatePostTags.Count()
                    }
                );
            var list = groupQuery.ToList().Select(t =>
            {
                var dto = ObjectMapper.Map<Tag, TagWithCountDto>(t.tag);
                dto.PostCount = t.Count;
                return dto;
             }).ToList();
            return list;
        }
    }
}
