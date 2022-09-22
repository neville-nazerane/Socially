using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Models
{
    public class ForgotPasswordModel
    {
        public string UserName { get; set; }
        public string Token { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required, Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
    }
}
