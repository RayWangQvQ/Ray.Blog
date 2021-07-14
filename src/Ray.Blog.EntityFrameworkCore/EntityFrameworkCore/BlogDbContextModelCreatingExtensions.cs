using Microsoft.EntityFrameworkCore;
using Ray.Blog.Categories;
using Ray.Blog.Comments;
using Ray.Blog.Posts;
using Ray.Blog.Tags;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Ray.Blog.EntityFrameworkCore
{
    public static class BlogDbContextModelCreatingExtensions
    {
        public static void ConfigureBlog(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(BlogConsts.DbTablePrefix + "YourEntities", BlogConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});

            builder.Entity<Category>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + "Categories", BlogConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<Post>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + "Posts", BlogConsts.DbSchema);
                b.ConfigureByConvention();

                b.HasOne<Category>()
                    .WithMany()
                    .IsRequired()
                    .HasForeignKey(p => p.CategoryId);
            });

            builder.Entity<Tag>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + "Tags", BlogConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<RelatePostTag>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + "RelatePostTags", BlogConsts.DbSchema);
                b.ConfigureByConvention();

                b.HasKey(x => new { x.PostId, x.TagId });//联合主键

                b.HasOne<Post>()
                .WithMany(p => p.RelatePostTags)
                .HasForeignKey(r => r.PostId);

                b.HasOne<Tag>()
                .WithMany(t => t.RelatePostTags)
                .HasForeignKey(r => r.TagId);
            });

            builder.Entity<Comment>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + "Comments", BlogConsts.DbSchema);
                b.ConfigureByConvention();

                b.HasOne<Post>()
                .WithMany()
                .HasForeignKey(c => c.PostId);

                b.HasOne<Comment>()
                .WithMany()
                .HasForeignKey(c => c.RepliedCommentId);
            });
        }
    }
}