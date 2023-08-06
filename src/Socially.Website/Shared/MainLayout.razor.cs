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

        [Inject]
        public SignalRListener SignalRListener { get; set; }

        protected override void OnInitialized()
        {
            SignalRListener.OnUserUpdated += SignalRListener_OnUserUpdated;
        }

        private void SignalRListener_OnUserUpdated(object sender, Models.RealtimeEventArgs.UserUpdatedEventArgs e)
        {
            StateHasChanged();
        }
    }
}
