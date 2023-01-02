using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Converters
{
    public class DateTimeToBoolConverter : ValueConverterBase<DateTime?, bool>
    {
        protected override bool Convert(DateTime? input, object parameter) => input.HasValue;

        protected override DateTime? ConvertBack(bool response, object parameter) => response ? DateTime.Now : null;
    }
}
