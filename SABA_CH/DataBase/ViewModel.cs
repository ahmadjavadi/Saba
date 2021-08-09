using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Windows;
using System.Windows.Data;
using MeterStatus;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH
{
    public class ShowOBISs
    {
        
        public ICollectionView CollectShowObiSs { get; private set; }
        public List<ShowOBISs_Result> _lstShowOBISs;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowOBISs(string filte)
        {
            try
            {

                Bank.Database.Connection.Open();
                _lstShowOBISs = new List<ShowOBISs_Result>();
                foreach (ShowOBISs_Result item in Bank.ShowOBISs(filte, CommonData.LanguagesID))
                    _lstShowOBISs.Add(item);
                CollectShowObiSs = CollectionViewSource.GetDefaultView(_lstShowOBISs);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }
        }



    }
    public class ShowDiatinctObiSs
    {
        public ICollectionView CollectShowDiatinctObiSs { get; private set; }
        public List<ShowDiatinctOBISs_Result> _lstShowDiatinctOBISs;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowDiatinctObiSs(string filte)
        {
            try
            {
                Bank.Database.Connection.Open();
                _lstShowDiatinctOBISs = new List<ShowDiatinctOBISs_Result>();
                foreach (ShowDiatinctOBISs_Result item in Bank.ShowDiatinctOBISs(filte, CommonData.LanguagesID))
                    _lstShowDiatinctOBISs.Add(item);
                CollectShowDiatinctObiSs = CollectionViewSource.GetDefaultView(_lstShowDiatinctOBISs);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }

        }
    }
    public class ShowObiSsDesc
    {
        public ICollectionView CollectShowObiSsDesc { get; private set; }
        public List<ShowOBISsDesc_Result> _lstShowOBISsDesc;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowObiSsDesc(string filter)
        {
            try
            {
                Bank.Database.Connection.Open();
                _lstShowOBISsDesc = new List<ShowOBISsDesc_Result>();
                foreach (ShowOBISsDesc_Result item in Bank.ShowOBISsDesc(filter, CommonData.LanguagesID))
                    _lstShowOBISsDesc.Add(item);
                CollectShowObiSsDesc = CollectionViewSource.GetDefaultView(_lstShowOBISsDesc);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
    }
    public class ShowDangelInfo
    {
        public ICollectionView CollectShowDangelInfo { get; private set; }
        public List<ShowDangelInfo_Result> _lstShowDangelInfo;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowDangelInfo(string filter)
        {
            try
            {
                Bank.Database.Connection.Open();
                _lstShowDangelInfo = new List<ShowDangelInfo_Result>();
                foreach (ShowDangelInfo_Result item in Bank.ShowDangelInfo(filter))
                    _lstShowDangelInfo.Add(item);
                CollectShowDangelInfo = CollectionViewSource.GetDefaultView(_lstShowDangelInfo);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
    }
    public class ShowMeter
    {
        public ICollectionView CollectShowMeters { get; private set; }
        public List<ShowMeter_Result> _lstShowMeters;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowMeter(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowMeters = new List<ShowMeter_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowMeter_Result item in Bank.ShowMeter(filter, CommonData.UserID))
                _lstShowMeters.Add(item);
            CollectShowMeters = CollectionViewSource.GetDefaultView(_lstShowMeters);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowConsumedWater207
    {
        public ICollectionView CollectShowConsumedWater207S { get; private set; }
        public List<ShowConsumedWater207_Result> _lstShowConsumedWater207s;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowConsumedWater207(decimal? meterId)
        {
            Bank.Database.Connection.Open();
            _lstShowConsumedWater207s = new List<ShowConsumedWater207_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowConsumedWater207_Result item in Bank.ShowConsumedWater207(meterId, CommonData.LanguagesID))
                //if(item.ErrorMessage=="")
                 _lstShowConsumedWater207s.Add(item);
            CollectShowConsumedWater207S = CollectionViewSource.GetDefaultView(_lstShowConsumedWater207s);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowESubOffice
    {
        public ICollectionView CollectShowESubOffices { get; private set; }
        public List<ShowESubOffice_Result> _lstShowESubOffices;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowESubOffice(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowESubOffices = new List<ShowESubOffice_Result>();
            foreach (ShowESubOffice_Result item in Bank.ShowESubOffice(filter))
                _lstShowESubOffices.Add(item);
            CollectShowESubOffices = CollectionViewSource.GetDefaultView(_lstShowESubOffices);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowSubOffice
    {
        public ICollectionView CollectShowSubOffices { get; private set; }
        public List<ShowSubOffice_Result> _lstShowSubOffices;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowSubOffice(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowSubOffices = new List<ShowSubOffice_Result>();
            foreach (ShowSubOffice_Result item in Bank.ShowSubOffice(filter))
                _lstShowSubOffices.Add(item);
            CollectShowSubOffices = CollectionViewSource.GetDefaultView(_lstShowSubOffices);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowChangeDb
    {
        public ICollectionView CollectShowSubOffices { get; private set; }
        public List<ShowChangeDB_Result> _lstShowChangeDB;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowChangeDb(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowChangeDB = new List<ShowChangeDB_Result>();
            foreach (ShowChangeDB_Result item in Bank.ShowChangeDB(filter))
                _lstShowChangeDB.Add(item);
            CollectShowSubOffices = CollectionViewSource.GetDefaultView(_lstShowChangeDB);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowEOffice
    {
        public ICollectionView CollectShowEOffices { get; private set; }
        public List<ShowEOffice_Result> _lstShowEOffices;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowEOffice(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowEOffices = new List<ShowEOffice_Result>();
            foreach (ShowEOffice_Result item in Bank.ShowEOffice(filter))
                _lstShowEOffices.Add(item);
            CollectShowEOffices = CollectionViewSource.GetDefaultView(_lstShowEOffices);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowConsumedWater
    {
        public ICollectionView CollectShowConsumedWaters { get; private set; }
        public List<ShowConsumedWater_Result> _lstShowConsumedWaters;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowConsumedWater(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowConsumedWaters = new List<ShowConsumedWater_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowConsumedWater_Result item in Bank.ShowConsumedWater(filter, CommonData.LanguagesID, CommonData.UserID))
                _lstShowConsumedWaters.Add(item);
            CollectShowConsumedWaters = CollectionViewSource.GetDefaultView(_lstShowConsumedWaters);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
        public ShowConsumedWater(string filter, decimal? languagesId = 1 )
        {
            Bank.Database.Connection.Open();
            _lstShowConsumedWaters = new List<ShowConsumedWater_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowConsumedWater_Result item in Bank.ShowConsumedWater(filter, languagesId, CommonData.UserID))
                _lstShowConsumedWaters.Add(item);
            CollectShowConsumedWaters = CollectionViewSource.GetDefaultView(_lstShowConsumedWaters);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowCurve
    {
        public ICollectionView CollectShowConsumedWaters { get; private set; }
        public List<ShowCurve_Result> _lstShowCurve;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowCurve(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowCurve = new List<ShowCurve_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowCurve_Result item in Bank.ShowCurve(filter))
                _lstShowCurve.Add(item);
            CollectShowConsumedWaters = CollectionViewSource.GetDefaultView(_lstShowCurve);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowConsumedActiveEnergy
    {
        public ICollectionView CollectShowConsumedActiveEnergys { get; private set; }
        public List<ShowConsumedActiveEnergy_Result> _lstShowConsumedActiveEnergys;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowConsumedActiveEnergy(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowConsumedActiveEnergys = new List<ShowConsumedActiveEnergy_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowConsumedActiveEnergy_Result item in Bank.ShowConsumedActiveEnergy(filter, CommonData.LanguagesID, CommonData.UserID))
                _lstShowConsumedActiveEnergys.Add(item);
            CollectShowConsumedActiveEnergys = CollectionViewSource.GetDefaultView(_lstShowConsumedActiveEnergys);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowCardTOMeter
    {
        public ICollectionView CollectShowCardToMeter { get; private set; }
        public List<ShowCardTOMeter_Result> _lstShowCardTOMeter;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowCardTOMeter(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowCardTOMeter = new List<ShowCardTOMeter_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowCardTOMeter_Result item in Bank.ShowCardTOMeter(filter, ""))
                _lstShowCardTOMeter.Add(item);
            CollectShowCardToMeter = CollectionViewSource.GetDefaultView(_lstShowCardTOMeter);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowAllCardsToMeters
    {
        public ICollectionView CollectShowAllCardsToMeters { get; private set; }
        public List<ShowALLCardsTOMeters_Result> _lstShowAllCardsTOMeters;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowAllCardsToMeters(string filter)
        {
           
            Bank.Database.Connection.Open();
            _lstShowAllCardsTOMeters = new List<ShowALLCardsTOMeters_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowALLCardsTOMeters_Result item in Bank.ShowALLCardsTOMeters(filter))
                _lstShowAllCardsTOMeters.Add(item);
            CollectShowAllCardsToMeters = CollectionViewSource.GetDefaultView(_lstShowAllCardsTOMeters);

            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowReports
    {
        public ICollectionView CollectShowReports { get; private set; }
        public List<ShowReports_Result> _lstShowReports;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowReports(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowReports = new List<ShowReports_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowReports_Result item in Bank.ShowReports(filter))
                _lstShowReports.Add(item);
            CollectShowReports = CollectionViewSource.GetDefaultView(_lstShowReports);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
   

    public class ShowObisToReport
    {
        public ICollectionView CollectShowObisToReport { get; private set; }
        public List<ShowOBISToReport_Result> _lstShowOBISToReport;

        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowObisToReport(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowOBISToReport = new List<ShowOBISToReport_Result>();
            foreach (ShowOBISToReport_Result item in Bank.ShowOBISToReport(filter))
                _lstShowOBISToReport.Add(item);
            CollectShowObisToReport = CollectionViewSource.GetDefaultView(_lstShowOBISToReport);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }


    public class ShowObisToExcel
    {
        public ICollectionView CollectShowObisToExcel { get; private set; }
        public List<ShowOBISToExcel_Result> _lstShowObisToExcel;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowObisToExcel(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowObisToExcel = new List<ShowOBISToExcel_Result>();
            foreach (ShowOBISToExcel_Result item in Bank.ShowOBISToExcel(filter))
                _lstShowObisToExcel.Add(item);
            CollectShowObisToExcel = CollectionViewSource.GetDefaultView(_lstShowObisToExcel);
            Bank.Database.Connection.Close();
            Bank.Dispose();

        }

    }

    public class ShowCustomerReport
    {
        public ICollectionView CollectShowCustomerReport { get; private set; }
        public List<ShowCustomerReport_Result> _lstShowCustomerReport;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowCustomerReport(string filter)
        {
            try
            {
                Bank.Database.Connection.Open();
                _lstShowCustomerReport = new List<ShowCustomerReport_Result>();
                foreach (ShowCustomerReport_Result item in Bank.ShowCustomerReport(filter))
                    _lstShowCustomerReport.Add(item);
                CollectShowCustomerReport = CollectionViewSource.GetDefaultView(_lstShowCustomerReport);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
        }
    }
    public class ShowConsumedatariffctiveenergypivot
    {
        public ICollectionView CollectShowConsumedatariffctiveenergypivot { get; private set; }
        public List<ShowConsumedatariffctiveenergypivot_Result> _lstShowConsumedatariffctiveenergypivot;

        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowConsumedatariffctiveenergypivot(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowConsumedatariffctiveenergypivot = new List<ShowConsumedatariffctiveenergypivot_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowConsumedatariffctiveenergypivot_Result item in Bank.ShowConsumedatariffctiveenergypivot(filter))
                _lstShowConsumedatariffctiveenergypivot.Add(item);
            CollectShowConsumedatariffctiveenergypivot = CollectionViewSource.GetDefaultView(_lstShowConsumedatariffctiveenergypivot);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowConsumedactiveenergypivot
    {
        public ICollectionView CollectShowConsumedactiveenergypivot { get; private set; }
        public List<ShowConsumedactiveenergypivot_Result> _lstShowConsumedactiveenergypivot;
        public Lstconsumedpower Lstpower;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowConsumedactiveenergypivot(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowConsumedactiveenergypivot = new List<ShowConsumedactiveenergypivot_Result>();
            Lstpower = new Lstconsumedpower();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowConsumedactiveenergypivot_Result item in Bank.ShowConsumedactiveenergypivot(filter, CommonData.UserID))
            {
                Consumedpower Cpower = new Consumedpower();
                Cpower.ACtiveEnergy = item.ActiveEnergy;
                Cpower.ReACtiveEnergy = item.ReActiveEnergy;
                Cpower.CReactiveEnergy = item.CREActiveEnergy;
                Cpower.NumberofNominalDemandViolation = item.NumberofNominalDemandViolation;
                Cpower.MaxDemand = item.MaxDemand;
                Cpower.RealDemand = item.RealDemand;
                Cpower.ConsumedDate = item.ConsumedDate;
                Cpower.MeterNumber = item.MeterNumber;
                string Filter = filter + " and Main.ConsumedDate='" + item.ConsumedDate + "'";
                ShowConsumedatariffctiveenergypivot tariff = new ShowConsumedatariffctiveenergypivot(Filter);
                Cpower.lstTariff = tariff._lstShowConsumedatariffctiveenergypivot;
                Lstpower.ListOfTariff.Add(Cpower);

                //_lstShowConsumedatariffctiveenergypivot = tariff._lstShowConsumedatariffctiveenergypivot;


            }
            CollectShowConsumedactiveenergypivot = CollectionViewSource.GetDefaultView(_lstShowConsumedactiveenergypivot);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowConsumedactiveenergypivotManage
    {
        public ICollectionView CollectShowConsumedactiveenergypivotManage { get; private set; }
        public List<ShowConsumedactiveenergypivot_Result> _lstShowConsumedactiveenergypivotManage;
        public Lstconsumedpower Lstpower;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowConsumedactiveenergypivotManage(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowConsumedactiveenergypivotManage = new List<ShowConsumedactiveenergypivot_Result>();
            Lstpower = new Lstconsumedpower();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowConsumedactiveenergypivot_Result item in Bank.ShowConsumedactiveenergypivot(filter, CommonData.UserID))
            {
                Consumedpower cpower = new Consumedpower();
                cpower.ACtiveEnergy = item.ActiveEnergy;
                cpower.ReACtiveEnergy = item.ReActiveEnergy;
                cpower.CReactiveEnergy = item.CREActiveEnergy;
                cpower.NumberofNominalDemandViolation = item.NumberofNominalDemandViolation;
                cpower.MaxDemand = item.MaxDemand;
                cpower.RealDemand = item.RealDemand;
                cpower.ConsumedDate = item.ConsumedDate;
                cpower.MeterNumber = item.MeterNumber;
                string Filter = " and Main.MeterID=" + item.MeterID + " and Main.ConsumedDate='" + item.ConsumedDate + "'";
                ShowConsumedatariffctiveenergypivot tariff = new ShowConsumedatariffctiveenergypivot(Filter);
                cpower.lstTariff = tariff._lstShowConsumedatariffctiveenergypivot;
                Lstpower.ListOfTariff.Add(cpower);

                //_lstShowConsumedatariffctiveenergypivot = tariff._lstShowConsumedatariffctiveenergypivot;


            }
            CollectShowConsumedactiveenergypivotManage = CollectionViewSource.GetDefaultView(_lstShowConsumedactiveenergypivotManage);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class Lstconsumedpower
    {
        public List<Consumedpower> ListOfTariff = new List<Consumedpower>();
    }
    public class ListOfTariff : List<Consumedpower>
    {
    }
    public class Consumedpower
    {
        public string ACtiveEnergy { get; set; }
        public string ReACtiveEnergy { get; set; }
        public string CReactiveEnergy { get; set; }
        public string NumberofNominalDemandViolation { get; set; }
        public string MaxDemand { get; set; }
        public string RealDemand { get; set; }
        public string ConsumedDate { get; set; }
        public string MeterNumber { get; set; }
        public List<ShowConsumedatariffctiveenergypivot_Result> lstTariff { get; set; }

    }

    public class ShowObisTypeToReport
    {
        public ICollectionView CollectShowObisTypeToReport { get; private set; }
        public List<ShowOBISTypeToReport_Result> _lstShowOBISTypeToReport;

        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowObisTypeToReport(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowOBISTypeToReport = new List<ShowOBISTypeToReport_Result>();
            foreach (ShowOBISTypeToReport_Result item in Bank.ShowOBISTypeToReport(filter, CommonData.LanguagesID))
                _lstShowOBISTypeToReport.Add(item);
            CollectShowObisTypeToReport = CollectionViewSource.GetDefaultView(_lstShowOBISTypeToReport);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    //public class ShowMeterToCustomer
    //{
    //    public ICollectionView CollectShowMeterToCustomer { get; private set; }
    //    public List<ShowMeterToCustomer_Result> _lstShowMeterToCustomer;
    //    public SabaNewEntities Bank = new SabaNewEntities();
    //    public ShowMeterToCustomer(string filter)
    //    {
    //        try
    //        {
    //            Bank.Database.Connection.Open();
    //            _lstShowMeterToCustomer = new List<ShowMeterToCustomer_Result>();
    //            foreach (ShowMeterToCustomer_Result item in Bank.ShowMeterToCustomer(filter))
    //                _lstShowMeterToCustomer.Add(item);
    //            CollectShowMeterToCustomer = CollectionViewSource.GetDefaultView(_lstShowMeterToCustomer);
    //            Bank.Database.Connection.Close();
    //            Bank.Dispose();

    //        }
    //        catch (Exception ex)
    //        {
    //            Bank.Database.Connection.Close();
    //            Bank.Dispose();
    //            return;
    //        }
    //    }
    //}

    public class ShowToken
    {
        public ICollectionView CollectShowToken { get; private set; }
        public List<ShowToken_Result> _lstShowToken;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowToken(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowToken = new List<ShowToken_Result>();
            foreach (ShowToken_Result item in Bank.ShowToken(filter, CommonData.LanguagesID))
                _lstShowToken.Add(item);
            CollectShowToken = CollectionViewSource.GetDefaultView(_lstShowToken);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
   
    public class ShowAllCreditTokenWithNecessary
    {
        public ICollectionView CollectShowAllCreditTokenWithNesensery { get; private set; }
        public List<ShowAllCreditTokenWithNecessary_Result> _lstShowAllCreditTokenWithNecessary;
        public SabaNewEntities Bank = new SabaNewEntities();

        public ShowAllCreditTokenWithNecessary(string startDate, string endDate, decimal userID, decimal groupID, decimal groupType, bool isNecessary,bool withCredit,bool type)
        {
            try
            {

           
            Bank.Database.Connection.Open();
            _lstShowAllCreditTokenWithNecessary = new List<ShowAllCreditTokenWithNecessary_Result>();
            foreach (var item in Bank.ShowAllCreditTokenWithNecessary(startDate, endDate,  userID, groupID, groupType,isNecessary,withCredit ,type,2))
                _lstShowAllCreditTokenWithNecessary.Add(item);
            CollectShowAllCreditTokenWithNesensery = CollectionViewSource.GetDefaultView(_lstShowAllCreditTokenWithNecessary);
            Bank.Database.Connection.Close();
            Bank.Dispose();
            }
            catch (Exception ex)
            {
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
        }
    }
    public class ShowCredit303
    {
        public ICollectionView CollectShowCredit303 { get; private set; }
        public List<ShowCredit303_Result> _lstShowCredit303;
        public SabaNewEntities Bank = new SabaNewEntities();

        public ShowCredit303(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowCredit303 = new List<ShowCredit303_Result>();
            foreach (ShowCredit303_Result item in Bank.ShowCredit303(filter))
                _lstShowCredit303.Add(item);
            CollectShowCredit303 = CollectionViewSource.GetDefaultView(_lstShowCredit303);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowCustomers
    {
        public ICollectionView CollectShowCustomers { get; private set; }
        public List<ShowCustomers_Result> _lstShowCustomers;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowCustomers(string filter)
        {
            try
            {
                Bank.Database.Connection.Open();
                _lstShowCustomers = new List<ShowCustomers_Result>();
                var x = Bank.ShowCustomers(filter);
                foreach (ShowCustomers_Result item in x)
                    _lstShowCustomers.Add(item);
                CollectShowCustomers = CollectionViewSource.GetDefaultView(_lstShowCustomers);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                    
            }
            
        }
    }
    public class ShowConsumedWaterPivot
    {
        public ICollectionView CollectShowConsumedWaterPivot { get; private set; }
        public List<ShowConsumedWaterPivot_Result> _lstShowConsumedWaterPivot;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowConsumedWaterPivot(string filter, string managementFilter)
        {
            Bank.Database.Connection.Open();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            _lstShowConsumedWaterPivot = new List<ShowConsumedWaterPivot_Result>();
            try
            {
                foreach (ShowConsumedWaterPivot_Result item in Bank.ShowConsumedWaterPivot(filter, managementFilter, CommonData.UserID, CommonData.LanguagesID))
                    _lstShowConsumedWaterPivot.Add(item);
                
            }
            catch(Exception ex)
            {
                CollectShowConsumedWaterPivot = CollectionViewSource.GetDefaultView(_lstShowConsumedWaterPivot);
                Bank.Database.Connection.Close();
                Bank.Dispose();
                return;
            }

            CollectShowConsumedWaterPivot = CollectionViewSource.GetDefaultView(_lstShowConsumedWaterPivot);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowManagmentConsumedWaterPivot
    {
        public ICollectionView CollectShowManagmentConsumedWaterPivot { get; private set; }
        public List<ShowManagmentConsumedWaterPivot_Result> _lstShowManagmentConsumedWaterPivot;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowManagmentConsumedWaterPivot(string filter, string managementFilter)
        {
            Bank.Database.Connection.Open();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 2400;
            _lstShowManagmentConsumedWaterPivot = new List<ShowManagmentConsumedWaterPivot_Result>();
            foreach (ShowManagmentConsumedWaterPivot_Result item in Bank.ShowManagmentConsumedWaterPivot(filter, managementFilter, CommonData.UserID, CommonData.LanguagesID))
                _lstShowManagmentConsumedWaterPivot.Add(item);
            CollectShowManagmentConsumedWaterPivot = CollectionViewSource.GetDefaultView(_lstShowManagmentConsumedWaterPivot);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
   

    public class ShowManagmentConsumedactiveenergypivot
    {
        public ICollectionView CollectShowManagmentConsumedactiveenergypivot { get; private set; }
        public List<ShowManagmentConsumedactiveenergypivot_Result> _lstShowManagmentConsumedactiveenergypivot;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowManagmentConsumedactiveenergypivot(string filter)
        {
            Bank.Database.Connection.Open();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 2400;
            _lstShowManagmentConsumedactiveenergypivot = new List<ShowManagmentConsumedactiveenergypivot_Result>();
            foreach (ShowManagmentConsumedactiveenergypivot_Result item in Bank.ShowManagmentConsumedactiveenergypivot(filter, CommonData.UserID))
                _lstShowManagmentConsumedactiveenergypivot.Add(item);
            CollectShowManagmentConsumedactiveenergypivot = CollectionViewSource.GetDefaultView(_lstShowManagmentConsumedactiveenergypivot);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowObisValueHeader
    {
        public ICollectionView CollectShowObisValueHeader { get; private set; }
        public List<ShowOBISValueHeader_Result> _lstShowOBISValueHeader = new List<ShowOBISValueHeader_Result>();
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowObisValueHeader(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowOBISValueHeader = new List<ShowOBISValueHeader_Result>();
            foreach (ShowOBISValueHeader_Result item in Bank.ShowOBISValueHeader(filter, CommonData.UserID))
                _lstShowOBISValueHeader.Add(item);

            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowConsumedWaterForVee
    {
        public ICollectionView CollectShowConsumedWaterForVee { get; private set; }
        public List<ShowConsumedWaterForVee_Result> _lstShowConsumedWaterForVee;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowConsumedWaterForVee(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowConsumedWaterForVee = new List<ShowConsumedWaterForVee_Result>();
            foreach (ShowConsumedWaterForVee_Result item in Bank.ShowConsumedWaterForVee(filter))
                _lstShowConsumedWaterForVee.Add(item);
            CollectShowConsumedWaterForVee = CollectionViewSource.GetDefaultView(_lstShowConsumedWaterForVee);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }


    public class ShowConsumedWaterForVee303
    {
        public ICollectionView CollectShowConsumedWaterForVee303 { get; private set; }
        public List<ShowConsumedWaterForVee303_Result> _lstShowConsumedWaterForVee303;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowConsumedWaterForVee303(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowConsumedWaterForVee303 = new List<ShowConsumedWaterForVee303_Result>();
            foreach (ShowConsumedWaterForVee303_Result item in Bank.ShowConsumedWaterForVee303(filter))
                _lstShowConsumedWaterForVee303.Add(item);
            CollectShowConsumedWaterForVee303 = CollectionViewSource.GetDefaultView(_lstShowConsumedWaterForVee303);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowVeeConsumedWaterForVee
    {
        public ICollectionView CollectShowVeeConsumedWaterForVee { get; private set; }
        public List<ShowVEEConsumedWaterForVEE_Result> _lstShowVEEConsumedWaterForVEE;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowVeeConsumedWaterForVee(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowVEEConsumedWaterForVEE = new List<ShowVEEConsumedWaterForVEE_Result>();
            foreach (ShowVEEConsumedWaterForVEE_Result item in Bank.ShowVEEConsumedWaterForVEE(filter))
                _lstShowVEEConsumedWaterForVEE.Add(item);
            CollectShowVeeConsumedWaterForVee = CollectionViewSource.GetDefaultView(_lstShowVEEConsumedWaterForVEE);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowVeeConsumedWater 
    {
        public ICollectionView CollectShowVeeConsumedWaterForVee { get; private set; }
        public List<ShowVeeConsumedWater_Result> _lstShowVeeConsumedWater;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowVeeConsumedWater(decimal? meterId)
        {
            Bank.Database.Connection.Open();
            _lstShowVeeConsumedWater = new List<ShowVeeConsumedWater_Result>();
            foreach (ShowVeeConsumedWater_Result item in Bank.ShowVeeConsumedWater(meterId))
                _lstShowVeeConsumedWater.Add(item);
            CollectShowVeeConsumedWaterForVee = CollectionViewSource.GetDefaultView(_lstShowVeeConsumedWater);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowModem
    {
        public ICollectionView CollectShowModem { get; private set; }
        public List<ShowModem_Result> _lstShowModem;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowModem(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowModem = new List<ShowModem_Result>();
            foreach (ShowModem_Result item in Bank.ShowModem(filter))
                _lstShowModem.Add(item);
            CollectShowModem = CollectionViewSource.GetDefaultView(_lstShowModem);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowObisUnits
    {
        public ICollectionView CollectShowObisUnits { get; private set; }
        public List<ShowOBISUnits_Result> _lstShowOBISUnits;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowObisUnits(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowOBISUnits = new List<ShowOBISUnits_Result>();
            foreach (ShowOBISUnits_Result item in Bank.ShowOBISUnits(filter))
                _lstShowOBISUnits.Add(item);
            CollectShowObisUnits = CollectionViewSource.GetDefaultView(_lstShowOBISUnits);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowMeterForVee
    {
        public ICollectionView CollectShowMeterForVee { get; private set; }
        public List<ShowMeterForVEE_Result> _lstShowMeterForVEE;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowMeterForVee(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowMeterForVEE = new List<ShowMeterForVEE_Result>();
            foreach (ShowMeterForVEE_Result item in Bank.ShowMeterForVEE(filter))
                _lstShowMeterForVEE.Add(item);
            CollectShowMeterForVee = CollectionViewSource.GetDefaultView(_lstShowMeterForVEE);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowOffice
    {
        public ICollectionView CollectShowOffice { get; private set; }
        public List<ShowOffice_Result> _lstShowOffice;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowOffice(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowOffice = new List<ShowOffice_Result>();
            foreach (ShowOffice_Result item in Bank.ShowOffice(filter))
                _lstShowOffice.Add(item);
            CollectShowOffice = CollectionViewSource.GetDefaultView(_lstShowOffice);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowMeterToGroup
    {
        public ICollectionView CollectShowMeterToGroup { get; set; }
        public List<ShowMeterToGroup_Result> _lstShowMeterToGroup { get; set; }
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowMeterToGroup(string filter, decimal meterId)
        {
            Bank.Database.Connection.Open();
            _lstShowMeterToGroup = new List<ShowMeterToGroup_Result>();
            foreach (ShowMeterToGroup_Result item in Bank.ShowMeterToGroup(filter, meterId, CommonData.UserID))
                _lstShowMeterToGroup.Add(item);
            CollectShowMeterToGroup = CollectionViewSource.GetDefaultView(_lstShowMeterToGroup);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
   


    public class ShowUserToGroup
    {
        public ICollectionView CollectShowUserToGroup { get; set; }
        public List<ShowUserToGroup_Result> _lstShowUserToGroup { get; set; }
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowUserToGroup(string filter, int? userId)
        {
            Bank.Database.Connection.Open();
            _lstShowUserToGroup = new List<ShowUserToGroup_Result>();
            foreach (ShowUserToGroup_Result item in Bank.ShowUserToGroup(filter, userId))
                _lstShowUserToGroup.Add(item);
            CollectShowUserToGroup = CollectionViewSource.GetDefaultView(_lstShowUserToGroup);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowDataBasesInfo
    {
        public ICollectionView CollectShowDataBasesInfo { get; private set; }
        public List<ShowDataBasesInfo_Result> _lstShowDataBasesInfo;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowDataBasesInfo(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowDataBasesInfo = new List<ShowDataBasesInfo_Result>();
            foreach (ShowDataBasesInfo_Result item in Bank.ShowDataBasesInfo(filter))
                _lstShowDataBasesInfo.Add(item);
            CollectShowDataBasesInfo = CollectionViewSource.GetDefaultView(_lstShowDataBasesInfo);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }


    public class ShowGroups
    {
        public ICollectionView CollectShowGroups { get; private set; }
        public List<ShowGroups_Result> _lstShowGroups;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowGroups(string filter, int type, decimal? languageId)
        {
            Bank.Database.Connection.Open();
            _lstShowGroups = new List<ShowGroups_Result>();
            foreach (ShowGroups_Result item in Bank.ShowGroups(filter, type, languageId, CommonData.UserID))
                _lstShowGroups.Add(item);
            CollectShowGroups = CollectionViewSource.GetDefaultView(_lstShowGroups);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowProvinces
    {
        public ICollectionView CollectShowProvinces { get; private set; }
        public List<ShowProvinces_Result> _lstShowProvinces;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowProvinces(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowProvinces = new List<ShowProvinces_Result>();
            foreach (ShowProvinces_Result item in Bank.ShowProvinces(filter))
                _lstShowProvinces.Add(item);
            CollectShowProvinces = CollectionViewSource.GetDefaultView(_lstShowProvinces);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowObisValueDetail
    {
        public ICollectionView CollectShowObisValueDetail { get; private set; }
        public List<ShowOBISValueDetail_Result> _lstShowOBISValueDetail = new List<ShowOBISValueDetail_Result>();
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowObisValueDetail(string filter, decimal? languageId)
        {
            Bank.Database.Connection.Open();
            _lstShowOBISValueDetail = new List<ShowOBISValueDetail_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (ShowOBISValueDetail_Result item in Bank.ShowOBISValueDetail(filter, languageId, CommonData.UserID))
                _lstShowOBISValueDetail.Add(item);
            CollectShowObisValueDetail = CollectionViewSource.GetDefaultView(_lstShowOBISValueDetail);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowReportObisValueDetail
    {
        public ICollectionView CollectShowReportObisValueDetail { get; private set; }
        public List<ShowReportOBISValueDetails_Result> _lstShowReportOBISValueDetail = new List<ShowReportOBISValueDetails_Result>();
        public SabaNewEntities Bank = new SabaNewEntities();

        public ShowReportObisValueDetail(string filter,decimal? language)
        {
            try
            {
                Bank.Database.Connection.Open();
                ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
                _lstShowReportOBISValueDetail = new List<ShowReportOBISValueDetails_Result>();
                foreach (ShowReportOBISValueDetails_Result item in Bank.ShowReportOBISValueDetails(filter,language,CommonData.UserID))                
                    _lstShowReportOBISValueDetail.Add(item);
                CollectShowReportObisValueDetail = CollectionViewSource.GetDefaultView(_lstShowReportOBISValueDetail);
                Bank.Database.Connection.Close();
                Bank.Dispose();

            }
            catch (Exception ex)
            {
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
        }

    }

    public class ShowErrorControlReport
    {
        public ICollectionView CollectShowErrorControlReport { get; private set; }
        public List<showErrorControlReport_Result> _lstShowErrorControlReport = new List<showErrorControlReport_Result>();
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowErrorControlReport(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowErrorControlReport = new List<showErrorControlReport_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (showErrorControlReport_Result item in Bank.showErrorControlReport(filter, CommonData.UserID))
                _lstShowErrorControlReport.Add(item);
            CollectShowErrorControlReport = CollectionViewSource.GetDefaultView(_lstShowErrorControlReport);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowFilterforNominalDemandReport
    {
        public ICollectionView CollectShowFilter { get; private set; }
        public List<showFilterforNominalDemandReport_Result> _lstShowFilter = new List<showFilterforNominalDemandReport_Result>();
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowFilterforNominalDemandReport()
        {
            Bank.Database.Connection.Open();
            _lstShowFilter = new List<showFilterforNominalDemandReport_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 24000;
            foreach (showFilterforNominalDemandReport_Result item in Bank.showFilterforNominalDemandReport(CommonData.LanguagesID))
                _lstShowFilter.Add(item);
            CollectShowFilter = CollectionViewSource.GetDefaultView(_lstShowFilter);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowErrorControlReportNew
    {
        public ICollectionView CollectshowErrorControlReportNew { get; private set; }
        public List<showErrorControlReportNew_Result> _lstshowErrorControlReportNew = new List<showErrorControlReportNew_Result>();
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowErrorControlReportNew(string filter, string filter2)
        {
            Bank.Database.Connection.Open();
            _lstshowErrorControlReportNew = new List<showErrorControlReportNew_Result>();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 2000;
            foreach (showErrorControlReportNew_Result item in Bank.showErrorControlReportNew(filter, filter2, CommonData.UserID))
                _lstshowErrorControlReportNew.Add(item);
            CollectshowErrorControlReportNew = CollectionViewSource.GetDefaultView(_lstshowErrorControlReportNew);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowNominalDemandViolation
    {
        public ICollectionView CollectShowNominalDemandViolation { get; private set; }
        public List<ShowNominalDemandViolation_Result> _lstShowNominalDemandViolation = new List<ShowNominalDemandViolation_Result>();
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowNominalDemandViolation(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowNominalDemandViolation = new List<ShowNominalDemandViolation_Result>();
            foreach (ShowNominalDemandViolation_Result item in Bank.ShowNominalDemandViolation(filter, CommonData.UserID))
                _lstShowNominalDemandViolation.Add(item);
            CollectShowNominalDemandViolation = CollectionViewSource.GetDefaultView(_lstShowNominalDemandViolation);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowNegativeCredit
    {
        public ICollectionView CollectShowNegativeCredit { get; private set; }
        public List<ShowNegativeCredit_Result> _lstShowNegativeCredit = new List<ShowNegativeCredit_Result>();
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowNegativeCredit(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowNegativeCredit = new List<ShowNegativeCredit_Result>();
            foreach (ShowNegativeCredit_Result item in Bank.ShowNegativeCredit(filter, CommonData.UserID))
                _lstShowNegativeCredit.Add(item);
            CollectShowNegativeCredit = CollectionViewSource.GetDefaultView(_lstShowNegativeCredit);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowCountries
    {
        public ICollectionView CollectShowCountries { get; private set; }
        public List<ShowCountries_Result> _lstShowCountries;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowCountries(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowCountries = new List<ShowCountries_Result>();
            foreach (ShowCountries_Result item in Bank.ShowCountries(filter))
                _lstShowCountries.Add(item);
            CollectShowCountries = CollectionViewSource.GetDefaultView(_lstShowCountries);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowPlains
    {
        public ICollectionView CollectShowPlains { get; private set; }
        public List<ShowPlains_Result> _lstShowPlains;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowPlains(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowPlains = new List<ShowPlains_Result>();
            foreach (ShowPlains_Result item in Bank.ShowPlains(filter))
                _lstShowPlains.Add(item);
            CollectShowPlains = CollectionViewSource.GetDefaultView(_lstShowPlains);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowAreas
    {
        public ICollectionView CollectShowAreas { get; private set; }
        public List<ShowAreas_Result> _lstShowAreas;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowAreas(string filter)
        {
            Bank.Database.Connection.Open();
            List<ShowAreas_Result> _lstShowAreas = new List<ShowAreas_Result>();
            foreach (ShowAreas_Result item in Bank.ShowAreas(filter))
                _lstShowAreas.Add(item);
            CollectShowAreas = CollectionViewSource.GetDefaultView(_lstShowAreas);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowCatchments
    {
        public ICollectionView CollectShowCatchments { get; private set; }
        public List<ShowCatchments_Result> _lstShowCatchments;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowCatchments(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowCatchments = new List<ShowCatchments_Result>();
            foreach (ShowCatchments_Result item in Bank.ShowCatchments(filter))
                _lstShowCatchments.Add(item);
            CollectShowCatchments = CollectionViewSource.GetDefaultView(_lstShowCatchments);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowCities
    {
        public ICollectionView CollectShowCities { get; private set; }
        public List<ShowCities_Result> _lstShowCities;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowCities(string filter)
        {
            Bank.Database.Connection.Open();
            _lstShowCities = new List<ShowCities_Result>();
            foreach (ShowCities_Result item in Bank.ShowCities(filter))
                _lstShowCities.Add(item);
            CollectShowCities = CollectionViewSource.GetDefaultView(_lstShowCities);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowWindows
    {
        public ICollectionView CollectShowWindows { get; private set; }
        public List<ShowWindows_Result> _lstShowWindows;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowWindows(string filter)
        {
            Bank.Database.Connection.Open();
            List<ShowWindows_Result> _lstShowWindows = new List<ShowWindows_Result>();
            foreach (ShowWindows_Result item in Bank.ShowWindows(filter, CommonData.LanguagesID))
                _lstShowWindows.Add(item);
            CollectShowWindows = CollectionViewSource.GetDefaultView(_lstShowWindows);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class Showusers
    {
        public ICollectionView CollectShowUsers { get; private set; }
        public List<ShowUsers_Result> _lstShowUsers;
        public SabaNewEntities Bank = new SabaNewEntities();
        public Showusers(string filte)
        {
            Bank.Database.Connection.Open();
            _lstShowUsers = new List<ShowUsers_Result>();
            foreach (ShowUsers_Result item in Bank.ShowUsers(filte))
                _lstShowUsers.Add(item);
            CollectShowUsers = CollectionViewSource.GetDefaultView(_lstShowUsers);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowTranslateofLable
    {
        public ShowTranslateofLable_Result TranslateofLable = null;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowTranslateofLable(string filte)
        {
            Bank.Database.Connection.Open();
            foreach (ShowTranslateofLable_Result item in Bank.ShowTranslateofLable(filte))
            {
                TranslateofLable = new ShowTranslateofLable_Result();
                TranslateofLable = item;
            }
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ShowTranslateofMessage
    {
        public ShowTranslateofMessage_Result TranslateofMessage = null;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowTranslateofMessage(string filte)
        {
            Bank.Database.Connection.Open();
            foreach (ShowTranslateofMessage_Result item in Bank.ShowTranslateofMessage(filte))
            {
                TranslateofMessage = new ShowTranslateofMessage_Result();
                TranslateofMessage = item;
            }
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }

    public class ShowTranslate
    {
        public ICollectionView CollectShowTranslate { get; private set; }
        //public ShowTranslate_Result Translate = null;
        public List<ShowTranslate_Result> _lstShowTranslate;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowTranslate(string filter)
        { 
            try 
	        {
                Bank.Database.Connection.Open();
                _lstShowTranslate = new List<ShowTranslate_Result>();
                foreach (ShowTranslate_Result item in Bank.ShowTranslate(filter))
                    _lstShowTranslate.Add(item);

                CollectShowTranslate = CollectionViewSource.GetDefaultView(_lstShowTranslate);
                Bank.Database.Connection.Close();
                Bank.Dispose();

            }
	        catch (Exception ex)
	        {
              MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
	        }
      }
    }
    public class ShowLanguage
    {
        public ICollectionView CollectShowLanguage { get; private set; }
        public List<ShowLanguage_Result> _lstShowLanguage;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowLanguage(string filter)
        {
            try
            {
                Bank.Database.Connection.Open();
                List<ShowLanguage_Result> _lstShowLanguage = new List<ShowLanguage_Result>();
                foreach (ShowLanguage_Result item in Bank.ShowLanguage(filter))
                    _lstShowLanguage.Add(item);
                CollectShowLanguage = CollectionViewSource.GetDefaultView(_lstShowLanguage);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
             
            catch (Exception ex)
            {
                if (ex.ToString().ToLower().Contains("sql"))
                    throw ex;
                else
                {
                    MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
                }
		      
	        }
        }
    }
    public class ShowDeviceModel
    {
        public ICollectionView CollectShowDeviceModel { get; private set; }
        public List<ShowDeviceModel_Result> _lstShowDeviceModel;

        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowDeviceModel(string filte)
        {
            Bank.Database.Connection.Open();
            _lstShowDeviceModel = new List<ShowDeviceModel_Result>();
            foreach (ShowDeviceModel_Result item in Bank.ShowDeviceModel(filte))
                _lstShowDeviceModel.Add(item);
            CollectShowDeviceModel = CollectionViewSource.GetDefaultView(_lstShowDeviceModel);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }

    }
    public class ShowDeviceModelandSoftversion
    {
        public ICollectionView CollectShowDeviceModelandSoftversion { get; private set; }
        public List<ShowDeviceModelandSoftversion_Result> _lstShowDeviceModelandSoftversion;

        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowDeviceModelandSoftversion(string filte)
        {
            Bank.Database.Connection.Open();
            _lstShowDeviceModelandSoftversion = new List<ShowDeviceModelandSoftversion_Result>();
            foreach (ShowDeviceModelandSoftversion_Result item in Bank.ShowDeviceModelandSoftversion(filte))
                _lstShowDeviceModelandSoftversion.Add(item);
            CollectShowDeviceModelandSoftversion = CollectionViewSource.GetDefaultView(_lstShowDeviceModelandSoftversion);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }

    }
    public class ShowDeviceType
    {
        public ICollectionView CollectShowDeviceType { get; private set; }
        public List<ShowDeviceType_Result> _lstShowDeviceType;

        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowDeviceType(string filte)
        {
            Bank.Database.Connection.Open();
            List<ShowDeviceType_Result> _lstShowDeviceType = new List<ShowDeviceType_Result>();
            foreach (ShowDeviceType_Result item in Bank.ShowDeviceType(filte))
                _lstShowDeviceType.Add(item);
            CollectShowDeviceType = CollectionViewSource.GetDefaultView(_lstShowDeviceType);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }

    }
    public class UserToGr : INotifyPropertyChanged
    {
        public string _GroupName = "";
        public decimal? _GroupID = 1000000;
        public decimal? _UserID = 1000000;
        public bool _Isvisable = false;
        public int _GroupType = 1;

        public string GroupName
        {
            get { return _GroupName; }
            set
            {
                _GroupName = value;
                NotifyPropertyChanged("GroupName");
            }
        }
        public int GroupType
        {
            get { return _GroupType; }
            set
            {
                _GroupType = value;
                NotifyPropertyChanged("GroupType");
            }
        }
        public decimal? GroupId
        {
            get { return _GroupID; }
            set
            {
                _GroupID = value;
                NotifyPropertyChanged("GroupID");
            }
        }

        public decimal? UserId
        {
            get { return _UserID; }
            set
            {
                _UserID = value;
                NotifyPropertyChanged("UserID");
            }
        }

        public bool Isvisable
        {
            get { return _Isvisable; }
            set
            {
                _Isvisable = value;
                NotifyPropertyChanged("Isvisable");
            }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
    public class MeterToGr : INotifyPropertyChanged
    {
        public string _GroupName = "";
        public string _MeterNumber = "";
        public int _GroupType = 0;
        public decimal? _GroupID = 1000000;
        public decimal? _MeterID = 1000000;
        public bool _Isvisable = false;

        public int GroupType
        {
            get { return _GroupType; }
            set
            {
                _GroupType = value;
                NotifyPropertyChanged("GroupType");
            }
        }
        public string GroupName
        {
            get { return _GroupName; }
            set
            {
                _GroupName = value;
                NotifyPropertyChanged("GroupName");
            }
        }
        public string MeterNumber
        {
            get { return _MeterNumber; }
            set
            {
                _MeterNumber = value;
                NotifyPropertyChanged("MeterNumber");
            }
        }
        public decimal? GroupId
        {
            get { return _GroupID; }
            set
            {
                _GroupID = value;
                NotifyPropertyChanged("GroupID");
            }
        }

        public decimal? MeterId
        {
            get { return _MeterID; }
            set
            {
                _MeterID = value;
                NotifyPropertyChanged("MeterID");
            }
        }

        public bool Isvisable
        {
            get { return _Isvisable; }
            set
            {
                _Isvisable = value;
                NotifyPropertyChanged("Isvisable");
            }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class SelectedMeter : INotifyPropertyChanged
    {
        public decimal? _MeterID = 1000000;
        public string _MeterNumber = "";
        public string _ReadDate = "";
        public bool _Isvisable = false;
        public string _WatersubscriptionNumber = "";
        public string _ElecsubscriptionNumber = "";
        public string _HasError = "false";
        public string _SourceTypeName = "";
        public decimal? MeterId
        {
            get { return _MeterID; }
            set
            {
                _MeterID = value;
                NotifyPropertyChanged("MeterID");
            }
        }
        public string MeterNumber
        {
            get { return _MeterNumber; }
            set
            {
                _MeterNumber = value;
                NotifyPropertyChanged("MeterNumber");
            }
        }
        public string SourceTypeName
        {
            get { return _SourceTypeName; }
            set
            {
                _SourceTypeName = value;
                NotifyPropertyChanged("SourceTypeName");
            }
        }
        public string ReadDate
        {
            get { return _ReadDate; }
            set
            {
                _ReadDate = value;
                NotifyPropertyChanged("ReadDate");
            }
        }
        public string WatersubscriptionNumber
        {
            get { return _WatersubscriptionNumber; }
            set
            {
                _WatersubscriptionNumber = value;
                NotifyPropertyChanged("WatersubscriptionNumber");
            }
        }
        public string ElecsubscriptionNumber
        {
            get { return _ElecsubscriptionNumber; }
            set
            {
                _ElecsubscriptionNumber = value;
                NotifyPropertyChanged("ElecsubscriptionNumber");
            }
        }
        public bool Isvisable
        {
            get { return _Isvisable; }
            set
            {
                _Isvisable = value;
                NotifyPropertyChanged("Isvisable");
            }
        }
        public string HasError
        {
            get { return _HasError; }
            set
            {
                _HasError = value;
                NotifyPropertyChanged("HasError");
            }
        }

        public decimal CustomerId { get; internal set; }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
    public class ShowSoftversionToDeviceModel
    {
        public ICollectionView CollectShowSoftversionToDeviceModel { get; private set; }
        public List<ShowSoftversionToDeviceModel_Result> _lstShowSoftversionToDeviceModel;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowSoftversionToDeviceModel(string filte)
        {
            Bank.Database.Connection.Open();
            List<ShowSoftversionToDeviceModel_Result> _lstShowSoftversionToDeviceModel = new List<ShowSoftversionToDeviceModel_Result>();
            foreach (ShowSoftversionToDeviceModel_Result item in Bank.ShowSoftversionToDeviceModel(filte))
                _lstShowSoftversionToDeviceModel.Add(item);
            CollectShowSoftversionToDeviceModel = CollectionViewSource.GetDefaultView(_lstShowSoftversionToDeviceModel);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }

    }
    public class ShowLocations
    {
        public ICollectionView CollectShowLocations { get; private set; }
        public List<ShowLocations_Result> _lstShowLocations;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowLocations(string filte)
        {
            Bank.Database.Connection.Open();
            _lstShowLocations = new List<ShowLocations_Result>();
            foreach (ShowLocations_Result item in Bank.ShowLocations(filte))
                _lstShowLocations.Add(item);
            CollectShowLocations = CollectionViewSource.GetDefaultView(_lstShowLocations);            
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }

    }
    public class ShowObisType
    {
        public ICollectionView CollectShowOBISType { get; private set; }
        public List<ShowOBISType_Result> _lstShowOBISType;
        public SabaNewEntities Bank = new SabaNewEntities();
        public ShowObisType(string filte)
        {
            Bank.Database.Connection.Open();
            _lstShowOBISType = new List<ShowOBISType_Result>();
            foreach (ShowOBISType_Result item in Bank.ShowOBISType(filte, CommonData.LanguagesID))
                _lstShowOBISType.Add(item);
            CollectShowOBISType = CollectionViewSource.GetDefaultView(_lstShowOBISType);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }

    }
    public class UserPermissions : INotifyPropertyChanged
    {

        public bool _CanInsert = true;
        public bool _CanUpdate = true;
        public bool _CanDel = true;
        public bool _CanImportFromFile = true;
        public string _ButtonName = "";
        public string _ButtonID = "";
        public string _CanInsertImage = "";
        public string _CanUpdateImage = "";
        public string _CanDelImage = "";
        public string _CanImportFromFilImage = "";
        public string _CanShowImage = "";

        public string CanInsertImage
        {
            get { return _CanInsertImage; }
            set
            {
                _CanInsertImage = value;
                NotifyPropertyChanged("CanInsertImage");
            }
        }
        public string CanShowImage
        {
            get { return _CanShowImage; }
            set
            {
                _CanShowImage = value;
                NotifyPropertyChanged("CanShowImage");
            }
        }
        public string CanUpdateImage
        {
            get { return _CanUpdateImage; }
            set
            {
                _CanUpdateImage = value;
                NotifyPropertyChanged("CanUpdateImage");
            }
        }

        public string CanDelImage
        {
            get { return _CanDelImage; }
            set
            {
                _CanDelImage = value;
                NotifyPropertyChanged("CanDelImage");
            }
        }
        public string CanImportFromFilImage
        {
            get { return _CanImportFromFilImage; }
            set
            {
                _CanImportFromFilImage = value;
                NotifyPropertyChanged("CanImportFromFilImage");
            }
        }

        public bool CanInsert
        {
            get { return _CanInsert; }
            set
            {
                _CanInsert = value;
                NotifyPropertyChanged("CanInsert");
            }
        }
        public bool CanUpdate
        {
            get { return _CanUpdate; }
            set
            {
                _CanUpdate = value;
                NotifyPropertyChanged("CanUpdate");
            }
        }
        public bool CanDel
        {
            get { return _CanDel; }
            set
            {
                _CanDel = value;
                NotifyPropertyChanged("CanDel");
            }
        }
        public bool CanImportFromFile
        {
            get { return _CanImportFromFile; }
            set
            {
                _CanImportFromFile = value;
                NotifyPropertyChanged("CanImportFromFile");
            }
        }
        public string ButtonName
        {
            get { return _ButtonName; }
            set
            {
                _ButtonName = value;
                NotifyPropertyChanged("ButtonName");
            }
        }
        public string ButtonId
        {
            get { return _ButtonID; }
            set
            {
                _ButtonID = value;
                NotifyPropertyChanged("ButtonID");
            }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
    public class ShowButtonAccess
    {
        public ICollectionView ButtonCollection { get; private set; }
        SabaNewEntities Bank = new SabaNewEntities();
        public List<UserPermissions> Buttonlist = new List<UserPermissions>();
        public ShowButtonAccess(string filter, string otherFilter)
        {
            Bank.Database.Connection.Open();
            foreach (ShowButtonAccess_Result item in Bank.ShowButtonAccess(filter, otherFilter, CommonData.LanguagesID))
            {
                UserPermissions us = new UserPermissions();
                us.ButtonName = item.ButtonDesc;
                us.ButtonId = item.ButtonID.ToString();
                if (!item.CanShow)
                    us.CanShowImage = @"..\Image\erase.png";
                else
                    us.CanShowImage = @"..\Image\button_ok.png";
                if (!item.CanInsert)
                    us.CanInsertImage = @"..\Image\erase.png";
                else
                    us.CanInsertImage = @"..\Image\button_ok.png";
                if (!item.CanDelete)
                    us.CanDelImage = @"..\Image\erase.png";
                else
                    us.CanDelImage = @"..\Image\button_ok.png";

                if (!item.CanImportFromFile)
                    us.CanImportFromFilImage = @"..\Image\erase.png";
                else
                    us.CanImportFromFilImage = @"..\Image\button_ok.png";

                Buttonlist.Add(us);
            }
            ButtonCollection = CollectionViewSource.GetDefaultView(Buttonlist);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }    
    public class SelectDetail 
    {
        public string   Code {get;set;}
        public string Desc { get; set; }
        public decimal? ID { get; set; }
        public decimal? ID2 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        
    }
    public class MeetrsStatus
    {
        public List<Statusvalue> MeetrsList = new List<Statusvalue>();

    }
    public class Statusvalue
    {
        _303 Obj303 = new _303();
        public string StatusDesc { get; set; }
        public List<Status_Result> List = new List<Status_Result>();
    }


    //public class SelectedMeterInformation
    //{
     
    //    public List<ShowConsumedWaterPivot_Result> ShowConsumedWater1;
      
    //    public List<ShowOBISValueHeader_Result> ShowObisHeader1;

    //    public List<ShowMeter_Result> ShowMeters1;

    //}

}
