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
    }
}
