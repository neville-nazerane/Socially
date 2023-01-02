using Socially.Mobile.Logic.Models.Mappings;
using Socially.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Models
{
    public partial class UserSummaryModel : ICachable<int, UserSummaryModel>
    {
        public void CopyFrom(UserSummaryModel data) => this.CloneFrom(data);

        public int GetCacheKey() => Id;

    }
}
