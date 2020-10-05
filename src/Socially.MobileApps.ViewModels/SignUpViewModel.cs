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

        public ICommand SubmitCmd { get; set; }

        public ICommand PreviousCmd { get; set; }

        public SignUpModel Model { get; set; }

        private int inputIndex;
        private readonly InputContext[] inputContexts;

        public SignUpViewModel(IApiConsumer apiConsumer, IThemeControl themeControl, IPageControl pageControl) : base(themeControl)
        {
            _apiConsumer = apiConsumer; 
            _pageControl = pageControl;
            SubmitCmd = BuildCommand(NextAsync);
            PreviousCmd = BuildCommand(Previous);
            Model = new SignUpModel();

            inputContexts = new InputContext[] {
                new InputContext(nameof(SignUpModel.Email), VerifyEmailAsync),
                new InputContext(nameof(SignUpModel.UserName), VerifyUserNameAsync),
                new InputContext(nameof(SignUpModel.Password), isPassword: true),
                new InputContext(nameof(SignUpModel.ConfirmPassword), "Confirm Password", VerifyConfPasswordAsync, true)
            };
            SetupIndex();
        }

        private async Task NextAsync()
        {
            var context = inputContexts[inputIndex];
            if (context.SubmitAction is null || await context.SubmitAction())
            {
                inputIndex++;
                if (inputIndex == inputContexts.Length) await CompleteAsync();
                else SetupIndex();
            }
        }

        private void Previous()
        {
            inputIndex--;
            SetupIndex();
        }

        private void SetupIndex()
        {
            var context = inputContexts[inputIndex];
            LabelText = context.LabelText;
            IsPassword = context.IsPassword;
            EntryBinding = context.EntryBinding;
        }

        private Task CompleteAsync()
        {
            return _pageControl.DisplayAlert("DONE!", "You have signed up with the username " + Model.UserName, "Ok");
        }

        private async Task<bool> VerifyEmailAsync()
        {
            if (string.IsNullOrWhiteSpace(Model.Email)) return false;
            Model.Email = Model.Email.Trim().ToLower();
            try
            {
                await _apiConsumer.VerifyAccountEmailAsync(Model.Email);
            }
            catch
            {
                ErrorMessage = "Can't use this email";
                return false;
            }
            return true;
        }

        private async Task<bool> VerifyUserNameAsync()
        {
            if (string.IsNullOrWhiteSpace(Model.UserName)) return false;
            Model.UserName = Model.UserName.Trim().ToLower();
            try
            {
                await _apiConsumer.VerifyAccountUsernameAsync(Model.UserName);
            }
            catch
            {
                ErrorMessage = "Can't do this username";
                return true;
            }
            return true;
        }

        private async Task<bool> VerifyConfPasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(Model.ConfirmPassword)) return false;
            if (Model.Password != Model.ConfirmPassword)
            {
                ErrorMessage = "Passwords don't match";
                return false;
            }
            else
            {
                var res = await _apiConsumer.SignUpAsync(Model);
                if (res.IsSuccess) return true;
                    
                else
                {
                    ErrorMessage = res.Errors.Select(e => $"{e.Key}: {e.Value.First()}").First();
                    return false;
                }
            }
        }

        private class InputContext
        {

            public InputContext()
            {

            }

            public InputContext(string labelAndBinding,
                                Func<Task<bool>> submitAction = null,
                                bool isPassword = false) 
                                        : this(labelAndBinding, labelAndBinding, submitAction, isPassword)
            {

            }

            public InputContext(string labelText,
                                string entryBinding,
                                Func<Task<bool>> submitAction = null,
                                bool isPassword = false)
            {
                LabelText = labelText;
                EntryBinding = entryBinding;
                SubmitAction = submitAction;
                IsPassword = isPassword;
            }

            public string LabelText { get; set; }

            public string EntryBinding { get; set; }

            public Func<Task<bool>> SubmitAction { get; set; }
            public bool IsPassword { get; internal set; }
        }

    }
}
