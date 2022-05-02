using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ray.Blog.Comments;
using Ray.Blog.Posts;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Database;

namespace Ray.Blog.EntityFrameworkCore;

[DependsOn(
    typeof(BlogDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityServerEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule)
    )]
[DependsOn(typeof(BlobStoringDatabaseEntityFrameworkCoreModule))]
public class BlogEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        BlogEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<BlogDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        context.Services.AddAbpDbContext<BlogSecondDbContext>();
        context.Services.AddAbpDbContext<BlogBlobDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also BlogMigrationsDbContextFactory for EF Core tooling. */
            options.UseMySQL();
        });

        Configure<AbpEntityOptions>(options =>
        {
            options.Entity<Post>(postOptions =>
            {
                postOptions.DefaultWithDetailsFunc = query =>
                    query
                        .Include(p => p.RelatePostTags)
                        .Include(p => p.ThumbUpHistories);
            });

            options.Entity<Comment>(options =>
            {
                options.DefaultWithDetailsFunc = query =>
                    query.Include(c => c.ThumbUpHistories);
            });
        });
    }
}
