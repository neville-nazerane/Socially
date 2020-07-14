using Socially.Core.Models;
using Socially.MobileApps.Contracts;
using Socially.MobileApps.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Socially.MobileApps.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private string _labelText;
        private string _errorMessage;
        private ICommand _submitCmd;
        private ICommand _previousCmd;
        private string _entryBinding;
        private readonly IApiConsumer _apiConsumer;

        public string EntryBinding { get => _entryBinding; set => SetProperty(ref _entryBinding, value); }

        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        public string LabelText { get => _labelText; set => SetProperty(ref _labelText, value); }

        public ICommand SubmitCmd { get => _submitCmd; set => SetProperty(ref _submitCmd, value); }

        public ICommand PreviousCmd { get => _previousCmd; set => SetProperty(ref _previousCmd, value); }
         
        public SignUpModel Model { get; set; }

        public SignUpViewModel(IApiConsumer apiConsumer, IThemeControl themeControl) : base(themeControl)
        {
            _apiConsumer = apiConsumer;
            Model = new SignUpModel();

            EntryBinding = LabelText = nameof(SignUpModel.Email);
            SubmitCmd = new Command(() =>
            {
                EntryBinding = LabelText = nameof(SignUpModel.UserName);
                SubmitCmd = new Command(() =>
                {
                    EntryBinding = LabelText = nameof(SignUpModel.Password);
                    SubmitCmd = new Command(() =>
                    {
                        EntryBinding = LabelText = nameof(SignUpModel.ConfirmPassword);
                    });
                });
            });
        }



    }
}
