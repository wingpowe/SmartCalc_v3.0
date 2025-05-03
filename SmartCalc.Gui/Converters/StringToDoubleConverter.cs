using System;
using System.Collections.Generic;
using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SmartCalc.Gui.Converters
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            return 0.0;
        }
    }
}
