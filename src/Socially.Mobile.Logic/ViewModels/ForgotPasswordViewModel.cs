﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Exceptions;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.Utils;
using Socially.MobileApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{
    public partial class ForgotPasswordViewModel : ViewModelBase
    {

        private readonly IMessaging _messaging;
        private readonly IApiConsumer _apiConsumer;
        private readonly ISocialLogger _logger;

        [ObservableProperty]
        string email;

        [ObservableProperty]
        string error;


        public ForgotPasswordViewModel(IMessaging messaging,
                                       IApiConsumer apiConsumer,
                                       ISocialLogger logger)
        {

            _messaging = messaging;
            _apiConsumer = apiConsumer;
            _logger = logger;
        }

        [RelayCommand]
        public async Task SubmitAsync()
        {
            if (email is null)
            {
                Error = "Enter an email";
            }
            else
            {
                try
                {
                    await _apiConsumer.ForgotPasswordAsync(Email);
                    await _messaging.DisplayAsync("Done!", "An email has been sent", "Ok");
                }
                catch (ErrorForClientException clientException)
                {
                    Error = clientException.Errors
                                           .SelectMany(e => e.Errors)
                                           .FirstOrDefault();
                    if (string.IsNullOrEmpty(Error))
                        Error = "Failed to use email";
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex);
                    Error = "Failed. Try again";
                }
            }
        }

    }
}
