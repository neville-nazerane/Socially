using Microsoft.AspNetCore.Components.Authorization;
using Socially.Website.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Socially.Website.Services
{
    public class AuthService
    {
        //private readonly UserData _data;
        //private readonly AuthProvider _stateProvider;

        //public event EventHandler<LoginEvent> LoginChanged;

        //public bool IsAuthenticated => _data.Token is not null;

        //public AuthService(AuthenticationStateProvider authenticationStateProvider)
        //{
        //    _data = new UserData();
        //    _stateProvider = (AuthProvider)authenticationStateProvider;
        //}

        //public ValueTask<string> GetTokenAsync(CancellationToken cancellationToken = default)
        //{
        //    return ValueTask.FromResult(_data.Token);
        //}

        //public Task SetAsync(string token, CancellationToken cancellationToken = default)
        //{
        //    _data.Token = token;
        //    var handler = new JwtSecurityTokenHandler();
        //    var token = handler.ReadJwtToken(jwt);
        //    _stateProvider.SetAuth();
        //    return Task.CompletedTask;
        //}

    }
}
