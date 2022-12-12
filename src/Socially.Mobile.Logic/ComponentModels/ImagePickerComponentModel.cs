using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Http;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.ViewModels;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ComponentModels
{
    public partial class ImagePickerComponentModel : ViewModelBase<ObservableCollection<string>>
    {
        private readonly IApiConsumer _apiConsumer;
        private readonly ISocialLogger _logger;
        private readonly IMessaging _messaging;

        public ImagePickerComponentModel(IApiConsumer apiConsumer,
                                         ISocialLogger logger,
                                         IMessaging messaging)
        {
            _apiConsumer = apiConsumer;
            _logger = logger;
            _messaging = messaging;
        }

        public override void OnException(Exception ex)
        {
            _logger.LogException(ex);
            _messaging.DisplayAsync("Failed!", "Please try again", "OK");
        }

        public override async Task<ObservableCollection<string>> GetFromServerAsync(CancellationToken cancellationToken = default)
            => new(await _apiConsumer.GetAllImagesOfUserAsync(cancellationToken));

        public async Task AddImageAsync(UploadContext context)
        {
            try
            {
                await _apiConsumer.UploadAsync(new()
                {
                    ImageContext = context
                });
                await GetAsync();
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

    }
}
