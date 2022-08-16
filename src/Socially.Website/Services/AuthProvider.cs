using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Socially.Apps.Consumer.Services;
using Socially.Core.Models;
using Socially.Website.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Socially.Website.Services
{
    public class AuthProvider : AuthenticationStateProvider
    {

        private TokenResponseModel tokenData;
        private DateTime? expiary;
        private readonly IJSRuntime _jSRuntime;

        private static ClaimsPrincipal FailedLogin
            => new (new ClaimsIdentity(Array.Empty<Claim>(), string.Empty));


        public AuthProvider(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        private async Task<ClaimsPrincipal> GetPrincipalAsync()
        {
            var tokenStr = await GetTokenAsync();
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

        public async ValueTask<TokenResponseModel> GetTokenInfoAsync(CancellationToken cancellationToken = default)
        {
            await LazyLoadAsync(cancellationToken);
            return tokenData;
        }

        public async ValueTask<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            await LazyLoadAsync(cancellationToken);
            return tokenData?.AccessToken;
        }

        public async ValueTask<DateTime?> GetExiparyAsync(CancellationToken cancellationToken = default)
        {
            await LazyLoadAsync(cancellationToken);
            return expiary;
        }

        private async ValueTask LazyLoadAsync(CancellationToken cancellationToken = default)
        {
            if (tokenData is null)
            {
                string dataStr = await _jSRuntime.InvokeAsync<string>("getData", "tokenData");
                if (dataStr is null) return;

                tokenData = JsonSerializer.Deserialize<TokenResponseModel>(dataStr);
                if (tokenData is null)
                    return;

                var readToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenData.AccessToken);

                expiary = readToken.ValidTo;
            }
        }

        public async Task SetAsync(TokenResponseModel res, CancellationToken cancellationToken = default)
        {
            string strData = res == null ? null : JsonSerializer.Serialize(res);
            await _jSRuntime.InvokeVoidAsync("setData", "tokenData", strData);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

    }
}
