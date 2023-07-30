using System;

namespace Socially.Website.Models.RealtimeEventArgs
{
    public class ErrorEventArgs : EventArgs
    {

        public Guid RequestId { get; set; }

        public string ErrorMessage { get; set; }

    }
}
