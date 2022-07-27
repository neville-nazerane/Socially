using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Socially.Website.Shared
{
    public class SociallyLayoutBase : LayoutComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
                await JSRuntime.InvokeVoidAsync("window.onBlazorLoaded");

        }
    }
}
