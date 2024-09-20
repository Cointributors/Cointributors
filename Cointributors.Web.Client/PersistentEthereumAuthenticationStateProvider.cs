using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Nethereum.UI;
using System.Security.Claims;

namespace Cointributors.Web.Client;

public class PersistentEthereumAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private static readonly AuthenticationState DefaultUnauthenticated = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    private readonly Claim[]? _persistentClaims = null;

    protected IEthereumHostProvider EthereumHostProvider { get; set; }
    protected SelectedEthereumHostProviderService SelectedHostProviderService { get; }

    public PersistentEthereumAuthenticationStateProvider(SelectedEthereumHostProviderService selectedHostProviderService, PersistentComponentState state)
    {
        SelectedHostProviderService = selectedHostProviderService;
        SelectedHostProviderService.SelectedHostProviderChanged += SelectedHostProviderChanged;
        InitSelectedHostProvider();


        if (!state.TryTakeFromJson<UserInfo>(nameof(UserInfo), out var userInfo) || userInfo is null)
        {
            return;
        }

        _persistentClaims = [
            new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email) ];
    }

    public void NotifyStateHasChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private Task SelectedHostProviderChanged(IEthereumHostProvider newEthereumHostProvider)
    {
        if (EthereumHostProvider != newEthereumHostProvider)
        {
            if (EthereumHostProvider != null)
            {
                EthereumHostProvider.SelectedAccountChanged -= SelectedAccountChanged;
            }
            InitSelectedHostProvider();
        }

        return Task.CompletedTask;

    }

    public void InitSelectedHostProvider()
    {
        EthereumHostProvider = SelectedHostProviderService.SelectedHost;
        if (SelectedHostProviderService.SelectedHost != null)
        {
            EthereumHostProvider.SelectedAccountChanged += SelectedAccountChanged;
        }
    }

    private async Task SelectedAccountChanged(string ethereumAddress)
    {
        if (string.IsNullOrEmpty(ethereumAddress))
        {
            await NotifyAuthenticationStateAsEthereumDisconnected();
        }
        else
        {
            await NotifyAuthenticationStateAsEthereumConnected(ethereumAddress);
        }
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_persistentClaims != null && EthereumHostProvider != null && EthereumHostProvider.Available)
        {
            var currentAddress = await EthereumHostProvider.GetProviderSelectedAccountAsync();
            if (currentAddress != null)
            {
                var claimsPrincipal = JoinEthereumWithPersistentClaimsPrincipal(currentAddress);
                return new AuthenticationState(claimsPrincipal);
            }
        }
        else if (_persistentClaims != null)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(_persistentClaims, "PersistentEthereum")));
        }

        return DefaultUnauthenticated;

    }

    public async Task NotifyAuthenticationStateAsEthereumConnected()
    {
        var currentAddress = await EthereumHostProvider.GetProviderSelectedAccountAsync();
        await NotifyAuthenticationStateAsEthereumConnected(currentAddress);
    }

    public async Task NotifyAuthenticationStateAsEthereumConnected(string currentAddress)
    {
        if (_persistentClaims != null)
        {
            var claimsPrincipal = JoinEthereumWithPersistentClaimsPrincipal(currentAddress);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        else
        {
            NotifyAuthenticationStateChanged(Task.FromResult(DefaultUnauthenticated));
        }
    }

    public async Task NotifyAuthenticationStateAsEthereumDisconnected()
    {

        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public ClaimsPrincipal JoinEthereumWithPersistentClaimsPrincipal(string ethereumAddress)
    {
        var claimsIdentity = new ClaimsIdentity(_persistentClaims!.Concat([new Claim(ClaimTypes.Role, "EthereumConnected")]), "PersistentEthereum");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return claimsPrincipal;
    }

    public void Dispose()
    {
        if (EthereumHostProvider != null)
        {
            EthereumHostProvider.SelectedAccountChanged -= SelectedAccountChanged;
        }

        if (SelectedHostProviderService != null)
        {
            SelectedHostProviderService.SelectedHostProviderChanged -= SelectedHostProviderChanged;
        }
    }
}