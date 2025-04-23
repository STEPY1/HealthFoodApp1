using System.Globalization;

namespace HealthFoodApp.Converters;

public class BoolToPasswordIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isVisible && parameter is string paramString)
        {
            string[] options = paramString.Split(';');
            if (options.Length == 2)
            {
                return isVisible ? options[0] : options[1];
            }
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
