using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VEE.WaterData
{
    public class Water_data
    {
        public double Water_Consumption;
        public double Month_Maximum_Debi;
        public string Consumption_Obis;
        public string Debi_Obis;
        public bool IsValidData;
        public Water_data(bool isValid)
        {
            IsValidData = isValid;
        }

        public Water_data()
        {
            IsValidData = false;
        }

        public Water_data(string Month_Water_Consumption, string Month_Maximum_Debi, bool IsValidData)
        {
            try
            {
                if (string.IsNullOrEmpty(Month_Water_Consumption))
                    this.Water_Consumption = -1;
                else
                {                  
                    this.Water_Consumption = double.Parse(Month_Water_Consumption.Replace("/", "."), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                }

                if (string.IsNullOrEmpty(Month_Maximum_Debi))
                    this.Month_Maximum_Debi = -1;
                else if (Month_Maximum_Debi == "-5")
                    this.Month_Maximum_Debi = -5;
                else
                {
                    if (!string.IsNullOrEmpty(Month_Maximum_Debi))
                        Month_Maximum_Debi = Month_Maximum_Debi.Replace("/", ".");
                    if (Month_Maximum_Debi.Contains(":"))
                    {
                        var d = Month_Maximum_Debi.Split(new char[] { ':' });
                         
                        this.Month_Maximum_Debi = double.Parse(d[0]+"."+d[1], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                    }
                    else
                        this.Month_Maximum_Debi = double.Parse(Month_Maximum_Debi, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                
                this.IsValidData = IsValidData;

            }
            catch (Exception ex)
            {
                this.Water_Consumption = -1;
                this.Month_Maximum_Debi = -1;
                this.IsValidData = false;
            }
        }

        public Water_data GetWaterData()
        {
            Water_data wd = new Water_data();
            wd.Consumption_Obis = this.Consumption_Obis;
            wd.Debi_Obis = this.Debi_Obis;
            wd.IsValidData = this.IsValidData;
            wd.Month_Maximum_Debi = this.Month_Maximum_Debi;
            wd.Water_Consumption = this.Water_Consumption;
            return wd;
        }
    }
}
