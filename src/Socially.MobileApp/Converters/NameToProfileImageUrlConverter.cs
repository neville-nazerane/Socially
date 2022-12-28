using Socially.MobileApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Converters
{
    public class NameToProfileImageUrlConverter : ValueConverterBase<string, string>
    {
        protected override string Convert(string input, object parameter)
            => $"{Configs.ImageBase}/userprofiles/{input}";

        protected override string ConvertBack(string response, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
