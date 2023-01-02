using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Models
{
    public class GroupedUsers : ObservableCollection<UserSummaryModel>
    {
        public string Name { get; set; }

        //public int Order { get; set; }

        public GroupedUsers(IEnumerable<UserSummaryModel> collection, string name) : base(collection)
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
