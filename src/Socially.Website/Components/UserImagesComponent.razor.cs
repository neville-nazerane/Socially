using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Socially.Apps.Consumer.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class UserImagesComponent
    {

        InputFile uploadInput;

        bool isLoading = false;
        private IEnumerable<string> images = null;

        [Inject]
        public IApiConsumer Consumer { get; set; }

        async void ImageUploaded(InputFileChangeEventArgs args)
        {
            System.Console.WriteLine(args.File.Name);
            await Consumer.UploadAsync(new Core.Models.ImageUploadModel
            {
                ImageContext = new Core.Models.UploadContext
                {
                    FileName = args.File.Name,
                    Stream = args.File.OpenReadStream()
                }
            });
        }

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            try
            {
                images = await Consumer.GetAllImagesOfUserAsync();
            }
            finally
            {
                isLoading = false;
            }
        }

    }
}
