using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Models
{
    public class DisplayCommentModel
    {

        public int Id { get; set; }

        public int CreatorId { get; set; }

        public string Text { get; set; }

        public ICollection<DisplayCommentModel> Comments { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsLikedByCurrentUser { get; set; }
    }
}
