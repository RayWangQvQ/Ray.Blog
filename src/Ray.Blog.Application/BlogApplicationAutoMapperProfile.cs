using AutoMapper;
using Ray.Blog.Blog;
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
        }
    }
}
