using System;
using System.Globalization;

namespace RsaDateTime
{
   public static class ExtensionMethods
    {
        public static string ToPersianString(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2} {3}",
                pc.GetYear(dateTime).ToString("00"),
                pc.GetMonth(dateTime).ToString("00"),
                pc.GetDayOfMonth(dateTime).ToString("00"),
                dateTime.ToString("HH:mm:ss")
                );
        }
        //public static string ToPersianString(int year, int month, int day, int hour=0, int min=0,int sec=0)
        //{
        //    PersianCalendar pc = new PersianCalendar();
        //    return string.Format("{0}/{1}/{2} {3}",
        //        pc.GetYear(dateTime).ToString("00"),
        //        pc.GetMonth(dateTime).ToString("00"),
        //        pc.GetDayOfMonth(dateTime).ToString("00"),
        //        dateTime.ToString("HH:mm:ss")
        //        );
        //}
        public static string ToPersianDateString(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}",
                pc.GetYear(dateTime).ToString("00"),
                pc.GetMonth(dateTime).ToString("00"),
                pc.GetDayOfMonth(dateTime).ToString("00")               
                );
        }

        public static string ToPersianDateString(string  dt)
        {
            var dateTime = Convert.ToDateTime(dt);
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}",
                pc.GetYear(dateTime).ToString("00"),
                pc.GetMonth(dateTime).ToString("00"),
                pc.GetDayOfMonth(dateTime).ToString("00")
                );
        } 
        public static PersianCalendar CurrentPersianDateTime(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc;
        }
        public static string CurrentPersianDateString(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}",
                pc.GetYear(dateTime).ToString("00"),
                pc.GetMonth(dateTime).ToString("00"),
                pc.GetDayOfMonth(dateTime).ToString("00")                
                );
        }

        public static string ToRsaString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public static string ToMiladiString(this DateTime dateTime)
        {
            return dateTime.ToMiladiString();
        }
        
    }
}
