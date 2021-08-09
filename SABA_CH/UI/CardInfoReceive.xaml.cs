using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading; 
using Card;
using CARD;
using MeterStatus;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;
using SABA_CH.Token;
using RsaDateTime;
using SABA_CH.VEEClasses;

namespace SABA_CH
{
	/// <summary>
	/// Interaction logic for CardInfoReceive.xaml
	/// </summary>
	public partial class CardInfoReceive : System.Windows.Window
	{
        string _startdate = "";
        string _enddate = "";
        string _endDate207 = ""; 
        public ShowTranslateofLable Tr;        
		public readonly int WindowId=5;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public string MeterNUMBER = "";
        public string CardNumber = "";
        // current Water
        string _waterConsuptin207 = "";
        public List<ShowOBISValueDetail_Result> RowFlowConsumeWater = new List<ShowOBISValueDetail_Result>();
        public string MeterNumber = "";
        //
        //DateTime _creditStartDate = PersianDate.Now;
        DateTime _creditStartDate = DateTime.Now;
        bool _activeCreaditeActivation;
        int _disconnectivityOnNegativeCredit, _disconnectivityOnExpiredCredit, _creditCapabilityActivation;
        //For Permision
        bool _isEmpty1;
        bool _isEmpty2;
        //
        public TabItem TabPag
        {
            get { return _tabPag; }
            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
        public List<ReverseConsumedOBIS> _lstConsumedWater;
        public ICollectionView GroupedCustomers { get; private set; }
        System.Windows.Controls.Button _selectedButton;

	    public CardInfoReceive()
		{
            
			this.InitializeComponent();
            //if (CommonData.SelectedMeterNumber.StartsWith("207"))
            //{
            //    VeeMeterData vee = new VeeMeterData();
            //    vee.Vee207data(CommonData.SelectedMeterID, CommonData.SelectedMeterNumber, CommonData.CustomerId);
            //}
            ChangeLanguage();
            ShowTabItem();
            ChangeFlowDirection();
            if (CommonData.LanguagesID==2)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(1065);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            }    
            RefreshDataGrids(); 
            // Hidden Transfer Creedit Setting
            TeransferSettingGB.IsEnabled = false;

            if (CommonData.MeterNumber.StartsWith("207") )
            {
                ChangelstStatusposition(true);
                WaterCreditSettingGrid207.Visibility = Visibility.Visible;
                WaterCreditSettingGrid.Visibility = Visibility.Hidden;
                txtSerialNumber207.Text = CommonData.MeterNumber;
                if (CommonData.LanguagesID == 2)
                {
                    enddate207en.Visibility = Visibility.Hidden;
                    enddate207.Visibility = Visibility.Visible;
                }
                else
                {
                    enddate207en.Visibility = Visibility.Visible;
                    enddate207.Visibility = Visibility.Hidden;
                }
            }
            else 
            {
                    ChangelstStatusposition(false);
                    enddate207en.Visibility = Visibility.Hidden;
                    enddate207.Visibility = Visibility.Hidden;
                    WaterCreditSettingGrid207.Visibility = Visibility.Hidden;
                    WaterCreditSettingGrid.Visibility = Visibility.Visible;
                    txtSerialNumber.Text = CommonData.MeterNumber;
                }
           
            if (CommonData.LanguagesID == 2)
            {
                datePickerStart303.Visibility = Visibility.Visible;
                datePickerend.Visibility = Visibility.Visible;
                datePickerStart303en.Visibility = Visibility.Hidden;
                datePickerenden.Visibility = Visibility.Hidden;
                //Thread.CurrentThread.CurrentCulture = new CultureInfo(1065);
                //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                int year = datePickerStart303.SelectedDate.Year ;
                string start = datePickerStart303.Text;
                start = start.Replace(datePickerStart303.SelectedDate.Year.ToString(), year.ToString());
                datePickerStart303.Text = start;

                year = datePickerend.SelectedDate.Year;
                string end = datePickerend.Text;
                end = end.Replace(datePickerend.SelectedDate.Year.ToString(), year.ToString());
                datePickerend.Text = end;

            }
            else
            {
                datePickerStart303en.Visibility = Visibility.Visible;
                datePickerenden.Visibility = Visibility.Visible;
                datePickerStart303.Visibility = Visibility.Hidden;
                datePickerend.Visibility = Visibility.Hidden;
                datePickerStart303en.SelectedDate = DateTime.Now;
                datePickerenden.SelectedDate = DateTime.Now;
                enddate207en.SelectedDate = DateTime.Now;
            }
            
		}
        private void ShowTabItem()
        {
            try
            {
                ShowButtonAccess_Result showCreadit = CommonData.ShowButtonBinding("", 45);
                tabitemCredittocard.DataContext = showCreadit;
                if (showCreadit.CanShow != null)
                {
                    if (showCreadit.CanShow)
                    {
                        tabitemCredittocard.Header = Tr.TranslateofLable.Object27;
                        LayoutRoot.DataContext = Tr.TranslateofLable;
                        tabitemCredittocard.Header = Tr.TranslateofLable.Object27;
                        this.DataContext = Tr.TranslateofLable;
                        tabitemCredittocard.DataContext = Tr.TranslateofLable;
                    }

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());   
                CommonData.WriteLOG(ex);
            }
        }
        public void SetDate()
        {
            try
            {
                _startdate = datePickerStart303.Text;
                _enddate = datePickerend.Text;
                int day = Convert.ToInt32(_startdate.Substring(_startdate.LastIndexOf('/') + 1, _startdate.Length - _startdate.LastIndexOf('/') - 1));
                int month = Convert.ToInt32(_startdate.Substring(_startdate.IndexOf('/') + 1, _startdate.LastIndexOf('/') - _startdate.IndexOf('/') - 1));
                if (day < 10)

                    _startdate = _startdate.Substring(0, _startdate.Length - 1) + "0" + day;

                if (month < 10)
                    _startdate = _startdate.Substring(0, _startdate.IndexOf('/') + 1) + "0" + _startdate.Substring(_startdate.IndexOf('/') + 1, _startdate.Length - _startdate.IndexOf('/') - 1);
                day = Convert.ToInt32(_enddate.Substring(_enddate.LastIndexOf('/') + 1, _enddate.Length - _enddate.LastIndexOf('/') - 1));
                month = Convert.ToInt32(_enddate.Substring(_enddate.IndexOf('/') + 1, _enddate.LastIndexOf('/') - _enddate.IndexOf('/') - 1));
                if (day < 10)

                    _enddate = _enddate.Substring(0, _enddate.Length - 1) + "0" + day;

                if (month < 10)
                    _enddate = _enddate.Substring(0, _enddate.IndexOf('/') + 1) + "0" + _enddate.Substring(_enddate.IndexOf('/') + 1, _enddate.Length - _enddate.IndexOf('/') - 1);
                if (CommonData.SelectedMeterNumber.StartsWith("207"))
                {
                    _endDate207 = enddate207.Text;
                    day = Convert.ToInt32(_endDate207.Substring(_endDate207.LastIndexOf('/') + 1, _endDate207.Length - _endDate207.LastIndexOf('/') - 1));
                    month = Convert.ToInt32(_endDate207.Substring(_endDate207.IndexOf('/') + 1, _endDate207.LastIndexOf('/') - _endDate207.IndexOf('/') - 1));
                    if (day < 10)

                        _endDate207 = _endDate207.Substring(0, _endDate207.Length - 1) + "0" + day;

                    if (month < 10)
                        _endDate207 = _endDate207.Substring(0, _endDate207.IndexOf('/') + 1) + "0" + _endDate207.Substring(_endDate207.IndexOf('/') + 1, _endDate207.Length - _endDate207.IndexOf('/') - 1);

                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); 
                CommonData.WriteLOG(ex);
            }
        }
        public void changebtnVisble()
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
                        
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public void ChangeLanguage()
        {
            try
            {
                Tr = CommonData.translateWindow(WindowId);
                LayoutRoot.DataContext = Tr.TranslateofLable;
                tabitemCredittocard.Header = Tr.TranslateofLable.Object27;
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
            //    LayoutRoot.FlowDirection = CommonData.FlowDirection;
            //    lstStatus.FlowDirection = CommonData.FlowDirection;
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
                changebtnVisble();
			    _tabCtrl.SelectedItem = _tabPag;
                if (!_tabCtrl.IsVisible)
                {

                    _tabCtrl.Visibility = Visibility.Visible;

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
            try
            {
                ClassControl.OpenWin[5] = false;
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
                Refreshs();
                TranslateHeaderOfGeneralGrid();
                //TranslateHeaderOfMeterStatusGrid();
                //TranslateHeaderOfWaterEventGrid();
                //TranslateHeaderOfELEventGrid();
                //TranslateHeaderOfGeneralEventGrid();
                TranslateHeaderOfDatagridgeneralWater();
                //TranslateHeaderOfDatagridCurveInfo();
                TranslateHeaderOfDatagridConsumedWater();
                TranslateHeaderOfDatagridGeneralInfoElectrical();
                TranslateHeaderOfDatagridPowerConsumption();
                TranslateHeaderOfDatagridTarrif();

                WaterCreditSettingGrid.DataContext = Tr.TranslateofLable;
                GridCreditTransferMode.DataContext = Tr.TranslateofLable;
   
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void Refreshs()
        {
            try
            {
                
                RefreshDatagridGeneral();
                //RefreshDatagridMeterStatus();
                //RefreshDatagridWaterEvent();
                //RefreshDatagridELEvent();
                //RefreshDatagridGeneralEvent();
                RefreshDatagridgeneralWater();               
                //RefreshDatagridCurveInfo();
                ////RefreshDatagridConsumedWaternew();
               
                RefreshDatagridGeneralInfoElectrical();
                RefreshDatagridPowerConsumptionnew();
                RefreshDatagridConsumedWaternew();
                //RefreshDatagridTarrif();
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
        
        
        //public void TranslateHeaderOfWaterEventGrid()
        //{
        //    try
        //    {
        //        DatagridWaterEvent.Columns[0].Header = tr.TranslateofLable.Object8.ToString();
        //        DatagridWaterEvent.Columns[1].Header = tr.TranslateofLable.Object7.ToString();
        //        DatagridWaterEvent.Columns[2].Header = tr.TranslateofLable.Object9.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }

        //}
        //public void TranslateHeaderOfELEventGrid()
        //{
        //    try
        //    {
        //        DatagridELEvent.Columns[0].Header = tr.TranslateofLable.Object8.ToString();
        //        DatagridELEvent.Columns[1].Header = tr.TranslateofLable.Object7.ToString();
        //        DatagridELEvent.Columns[2].Header = tr.TranslateofLable.Object9.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }

        //}
        //public void TranslateHeaderOfGeneralEventGrid()
        //{
        //    try
        //    {
        //        DatagridGeneralEvent.Columns[0].Header = tr.TranslateofLable.Object8.ToString();
        //        DatagridGeneralEvent.Columns[1].Header = tr.TranslateofLable.Object7.ToString();
        //        DatagridGeneralEvent.Columns[2].Header = tr.TranslateofLable.Object9.ToString();
        //        DatagridGeneralEvent.Columns[3].Header = tr.TranslateofLable.Object12.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }

        //}
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
        //public void TranslateHeaderOfDatagridCurveInfo()
        //{
        //    try
        //    {
        //        DatagridCurveInfo.Columns[0].Header = tr.TranslateofLable.Object8.ToString();
        //        DatagridCurveInfo.Columns[1].Header = tr.TranslateofLable.Object7.ToString();
        //        DatagridCurveInfo.Columns[2].Header = tr.TranslateofLable.Object9.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }

        //}

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
                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }

        }
        
        public void ChangeColumnisibility(DataGrid grid,int columnNumber,Visibility visable)
        {
            try
            {
                grid.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
               delegate {
                   if(grid.Columns.Count> columnNumber)
                       grid.Columns[columnNumber].Visibility = visable;
               }));
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
                DatagridPowerConsumption.Columns[0].Header = Tr.TranslateofLable.Object52;
                DatagridPowerConsumption.Columns[1].Header = Tr.TranslateofLable.Object53;
                DatagridPowerConsumption.Columns[2].Header = Tr.TranslateofLable.Object54;
                DatagridPowerConsumption.Columns[3].Header = Tr.TranslateofLable.Object55;
                DatagridPowerConsumption.Columns[4].Header = Tr.TranslateofLable.Object56;
                DatagridPowerConsumption.Columns[5].Header = Tr.TranslateofLable.Object57;
                DatagridPowerConsumption.Columns[6].Header = Tr.TranslateofLable.Object58;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }

        }

        public void TranslateHeaderOfDatagridTarrif()
        {
            try
            {
                //DatagridTarrif.Columns[0].Header = tr.TranslateofLable.Object8.ToString();
                //DatagridTarrif.Columns[1].Header = tr.TranslateofLable.Object7.ToString();
                //DatagridTarrif.Columns[2].Header = tr.TranslateofLable.Object9.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        
        public void RefreshDatagridGeneral()
        {
            try
            {
                DatagridGeneral.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate()
                {
                    string Filter = "  and (charindex('\"1\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.OBISValueHeaderID=" + CommonData.OBISValueHeaderID.Value.ToString();
                    ShowObisValueDetail generalvalue = new ShowObisValueDetail(Filter, CommonData.LanguagesID);
                    if (generalvalue._lstShowOBISValueDetail.Count > 0)
                    {
                        _isEmpty2 = false;
                        DatagridGeneral.ItemsSource = generalvalue._lstShowOBISValueDetail;
                    }
                    else
                        _isEmpty2 = true;
                     // Insert some icones for Meter Version >3
                    foreach (var item in generalvalue._lstShowOBISValueDetail)
                	{
                         if (item.ObisLatinDesc == "Software Version")
                         {
                            string lastChar = "4";
                            if (!string.IsNullOrEmpty(item.Value))
                                lastChar = item.Value.Substring(item.Value.Length-1,1);
                            
                            
                            if (item.Value.Contains("RSASEWM303L0D8071001"))
                                lastChar = "4";
                            try
                            {
                                if ((Int16.Parse(lastChar)) > 3)
                                {
                                    //GridCreditTransferMode.Width = 400;
                                    //CreditTransferModes207.Width = 400;
                                    TeransferSettingGB.IsEnabled = true;
                                    StartCredit.Visibility = Visibility.Hidden;
                                    _activeCreaditeActivation = true;
                                }
                                else
                                {
                                    //GridCreditTransferMode.Width = 400;
                                    //CreditTransferModes207.Width = 400;
                                    TeransferSettingGB.IsEnabled = false;
                                    _activeCreaditeActivation = false;
                                    _disconnectivityOnNegativeCredit = 0;
                                    _disconnectivityOnExpiredCredit = 0;
                                    _creditCapabilityActivation = 0;
                                    CommonData.creditCapabilityActivation = 0;
                                    CommonData.creditStartDate = "";
                                    CommonData.disconnectivityNegativeCredit = 0;
                                    CommonData.disconnectivityExpiredCredit = 0;


                                }

                            }
                            catch (Exception)
                            {//GridCreditTransferMode.Width = 400;
                             //CreditTransferModes207.Width = 400;
                                TeransferSettingGB.IsEnabled = false;
                                _activeCreaditeActivation = false;
                                _disconnectivityOnNegativeCredit = 0;
                                _disconnectivityOnExpiredCredit = 0;
                                _creditCapabilityActivation = 0;
                                CommonData.creditCapabilityActivation = 0;
                                CommonData.creditStartDate = "";
                                CommonData.disconnectivityNegativeCredit = 0;
                                CommonData.disconnectivityExpiredCredit = 0;

                            }
                             
                         }
                      // for current Water
                            if (item.Obis == "0000010000FF")
                                RowFlowConsumeWater.Add(item);
                       

                    }
                   

                    //
                    TranslateHeaderOfGeneralGrid();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
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
        //            string Filter = "  and (charindex(obiss.type,'\"3\"')>0 or charindex(obiss.type,'\"100\"')>0) and Header.MeterID=" + CommonData.MeterID + " and ReadDate=(select MAX(Readdate)from OBISValueHeader where  MeterID=" + CommonData.MeterID + ") ";
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
        //            string Filter = "  and (charindex(obiss.type,'\"4\"')>0 or charindex(obiss.type,'\"100\"')>0) and Header.MeterID=" + CommonData.MeterID + " and ReadDate=(select MAX(Readdate)from OBISValueHeader where  MeterID=" + CommonData.MeterID + ") ";
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
        public void RefreshDatagridGeneralEvent()
        {
            try
            {
                //DatagridGeneralEvent.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                //new Action(
                //delegate()
                //{
                //    string Filter = "  and (charindex(obiss.type,'\"5\"')>0 or charindex(obiss.type,'\"100\"')>0) and Header.MeterID=" + CommonData.MeterID + " and ReadDate=(select MAX(Readdate)from OBISValueHeader where  MeterID=" + CommonData.MeterID + ") ";
                //    ShowOBISValueDetail value = new ShowOBISValueDetail(Filter, CommonData.LanguagesID);
                //    DatagridGeneralEvent.ItemsSource = null;
                //    DatagridGeneralEvent.ItemsSource = value.CollectShowOBISValueDetail;
                //}
                //));
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
                    string filter = " and Main.MeterID=" + CommonData.MeterID.ToString();
                    ShowConsumedactiveenergypivot value = new ShowConsumedactiveenergypivot(filter);
                    DatagridPowerConsumption.ItemsSource = null;
                    if (value.Lstpower.ListOfTariff.Count>0)
                        DatagridPowerConsumption.ItemsSource = value.Lstpower.ListOfTariff;

                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
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
                    string filter = " and OBISHeaderID=" + CommonData.OBISValueHeaderID.Value.ToString();
                    ShowConsumedActiveEnergy value = new ShowConsumedActiveEnergy(filter);
                    DatagridPowerConsumption.ItemsSource = null;
                    if (value._lstShowConsumedActiveEnergys.Count>0)
                     DatagridPowerConsumption.ItemsSource = value._lstShowConsumedActiveEnergys;
                }));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);   
                CommonData.WriteLOG(ex);
            }
        }
        public void RefreshDatagridgeneralWater()
        {
            try
            {
                DatagridgeneralWater.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate
                {
                    string filter = "  and (charindex('\"6\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.OBISValueHeaderID=" + CommonData.OBISValueHeaderID.Value.ToString();
                    ShowObisValueDetail value = new ShowObisValueDetail(filter, CommonData.LanguagesID);
                    DatagridgeneralWater.ItemsSource = null;
                    if (value._lstShowOBISValueDetail.Count > 0)
                    {
                        _isEmpty1 = false;// for permision
                        // For Current Water
                        foreach (var item in value._lstShowOBISValueDetail)
                        {
                            if (item.Obis == "0802010000FF")
                                RowFlowConsumeWater.Add(item);
                            if (item.Obis == "0802010100FF")
                                RowFlowConsumeWater.Add(item);
                            if (item.Obis == "0802606200FF")
                                RowFlowConsumeWater.Add(item);
                            if (item.Obis == "0802606202FF")
                                RowFlowConsumeWater.Add(item);
                        }                    
                        DatagridgeneralWater.ItemsSource = value._lstShowOBISValueDetail;                         
                    }
                    else
                    {
                        _isEmpty1 = true;// for permision
                                        // for permision
                        if (_isEmpty1 && _isEmpty2 && CommonData.UserName.ToUpper() != "ADMIN")
                        {
                            MessageBox.Show("کاربر جاری اجازه مشاهده اطلاعات این کارت را ندارد");
                            btnSendCredit2Card.IsEnabled = false;
                            btnSendCredit2Card207.IsEnabled = false;
                            txtSerialNumber.Text = "";
                            txtSerialNumber207.Text = "";
                        }
                    }
                }));
             
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);  
                CommonData.WriteLOG(ex);
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
        //           string Filter = "  and (charindex(obiss.type,'\"7\"')>0   or charindex(obiss.type,'\"100\"')>0) and  Header.MeterID=" + CommonData.MeterID + " and ReadDate=(select MAX(Readdate)from OBISValueHeader where  MeterID=" + CommonData.MeterID + ") ";
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
        //public void RefreshDatagridConsumedWater()
        //{
        //    try
        //    {
        //        DatagridConsumedWater.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
        //       new Action(
        //       delegate()
        //       {
        //           string Filter = "  and (charindex(obiss.type,'\"8\"')>0    or charindex(obiss.type,'\"100\"')>0)  and Header.MeterID=" + CommonData.MeterID + " and ReadDate=(select MAX(Readdate)from OBISValueHeader where  MeterID=" + CommonData.MeterID + ") ";
        //           ShowOBISValueDetail value = new ShowOBISValueDetail(Filter, CommonData.LanguagesID);
        //           DatagridConsumedWater.ItemsSource = null;
        //           DatagridConsumedWater.ItemsSource = value.CollectShowOBISValueDetail;
        //       }));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
        //}

        #region newrefreshforconsumedwater
        public void RefreshDatagridConsumedWaternew()
        {
            try
            {
                ShowVeeConsumedWater value1 = new ShowVeeConsumedWater(CommonData.CardMeterID);
                if (CommonData.CardMeterNumber.StartsWith("207"))
                {
                    foreach (var item in value1._lstShowVeeConsumedWater)
                    {
                        item.PumpWorkingHour = item.Flow;
                    }
                }
                DatagridConsumedWater.ItemsSource = value1._lstShowVeeConsumedWater;
            }
            catch (Exception ex)
            {
               
                CommonData.WriteLOG(ex);
            }
        }
        public void RefreshDataGridConsumedWater207()
        {
            try
            {
                ShowVeeConsumedWater value1 = new ShowVeeConsumedWater(CommonData.CardMeterID);
                DatagridConsumedWater.ItemsSource = value1._lstShowVeeConsumedWater;
                ShowConsumedWater207 value = new ShowConsumedWater207(CommonData.SelectedMeterID);
                /////// Current Water
                //if (DatagridConsumedWater207.ItemsSource != null)
                //{
                //    ShowConsumedWater207_Result rw = new ShowConsumedWater207_Result();
                //    if (DatagridConsumedWater207.Items.Count > 0)
                //    {
                //        rw = (ShowConsumedWater207_Result)DatagridConsumedWater207.Items[0];
                //        this.MeterNumber = rw.MeterNumber;
                //    }
                //}
                if (this.MeterNumber != CommonData.MeterNumber)
                {
                    //ShowConsumedWater207 value = new ShowConsumedWater207(CommonData.SelectedMeterID);
                    if (value._lstShowConsumedWater207s.Count > 0)
                        try
                        {
                            _waterConsuptin207 = value._lstShowConsumedWater207s[1].s0;
                            NumericConverter.DecimalConverter(_waterConsuptin207);
                        }
                        catch (Exception)
                        {

                            _waterConsuptin207 = "0";
                        }
                }

                //////
                 
                    //DatagridConsumedWater207.ItemsSource = null;
                    if (value._lstShowConsumedWater207s.Count>0)
                    {
                        GroupedCustomers = new ListCollectionView(value._lstShowConsumedWater207s);
                        GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("ReadDate"));
                        //DatagridConsumedWater207.ItemsSource = GroupedCustomers;
                    }
              

            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);   
                CommonData.WriteLOG(ex);
            }
        }
        
