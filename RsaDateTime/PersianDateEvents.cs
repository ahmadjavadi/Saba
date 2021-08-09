using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RsaDateTime
{
   public  class PersianDateEvents
    {
        public static string[] GetDayEventInfo(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            var  oo = JObject.Parse(RsaDateTime.Properties.Resources.events_json);
            string[] getPersianEvents = oo["Persian Calendar"].Where(x => x != null && x.SelectToken("day").ToString() == pc.GetDayOfMonth(date).ToString() &&
            x.SelectToken("month").ToString() == pc.GetMonth(date).ToString() && (x.SelectToken("type").ToString() == "Iran"|| x.SelectToken("type").ToString() == "Islamic Iran"))
               // .Select(m => (string)m.SelectToken("holiday")).ToArray(); 
               .Select(m => (string)m.SelectToken("title")).ToArray();
            return getPersianEvents;
        }
        public static string[] GetDayEventInfo1(string month,string day)
        {
            PersianCalendar pc = new PersianCalendar();
            var oo = JObject.Parse(RsaDateTime.Properties.Resources.events_json);
            string[] getPersianEvents = oo["Persian Calendar"].Where(x => x != null && x.SelectToken("day").ToString() == day.ToString() &&
            x.SelectToken("month").ToString() == month && (x.SelectToken("type").ToString() == "Iran" || x.SelectToken("type").ToString() == "Islamic Iran"))
                .Select(m => (string)m.SelectToken("holiday")).ToArray();
            return getPersianEvents;
        }
    }
}
