using Microsoft.AspNetCore.Components;
using Socially.Models;
using Socially.Website.Services;
using System.Threading.Tasks;

namespace Socially.Website.Shared
{
    public partial class NavMenu
    {

        private ProfileUpdateModel profileInfo;

        [Inject]
        public AuthProvider AuthProvider { get; set; }

        [Inject]
        public CachedContext CachedContext { get; set; }

        async Task LogoutAsync() => await AuthProvider.SetAsync(null);

        protected override async Task OnInitializedAsync()
        {
            profileInfo = await CachedContext.GetCurrentProfileInfoAsync();
        }

    }
}
