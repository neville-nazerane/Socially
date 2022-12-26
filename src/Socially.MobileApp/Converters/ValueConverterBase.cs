using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Renderscripts.ScriptGroup;

namespace Socially.MobileApp.Converters
{
    public abstract class ValueConverterBase<TInput, TResponse> : IValueConverter
    {

        protected abstract TResponse Convert(TInput input, object parameter);

        protected abstract TInput ConvertBack(TResponse response, object parameter);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => Convert((TInput)value, parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => ConvertBack((TResponse)value, parameter);
    }

    public abstract class ValueConverterBase<TInput, TResponse, TParameter> : ValueConverterBase<TInput, TResponse>
    {
        protected abstract TResponse Convert(TInput input, TParameter parameter);
        
        protected override TResponse Convert(TInput input, object parameter)
            => Convert(input, (TParameter)parameter);

        protected abstract TInput ConvertBack(TResponse response, TParameter parameter);

        protected override TInput ConvertBack(TResponse response, object parameter)
            => ConvertBack(response, (TParameter)parameter);

    }

}
