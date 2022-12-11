﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class AccountViewModel : ViewModelBase
    {
        private readonly IApiConsumer _apiConsumer;
        private readonly ISocialLogger _logger;

        [ObservableProperty]
        ProfileUpdateModel profile;

        public AccountViewModel(IApiConsumer apiConsumer, ISocialLogger logger)
        {
            _apiConsumer = apiConsumer;
            _logger = logger;
        }

        public override async Task InitAsync()
        {
            Profile = (await _apiConsumer.GetUpdateProfileAsync()).ToMobileModel();
        }

        public async Task UpdateAsync()
        {
            try
            {
                IsLoading = true;
                await _apiConsumer.UpdateProfileAsync(Profile.ToModel());
            }
            catch (ErrorForClientException clientException) 
            {
                Validation = clientException.ToObservableCollection();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

    }
}
