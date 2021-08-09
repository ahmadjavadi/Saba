using System.Collections.Generic;
using RsaDateTime;
using VEE.WaterData;


namespace VEE
{
    public class Vee303
    {
        public Meter_Consumption_Data303 Meter_Consumption_Data303 = null;

        private PersianDate meterShutdownStartDate;
        private PersianDate meterShutdownEndDate;
        private int meterShutdownMonths;
        private bool? wasTheMeterOff;

        public Vee303(List<ShowConsumedWaterForVee303_Result> _LstConsumedDataForVee, List<ShowVEEConsumedWaterForVEE_Result> _lstVEE_Data, string meterNo, decimal? meterId, string softwareVersion)
        {
            try
            {
                Meter_Consumption_Data303.MeterId = meterId;
                Meter_Consumption_Data303 = new Meter_Consumption_Data303();
                Meter_Consumption_Data303.SerialNumber = _LstConsumedDataForVee[0].MeterNumber;
                GetMetersConsumptionDataFromDB(_LstConsumedDataForVee, _lstVEE_Data, _LstConsumedDataForVee[0].MeterNumber);

                hasError = false;
                Meter_Consumption_Data303.VerifyData(softwareVersion);
                //if (hasError)
                //{
                //    System.IO.File.AppendAllText("D:\\VeeData\\meters.txt", meterNo + "\r\n");
                //}
                Meter_Consumption_Data303.AddOBISToConsumtionData(meterNo);
            }
            catch (System.Exception)
            {
            }
        }
        public static bool hasError;

        public Vee303(List<ShowConsumedWaterForVee303_Result> lstorginal, List<ShowVEEConsumedWaterForVEE_Result> lstvee, PersianDate meterShutdownStartDate, PersianDate meterShutdownEndDate, int meterShutdownMonths, bool? wasTheMeterOff, string meterNo, decimal? meterId, string softwareVersion)
        {
            try
            {
                Meter_Consumption_Data303.MeterId = meterId;
                this.meterShutdownStartDate = meterShutdownStartDate;
                this.meterShutdownEndDate = meterShutdownEndDate;
                this.meterShutdownMonths = meterShutdownMonths;
                this.wasTheMeterOff = wasTheMeterOff;
                Meter_Consumption_Data303 = new Meter_Consumption_Data303();
                Meter_Consumption_Data303.SerialNumber = lstorginal[0].MeterNumber;
                GetMetersConsumptionDataFromDB(lstorginal, lstvee, lstorginal[0].MeterNumber);

                hasError = false;
                Meter_Consumption_Data303.VerifyData(softwareVersion);
                Meter_Consumption_Data303.AddOBISToConsumtionData(meterNo);
                if (hasError)
                {
                    //System.IO.File.AppendAllText("D:\\VeeData\\meters.txt", meterNo + "\r\n");
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
            s += Meter_Consumption_Data303.VEE_WaterDataString();
            s += Meter_Consumption_Data303.VEE_Monthly_Consumption_WaterDataString();
            s += Meter_Consumption_Data303.VEE_Total_WaterDataString();

            return s;
        }

        private void GetMetersConsumptionDataFromDB(List<ShowConsumedWaterForVee303_Result> consumedDataForVee, List<ShowVEEConsumedWaterForVEE_Result> vee_Data, string meterNo)
        {
            foreach (var item in consumedDataForVee)
            {
                Meter_Consumption_Data303.Add_To_Orginal_Water_Data_List(item, meterNo);
                bool findItemInVEEData = false;

                //foreach (var item1 in vee_Data)
                //{
                //    if (item1.ReadDate.Equals(item.ReadDate) && !item1.W1.StartsWith("-100"))
                //    {
                //        findItemInVEEData = true;
                //        item1.TotalWater1 = item.TotalWater1;
                //        Meter_Consumption_Data303.Add_To_Valid_Water_Data_List(item1,meterNo);
                //    }
                //}

                if (!findItemInVEEData)
                {
                    Meter_Consumption_Data303.Add_To_waterDataList(item, meterNo);
                }
            }
        }
    }
}
