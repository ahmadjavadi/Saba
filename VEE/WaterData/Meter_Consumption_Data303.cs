using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace VEE.WaterData
{
    public class Meter_Consumption_Data303
    {
        public string SerialNumber { get; set; }
        public static decimal? MeterId { get; internal set; }
        public static List<VeeConsumedWater> ConsumedWaterdata;
        public List<Water_Data_Reading_Class> Non_Valid_WaterDataList;
        public List<Water_Data_Reading_Class> OrginalWaterDataList;
        public List<Water_Data_Reading_Class> Valid_Monthly_Consumption_Data_List;

        public List<Water_Data_Reading_Class> Valid_Total_Consumption_Data_List;

        internal void Add_To_waterDataList(ShowConsumedWaterForVee_Result item,string meterNo)
        {
            Water_Data_Reading_Class WDRCobject = Add_Data_To_Data_Reading_Class_Object(item, true,meterNo);          
            
            if (Non_Valid_WaterDataList == null)
                Non_Valid_WaterDataList = new List<WaterData.Water_Data_Reading_Class>();
            Non_Valid_WaterDataList.Add(WDRCobject);
        }


        internal void Add_To_waterDataList(ShowConsumedWaterForVee303_Result item, string meterNo)
        {
            Water_Data_Reading_Class WDRCobject = Add_Data_To_Data_Reading_Class_Object(item, true, meterNo);

            if (Non_Valid_WaterDataList == null)
                Non_Valid_WaterDataList = new List<WaterData.Water_Data_Reading_Class>();
            Non_Valid_WaterDataList.Add(WDRCobject);
        }

        internal void Add_To_Valid_Water_Data_List(ShowVEEConsumedWaterForVEE_Result item,string meterNo)
        {
            Water_Data_Reading_Class Water_Data_Reading_Class = new Water_Data_Reading_Class(item.ReadDate, "", item.TotalWater1,meterNo,"");
           
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W1, item.Flow1, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W2, item.Flow2, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W3, item.Flow3, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W4, item.Flow4, true));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W5, item.Flow5, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W6, item.Flow6, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W7, item.Flow7, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W8, item.Flow8, true));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W9, item.Flow1, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W10, item.Flow10, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W11, item.Flow11, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W12, item.Flow12, true));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W13, item.Flow13, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W14, item.Flow14, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W15, item.Flow15, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W16, item.Flow16, true));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W17, item.Flow17, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W18, item.Flow18, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W19, item.Flow19, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W20, item.Flow20, true));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W21, item.Flow21, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W22, item.Flow22, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W23, item.Flow23, true));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W24, item.Flow24, true)); 

            if (Valid_Monthly_Consumption_Data_List == null)
                Valid_Monthly_Consumption_Data_List = new List<WaterData.Water_Data_Reading_Class>();
            Valid_Monthly_Consumption_Data_List.Add(Water_Data_Reading_Class);
        }

        internal void Add_To_Orginal_Water_Data_List(ShowConsumedWaterForVee_Result item,string meterNo)
        {
          Water_Data_Reading_Class WDRCobject =  Add_Data_To_Data_Reading_Class_Object(item, true,meterNo);
            

            if (OrginalWaterDataList == null)
                OrginalWaterDataList = new List<WaterData.Water_Data_Reading_Class>();
            OrginalWaterDataList.Add(WDRCobject);
        }
        internal void Add_To_Orginal_Water_Data_List(ShowConsumedWaterForVee303_Result item, string meterNo)
        {
            Water_Data_Reading_Class WDRCobject = Add_Data_To_Data_Reading_Class_Object(item, true, meterNo);


            if (OrginalWaterDataList == null)
                OrginalWaterDataList = new List<WaterData.Water_Data_Reading_Class>();
            OrginalWaterDataList.Add(WDRCobject);
        }

        internal void Add_To_waterDataList(ShowVEEConsumedWaterForVEE_Result data)
        {
            foreach (var item in OrginalWaterDataList)
            {
                if (item.Reading_Date.Equals(data.ReadDate) )
                {
                    Water_Data_Reading_Class WDRC = item.Get_Water_Data_Reading_Class();
                    
                    if (Non_Valid_WaterDataList == null)
                        Non_Valid_WaterDataList = new List<WaterData.Water_Data_Reading_Class>();

                    Non_Valid_WaterDataList.Add(WDRC);
                  
                    break;
                }

            }          
        }

        Water_Data_Reading_Class Add_Data_To_Data_Reading_Class_Object(ShowConsumedWaterForVee_Result item , bool isValid, string meterNo)
        {
            Water_Data_Reading_Class Water_Data_Reading_Class = new Water_Data_Reading_Class(item.ReadDate, "", item.TotalWater1.ToString(),meterNo,"");
            
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W1, item.Flow1, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W2, item.Flow2, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W3, item.Flow3, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W4, item.Flow4, isValid));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W5, item.Flow5, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W6, item.Flow6, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W7, item.Flow7, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W8, item.Flow8, isValid));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W9, item.Flow9, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W10, item.Flow10, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W11, item.Flow11, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W12, item.Flow12, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W13, item.Flow13, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W14, item.Flow14, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W15, item.Flow15, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W16, item.Flow16, isValid));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W17, item.Flow17, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W18, item.Flow18, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W19, item.Flow19, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W20, item.Flow20, isValid));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W21, item.Flow21, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W22, item.Flow22, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W23, item.Flow23, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W24, item.Flow24, isValid));
            return Water_Data_Reading_Class;
        }

        Water_Data_Reading_Class Add_Data_To_Data_Reading_Class_Object(ShowConsumedWaterForVee303_Result item, bool isValid, string meterNo)
        {
            Water_Data_Reading_Class Water_Data_Reading_Class = new Water_Data_Reading_Class(item.ReadDate, "", item.TotalWater1.ToString(), meterNo,item.TotalPumpworkingTime1);

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W1, item.PumpworkingTime1, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W2, item.PumpworkingTime2, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W3, item.PumpworkingTime3, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W4, item.PumpworkingTime4, isValid));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W5, item.PumpworkingTime5, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W6, item.PumpworkingTime6, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W7, item.PumpworkingTime7, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W8, item.PumpworkingTime8, isValid));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W9, item.PumpworkingTime9, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W10, item.PumpworkingTime10, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W11, item.PumpworkingTime11, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W12, item.PumpworkingTime12, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W13, item.PumpworkingTime13, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W14, item.PumpworkingTime14, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W15, item.PumpworkingTime15, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W16, item.PumpworkingTime16, isValid));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W17, item.PumpworkingTime17, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W18, item.PumpworkingTime18, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W19, item.PumpworkingTime19, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W20, item.PumpworkingTime20, isValid));

            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W21, item.PumpworkingTime21, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W22, item.PumpworkingTime22, isValid));
            Water_Data_Reading_Class.water_data_Reading_List.Add(new Water_data(item.W23, item.PumpworkingTime23, isValid)); 
            return Water_Data_Reading_Class;
        }
        internal void VerifyData()
        {
            bool validData = true;
            bool WaterData = true;
            try
            {
                if (Non_Valid_WaterDataList == null)
                    WaterData = false;
                if (Valid_Monthly_Consumption_Data_List == null)
                    validData = false;
                Verify_Consumption_Data303 Verify_Consumption_Data303 = new Verify_Consumption_Data303(OrginalWaterDataList);

                if (WaterData && Non_Valid_WaterDataList.Count > 0) // New Reading Water Data Exist
                {

                    if (!validData || Valid_Monthly_Consumption_Data_List.Count == 0)   //  درصورتیکه داده معتبر نداشته باشیم؟
                    {
                        // همه داده ها باید اعتبار سنجی شده ذخیره شوند
                        if (Non_Valid_WaterDataList.Count > 1)
                        {
                            Valid_Monthly_Consumption_Data_List = Verify_Consumption_Data303.Data_Correction(Non_Valid_WaterDataList, (Water_Data_Reading_Class)Non_Valid_WaterDataList[Non_Valid_WaterDataList.Count - 1]);

                        }
                        else if (Non_Valid_WaterDataList.Count == 1)// فقط یک قرائت داریم
                        {
                            Valid_Monthly_Consumption_Data_List = Non_Valid_WaterDataList;
                            Verify_Consumption_Data303.Data_Correction(Non_Valid_WaterDataList, (Water_Data_Reading_Class)Non_Valid_WaterDataList[Non_Valid_WaterDataList.Count - 1]);
                        }
                    }

                    else
                    {
                        // داده های جدید باید اعتبارسنجی شده و ذخیره شوند 
                        Non_Valid_WaterDataList = Verify_Consumption_Data303.Data_Correction(Non_Valid_WaterDataList, (Water_Data_Reading_Class)Valid_Monthly_Consumption_Data_List[0]);
                        Valid_Monthly_Consumption_Data_List.InsertRange(0, Verify_Consumption_Data303.Data_Correction(Non_Valid_WaterDataList, (Water_Data_Reading_Class)Valid_Monthly_Consumption_Data_List[0]));
                    }
                }

                else //داده جدیدی دریافت نشده است
                {

                }
            }
            catch (Exception ex)
            {
            }
        }
        internal void VerifyData(string softwareVersion)
        { 
            try
            {                 
                Verify_Consumption_Data303 Verify_Consumption_Data303 = new Verify_Consumption_Data303( OrginalWaterDataList);

                Valid_Monthly_Consumption_Data_List = Verify_Consumption_Data303.Data_Correction(Non_Valid_WaterDataList, (Water_Data_Reading_Class)Non_Valid_WaterDataList[Non_Valid_WaterDataList.Count - 1], softwareVersion);
            }
            catch (Exception ex)
            {
            }
        }

        public string VEE_WaterDataString()
        {
            string s = "";
            foreach (var item in Valid_Monthly_Consumption_Data_List)
            {
                s += "\r\n=============   VEE Consumption ===========================\r\n";
                s += item.Reading_Date + " , " + item.TotalWater + "  ";
                s += item.Error + "\r\n";
                foreach (var item1 in item.water_data_Reading_List)
                {
                    if(item1.Water_Consumption < 0)
                        s += " (  NA , NA ," + item1.IsValidData.ToString() + ")\r\n";
                    else
                        s += " (" + item1.Water_Consumption + "," + item1.Month_Maximum_Debi + "," + item1.IsValidData.ToString() + ")\r\n";
                }
            }
            return s;
        }

        public string VEE_Total_WaterDataString()
        {
            string s = "";
            if (Valid_Total_Consumption_Data_List == null)
                return "Null";
            foreach (var item in Valid_Total_Consumption_Data_List)
            {
                s += "\r\n++++++++++++++++   VEE Total Consumption +++++++++++++++++++++++++\r\n";
                s += item.Reading_Date + " \r\n*** " + item.TotalWater + "  ***";
                s += item.Error + "\r\n";
                foreach (var item1 in item.water_data_Reading_List)
                {
                    if (item1.Water_Consumption < 0)
                        s += " (  NA , NA ," + item1.IsValidData.ToString() + ")\r\n";
                    else
                        s += " (" + item1.Consumption_Obis + " : " + item1.Water_Consumption + " , " + item1.Debi_Obis + " : " + item1.Month_Maximum_Debi + "," + item1.IsValidData.ToString() + ")\r\n";
                }
            }
             return s;
        }
        public string VEE_Monthly_Consumption_WaterDataString()
        {
            string s = "";

            if (Valid_Monthly_Consumption_Data_List == null)
                return "Null";


            foreach (var item in Valid_Monthly_Consumption_Data_List)
            {
                s += "\r\n++++++++++++++++   VEE Monthly Consumption +++++++++++++++++++++++++\r\n";
                s += item.Reading_Date + " \r\n*** " + item.TotalWater + "  ***";
                s += item.Error + "\r\n";
                foreach (var item1 in item.water_data_Reading_List)
                {
                    if (item1.Water_Consumption < 0)
                        s += " (  NA , NA ," + item1.IsValidData.ToString() + ")\r\n";
                    else
                        s += " (" + item1.Consumption_Obis + " : " + item1.Water_Consumption + " , " + item1.Debi_Obis + " : " + item1.Month_Maximum_Debi + "," + item1.IsValidData.ToString() + ")\r\n";
                }
            }
            return s;
        }

        public string OrginalWaterDataString()
        {
            string s = "";

            if (OrginalWaterDataList == null)
                return "Null";

            foreach (var item in OrginalWaterDataList)
            {
                s += "\r\n========================================\r\n";
                s += item.Reading_Date + " \r\n*** " + item.TotalWater + "  ***";
                s += item.Error +"\r\n" ;
                foreach (var item1 in item.water_data_Reading_List)
                {
                    s += " (" + item1.Water_Consumption + "," + item1.Month_Maximum_Debi + "," + item1.IsValidData.ToString() + ")\r\n";
                }
            }
            return s;
        }

        internal void AddOBISToConsumtionData(string meterNo)
        {
            Valid_Total_Consumption_Data_List = new List<Water_Data_Reading_Class>();
            string OBISTotal = "0802010000";
            string OBISMonthly = "0802010100";
            string OBISDebi = "0802020500";
            int id = 0;
            if (Valid_Monthly_Consumption_Data_List != null)
                for (int counter = 0; counter < Valid_Monthly_Consumption_Data_List.Count; counter++)
                {
                    try
                    {


                        id = 101;
                        Water_Data_Reading_Class wdrc = new Water_Data_Reading_Class(Valid_Monthly_Consumption_Data_List[counter].Reading_Date,
                            Valid_Monthly_Consumption_Data_List[counter].Error, Valid_Monthly_Consumption_Data_List[counter].TotalWater, meterNo, Valid_Monthly_Consumption_Data_List[counter].TotalPumpWorkingTime);
                        Valid_Total_Consumption_Data_List.Add(wdrc);
                        wdrc.water_data_Reading_List = new List<Water_data>();
                        Water_Data_Reading_Class wdrcMonthly = Valid_Monthly_Consumption_Data_List[counter].Get_Water_Data_Reading_Class();
                        double total = double.Parse(Valid_Monthly_Consumption_Data_List[counter].TotalWater, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

                        for (int i = 0; i < 24; i++)
                        {
                            if (i == 0)
                                id = 255;
                            else if (i == 1)
                                id = 101;
                            else if (id > 124)
                                continue;
                            Water_data wd;


                            wdrcMonthly.water_data_Reading_List[i].Consumption_Obis = OBISMonthly + (id).ToString("X2");
                            wdrcMonthly.water_data_Reading_List[i].Debi_Obis = OBISDebi + (id).ToString("X2");

                            wd = wdrcMonthly.water_data_Reading_List[i].GetWaterData();
                            wd.Consumption_Obis = OBISTotal + (id).ToString("X2");

                            wd.Water_Consumption = Math.Round(total, 3);
                            if (wdrcMonthly.water_data_Reading_List[i].Water_Consumption >= 0)
                            {
                                total = total - double.Parse(wdrcMonthly.water_data_Reading_List[i].Water_Consumption.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                                if (total < 0)
                                    total = 0;
                            }

                            else
                                wd.Water_Consumption = wdrcMonthly.water_data_Reading_List[i].Water_Consumption;

                            id++;

                            wdrc.water_data_Reading_List.Add(wd);
                            Valid_Monthly_Consumption_Data_List[counter] = wdrcMonthly;

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
        }
    }
}