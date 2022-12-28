using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Converters
{
    public class TimeToDurationConverter : ValueConverterBase<DateTime, string>
    {
        protected override string Convert(DateTime input, object parameter)
        {
            var span = DateTime.UtcNow - input;
            if (span.TotalDays > 1)
                return $"{span.Days} days ago";
            if (span.TotalHours > 1)
                return $"{span.Hours} hours ago";
            if (span.Minutes > 1)
                return $"{span.Minutes} minutes ago";
            else return $"{span.Seconds} seconds ago";
        }

        protected override DateTime ConvertBack(string response, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
