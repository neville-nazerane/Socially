using Microsoft.AspNetCore.Components;
using Socially.Models;
using Socially.Website.Services;
using System;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class PostDisplay
    {

        [Parameter]
        public PostDisplayModel Post { get; set; }

        [Parameter]
        public Action OnDelete { get; set; }

        [Inject]
        public UserContext UserContext { get; set; }

        ProfileUpdateModel currentuser;

        protected override async Task OnInitializedAsync()
        {
            currentuser = await UserContext.GetProfileInfoAsync();
        }

        void Delete() => OnDelete?.Invoke();

    }
}
