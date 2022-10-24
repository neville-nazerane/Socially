using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Services;
using System.Threading.Tasks;

namespace Socially.Website.Shared
{
    public partial class MainLayout
    {
        private ProfileUpdateModel profileInfo;

        [Inject]
        public AuthProvider AuthProvider { get; set; }

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public IConfiguration Config { get; set; }

        async Task LogoutAsync() => await AuthProvider.SetAsync(null);

        protected override async Task OnInitializedAsync()
        {
            try
            {
                profileInfo = await Consumer.GetUpdateProfileAsync();
            }
            catch
            {

            }
        }

    }
}
