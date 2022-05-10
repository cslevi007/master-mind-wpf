using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MasterMindWPF
{
    class StringToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString().Split('_')[0])
            {
                case "blue":
                    return new SolidColorBrush(Colors.Blue);
                case "green":
                    return new SolidColorBrush(Colors.Green);
                case "purple":
                    return new SolidColorBrush(Colors.Purple);
                case "orange":
                    return new SolidColorBrush(Colors.Orange);
                case "red":
                    return new SolidColorBrush(Colors.Red);
                case "pnk":
                    return new SolidColorBrush(Colors.Pink);
                default:
                    return new SolidColorBrush(Colors.Blue);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
