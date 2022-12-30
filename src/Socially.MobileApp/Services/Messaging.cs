using Socially.Mobile.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Services
{
    internal class Messaging : IMessaging
    {
        public Task DisplayAsync(string title, string message, string button) => Shell.Current.DisplayAlert(title, message, button);
        public Task<string> DisplayActionSheet(string title, string message, params string[] buttons) => Shell.Current.DisplayActionSheet(title, message, null, buttons);

        

    }
}
