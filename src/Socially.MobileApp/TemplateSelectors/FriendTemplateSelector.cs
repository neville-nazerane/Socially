using Socially.Mobile.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.TemplateSelectors
{
    public class FriendTemplateSelector : DataTemplateSelector
    {

        public DataTemplate FriendTemplate { get; set; }

        public DataTemplate RequestTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is DetailedUser user)
            {
                switch (user.Type)
                {
                    case UserType.Friend:
                        return FriendTemplate;
                    case UserType.Request:
                        return RequestTemplate;
                }
            }
            return null;
        }
    }
}
