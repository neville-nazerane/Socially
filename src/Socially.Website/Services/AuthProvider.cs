using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Socially.Apps.Consumer.Models;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public class AuthProvider : AuthenticationStateProvider, IAuthAccess
    {

        private const string tokenKey = "tokenData";

        private readonly IJSRuntime _jSRuntime;

        private StoredToken storedToken;
        private TaskCompletionSource<StoredToken> setterLock;



        private static ClaimsPrincipal FailedLogin
            => new (new ClaimsIdentity(Array.Empty<Claim>(), string.Empty));

        public AuthProvider(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        private async ValueTask<ClaimsPrincipal> GetPrincipalAsync()
        {
            var stored = await GetStoredTokenAsync();
            var tokenStr = stored?.AccessToken;
            if (string.IsNullOrEmpty(tokenStr)) return null;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenStr);
            var principle = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "local"));
            return principle;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var login = (await GetPrincipalAsync()) ?? FailedLogin;
            return new AuthenticationState(login);
        }

        public async ValueTask<StoredToken> GetStoredTokenAsync()
        {
            try
            {
                if (setterLock is not null) return await setterLock.Task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to wait for lock: {ex.Message}");
            }

            if (storedToken is null)
            {
                try
                {
                    string dataStr = await _jSRuntime.InvokeAsync<string>("getData", tokenKey);
                    if (dataStr is not null)
                        storedToken = JsonSerializer.Deserialize<StoredToken>(dataStr);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }

            return storedToken;
        }

        public async ValueTask SetStoredTokenAsync(TokenResponseModel res)
        {
            if (setterLock is not null) await setterLock.Task;

            setterLock = new();

            try
            {

                if (res is null)
                {
                    await _jSRuntime.InvokeVoidAsync("removeData", tokenKey);
                    storedToken = null;
                }
                else
                {

                    storedToken = new()
                    {
                        AccessToken = res.AccessToken,
                        RefreshToken = res.RefreshToken,
                        Expiary = DateTime.UtcNow.AddSeconds(res.ExpiresIn)
                    };
                    await _jSRuntime.InvokeVoidAsync("setData", tokenKey, JsonSerializer.Serialize(storedToken));
                }
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                setterLock.TrySetResult(storedToken);
            }
            catch (Exception ex)
            {
                setterLock.TrySetException(ex);
                throw;
            }
            finally
            {
                setterLock = null;
            }
        }

    }
}
