using AutoMapper;
using Ray.Blog.Categories;

namespace Ray.Blog.Blazor
{
    public class BlogBlazorAutoMapperProfile : Profile
    {
        public BlogBlazorAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the Blazor project.

            CreateMap<CategoryDto, CreateCategoryDto>();
        }
    }
}
