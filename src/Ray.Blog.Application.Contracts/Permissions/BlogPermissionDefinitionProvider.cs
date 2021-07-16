using Ray.Blog.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Ray.Blog.Permissions
{
    public class BlogPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(BlogPermissions.GroupName, L(BlogPermissions.GroupName));

            //Define your own permissions here. Example:
            //myGroup.AddPermission(BlogPermissions.MyPermission1, L("Permission:MyPermission1"));

            var categoryPermission = myGroup.AddPermission(BlogPermissions.Categories.Default, L(BlogPermissions.Categories.Default));
            categoryPermission.AddChild(BlogPermissions.Categories.Create, L(BlogPermissions.Categories.Create));
            categoryPermission.AddChild(BlogPermissions.Categories.Edit, L(BlogPermissions.Categories.Edit));
            categoryPermission.AddChild(BlogPermissions.Categories.Delete, L(BlogPermissions.Categories.Delete));

            var tagPermission = myGroup.AddPermission(BlogPermissions.Tags.Default, L(BlogPermissions.Tags.Default));
            tagPermission.AddChild(BlogPermissions.Tags.Create, L(BlogPermissions.Tags.Create));
            tagPermission.AddChild(BlogPermissions.Tags.Edit, L(BlogPermissions.Tags.Edit));
            tagPermission.AddChild(BlogPermissions.Tags.Delete, L(BlogPermissions.Tags.Delete));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BlogResource>("Permission." + name);
        }
    }
}
