using Socially.Core.Models;
using Socially.MobileApps.Contracts;
using Socially.MobileApps.Models;
using Socially.MobileApps.Services;
using Socially.MobileApps.Services.HttpServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Socially.MobileApps.ViewModels
{
    public class SignUpFlowViewModel
    {
        private const string usernameTitle = "Username";
        private const string passwordTitle = "Password";
        private const string confPasswordTitle = "Confirm Password";
        private readonly IApiConsumer _apiConsumer;
        private readonly IThemeControl _themeControl;

        public string Title { get; set; }

        public ICommand ThemeCommand => new Command(async () => await _themeControl.DisplayThemePickerAsync());

        public ObservableCollection<SignUpInputContext> Inputs { get; set; }

        public SignUpFlowViewModel(IApiConsumer apiConsumer, IThemeControl themeControl) 
        {
            Inputs = new ObservableCollection<SignUpInputContext> { BuildEmailContext() };
            _apiConsumer = apiConsumer;
            _themeControl = themeControl;
        }

        private SignUpInputContext BuildEmailContext()
        {
            return new SignUpInputContext
            {
                Title = "Email",
                Instructions = "Enter your valid email",
                NextCommand = BuildCommand(VerifyEmailAsync),
                ButtonText = "Choose UserName"
            };
        }

        private SignUpInputContext BuildUsernameContext()
        {
            return new SignUpInputContext
            {
                Title = usernameTitle,
                Instructions = "Pick your username!",
                NextCommand = BuildCommand(VerifyUsernameAsync),
                ButtonText = "Pick Password"
            };
        }

        private SignUpInputContext BuildPasswordContext()
        {
            return new SignUpInputContext
            {
                Title = passwordTitle,
                IsPassword = true,
                Instructions = "Choose a password",
                NextCommand = BuildCommand(PasswordAsync),
                ButtonText = "Confirm Password"
            };
        }

        private SignUpInputContext BuildConfPasswordContext()
        {
            return new SignUpInputContext
            {
                Title = confPasswordTitle,
                IsPassword = true,
                Instructions = "What's that password again?",
                NextCommand = BuildCommand(ConfirmPasswordAsync)
            };
        }

        private async Task VerifyEmailAsync(SignUpInputContext context)
        {
            context.Text = context.Text.Trim();
            if (string.IsNullOrWhiteSpace(context.Text))
            {
                context.ErrorMessage = "Enter your email";
                return;
            }

            bool exists = await _apiConsumer.VerifyAccountEmailAsync(context.Text);
            if (exists)
            {
                context.ErrorMessage = "There is already an account registered with this account";
            }
            else
            {
                if (!Inputs.Any(i => i.Title == usernameTitle))
                    Inputs.Add(BuildUsernameContext());
            }
        }

        private async Task VerifyUsernameAsync(SignUpInputContext context)
        {
            context.Text = context.Text.Trim();
            if (string.IsNullOrWhiteSpace(context.Text))
            {
                context.ErrorMessage = "Enter a username";
                return;
            }

            bool exists = await _apiConsumer.VerifyAccountUsernameAsync(context.Text);
            if (exists)
            {
                context.ErrorMessage = "Username is taken!";
            }
            else
            {
                if (!Inputs.Any(i => i.Title == passwordTitle))
                    Inputs.Add(BuildPasswordContext());
            }
        }

        private async Task PasswordAsync(SignUpInputContext context)
        {
            if (string.IsNullOrWhiteSpace(context.Text))
            {
                context.ErrorMessage = "Enter a password";
                return;
            }


            if (!Inputs.Any(i => i.Title == confPasswordTitle))
                Inputs.Add(BuildConfPasswordContext());
        }

        private Task ConfirmPasswordAsync(SignUpInputContext context)
        {
            if (context.Text != GetText(passwordTitle))
            {
                context.ErrorMessage = "Passwords don't match";
            }
            else
                context.ErrorMessage = null;
            return Task.CompletedTask;
        }

        private string GetText(string title) => Inputs.SingleOrDefault(i => i.Title == title).Text;

        private Command<SignUpInputContext> BuildCommand(Func<SignUpInputContext, Task> action)
        {
            Action triggerEnabledChange = null;
            var cmd = new Command<SignUpInputContext>(async context => {
                context.IsEnabled = false;
                triggerEnabledChange();
                try
                {
                    await action(context);
                }
                catch
                {
                    context.ErrorMessage = "You messed up";
                    // after errors are serialized right, the errors go in here
                }
                context.IsEnabled = true;
                triggerEnabledChange();
            },
            context => context?.IsEnabled == true);
            triggerEnabledChange = () => cmd.ChangeCanExecute();
            return cmd;
        }

    }
}
