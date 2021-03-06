using AutoMapper;
using Ray.Blog.Categories;
using Ray.Blog.Comments;
using Ray.Blog.Posts;
using Ray.Blog.Tags;

namespace Ray.Blog;

public class BlogApplicationAutoMapperProfile : Profile
{
    public BlogApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Tag, TagDto>();
        CreateMap<TagDto, Tag>();
        CreateMap<CreateTagDto, Tag>();
        CreateMap<Tag, TagWithCountDto>();

        CreateMap<Post, PostDto>();
        CreateMap<PostDto, Post>();
        CreateMap<CreatePostDto, Post>();
        CreateMap<Category, CategoryLookupDto>();
        CreateMap<Tag, TagLookupDto>();

        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
        CreateMap<CreateCategoryDto, Category>();

        CreateMap<Comment, CommentDto>();
        CreateMap<CreateCommentDto, Comment>();

        CreateMap<PostThumbUpHistory, PostThumbUpHistoryDto>();
        CreateMap<CommentThumbUpHistory, CommentThumbUpHistoryDto>();
    }
}
