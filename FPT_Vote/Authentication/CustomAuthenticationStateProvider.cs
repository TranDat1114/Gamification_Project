using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace FPT_Vote.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _storage;
    private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ProtectedSessionStorage storage)
    {
        _storage = storage;
    }

    public async Task MarkUserAsAuthenticated(string userName)
    {
        await _storage.SetAsync("userName", userName);
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, userName)
        }, "apiauth_type");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _storage.DeleteAsync("userName");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        try
        {
            var userSessionStorageResult = await _storage.GetAsync<UserSession>("UserSession");
            var UserSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
            if (UserSession != null)
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, UserSession.Email),
                new(ClaimTypes.Role, UserSession.Role)
            }, "CustomAuth"))));
            }
            else
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }
        catch (System.Exception)
        {

            return await Task.FromResult(new AuthenticationState(_anonymous));

        }
    }

    public async Task UpdateAuthenticationState(UserSession userSession)
    {
        ClaimsPrincipal claimsPrincipal;
        if (userSession != null)
        {
            await _storage.SetAsync("UserSession", userSession);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, userSession.Email),
                new(ClaimTypes.Role, userSession.Role)
            }, "CustomAuth"));

        }
        else
        {
            await _storage.DeleteAsync("UserSession");
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));

    }
}
