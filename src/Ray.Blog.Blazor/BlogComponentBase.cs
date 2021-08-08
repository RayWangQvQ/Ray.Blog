using Blazorise;
using Microsoft.AspNetCore.Components;
using Ray.Blog.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Ray.Blog.Blazor
{
    public abstract class BlogComponentBase : AbpComponentBase
    {
        protected BlogComponentBase()
        {
            LocalizationResource = typeof(BlogResource);
        }
    }
}
