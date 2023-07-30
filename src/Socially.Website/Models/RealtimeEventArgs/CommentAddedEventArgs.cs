using Socially.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Website.Models.RealtimeEventArgs
{
    public class CommentAddedEventArgs : EventArgs
    {

        public int PostId { get; init; }

        public int? ParentCommentId { get; init; }

        public DisplayCommentModel Comment { get; init; }

    }
}
