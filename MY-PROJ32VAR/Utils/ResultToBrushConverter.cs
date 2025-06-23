using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MY_PROJ32VAR.Utils
{
    public class ResultToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? Brushes.LightGreen : Brushes.White;
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
