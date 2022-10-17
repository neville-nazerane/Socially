using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Models
{
    public class FriendRequestExistsErrorModel
    {


        public int RequesterId { get; set; }

        public DateTime RequestedOn { get; set; }

    }
}
