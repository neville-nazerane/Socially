using System;

namespace Socially.Website.Models.RealtimeEventArgs
{
    public class CommentDeletedEventArgs : EventArgs
    {

        public int CommentId { get; set; }

    }
}
