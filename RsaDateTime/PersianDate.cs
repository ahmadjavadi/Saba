using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RsaDateTime
{
    public class PersianDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }

        public PersianDate()
        {

        }
        public PersianDate(int year, int month,int day, int hour=0, int minute=0, int second=0 )
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }
        public String CurrentDate()
        {
            var dateTime = System.DateTime.Now;
            PersianCalendar pc = new PersianCalendar();

            Year = pc.GetYear(dateTime);
            Month = pc.GetMonth(dateTime);
            Day = pc.GetDayOfMonth(dateTime);

            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;
            return string.Format("{0}/{1}/{2} {3}:{4}:{5}",
              Year.ToString("00"),
              Month.ToString("00"),
              Day.ToString("00"),
              Hour.ToString("00"),
              Minute.ToString("00"),
              Second.ToString("00")

               ); 
        }
        public PersianDate Now()
        {
            var dateTime = System.DateTime.Now;
            PersianCalendar pc = new PersianCalendar();

            Year = pc.GetYear(dateTime);
            Month = pc.GetMonth(dateTime);
            Day = pc.GetDayOfMonth(dateTime);

            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;

            return this;
        }
        public PersianDate(DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();

            Year = pc.GetYear(dateTime);
            Month = pc.GetMonth(dateTime);
            Day = pc.GetDayOfMonth(dateTime);

            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;
        }

        public PersianDate ConvertToPersianDate(string persianDateTime)
        {
            try
            {
                var delimiters = new char[] { ' ', ':', '/' };
                var pd = persianDateTime.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                if (pd.Length > 0 && pd.Length < 4)
                {
                    Year = Convert.ToInt32(pd[0]);
                    Month = Convert.ToInt32(pd[1]);
                    Day = Convert.ToInt32(pd[2]);
                }
                else if (pd.Length > 0)
                {
                    Year = Convert.ToInt32(pd[0]);
                    Month = Convert.ToInt32(pd[1]);
                    Day = Convert.ToInt32(pd[2]);
                    if (pd.Length > 3)
                        Hour = Convert.ToInt32(pd[3]);
                    if (pd.Length > 4)
                        Minute = Convert.ToInt32(pd[4]);
                    if(pd.Length>5)
                        Second = Convert.ToInt32(pd[5]);
                }
                else
                {
                    throw new Exception("Invalid DateTime Format");
                }
                return this;
            }
            catch (Exception)
            {
                throw new Exception("Invalid DateTime Format");
            }           
           
        }


        public DateTime ConvertToGeorgianDateTime()
        {
            try
            {
                return new DateTime(Year, Month, Day, Hour, Minute, Second, new PersianCalendar());
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        public DateTime ConvertToGeorgianDateTime(string persianDateTime)
        {
            try
            {
                int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0;
                var delimiters = new char[] { ' ', ':', '/' };
                var pd = persianDateTime.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                if (pd.Length > 0 && pd.Length < 4)
                {
                    year = Convert.ToInt32(pd[0]);
                    month = Convert.ToInt32(pd[1]);
                    day = Convert.ToInt32(pd[2]);
                }
                else if (pd.Length > 0)
                {
                    year = Convert.ToInt32(pd[0]);
                    month = Convert.ToInt32(pd[1]);
                    day = Convert.ToInt32(pd[2]);

                    hour = Convert.ToInt32(pd[3]);
                    minute = Convert.ToInt32(pd[4]);
                    second = Convert.ToInt32(pd[5]);
                }
                else
                {
                    throw new Exception("Invalid DateTime Format");
                }

                return new DateTime(year, month, day, hour, minute, second, new PersianCalendar());
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int ComparePersianDateTimeStrings(string persianDateTime1, string persianDateTime2)
        {
            return ConvertToGeorgianDateTime(persianDateTime1).CompareTo(ConvertToGeorgianDateTime(persianDateTime2));         
        }

        public void AddYear(int value)
        {
            var dateTime = ConvertToGeorgianDateTime().AddYears(value);
            PersianCalendar pc = new PersianCalendar();

            Year = pc.GetYear(dateTime);
            Month = pc.GetMonth(dateTime);
            Day = pc.GetDayOfMonth(dateTime);

            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}/{2} {3}:{4}:{5}",
                 Year.ToString("00"),
                 Month.ToString("00"),
                 Day.ToString("00"),
                 Hour.ToString("00"),
                 Minute.ToString("00"), 
                 Second.ToString("00")

                  );
        }
        public  string ToDateString()
        {
            return string.Format("{0}/{1}/{2} {3}:{4}:{5}",
                 Year.ToString("00"),
                 Month.ToString("00"),
                 Day.ToString("00")
                

                  );
        }

        public void AddDays(int d)
        {
            var dateTime = ConvertToGeorgianDateTime();
            dateTime.AddDays(d);
            PersianCalendar pc = new PersianCalendar();

            Year = pc.GetYear(dateTime);
            Month = pc.GetMonth(dateTime);
            Day = pc.GetDayOfMonth(dateTime);

            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;
        }


        public void AddMonths(int m)
        {
            var dateTime = ConvertToGeorgianDateTime();
            dateTime.AddMonths(m);
            PersianCalendar pc = new PersianCalendar();

            Year = pc.GetYear(dateTime);
            Month = pc.GetMonth(dateTime);
            Day = pc.GetDayOfMonth(dateTime);

            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;
        }

        public void AddYears(int y)
        {
            var dateTime = ConvertToGeorgianDateTime();
            dateTime.AddMonths(y);
            PersianCalendar pc = new PersianCalendar();

            Year = pc.GetYear(dateTime);
            Month = pc.GetMonth(dateTime);
            Day = pc.GetDayOfMonth(dateTime);

            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;
        }
    }
}
