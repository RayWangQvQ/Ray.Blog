using Ray.Blog.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Ray.Blog
{
    [DependsOn(
        typeof(BlogEntityFrameworkCoreTestModule)
        )]
    public class BlogDomainTestModule : AbpModule
    {

    }
}