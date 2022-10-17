using Socially.Apps.Consumer.Services;
using Socially.Models;
using System.Threading.Tasks;

namespace Socially.Website.Services
{
    public class UserContext
    {
        private readonly IApiConsumer _consumer;
        ProfileUpdateModel _ProfileInfo;

        public UserContext(IApiConsumer consumer, AuthProvider authProvider)
        {
            _consumer = consumer;
            authProvider.AuthenticationStateChanged += AuthProvider_AuthenticationStateChanged;
        }

        private void AuthProvider_AuthenticationStateChanged(Task<Microsoft.AspNetCore.Components.Authorization.AuthenticationState> task)
        {
            _ProfileInfo = null;
        }

        public async ValueTask<ProfileUpdateModel> GetProfileInfoAsync() => _ProfileInfo ??= await _consumer.GetUpdateProfileAsync();



    }
}
