using Volo.Abp.Modularity;

namespace Ray.Blog;

[DependsOn(
    typeof(BlogApplicationModule),
    typeof(BlogDomainTestModule)
    )]
public class BlogApplicationTestModule : AbpModule
{

}
