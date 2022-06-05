using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ray.Blog.Categories;
using Ray.Blog.Comments;
using Ray.Blog.Posts;
using Ray.Blog.Tags;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Ray.Blog.Data
{
    public class BlogDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<IdentityUser, Guid> _identityUseRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<Tag, Guid> _tagRepository;
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly IRepository<Comment, Guid> _commentRepository;

        public BlogDataSeedContributor(
            IRepository<IdentityUser, Guid> identityUseRepository,
            IRepository<Category, Guid> categoryRepository,
            IRepository<Tag, Guid> tagRepository,
            IRepository<Post, Guid> postRepository,
            IRepository<Comment, Guid> commentRepository
            )
        {
            _identityUseRepository = identityUseRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            IdentityUser user = await _identityUseRepository.FirstOrDefaultAsync();

            //分类
            Category sketchCategory = await _categoryRepository.FirstOrDefaultAsync(x => x.Name == "Sketch");
            if (sketchCategory == null)
            {
                sketchCategory = await _categoryRepository.InsertAsync(new Category("Sketch", "随想", ""), true);
            }

            //标签
            Tag dotNetTag = await _tagRepository.FirstOrDefaultAsync(x => x.Name == "DotNet");
            if (dotNetTag == null)
            {
                dotNetTag = await _tagRepository.InsertAsync(new Tag("DotNet", "DotNet"), true);
            }
            Tag dddTag = await _tagRepository.FirstOrDefaultAsync(x => x.Name == "DDD");
            if (dotNetTag == null)
            {
                dddTag = await _tagRepository.InsertAsync(new Tag("DDD", "DDD"), true);
            }
            Tag otherTag = await _tagRepository.FirstOrDefaultAsync(x => x.Name == "Other");
            if (otherTag == null)
            {
                otherTag = await _tagRepository.InsertAsync(new Tag("Other", "其他"), true);
            }

            //博文
            Post post = await _postRepository.FirstOrDefaultAsync(x => x.Title == "Hello word!");
            if (post == null)
            {
                post = new Post(sketchCategory.Id, "Hello word!");
                post.Markdown = "这是我的第一篇博文，你好世界！";
                post = await _postRepository.InsertAsync(post, true);

                post.AddTag(otherTag.Id);
                post.ThumbUp(user.Id);
                await _postRepository.UpdateAsync(post, true);
            }

            //评论
            if (await _commentRepository.CountAsync(x=>x.PostId== post.Id) == 0)
            {
                await _commentRepository.InsertAsync(new Comment(post.Id, "这是第一条评论~"), true);

                var comment = await _commentRepository.InsertAsync(new Comment(post.Id, "评论也可以点赞哦"), true);
                comment.ThumbUp(user.Id);
                await _commentRepository.UpdateAsync(comment);
            }
        }
    }
}
