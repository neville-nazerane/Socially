using Socially.Core.Models;
using Socially.MobileApps.Models;
using Socially.MobileApps.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Socially.MobileApps.ViewModels
{
    public class SignUpFlowViewModel
    {
        private const string usernameTitle = "Username";
        private const string passwordTitle = "Password";
        private const string confPasswordTitle = "Confirm Password";
        private readonly IApiConsumer _apiConsumer;

        public SignUpModel SignUpData { get; set; }

        public ObservableCollection<SignUpInputContext> Inputs { get; set; }

        public SignUpFlowViewModel(IApiConsumer apiConsumer)
        {
            Inputs = new ObservableCollection<SignUpInputContext> { BuildEmailContext() };
            _apiConsumer = apiConsumer;

            SignUpData = new SignUpModel();
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
                NextCommand = BuildCommand(VerifyUsernameAsync)
            };
        }

        private async Task VerifyEmailAsync(SignUpInputContext context)
        {
            if (string.IsNullOrWhiteSpace(context.Text))
            {
                context.ErrorMessage = "Enter your email";
                return;
            }

            bool exists = await _apiConsumer.VerifyAccountEmailAsync(context.Text.Trim());
            if (exists)
            {
                context.ErrorMessage = "There is already an account registered with this account";
            }
            else
            {
                SignUpData.Email = context.Text;
                if (!Inputs.Any(i => i.Title == usernameTitle))
                    Inputs.Add(BuildUsernameContext());
            }
        }

        private async Task VerifyUsernameAsync(SignUpInputContext context)
        {
            if (string.IsNullOrWhiteSpace(context.Text))
            {
                context.ErrorMessage = "Enter a username";
                return;
            }

            bool exists = await _apiConsumer.VerifyAccountUsernameAsync(context.Text.Trim());
            if (exists)
            {
                context.ErrorMessage = "Username is taken!";
            }
            else
            {
                SignUpData.UserName = context.Text;
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

            SignUpData.Password = context.Text;

            if (!Inputs.Any(i => i.Title == confPasswordTitle))
                Inputs.Add(BuildConfPasswordContext());
        }

        private async Task ConfirmPasswordAsync(SignUpInputContext context)
        {
            if (context.Text != SignUpData.Password)
            {
                context.ErrorMessage = "Passwords don't match";
                return;
            }
            else
                context.ErrorMessage = null;
        }

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
                catch (Exception e)
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
