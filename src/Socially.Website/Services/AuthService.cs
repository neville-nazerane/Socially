using Socially.Website.Models;

namespace Socially.Website.Services
{
    public class AuthService
    {
        private readonly UserData _data;

        public AuthService()
        {
            _data = new UserData();
        }

        public ValueTask<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(_data.Token);
        }

        public Task SetAsync(string token)
        {
            _data.Token = token;
            return Task.CompletedTask;
        }

    }
}
