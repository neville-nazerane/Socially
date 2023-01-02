using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{
    public class ProfileImagesViewModel : ViewModelBase<ObservableCollection<string>>
    {
        private readonly ISocialLogger _socialLogger;
        private readonly IApiConsumer _apiConsumer;

        public ProfileImagesViewModel(ISocialLogger socialLogger, 
                                      IApiConsumer apiConsumer)
        {
            _socialLogger = socialLogger;
            _apiConsumer = apiConsumer;
        }

        public override void OnException(Exception ex) => _socialLogger.LogException(ex);

        public override async Task<ObservableCollection<string>> GetFromServerAsync(CancellationToken cancellationToken = default)
            => new(await _apiConsumer.GetAllImagesOfUserAsync());

    }
}
