using System;
using Windows.UI.Xaml.Data;

namespace Pomodoro.Converters {

    //https://docs.microsoft.com/en-us/windows/uwp/data-binding/data-binding-in-depth
    public class StringToDoubleConverter : IValueConverter {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language) {
            if (!double.TryParse((string)value, out double StringConvertedToDouble)) {
                return 5M;
            }
            return StringConvertedToDouble;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language) {
            return value.ToString();
        }
    }
}