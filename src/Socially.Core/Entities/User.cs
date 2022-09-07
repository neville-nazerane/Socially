using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Socially.Core.Entities
{
    public class User : IdentityUser<int>
    {

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        public IEnumerable<ProfileImage> ProfileImages { get; set; }

    }
}
