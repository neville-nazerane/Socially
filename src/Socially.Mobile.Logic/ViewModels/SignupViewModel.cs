﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{
    public partial class SignupViewModel : ViewModelBase<SignUpModel>
    {
        private readonly INavigationControl _navigation;
        private readonly IMessaging _messaging;
        private readonly IApiConsumer _apiConsumer;
        private readonly ISocialLogger _logger;


        public SignupViewModel(INavigationControl navigation,
                               IMessaging messaging,
                               IApiConsumer apiConsumer,
                               ISocialLogger logger)
        {
            
            _navigation = navigation;
            _messaging = messaging;
            _apiConsumer = apiConsumer;
            _logger = logger;
        }

        [RelayCommand]
        Task GoToLoginAsync() => _navigation.GoToLoginAsync();

        [RelayCommand]
        Task GoToForgotPasswordAsync() => _navigation.GoToForgotPasswordAsync();

        public override void OnException(Exception ex) => _logger.LogException(ex);

        public override string ErrorOnException => "Failed to signup. Please try again";

        public override async Task SubmitToServerAsync(SignUpModel model, CancellationToken cancellationToken = default)
        {
            await _apiConsumer.SignupAsync(model.ToModel(), cancellationToken);
            await _messaging.DisplayAsync("Done!", "Your Account has been created", "Go to Login");
            await _navigation.GoToLoginAsync();
        }

        public override Task OnValidationChangedAsync()
        {
            var error = Validation?.FirstOrDefault(v => !string.IsNullOrEmpty(v.ErrorMessage));
            if (error is not null)
            {
                string field = error.MemberNames.FirstOrDefault();
                return _messaging.DisplayAsync(field, error.ErrorMessage, "OK");
            }
            return Task.CompletedTask;
        }

    }
}
