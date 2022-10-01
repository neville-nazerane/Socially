using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Entities
{
    public class Friend
    {
        public int Id { get; set; }

        [Required]
        public int? OwnerUserId { get; set; }
        public User OwnerUser { get; set; }

        [Required]
        public int? FriendUserId { get; set; }
        public User FriendUser { get; set; }

    }
}
