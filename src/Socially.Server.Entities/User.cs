using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Socially.Server.Entities
{
    public class User : IdentityUser<int>
    {

        public User()
        {
            CreatedOn = DateTime.UtcNow;
        }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey(nameof(ProfilePicture))]
        public int? ProfilePictureId { get; set; }
        public ProfileImage ProfilePicture { get; set; }

        public IEnumerable<ProfileImage> ProfileImages { get; set; }

        [InverseProperty(nameof(Friend.OwnerUser))]
        public IEnumerable<Friend> Friends { get; set; }

        [InverseProperty(nameof(FriendRequest.For))]
        public IEnumerable<FriendRequest> RecievedFriendRequests { get; set; }

        [InverseProperty(nameof(FriendRequest.Requester))]
        public IEnumerable<FriendRequest> SentFriendRequests { get; set; }

        public IEnumerable<Post> Posts { get; set; }

    }
}
