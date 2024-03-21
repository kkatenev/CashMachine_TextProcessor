using System;
using System.Globalization;
using System.Windows.Data;

namespace TextProcessor
{
    public class MultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2)
            {
                if (values[0] is int filesCount && values[1] is bool isProcess)
                {
                    return filesCount > 0 && !isProcess;
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
