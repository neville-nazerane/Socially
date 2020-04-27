using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Socially.MobileApps.Models
{
    public class SignUpInputContext : BindableBase
    {
        private bool _isEnabled;
        private string _errorMessage;

        public string Text { get; set; }

        public bool IsPassword { get; set; }

        public Command<SignUpInputContext> NextCommand { get; set; }

        public string Title { get; set; }

        public string Instructions { get; set; }

        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        public bool IsEnabled { get => _isEnabled; set => SetProperty(ref _isEnabled, value); }

        public SignUpInputContext()
        {
            IsEnabled = true;
        }

    }
}
