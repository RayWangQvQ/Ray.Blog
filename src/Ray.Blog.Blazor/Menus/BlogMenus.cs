namespace Ray.Blog.Blazor.Menus
{
    public class BlogMenus
    {
        private const string Prefix = "Blog";
        public const string Home = Prefix + ".Home";

        //Add your menu items here...

        public const string Categories = Prefix + ".Categories";
        public const string Posts = Prefix + ".Posts";
        public const string Tags = Prefix + ".Tags";

        public const string Admin = Prefix + ".Admin";
        public const string AdminCategories = Admin + ".Categories";
        public const string AdminPosts = Admin + ".Posts";
    }
}