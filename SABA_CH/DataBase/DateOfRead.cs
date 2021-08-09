using System.Windows;
using System.Windows.Controls;

namespace SABA_CH.DataBase
{
    class DateOfRead : DataTemplateSelector
    {
        public DataTemplate ReadDatelist { get; set; }     
        public override DataTemplate SelectTemplate(object item,
                      DependencyObject container)
        {

            return ReadDatelist;
        }
    }
}
