using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Database;

namespace Ray.Blog;

[DependsOn(
    typeof(BlogDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(BlogApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
//[DependsOn(typeof(AbpBlobStoringFileSystemModule))]
public class BlogApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<BlogApplicationModule>();
        });

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                //container.UseFileSystem(fileSystem =>
                //{
                //    fileSystem.BasePath = "./";
                //});

                container.UseDatabase();
            });
        });
    }
}
