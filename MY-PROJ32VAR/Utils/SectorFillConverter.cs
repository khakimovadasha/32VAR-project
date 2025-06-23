using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MY_PROJ32VAR.Utils
{
    public class SectorFillConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = (bool)value;
            return isSelected ? Brushes.SteelBlue : Brushes.LightGray;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
} 