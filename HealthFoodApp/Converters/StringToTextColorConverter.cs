using System.Globalization;

namespace HealthFoodApp.Converters;

public class StringToTextColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return Application.Current.Resources["Gray600"];

        bool isEqual = value.ToString().Equals(parameter.ToString(), StringComparison.OrdinalIgnoreCase);

        // Return primary color if equal, gray if not
        return isEqual ? Application.Current.Resources["Primary"] : Application.Current.Resources["Gray600"];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
