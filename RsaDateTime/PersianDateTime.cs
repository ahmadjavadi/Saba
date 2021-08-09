using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RsaDateTime
{
  public  class PersianDateTime
    {
        public static DateTime ConvertToGeorgianDateTime(string persianDateTime)
        {
            int year = 0, month = 0, day = 0, hour=0, minutes = 0, second = 0;
            var delimiters = new char[] { ' ', ':', '/' };
            var pd = persianDateTime.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (pd.Length > 0 && pd.Length < 6)
            {
                year = Convert.ToInt32(pd[0]);
                month = Convert.ToInt32(pd[1]);
                day = Convert.ToInt32(pd[2]);
            }
            else if (pd.Length >5)
            {
                year = Convert.ToInt32(pd[0]);
                month = Convert.ToInt32(pd[1]);
                day = Convert.ToInt32(pd[2]);

                hour = Convert.ToInt32(pd[3]);
                minutes = Convert.ToInt32(pd[4]);
                second = Convert.ToInt32(pd[5]);
            }
            else
            {
                throw new Exception("Invalid DateTime Format");
            }

            return new DateTime(year,month,day,hour,minutes,second, new PersianCalendar());
        }
        public static DateTime ConvertToGeorgianDateTime1(string persianDateTime)
        {
            int year = 0, month = 0, day = 0, hour = 0, minutes = 0, second = 0;
            var delimiters = new char[] { ' ', ':', '/' };
            var pd = persianDateTime.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (pd.Length > 0 && pd.Length < 6)
            {
                year = Convert.ToInt32(pd[0]);
                month = Convert.ToInt32(pd[1]);
                day = Convert.ToInt32(pd[2]);
            }
            else if (pd.Length > 5)
            {
                year = Convert.ToInt32(pd[0]);
                month = Convert.ToInt32(pd[1]);
                day = Convert.ToInt32(pd[2]);

                hour = Convert.ToInt32(pd[3]);
                minutes = Convert.ToInt32(pd[4]);
                second = Convert.ToInt32(pd[5]);
            }
            else
            {
                throw new Exception("Invalid DateTime Format");
            }

            return new DateTime(year, month, day, hour, minutes, second);
        }

        public static int  ComparePersianDateTimeStrings(string persianDateTime1, string persianDateTime2)
        {
            return ConvertToGeorgianDateTime(persianDateTime1).CompareTo(ConvertToGeorgianDateTime(persianDateTime2));

        }

        public static string ConvertToPersianDateTime(string ClockValue)
        {
            try
            {
                var d = ClockValue.Split(new char[] { '/', ' ', '-', ':' });
                if (d.Length > 2)
                {
                    if (d[0].Length == 4)
                    {
                        if (d[0].StartsWith("139") || d[0].StartsWith("140"))
                            return ClockValue;
                        if (d.Length >= 6)
                            return new DateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), Convert.ToInt32(d[3]), Convert.ToInt32(d[4]), Convert.ToInt32(d[5])).ToPersianString();
                        else
                            return new DateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), 0, 0, 0).ToPersianString();
                    }
                }
                var dt = Convert.ToDateTime(ClockValue, DateTimeFormatInfo.InvariantInfo);
                var s = dt.ToRsaString();
                if (s.Contains("139") || s.Contains("140"))
                    return s;
 
                return  Convert.ToDateTime(ClockValue).ToPersianString();

            }
            catch (Exception ex)
            {               
                return "1300/01/01 00:00:00";
            }
        }
        public static string ConvertToPersianDateTimeSaba(string ClockValue)
        {
            try
            {
                var d = ClockValue.Split(new char[] { '/', ' ', '-', ':' });
                if (d.Length > 2)
                {
                    if (d[0].Length == 4)
                    {
                        if (d[0].StartsWith("139") || d[0].StartsWith("140"))
                            return ClockValue;
                        if (d.Length >= 6)
                            return new DateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), Convert.ToInt32(d[3]), Convert.ToInt32(d[4]), Convert.ToInt32(d[5])).ToPersianString();
                        else
                            return new DateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), 0, 0, 0).ToPersianString();
                    }
                }

                return "1300/01/01 00:00:00";
            }
            catch (Exception ex)
            {
                return "1300/01/01 00:00:00";
            }
        }
        public static string ConvertToPersianDate(string ClockValue)
        {
            try
            {
                var d = ClockValue.Split(new char[] { '/', ' ', '-', ':' });
                if (d.Length > 2)
                {
                    if (d[0].Length == 4)
                    {
                        if (d[0].StartsWith("139") || d[0].StartsWith("140"))
                            return ClockValue;
                        if (d.Length >= 6)
                            return new DateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), Convert.ToInt32(d[3]), Convert.ToInt32(d[4]), Convert.ToInt32(d[5])).ToPersianDateString();
                        else
                            return new DateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), 0, 0, 0).ToPersianDateString();
                    }
                }
                return Convert.ToDateTime(ClockValue).ToPersianDateString();

            }
            catch (Exception ex)
            {
                return "1300/01/01 00:00:00";
            }
        }
        public static string ConvertToPersianDateSaba(string ClockValue)
        {
            try
            {
                var d = ClockValue.Split(new char[] { '/', ' ', '-', ':' });
                if (d.Length > 2)
                {
                    if (d[0].Length == 4)
                    {
                        if (d[0].StartsWith("139") || d[0].StartsWith("140"))
                            return ClockValue;
                        if (d.Length >= 6)
                            return new DateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), Convert.ToInt32(d[3]), Convert.ToInt32(d[4]), Convert.ToInt32(d[5])).ToPersianDateString();
                        else
                            return new DateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), 0, 0, 0).ToPersianDateString();
                    }
                }
                return "1300/01/01 00:00:00";

            }
            catch (Exception ex)
            {
                return "1300/01/01 00:00:00";
            }
        }

        public static DateTime ConvertToGeorgianDateTime(int year, int month, int day, int hour=0, int minutes=0, int second=0)
        {
            //System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar= new PersianCalendar();
            DateTime dt= new DateTime(year,month,day,hour,minutes,second, new PersianCalendar());


            //CultureInfo cultureInfo = new CultureInfo("en-EN");
            //DateTimeFormatInfo dtfi = cultureInfo.DateTimeFormat;
            //dtfi.Calendar = new GregorianCalendar();
            try
            {
                System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar = new GregorianCalendar();
            }
            catch (Exception)
            {
            }
                        
            return new DateTime(dt.Year,dt.Month,dt.Day,dt.Hour,dt.Minute,dt.Second);
        }

        public static string ConvertTo24HTime(string datetime)
        {
            try
            {
                if (datetime.Contains("ق"))
                {
                    return datetime.Replace(" ق.ظ", "");
                }
                else if (datetime.Contains("ب"))
                {
                    datetime = datetime.Replace(" ب.ظ", "");
                    return Convert.ToDateTime(datetime).AddHours(12).ToString("yyyy/MM/dd HH:mm:ss");

                }
                return datetime;
            }
            catch (Exception ex)
            {
                 return "1300/01/01 00:00:00";
            }            
        }

         
    }
}
