namespace SABA_CH.Token
{
    class CalendarConvert
    {
        // Structure for holding miladi dates elements
        public struct MiladiDate
        {
            public int year;
            public int month;
            public int day;
        }

        // Structure for holding shamsi dates elements
        public struct ShamsiDate
        {
            public int year;
            public int month;
            public int day;
        }

        // This function is used for converting miladi dates to shamsi dates.
        public ShamsiDate MiladiToShamsi(MiladiDate miladiDate)
        {
            int shamsiDay = 1, shamsiMonth = 1, shamsiYear;
            int dayCount, farvardinDayDiff, deyDayDiff;
            int[] sumDayMiladmonth = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            int[] sumDayMiladmonthLeap = { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335 };
            ShamsiDate shamsiDate;

            farvardinDayDiff = 79;
            if (MiladiIsLeap(miladiDate.year))
                dayCount = sumDayMiladmonthLeap[miladiDate.month - 1] + miladiDate.day;
            else
                dayCount = sumDayMiladmonth[miladiDate.month - 1] + miladiDate.day;
            deyDayDiff = MiladiIsLeap(miladiDate.year - 1) ? 11 : 10;
            if (dayCount > farvardinDayDiff)
            {
                dayCount = dayCount - farvardinDayDiff;
                if (dayCount <= 186)
                {
                    switch (dayCount % 31)
                    {
                        case 0:
                            shamsiMonth = dayCount / 31;
                            shamsiDay = 31;
                            break;
                        default:
                            shamsiMonth = (dayCount / 31) + 1;
                            shamsiDay = (dayCount % 31);
                            break;
                    }
                    shamsiYear = miladiDate.year - 621;
                }
                else
                {
                    dayCount = dayCount - 186;
                    switch (dayCount % 30)
                    {
                        case 0:
                            shamsiMonth = (dayCount / 30) + 6;
                            shamsiDay = 30;
                            break;
                        default:
                            shamsiMonth = (dayCount / 30) + 7;
                            shamsiDay = (dayCount % 30);
                            break;
                    }
                    shamsiYear = miladiDate.year - 621;
                }
            }
            else
            {
                dayCount = dayCount + deyDayDiff;
                switch (dayCount % 30)
                {
                    case 0:
                        shamsiMonth = (dayCount / 30) + 9;
                        shamsiDay = 30;
                        break;
                    default:
                        shamsiMonth = (dayCount / 30) + 10;
                        shamsiDay = (dayCount % 30);
                        break;
                }
                shamsiYear = miladiDate.year - 622;
            }
            shamsiDate.year = shamsiYear;
            shamsiDate.month = shamsiMonth;
            shamsiDate.day = shamsiDay;

            return shamsiDate;
        }

        // This function is used for converting shamsi dates to miladi dates.


        public MiladiDate ShamsiToMiladi(ShamsiDate shamsiDate)
        {
            MiladiDate miladiDate, farvardin1st;
            int marchDayDiff, remainDay = 0;
            int dayCount, miladiMonth, miladiYear, i;

            // This buffer has day count of Miladi month from April to January for a none year.
            int[] miladmonth = { 30, 31, 30, 31, 31, 30, 31, 30, 31, 31, 28, 31 };
            miladiYear = shamsiDate.year + 621;

            // Detemining the Farvardin the First
            if (MiladiIsLeap(miladiYear))
            {
                // This is a Miladi leap year so Shamsi is leap too so the 1st of Farvardin is March 20 (3/20) 
                farvardin1st.month = 3;
                farvardin1st.day = 20;
                marchDayDiff = 12;
            }
            else
            {
                // This is not a Miladi leap year so Shamsi is not leap too so the 1st of Farvardin is March 21 (3/21) 
                farvardin1st.month = 3;
                farvardin1st.day = 21;
                marchDayDiff = 11;
            }

            // If next year is leap we will add one day to Feb.
            if (MiladiIsLeap(miladiYear + 1))
                miladmonth[10] = miladmonth[10] + 1; //Adding one day to Feb

            // Calculate the day count for input shamsi date from 1st Farvadin
            if ((shamsiDate.month >= 1) && (shamsiDate.month <= 6))
                dayCount = ((shamsiDate.month - 1) * 31) + shamsiDate.day;
            else
                dayCount = (6 * 31) + ((shamsiDate.month - 7) * 30) + shamsiDate.day;

            // Finding the correspond miladi month and day
            if (dayCount <= marchDayDiff) //So we are in 20(for leap year) or 21for none leap year) to 31 march
            {
                miladiDate.day = dayCount + (31 - marchDayDiff);
                miladiDate.month = 3;
                miladiDate.year = miladiYear;
            }
            else
            {
                remainDay = dayCount - marchDayDiff;

                i = 0; //starting from April
                while ((remainDay > miladmonth[i]))
                {
                    remainDay = remainDay - miladmonth[i];
                    i++;
                }
                miladiDate.day = remainDay;

                if (i > 8) // We are in the next Miladi Year
                {
                    miladiMonth = i - 8;
                    miladiYear++;
                }
                else
                    miladiMonth = i + 4;
                miladiDate.month = miladiMonth;
                miladiDate.year = miladiYear;

            }
            return miladiDate;
        }

        // This function check a miladiYear is leap or not.
        private bool MiladiIsLeap(int year)
        {
            if (((year % 100) != 0 && (year % 4) == 0) || ((year % 100) == 0 && (year % 400) == 0))
                return true;
            else
                return false;
        }
    }
}
