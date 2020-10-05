using System;
using System.Collections.Generic;
using System.Text;

namespace Socially.MobileApps.Models
{
    public class SignUpModel : BindableBase
    {
        private string _userName;
        private string _email;
        private string _password;
        private string _confirmPassword;

        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }

    }
}