        public DataTable GetDataTableForReport(DataGrid dtg, string filter)
        {
            DataTable dt = new DataTable();
            try
            {
                ShowConsumedWater_Result row = null;
                ShowConsumedWater_Result re = new ShowConsumedWater_Result();
                Type t = re.GetType();
                MemberInfo[] members = t.GetMembers(BindingFlags.NonPublic |
                BindingFlags.Instance);
                foreach (MemberInfo member in members)
                {
                    DataColumn dc = new DataColumn();
                    int start = member.Name.IndexOf('<');
                    int end = member.Name.IndexOf('>');
                    if (start >= 0)
                    {
                        dc.ColumnName = (member.Name.Substring(start + 1, end - start - 1));
                        dt.Columns.Add(dc);
                    }

                }
                //  add each of the data rows to the table
                SabaNewEntities bank = new SabaNewEntities();

                bank.Database.Connection.Open();
                foreach (var item in bank.ShowConsumedWater(filter, CommonData.LanguagesID, CommonData.UserID))
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
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
            return dt;
        }
        public void ExecuteReport(DataTable dt, string MeterNumber, List<ReverseConsumedOBIS> revercecomumedobis)
        {
            try
            {
                List<ReverseConsumedOBIS> newlist = new List<ReverseConsumedOBIS>();
                string x = "OBISDesc";// "";
                string y = "ConsumedDate";
                string z = "ConsumedWater";
                DataTable newDt = new DataTable();
                newDt = PivotTable.GetInversedDataTable(dt, x, y, z, "-", false);

                newlist = DataTable2List(newDt, MeterNumber);
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
                    newrow.Column0Name = dt.Columns[1].ColumnName.ToString();
                    newrow.Column1Name = dt.Columns[2].ColumnName.ToString();
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
                   string filter = "  and (charindex('\"9\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.OBISValueHeaderID=" + CommonData.OBISValueHeaderID.ToString();
                   ShowObisValueDetail value = new ShowObisValueDetail(filter, CommonData.LanguagesID);
                   DatagridGeneralInfoElectrical.ItemsSource = null;
                   if (value._lstShowOBISValueDetail.Count>0)
                    DatagridGeneralInfoElectrical.ItemsSource = value._lstShowOBISValueDetail;
               }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        //public void RefreshDatagridTarrif()
        //{
        //    try
        //    {
        //         DatagridConsumedWater.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
        //      new Action(
        //      delegate()
        //      {
        //          string Filter = "  and (charindex(obiss.type,'\"11\"')>0    or charindex(obiss.type,'\"100\"')>0)  and Header.MeterID=" + CommonData.MeterID + " and ReadDate=(select MAX(Readdate)from OBISValueHeader where  MeterID=" + CommonData.MeterID + ") ";
        //        ShowOBISValueDetail value = new ShowOBISValueDetail(Filter, CommonData.LanguagesID);
        //        DatagridTarrif.ItemsSource = null;
        //        DatagridTarrif.ItemsSource = value.CollectShowOBISValueDetail;
        //      }));

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
        //}

        #region ProgressBar
       // public void changeProgressBarTag(string text)
       // {
       //     ProgressBar_ReadingCard.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
       //     new Action(
       //    delegate()
       //    {
       //        if (text.Length > 80)
       //        {
       //            ProgressBar_ReadingCard.FontSize = 20;
       //        }
       //        else
       //            ProgressBar_ReadingCard.FontSize = 26;

       //        ProgressBar_ReadingCard.Tag = text;

       //    }));
       // }
       // public void changeProgressBar_MaximumValue(int max)
       // {
       //     ProgressBar_ReadingCard.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
       //new Action(
       //    delegate()
       //    {
       //        ProgressBar_ReadingCard.Maximum = max;

       //    }));
       // }
       // public void changeProgressBarValue(double value)
       // {
       //     ProgressBar_ReadingCard.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
       //new Action(
       //    delegate()
       //    {
       //        if (value != 1000)
       //        {
       //            CommonData.counter += value;
       //        }
       //        if (value == 0)
       //        {
       //            CommonData.counter = 0;
       //            ProgressBar_ReadingCard.Visibility = System.Windows.Visibility.Visible;
       //            ProgressBar_ReadingCard.ClearValue(ProgressBar.ValueProperty);
       //            ProgressBar_ReadingCard.SetValue(ProgressBar.ValueProperty, 0.0);
       //            ProgressBar_ReadingCard.Value = 0;
       //            ProgressBar_ReadingCard.BeginAnimation(ProgressBar.ValueProperty, null);
       //            ProgressBar_ReadingCard.Value = 0;
       //        }
       //        else if (value == 1000)
       //        {
       //            ProgressBar_ReadingCard.IsIndeterminate = false;
       //            ProgressBar_ReadingCard.Orientation = Orientation.Horizontal;

       //            Duration duration = new Duration(TimeSpan.FromSeconds(1));
       //            DoubleAnimation doubleanimation = new DoubleAnimation(ProgressBar_ReadingCard.Maximum, duration);

       //            ProgressBar_ReadingCard.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);

       //        }

       //        else
       //        {
       //            ProgressBar_ReadingCard.IsIndeterminate = false;
       //            ProgressBar_ReadingCard.Orientation = Orientation.Horizontal;

       //            Duration duration = new Duration(TimeSpan.FromSeconds(1));
       //            DoubleAnimation doubleanimation = new DoubleAnimation(ProgressBar_ReadingCard.Value + value, duration);
       //            ProgressBar_ReadingCard.Value = CommonData.counter;

       //            ProgressBar_ReadingCard.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);

       //        }

       //    }));
       // }
        #endregion ProgressBar
        
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
        public void changeTabVisibility(TabItem tb, Visibility visible)
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
            try
            {
            //    txtWaterCredit.Text=string.Format("{0:0,0}", txtWaterCredit.Text);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

     

        private void datePickerStart_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void btnSendCredit2Card_Click(object sender, RoutedEventArgs e)
        {
            CommonData.mainwindow.changeProgressBarTag("");
            MeterNUMBER = txtSerialNumber.Text;
            bool correctData = true;
            btnSendCredit2Card.IsEnabled = false;
            // insert for version 4 or upper
            if (TeransferSettingGB.IsEnabled && _creditCapabilityActivation == 1)
            {
                string persianDate1 = datePickerCredit.Text;               
                PersianCalendar p = new PersianCalendar();      
                string[] d = persianDate1.Split('/');
                _creditStartDate = p.ToDateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), 0, 0, 0, 0);           
            }      
            //
            try
            {
                if (txtSerialNumber.Text.Length==11)
                    CommonData.CardMeterNumber = txtSerialNumber.Text.Substring(2, 1) + txtSerialNumber.Text.Substring(5, 6);
                if (txtSerialNumber.Text.Length == 13)
                    CommonData.CardMeterNumber = txtSerialNumber.Text.Substring(7, 6);

                Convert.ToInt32(CommonData.CardMeterNumber);
            }
            catch (Exception)
            {
                txtWaterCredit.Background = Brushes.Bisque;
                CommonData.CardMeterNumber = "";
                txtWaterCredit.Clear();
                correctData = false;
            }

            if (txtWaterCredit.Text.Length == 0)
            {

                txtWaterCredit.Background = Brushes.Bisque;
                txtWaterCredit.Clear();
                correctData = false;
            }
            else
            {
                try
                {
                    double d = NumericConverter.DoubleConverter(txtWaterCredit.Text);
                    if (d > 1000000)
                    {
                        txtWaterCredit.Background = Brushes.Bisque;
                        txtWaterCredit.Clear();
                        correctData = false;
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message25);
                        MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message25);
                        btnSendCredit2Card.IsEnabled = true;
                        return;
                    }
                    //if (lbllw_0000603E01_2.Content.ToString() != "")
                    //{

                    //    double d1 = -1;
                    //    double.TryParse(lbllw_0000603E01_2.Content.ToString(), out d1);

                    //    if (chk1.IsChecked == false && d1 > 0)
                    //        if (d + d1 > 1999990)
                    //        {
                    //            MessageBoxResult mr = MessageBox.Show("مجموع حق آبه داده شده به کنتور با مقدار جدید بیش از حداکثر حق آبه مجاز است. در صورت تمایل به تخصیص حق آبه موردنظر، دکمه تایید را بزنید."
                    //                , "هشدار", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
                    //            if (mr == MessageBoxResult.Cancel)
                    //                return;
                    //        }
                    //}
                     Int64 i = Convert.ToInt64(d * 1000);
                }
                catch (Exception)
                {
                    txtWaterCredit.Background = Brushes.Bisque;
                    txtWaterCredit.Clear();
                    correctData = false;
                }
            }



