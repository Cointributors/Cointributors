using Cointributors.Data;
using Cointributors.Data.Models;
using Cointributors.Web.Components;
using Cointributors.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nethereum.Metamask;
using Nethereum.Metamask.Blazor;
using Nethereum.UI;

namespace Cointributors.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddGitHub(options =>
            {
                options.ClientId = builder.Configuration.GetValue<string>("Authentication:GitHub:ClientId")!;
                options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:GitHub:ClientSecret")!;

                options.Scope.Add("read:user");
                options.Scope.Add("user:email");
                options.Scope.Add("repo");
                options.Scope.Add("gist");

                options.SaveTokens = true;
            })
            .AddIdentityCookies(options =>
            {
                options.ApplicationCookie!.Configure(cookieOptions =>
                {
                    cookieOptions.ExpireTimeSpan = TimeSpan.FromDays(14);
                    cookieOptions.SlidingExpiration = true;
                });
            });

        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")!, o =>
            {
#if DEBUG
                o.SetPostgresVersion(11, 0);
#else
                o.SetPostgresVersion(16, 3);
#endif
            }));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();

        builder.Services.AddScoped<IMetamaskInterop, MetamaskBlazorInterop>();
        builder.Services.AddScoped<MetamaskHostProvider>();
        //Add metamask as the selected ethereum host provider
        builder.Services.AddScoped(services =>
        {
            var metamaskHostProvider = services.GetService<MetamaskHostProvider>();
            var selectedHostProvider = new SelectedEthereumHostProviderService();
            selectedHostProvider.SetSelectedEthereumHostProvider(metamaskHostProvider);
            return selectedHostProvider;
        });

        builder.Services.AddScoped<IEthereumHostProvider, MetamaskHostProvider>();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        app.Run();
    }
}
