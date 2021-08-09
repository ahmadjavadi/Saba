using System;
using System.Linq;
using System.Windows.Markup;

namespace SABA_CH.Converter
{
    public class EnumDescriptionConverter : MarkupExtension
    {
        private readonly Type _type;
        public EnumDescriptionConverter(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type)
                .Cast<object>()
                .Select(e => new { Value = (int)e, DisplayName = e.ToString() });
        }
        //public string GetEnumDescription(Enum enumObj)
        //{
        //    if (enumObj == null)
        //    {
        //        return string.Empty;
        //    }
        //    FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

        //    object[] attribArray = fieldInfo.GetCustomAttributes(false);

        //    if (attribArray.Length == 0)
        //    {
        //        return enumObj.ToString();
        //    }
        //    else
        //    {
        //        DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
        //        return attrib.Description;
        //    }
        //}

        //public override object ProvideValue(IServiceProvider serviceProvider)
        //{
        //    throw new NotImplementedException();
        //}

        //object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    Enum myEnum = (Enum)value;
        //    if (myEnum == null)
        //    {
        //        return null;
        //    }
        //    string description = GetEnumDescription(myEnum);
        //    if (!string.IsNullOrEmpty(description))
        //    {
        //        return description;
        //    }
        //    return myEnum.ToString();
        //}

        //object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    return string.Empty;
        //}
    }

}
