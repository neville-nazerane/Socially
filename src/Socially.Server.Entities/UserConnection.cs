using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Entities
{

    [Index(nameof(UserId))]
    [Index(nameof(ConnectionId))]
    public class UserConnection
    {

        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(50)]
        public string ConnectionId { get; set; }

    }
}
