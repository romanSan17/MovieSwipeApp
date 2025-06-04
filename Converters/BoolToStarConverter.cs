using System.Globalization;

namespace MovieSwipeApp.Converters;

public class BoolToStarConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (value is int r && r > 0) ? new string('★', r) : "☆";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        0;
}
