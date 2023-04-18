using System.Threading.Tasks;
using Acme.BookStore.Localization;
using Acme.BookStore.MultiTenancy;
using Acme.BookStore.Permissions;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace Acme.BookStore.Blazor.Menus;

public class BookStoreMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<BookStoreResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                BookStoreMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home",
                order: 0
            )
        );

		var bookStoreMenu = new ApplicationMenuItem(
			"BookStore",
			l["Menu:BookStore"],
			icon: "fa fa-book"
		);

		bookStoreMenu.AddItem(
			new ApplicationMenuItem(
				"BookStore.Books",
				l["Menu:Books"],
				url: "/books"
			)
		);

		if (await context.IsGrantedAsync(BookStorePermissions.Authors.Default))
		{
			bookStoreMenu.AddItem(
				new ApplicationMenuItem(
					"BookStore.Authors",
					l["Menu:Authors"],
					url: "/authors"
				)
			);
		}

		context.Menu.AddItem(bookStoreMenu);

		if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        await Task.CompletedTask;

        return;
    }
}
