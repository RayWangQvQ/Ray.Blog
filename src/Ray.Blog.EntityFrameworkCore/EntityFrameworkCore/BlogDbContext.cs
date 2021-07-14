using Microsoft.EntityFrameworkCore;
using Ray.Blog.Categories;
using Ray.Blog.Comments;
using Ray.Blog.Hots;
using Ray.Blog.Messages;
using Ray.Blog.Posts;
using Ray.Blog.Sayings;
using Ray.Blog.Signatures;
using Ray.Blog.Tags;
using Ray.Blog.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users.EntityFrameworkCore;

namespace Ray.Blog.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See BlogMigrationsDbContext for migrations.
     */
    [ConnectionStringName("MySql")]
    public class BlogDbContext : AbpDbContext<BlogDbContext>
    {
        public DbSet<AppUser> Users { get; set; }


        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }


        public DbSet<Comment> Comments { get; set; }


        public DbSet<FriendLink> FriendLinks { get; set; }

        public DbSet<Hot> Hots { get; set; }

        public DbSet<Saying> Sayings { get; set; }

        public DbSet<Signature> Signatures { get; set; }

        public DbSet<Message> Messages { get; set; }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the shared tables (with included modules) here */

            builder.Entity<AppUser>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); //Sharing the same table "AbpUsers" with the IdentityUser

                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                /* Configure mappings for your additional properties
                 * Also see the BlogEfCoreEntityExtensionMappings class
                 */
            });

            /* Configure your own tables/entities inside the ConfigureBlog method */

            builder.ConfigureBlog();
        }
    }
}
