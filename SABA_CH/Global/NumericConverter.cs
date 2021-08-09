using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SABA_CH.Global
{
    public class NumericConverter
    {
        public static int IntConverter(string value)
        {
            return int.Parse(value.Trim(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
        public static double DoubleConverter(string value)
        {
            value = value.Replace("/", ".");
            return double.Parse(value.Trim(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        public static decimal DecimalConverter(string value)
        {
            value = value.Replace("/", ".");
            return decimal.Parse(value.Trim(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        internal static int IntConverter(string value, NumberStyles hexNumber)
        {            
            return int.Parse(value.Trim(),hexNumber,System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }
}
