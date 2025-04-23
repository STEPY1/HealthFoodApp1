using System.Globalization;

namespace HealthFoodApp.Converters;

public class StringEqualConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        bool isEqual = value.ToString().Equals(parameter.ToString(), StringComparison.OrdinalIgnoreCase);

        // If we're converting to a boolean, return the result directly
        if (targetType == typeof(bool))
            return isEqual;

        // Otherwise, check if TrueValue and FalseValue are provided
        var attributes = new[] { "TrueValue", "FalseValue" };
        foreach (var attribute in attributes)
        {
            if (attribute.StartsWith(isEqual ? "True" : "False"))
            {
                var attributeValue = GetAttributeValue(attribute);
                if (attributeValue != null)
                    return attributeValue;
            }
        }

        // Default fallback
        return isEqual;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    private object GetAttributeValue(string attributeName)
    {
        // This is a placeholder - in MAUI, the attribute values are handled by the binding system
        return null;
    }
}
