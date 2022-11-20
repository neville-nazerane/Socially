using Microsoft.AspNetCore.Components;
using Socially.Models;
using Socially.Website.Services;
using System.Threading.Tasks;

namespace Socially.Website.Pages.Profile
{
    public partial class ProfileLayout
    {

        [Inject]
        public CachedContext CachedContext { get; set; }

        UserSummaryModel profileInfo;


        protected override async Task OnInitializedAsync()
        {
            profileInfo = await CachedContext.GetCurrentProfileInfoAsync();
        }

    }
}
