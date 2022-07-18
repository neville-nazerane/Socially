using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Socially.Website.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Socially.Website.Services
{
    public class AuthProvider : AuthenticationStateProvider
    {
        private string token;
        private readonly IJSRuntime _jSRuntime;

        private static ClaimsPrincipal FailedLogin
            => new (new ClaimsIdentity(Array.Empty<Claim>(), ""));


        public AuthProvider(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        private async Task<ClaimsPrincipal> GetPrincipalAsync()
        {
            var tokenStr = await GetTokenAsync();
            if (tokenStr is null) return null;
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

        public async ValueTask<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            if (token is null)
                token = await _jSRuntime.InvokeAsync<string>("getData", "token");
            return token;
        }

        public async Task SetAsync(string jwt, CancellationToken cancellationToken = default)
        {
            token = jwt;
            await _jSRuntime.InvokeVoidAsync("setData", jwt);
        }

    }
}
