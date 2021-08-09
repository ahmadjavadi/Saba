using System.Collections.Generic;
using RsaDateTime;
using VEE.WaterData;

namespace VEE
{
    public class Vee207
    {
        public  Meter_Consumption_Data Meter_Consumption_Data =null;
        
        private PersianDate meterShutdownStartDate;
        private PersianDate meterShutdownEndDate;
        private int meterShutdownMonths;
        private bool? wasTheMeterOff;

        public Vee207(List<ShowConsumedWaterForVee_Result> _LstConsumedDataForVee, List<ShowVEEConsumedWaterForVEE_Result> _lstVEE_Data, string meterNo, decimal? meterId)
        {
            try
            {
                Meter_Consumption_Data.MeterId = meterId;
                Meter_Consumption_Data = new Meter_Consumption_Data();
                Meter_Consumption_Data.SerialNumber = _LstConsumedDataForVee[0].MeterNumber;
                GetMetersConsumptionDataFromDB(_LstConsumedDataForVee, _lstVEE_Data, _LstConsumedDataForVee[0].MeterNumber);
                
                hasError = false;
                Meter_Consumption_Data.VerifyData();
                //if (hasError)
                //{
                //    System.IO.File.AppendAllText("D:\\VeeData\\meters.txt", meterNo + "\r\n");
                //}
                Meter_Consumption_Data.AddOBISToConsumtionData(meterNo);
            }
            catch (System.Exception)
            {
            }
        }
        public static bool hasError;
        
        public Vee207(List<ShowConsumedWaterForVee_Result> lstorginal, List<ShowVEEConsumedWaterForVEE_Result> lstvee, PersianDate meterShutdownStartDate, PersianDate meterShutdownEndDate, int meterShutdownMonths, bool? wasTheMeterOff, string meterNo, decimal? meterId)
        {
            try
            {
                Meter_Consumption_Data.MeterId = meterId;
                this.meterShutdownStartDate = meterShutdownStartDate;
                this.meterShutdownEndDate = meterShutdownEndDate;
                this.meterShutdownMonths = meterShutdownMonths;
                this.wasTheMeterOff = wasTheMeterOff;
                Meter_Consumption_Data = new Meter_Consumption_Data();
                Meter_Consumption_Data.SerialNumber = lstorginal[0].MeterNumber;
                GetMetersConsumptionDataFromDB(lstorginal, lstvee, lstorginal[0].MeterNumber);
                
                hasError = false; 
                Meter_Consumption_Data.VerifyData();
                Meter_Consumption_Data.AddOBISToConsumtionData(meterNo);
                if (hasError)
                {
                   // System.IO.File.AppendAllText("D:\\VeeData\\meters.txt", meterNo + "\r\n");
                }
            }
            catch (System.Exception)
            {
            }            
        }

        public string ShowMeterData()
        {
            string s = "";

            //s = meterData.OrginalWaterDataString();
            s += Meter_Consumption_Data.VEE_WaterDataString();
            s += Meter_Consumption_Data.VEE_Monthly_Consumption_WaterDataString();
            s += Meter_Consumption_Data.VEE_Total_WaterDataString();

            return s;
        }

        private void GetMetersConsumptionDataFromDB(List<ShowConsumedWaterForVee_Result> consumedDataForVee, List<ShowVEEConsumedWaterForVEE_Result> vee_Data, string meterNo)
        {
            foreach (var item in consumedDataForVee)
            {
                Meter_Consumption_Data.Add_To_Orginal_Water_Data_List(item,meterNo);
                bool findItemInVEEData = false;

                //foreach (var item1 in vee_Data)
                //{
                //    if (item1.ReadDate.Equals(item.ReadDate) && !item1.W1.StartsWith("-100"))
                //    {
                //        findItemInVEEData = true;
                //        item1.TotalWater1 = item.TotalWater1;
                //        Meter_Consumption_Data.Add_To_Valid_Water_Data_List(item1,meterNo);
                //    }
                //}

                if (!findItemInVEEData)
                {
                    Meter_Consumption_Data.Add_To_waterDataList(item,meterNo);
                }
            }
        }
    }
}