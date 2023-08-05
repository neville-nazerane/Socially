using Socially.Models;
using System;

namespace Socially.Website.Models.RealtimeEventArgs
{
    public class UserUpdatedEventArgs : EventArgs
    {

        public UserSummaryModel User { get; set; }

    }
}
