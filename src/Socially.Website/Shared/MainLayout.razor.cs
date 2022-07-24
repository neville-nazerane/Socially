using Microsoft.AspNetCore.Components;
using Socially.Website.Services;

namespace Socially.Website.Shared
{
    public partial class MainLayout
    {

        [Inject]
        public AuthProvider AuthProvider { get; set; }

        Task LogoutAsync()
        {
            return AuthProvider.SetAsync(string.Empty);
        }

    }
}
