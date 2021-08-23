using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace TryWindow
{
    public class BoolReverseConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !bool.Parse(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !bool.Parse(value.ToString());
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
