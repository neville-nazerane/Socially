using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Socially.Website.Services;
using System.Threading.Tasks;

namespace Socially.Website.Shared
{
    public partial class MainLayout
    {

        [Inject]
        public AuthProvider AuthProvider { get; set; }

        Task LogoutAsync()
        {
            return AuthProvider.SetAsync(null);
        }


    }
}
