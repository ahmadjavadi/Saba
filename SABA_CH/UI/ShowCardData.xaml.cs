using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MeterStatus;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;
using Button = System.Windows.Controls.Button;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for ShowCardData.xaml
    /// </summary>
    
    public partial class ShowCardData : System.Windows.Window
    {
        public ShowTranslateofLable Tr;
        public readonly int WindowId = 34;
        string WaterConsuptin207 = "";
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        Button _selectedButton;
        public List<ReverseConsumedOBIS> _lstConsumedWater;
        public List<ShowOBISValueDetail_Result> RowFlowConsumeWater=new List<ShowOBISValueDetail_Result>();
       
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
        public ICollectionView GroupedCustomers { get; private set; }
        public ShowOBISValueHeader_Result SelecterReadOut;
        public string MeterNumber = "";
        public ShowCardData()
        {
            InitializeComponent();       
            ChangeLanguage();
            ChangeFlowDirection();
            txtMeterNumber.Text = CommonData.SelectedMeterNumber;            
            RereshGridHeader();
            RefreshDataGrids();
            CommonData.showmeterdata = this;
            //if (CommonData.SelectedMeterNumber.StartsWith("207"))
            // CommonData.mainwindow.Vee207data(CommonData.SelectedMeterID);
            
        }
        
        public void ChangebtnVisble()
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate
                {
                    if (CommonData.SelectedMeterNumber.StartsWith("207"))
                    {
                        btnS_1.Visibility = Visibility.Hidden;
                        btnS_18.Visibility = Visibility.Hidden;
                        btnS_2.Visibility = Visibility.Hidden;
                        btnS_20.Visibility = Visibility.Hidden;
                        btnS_21.Visibility = Visibility.Hidden;
                        //btnS_4.Visibility = Visibility.Hidden;
                        //btnS_5.Visibility = Visibility.Hidden;
                        //btnS_207.Visibility = Visibility.Visible;
                        //btnS_207.Margin = btnS_20.Margin;
                        //changelstStatusposition(true);
                    }
                    else
                    {
                        btnS_1.Visibility = Visibility.Visible;
                        btnS_18.Visibility = Visibility.Visible;
                        btnS_2.Visibility = Visibility.Visible;
                        btnS_20.Visibility = Visibility.Visible;
                        btnS_21.Visibility = Visibility.Visible;
                        //btnS_4.Visibility = Visibility.Visible;
                        //btnS_5.Visibility = Visibility.Visible;
                        //btnS_207.Visibility = Visibility.Hidden;
                        //changelstStatusposition(false);
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void ChangeLanguage()
        {
            try
            {
                Tr = CommonData.translateWindow(WindowId);
                LayoutRoot.DataContext = Tr.TranslateofLable;
                //tabitemCredittocard.Header = tr.TranslateofLable.Object27;
                this.DataContext = Tr.TranslateofLable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);

            }
        }
        public void ChangeFlowDirection()
        {
            try
            {
                //LayoutRoot.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        private void ShowTabItem()
        {
            try
            {
                ShowButtonAccess_Result showgeneral=CommonData.ShowButtonBinding("", 35);
                tabitemGeneral.DataContext = showgeneral;
                if (showgeneral.CanShow != null)
                {
                    if (showgeneral.CanShow)
                        tabitemGeneral.Header = Tr.TranslateofLable.Object1;
                   
                }
                ShowButtonAccess_Result meterStatus = CommonData.ShowButtonBinding("", 36);
                tabitemMeterStatus.DataContext = meterStatus;
                if (meterStatus.CanShow != null)
                {
                    if (meterStatus.CanShow)
                        tabitemMeterStatus.Header = Tr.TranslateofLable.Object28;

                }
                //ShowButtonAccess_Result WaterEvent = CommonData.ShowButtonBinding("", 37);
                //tabitemWaterEvent.DataContext = WaterEvent;
                //if (WaterEvent.CanShow != null)
                //{
                //    if (WaterEvent.CanShow)
                //        tabitemWaterEvent.Header = tr.TranslateofLable.Object29;

                //}
                //ShowButtonAccess_Result ElEvent = CommonData.ShowButtonBinding("", 38);
                //tabitemElEvent.DataContext = ElEvent;
                //if (ElEvent.CanShow != null)
                //{
                //    if (ElEvent.CanShow)
                //        tabitemElEvent.Header = tr.TranslateofLable.Object30;

                //}
                //ShowButtonAccess_Result GeneralEvent = CommonData.ShowButtonBinding("", 39);
                //tabitemGeneralEvent.DataContext = GeneralEvent;
                //if (GeneralEvent.CanShow != null)
                //{
                //    if (GeneralEvent.CanShow)
                //        tabitemGeneralEvent.Header = tr.TranslateofLable.Object31;

                //}
                ShowButtonAccess_Result generalWater = CommonData.ShowButtonBinding("", 30);
                tabitemgeneralWater.DataContext = generalWater;
                if (generalWater.CanShow != null)
                {
                    if (generalWater.CanShow)
                        tabitemgeneralWater.Header = Tr.TranslateofLable.Object32;

                }
                //ShowButtonAccess_Result CurveInfo = CommonData.ShowButtonBinding("", 31);
                //tabitemCurveInfo.DataContext = CurveInfo;
                //if (CurveInfo.CanShow != null)
                //{
                //    if (CurveInfo.CanShow)
                //        tabitemCurveInfo.Header = tr.TranslateofLable.Object33;

                //}
                ShowButtonAccess_Result consumedWater = CommonData.ShowButtonBinding("", 32);
                tabitemConsumedWater.DataContext = consumedWater;
                tabitemConsumedWaterchart.DataContext = consumedWater;
                if (consumedWater.CanShow != null)
                {
                    if (consumedWater.CanShow)
                    {
                        tabitemConsumedWater.Header = Tr.TranslateofLable.Object34;
                        tabitemConsumedWaterchart.Header = Tr.TranslateofLable.Object65;
                    }

                }
                ShowButtonAccess_Result generalInfoElectrical = CommonData.ShowButtonBinding("", 33);
                tabitemGeneralInfoElectrical.DataContext = generalInfoElectrical;
                if (generalInfoElectrical.CanShow != null)
                {
                    if (generalInfoElectrical.CanShow)
                        tabitemGeneralInfoElectrical.Header = Tr.TranslateofLable.Object35;

                }
                ShowButtonAccess_Result powerConsumption = CommonData.ShowButtonBinding("", 40);
                tabitemPowerConsumption.DataContext = powerConsumption;
                tabitemConsumedpowerchart.DataContext = powerConsumption;
                if (powerConsumption.CanShow != null)
                {
                    if (powerConsumption.CanShow)
                    {
                        tabitemPowerConsumption.Header = Tr.TranslateofLable.Object36;
                        tabitemConsumedpowerchart.Header = Tr.TranslateofLable.Object66;
                    }
                }

                //ShowButtonAccess_Result Tariffs = CommonData.ShowButtonBinding("", 41);
                //tabitemTariffs.DataContext = Tariffs;
                //if (Tariffs.CanShow != null)
                //{
                //    if (Tariffs.CanShow)
                //        tabitemTariffs.Header = tr.TranslateofLable.Object37;

                //}
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                ChangebtnVisble();
                
                ShowTabItem();
                
                _tabCtrl.SelectedItem = _tabPag;
                if (!_tabCtrl.IsVisible)
                {

                    _tabCtrl.Visibility = Visibility.Visible;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                ClassControl.OpenWin[WindowId] = false;
                CommonData.showmeterdata = null;
                _tabCtrl.Items.Remove(_tabPag);
                if (!_tabCtrl.HasItems)
                {

                    _tabCtrl.Visibility = Visibility.Hidden;

                }
                CommonData.mainwindow.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        public void RefreshDataGrids()
        {
            try
            {
               // refreshs();
                TranslateHeaderOfGeneralGrid();
                TranslateHeaderOfMeterStatusGrid();
                //TranslateHeaderOfWaterEventGrid();
                //TranslateHeaderOfELEventGrid();
                //TranslateHeaderOfGeneralEventGrid();
                TranslateHeaderOfDatagridgeneralWater();
                //TranslateHeaderOfDatagridCurveInfo();
                //TranslateHeaderOfDatagridConsumedWater();
                TranslateHeaderOfDatagridGeneralInfoElectrical();
                TranslateHeaderOfDatagridPowerConsumption();
                TranslateHeaderOfDatagridConsumedWater();
                //TranslateHeaderOfDatagridTarrif();
                TranslateDatagridHeader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void TranslateHeaderOfDatagridConsumedWater()
        {
            try
            {

                if (CommonData.MeterNumber.StartsWith("207"))
                {
                    DatagridConsumedWater.Columns[0].Header = "(Lit/Sec) دبی حداکثر";
                    //DatagridConsumedWater.ItemsSource
                    //ChangeColumnisibility(DatagridConsumedWater, 0, Visibility.Hidden);
                }
                else
                {
                    DatagridConsumedWater.Columns[0].Header = "(Hour.Min) ساعت کارکرد الکتروپمپ ";
                        //    ChangeColumnisibility(DatagridConsumedWater, 3, Visibility.Visible);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        public void ChangeColumnisibility(DataGrid grid, int columnNumber, Visibility visable)
        {
            try
            {
                grid.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
               delegate {
                   grid.Columns[columnNumber].Visibility = visable;
               }));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void refreshs()
        {
            try
            {
                //if (CommonData.SelectedMeterNumber.StartsWith("207"))
                //    CommonData.mainwindow.Vee207data(CommonData.SelectedMeterID);
                RefreshDatagridGeneral();
                //RefreshDatagridMeterStatus();
                //RefreshDatagridWaterEvent();
                //RefreshDatagridELEvent();
                //RefreshDatagridGeneralEvent();
                RefreshDatagridgeneralWater();
                //RefreshDatagridCurveInfo();
                //RefreshDatagridConsumedWater();
                //RefreshDatagridConsumedWaternew();
                //RefreshDataGridConsumedWater207();
                RefreshDatagridGeneralInfoElectrical();
                RefreshDatagridPowerConsumptionnew();
                RefreshDatagridConsumedWaternew();
                RefreshGridCurve();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void TranslateDatagridHeader()
        {
            try
            {
                DatagridHeader.Columns[0].Header = Tr.TranslateofLable.Object43;
                DatagridHeader.Columns[1].Header = Tr.TranslateofLable.Object12;
                DatagridHeader.Columns[2].Header = Tr.TranslateofLable.Object44;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void TranslateHeaderOfGeneralGrid()
        {
            try
            {
                DatagridGeneral.Columns[0].Header = Tr.TranslateofLable.Object8;
                DatagridGeneral.Columns[1].Header = Tr.TranslateofLable.Object7;
                DatagridGeneral.Columns[2].Header = Tr.TranslateofLable.Object9;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        public void TranslateHeaderOfMeterStatusGrid()
        {
            try
            {
                //DatagridMeterStatus.Columns[0].Header = tr.TranslateofLable.Object8.ToString();
                //DatagridMeterStatus.Columns[1].Header = tr.TranslateofLable.Object7.ToString();
                //DatagridMeterStatus.Columns[2].Header = tr.TranslateofLable.Object9.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        public void RefreshGridCurve()
        {
            try
            {
                DatagridgeneralWater.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
               new Action(
               delegate
               {
                   

                 string filter = " and ReadoutID=" + SelecterReadOut.OBISValueHeaderID.ToString() + " and MeterID = " + CommonData.SelectedMeterID.ToString();
                   ShowCurve oShowCurve=new ShowCurve(filter);
                   gridCurveDotes.DataContext = oShowCurve._lstShowCurve;
                  
                   gridCurve3.DataContext= oShowCurve._lstShowCurve;
                 
               }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }

        }
        public void TranslateHeaderOfDatagridgeneralWater()
        {
            try
            {
                DatagridgeneralWater.Columns[0].Header = Tr.TranslateofLable.Object8;
                DatagridgeneralWater.Columns[1].Header = Tr.TranslateofLable.Object7;
                DatagridgeneralWater.Columns[2].Header = Tr.TranslateofLable.Object9;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        } 
        public void TranslateHeaderOfDatagridGeneralInfoElectrical()
        {
            try
            {
                DatagridGeneralInfoElectrical.Columns[0].Header = Tr.TranslateofLable.Object8;
                DatagridGeneralInfoElectrical.Columns[1].Header = Tr.TranslateofLable.Object7;
                DatagridGeneralInfoElectrical.Columns[2].Header = Tr.TranslateofLable.Object9;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }

        }

        public void TranslateHeaderOfDatagridPowerConsumption()
        {
            try
            {
                DatagridPowerConsumption.Columns[0].Header = Tr.TranslateofLable.Object46;
                DatagridPowerConsumption.Columns[1].Header = Tr.TranslateofLable.Object47;
                DatagridPowerConsumption.Columns[2].Header = Tr.TranslateofLable.Object48;
                DatagridPowerConsumption.Columns[3].Header = Tr.TranslateofLable.Object49;
                DatagridPowerConsumption.Columns[4].Header = Tr.TranslateofLable.Object50;
                DatagridPowerConsumption.Columns[5].Header = Tr.TranslateofLable.Object51;
                DatagridPowerConsumption.Columns[6].Header = Tr.TranslateofLable.Object52;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }


        //public void TranslateHeaderOfDatagridTarrif()
        //{
        //    try
        //    {
        //        DatagridTarrif.Columns[0].Header = tr.TranslateofLable.Object8.ToString();
        //        DatagridTarrif.Columns[1].Header = tr.TranslateofLable.Object7.ToString();
        //        DatagridTarrif.Columns[2].Header = tr.TranslateofLable.Object9.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }

        //}

        public void RefreshDatagridGeneral()
        {
            try
            {
                DatagridGeneral.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate()
                {
                    string Filter = "  and (charindex('\"1\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.OBISValueHeaderID="+SelecterReadOut.OBISValueHeaderID.ToString();
                    ShowObisValueDetail Generalvalue = new ShowObisValueDetail(Filter, CommonData.LanguagesID);
                   
                    foreach (var item in Generalvalue._lstShowOBISValueDetail)
                    {
                        if (item.Obis == "0000010000FF")
                            RowFlowConsumeWater.Add(item);
                    }
                    DatagridGeneral.ItemsSource = Generalvalue.CollectShowObisValueDetail;
                    TranslateHeaderOfGeneralGrid();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public string RefreshDatagridMeterStatus(string OBIS)
        {
            try
            {
                string Value = "";
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                if (SelecterReadOut != null)
                {
                    string Filter = " and Header.OBISValueHeaderID=" + SelecterReadOut.OBISValueHeaderID.ToString() + "  and OBISs.OBIS='" + OBIS + "'";
                    ShowObisValueDetail eGeneralvalue = new ShowObisValueDetail(Filter, CommonData.LanguagesID);
                    foreach (ShowOBISValueDetail_Result item in Bank.ShowOBISValueDetail(Filter, CommonData.LanguagesID, CommonData.UserID))
                        Value = item.Value;
                    Bank.Database.Connection.Close();
                }
                return Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                return "";
            }
        }
        //public void RefreshDatagridWaterEvent()
        //{
        //    try
        //    {
        //        DatagridWaterEvent.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
        //        new Action(
        //        delegate()
        //        {
        //            string Filter = "  and (charindex('\"3\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.MeterID=" + CommonData.SelectedMeterID + " and TransferDate='" + CommonData.SelectedMeterReadDate + "'";
        //            ShowOBISValueDetail WGeneralvalue = new ShowOBISValueDetail(Filter, CommonData.LanguagesID);
        //            DatagridWaterEvent.ItemsSource = null;
        //            DatagridWaterEvent.ItemsSource = WGeneralvalue.CollectShowOBISValueDetail;
        //        }
        //        ));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
        //}
        //public void RefreshDatagridELEvent()
        //{
        //    try
        //    {
        //        DatagridELEvent.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
        //        new Action(
        //        delegate()
        //        {
        //            string Filter = "  and (charindex('\"4\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.MeterID=" + CommonData.SelectedMeterID + " and TransferDate='" + CommonData.SelectedMeterReadDate + "'";
        //            ShowOBISValueDetail value = new ShowOBISValueDetail(Filter, CommonData.LanguagesID);
        //            DatagridELEvent.ItemsSource = null;
        //            DatagridELEvent.ItemsSource = value.CollectShowOBISValueDetail;
        //        }
        //        ));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
        //}
        //public void RefreshDatagridGeneralEvent()
        //{
        //    try
        //    {
        //        DatagridGeneralEvent.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
        //        new Action(
        //        delegate()
        //        {
        //            string Filter = "  and (charindex('\"5\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.MeterID=" + CommonData.SelectedMeterID + " and TransferDate='" + CommonData.SelectedMeterReadDate + "'";
        //            ShowOBISValueDetail value = new ShowOBISValueDetail(Filter, CommonData.LanguagesID);
        //            DatagridGeneralEvent.ItemsSource = null;
        //            DatagridGeneralEvent.ItemsSource = value.CollectShowOBISValueDetail;
        //        }
        //        ));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
        //}
        public void RefreshDatagridgeneralWater()
        {
            try
            {
              
                DatagridgeneralWater.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate
                {
                    string filter = "  and (charindex('\"6\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.OBISValueHeaderID=" + SelecterReadOut.OBISValueHeaderID.ToString();
                    ShowObisValueDetail value = new ShowObisValueDetail(filter, CommonData.LanguagesID);
                    foreach (var item in value._lstShowOBISValueDetail)
                    {
                        if (item.Obis == "0802010000FF")
                            RowFlowConsumeWater.Add(item);
                        if (item.Obis == "0802010100FF")
                            RowFlowConsumeWater.Add(item);
                        if (item.Obis == "0802606202FF")
                            RowFlowConsumeWater.Add(item);
                        if (item.Obis == "0802606200FF")
                            RowFlowConsumeWater.Add(item);
                    }

                    DatagridgeneralWater.ItemsSource = null;
                    DatagridgeneralWater.ItemsSource = value.CollectShowObisValueDetail;

                }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        //public void RefreshDatagridCurveInfo()
        //{
        //    try
        //    {
        //        DatagridCurveInfo.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
        //       new Action(
        //       delegate()
        //       {
        //           string Filter = "  and (charindex('\"7\"',obiss.type)>0   or charindex('\"100\"',obiss.type)>0) and  Header.MeterID=" + CommonData.SelectedMeterID + " and TransferDate='" + CommonData.SelectedMeterReadDate + "'";
        //           ShowOBISValueDetail value = new ShowOBISValueDetail(Filter, CommonData.LanguagesID);
        //           DatagridCurveInfo.ItemsSource = null;
        //           DatagridCurveInfo.ItemsSource = value.CollectShowOBISValueDetail;
        //       }));

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
        //}
        #region newrefreshforconsumedwater
        public void RefreshDatagridConsumedWater()
        {
            try
            {
               
                string filter = " and Main.OBISHeaderID=" + SelecterReadOut.OBISValueHeaderID.ToString();
                ShowConsumedWater value = new ShowConsumedWater(filter);
                DatagridConsumedWater.ItemsSource = null;
                
                DatagridConsumedWater.ItemsSource = value._lstShowConsumedWaters;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshDatagridConsumedWaternew()
        {
            try
            {
                DatagridgeneralWater.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
               new Action(
               delegate ()
               {
                   ShowVeeConsumedWater value1 = new ShowVeeConsumedWater(CommonData.MeterID);
                   
                   if (CommonData.MeterNumber.StartsWith("207"))
                   {
                       foreach (var item in value1._lstShowVeeConsumedWater)
                       {
                           item.PumpWorkingHour = item.Flow;
                       }
                   }
                   DatagridConsumedWater.ItemsSource = value1._lstShowVeeConsumedWater;

                   return;
               }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
        }

        private int ComparerDate(string consumedDate1, string consumedDate2)
        {
          var  dateCompare1 = consumedDate1.Split(' ');
            var  dateCompare2 = consumedDate2.Split(' ');
            if (dateCompare1[0] == dateCompare2[0])
            {
                if (dateCompare1.Length == dateCompare2.Length)
                {
                    int r = string.Compare(dateCompare1[1], dateCompare2[1]) ;
                    if (r==0)
                    {
                        return 2;
                    }
                    else if (r==1)
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (dateCompare1.Length > dateCompare2.Length)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }
         
        public DataTable GetDataTableForReport(DataGrid dtg, string filter)
        {
            DataTable dt = new DataTable();
            try
            {
                ShowConsumedWater_Result row;
                ShowConsumedWater_Result re = new ShowConsumedWater_Result();
                Type t = re.GetType();
                MemberInfo[] members = t.GetMembers(BindingFlags.NonPublic |
                BindingFlags.Instance);
                foreach (MemberInfo member in members)
                {
                    DataColumn dc = new DataColumn();
                    int start = member.Name.IndexOf('<');
                    int end = member.Name.IndexOf('>');
                    int len = member.Name.Length;
                    if (start >= 0)
                    {
                        dc.ColumnName = (member.Name.Substring(start + 1, end - start - 1));
                        dt.Columns.Add(dc);
                    }

                }
                //  add each of the data rows to the table
                SabaNewEntities Bank = new SabaNewEntities();

                Bank.Database.Connection.Open();
                foreach (var item in Bank.ShowConsumedWater(filter, CommonData.LanguagesID, CommonData.UserID))
                {
                    DataRow dr;
                    dr = dt.NewRow();
                    row = item;
                    dr["ConsumedWater"] = row.ConsumedWater;
                    dr["OBISDesc"] = row.OBISDesc;
                    dr["ConsumedDate"] = row.ConsumedDate;
                    dr["MeterID"] = row.MeterID.ToString();
                    dr["MeterNumber"] = row.MeterNumber;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
            return dt;
        }
        public void ExecuteReport(DataTable dt, string meterNumber, List<ReverseConsumedOBIS> revercecomumedobis)
        {
            try
            {
                List<ReverseConsumedOBIS> newlist = new List<ReverseConsumedOBIS>();
                string x = "OBISDesc";// "";
                string y = "ConsumedDate";
                string z = "ConsumedWater";
                DataTable newDt = new DataTable();
                newDt = PivotTable.GetInversedDataTable(dt, x, y, z, "-", false);

                newlist = DataTable2List(newDt, meterNumber);
                foreach (var item in newlist)
                {
                    revercecomumedobis.Add(item);
                }


            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }
        }

        public List<ReverseConsumedOBIS> DataTable2List(DataTable dt, string meterNumber)
        {

            try
            {
                foreach (DataRow item in dt.Rows)
                {
                    ReverseConsumedOBIS newrow = new ReverseConsumedOBIS();
                    newrow.RowName = item[0].ToString();
                    newrow.Column0Value = item[1].ToString();
                    newrow.Column1Value = item[2].ToString();
                    newrow.Column0Name = dt.Columns[1].ColumnName;
                    newrow.Column1Name = dt.Columns[2].ColumnName;
                    newrow.MeterNumber = meterNumber;
                    _lstConsumedWater.Add(newrow);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
            return _lstConsumedWater;
        }
        #endregion newrefreshforconsumedwater
        public void RefreshDatagridGeneralInfoElectrical()
        {
            try
            {
                DatagridGeneralInfoElectrical.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
               new Action(
               delegate
               {
                   string Filter = "  and (charindex('\"9\"',obiss.type)>0    or charindex('\"100\"',obiss.type)>0) and Header.OBISValueHeaderID=" + SelecterReadOut.OBISValueHeaderID.ToString();
                   ShowObisValueDetail value = new ShowObisValueDetail(Filter, CommonData.LanguagesID);
                   DatagridGeneralInfoElectrical.ItemsSource = null;
                   DatagridGeneralInfoElectrical.ItemsSource = value.CollectShowObisValueDetail;
               }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshDatagridPowerConsumption()
        {
            try
            {
                DatagridPowerConsumption.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate
                {
                    string Filter = " and Main.OBISHeaderID=" + SelecterReadOut.OBISValueHeaderID.ToString();
                    ShowConsumedActiveEnergy value = new ShowConsumedActiveEnergy(Filter);
                    DatagridPowerConsumption.ItemsSource = null;
                    DatagridPowerConsumption.ItemsSource = value._lstShowConsumedActiveEnergys;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshDatagridPowerConsumptionnew()
        {
            try
            {
                DatagridPowerConsumption.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate
                {
                    string Filter = " and Main.MeterID=" + CommonData.SelectedMeterID.ToString() ;
                    ShowConsumedactiveenergypivot value = new ShowConsumedactiveenergypivot(Filter);
                    DatagridPowerConsumption.ItemsSource = null;
                    DatagridPowerConsumption.ItemsSource = value.Lstpower.ListOfTariff;
                    try
                    {
                        chartpower.DataSource = value.Lstpower.ListOfTariff;
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        
        public void RefreshDatagridPowerConsumption1()
        {
            try
            {
                DatagridPowerConsumption.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate
                {
                    string filter = " and Main.OBISHeaderID=" + SelecterReadOut.OBISValueHeaderID.ToString();
                    ShowConsumedActiveEnergy value = new ShowConsumedActiveEnergy(filter);
                    DatagridPowerConsumption.ItemsSource = null;
                    DatagridPowerConsumption.ItemsSource = value._lstShowConsumedActiveEnergys;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
       
        public void RereshGridHeader()
        {
            try
            {
                string filter = " and Main.MeterID="+CommonData.SelectedMeterID;
                ShowObisValueHeader obisValueHeader = new ShowObisValueHeader(filter);
                DatagridHeader.ItemsSource = null;
                DatagridHeader.ItemsSource = obisValueHeader._lstShowOBISValueHeader;                
                if (DatagridHeader.Items != null)
                    DatagridHeader.SelectedIndex = DatagridHeader.Items.Count-1;

                if (CommonData.SelectedMeterNumber.StartsWith("207"))
                {
                    Meter207Status();
                    changelstStatusposition(true);
                    ChangebtnVisble();

                }
                else
                {
                    changelstStatusposition(false);
                    ChangebtnVisble();
                }

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
       

        internal void ChangeVisilibityOfSendInfo2Card(Visibility visibility, string date)
        {
            //tabitemCreditsassigned.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
            //    new Action(
            //        delegate()
            //        {
            //            try
            //            {
            //                //SendInfo2Card.Visibility = visibility;
            //txtWaterCredit.Text = "";
            //chk1.IsChecked = false;
            //chk2.IsChecked = false;
            //chk3.IsChecked = false;
            //chk4.IsChecked = false;
            //chk5.IsChecked = false;
            //datePickerend.SelectedDate = PersianDate.Now;
            //datePickerStart.SelectedDate = PersianDate.Now;

            //string s = DateTime.Now.ToString("HH:mm:ss");
            //TimePickerend.txtHours.Text = s.Substring(0, s.IndexOf(":"));
            //TimePickerend.txtMinutes.Text = PersianDate.Now.Minute.ToString();
            //TimePickerend.txtSecound.Text = PersianDate.Now.Second.ToString();

            //startTimePicker.txtHours.Text = s.Substring(0, s.IndexOf(":"));
            //startTimePicker.txtMinutes.Text = PersianDate.Now.Minute.ToString();
            //startTimePicker.txtSecound.Text = PersianDate.Now.Second.ToString();

            //lblw_getDate.Content = date;
            //lble_getDate.Content = date;
            //lbl_getDate.Content = date;
            //    }
            //    catch { }
            //}));
        }
        public void ChangeTabVisibility(TabItem tb, Visibility visible)
        {
            tabControlMain.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                delegate
                {
                    tb.Visibility = visible;
                    tabitemGeneral.Focus();
                }));
        }

        private void txtWaterCredit_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void datePickerend_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void datePickerStart_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void btnSendCredit2Card_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataGrids();
        }

        private void DatagridHeader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelecterReadOut = new ShowOBISValueHeader_Result();
                if (DatagridHeader.SelectedItem != null)
                {
                    DatagridHeader.IsEnabled = false;
                    SelecterReadOut = (ShowOBISValueHeader_Result)DatagridHeader.SelectedItem;

                    CommonData.SelectedMeterReadDate = SelecterReadOut.ReadDate;
                    
                    if (SelecterReadOut.ReadDate != txtReadDate.Text || SelecterReadOut.TransferDate!= CommonData.SelectedMeterTransferDate)
                    {
                        txtReadDate.Text = CommonData.SelectedMeterReadDate;
                        CommonData.SelectedMeterTransferDate = SelecterReadOut.TransferDate;
                        refreshs();
                    }
                    if (CommonData.SelectedMeterNumber.StartsWith("207"))
                    {
                        Meter207Status();
                        changelstStatusposition(true);
                    }
                    else
                        changelstStatusposition(false);
                    DatagridHeader.IsEnabled = true;

                }
                else
                {
                    txtReadDate.Text = "";
                    DatagridGeneral.ItemsSource = null;
                    DatagridgeneralWater.ItemsSource = null;
                    DatagridGeneralInfoElectrical.ItemsSource = null;
                    DatagridPowerConsumption.ItemsSource = null;
                    DatagridConsumedWater.ItemsSource = null;
                }

            }
            catch (Exception ex)
            {
                DatagridHeader.IsEnabled = true;
                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
            
        }

        private void btnS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _303 obj303 = new _303();
                _207 obj207 = new _207();
                List<Status_Result> List = new List<Status_Result>();

                if (_selectedButton != null)
                    _selectedButton.Background = Brushes.SkyBlue;
                string value = "";
                Button btn = sender as Button;
                _selectedButton = btn;
                btn.Background = Brushes.LimeGreen;
                string btnName = btn.Name.Replace("btnS_", "");
                switch (btnName)
                {
                    case "1":
                        try
                        {
                            string value1 = RefreshDatagridMeterStatus("0000603F00FF");
                            string value2 = RefreshDatagridMeterStatus("0000603F01FF");
                            string value3 = RefreshDatagridMeterStatus("0000603F02FF");
                            List = obj303.PerformanceMeteroncreditevents(value1.PadLeft(8, '0'), value2.PadLeft(8, '0'), value3.PadLeft(8, '0'), CommonData.LanguageName);
                            //Value = RefreshDatagrid_303("0000600A02FF");
                            //List = Obj303.StatuseGeneralRegister2(Value.PadLeft(8, '0'), CommonData.LanguageName, 2);

                        }
                        catch { }
                        break;

                    case "2":
                        try
                        {
                            //Value = RefreshDatagrid_303("0000600302FF");
                            //List = Obj303.statusRegister("0000600302FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            string Value = RefreshDatagridMeterStatus("0000600A03FF");
                            if (Value != "")
                                List = obj303.statusRegister_0000600A03FF(Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Value = RefreshDatagridMeterStatus("0000600302FF");
                                if (Value != "")
                                    List = obj303.statusRegister_0000600302FF(Value.PadLeft(8, '0'), CommonData.LanguageName);
                                else
                                {
                                    Status s = new Status();

                                    Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                    List.Add(sr);

                                }
                            }                           
                        }
                        catch
                        {
                            // ignored
                        }
                        break;

                    case "3":
                        try
                        {

                            string Value = RefreshDatagridMeterStatus("0000600A01FF");
                            List = obj303.statusRegister("0000600A01FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "4":
                        try
                        {
                            var Value = RefreshDatagridMeterStatus("0000600A02FF");
                            List = obj303.statusRegister("0000600A02FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch
                        {
                            // ignored
                        }
                        break;

                    case "5":
                        try
                        {
                            var Value = RefreshDatagridMeterStatus("0000600A03FF");
                            List = obj303.statusRegister("0000600A03FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch
                        {
                            // ignored
                        }
                        break;

                    case "6":
                        try
                        {
                            string Value = RefreshDatagridMeterStatus("0000600A04FF");
                            List = obj303.statusRegister("0000600A04FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "7":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600A05FF");
                            List = obj303.statusRegister("0000600A05FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "8":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600A01FF");
                            List = obj303.statusRegister("0000600A01FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "9": try
                        {
                            string Value = RefreshDatagridMeterStatus("0000600A01FF");
                            List = obj303.statusRegister("0000600A01FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "10":
                        try
                        {
                            string Value = RefreshDatagridMeterStatus("0000600A01FF");
                            List = obj303.statusRegister("0000600A01FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "11":
                        try
                        {
                            string Value = RefreshDatagridMeterStatus("0000616100FF");
                            List = obj303.statusRegister("0000616100FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "12": try
                        {
                            string Value = RefreshDatagridMeterStatus("0000616102FF");
                            List = obj303.statusRegister("0000616102FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "13":
                        try
                        {
                            string Value = RefreshDatagridMeterStatus("0100600A03FF");
                            List = obj303.statusRegister("0100600A03FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "14":
                        try
                        {
                            string Value = RefreshDatagridMeterStatus("0100600501FF");
                            List = obj303.statusRegister("0100600501FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "15":
                        try
                        {
                            string Value = RefreshDatagridMeterStatus("0100600A00FF");
                            List = obj303.statusRegister("0100600A00FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "16":
                        try
                        {
                            string Value = RefreshDatagridMeterStatus("0100600A01FF");
                            if (Value!="")
                            List = obj303.statusRegister("0100600A01FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "17":
                        try
                        {
                            string Value = RefreshDatagridMeterStatus("0100616101FF");
                            if (Value!="")
                                List = obj303.statusRegister("0100616101FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                             else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                            
                        }
                        catch { }
                        break;

                    case "18":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0802606101FF");
                            if (Value != "")
                                List = obj303.statusRegister_0802606101FF(Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Value = RefreshDatagridMeterStatus("0000600A02FF");
                                if (Value != "")
                                    List = obj303.GetPumpStatusFrom_0000600A02FF( Value.PadLeft(8, '0'), CommonData.LanguageName);
                                else

                                {
                                    Status s = new Status();

                                    Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                    List.Add(sr);

                                }
                            }
                        }
                        catch { }
                        break;

                    case "19":
                        try
                        {
                            string Value = RefreshDatagridMeterStatus("0000600303FF");
                            if (Value!="")
                                List = obj303.statusRegister("0000600303FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                             else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }

                        catch { }
                        break;

                    case "20":
                        try
                        {
                            string Value = "";
                            //Value = RefreshDatagrid_303("0000600404FF");
                            //List = Obj303.statusRegister("0000600404FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            Value = RefreshDatagridMeterStatus("0000600A02FF");
                            if (Value != "")
                                List = obj303.statusRegister_0000600A02FF(Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Value = RefreshDatagridMeterStatus("0000600404FF");
                                if (Value != "")
                                    List = obj303.statusRegister_0000600404FF(Value.PadLeft(8, '0'), CommonData.LanguageName);
                                else
                                {
                                    Status s = new Status();

                                    Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                    List.Add(sr);
                                }

                            }

                        }
                        catch (Exception ex) { }
                        break;

                    case "21":
                        try
                        {
                            string Value = RefreshDatagridMeterStatus("0000603E04FF");
                            
                            if (Value != "")
                            {
                                if (SelecterReadOut.SourceTypeName.Contains("PC"))
                                {
                                   // Value = Convert.ToInt32(Value, 2).ToString("X");
                                }
                           
                                List = obj303.statusRegister("0000603E04FF", Value, CommonData.LanguageName);
                                foreach (var item in List)
                                {
                                    item.IsStatuseTrue = Status.dontCare;
                                }

                            }
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;
                    case "207":
                        try
                        {
                            string Value1 = RefreshDatagridMeterStatus("0802606101FF");
                            string Value2 = RefreshDatagridMeterStatus("0000603E01FF");
                            Value1 = Value1.Trim();
                            List = obj207.StatusRegister207_0802606101FF(Value2,Value1, CommonData.LanguageName);
                        }
                        catch { }
                        break;
                    default:
                        break;
                }
                //Datagrid_303.ItemsSource = null;

                //Datagrid_303.ItemsSource = List;
                changelist(List);
            }
            catch
            {
            }
        }
        public void changelist(List<Status_Result> List)
        {
            try
            {
                lstStatus.Items.Clear();
                foreach (var item in List )
        	    {
                     if (item.IsStatuseTrue.ToString() =="True" || item.IsStatuseTrue.ToString()=="False")
                    {
                            CheckBox chBox=new CheckBox();
                            //chBox.MouseDown  += new System.Windows.Input.MouseButtonEventHandler(this.cb_MouseDown);
                            chBox.IsHitTestVisible = false;
                            lstStatus.Items.Add(chBox);   
                    
                 
                        chBox.Content  = item.Description;

                        if (item.IsStatuseTrue.ToString() == "True")
                        {
                            chBox.IsChecked = true;
                            chBox.Background = Brushes.GreenYellow;
                            chBox.Foreground = Brushes.Green;
                        }
                        else
                            chBox.IsChecked = false;
                        }
                    else if(item.IsStatuseTrue.ToString()=="dontCare")
                    {
                         Label Lb1=new Label();
                        Lb1.Content =item.Description;
                        lstStatus.Items.Add(Lb1);
                  
                    }
		 
        	    }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        //private void cb_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    e.Handled = true;

        //}
        private void btnStatus_MouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = Brushes.Orange;
            if (_selectedButton != null)
                _selectedButton.Background = Brushes.LimeGreen;
        }

        private void BtnStatus_MouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = Brushes.SkyBlue;
            if (_selectedButton != null)
                _selectedButton.Background = Brushes.LimeGreen;
        }
        private void ToolStripButtonExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                string selectedtab = "";
                if (tabControlMain.SelectedItem == tabitemPowerConsumption)
                    selectedtab = "Power Consumption";
                else if (tabControlMain.SelectedItem == tabitemConsumedWater)
                    selectedtab = "Water Consumption";
                else if (tabControlMain.SelectedItem == tabitemgeneralWater)
                    selectedtab = "General Information water";
                else if (tabControlMain.SelectedItem == tabitemGeneralInfoElectrical)
                    selectedtab = "General information Power";
                else if (tabControlMain.SelectedItem == tabitemGeneral)
                    selectedtab = "Meters General Information";
                dlg.FileName = selectedtab + "(" + txtMeterNumber.Text + ")"; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = " (.xlsx)|*.xlsx"; // Filter files by extension 
                string sourceTypeName = "";
                if (SelecterReadOut!=null)
                {
                    sourceTypeName = SelecterReadOut.SourceTypeName;
                }
                Nullable<bool> result = dlg.ShowDialog();
                string filePath = dlg.FileName;
                if (result.Value)
                {
                    if (tabControlMain.SelectedItem == tabitemPowerConsumption)
                    {
                        if (DatagridPowerConsumption.ItemsSource==null)
                        {
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message80);
                            return;
                        }
                        List<ShowVeeConsumedWater_Result> sublist = new List<ShowVeeConsumedWater_Result>();
                        object[] header = { "ConsumedDate", "ACtiveEnergy", "ReACtiveEnergy", "CReactiveEnergy", "NumberofNominalDemandViolation", "MaxDemand", "RealDemand", "lstTariff" };
                        object[] headerText = { "ConsumedDate", "ACtiveEnergy", "ReACtiveEnergy", "CReactiveEnergy", "NumberofNominalDemandViolation", "MaxDemand", "RealDemand" };
                        object[] subheaderText = { "Tariff1", "Tariff2", "Tariff3", "Tariff4", "Tariff5", "Tariff6", "Tariff7", "Tariff8", "Tariff9", "Tariff10", "Tariff11", "Tariff12", "Tariff13", "Tariff14", "Tariff15", "Tariff16", "Tariff17", "Tariff18", "Tariff19", "Tariff20", "Tariff21", "Tariff22", "Tariff23", "Tariff24", "Tariff25", "Tariff26" };
                        object[] subheader = { "description", "t1", "t2", "t3", "t4", "t5", "t6", "t7", "t8", "t9", "t10", "t11", "t12", "t13", "t14", "t15", "t16", "t17", "t18", "t19", "t20", "t21", "t22", "t23", "t24", "t25", "t26" };
                        ExcelSerializernew<Consumedpower, ShowVeeConsumedWater_Result, List<Consumedpower>> s = new ExcelSerializernew<Consumedpower, ShowVeeConsumedWater_Result, List<Consumedpower>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridPowerConsumption.ItemsSource);

                        
                        s.header = header;
                        s.SubheaderText = subheaderText;
                        s.headerText = headerText;
                        s.Subheader = subheader;
                        s.CreateWorkSheet(filePath, Tr.TranslateofLable.Object36, txtMeterNumber.Text, (List<Consumedpower>)view.SourceCollection, txtReadDate.Text, true, sourceTypeName);
                    }
                    else if (tabControlMain.SelectedItem == tabitemConsumedWater)
                    {
                        if (DatagridConsumedWater.ItemsSource == null)
                        {
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message80);
                            return;
                        }
                         
                        object[] header = { "ConsumedDate", "MonthlyConsumption", "TotalConsumption", "PumpWorkingHour" };
                        object[] headerText = { "ConsumedDate", "Consumed Water", "Total Consumed Water", "Time of pump working" };
                        ExcelSerializernew<ShowVeeConsumedWater_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowVeeConsumedWater_Result>> s =
                            new ExcelSerializernew<ShowVeeConsumedWater_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowVeeConsumedWater_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridConsumedWater.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(filePath, Tr.TranslateofLable.Object34, txtMeterNumber.Text, (List<ShowVeeConsumedWater_Result>)view.SourceCollection, txtReadDate.Text, false, sourceTypeName);
                    }
                    else if (tabControlMain.SelectedItem == tabitemgeneralWater)
                    {
                        if (DatagridgeneralWater.ItemsSource == null)
                        {
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message80);
                            return;
                        }
                        object[] header = { "ObisDesc", "Value", "OBISUnitDesc" };
                        object[] headerText = { "Title", "Value", "Unit" };
                        ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>> s = new ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridgeneralWater.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(filePath, Tr.TranslateofLable.Object3, txtMeterNumber.Text, (List<ShowOBISValueDetail_Result>)view.SourceCollection, txtReadDate.Text, false, sourceTypeName);
                    }
                    else if (tabControlMain.SelectedItem == tabitemGeneralInfoElectrical)
                    {
                        if (DatagridGeneralInfoElectrical.ItemsSource == null)
                        {
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message80);
                            return;
                        }
                        object[] header = { "ObisDesc", "Value", "OBISUnitDesc" };
                        object[] headerText = { "Title", "Value", "Unit" };
                        ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>> s = new ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridGeneralInfoElectrical.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(filePath, Tr.TranslateofLable.Object35, txtMeterNumber.Text, (List<ShowOBISValueDetail_Result>)view.SourceCollection, txtReadDate.Text, false, sourceTypeName);
                    }
                    else if (tabControlMain.SelectedItem == tabitemGeneral)
                    {
                        if (DatagridGeneral.ItemsSource == null)
                        {
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message80);
                            return;
                        }
                        object[] header = { "ObisDesc", "Value", "OBISUnitDesc" };
                        object[] headerText = { "Title", "Value", "Unit" };
                        ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>> s = new ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridGeneral.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(filePath, Tr.TranslateofLable.Object1, txtMeterNumber.Text, (List<ShowOBISValueDetail_Result>)view.SourceCollection, txtReadDate.Text, false, sourceTypeName);
                    }
                    else if (tabControlMain.SelectedItem==tabitemMeterStatus)
                    {
                        //object[] header = { "Description", "IsStatuseTrue" };
                        //object[] headerText = { "Title", "Value", "Unit" };
                        //int usedRows = 14;
                        //MeetrsStatus ms= CreateListOfStatusValue();
                        //ExcelSerializernew<statusvalue, ShowConsumedatariffctiveenergypivot_Result, List<statusvalue>> s = new ExcelSerializernew<statusvalue, ShowConsumedatariffctiveenergypivot_Result, List<statusvalue>>();
                        //ICollectionView view = CollectionViewSource.GetDefaultView(ms.MeetrsList);
                        //s.header = header;
                        //s.headerText = headerText;
                        //foreach (var item in ms.MeetrsList)
                        //{
                        //    s.CreateSplitRows(CommonData.ExPackage.Workbook.Worksheets[j + 2], usedRows[j] + 2, item.StatusDesc);
                        //}
                        //s.CreateWorkSheet(FilePath, "test", txtMeterNumber.Text, (List<statusvalue>)view.SourceCollection, txtReadDate.Text, false);
                        
                        

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        
        public MeetrsStatus CreateListOfStatusValue( )
        {
            MeetrsStatus MS = new MeetrsStatus();
            try
            {
                string Value1 = "";
                string Value2 = "";
                string Value3 = "";
                string[] OBIDS;
               
                _303 Obj303 = new _303();
                List<Status_Result> List = new List<Status_Result>();
                if (!CommonData.MeterNumber.StartsWith("207"))
                {
                    //if (OBISs.Contains("0000603F00FF") || OBISs.Contains("0000603F01FF") || OBISs.Contains("0000603F02FF"))
                    //{
                        try
                        {
                            Obj303 = new _303();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();
                            Value1 = RefreshDatagridMeterStatus("0000603F00FF");
                            Value2 = RefreshDatagridMeterStatus("0000603F01FF");
                            Value3 = RefreshDatagridMeterStatus("0000603F02FF");
                            List = Obj303.PerformanceMeteroncreditevents(Value1.PadLeft(8, '0'), Value2.PadLeft(8, '0'), Value3.PadLeft(8, '0'), CommonData.LanguageName);
                            sv.StatusDesc = Tr.TranslateofLable.Object49;
                            sv.List = List;
                            MS.MeetrsList.Add(sv);
                        }
                        catch { }
                    //}
                    //if (OBISs.Contains("0000600A03FF"))
                    //{
                        try
                        {
                            string Value = "";
                            Obj303 = new _303();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();
                            Value = RefreshDatagridMeterStatus("0000600A03FF");
                            List = Obj303.statusRegister_0000600A03FF(Value.PadLeft(8, '0'), CommonData.LanguageName);
                            sv.StatusDesc = Tr.TranslateofLable.Object48;
                            sv.List = List;
                            MS.MeetrsList.Add(sv);
                        }
                        catch { }
                    //}
                    //if (OBISs.Contains("0000600A02FF"))
                    //{
                        try
                        {
                            Obj303 = new _303();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();
                            string Value = RefreshDatagridMeterStatus("0000600A02FF");
                            List = Obj303.statusRegister_0000600A02FF(Value.PadLeft(8, '0'), CommonData.LanguageName);
                            sv.StatusDesc = Tr.TranslateofLable.Object47;
                            sv.List = List;
                            MS.MeetrsList.Add(sv);
                        }
                        catch
                        {
                        }


                    //}
                    //if (OBISs.Contains("0000600A02FF"))
                    //{
                        try
                        {
                          
                            Obj303 = new _303();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();
                            string Value = RefreshDatagridMeterStatus("0000600A02FF");
                            List = Obj303.StatuseGeneralRegister2(Value.PadLeft(8, '0'), CommonData.LanguageName, 1);
                            sv.StatusDesc = Tr.TranslateofLable.Object46;
                            sv.List = List;
                            MS.MeetrsList.Add(sv);
                        }
                        catch { }
                    //}
                }
                if (CommonData.MeterNumber.StartsWith("207"))

                    //if (OBISs.Contains("0802606101FF"))
                    //{
                        try
                        {
                            _207 Obj207 = new _207();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();

                            string Value11 = RefreshDatagridMeterStatus("0802606101FF");
                            string Value21 = RefreshDatagridMeterStatus("0000603E01FF");
                            Value11 = Value11.Trim();
                            List = Obj207.StatusRegister207_0802606101FF(Value21,Value11, CommonData.LanguageName);
                            sv.StatusDesc = Tr.TranslateofLable.Object50;
                            sv.List = List;
                            MS.MeetrsList.Add(sv);
                        }
                        catch { }
                    //}


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
            return MS;
        }

        public object tabitem_303 { get; set; }

        private void mndatgrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                (sender as DataGrid).Columns[1].Header = Tr.TranslateofLable.Object55;
                (sender as DataGrid).Columns[2].Header = Tr.TranslateofLable.Object56;
                (sender as DataGrid).Columns[3].Header = Tr.TranslateofLable.Object57;
                (sender as DataGrid).Columns[4].Header = Tr.TranslateofLable.Object58;
                (sender as DataGrid).Columns[5].Header = Tr.TranslateofLable.Object59;
                (sender as DataGrid).Columns[6].Header = Tr.TranslateofLable.Object60;
                (sender as DataGrid).Columns[7].Header = Tr.TranslateofLable.Object61;
                (sender as DataGrid).Columns[8].Header = Tr.TranslateofLable.Object62;
                (sender as DataGrid).Columns[9].Header = Tr.TranslateofLable.Object63;
                (sender as DataGrid).Columns[10].Header = Tr.TranslateofLable.Object64;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                //DiagramPower.Width = chartpower.Width;
                //DiagramWater.Width = chart1.Width;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        //private void tabitemMeterStatus_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (CommonData.SelectedMeterNumber.StartsWith("207"))
        //    {
        //        Meter207Status();
        //        changelstStatusposition(true);
        //    }
        //    else
        //        changelstStatusposition(false);

        //}
        private void Meter207Status()
        {
            try
            {
                if (CommonData.SelectedMeterNumber.StartsWith("207"))
                {
                    _207 Obj207 = new _207();
                    List<Status_Result> List = new List<Status_Result>();
                    string Value1 = "", Value2 = "";
                    Value1 = RefreshDatagridMeterStatus("0802606101FF");
                    Value2 = RefreshDatagridMeterStatus("0000603E01FF");
                    Value1 = Value1.Trim();
                    List = Obj207.StatusRegister207_0802606101FF(Value2,Value1, CommonData.LanguageName);
                    //foreach (var item in List)
                    //{
                    //    item.IsStatuseTrue = Status.dontCare;
                    //}
                    changelist(List);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                CommonData.WriteLOG(ex);
            }
        }
        private void changelstStatusposition(bool Is207)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate()
                {
                    if (Is207)
                    {
                        lstStatus.SetValue(Grid.ColumnProperty, 0);
                        lstStatus.SetValue(Grid.ColumnSpanProperty, 2);
                    }
                    else
                    {
                        lstStatus.SetValue(Grid.ColumnProperty, 1);
                        lstStatus.SetValue(Grid.ColumnSpanProperty, 1);
                        lstStatus.Items.Clear();
                    }
                }));
        }

        private void tabitemGeneral_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void tabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tabControlMain.SelectedItem == tabitemMeterStatus)// || tabControlMain.SelectedItem == tabitemConsumedWater207)
                {
                    ToolStripButtonExport.Visibility = Visibility.Collapsed;
                    lblExport.Visibility = Visibility.Collapsed;
                    row2.Height = new GridLength(0);
                    row1.Height = new GridLength(1, GridUnitType.Star);
                    //Gridtoolbar.Width = new GridLength(42);
                    
                }
                else
                {
                    ToolStripButtonExport.Visibility = Visibility.Visible;
                    lblExport.Visibility = Visibility.Visible;
                    row2.Height = new GridLength(15,GridUnitType.Star);
                    row1.Height = new GridLength(25, GridUnitType.Star);

                }

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void DatagridHeader_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcount.Content =rowcount+"="+ DatagridHeader.Items.Count;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
            
        }

        void ctxExportCsv(DataGrid dg)
        {
            //var o = ((DataGridColumnHeader)((ContextMenu)((MenuItem)sender).Parent).DataContext).Column;
            //DataGrid dg = o.GetDataGridParent();
            dg.SelectAllCells();
            dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dg);
            dg.UnselectAllCells();
            String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            try
            {
                StreamWriter sw = new StreamWriter("export.csv");
                sw.WriteLine(result);
                sw.Close();
                Process.Start("export.csv");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ctxExportCsv(DatagridGeneral);
        }

        //private void gridCurveDotes_Loaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ShowCurve_Result v=new ShowCurve_Result();
        //        this.DataContext = v;
        //    }
        //    catch (Exception ex)
        //    {
                    
        //        throw;
        //    }
        //}


        //private void tabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (tabControlMain.SelectedItem == tabitemMeterStatus && CommonData.SelectedMeterNumber.StartsWith("207"))
        //        {
        //            Meter207Status();
        //            changelstStatusposition(true);
        //        }
        //        else if(!CommonData.SelectedMeterNumber.StartsWith("207"))
        //            changelstStatusposition(false);


        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
        //}



    }
}