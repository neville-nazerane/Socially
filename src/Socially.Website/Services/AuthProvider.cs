using Microsoft.AspNetCore.Components.Authorization;
using Socially.Website.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Socially.Website.Services
{
    public class AuthProvider : AuthenticationStateProvider
    {

        private readonly UserData _data;
        private ClaimsPrincipal _principle = null;

        private static ClaimsPrincipal FailedLogin
            => new (new ClaimsIdentity(Array.Empty<Claim>(), string.Empty));


        public AuthProvider()
        {
            _data = new();
        }


        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var login = _principle ?? FailedLogin;
            // verify if the user is logged in.
            // if not:
            return Task.FromResult(new AuthenticationState(login));
        }

        public ValueTask<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(_data.Token);
        }

        public Task SetAsync(string jwt, CancellationToken cancellationToken = default)
        {
            _data.Token = jwt;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            _principle = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "local"));
            var princi = Task.FromResult(new AuthenticationState(_principle));
            NotifyAuthenticationStateChanged(princi);
            return Task.CompletedTask;
        }

    }
}
