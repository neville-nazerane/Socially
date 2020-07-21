using Socially.MobileApps.Contracts;
using Socially.MobileApps.Models;
using Socially.MobileApps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.FluentInjector.Utilities;
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
        private bool _isPassword;
        private readonly IApiConsumer _apiConsumer;
        private readonly IPageControl _pageControl;

        public string EntryBinding { get => _entryBinding; set => SetProperty(ref _entryBinding, value); }

        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        public string LabelText { get => _labelText; set => SetProperty(ref _labelText, value); }

        public bool IsPassword { get => _isPassword; set => SetProperty(ref _isPassword, value); }

        public ICommand SubmitCmd { get => _submitCmd; set => SetProperty(ref _submitCmd, value); }

        public ICommand PreviousCmd { get => _previousCmd; set => SetProperty(ref _previousCmd, value); }

        public SignUpModel Model { get; set; }

        public SignUpViewModel(IApiConsumer apiConsumer, IThemeControl themeControl, IPageControl pageControl) : base(themeControl)
        {
            _apiConsumer = apiConsumer;
            _pageControl = pageControl;
            Model = new SignUpModel();
            SetUpEmailInput();
        }

        private void SetUpEmailInput()
        {
            EntryBinding = LabelText = nameof(SignUpModel.Email);
            SubmitCmd = BuildCommand(VerifyEmailAsync);
            IsPassword = false;
        }

        private void SetUpUserNameInput()
        {
            EntryBinding = LabelText = nameof(SignUpModel.UserName);
            SubmitCmd = BuildCommand(VerifyUserNameAsync);
            PreviousCmd = BuildCommand(SetUpEmailInput);
            IsPassword = false;
        }

        private void SetUpPasswordInput()
        {
            EntryBinding = LabelText = nameof(SignUpModel.Password);
            SubmitCmd = BuildCommand(VerifyPassword);
            PreviousCmd = BuildCommand(SetUpUserNameInput);
            IsPassword = true;
        }

        private void SetUpConfPasswordInput()
        {
            EntryBinding = LabelText = nameof(SignUpModel.ConfirmPassword);
            SubmitCmd = BuildCommand(VerifyConfPasswordAsync);
            PreviousCmd = BuildCommand(SetUpPasswordInput);
            IsPassword = true;
        }

        private async Task VerifyEmailAsync()
        {
            Model.Email = Model.Email.Trim().ToLower();
            try
            {
                await _apiConsumer.VerifyAccountEmailAsync(Model.Email);
            }
            catch
            {
                ErrorMessage = "Can't use this email";
                return;
            }
            SetUpUserNameInput();
        }

        private async Task VerifyUserNameAsync()
        {
            Model.UserName = Model.UserName.Trim().ToLower();
            try
            {
                await _apiConsumer.VerifyAccountUsernameAsync(Model.UserName);
            }
            catch
            {
                ErrorMessage = "Can't do this username";
                return;
            }
            SetUpPasswordInput();
        }

        private void VerifyPassword()
        {
            SetUpConfPasswordInput();
        }

        private async Task VerifyConfPasswordAsync()
        {
            if (Model.Password != Model.ConfirmPassword)
            {
                ErrorMessage = "Passwords don't match";
                SetUpPasswordInput();
            }
            else
            {
                var res = await _apiConsumer.SignUpAsync(Model);
                if (res.IsSuccess)
                    await _pageControl.DisplayAlert("DONE!", "You have signed up with the username " + Model.UserName, "Ok");
                else
                    ErrorMessage = res.Errors.Select(e => $"{e.Key}: {e.Value.First()}").First();
            }
        }

    }
}
