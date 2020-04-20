using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Socially.MobileApps.Models
{
    public class SignUpInputContext
    {

        public string Text { get; set; }

        public bool IsPassword { get; set; }

        public Command<SignUpInputContext> NextCommand { get; set; }

        public string Title { get; set; }

        public string Instructions { get; set; }

        public string ErrorMessage { get; set; }

    }
}
