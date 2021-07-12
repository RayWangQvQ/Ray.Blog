using Microsoft.EntityFrameworkCore;
using Ray.Blog.Categories;
using Ray.Blog.Hots;
using Ray.Blog.Messages;
using Ray.Blog.Posts;
using Ray.Blog.Sayings;
using Ray.Blog.Signatures;
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
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
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
            });
            builder.Entity<RelatePostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.RelatePostTags)
                .HasForeignKey(pt => pt.PostId);
            builder.Entity<RelatePostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.RelatePostTags)
                .HasForeignKey(pt => pt.TagId);


            builder.Entity<FriendLink>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + "FriendLinks", BlogConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<Hot>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + "Hots", BlogConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<Saying>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + "Sayings", BlogConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<Signature>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + "Signatures", BlogConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<Message>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + "Messages", BlogConsts.DbSchema);
                b.ConfigureByConvention();
            });
        }
    }
}