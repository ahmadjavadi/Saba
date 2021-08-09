using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _303
{
    public class StatusContext
    {
        public static string GetContext(int messageID, string language)
        {
            switch (language)
            {
                case "Farsi":
                    return PersianContext(messageID);
                case "English":
                    return EnglishContext(messageID);
                default:
                    break;
            }
            return null;
        }

    

        private static string PersianContext(int messageID)
        {
            switch (messageID)
            {
                case 0:
                    return "اطلاعات مربوطه در دسترس نیست";
                    
                #region عملکرد کنتور در رویدادهای اعتباری		 	 
                case 1:
                    return "مدت زمانی که کنتور قبل از قطع رله ، به علت اتمام تاریخ پایان اعتبار ، در نظر می گیرد.";
                
                case 2:
                    return "با منفی شدن اعتبار رله قطع می شود.";

                case 3:
                    return "با منفی شدن اعتبار رله قطع نمی شود.";

                case 4:
                    return  "با اتمام تاریخ پایان اعتبار رله قطع می شود. ";

                case 5:
                    return  "با اتمام تاریخ پایان اعتبار رله قطع نمی شود. ";
               
                #endregion عملکرد کنتور در رویدادهای اعتباری

                #region وضعیت قطع رله
                case 6:
                    return "جریان اصلی قطع می باشد.";
                case 7:
                    return "جریان اصلی قطع نمی باشد.";

                case 8:
                    return "رله اصلی ، در حالت قطع از مرکز می باشد.";
                case 9:
                    return "رله اصلی ، در حالت قطع از مرکز نمی باشد.";

                case 10:
                    return "رله اصلی ، در حالت وصل می باشد.";
                case 11:
                    return "رله اصلی ، در حالت وصل نمی باشد.";

                case 12:
                    return "رله اصلی در حالت آماده به وصل می باشد.";
                case 13:
                    return "رله اصلی در حالت آماده به وصل نمی باشد.";


                case 24:
                    return "قطع رله به علت اتمام تاریخ پایان اعتبار می باشد.";
                case 25:
                    return "قطع رله به علت اتمام تاریخ پایان اعتبار می باشد.";

                case 26:
                    return "قطع رله به علت قرار گرفتن در بازه زمانی پیک می باشد.";
                case 27:
                    return "قطع رله به علت قرار گرفتن در بازه زمانی پیک می باشد.";

                case 28:
                    return "قطع رله به علت محدودیت مصرف می باشد. ";
                case 29:
                    return "قطع رله به علت محدودیت مصرف می باشد. ";

                case 30:
                    return " رله به علت انجام عمل soft starter قطع می باشد.";
                case 31:
                    return " رله به علت انجام عمل soft starter قطع می باشد.";

                case 32:
                    return " رله به علت stabilizer قطع می باشد.";
                case 33:
                    return " رله به علت stabilizer قطع می باشد.";

                case 34:
                    return "ر له به علت دمای پایین از کار افتاده است.";
                case 35:
                    return "ر له به علت دمای پایین از کار افتاده است.";
               		 
                #endregion وضعیت قطع رله

                default:
                    break;
            }
            return null;
        }

        private static string EnglishContext(int messageID)
        {
            throw new NotImplementedException();
        }
    }
}
