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
        private readonly IApiConsumer _apiConsumer;

        public ObservableCollection<SignUpInputContext> Inputs { get; set; }

        public SignUpFlowViewModel(IApiConsumer apiConsumer)
        {
            Inputs = new ObservableCollection<SignUpInputContext> { BuildEmailContext() };
            _apiConsumer = apiConsumer;
        }

        private SignUpInputContext BuildEmailContext()
        {
            return new SignUpInputContext
            {
                Title = "Email",
                Instructions = "Enter your valid email",
                NextCommand = BuildCommand(VerifyEmailAsync)
            };
        }

        private SignUpInputContext BuildUsernameContext()
        {
            return new SignUpInputContext
            {
                Title = "Username",
                Instructions = "Pick your username!",
                NextCommand = BuildCommand(VerifyUsername)
            };
        }

        private async Task VerifyEmailAsync(SignUpInputContext context)
        {
            bool exists = await _apiConsumer.VerifyAccountEmailAsync(context.Text.Trim());
            if (exists)
            {
                context.ErrorMessage = "There is already an account registered with this account";
            }
            else
            {
                if (!Inputs.Any(i => i.Title == "Username"))
                    Inputs.Add(BuildUsernameContext());
            }
        }

        private async Task VerifyUsername(SignUpInputContext context)
        {
            context.ErrorMessage = (await _apiConsumer.VerifyAccountUsernameAsync(context.Text.Trim())).ToString();
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
                catch
                {
                    context.ErrorMessage = "You messed up";
                    // after errors are serialized right, the errors go in here
                }
                context.IsEnabled = true;
                triggerEnabledChange();
            },
            context => context.IsEnabled);
            triggerEnabledChange = () => cmd.ChangeCanExecute();
            return cmd;
        }

    }
}
