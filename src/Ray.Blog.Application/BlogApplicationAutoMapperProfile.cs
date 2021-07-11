using AutoMapper;
using Ray.Blog.Blog;
using Ray.Blog.Categories;
using Ray.Blog.Posts;
using Ray.Blog.Tags;

namespace Ray.Blog
{
    public class BlogApplicationAutoMapperProfile : Profile
    {
        public BlogApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();

            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
            CreateMap<RelatePostTag, RelatePostTagDto>();
            CreateMap<RelatePostTagDto, RelatePostTag>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
        }
    }
}
