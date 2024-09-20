using Cointributors.Data;
using Cointributors.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cointributors.Web.Components.Account;

internal sealed class IdentityUserAccessor(UserManager<User> userManager, DataContext dataContext, IdentityRedirectManager redirectManager)
{
    public async Task<User> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }

    public async Task<string> GetGitHubAccessToken(HttpContext context)
    {
        var userId = userManager.GetUserId(context.User)!;

        var accessToken = await dataContext.UserTokens.Where(t => t.UserId == userId && t.Name == "access_token").SingleAsync();

        return accessToken.Value!;
    }
}
