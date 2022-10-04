using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Entities
{
    public class PostLike
    {

        public int Id { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Required]
        public int? UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int? PostId { get; set; }
        public Post Post { get; set; }

        public int? CommentId { get; set; }
        public Comment Comment { get; set; }

    }
}
