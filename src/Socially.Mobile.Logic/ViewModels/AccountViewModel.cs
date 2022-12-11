using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Apps.Consumer.Exceptions;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Models.Mappings;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{
    public partial class AccountViewModel : ViewModelBase<ProfileUpdateModel>
    {
        private readonly IApiConsumer _apiConsumer;
        private readonly ISocialLogger _logger;

        public AccountViewModel(IApiConsumer apiConsumer, ISocialLogger logger)
        {
            _apiConsumer = apiConsumer;
            _logger = logger;
        }

        public override void OnException(Exception ex) => _logger.LogException(ex);

        public override async Task<ProfileUpdateModel> GetFromServerAsync(CancellationToken cancellationToken = default)
            => (await _apiConsumer.GetUpdateProfileAsync()).ToMobileModel();

        public override Task SubmitToServerAsync(ProfileUpdateModel model, CancellationToken cancellationToken = default)
            => _apiConsumer.UpdateProfileAsync(Model.ToModel());

    }
}
