using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Core.Models
{
    public class ProfileUpdateModel
    {

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string ProfilePictureFileName { get; set; }
    }
}
