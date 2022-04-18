using Ray.Blog.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Ray.Blog.Permissions;

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

        var postPermission = myGroup.AddPermission(BlogPermissions.Posts.Default, L(BlogPermissions.Posts.Default));
        postPermission.AddChild(BlogPermissions.Posts.Create, L(BlogPermissions.Posts.Create));
        postPermission.AddChild(BlogPermissions.Posts.Edit, L(BlogPermissions.Posts.Edit));
        postPermission.AddChild(BlogPermissions.Posts.Delete, L(BlogPermissions.Posts.Delete));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BlogResource>("Permission." + name);
    }
}
