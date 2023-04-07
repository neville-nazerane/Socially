using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Models.PubMessages
{
    class RefreshPostMessage : PublishMessage
    {

        public int PostId { get; init; }

        public RefreshPostMessage() : base(PublishType.RefreshPost)
        {
        }
    }
}
