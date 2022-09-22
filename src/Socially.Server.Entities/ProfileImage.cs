using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Entities
{

    [Index(nameof(FileName), nameof(UserId), IsUnique = true)]
    public class ProfileImage
    {

        public int Id { get; set; }

        [Required, MaxLength(45)]
        public string FileName { get; set; }

        public int UserId { get; set; }

        [InverseProperty(nameof(Entities.User.ProfileImages))]
        public User User { get; set; }


    }
}
