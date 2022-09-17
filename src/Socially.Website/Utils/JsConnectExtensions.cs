using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Socially.Website.Utils
{
    public static class JsConnectExtensions
    {

        public static ValueTask TriggerClickAsync(this IJSRuntime jSRuntime, ElementReference? reference)
            => jSRuntime.InvokeVoidAsync("triggerClick", reference);

    }
}
