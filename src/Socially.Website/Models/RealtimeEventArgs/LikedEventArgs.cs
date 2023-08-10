using System;

namespace Socially.Website.Models.RealtimeEventArgs
{
    public class LikedEventArgs : EventArgs
    {

        public int PostId { get; set; }

        public int? CommentId { get; set; }
        public int LikeCount { get; set; }
    }
}
