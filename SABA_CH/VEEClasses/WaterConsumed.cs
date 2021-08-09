using System;
using System.Collections.Generic;
using System.Windows;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.VEEClasses
{
    class WaterConsumed
    {
        public List<AllMetersConsumedWaterPivot> _lstAllMetersWaterDetails;
        #region For Corecting Cards
        public void ShowAllWaterDetails()
        {
            try
            {
                ShowObisValueHeader ValueHeder;
                ShowConsumedWaterPivot WaterPivot;
                ShowMeter allMeres = new ShowMeter("");

                _lstAllMetersWaterDetails = new List<AllMetersConsumedWaterPivot>();
                List<AllMetersReadout> _lstAllMetersReadout = new List<AllMetersReadout>();

                foreach (var item in allMeres._lstShowMeters)
                {
                    ValueHeder = new ShowObisValueHeader(" and Main.Meterid=" + item.MeterID);
                    AllMetersConsumedWaterPivot AllReadMeter = new AllMetersConsumedWaterPivot();
                    foreach (var item1 in ValueHeder._lstShowOBISValueHeader)
                    {
                        AllMetersReadout ReadOut = new AllMetersReadout();
                        WaterPivot = new ShowConsumedWaterPivot(" and Main.MeterID=" + item1.MeterID + " and Main.ReadDate= '" + item1.ReadDate.ToString() + "'", "");
                        ReadOut.ConsumedWaterDetailes = WaterPivot._lstShowConsumedWaterPivot;
                        ReadOut.ReadDateMeters = item1.ReadDate;
                        ReadOut.TrasferDateMaters = item1.TransferDate;
                        _lstAllMetersReadout.Add(ReadOut);
                    }

                    AllReadMeter.MetersNumber = item.MeterNumber;
                    AllReadMeter.ConsumedWaterDetailes = _lstAllMetersReadout;
                    _lstAllMetersWaterDetails.Add(AllReadMeter);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        #endregion For Corecting Cards
    }
   
    public class AllMetersConsumedWaterPivot
    {
        public string MetersNumber;
        public List<AllMetersReadout> ConsumedWaterDetailes = new List<AllMetersReadout>();
    }
    public class AllMetersReadout
    {
        public string ReadDateMeters;
        public string TrasferDateMaters;
        public List<ShowConsumedWaterPivot_Result> ConsumedWaterDetailes = new List<ShowConsumedWaterPivot_Result>();
    }
  
}
