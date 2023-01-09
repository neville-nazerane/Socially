using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Converters
{
    public class TimeToDurationConverter : ValueConverterBase<DateTime, string>
    {
        protected override string Convert(DateTime input, object parameter) => input.ToDuration();

        protected override DateTime ConvertBack(string response, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
