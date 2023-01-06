using Microsoft.AspNetCore.Components;
using Schedule.MauiBlazor.OAuth;

namespace Schedule.MauiBlazor.Pages.Account;

public partial class RedirectToLogout
{
    [Inject]
    public ExternalAuthService ExternalAuthService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected async override Task OnInitializedAsync()
    {
        if (CurrentUser.IsAuthenticated)
        {
            await ExternalAuthService.SignOutAsync();
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
    }
}