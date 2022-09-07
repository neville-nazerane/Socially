using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Core.Entities
{

    [Index(nameof(FileName), nameof(UserId), IsUnique = true)]
    public class ProfileImage
    {

        public int Id { get; set; }

        [Required, MaxLength(45)]
        public string FileName { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }


    }
}
