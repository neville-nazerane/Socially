using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Models
{
    public class GroupedUsers : ObservableCollection<DetailedUser>
    {
        public string Name { get; set; }


        public UserType Type { get; set; }

        public GroupedUsers(IEnumerable<DetailedUser> collection, string name) : base(collection)
        {
            Name = name;
            //Order = order;
        }

        //protected override void InsertItem(int index, UserSummaryModel item)
        //{
        //    base.InsertItem(index, item);
        //}

    }
}
