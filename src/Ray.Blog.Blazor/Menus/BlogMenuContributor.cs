﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ray.Blog.Localization;
using Ray.Blog.Permissions;
using Volo.Abp.Account.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace Ray.Blog.Blazor.Menus
{
    public class BlogMenuContributor : IMenuContributor
    {
        private readonly IConfiguration _configuration;

        public BlogMenuContributor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
            else if (context.Menu.Name == StandardMenus.User)
            {
                await ConfigureUserMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<BlogResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    BlogMenus.Home,
                    l["Menu:Home"],
                    "/",
                    icon: "fas fa-home"
                )
            );

            context.Menu.Items.Insert(
                1,
                new ApplicationMenuItem(
                    BlogMenus.Posts,
                    l["Menu:Posts"],
                    "/posts",
                    icon: "fas fa-book-open"
                )
            );

            context.Menu.Items.Insert(
                2,
                new ApplicationMenuItem(
                    BlogMenus.Categories,
                    l["Menu:Categories"],
                    "/categories",
                    icon: "fas fa-folder-open"
                )
            );

            context.Menu.Items.Insert(
                3,
                new ApplicationMenuItem(
                    BlogMenus.Tags,
                    l["Menu:Tags"],
                    "/tags",
                    icon: "fas fa-bookmark"
                )
            );

            if (await context.IsGrantedAsync(BlogPermissions.Categories.Default))
            {
                context.Menu.AddItem(new ApplicationMenuItem(
                    "Categories",
                    l["Menu:Categories"],
                    "/categories"
                    ));
            }
        }

        private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
        {
            var accountStringLocalizer = context.GetLocalizer<AccountResource>();
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

            var identityServerUrl = _configuration["AuthServer:Authority"] ?? "";

            if (currentUser.IsAuthenticated)
            {
                context.Menu.AddItem(new ApplicationMenuItem(
                    "Account.Manage",
                    accountStringLocalizer["ManageYourProfile"],
                    $"{identityServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
                    icon: "fa fa-cog",
                    order: 1000,
                    null));
            }

            return Task.CompletedTask;
        }
    }
}
