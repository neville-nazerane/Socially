using Microsoft.AspNetCore.Components;
using Socially.Website.Services;

namespace Socially.Website.Components
{
    public partial class AuthConditional
    {

        [Parameter]
        public RenderFragment Authorized { get; set; }

        [Parameter]
        public RenderFragment UnAuthorized { get; set; }

        [Inject]
        public AuthService AuthService { get; set; }

        bool isAuthenticated = false;

        protected override void OnInitialized()
        {
            AuthService.LoginChanged += LoginChanged;
            isAuthenticated = AuthService.IsAuthenticated;
            base.OnInitialized();
        }

        private void LoginChanged(object sender, Models.LoginEvent e)
        {
            isAuthenticated = AuthService.IsAuthenticated;
        }
    }
}
