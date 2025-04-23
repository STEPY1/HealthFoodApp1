using System.Globalization;

namespace HealthFoodApp.Converters;

public class StringToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return Colors.Transparent;

        bool isEqual = value.ToString().Equals(parameter.ToString(), StringComparison.OrdinalIgnoreCase);

        // Return white if equal, transparent if not
        return isEqual ? Colors.White : Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
