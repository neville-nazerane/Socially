using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Socially.Mobile.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Services
{
    internal class SociallyLogger : ISocialLogger
    {
        public void LogException(Exception ex, string message = null)
        {
            var properties = new Dictionary<string, string>
            {
                { nameof(message), message },
            };
            Crashes.TrackError(ex, properties);
        }
    }
}
