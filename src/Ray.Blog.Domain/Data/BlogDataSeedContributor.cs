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

namespace Ray.Blog.Data
{
    public class BlogDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<Tag, Guid> _tagRepository;
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly IRepository<Comment, Guid> _commentRepository;

        public BlogDataSeedContributor(
            IRepository<Category, Guid> categoryRepository,
            IRepository<Tag, Guid> tagRepository,
            IRepository<Post, Guid> postRepository,
            IRepository<Comment, Guid> commentRepository
            )
        {
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            //分类
            Category category = await _categoryRepository.FirstOrDefaultAsync(x => x.Name == "Food");
            if (category == null)
            {
                category = await _categoryRepository.InsertAsync(new Category("Food", "美食区"), true);
            }

            //标签
            Tag tag = await _tagRepository.FirstOrDefaultAsync(x => x.Name == "Food");
            if (tag == null)
            {
                tag = await _tagRepository.InsertAsync(new Tag("Food", "美食"), true);
            }

            //博文
            var post = await _postRepository.FirstOrDefaultAsync(x => x.Title == "美食日记（1）");
            if (post == null)
            {
                post = new Post(category.Id, "美食日记（1）");
                post.AddTag(tag.Id);
                post = await _postRepository.InsertAsync(post, true);
            }

            //评论
            var comment = await _commentRepository.FirstOrDefaultAsync(x => x.PostId == post.Id && x.Text == "太棒了");
            if (comment == null)
            {
                comment = await _commentRepository.InsertAsync(new Comment(post.Id, "太棒了"), true);
            }
        }
    }
}
