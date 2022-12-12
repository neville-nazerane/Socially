﻿using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Models.Mappings;
using Socially.Mobile.Logic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{
    public class HomeViewModel : ViewModelBase<ObservableCollection<PostDisplayModel>>
    {

        private readonly ISocialLogger _logger;
        private readonly IApiConsumer _apiConsumer;

        public HomeViewModel(ISocialLogger logger, IApiConsumer apiConsumer)
        {
            _logger = logger;
            _apiConsumer = apiConsumer;
        }

        public override void OnException(Exception ex) => _logger.LogException(ex);

        public override async Task<ObservableCollection<PostDisplayModel>> GetFromServerAsync(CancellationToken cancellationToken = default)
            => new(await _apiConsumer.GetHomePostsAsync(10, null, cancellationToken).ToMobileModel());

    }
}