using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterStatus
{
    public class GeneralFunction
    {
        public static string ReverseStatusValue(string OBISValue)
        {
            try
            {
                string str = OBISValue.Substring(6, 2) + OBISValue.Substring(4, 2) + OBISValue.Substring(2, 2) + OBISValue.Substring(0, 2);
                return str;
            }
            catch
            {
                return "";
            }
        }

        public static bool ConvertToBolean(string s)
        {
            if (s == "00")
                return false;
            else
                return
                    true;
        }
        public static string ReverseString(string s)
        {
            char[] array = s.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public static string Hex2Binary(string value, int fixedLenght)
        {
            try
            {
                return Convert2FixString(System.Convert.ToString(System.Convert.ToInt16(value, 16), 2), "0", fixedLenght);
            }
            catch
            {
                return Convert2FixString("0", "0", fixedLenght);
            }
        }

        public static string Convert2FixString(string value, string FixChar, int FixNum)
        {
            int i = FixNum - value.Length;
            if (i > 0)
            {
                string mj = RepeatChars(FixChar, i) + value;
                return RepeatChars(FixChar, i) + value;
            }
            else
                return value;
        }

        public static string RepeatChars(string value, int RepeatNum)
        {
            string tmp = "";
            for (int i = 0; i < RepeatNum; i++)
                tmp += value;
            return tmp;
        }
    }
}
