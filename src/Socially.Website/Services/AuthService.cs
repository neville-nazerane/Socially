using Socially.Website.Models;

namespace Socially.Website.Services
{
    public class AuthService
    {
        private readonly UserData _data;

        public event EventHandler<LoginEvent> LoginChanged;

        public bool IsAuthenticated => _data.Token is not null;

        public AuthService()
        {
            _data = new UserData();
        }

        public ValueTask<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(_data.Token);
        }

        public Task SetAsync(string token, CancellationToken cancellationToken = default)
        {
            _data.Token = token;
            LoginChanged?.Invoke(this, new LoginEvent(_data));
            return Task.CompletedTask;
        }

    }
}
