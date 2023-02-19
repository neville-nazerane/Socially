using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Services;
using Socially.MobileApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Components
{
    public class CurrentProfilePic : ProfileImage
    {
        private readonly UserSummaryModel _user;

        public CurrentProfilePic()
        {
            var cacheContext = ServicesUtil.Get<ICachedContext>();
            _user = cacheContext.GetCurrentUser();
        }

        private void User_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            BindingContext = _user.ProfilePicUrl;
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (Parent is null)
                _user.PropertyChanged -= User_PropertyChanged;
            else
            {
                _user.PropertyChanged += User_PropertyChanged;
                BindingContext = _user.ProfilePicUrl;
            }
        }

    }
}
