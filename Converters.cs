using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PicTools
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = Colors.Black;
            if (value is string)
            {
                string val = System.Convert.ToString(value);
                color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(val);
            }
            if (value is Color)
            {
                color = (Color)value;
            }
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush)
            {
                SolidColorBrush brush = (SolidColorBrush)value;
                return brush.Color;
            }
            if (value is Color)
            {
                return value;
            }
            return Colors.Black;
        }
    }
    public class InverseColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = System.Convert.ToString(value);
            Color color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(val);

            if (color.R + color.G + color.B > 400)
                return new SolidColorBrush(Colors.Black);
            else
                return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PercentValueConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null) return 0;

            if (value.Length == 2 && targetType.Name == "Double") {
                double rate = System.Convert.ToDouble(value[0]);
                double baseval = System.Convert.ToDouble(value[1]);

                double result = baseval * rate / 100;

                if (result > baseval) result = baseval;

                return result;
            }

            if (value.Length == 2 && targetType.Name == "CornerRadius")
            {
                double rate = System.Convert.ToDouble(value[0]);
                double baseval = System.Convert.ToDouble(value[1]);

                CornerRadius r = new CornerRadius();
                double val = baseval * rate / 100;
                r.TopLeft = val;
                r.TopRight = val;
                r.BottomLeft = val;
                r.BottomRight = val;
                return r;
            }

            if (value.Length == 3 && targetType.Name == "Thickness")
            {
                double x = System.Convert.ToDouble(value[0]);
                double y = System.Convert.ToDouble(value[1]);
                double baseval = System.Convert.ToDouble(value[2]);

                Thickness t = new Thickness(0.0, 0.0, 0.0, 0.0);
                t.Left = baseval * x / 100;
                t.Top = baseval * y / 100;
                return t;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return FontWeights.Bold;
            }
            return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Point2MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Point p = (Point)value;
            Thickness c = new Thickness(0.0, 0.0, 0.0, 0.0);
            if (p != null)
            {
                c.Top = p.Y;
                c.Left = p.X;
            }
            return c;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Point p = new Point(0, 0);
            Thickness c = (Thickness)value;
            if (c != null)
            {
                p.Y = c.Top;
                p.X = c.Left;
            }
            return p;
        }
    }
    public class PaddingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int p = System.Convert.ToInt32(value);
            return new Thickness(p, p, p, p);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                return (Visibility)value == Visibility.Visible;
            }

            return false;
        }
    }

    public class Rect2ClipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect rect = (Rect)value;
            if (rect != null)
            {
                return new RectangleGeometry(rect);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class Rect2WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect rect = (Rect)value;
            if (rect != null)
            {
                return rect.Width;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class Rect2HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect rect = (Rect)value;
            if (rect != null)
            {
                return rect.Height;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class Rect2MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Rect rect = (Rect)value;
            if (rect != null)
            {
                return new Thickness(-rect.X, 0, 0, 0);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class Alignment2BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = System.Convert.ToString(value);

            if (parameter == null)
            {
                return false;
            }
            string align = System.Convert.ToString(parameter);
            if (align == val)
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = System.Convert.ToBoolean(value);
            string align = System.Convert.ToString(parameter);
            if (isChecked)
            {
                return Enum.Parse(typeof(HorizontalAlignment), align);
            }
            else
            {
                return HorizontalAlignment.Left;
            }

        }
    }
}
