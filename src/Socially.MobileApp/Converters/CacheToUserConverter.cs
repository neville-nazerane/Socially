using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Services;
using Socially.MobileApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Converters
{
    public class CacheToUserConverter : ValueConverterBase<int, object, string>
    {
        ICachedContext cacheContext;

        protected override object Convert(int input, string parameter)
        {
            cacheContext ??= ServicesUtil.Get<ICachedContext>();
            var user = cacheContext.GetUser(input);
            switch (parameter)
            {
                case nameof(UserSummaryModel.FirstName):
                    return user.FirstName;
                case nameof(UserSummaryModel.LastName):
                    return user.LastName;
                case nameof(UserSummaryModel.ProfilePicUrl):
                    return $"{Configs.ImageBase}/userprofiles/{user.ProfilePicUrl}";
                default: throw new Exception($"The parameter '{parameter}' isn't setup for {nameof(CacheToUserConverter)}");
            }
        }

        protected override int ConvertBack(object response, string parameter) 
            => throw new NotImplementedException("UI shouldn't update user cache");
    }
}
