using System.Globalization;
using System.Windows.Data;

namespace NeliasBookManager.presentation.Converter
{
    public class StringTointConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() ?? string.Empty;
        }

        // Konverterar från string (UI) till int (ViewModel)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value as string, out int result))
                return result;

            // Om inmatningen är ogiltig, returnera ett standardvärde
            return 0;
        }
    }
}
