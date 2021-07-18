using Lsw.Abp.BackgroundWorkers.Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Ray.Blog.Workers
{
    public class TestWorker : HangfireBackgroundWorkerBase
    {
        public TestWorker()
        {
            CronExpression = "0 * * * *";
        }

        public override Task ExecuteAsync()
        {
            Logger.LogInformation("测试......");
            return Task.CompletedTask;
        }
    }
}
