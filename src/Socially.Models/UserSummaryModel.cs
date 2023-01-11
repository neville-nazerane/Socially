using Socially.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Models
{
    public class UserSummaryModel : ICachable<int, UserSummaryModel>
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ProfilePicUrl { get; set; }

        public void CopyFrom(UserSummaryModel data)
        {
            Id = data.Id;
            UserName = data.UserName;
            FirstName = data.FirstName;
            LastName = data.LastName;
            ProfilePicUrl = data.ProfilePicUrl;
        }

        public int GetCacheKey() => Id;


    }
}
