using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Entities
{
    public class Comment
    {

        public Comment()
        {
            CreatedOn = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [MaxLength(2000)]
        public string Text { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [Required]
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        [Required]
        public int? PostId { get; set; }
        public Post Post { get; set; }

        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public IEnumerable<PostLike> Likes { get; set; }
        public int? LikeCount { get; set; }
    }
}
