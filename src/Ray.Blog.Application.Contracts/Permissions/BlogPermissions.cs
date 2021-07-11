namespace Ray.Blog.Permissions
{
    public static class BlogPermissions
    {
        public const string GroupName = "Blog";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";

        public const string Tag = GroupName + "_Tag";
        public const string Tag_Create = Tag + "_Create";
    }
}