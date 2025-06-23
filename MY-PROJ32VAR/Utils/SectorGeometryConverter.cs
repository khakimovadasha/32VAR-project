using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using MY_PROJ32VAR.ViewModels;

namespace MY_PROJ32VAR.Utils
{
    public class SectorGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SectorViewModel svm)
            {
                int index = svm.Index;
                int count = svm.TotalSectors;
                double angle = 360.0 / count;
                double startAngle = index * angle;
                double endAngle = startAngle + angle;
                double radius = 50;
                Point center = new Point(60, 60);
                if (parameter is string param && param == "large")
                {
                    radius = 100;
                    center = new Point(110, 110);
                }
                Point start = new Point(center.X + radius * Math.Cos(Math.PI * startAngle / 180), center.Y + radius * Math.Sin(Math.PI * startAngle / 180));
                Point end = new Point(center.X + radius * Math.Cos(Math.PI * endAngle / 180), center.Y + radius * Math.Sin(Math.PI * endAngle / 180));
                bool isLargeArc = angle > 180;
                var geom = new PathGeometry();
                var fig = new PathFigure { StartPoint = center };
                fig.Segments.Add(new LineSegment(start, true));
                fig.Segments.Add(new ArcSegment(end, new Size(radius, radius), 0, isLargeArc, SweepDirection.Clockwise, true));
                fig.Segments.Add(new LineSegment(center, true));
                geom.Figures.Add(fig);
                return geom;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
} 