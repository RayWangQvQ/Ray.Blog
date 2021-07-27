using Microsoft.AspNetCore.Authorization;
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
    public class PostAppService : CrudAppService<Post, PostDto, Guid,
        PagedAndSortedResultRequestDto, CreatePostDto>,
        IPostAppService
    {
        private readonly IRepository<Tag, Guid> _tagRepository;

        public PostAppService(
            IRepository<Post, Guid> repository,
            IRepository<Tag, Guid> tagRepository
            ) : base(repository)
        {
            _tagRepository = tagRepository;
        }

        [AllowAnonymous]
        public override async Task<PostDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var entity = await GetEntityByIdAsync(id);

            var dto = await MapToGetOutputDtoAsync(entity);

            await SetTagsOfPost(dto);

            return dto;

            return await base.GetAsync(id);
        }

        [AllowAnonymous]
        public override Task<PagedResultDto<PostDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return base.GetListAsync(input);
        }

        private async Task SetTagsOfPost(PostDto postDto)
        {
            var tagIds = postDto.RelatePostTags.Select(x => x.TagId);

            var tags = (await _tagRepository.GetQueryableAsync()).Where(t => tagIds.Contains(t.Id)).ToList();

            postDto.Tags = ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);
        }
    }
}
