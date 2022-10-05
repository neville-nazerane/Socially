using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Socially.Server.Entities
{

    public class FriendRequest
    {

        public FriendRequest()
        {
            RequestedOn = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Required]
        public int? RequesterId { get; set; }
        public User Requester { get; set; }

        [Required]
        public int? ForId { get; set; }
        public User For { get; set; }

        [Required]
        public DateTime? RequestedOn { get; set; }

        public DateTime? RespondedOn { get; set; }

        public bool? IsAccepted { get; set; }

    }
}
