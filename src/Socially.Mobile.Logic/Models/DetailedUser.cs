using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Models
{
    public class DetailedUser
    {

        public UserType Type { get; set; }

        public UserSummaryModel User { get; set; }

        public DetailedUser(UserType type, UserSummaryModel user)
        {
            Type = type;
            User = user;
        }

    }
}
