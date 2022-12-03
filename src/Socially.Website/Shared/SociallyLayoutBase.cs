using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Socially.Website.Shared
{
    public class SociallyLayoutBase : LayoutComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IConfiguration Configuration { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("window.onBlazorLoaded");
                await JSRuntime.InvokeVoidAsync("window.executeAppInsights", Configuration["appAI"]);
            }
        }
    }
}