            if (!correctData)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message26);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message26);
                btnSendCredit2Card.IsEnabled = true;
                return;
            }
            ChangeButtonVisibility(btnSendCredit2Card, Visibility.Hidden);
            txtWaterCredit.Background = Brushes.WhiteSmoke;
            txtSerialNumber.Background = Brushes.WhiteSmoke;
            CommonData.mainwindow.changeProgressBarValue(0);
            DateTime dts = DateTime.Now;
            DateTime dte = DateTime.Now;
            if (CommonData.LanguagesID == 2)
            {
                SetDate();
                
                int year = Int16.Parse(_startdate.Substring(0, 4));

                if (year >= 1410 || year <= 1390)
                {
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message61);
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message61);
                    btnSendCredit2Card.IsEnabled = true;
                    ChangeButtonVisibility(btnSendCredit2Card, Visibility.Visible);
                    return;
                }

                int month = Int16.Parse(_startdate.Substring(5, 2));
                int day = Int16.Parse(_startdate.Substring(8, 2));

                int hour = startTimePicker.DateTimeValue.Value.Hour;
                int minute = startTimePicker.DateTimeValue.Value.Minute;
                int second = startTimePicker.DateTimeValue.Value.Second;
                dts = PersianDateTime.ConvertToGeorgianDateTime(year,month,day,hour,minute,second);

                year = Int16.Parse(_enddate.Substring(0, 4));

                if (year >= 1410 || year <= 1390)
                {
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message62);
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message62);
                    btnSendCredit2Card.IsEnabled = true;
                    ChangeButtonVisibility(btnSendCredit2Card, Visibility.Visible);
                    return;
                }
                month = Int16.Parse(_enddate.Substring(5, 2));
                day = Int16.Parse(_enddate.Substring(8, 2));
                
                hour = TimePickerend.DateTimeValue.Value.Hour;
                minute = TimePickerend.DateTimeValue.Value.Minute;
                second = TimePickerend.DateTimeValue.Value.Second;
                dte = PersianDateTime.ConvertToGeorgianDateTime(year, month, day, hour, minute, second);
                //if(!dte.ToString().Contains("M"))
                //{
                //    string trr = dte.ToMiladiString();
                //    //dte = DateTime.Parse(trr);
                //    dte = DateTime.ParseExact(trr, "MM/dd/yyyy hh:mm:ss tt",null);
                //}
            }
            else
            {
                if (Int16.Parse(datePickerStart303en.Text.Substring(datePickerStart303en.Text.LastIndexOf('/') + 1, 4)) > 2022 || Int16.Parse(datePickerStart303en.Text.Substring(datePickerStart303en.Text.LastIndexOf('/') + 1, 4)) < 2012)
                {
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message61);
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message61);
                    btnSendCredit2Card.IsEnabled = true;
                    ChangeButtonVisibility(btnSendCredit2Card, Visibility.Visible);
                    return;
                }
                DateTime pd = Convert.ToDateTime(datePickerStart303en.Text);
                TimeSpan ts = new TimeSpan(startTimePicker.DateTimeValue.Value.Hour, startTimePicker.DateTimeValue.Value.Minute, startTimePicker.DateTimeValue.Value.Second);
                pd = pd.Date + ts;
               
                dts = pd;
                if (Int16.Parse(datePickerStart303en.Text.Substring(datePickerStart303en.Text.LastIndexOf('/') + 1, 4)) > 2022 || Int16.Parse(datePickerStart303en.Text.Substring(datePickerStart303en.Text.LastIndexOf('/') + 1, 4)) < 2012)
                {
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message62);
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message62);
                    btnSendCredit2Card.IsEnabled = true;
                    ChangeButtonVisibility(btnSendCredit2Card, Visibility.Visible);
                    return;
                }
                pd = Convert.ToDateTime(datePickerenden.Text);
                ts = new TimeSpan(TimePickerend.DateTimeValue.Value.Hour, TimePickerend.DateTimeValue.Value.Minute, TimePickerend.DateTimeValue.Value.Second);
                pd = pd.Date + ts;
                
                dte = pd;
            }
            CommonData.mainwindow.changeProgressBarValue(1);

            UInt64 number = UInt64.Parse(CommonData.CardMeterNumber);
            string serialNumber = number.ToString("x");

            if (dts >= dte)
            {
                CommonData.mainwindow.changeProgressBarTag("تاریخ پایان اعتبار را به درستی وارد کنید");
                MessageBox.Show("تاریخ پایان اعتبار را به درستی وارد کنید");
                datePickerend.Background = Brushes.Bisque;
                btnSendCredit2Card.IsEnabled = true;
                ChangeButtonVisibility(btnSendCredit2Card, Visibility.Visible);
                return;
            }
            datePickerend.Background = Brushes.WhiteSmoke;
            WaterCreditSettingGrid.IsEnabled = false;
            Token.Token t = new Token.Token(serialNumber, txtWaterCredit.Text, dts, dte, GetCreditTransferModem());

            if (_activeCreaditeActivation)
            {
                
                if (_creditCapabilityActivation == 1 && ActiveRadio1.IsChecked==true)
                {
                    CommonData.creditCapabilityActivation = 1;
                    CommonData.creditStartDate = datePickerCredit.Text;
                }
                else
                    CommonData.creditCapabilityActivation = _creditCapabilityActivation;
              
               CommonData.disconnectivityNegativeCredit = _disconnectivityOnNegativeCredit;              
                
               CommonData.disconnectivityExpiredCredit = _disconnectivityOnExpiredCredit;
           
            }


            t.getToken += new tokenDataReceived(GetTokenFromUsb);
            btnSendCredit2Card.IsEnabled = true;
            

        }
        public void ChangeButtonVisibility(System.Windows.Controls.Button btn, Visibility visibility)
        {

            btn.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                    delegate()
                    {
                        btn.Visibility = visibility;
                    }));
        }
        public void Send2Card()
        {

            string ErControl = "";
            // ch3 - bit 7
            if (chk3.IsChecked == true)
                ErControl += "1";
            else
                ErControl += "0";
            // ch1 - bit 6
            if (chk1.IsChecked == true)
                ErControl += "1";
            else
                ErControl += "0";
            // ch2 - bit 5
            if (chk2.IsChecked == true)
                ErControl += "1";
            else
                ErControl += "0";
            // ch4 - bit 4
            if (chk4.IsChecked == true)
                ErControl += "1";
            else
                ErControl += "0";

            string[,] cardSendData = new string[3, 3];
            //Credit Remained
            cardSendData[0, 0] = (Convert.ToInt32(txtWaterCredit.Text) * 10).ToString("X8");
            cardSendData[1, 0] = "1003";
            cardSendData[2, 0] = "00";
            //End Credit Date
            //int y = Convert.ToInt16(datePicker1.Text.Substring(0, 2));
            //int m = Convert.ToInt16(datePicker1.Text.Substring(3, 2));
            //int d = Convert.ToInt16(datePicker1.Text.Substring(6, 2));
            //CardSendData[0, 1] = "00" + y.ToString("X2") + m.ToString("X2") + d.ToString("X2");
            //CardSendData[1, 1] = "1003";
            //CardSendData[2, 1] = "01";
            ////Error Control
            //CardSendData[0, 2] = Convert.ToInt16(ErControl + "0000", 2).ToString("X8");
            //CardSendData[1, 2] = "1003";
            //CardSendData[2, 2] = "03";
            //CardManager card1 = new CardManager();
            //card1.Write2Card(CardSendData, true);

        }
        private void GetTokenFromUsb(object sender, TokenEventArgs arg)
        {
            CommonData.mainwindow.changeProgressBarValue(1);

            CardManager cm = new CardManager();

            //
            //try
            //{
            //    if (TeransferSettingGB.IsVisible)
            //    {
            //}
            //else
            //{
            //    ActiveCreaditeActivation = false;
            //    disconnectivity_On_Negative_Credit = 0;
            //    disconnectivity_On_Expired_Credit = 0;
            //    credit_Capability_Activation = 0;
            //}

            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.ToString());
            //}
       
            int m = cm.WriteTokenTo303Card(arg.TokenData,MeterNUMBER, _activeCreaditeActivation, _creditCapabilityActivation, _creditStartDate, _disconnectivityOnNegativeCredit, _disconnectivityOnExpiredCredit);
            if (m == 1)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message24);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message24);
            }
            else if (m == 0)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message15);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message15);
            }
            else if (m == -1)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message14);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message14);
            }
            else if (m == -2)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message84);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message84);
            }

                CommonData.mainwindow.changeProgressBarValue(1000);
            
            ChangeButtonVisibility(btnSendCredit2Card, Visibility.Visible);
            ChangeGridEnable(true);

        }
        public void ChangeGridEnable(bool isEnable)
        {

            WaterCreditSettingGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            new Action(
              delegate {
                  WaterCreditSettingGrid.IsEnabled = isEnable;
              }));
        }
        string GetCreditTransferModem()
        {
            //StringBuilder CredirtransferModes = new StringBuilder("00000", 5);
            int intCreditMode = 0;
            //***********************************************************************
            if (chk1.IsChecked == true)
            {
                intCreditMode = 1;
                //CredirtransferModes[0] = '1';
            }
            //***********************************************************************
            if (chk2.IsChecked == true)
            {
                intCreditMode += Convert.ToInt32(Math.Pow(2, 1));

                //CredirtransferModes[1] = '1';
            }


            //***********************************************************************
            if (chk3.IsChecked == true)
            {
                intCreditMode += Convert.ToInt32(Math.Pow(2, 2));
                //CredirtransferModes[2] = '1';
            }

            //***********************************************************************
            if (chk4.IsChecked == true)
            {
                intCreditMode += Convert.ToInt32(Math.Pow(2, 3));
                //CredirtransferModes[3] = '1';
            }


            //***********************************************************************
            if (chk5.IsChecked == true)
            {
                intCreditMode += Convert.ToInt32(Math.Pow(2, 4));
                //CredirtransferModes[4] = '1';
            }


            //***********************************************************************           
            return intCreditMode.ToString("X");
        }
        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataGrids();
        }

        private void tabitemCredittocard_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (CommonData.LanguagesID == 2)
            //    {
            //        datePickerStart303.SelectedDate = PersianDate.Now;
            //        datePickerend.SelectedDate = PersianDate.Now;
            //    }
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            //}
        }

        private void btnSendCredit2Card207_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              

                string erControl = "";
                // ch3 - bit 7
                if (chk3207.IsChecked==true)
                    erControl += "1";
                else
                    erControl += "0";
                // ch1 - bit 6
                if (chk1207.IsChecked==true)
                    erControl += "1";
                else
                    erControl += "0";
                // ch2 - bit 5
                if (chk2207.IsChecked==true)
                    erControl += "1";
                else
                    erControl += "0";
                // ch4 - bit 4
                if (chk4207.IsChecked==true)
                    erControl += "1";
                else
                    erControl += "0";
                if (txtWaterCredit207.Text == "")
                {
                    System.Windows.Forms.MessageBox.Show("لطفا مقدار حق آبه را مشخص کنید");
                    return;
                }
                
                string creditRemained = GeneralFunctions.Convert2FixString(GeneralFunctions.int2Hex((10 *Convert.ToDecimal( txtWaterCredit207.Text)).ToString()), "0", 8);
                SetDate();

                string pd = "";
                if (CommonData.LanguagesID == 2)
                {
                    var PersianDate = DateTime.Now.CurrentPersianDateString();
                    var re1 = _endDate207.CompareTo(PersianDate);
                    if (re1 < 1)
                    {
                        System.Windows.Forms.MessageBox.Show("لطفا تاریخ پایان اعتبار را به درستی مشخص کنید");
                        return;
                    }
                    pd = _endDate207;
                }
                else
                {
                    enddate207en.SelectedDate = Convert.ToDateTime(enddate207en.Text);
                    pd = enddate207en.SelectedDate.Value.Year + "/";
                    if (enddate207en.SelectedDate.Value.Month.ToString().Length < 2)
                        pd = pd + "0" + enddate207en.SelectedDate.Value.Month + "/";
                    else
                        pd = pd + enddate207en.SelectedDate.Value.Month + "/";
                    if (enddate207en.SelectedDate.Value.Day.ToString().Length < 2)
                        pd = pd + "0" + enddate207en.SelectedDate.Value.Day;
                    else
                        pd = pd + enddate207en.SelectedDate.Value.Day;
                }
                string endCreditDate = GeneralFunctions.Convert2FixString(GeneralFunctions.Convert10DigitDate28DigitHex(pd), "0", 8);
                string newErrorControl = GeneralFunctions.Binary2Hex(erControl, 1);
                string errorControl = GeneralFunctions.Binary2Hex(erControl + "0000", 8);
                MeterNUMBER = txtSerialNumber207.Text;
                newErrorControl = newErrorControl.PadLeft(2, '0');
                CardManager cm = new CardManager();

                int m = cm.WriteCreditTo207Card(creditRemained, endCreditDate, errorControl, MeterNUMBER);

                 CardReadOut cr = cm.getCardData();
                if (m==-1)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message14);
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message14);
                    CommonData.mainwindow.changeProgressBarValue(1000);
                }
                else if (m == -2)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message84);
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message84);
                    CommonData.mainwindow.changeProgressBarValue(1000);
                }

                else if (m == 1)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message24);
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message24);
                    CommonData.mainwindow.changeProgressBarValue(1000);
                    ObjectParameter result = new ObjectParameter("Result", 10000000000000000);
                    ObjectParameter errMsg = new ObjectParameter("ErrMSG", "");
                    ObjectParameter tokenId = new ObjectParameter("TokenID", 10000000000000000);
                    //System.Data.Objects.ObjectParameter IDCredit = new System.Data.Objects.ObjectParameter("IDCredit", 10000000000000000);

                    if (errorControl.Length < 2)
                        errorControl = "0" + errorControl;
                    string endDate = enddate207.Text;
                    // token = Convert.ToDecimal(finalToken);
                    if (CommonData.LanguagesID == 2)
                    {

                      
                        string[] str = endDate.Split('/');
                        if (CommonData.LanguagesID == 2)
                        {
                            endDate = PersianDateTime.ConvertToGeorgianDateTime(
                           Convert.ToInt32(str[0]),
                           Convert.ToInt32(str[1]),
                           Convert.ToInt32(str[2])).ToRsaString();
                        }
                    }
                    else
                        endDate = enddate207en.Text;
                    DateTime startDateTime, endDateTime;
                    endDateTime = Convert.ToDateTime(endDate);
                    startDateTime = DateTime.Now;
                    ObjectParameter cardId = new ObjectParameter("CardID", 1000000000000);
                    SQLSPS.INSCards(cr.CardNumber, cardId, result, errMsg);
                    SQLSPS.InsToken("", CommonData.UserID, "", 0, "", DateTime.Now.ToRsaString(), Convert.ToDecimal(cardId.Value), 0, 
                        CommonData.CardMeterID, newErrorControl, startDateTime.ToMiladiString(), endDateTime.ToMiladiString(), Convert.ToDecimal(txtWaterCredit207.Text),
                        "", CommonData.OBISValueHeaderID,0,null,0,0,tokenId, result, errMsg);
                
                }
                else if (m == 0)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message15);
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message15);
                    CommonData.mainwindow.changeProgressBarValue(1000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btnS_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                _303 obj303 = new _303();
                _207 obj207 = new _207();
                List<Status_Result> list = new List<Status_Result>();


                if (_selectedButton != null)
                    _selectedButton.Background = Brushes.SkyBlue;
                System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
                _selectedButton = btn;
                btn.Background = Brushes.LimeGreen;
                string btnName = btn.Name.Replace("btnS_", "");
                switch (btnName)
                {
                    case "1":
                        try
                        {
                            string value1 = RefreshDatagrid_303("0000603F00FF");
                            string value2 = RefreshDatagrid_303("0000603F01FF");
                            string value3 = RefreshDatagrid_303("0000603F02FF");
                            list = obj303.PerformanceMeteroncreditevents(value1.PadLeft(8, '0'), value2.PadLeft(8, '0'), value3.PadLeft(8, '0'), CommonData.LanguageName);
                          
                        }
                        catch { }
                        break;

                    case "2":
                        try
                        {
                            //Value = RefreshDatagrid_303("0000600302FF");
                            //List = Obj303.statusRegister("0000600302FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            string value = RefreshDatagrid_303("0000600A03FF");
                            if (value != "")
                                list = obj303.statusRegister_0000600A03FF(value.Substring(0, 8), CommonData.LanguageName);
                            else
                            {
                                value = RefreshDatagrid_303("0000600302FF");
                                
                                if (value != "")
                                    list = obj303.statusRegister_0000600302FF(value.PadLeft(8, '0'), CommonData.LanguageName);
                                else
                                {
                                    Status s = new Status();
                                    Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                    list.Add(sr);
                                }
                            }

                        }
                        catch { }
                        break;

                    case "3":
                        try
                        {
                            string value = RefreshDatagrid_303("0000600A01FF");
                            if(value!="")
                            list = obj303.statusRegister("0000600A01FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();
                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);
                            }
                        }
                        catch { }
                        break;

                    case "4":
                        try
                        {
                            string value = RefreshDatagrid_303("0000600A02FF");
                            if(value!="")
                             list = obj303.statusRegister("0000600A02FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();
                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);
                            }
                        }
                        catch { }
                        break;

                    case "5":
                        try
                        {
                            string value = RefreshDatagrid_303("0000600A03FF");
                            if(value!="")
                                list = obj303.statusRegister("0000600A03FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();
                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);
                            }
                        }
                        catch { }
                        break;

                    case "6":
                        try
                        {
                            string value = RefreshDatagrid_303("0000600A04FF");
                            list = obj303.statusRegister("0000600A04FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "7":
                        try
                        {
                            string value = RefreshDatagrid_303("0000600A05FF");
                            list = obj303.statusRegister("0000600A05FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "8":
                        try
                        {
                            string value = RefreshDatagrid_303("0000600A01FF");
                            list = obj303.statusRegister("0000600A01FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch
                        {
                            // ignored
                        }
                        break;

                    case "9": try
                        {
                            string value = RefreshDatagrid_303("0000600A01FF");
                            list = obj303.statusRegister("0000600A01FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "10":
                        try
                        {
                            string value = RefreshDatagrid_303("0000600A01FF");
                            list = obj303.statusRegister("0000600A01FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "11":
                        try
                        {
                            string value = RefreshDatagrid_303("0000616100FF");
                            list = obj303.statusRegister("0000616100FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "12": try
                        {
                            string value = RefreshDatagrid_303("0000616102FF");
                            list = obj303.statusRegister("0000616102FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "13":
                        try
                        {
                            string value = RefreshDatagrid_303("0100600A03FF");
                            if(value!="")
                                list = obj303.statusRegister("0100600A03FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();
                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);
                            }
                        }
                        catch { }
                        break;

                    case "14":
                        try
                        {
                            string value = RefreshDatagrid_303("0100600501FF");
                            if(value!="")
                                list = obj303.statusRegister("0100600501FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();
                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "15":
                        try
                        {
                            string value = RefreshDatagrid_303("0100600A00FF");
                            if(value!="")
                                list = obj303.statusRegister("0100600A00FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();
                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);

                            }
                        }
                        catch
                        {
                            // ignored
                        }
                        break;

                    case "16":
                        try
                        {
                            string value = RefreshDatagrid_303("0100600A01FF");
                            if(value!="")
                            list = obj303.statusRegister("0100600A01FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();
                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "17":
                        try
                        {
                            string value = RefreshDatagrid_303("0100616101FF");
                            if(value!="")
                                list = obj303.statusRegister("0100616101FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();
                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "18":                        
                        try
                        {
                            string value = "";
                            //Value = RefreshDatagrid_303("0802606101FF");
                            //List = Obj303.statusRegister("0802606101FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            value = RefreshDatagrid_303("0000600A02FF");
                            if (value != "")
                                list = obj303.GetPumpStatusFrom_0000600A02FF( value.PadLeft(8, '0'), CommonData.LanguageName);
                            //list = obj303.statusRegister("0000600A02FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                value = RefreshDatagrid_303("0802606101FF");
                                if (value != "")
                                    list = obj303.statusRegister("0802606101FF", value.PadLeft(8, '0'), CommonData.LanguageName);

                                else
                                {
                                    Status s = new Status();

                                    Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                    list.Add(sr);

                                }
                            }
                        }
                        
                        catch { }
                        break;

                    case "19":
                        try
                        {
                            string value = RefreshDatagrid_303("0000600303FF");
                            if(value!="")
                             list = obj303.statusRegister("0000600303FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();
                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);
                            }
                        }
                        catch { }
                        break;

                    case "20":
                        try
                        {
                            //Value = RefreshDatagrid_303("0000600404FF");
                            //List = Obj303.statusRegister("0000600404FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            string value = RefreshDatagrid_303("0000600A02FF");
                            if (value != "")
                            {
                                list = obj303.statusRegister("0000600A02FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                                //list = obj303.StatuseGeneralRegister2(value.PadLeft(8, '0'), CommonData.LanguageName, 1);
                            }
                            else
                            {
                                Status s = new Status();
                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);
                            }

                        }
                        catch { }
                        break;

                    case "21":
                        try
                        {
                            string value = RefreshDatagrid_303("0000603E04FF");
                            if(value!="")
                            {
                                list = obj303.statusRegister("0000603E04FF", value.PadLeft(8, '0'), CommonData.LanguageName);
                                foreach (var item in list)
                                {
                                    item.IsStatuseTrue = Status.dontCare;
                                }

                            }
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                list.Add(sr);

                            }
                        }
                        catch { }
                        break;
                    case "207":
                        try
                        {
                            string value1 = RefreshDatagrid_303("0802606101FF");
                            string value2 = RefreshDatagrid_303("0000603E01FF");
                            value1 = value1.Trim();
                            list = obj207.StatusRegister207_0802606101FF(value2,value1, CommonData.LanguageName);
                        }
                        catch { }
                        break;
                    default:
                        break;
                }
                //Datagrid_303.ItemsSource = null;

                //Datagrid_303.ItemsSource = List;
                Changelist(list);
            }
            catch
            {
            }
        }
        public string RefreshDatagrid_303(string obis)
        {
            try
            {
                string value = "";
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                string filter = " and Header.OBISValueHeaderID=" + CommonData.OBISValueHeaderID.Value.ToString() +"  and OBISs.OBIS='" + obis + "'";
                ShowObisValueDetail EGeneralvalue = new ShowObisValueDetail(filter, CommonData.LanguagesID);
                foreach (ShowOBISValueDetail_Result item in bank.ShowOBISValueDetail(filter, CommonData.LanguagesID, CommonData.UserID))
                    value = item.Value;
                bank.Database.Connection.Close();
                return value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                
                CommonData.WriteLOG(ex);                
                return "";

            }
        }
        public void Changelist(List<Status_Result> list)
        {
            try
            {
                lstStatus.Items.Clear();
                foreach (var item in list)
                {
                    if (item.IsStatuseTrue.ToString() == "True" || item.IsStatuseTrue.ToString() == "False")
                    {
                        CheckBox chBox = new CheckBox();
                        chBox.IsHitTestVisible = false;
                        lstStatus.Items.Add(chBox);
                        chBox.Content = item.Description;
                        if (item.IsStatuseTrue.ToString() == "True")
                        {
                            chBox.IsChecked = true;
                            chBox.Background = Brushes.GreenYellow;
                            chBox.Foreground = Brushes.Green;
                        }
                        else
                            chBox.IsChecked = false;
                    }
                    else if (item.IsStatuseTrue.ToString() == "dontCare")
                    {
                        Label lb1 = new Label();
                        lb1.Content = item.Description;
                        lstStatus.Items.Add(lb1);

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
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
                dlg.FileName = selectedtab + "(" + CommonData.MeterNumber + ")"; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = " (.xlsx)|*.xlsx"; // Filter files by extension 
                Nullable<bool> result = dlg.ShowDialog();
                string filePath = dlg.FileName;
                if (result.Value)
                {
                    if (tabControlMain.SelectedItem == tabitemPowerConsumption)
                    {
                        List<ShowConsumedatariffctiveenergypivot_Result> sublist = new List<ShowConsumedatariffctiveenergypivot_Result>();
                        object[] header = { "ConsumedDate", "ACtiveEnergy", "ReACtiveEnergy", "CReactiveEnergy", "NumberofNominalDemandViolation", "MaxDemand", "RealDemand", "lstTariff" };
                        object[] headerText = { "ConsumedDate", "ACtiveEnergy", "ReACtiveEnergy", "CReactiveEnergy", "NumberofNominalDemandViolation", "MaxDemand", "RealDemand" };
                        object[] subheaderText = { "Tariff1", "Tariff2", "Tariff3", "Tariff4", "Tariff5", "Tariff6", "Tariff7", "Tariff8", "Tariff9", "Tariff10", "Tariff11", "Tariff12", "Tariff13", "Tariff14", "Tariff15", "Tariff16", "Tariff17", "Tariff18", "Tariff19", "Tariff20", "Tariff21", "Tariff22", "Tariff23", "Tariff24", "Tariff25", "Tariff26" };
                        object[] subheader = { "description", "t1", "t2", "t3", "t4", "t5", "t6", "t7", "t8", "t9", "t10", "t11", "t12", "t13", "t14", "t15", "t16", "t17", "t18", "t19", "t20", "t21", "t22", "t23", "t24", "t25", "t26" };
                        ExcelSerializernew<Consumedpower, ShowConsumedatariffctiveenergypivot_Result, List<Consumedpower>> s = new ExcelSerializernew<Consumedpower, ShowConsumedatariffctiveenergypivot_Result, List<Consumedpower>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridPowerConsumption.ItemsSource);
                        s.header = header;
                        s.SubheaderText = subheaderText;
                        s.headerText = headerText;
                        s.Subheader = subheader;
                        s.CreateWorkSheet(filePath, "Power Consumption", CommonData.MeterNumber, (List<Consumedpower>)view.SourceCollection, CommonData.ClockValue, true,"Card");
                    }
                    else if (tabControlMain.SelectedItem == tabitemConsumedWater)
                    {
                        //object[] header = { "ConsumedDate", "w", "WT"};
                        //object[] headerText = { "ConsumedDate", "Consumed Water", "Total Consumed Water" };
                        //ExcelSerializernew<ShowConsumedWaterPivot_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowConsumedWaterPivot_Result>> s = new ExcelSerializernew<ShowConsumedWaterPivot_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowConsumedWaterPivot_Result>>();
                        //ICollectionView view = CollectionViewSource.GetDefaultView(DatagridConsumedWater.ItemsSource);
                        //s.header = header;
                        //s.headerText = headerText;
                        //s.CreateWorkSheet(filePath, "Water Consumption", CommonData.MeterNumber, (List<ShowConsumedWaterPivot_Result>)view.SourceCollection, CommonData.ClockValue, false, "Card");



                        object[] header = { "ConsumedDate", "MonthlyConsumption", "TotalConsumption", "PumpWorkingHour" };
                        object[] headerText = { "ConsumedDate", "Consumed Water", "Total Consumed Water", "Time of pump working" };
                        ExcelSerializernew<ShowVeeConsumedWater_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowVeeConsumedWater_Result>> s =
                            new ExcelSerializernew<ShowVeeConsumedWater_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowVeeConsumedWater_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridConsumedWater.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(filePath, Tr.TranslateofLable.Object34, CommonData.MeterNumber, (List<ShowVeeConsumedWater_Result>)view.SourceCollection, CommonData.ClockValue, false, "Card");










                    }
                    else if (tabControlMain.SelectedItem == tabitemgeneralWater)
                    {
                        object[] header = { "ObisDesc", "Value", "OBISUnitDesc" };
                        object[] headerText = { "Title", "Value", "Unit" };
                        ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>> s = new ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridgeneralWater.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(filePath, "General Information water", CommonData.MeterNumber, (List<ShowOBISValueDetail_Result>)view.SourceCollection, CommonData.ClockValue, false, "Card");
                    }
                    else if (tabControlMain.SelectedItem == tabitemGeneralInfoElectrical)
                    {
                        object[] header = { "ObisDesc", "Value", "OBISUnitDesc" };
                        object[] headerText = { "Title", "Value", "Unit" };
                        ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>> s = new ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridGeneralInfoElectrical.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(filePath, "General information Power", CommonData.MeterNumber, (List<ShowOBISValueDetail_Result>)view.SourceCollection, CommonData.ClockValue, false, "Card");
                    }
                    else if (tabControlMain.SelectedItem == tabitemGeneral)
                    {
                        object[] header = { "ObisDesc", "Value", "OBISUnitDesc" };
                        object[] headerText = { "Title", "Value", "Unit" };
                        ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>> s = new ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridGeneral.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(filePath, "Meters General Information", CommonData.MeterNumber, (List<ShowOBISValueDetail_Result>)view.SourceCollection, CommonData.ClockValue, false, "Card");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
            
        }

        private void btnStatus_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
                if(btn != null)
                    btn.Background = Brushes.Orange;
                if (_selectedButton != null)
                    _selectedButton.Background = Brushes.LimeGreen;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void BtnStatus_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
                if(btn!= null)
                    btn.Background = Brushes.SkyBlue;
                if (_selectedButton != null)
                    _selectedButton.Background = Brushes.LimeGreen;
            }
            catch(Exception ex)
            {}
        }
        public void changeTabVisibility( Visibility visible)
        {
            tabitemCredittocard.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                delegate
                {
                    tabitemCredittocard.Visibility = visible;
                    tabitemGeneral.Focus();
                }));
        }
        private void Meter207Status()
        {
            try
            {
                if (CommonData.SelectedMeterNumber.StartsWith("207"))
                {
                    _207 obj207 = new _207();
                    List<Status_Result> list = new List<Status_Result>();
                  
                    string value1 = RefreshDatagrid_303("0802606101FF");
                    string value2 = RefreshDatagrid_303("0000603E01FF");
                    value1 = value1.Trim();
                    list = obj207.StatusRegister207_0802606101FF(value2,value1, CommonData.LanguageName);
                  
                    Changelist(list);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        private void ChangelstStatusposition(bool is207)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate
                {
                    if (is207)
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
        private void tabitemMeterStatus_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommonData.SelectedMeterNumber.StartsWith("207"))
                {
                    Meter207Status();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void tabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tabControlMain.SelectedItem == tabitemMeterStatus)// || tabControlMain.SelectedItem == tabitemConsumedWater207)
                {
                    ToolStripButtonImport.Visibility = Visibility.Collapsed;
                    lblExport.Visibility = Visibility.Collapsed;

                }
                else
                {
                    ToolStripButtonImport.Visibility = Visibility.Visible;
                    lblExport.Visibility = Visibility.Visible;

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void txtWaterCredit207_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                foreach (char item in e.Text)
                    if (!Char.IsDigit(item) && !Char.IsControl(item))
                        e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void txtWaterCredit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                foreach (char item in e.Text)
                    if (!Char.IsDigit(item) && !Char.IsControl(item))
                        e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void chk20_Click(object sender, RoutedEventArgs e)
        {
            if (chk20.IsChecked == true)
            {
                //disconnectivity_On_Negative_Credit = 1;
                ActiveRadio2.Visibility = Visibility.Visible;
                DiActiveRadio2.Visibility = Visibility.Visible;
                ActiveGrid2.Visibility = Visibility.Visible;
                _activeCreaditeActivation = true;
                _disconnectivityOnNegativeCredit = 1;
            }
            else
            {
                //disconnectivity_On_Negative_Credit = -1;
                ActiveRadio2.Visibility = Visibility.Hidden;
                DiActiveRadio2.Visibility = Visibility.Hidden;
                ActiveGrid2.Visibility = Visibility.Hidden;             
                _disconnectivityOnNegativeCredit = 0;
            }
        }

        private void chk30_Click(object sender, RoutedEventArgs e)
        {
            if (chk30.IsChecked == true)
            {
                //disconnectivity_On_Expired_Credit = 1;
                ActiveRadio3.Visibility = Visibility.Visible;
                DiActiveRadio3.Visibility = Visibility.Visible;
                ActiveGrid3.Visibility = Visibility.Visible;
                _activeCreaditeActivation = true;
                _disconnectivityOnExpiredCredit = 1;
            }
            else
            {
                //  disconnectivity_On_Expired_Credit = -1;
                ActiveRadio3.Visibility = Visibility.Hidden;
                DiActiveRadio3.Visibility = Visibility.Hidden;
                ActiveGrid3.Visibility = Visibility.Hidden;
                _disconnectivityOnExpiredCredit = 0;
            }
        }
        
        private void datePickerCredit_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            _creditStartDate = datePickerCredited.SelectedDate.Value;
        }

        private void ActiveRadio_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveRadio1.IsChecked == true && _creditCapabilityActivation!=0)
                _creditCapabilityActivation = 1;
            else if (ActiveRadio1.IsChecked == false && _creditCapabilityActivation != 0)
                _creditCapabilityActivation = -1;

             if (ActiveRadio2.IsChecked == true && _disconnectivityOnNegativeCredit!=0)
                _disconnectivityOnNegativeCredit = 1;
            else if (ActiveRadio2.IsChecked == false && _disconnectivityOnNegativeCredit != 0)
                _disconnectivityOnNegativeCredit = -1;

             if (ActiveRadio3.IsChecked == true && _disconnectivityOnExpiredCredit!=0)
                _disconnectivityOnExpiredCredit = 1;
            else if (ActiveRadio3.IsChecked == false && _disconnectivityOnExpiredCredit != 0)
                _disconnectivityOnExpiredCredit = -1;
             
        }

        private void datePickerStart303_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            datePickerStart303.Focus();
            datePickerStart303.Focus();
        }

        private void datePickerend_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            datePickerend.Focus();
            datePickerend.Focus();
        }

        private void enddate207_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            enddate207.Focus();
            enddate207.Focus();
        }

        private void datePickerCredit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            datePickerCredit.Focus();
            datePickerCredit.Focus();
        }

        //private void DiActiveRadio_Click(object sender, RoutedEventArgs e)
        //{
        //    if (ActiveRadio1.IsChecked == true)
        //        credit_Capability_Activation = 1;
        //    else if (ActiveRadio1.IsChecked == false)
        //        credit_Capability_Activation = -1;

        //    else if (ActiveRadio2.IsChecked == true)
        //        disconnectivity_On_Negative_Credit = 1;
        //    else if (ActiveRadio2.IsChecked == false)
        //        disconnectivity_On_Negative_Credit = -1;

        //    else if (ActiveRadio3.IsChecked == true)
        //        disconnectivity_On_Expired_Credit = 1;
        //    else if (ActiveRadio3.IsChecked == false)
        //        disconnectivity_On_Expired_Credit = -1;
        //}

        private void chk10_Checked(object sender, RoutedEventArgs e)
        {

            StartCredit.Visibility = Visibility.Visible;
            datePickerCredited.Visibility = Visibility.Visible;
            datePickerCredit.Visibility = Visibility.Visible;
            _creditCapabilityActivation = 1;
            ActiveGrid1.Visibility = Visibility.Visible;
            _activeCreaditeActivation = true;

        }

        private void chk10_Unchecked(object sender, RoutedEventArgs e)
        {

            StartCredit.Visibility = Visibility.Hidden;
            datePickerCredited.Visibility = Visibility.Hidden;
            datePickerCredit.Visibility = Visibility.Hidden;  
            ActiveGrid1.Visibility = Visibility.Hidden;
            _creditCapabilityActivation = 0;
        }
    }
}