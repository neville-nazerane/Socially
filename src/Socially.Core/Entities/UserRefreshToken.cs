using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Core.Entities
{
    public class UserRefreshToken
    {

        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string RefreshToken { get; set; }

        [Required]
        public bool? IsEnabled { get; set; }

        [Required]
        public DateTime? ExpiresOn { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }


    }
}
