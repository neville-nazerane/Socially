using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Socially.Apps.Consumer.Services;
using Socially.Website.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class UserImagesComponent
    {

        InputFile uploadInput;

        bool isLoading = false;
        private List<string> images = null;

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        async void TriggerTheKracken()
        {
            await JSRuntime.TriggerClickAsync(uploadInput.Element);
        }

        async void ImageUploaded(InputFileChangeEventArgs args)
        {
            var res = await Consumer.UploadAsync(new Core.Models.ImageUploadModel
            {
                ImageContext = new Core.Models.UploadContext
                {
                    FileName = args.File.Name,
                    Stream = args.File.OpenReadStream(5 * 1024 * 1024)
                }
            });
            if (res.IsSuccessStatusCode)
            {
                var fileName = await res.Content.ReadAsStringAsync();
                System.Console.WriteLine(fileName);
                images.Add(fileName);
                StateHasChanged();
            }
        }

        async void DeleteAsync(string fileName)
        {
            await Consumer.DeleteImageAsync(fileName);
            images.Remove(fileName);
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            try
            {
                images = (await Consumer.GetAllImagesOfUserAsync()).ToList();
            }
            finally
            {
                isLoading = false;
            }
        }

    }
}
