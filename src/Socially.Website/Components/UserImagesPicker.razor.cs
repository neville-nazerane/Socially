using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Socially.Apps.Consumer.Services;
using Socially.Website.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class UserImagesPicker
    {

        InputFile uploadInput;

        bool isLoading = false;
        private List<string> images = null;

        [Inject]
        public IConfiguration Config { get; set; }

        [Parameter]
        public Func<string, Task> OnImageSelected { get; set; }

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
            var res = await Consumer.UploadAsync(new Socially.Models.ImageUploadModel
            {
                ImageContext = new Socially.Models.UploadContext
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

        void SelectImage(string fileName)
        {
            OnImageSelected?.Invoke(fileName);
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
