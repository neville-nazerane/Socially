using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Socially.Website.Services;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class ProfilePictureIcon
    {

        string fileName;


        [Inject]
        public IConfiguration Config { get; set; }

        [Inject]
        public AuthProvider AuthProvider { get; set; }

        [Inject]
        public UserContext Context { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await UpdateAsync();
            AuthProvider.AuthenticationStateChanged += AuthProvider_AuthenticationStateChanged;
        }

        private async void AuthProvider_AuthenticationStateChanged(Task<Microsoft.AspNetCore.Components.Authorization.AuthenticationState> task)
        {
            await UpdateAsync();
        }

        async Task UpdateAsync()
        {
            fileName = (await Context.GetProfileInfoAsync())?.ProfilePictureFileName;
        }

    }
}
