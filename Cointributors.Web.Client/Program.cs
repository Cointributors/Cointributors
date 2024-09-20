using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Nethereum.Metamask;
using Nethereum.Metamask.Blazor;
using Nethereum.UI;

namespace Cointributors.Web.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        //builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
        builder.Services.AddSingleton<AuthenticationStateProvider, PersistentEthereumAuthenticationStateProvider>();

        builder.Services.AddSingleton<IMetamaskInterop, MetamaskBlazorInterop>();
        builder.Services.AddSingleton<MetamaskInterceptor>();
        builder.Services.AddSingleton<MetamaskHostProvider>();

        //Add metamask as the selected ethereum host provider
        builder.Services.AddSingleton(services =>
        {
            var metamaskHostProvider = services.GetService<MetamaskHostProvider>();
            var selectedHostProvider = new SelectedEthereumHostProviderService();
            selectedHostProvider.SetSelectedEthereumHostProvider(metamaskHostProvider);
            return selectedHostProvider;
        });

        await builder.Build().RunAsync();
    }
}
