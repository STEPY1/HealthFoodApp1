using System.Globalization;

namespace HealthFoodApp.Converters;

public class PercentToWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string percentString && double.TryParse(parameter?.ToString(), out double maxWidth))
        {
            // Remove the '%' symbol if present
            percentString = percentString.Replace("%", "").Trim();

            if (double.TryParse(percentString, out double percent))
            {
                return (percent / 100.0) * maxWidth;
            }
        }

        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

