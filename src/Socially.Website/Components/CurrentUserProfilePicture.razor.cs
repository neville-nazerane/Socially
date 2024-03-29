﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Socially.Website.Services;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class CurrentUserProfilePicture
    {

        string fileName;


        [Inject]
        public IConfiguration Config { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthProvider { get; set; }

        [Inject]
        public CachedContext Context { get; set; }

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
            fileName = (await Context.GetCurrentProfileInfoAsync())?.ProfilePicUrl;
        }

    }
}
