using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VEE.WaterData
{
    public class Water_Data_Reading_Class
    {

        public string Reading_Date;        
        public string Error = "";
        public string TotalWater;
        public string TotalPumpWorkingTime;
        public string MeterNo;

        public List<Water_data> water_data_Reading_List = new List<Water_data>();

        public Water_Data_Reading_Class()
        { }

        public Water_Data_Reading_Class(string Reading_Date, string Error, string TotalWater,string meterNo, string totalPumpWorkingTime)
        {
            this.Reading_Date = Reading_Date;
            this.MeterNo = meterNo;
            // this.Reading_Time = Reading_Time;            
            this.Error = Error;
            this.TotalWater = TotalWater;
            this.TotalPumpWorkingTime = totalPumpWorkingTime;
        }

        public Water_Data_Reading_Class Get_Water_Data_Reading_Class()
        {
            Water_Data_Reading_Class d = new Water_Data_Reading_Class(this.Reading_Date, this.Error, this.TotalWater,this.MeterNo,this.TotalPumpWorkingTime);
            foreach (var item in this.water_data_Reading_List)
            {
                d.water_data_Reading_List.Add(new Water_data(item.Water_Consumption.ToString(), item.Month_Maximum_Debi.ToString(), item.IsValidData));
            }
            return d;
        }
    }
}