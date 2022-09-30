using Socially.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Managers.Exceptions
{
    public class FriendRequestExistsException : Exception
    {

        public FriendRequestExistsErrorModel Model { get; set; }

        public FriendRequestExistsException(FriendRequestExistsErrorModel model)
        {
            Model = model;
        }
    }
}
