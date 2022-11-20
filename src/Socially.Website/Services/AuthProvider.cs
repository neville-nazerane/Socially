using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
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
    public class AuthProvider : AuthenticationStateProvider
    {
        private const string tokenKey = "tokenData";
        private TokenResponseModel tokenData;
        private DateTime? expiary;
        private readonly IJSRuntime _jSRuntime;

        private static ClaimsPrincipal FailedLogin
            => new (new ClaimsIdentity(Array.Empty<Claim>(), string.Empty));

        public AuthProvider(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        private async ValueTask<ClaimsPrincipal> GetPrincipalAsync()
        {
            var tokenStr = await GetTokenAsync();
            if (string.IsNullOrEmpty(tokenStr)) return null;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenStr);
            var principle = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "local"));
            return principle;
        }

        public async ValueTask<int> GetUserIdAsync()
        {
            var principle = await GetPrincipalAsync();
            Console.WriteLine(principle.FindFirst("nameid").Value);
            //Console.WriteLine(principle.FindFirst(ClaimTypes.NameIdentifier).Value);
            return int.Parse(principle.FindFirst("nameid")?.Value ?? "0");
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
                string dataStr = await _jSRuntime.InvokeAsync<string>("getData", tokenKey);
                if (dataStr is null)
                {
                    tokenData = null;
                    return;
                }
                tokenData = JsonSerializer.Deserialize<TokenResponseModel>(dataStr);
                if (tokenData is null)
                    return;

                var readToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenData.AccessToken);

                expiary = readToken.ValidTo;
            }
        }

        public async ValueTask SetAsync(TokenResponseModel res, CancellationToken cancellationToken = default)
        {
            tokenData = res;
            if (res is null)
            {
                await _jSRuntime.InvokeVoidAsync("removeData", tokenKey);
            }
            else 
                await _jSRuntime.InvokeVoidAsync("setData", tokenKey, JsonSerializer.Serialize(res));
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

    }
}
