using Socially.MobileApps.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Socially.MobileApps.ViewModels
{
    public class SignUpFlowViewModel
    {

        public ObservableCollection<SignUpInputContext> Inputs { get; set; }

        public SignUpFlowViewModel()
        {
            Inputs = new ObservableCollection<SignUpInputContext>();
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
            },
            context => context.IsEnabled);
            triggerEnabledChange = () => cmd.ChangeCanExecute();
            return cmd;
        }

    }
}
