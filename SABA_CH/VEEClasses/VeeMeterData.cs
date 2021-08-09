using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SABA_CH.DataBase;
using SABA_CH.Global;
using VEE;
using VEE.WaterData;
using ShowConsumedWaterForVee_Result = VEE.ShowConsumedWaterForVee_Result;
using ShowVEEConsumedWaterForVEE_Result = VEE.ShowVEEConsumedWaterForVEE_Result;

namespace SABA_CH.VEEClasses
{
   public class VeeMeterData
    {
        public void Vee207data(decimal? MeterID, string meterNo, decimal? customerId)
        {
            try
            {
                //if (customerId == null || customerId == 0)
                //    return;
                //if (meterNo == "20739570")
                //    meterNo = "20739570";
                    ObjectParameter Result = new ObjectParameter("Result", 1000000);
                ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
                string Filter = "  and Main.MeterID=" + MeterID;
                ShowConsumedWaterForVee orginaldata = new ShowConsumedWaterForVee(Filter);
                 
                ShowVeeConsumedWater consumedWater = new ShowVeeConsumedWater(MeterID);
                List<ShowConsumedWaterForVee_Result> lstorginal = new List<ShowConsumedWaterForVee_Result>();
                 

                foreach (var item in orginaldata._lstShowConsumedWaterForVee)
                {
                    ShowConsumedWaterForVee_Result ordata = new ShowConsumedWaterForVee_Result();
                    ordata.MeterNumber = item.MeterNumber;
                    ordata.ReadDate = item.ReadDate.Replace(" ق.ظ", "").Replace(" ب.ظ", "");


                    if (
                        string.IsNullOrEmpty(item.TotalWater1.ToString()) ||
                        string.IsNullOrEmpty(item.W1) ||
                        string.IsNullOrEmpty(item.W2) ||
                        string.IsNullOrEmpty(item.W3) ||
                        string.IsNullOrEmpty(item.W4) ||
                        string.IsNullOrEmpty(item.W5) ||
                        string.IsNullOrEmpty(item.W6) ||
                        string.IsNullOrEmpty(item.W7) ||
                        string.IsNullOrEmpty(item.W8) ||
                        string.IsNullOrEmpty(item.W9) ||
                        string.IsNullOrEmpty(item.W10) ||
                        string.IsNullOrEmpty(item.W11) ||
                        string.IsNullOrEmpty(item.W12) ||
                        string.IsNullOrEmpty(item.W13) ||
                        string.IsNullOrEmpty(item.W14) ||
                        string.IsNullOrEmpty(item.W15) ||
                        string.IsNullOrEmpty(item.W16) ||
                        string.IsNullOrEmpty(item.W17) ||
                        string.IsNullOrEmpty(item.W18) ||
                        string.IsNullOrEmpty(item.W19) ||
                        string.IsNullOrEmpty(item.W20) ||
                        string.IsNullOrEmpty(item.W21) ||
                        string.IsNullOrEmpty(item.W22) ||
                        string.IsNullOrEmpty(item.W23) ||
                        string.IsNullOrEmpty(item.W24) ||
                        string.IsNullOrEmpty(item.Flow1) ||
                        string.IsNullOrEmpty(item.Flow2) ||
                        string.IsNullOrEmpty(item.Flow3) ||
                        string.IsNullOrEmpty(item.Flow4) ||
                        string.IsNullOrEmpty(item.Flow5) ||
                        string.IsNullOrEmpty(item.Flow6) ||
                        string.IsNullOrEmpty(item.Flow7) ||
                        string.IsNullOrEmpty(item.Flow8) ||
                        string.IsNullOrEmpty(item.Flow9) ||
                        string.IsNullOrEmpty(item.Flow10) ||
                        string.IsNullOrEmpty(item.Flow11) ||
                        string.IsNullOrEmpty(item.Flow12) ||
                        string.IsNullOrEmpty(item.Flow13) ||
                        string.IsNullOrEmpty(item.Flow14) ||
                        string.IsNullOrEmpty(item.Flow15) ||
                        string.IsNullOrEmpty(item.Flow16) ||
                        string.IsNullOrEmpty(item.Flow17) ||
                        string.IsNullOrEmpty(item.Flow18) ||
                        string.IsNullOrEmpty(item.Flow19) ||
                        string.IsNullOrEmpty(item.Flow20) ||
                        string.IsNullOrEmpty(item.Flow21) ||
                        string.IsNullOrEmpty(item.Flow22) ||
                        string.IsNullOrEmpty(item.Flow23) ||
                        string.IsNullOrEmpty(item.Flow24)
                        )
                        continue;
                    
                    ordata.TotalWater1 = item.TotalWater1.ToString();

                    ordata.W1 = item.W1;
                    ordata.W2 = item.W2;
                    ordata.W3 = item.W3;
                    ordata.W4 = item.W4;
                    ordata.W5 = item.W5;
                    ordata.W6 = item.W6;
                    ordata.W7 = item.W7;
                    ordata.W8 = item.W8;
                    ordata.W9 = item.W9;
                    ordata.W10 = item.W10;
                    ordata.W11 = item.W11;
                    ordata.W12 = item.W12;
                    ordata.W13 = item.W13;
                    ordata.W14 = item.W14;
                    ordata.W15 = item.W15;
                    ordata.W16 = item.W16;
                    ordata.W17 = item.W17;
                    ordata.W18 = item.W18;
                    ordata.W19 = item.W19;
                    ordata.W20 = item.W20;
                    ordata.W21 = item.W21;
                    ordata.W22 = item.W22;
                    ordata.W23 = item.W23;
                    ordata.W24 = item.W24;
                    ordata.Flow1 = item.Flow1;
                    ordata.Flow2 = item.Flow2;
                    ordata.Flow3 = item.Flow3;
                    ordata.Flow4 = item.Flow4;
                    ordata.Flow5 = item.Flow5;
                    ordata.Flow6 = item.Flow6;
                    ordata.Flow7 = item.Flow7;
                    ordata.Flow8 = item.Flow8;
                    ordata.Flow9 = item.Flow9;
                    ordata.Flow10 = item.Flow10;
                    ordata.Flow11 = item.Flow11;
                    ordata.Flow12 = item.Flow12;
                    ordata.Flow13 = item.Flow13;
                    ordata.Flow14 = item.Flow14;
                    ordata.Flow15 = item.Flow15;
                    ordata.Flow16 = item.Flow16;
                    ordata.Flow17 = item.Flow17;
                    ordata.Flow18 = item.Flow18;
                    ordata.Flow19 = item.Flow19;
                    ordata.Flow20 = item.Flow20;
                    ordata.Flow21 = item.Flow21;
                    ordata.Flow22 = item.Flow22;
                    ordata.Flow23 = item.Flow23;
                    ordata.Flow24 = item.Flow24;
                    lstorginal.Add(ordata);
                }

                 
                Meter_Consumption_Data.ConsumedWaterdata = new List<VEE.VeeConsumedWater>();
                foreach (var item in consumedWater._lstShowVeeConsumedWater)
                {
                    Meter_Consumption_Data.ConsumedWaterdata.Add
                        (new VEE.VeeConsumedWater() { 
                            ConsumedDate=item.ConsumedDate,
                            Flow=item.Flow,
                            IsValid=item.IsValid,
                            MeterId=item.MeterId,
                            CustomerId=item.CustomerId,
                            MonthlyConsumption=item.MonthlyConsumption,
                            TotalConsumption=item.TotalConsumption});
                }
                Vee207 _207;

                for (int i = lstorginal.Count - 1; i > 0; i--)
                {
                    if (i >= lstorginal.Count)
                        i = lstorginal.Count - 1;
                    if (i == 0)
                        break;
                    var dataNew = lstorginal[i - 1];
                    var dataOld = lstorginal[i];

                    var res = dataOld.ReadDate.Substring(0, 7).CompareTo(dataNew.ReadDate.Substring(0, 7));
                    

                    if (ConvertToDecimal(dataOld.TotalWater1) <= ConvertToDecimal(dataNew.TotalWater1) && res == 0)
                    {
                        lstorginal.Remove(dataOld);
                        i++;
                    }

                    else if (res == 1 && ConvertToDecimal(dataOld.TotalWater1) <= ConvertToDecimal(dataNew.TotalWater1))
                    {
                        lstorginal.Remove(dataNew);
                        i++;
                    }
                }
                
                //SaveToExcell(lstorginal, meterNo);

                if (CommonData.GetShutdownInterval)
                    _207 = new Vee207(lstorginal, null, CommonData.MeterShutdownStartDate, CommonData.MeterShutdownEndDate, CommonData.MeterShutdownMonths, CommonData.WasTheMeterOff, meterNo, MeterID);
                else
                    _207 = new Vee207(lstorginal, null, meterNo, MeterID);




                if (Meter_Consumption_Data.ConsumedWaterdata.Count > 0)
                {
                    //SaveToExcell(Meter_Consumption_Data.ConsumedWaterdata, meterNo);
                    try
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("CustomerId");
                        dt.Columns.Add("MeterId");
                        dt.Columns.Add("ConsumedDate");
                        dt.Columns.Add("Flow");
                        dt.Columns.Add("MonthlyConsumption");
                        dt.Columns.Add("TotalConsumption");
                        dt.Columns.Add("IsValid");
                        dt.Columns.Add("PumpWorkingHour");
                        for (int i = 0; i < Meter_Consumption_Data.ConsumedWaterdata.Count; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["CustomerId"] = customerId;
                            dr["MeterId"] = MeterID;
                            dr["ConsumedDate"] = Meter_Consumption_Data.ConsumedWaterdata[i].ConsumedDate;
                            dr["Flow"] = Meter_Consumption_Data.ConsumedWaterdata[i].Flow.Replace("/", ".");
                            dr["MonthlyConsumption"] = Meter_Consumption_Data.ConsumedWaterdata[i].MonthlyConsumption.Replace("/",".");
                            dr["TotalConsumption"] = Meter_Consumption_Data.ConsumedWaterdata[i].TotalConsumption.Replace("/", ".");
                            dr["IsValid"] = true;
                            dr["PumpWorkingHour"] = "";
                            dt.Rows.Add(dr);
                        }
                        SabaNewEntities Bank = new SabaNewEntities();
                        Bank.Database.Connection.Open();
                        SQLSPS.UPDVeeConsumedWater(dt);

                        //foreach (Process proc in Process.GetProcesses())
                        //    if (proc.ProcessName.ToUpper().Equals("EXCEL") && Process.GetCurrentProcess().Id != proc.Id)
                        //    {
                        //        //System.Windows.Forms.MessageBox.Show(proc.ProcessName.ToUpper());
                        //        proc.Kill();

                        //    }

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                CommonData.WriteLOG(ex);

            }
        }


        public void Vee303data(decimal? MeterID, string meterNo, decimal? customerId,string softwareVersion)
        {
            try
            {
                //if (customerId == null || customerId == 0)
                //    return;


                //if (meterNo != "1939300020242")
                //          return;

                if (string.IsNullOrEmpty(softwareVersion) || softwareVersion == "SEWM303")
                {
                    if (meterNo.Length == 11)
                    {
                        softwareVersion = "SEWM303_VI2";
                    }
                    else
                    {
                        softwareVersion = "SEWM303_VI3";

                    }
                }

                if (softwareVersion.Length != 11)
                {
                    switch (softwareVersion)
                    {
                        case "RSASEWM303L0D3051301": softwareVersion = "SEWM303_VD1"; break;
                        case "RSASEWM303L0D3072701": softwareVersion = "SEWM303_VD1"; break;
                        case "RSASEWM303L0D3082701": softwareVersion = "SEWM303_VD1"; break;
                        case "RSASEWM303L0I3082701": softwareVersion = "SEWM303_VI1"; break;
                        case "RSASEWM303L0I3072701": softwareVersion = "SEWM303_VI1"; break;
                        case "RSASEWM303L0I3051301": softwareVersion = "SEWM303_VI1"; break;
                        case "RSASEWM303L0I3071702": softwareVersion = "SEWM303_VI2"; break;
                        case "RSASEWM303L0I3093002": softwareVersion = "SEWM303_VI2"; break;
                        case "RSASEWM303L0D3093002": softwareVersion = "SEWM303_VD2"; break;
                        case "RSASEWM303L0I4120903": softwareVersion = "SEWM303_VI3"; break;
                        case "RSASEWM303L0D4120903": softwareVersion = "SEWM303_VD3"; break;
                        case "RSASEWM303L0D6112104": softwareVersion = "SEWM303_VD4"; break;
                        case "RSASEWM303L0I6112104": softwareVersion = "SEWM303_VI4"; break;
                        default:
                            softwareVersion = "SEWM303_VI4";
                            break;
                    };
                }

                ObjectParameter Result = new ObjectParameter("Result", 1000000);
                ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
                string Filter = "  and Meter.MeterID=" + MeterID;
                ShowConsumedWaterForVee303 orginaldata = new ShowConsumedWaterForVee303(Filter);


                ShowVeeConsumedWater consumedWater = new ShowVeeConsumedWater(MeterID);
                List<VEE.ShowConsumedWaterForVee303_Result> lstorginal = new List<VEE.ShowConsumedWaterForVee303_Result>();

                var org = orginaldata._lstShowConsumedWaterForVee303.OrderByDescending(x => ConvertToDecimal(x.TotalWater1));
                InsertIntoWaterCunsumptionData(softwareVersion, lstorginal, org,false);
                if (lstorginal.Count == 0 && org.Count() > 0)
                {                                        
                    InsertIntoWaterCunsumptionData(softwareVersion, lstorginal, org, true);                                    
                }
                Meter_Consumption_Data303.ConsumedWaterdata = new List<VEE.VeeConsumedWater>();
                foreach (var item in consumedWater._lstShowVeeConsumedWater)
                {
                    Meter_Consumption_Data303.ConsumedWaterdata.Add
                        (new VEE.VeeConsumedWater()
                        {
                            ConsumedDate = item.ConsumedDate,
                            Flow = item.Flow,
                            IsValid = item.IsValid,
                            MeterId = item.MeterId,
                            CustomerId = item.CustomerId,
                            MonthlyConsumption = item.MonthlyConsumption,
                            TotalConsumption = item.TotalConsumption
                        });
  
                }
                Vee303 _303;
               

                //if (meterNo == "20739570")
               //SaveToExcell(lstorginal, meterNo);

                if (CommonData.GetShutdownInterval)
                    _303 = new Vee303(lstorginal, null, CommonData.MeterShutdownStartDate, CommonData.MeterShutdownEndDate, CommonData.MeterShutdownMonths, CommonData.WasTheMeterOff, meterNo, MeterID, softwareVersion);
                else
                    _303 = new Vee303(lstorginal, null, meterNo, MeterID, softwareVersion);

                if (Meter_Consumption_Data303.ConsumedWaterdata.Count > 0)
                {
                   //SaveToExcell(Meter_Consumption_Data303.ConsumedWaterdata, meterNo);
                    try
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("CustomerId");
                        dt.Columns.Add("MeterId");
                        dt.Columns.Add("ConsumedDate");
                        dt.Columns.Add("Flow");
                        dt.Columns.Add("MonthlyConsumption");
                        dt.Columns.Add("TotalConsumption");
                        dt.Columns.Add("IsValid");
                        dt.Columns.Add("PumpWorkingHour");
                        for (int i = 0; i < Meter_Consumption_Data303.ConsumedWaterdata.Count; i++)
                        {
                            if (!(ConvertToDecimal(Meter_Consumption_Data303.ConsumedWaterdata[i].TotalConsumption) > 0))
                                continue;
                            DataRow dr = dt.NewRow();
                            dr["CustomerId"] = customerId;
                            dr["MeterId"] = MeterID;
                            dr["ConsumedDate"] = Meter_Consumption_Data303.ConsumedWaterdata[i].ConsumedDate;
                            dr["Flow"] = "";
                            dr["MonthlyConsumption"] = ReplaceSlash( Meter_Consumption_Data303.ConsumedWaterdata[i].MonthlyConsumption.Replace("/", "."));
                            dr["TotalConsumption"] = ReplaceSlash( Meter_Consumption_Data303.ConsumedWaterdata[i].TotalConsumption.Replace("/", "."));
                            dr["IsValid"] = true;
                            if (string.IsNullOrEmpty(Meter_Consumption_Data303.ConsumedWaterdata[i].Flow )|| Meter_Consumption_Data303.ConsumedWaterdata[i].Flow == "-1")
                                dr["PumpWorkingHour"] = "NA";
                            else
                                dr["PumpWorkingHour"] = ReplaceSlash( Meter_Consumption_Data303.ConsumedWaterdata[i].Flow.Replace("/", "."));
                            dt.Rows.Add(dr);
                        }
                         
                        SQLSPS.UPDVeeConsumedWater(dt);

                        //foreach (Process proc in Process.GetProcesses())
                        //    if (proc.ProcessName.ToUpper().Equals("EXCEL") && Process.GetCurrentProcess().Id != proc.Id)
                        //    {
                        //        //System.Windows.Forms.MessageBox.Show(proc.ProcessName.ToUpper());
                        //        proc.Kill();
                        //    }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                CommonData.WriteLOG(ex);
            }
        }

       

        public void Vee303data_NewCustomer(decimal? MeterID, string meterNo, decimal? customerId, string softwareVersion)
        {
            try
            {

                ObjectParameter Result = new ObjectParameter("Result", 1000000);
                ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
                string Filter = "  and Meter.MeterID=" + MeterID+ " and Main.Valid=1 ";
                Meter_Consumption_Data303.ConsumedWaterdata = new List<VEE.VeeConsumedWater>();

                ShowConsumedWater o1 = new ShowConsumedWater(Filter,1);
                var or1 = o1._lstShowConsumedWaters.Distinct().OrderBy(x => x.ConsumedDate).ToList();
                for (int i = 0; i < or1.Count; i++)
                {
                    var ordata = or1[i];
                    var date= RsaDateTime.ExtensionMethods.ToPersianDateString(ordata.ConsumedDate);
                    var cw = Meter_Consumption_Data303.ConsumedWaterdata.Where(x => x.ConsumedDate == date).FirstOrDefault();
                    if (cw == null)
                    {
                        Meter_Consumption_Data303.ConsumedWaterdata.Insert(0,new VEE.VeeConsumedWater() { ConsumedDate = date,MeterId=MeterID,CustomerId=customerId });
                        if (ordata.OBISDesc.ToLower().Contains("overall consumed volume of water"))
                        {
                            Meter_Consumption_Data303.ConsumedWaterdata[0].TotalConsumption = ordata.ConsumedWater;
                        }
                        else if (ordata.OBISDesc.ToLower().Contains("month consumed water"))
                        {
                            Meter_Consumption_Data303.ConsumedWaterdata[0].MonthlyConsumption = ordata.ConsumedWater;
                        }
                        else if (ordata.OBISDesc.ToLower().Contains("time of pump working"))
                        {
                            Meter_Consumption_Data303.ConsumedWaterdata[0].Flow = ordata.ConsumedWater;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (ordata.OBISDesc.ToLower() .Contains( "overall consumed volume of water"))
                        {
                            if (cw.TotalConsumption !=null && cw.TotalConsumption != ordata.ConsumedWater)
                            {
                            }
                            cw.TotalConsumption = ordata.ConsumedWater;
                        }
                        else if (ordata.OBISDesc.ToLower() .Contains( "month consumed water"))
                        {
                            cw.MonthlyConsumption = ordata.ConsumedWater;
                        }
                        else if (ordata.OBISDesc.ToLower().Contains("time of pump working"))
                        {
                            cw.Flow = ordata.ConsumedWater;
                        }
                        else
                        { 
                        }
                    }
                }         

                if (Meter_Consumption_Data303.ConsumedWaterdata.Count > 0)
                {
                    //SaveToExcell(Meter_Consumption_Data303.ConsumedWaterdata, meterNo);
                    try
                    {
                        if (meterNo.StartsWith("19"))
                        {
                             
                            for (int i =1; i < Meter_Consumption_Data303.ConsumedWaterdata.Count; i++)
                            {
                                Meter_Consumption_Data303.ConsumedWaterdata[i].MonthlyConsumption =
                                    (ConvertToDecimal( Meter_Consumption_Data303.ConsumedWaterdata[i-1].TotalConsumption )- 
                                    ConvertToDecimal(Meter_Consumption_Data303.ConsumedWaterdata[i].TotalConsumption)).ToString();
                                if (Meter_Consumption_Data303.ConsumedWaterdata[i].MonthlyConsumption.Contains("-"))
                                    Meter_Consumption_Data303.ConsumedWaterdata[i].MonthlyConsumption = "Invalid Data";

                            }
                        }

                        ShowVeeConsumedWater consumedWater1 = new ShowVeeConsumedWater(MeterID);
                        var preConsumption = new List<VEE.VeeConsumedWater>();
                        foreach (var item in consumedWater1._lstShowVeeConsumedWater)
                        {
                            preConsumption.Add
                                (new VEE.VeeConsumedWater()
                                {
                                    ConsumedDate = item.ConsumedDate,
                                    Flow = item.Flow,
                                    IsValid = item.IsValid,
                                    MeterId = item.MeterId,
                                    CustomerId = item.CustomerId,
                                    MonthlyConsumption = item.MonthlyConsumption,
                                    TotalConsumption = item.TotalConsumption
                                });
                        }
                        if (preConsumption.Count > 0)
                        {
                            if (Meter_Consumption_Data303.ConsumedWaterdata[0].ConsumedDate.CompareTo(preConsumption[0].ConsumedDate) <= 0)
                            {
                                return;
                            }
                        }

                        DataTable dt = new DataTable();
                        dt.Columns.Add("CustomerId");
                        dt.Columns.Add("MeterId");
                        dt.Columns.Add("ConsumedDate");
                        dt.Columns.Add("Flow");
                        dt.Columns.Add("MonthlyConsumption");
                        dt.Columns.Add("TotalConsumption");
                        dt.Columns.Add("IsValid");
                        dt.Columns.Add("PumpWorkingHour");
                        for (int i = 0; i < Meter_Consumption_Data303.ConsumedWaterdata.Count; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["CustomerId"] = customerId;
                            dr["MeterId"] = MeterID;
                            dr["ConsumedDate"] = Meter_Consumption_Data303.ConsumedWaterdata[i].ConsumedDate;
                            dr["Flow"] = "";
                            dr["MonthlyConsumption"] = Meter_Consumption_Data303.ConsumedWaterdata[i].MonthlyConsumption.Replace("/", ".");
                            dr["TotalConsumption"] = Meter_Consumption_Data303.ConsumedWaterdata[i].TotalConsumption.Replace("/", ".");
                            dr["IsValid"] = true;
                            if (Meter_Consumption_Data303.ConsumedWaterdata[i].Flow == "-1")
                                dr["PumpWorkingHour"] = "NA";
                            else              
                                dr["PumpWorkingHour"] = Meter_Consumption_Data303.ConsumedWaterdata[i].Flow.Replace("/", ".");
                            dt.Rows.Add(dr);
                        }

                        SQLSPS.UPDVeeConsumedWater(dt);

                        //foreach (Process proc in Process.GetProcesses())
                        //    if (proc.ProcessName.ToUpper().Equals("EXCEL") && Process.GetCurrentProcess().Id != proc.Id)
                        //    {
                        //        //System.Windows.Forms.MessageBox.Show(proc.ProcessName.ToUpper());
                        //        proc.Kill();
                        //    }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                CommonData.WriteLOG(ex);
            }
        }

        private static void InsertIntoWaterCunsumptionData(string softwareVersion, List<VEE.ShowConsumedWaterForVee303_Result> lstorginal, IOrderedEnumerable<DataBase.ShowConsumedWaterForVee303_Result> org,bool insertNullData)
        {
            foreach (var item in org)
            {
                VEE.ShowConsumedWaterForVee303_Result ordata = new VEE.ShowConsumedWaterForVee303_Result();
                ordata.MeterNumber = item.MeterNumber;
                ordata.ReadDate = item.ReadDate.Replace(" ق.ظ", "").Replace(" ب.ظ", "");
                
                ordata.CurrentWater1 = ReplaceSlash( item.CurrentWater1);
                ordata.TotalPumpworkingTime1 =ReplaceSlash( item.TotalPumpworkingTime1);
                if (string.IsNullOrEmpty(item.TotalWater1))
                    continue;
                if (!insertNullData)
                {
                    if (string.IsNullOrEmpty(item.W1) &&
                        string.IsNullOrEmpty(item.W2) &&
                        string.IsNullOrEmpty(item.W3) &&
                        string.IsNullOrEmpty(item.W4) &&
                        string.IsNullOrEmpty(item.W5) &&
                        string.IsNullOrEmpty(item.W6) &&
                        string.IsNullOrEmpty(item.W7) &&
                        string.IsNullOrEmpty(item.W8) &&
                        string.IsNullOrEmpty(item.W9) &&
                        string.IsNullOrEmpty(item.W10) &&
                        string.IsNullOrEmpty(item.W11) &&
                        string.IsNullOrEmpty(item.W12) &&
                        string.IsNullOrEmpty(item.W13) &&
                        string.IsNullOrEmpty(item.W14) &&
                        string.IsNullOrEmpty(item.W15) &&
                        string.IsNullOrEmpty(item.W16) &&
                        string.IsNullOrEmpty(item.W17) &&
                        string.IsNullOrEmpty(item.W18) &&
                        string.IsNullOrEmpty(item.W19) &&
                        string.IsNullOrEmpty(item.W20) &&
                        string.IsNullOrEmpty(item.W21) &&
                        string.IsNullOrEmpty(item.W22) &&
                        string.IsNullOrEmpty(item.W23) &&
                        !softwareVersion.EndsWith("1") &&
                        !softwareVersion.EndsWith("2")
                        )
                        continue;

                   

                    if (ConvertToDecimal(item.W1) == 0 &&
                        ConvertToDecimal(item.W2) == 0 &&
                        ConvertToDecimal(item.W3) == 0 &&
                        ConvertToDecimal(item.W4) == 0 &&
                        ConvertToDecimal(item.W5) == 0 &&
                        ConvertToDecimal(item.W6) == 0 &&
                        ConvertToDecimal(item.W7) == 0 &&
                        ConvertToDecimal(item.W8) == 0 &&
                        ConvertToDecimal(item.W9) == 0 &&
                        ConvertToDecimal(item.W10) == 0 &&
                        ConvertToDecimal(item.W11) == 0 &&
                        ConvertToDecimal(item.W12) == 0 &&
                        ConvertToDecimal(item.W13) == 0 &&
                        ConvertToDecimal(item.W14) == 0 &&
                        ConvertToDecimal(item.W15) == 0 &&
                        ConvertToDecimal(item.W16) == 0 &&
                        ConvertToDecimal(item.W17) == 0 &&
                        ConvertToDecimal(item.W18) == 0 &&
                        ConvertToDecimal(item.W19) == 0 &&
                        ConvertToDecimal(item.W20) == 0 &&
                        ConvertToDecimal(item.W21) == 0 &&
                        ConvertToDecimal(item.W22) == 0 &&
                        !softwareVersion.EndsWith("1") &&
                        !softwareVersion.EndsWith("2")
                    )
                        continue;
                }

                ordata.TotalWater1 = item.TotalWater1.Replace("/",".").ToString();

                if (string.IsNullOrEmpty(item.W1) &&
                        string.IsNullOrEmpty(item.W2) &&
                        string.IsNullOrEmpty(item.W3) &&
                        string.IsNullOrEmpty(item.W4) &&
                        string.IsNullOrEmpty(item.W5) &&
                        string.IsNullOrEmpty(item.W6) &&
                        string.IsNullOrEmpty(item.W7) &&
                        string.IsNullOrEmpty(item.W8) &&
                        string.IsNullOrEmpty(item.W9) &&
                        string.IsNullOrEmpty(item.W10) &&
                        string.IsNullOrEmpty(item.W11) &&
                        string.IsNullOrEmpty(item.W12) &&
                        string.IsNullOrEmpty(item.W13) &&
                        string.IsNullOrEmpty(item.W14) &&
                        string.IsNullOrEmpty(item.W15) &&
                        string.IsNullOrEmpty(item.W16) &&
                        string.IsNullOrEmpty(item.W17) &&
                        string.IsNullOrEmpty(item.W18) &&
                        string.IsNullOrEmpty(item.W19) &&
                        string.IsNullOrEmpty(item.W20) &&
                        string.IsNullOrEmpty(item.W21) &&
                        string.IsNullOrEmpty(item.W22) &&
                        string.IsNullOrEmpty(item.W23) &&
                        !softwareVersion.EndsWith("1") &&
                        !softwareVersion.EndsWith("2")
                        )
                    continue;

                
                ordata.W1 = ReplaceSlash(item.W1);                
                ordata.W2 = ReplaceSlash(item.W2);

                
                ordata.W3 = ReplaceSlash(item.W3);
                ordata.W4 = ReplaceSlash(item.W4);
                ordata.W5 = ReplaceSlash(item.W5);
                ordata.W6 = ReplaceSlash(item.W6);
                ordata.W7 = ReplaceSlash(item.W7);
                ordata.W8 = ReplaceSlash(item.W8);
                ordata.W9 = ReplaceSlash(item.W9);
                ordata.W10 =ReplaceSlash(item.W10);
                ordata.W11 =ReplaceSlash(item.W11);
                ordata.W12 =ReplaceSlash(item.W12);
                ordata.W13 =ReplaceSlash(item.W13);
                ordata.W14 =ReplaceSlash(item.W14);
                ordata.W15 =ReplaceSlash(item.W15);
                ordata.W16 =ReplaceSlash(item.W16);
                ordata.W17 =ReplaceSlash(item.W17);
                ordata.W18 =ReplaceSlash(item.W18);
                ordata.W19 =ReplaceSlash(item.W19);
                ordata.W20 =ReplaceSlash(item.W20);
                ordata.W21 =ReplaceSlash(item.W21);
                ordata.W22 =ReplaceSlash(item.W22);
                ordata.W23 = ReplaceSlash( item.W23);

                if (ConvertToDecimal(ordata.W1) == 0 && ConvertToDecimal(ordata.W2) == 0 && ConvertToDecimal(ordata.W3) == 0 && ConvertToDecimal(ordata.W4) == 0 && ConvertToDecimal(ordata.W5) == 0 && ConvertToDecimal(ordata.W6) == 0 &&
                    ConvertToDecimal(ordata.W7) == 0 && ConvertToDecimal(ordata.W8) == 0 && ConvertToDecimal(ordata.W9) == 0 && ConvertToDecimal(ordata.W10) == 0 && ConvertToDecimal(ordata.W11) == 0 && ConvertToDecimal(ordata.W12) == 0 &&
                    ConvertToDecimal(ordata.W13) == 0 && ConvertToDecimal(ordata.W14) == 0 && ConvertToDecimal(ordata.W15) == 0 && ConvertToDecimal(ordata.W16) == 0 && ConvertToDecimal(ordata.W17) == 0 && ConvertToDecimal(ordata.W18) == 0 &&
                    !softwareVersion.EndsWith("1") &&
                        !softwareVersion.EndsWith("2"))
                    continue;

                ordata.PumpworkingTime1 = ReplaceSlash( item.PumpworkingTime1);
                ordata.PumpworkingTime2 = ReplaceSlash( item.PumpworkingTime2);
                ordata.PumpworkingTime3 = ReplaceSlash( item.PumpworkingTime3);
                ordata.PumpworkingTime4 = ReplaceSlash( item.PumpworkingTime4);
                ordata.PumpworkingTime5 = ReplaceSlash( item.PumpworkingTime5);
                ordata.PumpworkingTime6 = ReplaceSlash( item.PumpworkingTime6);
                ordata.PumpworkingTime7 = ReplaceSlash( item.PumpworkingTime7);
                ordata.PumpworkingTime8 = ReplaceSlash( item.PumpworkingTime8);
                ordata.PumpworkingTime9 = ReplaceSlash( item.PumpworkingTime9);
                ordata.PumpworkingTime10 = ReplaceSlash( item.PumpworkingTime10);
                ordata.PumpworkingTime11 = ReplaceSlash( item.PumpworkingTime11);
                ordata.PumpworkingTime12 = ReplaceSlash( item.PumpworkingTime12);
                ordata.PumpworkingTime13 = ReplaceSlash( item.PumpworkingTime13);
                ordata.PumpworkingTime14 = ReplaceSlash( item.PumpworkingTime14);
                ordata.PumpworkingTime15 = ReplaceSlash( item.PumpworkingTime15);
                ordata.PumpworkingTime16 = ReplaceSlash( item.PumpworkingTime16);
                ordata.PumpworkingTime17 = ReplaceSlash( item.PumpworkingTime17);
                ordata.PumpworkingTime18 = ReplaceSlash( item.PumpworkingTime18);
                ordata.PumpworkingTime19 = ReplaceSlash( item.PumpworkingTime19);
                ordata.PumpworkingTime20 = ReplaceSlash( item.PumpworkingTime20);
                ordata.PumpworkingTime21 = ReplaceSlash( item.PumpworkingTime21);
                ordata.PumpworkingTime22 = ReplaceSlash( item.PumpworkingTime22);
                ordata.PumpworkingTime23 = ReplaceSlash( item.PumpworkingTime23);
                lstorginal.Add(ordata);
            }
            for (int i = lstorginal.Count - 1; i > 0; i--)
            {
                if (i >= lstorginal.Count)
                    i = lstorginal.Count - 1;
                if (i == 0)
                    break;
                var dataNew = lstorginal[i - 1];
                var dataOld = lstorginal[i];

                var res = dataOld.ReadDate.Substring(0, 7).CompareTo(dataNew.ReadDate.Substring(0, 7));


                if (string.IsNullOrEmpty(dataOld.TotalWater1) || ConvertToDecimal(dataOld.TotalWater1) <= ConvertToDecimal(dataNew.TotalWater1) && res == 0)
                {
                    lstorginal.Remove(dataOld);
                    i++;
                }

                else if (res == 1 && ConvertToDecimal(dataOld.TotalWater1) <= ConvertToDecimal(dataNew.TotalWater1))
                {
                    lstorginal.Remove(dataNew);
                    i++;
                }
            }

             
        }

        static string ReplaceSlash(string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Replace("/", ".");
            else
            { 
            }
            return value;
        }

        static decimal ConvertToDecimal(string value)
        {
            decimal res;
            try
            {
                if (string.IsNullOrEmpty(value))
                    return 0;
                if (decimal.TryParse(value, out res))
                    return res;
                return ConvertToDecimal(value.Replace(".", "/"));
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        private void SaveToExcell(List<VEE.VeeConsumedWater> lstorginal, string meterNo)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();


                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
                int j = 1;
                for (int i = 0; i < lstorginal.Count; i++)
                {
                    var currentReading = lstorginal[i];
                    //Add table headers going cell by cell.

                    oSheet.Cells[i + 1, 2] = currentReading.ConsumedDate;
                    oSheet.Cells[i + 1, 3] = currentReading.MonthlyConsumption;

                    oSheet.Cells[i + 1, 4] = currentReading.Flow;
                    oSheet.Cells[i + 1, 5] = currentReading.IsValid;

                    oSheet.Cells[i + 1, 6] = currentReading.TotalConsumption;
                }


                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "G1").Font.Bold = true;
                oSheet.get_Range("A1", "G1").VerticalAlignment =
                    Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                // Create an array to multiple values at once.



                oRng = oSheet.get_Range("A1", "G1");
                oRng.NumberFormat = "$0.00";

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "G1");
                oRng.EntireColumn.AutoFit();

                oXL.Visible = false;
                oXL.UserControl = false;
                if (!System.IO.Directory.Exists("D:\\VeeData\\" + meterNo))
                    System.IO.Directory.CreateDirectory("D:\\VeeData\\" + meterNo);
                oWB.SaveAs("D:\\VeeData\\" + meterNo + "\\veefinal " + meterNo + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
                oXL.Quit();
            }
            catch (Exception ex)
            {
            }
        }

      
        private void SaveToExcell(List<VEE.ShowConsumedWaterForVee_Result> lstorginal, string meterNo)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();


                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
                int j = 1;
                for (int i = 0; i < lstorginal.Count; i++)
                {
                    var currentReading = lstorginal[i];
                    //Add table headers going cell by cell.

                    oSheet.Cells[1, j + 1] = currentReading.ReadDate;
                    oSheet.Cells[2, j + 1] = currentReading.TotalWater1;

                    oSheet.Cells[4, j + 1] = currentReading.W1;
                    oSheet.Cells[5, j + 1] = currentReading.W2;
                    oSheet.Cells[6, j + 1] = currentReading.W3;
                    oSheet.Cells[7, j + 1] = currentReading.W4;
                    oSheet.Cells[8, j + 1] = currentReading.W5;
                    oSheet.Cells[9, j + 1] = currentReading.W6;
                    oSheet.Cells[10, j + 1] = currentReading.W7;
                    oSheet.Cells[11, j + 1] = currentReading.W8;
                    oSheet.Cells[12, j + 1] = currentReading.W9;
                    oSheet.Cells[13, j + 1] = currentReading.W10;
                    oSheet.Cells[14, j + 1] = currentReading.W11;
                    oSheet.Cells[15, j + 1] = currentReading.W12;
                    oSheet.Cells[16, j + 1] = currentReading.W13;
                    oSheet.Cells[17, j + 1] = currentReading.W14;
                    oSheet.Cells[18, j + 1] = currentReading.W15;
                    oSheet.Cells[19, j + 1] = currentReading.W16;
                    oSheet.Cells[20, j + 1] = currentReading.W17;
                    oSheet.Cells[21, j + 1] = currentReading.W18;
                    oSheet.Cells[22, j + 1] = currentReading.W19;
                    oSheet.Cells[23, j + 1] = currentReading.W20;
                    oSheet.Cells[24, j + 1] = currentReading.W21;
                    oSheet.Cells[25, j + 1] = currentReading.W22;
                    oSheet.Cells[26, j + 1] = currentReading.W23;
                    oSheet.Cells[27, j + 1] = currentReading.W24;

                    //oSheet.Cells[4, j + 2] = currentReading.Flow1;
                    //oSheet.Cells[5, j + 2] = currentReading.Flow2;
                    //oSheet.Cells[6, j + 2] = currentReading.Flow3;
                    //oSheet.Cells[7, j + 2] = currentReading.Flow4;
                    //oSheet.Cells[8, j + 2] = currentReading.Flow5;
                    //oSheet.Cells[9, j + 2] = currentReading.Flow6;
                    //oSheet.Cells[10, j + 2] = currentReading.Flow7;
                    //oSheet.Cells[11, j + 2] = currentReading.Flow8;
                    //oSheet.Cells[12, j + 2] = currentReading.Flow9;
                    //oSheet.Cells[13, j + 2] = currentReading.Flow10;
                    //oSheet.Cells[14, j + 2] = currentReading.Flow11;
                    //oSheet.Cells[15, j + 2] = currentReading.Flow12;
                    //oSheet.Cells[16, j + 2] = currentReading.Flow13;
                    //oSheet.Cells[17, j + 2] = currentReading.Flow14;
                    //oSheet.Cells[18, j + 2] = currentReading.Flow15;
                    //oSheet.Cells[19, j + 2] = currentReading.Flow16;
                    //oSheet.Cells[20, j + 2] = currentReading.Flow17;
                    //oSheet.Cells[21, j + 2] = currentReading.Flow18;
                    //oSheet.Cells[22, j + 2] = currentReading.Flow19;
                    //oSheet.Cells[23, j + 2] = currentReading.Flow20;
                    //oSheet.Cells[24, j + 2] = currentReading.Flow21;
                    //oSheet.Cells[25, j + 2] = currentReading.Flow22;
                    //oSheet.Cells[26, j + 2] = currentReading.Flow23;
                    //oSheet.Cells[27, j + 2] = currentReading.Flow24;
                    j = j + 2;

                }


                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "G1").Font.Bold = true;
                oSheet.get_Range("A1", "G1").VerticalAlignment =
                    Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                // Create an array to multiple values at once.



                oRng = oSheet.get_Range("A1", "G1");
                oRng.NumberFormat = "$0.00";

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "G1");
                oRng.EntireColumn.AutoFit();

                oXL.Visible = false;
                oXL.UserControl = false;
                if (!System.IO.Directory.Exists("D:\\VeeData\\" + meterNo))
                {
                    System.IO.Directory.CreateDirectory("D:\\VeeData\\" + meterNo);
                    oWB.SaveAs("D:\\VeeData\\" + meterNo + "\\Org data " + meterNo + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                        false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                    oWB.Close();
                    oXL.Quit();
                }
                else
                {
                    oWB.Close(false);
                    oXL.Quit();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SaveToExcell(List<VEE.ShowConsumedWaterForVee303_Result> lstorginal, string meterNo)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();


                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
                int j = 1;
                for (int i = 0; i < lstorginal.Count; i++)
                {
                    var currentReading = lstorginal[i];
                    //Add table headers going cell by cell.

                    oSheet.Cells[1, j + 1] = currentReading.ReadDate;
                    oSheet.Cells[2, j + 1] = currentReading.TotalWater1;

                    oSheet.Cells[4, j + 1] = currentReading.W1;
                    oSheet.Cells[5, j + 1] = currentReading.W2;
                    oSheet.Cells[6, j + 1] = currentReading.W3;
                    oSheet.Cells[7, j + 1] = currentReading.W4;
                    oSheet.Cells[8, j + 1] = currentReading.W5;
                    oSheet.Cells[9, j + 1] = currentReading.W6;
                    oSheet.Cells[10, j + 1] = currentReading.W7;
                    oSheet.Cells[11, j + 1] = currentReading.W8;
                    oSheet.Cells[12, j + 1] = currentReading.W9;
                    oSheet.Cells[13, j + 1] = currentReading.W10;
                    oSheet.Cells[14, j + 1] = currentReading.W11;
                    oSheet.Cells[15, j + 1] = currentReading.W12;
                    oSheet.Cells[16, j + 1] = currentReading.W13;
                    oSheet.Cells[17, j + 1] = currentReading.W14;
                    oSheet.Cells[18, j + 1] = currentReading.W15;
                    oSheet.Cells[19, j + 1] = currentReading.W16;
                    oSheet.Cells[20, j + 1] = currentReading.W17;
                    oSheet.Cells[21, j + 1] = currentReading.W18;
                    oSheet.Cells[22, j + 1] = currentReading.W19;
                    oSheet.Cells[23, j + 1] = currentReading.W20;
                    oSheet.Cells[24, j + 1] = currentReading.W21;
                    oSheet.Cells[25, j + 1] = currentReading.W22;
                    oSheet.Cells[26, j + 1] = currentReading.W23; 

                    //oSheet.Cells[4, j + 2] = currentReading.Flow1;
                    //oSheet.Cells[5, j + 2] = currentReading.Flow2;
                    //oSheet.Cells[6, j + 2] = currentReading.Flow3;
                    //oSheet.Cells[7, j + 2] = currentReading.Flow4;
                    //oSheet.Cells[8, j + 2] = currentReading.Flow5;
                    //oSheet.Cells[9, j + 2] = currentReading.Flow6;
                    //oSheet.Cells[10, j + 2] = currentReading.Flow7;
                    //oSheet.Cells[11, j + 2] = currentReading.Flow8;
                    //oSheet.Cells[12, j + 2] = currentReading.Flow9;
                    //oSheet.Cells[13, j + 2] = currentReading.Flow10;
                    //oSheet.Cells[14, j + 2] = currentReading.Flow11;
                    //oSheet.Cells[15, j + 2] = currentReading.Flow12;
                    //oSheet.Cells[16, j + 2] = currentReading.Flow13;
                    //oSheet.Cells[17, j + 2] = currentReading.Flow14;
                    //oSheet.Cells[18, j + 2] = currentReading.Flow15;
                    //oSheet.Cells[19, j + 2] = currentReading.Flow16;
                    //oSheet.Cells[20, j + 2] = currentReading.Flow17;
                    //oSheet.Cells[21, j + 2] = currentReading.Flow18;
                    //oSheet.Cells[22, j + 2] = currentReading.Flow19;
                    //oSheet.Cells[23, j + 2] = currentReading.Flow20;
                    //oSheet.Cells[24, j + 2] = currentReading.Flow21;
                    //oSheet.Cells[25, j + 2] = currentReading.Flow22;
                    //oSheet.Cells[26, j + 2] = currentReading.Flow23;
                    //oSheet.Cells[27, j + 2] = currentReading.Flow24;
                    j = j + 2;

                }


                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "G1").Font.Bold = true;
                oSheet.get_Range("A1", "G1").VerticalAlignment =
                    Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                // Create an array to multiple values at once.



                oRng = oSheet.get_Range("A1", "G1");
                oRng.NumberFormat = "$0.00";

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "G1");
                oRng.EntireColumn.AutoFit();

                oXL.Visible = false;
                oXL.UserControl = false;
                if (!System.IO.Directory.Exists("D:\\VeeData\\" + meterNo))
                {
                    System.IO.Directory.CreateDirectory("D:\\VeeData\\" + meterNo);
                    oWB.SaveAs("D:\\VeeData\\" + meterNo + "\\Org data " + meterNo + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                        false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    oWB.Close();
                    oXL.Quit();
                }
                else
                {
                    oWB.Close(false);
                    oXL.Quit();
                }
                
            }
            catch (Exception ex)
            {
            }
        }
        private void SaveToExcell(List<Water_Data_Reading_Class> lstorginal, string meterNo)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();


                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
                int j = 1;
                for (int i = 0; i < lstorginal.Count; i++)
                {
                    var currentReading = lstorginal[i];
                    //Add table headers going cell by cell.
                    oSheet.Cells[1, j + 1] = "Reading_Date";
                    oSheet.Cells[2, j + 1] = "TotalWater";
                    oSheet.Cells[1, j + 2] = currentReading.Reading_Date;
                    oSheet.Cells[2, j + 2] = currentReading.TotalWater;

                    oSheet.Cells[3, j + 2] = "Water_Consumption";
                    oSheet.Cells[3, j + 3] = "IsValidData";
                    for (int p = 0; p < currentReading.water_data_Reading_List.Count; p++)
                    {
                        //oSheet.Cells[4+p, j + 1] = currentReading.water_data_Reading_List[p].Month_Maximum_Debi;
                        oSheet.Cells[4 + p, j + 1] = currentReading.water_data_Reading_List[p].Water_Consumption;
                        oSheet.Cells[4 + p, j + 2] = currentReading.water_data_Reading_List[p].IsValidData;
                    }

                    j = j + 4;

                }


                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "G1").Font.Bold = true;
                oSheet.get_Range("A1", "G1").VerticalAlignment =
                    Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                // Create an array to multiple values at once.



