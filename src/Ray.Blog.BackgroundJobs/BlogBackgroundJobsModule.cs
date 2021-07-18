using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.Modularity;
using Hangfire.MySql;
using Volo.Abp;

namespace Ray.Blog.BackgroundJobs
{
    [DependsOn(
        typeof(BlogApplicationContractsModule),
        typeof(AbpBackgroundJobsHangfireModule)
    )]
    public class BlogBackgroundJobsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureHangfire(context, configuration);
        }

        private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddHangfire(config =>
            {
                config.UseStorage(new MySqlStorage(configuration.GetConnectionString("MySql"), new MySqlStorageOptions()));
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseHangfireDashboard();
        }
    }
}
