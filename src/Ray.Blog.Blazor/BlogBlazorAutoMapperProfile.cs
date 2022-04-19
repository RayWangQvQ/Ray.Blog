using AutoMapper;
using Ray.Blog.Categories;
using Ray.Blog.Posts;
using Ray.Blog.Tags;
using System.Linq;

namespace Ray.Blog.Blazor;

public class BlogBlazorAutoMapperProfile : Profile
{
    public BlogBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.
        CreateMap<CategoryDto, CreateCategoryDto>();
        CreateMap<TagDto, CreateTagDto>();
        CreateMap<PostDto, CreatePostDto>()
            .AfterMap((p, c) => c.TagIds = p.Tags.Select(x => x.Id).ToList());
    }
}