                oRng = oSheet.get_Range("A1", "G1");
                oRng.NumberFormat = "$0.00";

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "G1");
                oRng.EntireColumn.AutoFit();

                oXL.Visible = false;
                oXL.UserControl = false;
                if (!System.IO.Directory.Exists("D:\\VeeData\\" + meterNo))
                    System.IO.Directory.CreateDirectory("D:\\VeeData\\" + meterNo);
                oWB.SaveAs("D:\\VeeData\\" + meterNo + "\\valid data " + meterNo + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
                oXL.Quit();
            }
            catch (Exception ex)
            {
            }
        }

        private void SaveToExcell(List<VEE.ShowVEEConsumedWaterForVEE_Result> lstvee, string meterNo)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();


                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
                int j = 1;
                for (int i = 0; i < lstvee.Count; i++)
                {
                    var currentReading = lstvee[i];
                    //Add table headers going cell by cell.
                    oSheet.Cells[1, j + 1] = "Reading_Date";
                    oSheet.Cells[2, j + 1] = "TotalWater";
                    oSheet.Cells[1, j + 2] = currentReading.ReadDate;
                    oSheet.Cells[2, j + 2] = currentReading.ReadDate;

                    oSheet.Cells[4, j + 1] = currentReading.W1;
                    oSheet.Cells[5, j + 1] = currentReading.W2;
                    oSheet.Cells[6, j + 1] = currentReading.W3;
                    oSheet.Cells[7, j + 1] = currentReading.W4;
                    oSheet.Cells[8, j + 1] = currentReading.W5;
                    oSheet.Cells[9, j + 1] = currentReading.W6;
                    oSheet.Cells[10, j + 1] = currentReading.W7;
                    oSheet.Cells[11, j + 1] = currentReading.W8;
                    oSheet.Cells[12, j + 1] = currentReading.W9;
                    oSheet.Cells[13, j + 1] = currentReading.W10;
                    oSheet.Cells[14, j + 1] = currentReading.W11;
                    oSheet.Cells[15, j + 1] = currentReading.W12;
                    oSheet.Cells[16, j + 1] = currentReading.W13;
                    oSheet.Cells[17, j + 1] = currentReading.W14;
                    oSheet.Cells[18, j + 1] = currentReading.W15;
                    oSheet.Cells[19, j + 1] = currentReading.W16;
                    oSheet.Cells[20, j + 1] = currentReading.W17;
                    oSheet.Cells[21, j + 1] = currentReading.W18;
                    oSheet.Cells[22, j + 1] = currentReading.W19;
                    oSheet.Cells[23, j + 1] = currentReading.W20;
                    oSheet.Cells[24, j + 1] = currentReading.W21;
                    oSheet.Cells[25, j + 1] = currentReading.W22;
                    oSheet.Cells[26, j + 1] = currentReading.W23;
                    oSheet.Cells[27, j + 1] = currentReading.W24;

                    oSheet.Cells[4, j + 2] = currentReading.Flow1;
                    oSheet.Cells[5, j + 2] = currentReading.Flow2;
                    oSheet.Cells[6, j + 2] = currentReading.Flow3;
                    oSheet.Cells[7, j + 2] = currentReading.Flow4;
                    oSheet.Cells[8, j + 2] = currentReading.Flow5;
                    oSheet.Cells[9, j + 2] = currentReading.Flow6;
                    oSheet.Cells[10, j + 2] = currentReading.Flow7;
                    oSheet.Cells[11, j + 2] = currentReading.Flow8;
                    oSheet.Cells[12, j + 2] = currentReading.Flow9;
                    oSheet.Cells[13, j + 2] = currentReading.Flow10;
                    oSheet.Cells[14, j + 2] = currentReading.Flow11;
                    oSheet.Cells[15, j + 2] = currentReading.Flow12;
                    oSheet.Cells[16, j + 2] = currentReading.Flow13;
                    oSheet.Cells[17, j + 2] = currentReading.Flow14;
                    oSheet.Cells[18, j + 2] = currentReading.Flow15;
                    oSheet.Cells[19, j + 2] = currentReading.Flow16;
                    oSheet.Cells[20, j + 2] = currentReading.Flow17;
                    oSheet.Cells[21, j + 2] = currentReading.Flow18;
                    oSheet.Cells[22, j + 2] = currentReading.Flow19;
                    oSheet.Cells[23, j + 2] = currentReading.Flow20;
                    oSheet.Cells[24, j + 2] = currentReading.Flow21;
                    oSheet.Cells[25, j + 2] = currentReading.Flow22;
                    oSheet.Cells[26, j + 2] = currentReading.Flow23;
                    oSheet.Cells[27, j + 2] = currentReading.Flow24;
                    j = j + 4;

                }


                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "G1").Font.Bold = true;
                oSheet.get_Range("A1", "G1").VerticalAlignment =
                    Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                // Create an array to multiple values at once.



                oRng = oSheet.get_Range("A1", "G1");
                oRng.NumberFormat = "$0.00";

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "G1");
                oRng.EntireColumn.AutoFit();

                oXL.Visible = false;
                oXL.UserControl = false;
                if (!System.IO.Directory.Exists("D:\\VeeData\\" + meterNo))
                    System.IO.Directory.CreateDirectory("D:\\VeeData\\" + meterNo);
                oWB.SaveAs("D:\\VeeData\\" + meterNo + "\\vee data " + meterNo + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
                oXL.Quit();
            }
            catch (Exception ex)
            {
            }
        }


    }
}
