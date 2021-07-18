using Hangfire;
using Hangfire.MySql;
using Lsw.Abp.BackgroundWorkers.Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ray.Blog.Workers;
using System;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Ray.Blog
{
    [DependsOn(
        typeof(AbpAutofacModule)
    //typeof(AbpBackgroundWorkerHangfireModule)
    )]
    public class BlogBackgroundWorkersModule : AbpModule
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

            var worker = app.ApplicationServices.GetRequiredService<TestWorker>();
            RecurringJob.AddOrUpdate(() => worker.ExecuteAsync(), worker.CronExpression);
        }
    }
}
