using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Permissions;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Blazor.Pages;

public partial class Books
{
	public Books() // Constructor
	{
		CreatePolicyName = BookStorePermissions.Books.Create;
		UpdatePolicyName = BookStorePermissions.Books.Edit;
		DeletePolicyName = BookStorePermissions.Books.Delete;
	}
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		authorList = (await AppService.GetAuthorLookupAsync()).Items;
	}

	protected override async Task OpenCreateModalAsync()
	{
		if (!authorList.Any())
		{
			throw new UserFriendlyException(message: L["AnAuthorIsRequiredForCreatingBook"]);
		}

		await base.OpenCreateModalAsync();
		NewEntity.AuthorId = authorList.First().Id;
	}
}
