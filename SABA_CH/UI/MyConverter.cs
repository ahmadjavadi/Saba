using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SABA_CH.UI;

namespace SABA_CH
{
    public class MyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridCell cell = values[0] as DataGridCell;
            clsdatasource item = values[1] as clsdatasource;

            string propertyName = cell.Column.SortMemberPath;
            bool isTrue = System.Convert.ToBoolean(item.GetType().GetProperty(propertyName).GetValue(item, null));
            if (isTrue)
                return new SolidColorBrush(Colors.Green);
            return new SolidColorBrush(Colors.Red);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
