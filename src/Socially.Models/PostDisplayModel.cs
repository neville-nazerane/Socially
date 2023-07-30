using Socially.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Models
{
    public class PostDisplayModel : ICachable<int, PostDisplayModel>
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public int CreatorId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<DisplayCommentModel> Comments { get; set; }
        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }

        public void CopyFrom(PostDisplayModel data)
        {
            Text = data.Text;
            CreatorId = data.CreatorId;
            CreatedOn = data.CreatedOn;
            Comments = data.Comments;
            LikeCount = data.LikeCount;
            IsLikedByCurrentUser = data.IsLikedByCurrentUser;
        }

        public int GetCacheKey() => Id;

    }
}
