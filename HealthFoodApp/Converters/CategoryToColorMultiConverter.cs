using System.Globalization;

namespace HealthFoodApp.Converters;

public class CategoryToColorMultiConverter : IMultiValueConverter
{
    public Color TrueColor { get; set; }
    public Color FalseColor { get; set; }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is string title && values[1] is string selectedCategory)
        {
            return title == selectedCategory ? TrueColor : FalseColor;
        }

        return FalseColor;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
