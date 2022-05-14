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
            var user = await _identityUseRepository.FirstOrDefaultAsync();

            //分类
            Category foodCategory = await _categoryRepository.FirstOrDefaultAsync(x => x.Name == "Food");
            if (foodCategory == null)
            {
                foodCategory = await _categoryRepository.InsertAsync(new Category("Food", "美食区", ""), true);
            }

            //标签
            Tag dotNetTag = await _tagRepository.FirstOrDefaultAsync(x => x.Name == "DotNet");
            if (dotNetTag == null)
            {
                dotNetTag = await _tagRepository.InsertAsync(new Tag("DotNet", "DotNet"), true);
            }
            Tag tag = await _tagRepository.FirstOrDefaultAsync(x => x.Name == "Food");
            if (tag == null)
            {
                tag = await _tagRepository.InsertAsync(new Tag("Food", "美食"), true);
            }

            //博文
            var post = await _postRepository.FirstOrDefaultAsync(x => x.Title == "美食日记（1）");
            if (post == null)
            {
                post = new Post(foodCategory.Id, "美食日记（1）");
                post.Markdown = "这是一篇美食博客";
                post = await _postRepository.InsertAsync(post, true);

                post.AddTag(tag.Id);
                post.ThumbUp(user.Id);
                await _postRepository.UpdateAsync(post, true);
            }

            //评论
            if (await _commentRepository.FirstOrDefaultAsync(x => x.PostId == post.Id && x.Text == "太棒了") == null)
            {
                await _commentRepository.InsertAsync(new Comment(post.Id, "太棒了"), true);
            }
            if (await _commentRepository.FirstOrDefaultAsync(x => x.PostId == post.Id && x.Text == "赞") == null)
            {
                var comment = await _commentRepository.InsertAsync(new Comment(post.Id, "赞"), true);
                comment.ThumbUp(user.Id);
                await _commentRepository.UpdateAsync(comment);
            }
        }
    }
}
