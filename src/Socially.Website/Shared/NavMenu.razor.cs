using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Services;
using System.Threading.Tasks;

namespace Socially.Website.Shared
{
    public partial class NavMenu
    {

        private UserSummaryModel profileInfo;

        [Inject]
        public IAuthAccess AuthProvider { get; set; }

        [Inject]
        public CachedContext CachedContext { get; set; }

        async Task LogoutAsync() => await AuthProvider.SetStoredTokenAsync(null);

        protected override async Task OnInitializedAsync()
        {
            profileInfo = await CachedContext.GetCurrentProfileInfoAsync();
        }

    }
}
