using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ray.Blog.EntityFrameworkCore
{
    public class BlogBlobDbContextFactory
        : IDesignTimeDbContextFactory<BlogBlobDbContext>
    {
        public BlogBlobDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var builder = new DbContextOptionsBuilder<BlogBlobDbContext>()
                .UseMySql(configuration.GetConnectionString("AbpBlobStoring"), MySqlServerVersion.LatestSupportedServerVersion);
            return new BlogBlobDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Ray.Blog.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
