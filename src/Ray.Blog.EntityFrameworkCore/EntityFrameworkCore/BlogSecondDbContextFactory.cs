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
    public class BlogSecondDbContextFactory
        : IDesignTimeDbContextFactory<BlogSecondDbContext>
    {
        public BlogSecondDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var builder = new DbContextOptionsBuilder<BlogSecondDbContext>()
                .UseMySql(configuration.GetConnectionString("AbpPermissionManagement"), MySqlServerVersion.LatestSupportedServerVersion);
            return new BlogSecondDbContext(builder.Options);
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
