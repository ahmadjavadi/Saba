using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SABA_CH
{
    public static class ExtensionMethods
    {
        public static string ToPersianString(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            var  s= string.Format("{0}/{1}/{2} {3}",
                pc.GetYear(dateTime),
                pc.GetMonth(dateTime).ToString("00"),
                pc.GetDayOfMonth(dateTime).ToString("00"),
                dateTime.ToString("HH:mm:ss")
                );
            return s;
        }
        public static string ToPersianDate(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}",
                pc.GetYear(dateTime),
                pc.GetMonth(dateTime).ToString("00"),
                pc.GetDayOfMonth(dateTime).ToString("00")                
                );
        }
        public static string ToPersianString1(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}-{1}-{2} {3}",
                pc.GetYear(dateTime),
                pc.GetMonth(dateTime).ToString("00"),
                pc.GetDayOfMonth(dateTime).ToString("00"),
                dateTime.ToString("HHmmss")
                );
        }

        public static string ToRsaString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public static string ToMiladiString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }

    }
}
