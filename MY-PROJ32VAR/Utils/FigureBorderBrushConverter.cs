using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MY_PROJ32VAR.Utils
{
    public class FigureBorderBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = values[0] is bool b1 && b1;
            bool isCorrect = values[1] is bool b2 && b2;
            bool isWrong = values[2] is bool b3 && b3;
            if (isCorrect) return Brushes.LimeGreen;
            if (isWrong) return Brushes.OrangeRed;
            if (isSelected) return Brushes.DodgerBlue;
            return Brushes.LightGray;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
    }
} 