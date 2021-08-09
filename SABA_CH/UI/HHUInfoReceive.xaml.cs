using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Threading;
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
    /// Interaction logic for HHUInfoReceive.xaml
    /// </summary>
    public partial class HHUInfoReceive : System.Windows.Window
    {
        public ShowTranslateofLable tr = null;
         Button selectedButton;
        public ShowButtonAccess us = null;
        public readonly int windowID = 19;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
        public List<ReverseConsumedOBIS> _lstConsumedWater;
        public List<ReverseConsumedOBIS> _lstConsumedEnergy;
        public ICollectionView GroupedCustomers { get; private set; }
        public string ReadDate = "";
        public string HeaderID = "";
        public HHUInfoReceive(List<SelectedMeter> MeterList)
        {
            InitializeComponent();
            ChangeLanguage();
            ChangeFlowDirection();
            if (CommonData.LanguagesID==2)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(1065);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
            //else if (CommonData.LanguagesID==3)
            //{
            //    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-KW");
            //    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            //}
            CommonData.HHUDataReceive = this;
            CommonData.mainwindow.changeProgressBarTag("");
            CommonData.mainwindow.changeProgressBarValue(0);
            RefreshMeterNumberGrid(MeterList);
            RefreshDataGrids();            
            TranslateMeterNumberGrid();

        }
        public void changebtnVisble()
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate()
                {
                    if (txtMeterNumber.Text.StartsWith("207"))
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
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void TranslateMeterNumberGrid()
        {
            try
            {
                MeterNumberGrid.Columns[0].Header = tr.TranslateofLable.Object13;
                MeterNumberGrid.Columns[1].Header = tr.TranslateofLable.Object14;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
       public void  RefreshMeterNumberGrid(List<SelectedMeter> MeterList)
        {
            try 
	        {
                MeterNumberGrid.ItemsSource = MeterList;
	        }
	        catch (Exception ex)
	        {
		
		        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
	        }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            changebtnVisble();
            tabCtrl.SelectedItem = tabPag;
            if (!tabCtrl.IsVisible)
            {

                tabCtrl.Visibility = Visibility.Visible;

            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                ClassControl.OpenWin[windowID] = false;
                tabCtrl.Items.Remove(tabPag);
                if (!tabCtrl.HasItems)
                {

                    tabCtrl.Visibility = Visibility.Hidden;

                }
                CommonData.mainwindow.Focus();
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
                tr = CommonData.translateWindow(windowID);
                LayoutRoot.DataContext = tr.TranslateofLable;                
                this.DataContext = tr.TranslateofLable;
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
        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {

        }
        public void RefreshDataGrids()
        {
            try
            {
                //refreshs();
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

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void refreshs()
        {
            try
            {
                changebtnVisble();
                RefreshDatagridGeneral();
                RefreshDatagridMeterStatus();
                //RefreshDatagridWaterEvent();
                //RefreshDatagridELEvent();
                RefreshDatagridGeneralEvent();
                RefreshDatagridgeneralWater();
                //RefreshDatagridCurveInfo();                
                RefreshDatagridConsumedWaternew();
                RefreshDatagridGeneralInfoElectrical();
                RefreshDatagridPowerConsumptionnew();
                RefreshDataGridConsumedWater207();
                //RefreshDatagridConsumedEnergy();
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
                DatagridGeneral.Columns[0].Header = tr.TranslateofLable.Object1.ToString();
                DatagridGeneral.Columns[1].Header = tr.TranslateofLable.Object2.ToString();
                DatagridGeneral.Columns[2].Header = tr.TranslateofLable.Object3.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        public void TranslateHeaderOfMeterStatusGrid()
        {
            //try
            //{
            //    DatagridMeterStatus.Columns[0].Header = tr.TranslateofLable.Object1.ToString();
            //    DatagridMeterStatus.Columns[1].Header = tr.TranslateofLable.Object2.ToString();
            //    DatagridMeterStatus.Columns[2].Header = tr.TranslateofLable.Object3.ToString();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            //}

        }
        public void TranslateHeaderOfDatagridgeneralWater()
        {
            try
            {
                DatagridgeneralWater.Columns[0].Header = tr.TranslateofLable.Object1.ToString();
                DatagridgeneralWater.Columns[1].Header = tr.TranslateofLable.Object2.ToString();
                DatagridgeneralWater.Columns[2].Header = tr.TranslateofLable.Object3.ToString();

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
                DatagridConsumedWater.Columns[0].Header = tr.TranslateofLable.Object18.ToString();
                DatagridConsumedWater.Columns[1].Header = tr.TranslateofLable.Object25.ToString();
                DatagridConsumedWater.Columns[2].Header = tr.TranslateofLable.Object26.ToString();
                //DatagridConsumedWater.Columns[3].Header = tr.TranslateofLable.Object6.ToString();
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
                DatagridGeneralInfoElectrical.Columns[0].Header = tr.TranslateofLable.Object1.ToString();
                DatagridGeneralInfoElectrical.Columns[1].Header = tr.TranslateofLable.Object2.ToString();
                DatagridGeneralInfoElectrical.Columns[2].Header = tr.TranslateofLable.Object3.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }

        public void TranslateHeaderOfDatagridPowerConsumption()
        {
            try
            {
                DatagridPowerConsumption.Columns[0].Header = tr.TranslateofLable.Object18.ToString();
                DatagridPowerConsumption.Columns[1].Header = tr.TranslateofLable.Object19.ToString();
                DatagridPowerConsumption.Columns[2].Header = tr.TranslateofLable.Object20.ToString();
                DatagridPowerConsumption.Columns[3].Header = tr.TranslateofLable.Object21.ToString();
                DatagridPowerConsumption.Columns[4].Header = tr.TranslateofLable.Object22.ToString();
                DatagridPowerConsumption.Columns[5].Header = tr.TranslateofLable.Object23.ToString();
                DatagridPowerConsumption.Columns[6].Header = tr.TranslateofLable.Object24.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
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
                    string Filter = "  and (charindex('\"1\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.MeterID=" + CommonData.MeterID + " and Header.ReadDate= '" + ReadDate + "'";
                    ShowObisValueDetail Generalvalue = new ShowObisValueDetail(Filter, CommonData.LanguagesID);
                    if (Generalvalue._lstShowOBISValueDetail.Count>0)
                        DatagridGeneral.ItemsSource = Generalvalue._lstShowOBISValueDetail;
                    TranslateHeaderOfGeneralGrid();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshDatagridMeterStatus()
        {
            //try
            //{
            //    DatagridMeterStatus.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
            //    new Action(
            //    delegate()
            //    {
            //        string Filter = "  and (charindex(obiss.type,'\"2\"')>0 or charindex(obiss.type,'\"100\"')>0) and Header.MeterID=" + CommonData.MeterID + " and Header.TransferDate= '" + ReadDate + "'";
            //        ShowOBISValueDetail EGeneralvalue = new ShowOBISValueDetail(Filter, CommonData.LanguagesID);
            //        DatagridMeterStatus.ItemsSource = null;
            //        DatagridMeterStatus.ItemsSource = EGeneralvalue.CollectShowOBISValueDetail;
            //    }
            //    ));
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            //}
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
        public void RefreshDatagridgeneralWater()
        {
            try
            {
                DatagridgeneralWater.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate()
                {
                    string Filter = "  and (charindex('\"6\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.MeterID=" + CommonData.MeterID + " and Header.ReadDate= '" + ReadDate + "'";
                    ShowObisValueDetail value = new ShowObisValueDetail(Filter, CommonData.LanguagesID);
                    DatagridgeneralWater.ItemsSource = null;
                    if (value._lstShowOBISValueDetail.Count>0)
                    {
                        DatagridgeneralWater.ItemsSource = value._lstShowOBISValueDetail;
                    }
                    
                }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
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



        #region newrefreshforconsumedActiveenergy
        public void RefreshDatagridConsumedEnergy()
        {
            try
            {
                _lstConsumedEnergy = new List<ReverseConsumedOBIS>();
                List<ReverseConsumedOBIS> revercecomumedobis = new List<ReverseConsumedOBIS>();
                string Filter = " and Main.MeterID=" + CommonData.MeterID + " and DateOfReceivedFromSource='" + ReadDate.Substring(0, 10) + "'";
                DataTable dt = new DataTable();
                dt = GetDataTableForReportConsumedEnergy(DatagridPowerConsumption, Filter);
                //DatagridConsumedWater.ItemsSource = dt.DefaultView;
                ExecuteReport(dt, CommonData.MeterNumber, revercecomumedobis,2);

                GroupedCustomers = new ListCollectionView(revercecomumedobis);
                GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("MeterNumber"));
                DatagridPowerConsumption.ItemsSource = GroupedCustomers;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public DataTable GetDataTableForReportConsumedEnergy(DataGrid dtg, string Filter)
        {
            DataTable dt = new DataTable();
            try
            {
                ShowConsumedActiveEnergy_Result row = null;
                ShowConsumedActiveEnergy_Result re = new ShowConsumedActiveEnergy_Result();
                Type t = re.GetType();
                MemberInfo[] members = t.GetMembers(BindingFlags.NonPublic |
                BindingFlags.Instance);
                foreach (MemberInfo member in members)
                {
                    DataColumn dc = new DataColumn();
                    int start = member.Name.ToString().IndexOf('<');
                    int end = member.Name.ToString().IndexOf('>');
                    int len = member.Name.ToString().Length;
                    if (start >= 0)
                    {
                        dc.ColumnName = (member.Name.ToString().Substring(start + 1, end - start - 1));
                        dt.Columns.Add(dc);
                    }

                }
                //  add each of the data rows to the table
                SabaNewEntities Bank = new SabaNewEntities();

                Bank.Database.Connection.Open();
                foreach (var item in Bank.ShowConsumedActiveEnergy(Filter, CommonData.LanguagesID,CommonData.UserID))
                {
                    DataRow dr;
                    dr = dt.NewRow();
                    row = item;
                    dr["ConsumedWater"] = row.ConsumedWater.ToString();
                    dr["OBISDesc"] = row.OBISDesc.ToString();
                    dr["ConsumedDate"] = row.ConsumedDate.ToString();
                    dr["MeterID"] = row.MeterID.ToString();
                    dr["MeterNumber"] = row.MeterNumber.ToString();
                    dt.Rows.Add(dr);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
            return dt;
        }
        #endregion newrefreshforconsumedActiveenergy


        #region newrefreshforconsumedwater


        public void RefreshDatagridConsumedWaternew()
        {
            try
            {
                
                string Filter = " and Main.MeterID=" + CommonData.MeterID.ToString();
                ShowConsumedWaterPivot value = new ShowConsumedWaterPivot(Filter,"");
                DatagridConsumedWater.ItemsSource = null;
                if (value._lstShowConsumedWaterPivot.Count>0)
                 DatagridConsumedWater.ItemsSource = value._lstShowConsumedWaterPivot;

            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        public void RefreshDatagridConsumedWater()
        {
            try
            {
               
                string Filter = "   and Main.MeterID=" + CommonData.MeterID + " and Main.DateOfReceivedFromSource ='" + ReadDate + "'";
                ShowConsumedWater value = new ShowConsumedWater(Filter);
                DatagridConsumedWater.ItemsSource = null;
                DatagridConsumedWater.ItemsSource = value._lstShowConsumedWaters;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public DataTable GetDataTableForReport(DataGrid dtg, string Filter)
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
                    int start = member.Name.ToString().IndexOf('<');
                    int end = member.Name.ToString().IndexOf('>');
                    int len = member.Name.ToString().Length;
                    if (start >= 0)
                    {
                        dc.ColumnName = (member.Name.ToString().Substring(start + 1, end - start - 1));
                        dt.Columns.Add(dc);
                    }

                }
                //  add each of the data rows to the table
                SabaNewEntities Bank = new SabaNewEntities();

                Bank.Database.Connection.Open();
                foreach (var item in Bank.ShowConsumedWater(Filter, CommonData.LanguagesID, CommonData.UserID))
                {
                    DataRow dr;
                    dr = dt.NewRow();
                    row = item;
                    dr["ConsumedWater"] = row.ConsumedWater.ToString();
                    dr["OBISDesc"] = row.OBISDesc.ToString();
                    dr["ConsumedDate"] = row.ConsumedDate.ToString();
                    dr["MeterID"] = row.MeterID.ToString();
                    dr["MeterNumber"] = row.MeterNumber.ToString();
                    dt.Rows.Add(dr);
                }
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
            return dt;
        }
        public void ExecuteReport(DataTable dt, string MeterNumber, List<ReverseConsumedOBIS> revercecomumedobis,int type)
        {
            try
            {
                List<ReverseConsumedOBIS> newlist = new List<ReverseConsumedOBIS>();
                string x = "OBISDesc";// "";
                string y = "ConsumedDate";
                string z = "ConsumedWater";
                DataTable newDt = new DataTable();
                newDt = PivotTable.GetInversedDataTable(dt, x, y, z, "-", false);

                newlist = DataTable2List(newDt, MeterNumber, type);
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

        public List<ReverseConsumedOBIS> DataTable2List(DataTable dt, string MeterNumber,int type)
        {

            try
            {

                DataColumn dc = dt.Columns[0];
                if (type == 1)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ReverseConsumedOBIS newrow = new ReverseConsumedOBIS();
                        newrow.RowName = item[0].ToString();
                        newrow.Column0Value = item[1].ToString();
                        newrow.Column1Value = item[2].ToString();
                        newrow.Column0Name = dt.Columns[1].ColumnName.ToString();
                        newrow.Column1Name = dt.Columns[2].ColumnName.ToString();
                        newrow.MeterNumber = MeterNumber;
                        _lstConsumedWater.Add(newrow);
                    }
                    return _lstConsumedWater;
                }
                else 
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        ReverseConsumedOBIS newrow = new ReverseConsumedOBIS();
                        newrow.RowName = item[0].ToString();
                        newrow.Column0Value = item[1].ToString();
                        newrow.Column1Value = item[2].ToString();
                        newrow.Column0Name = dt.Columns[1].ColumnName.ToString();
                        newrow.Column1Name = dt.Columns[2].ColumnName.ToString();
                        newrow.MeterNumber = MeterNumber;
                        _lstConsumedEnergy.Add(newrow);
                    }
                    return _lstConsumedEnergy;
                }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                return null;
            }
            
        }
        #endregion newrefreshforconsumedwater




        public void RefreshDatagridGeneralInfoElectrical()
        {
            try
            {
                DatagridConsumedWater.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
               new Action(
               delegate()
               {
                   string Filter = "  and (charindex('\"9\"',obiss.type)>0    or charindex('\"100\"',obiss.type)>0) and Header.MeterID=" + CommonData.MeterID + " and Header.ReadDate= '" + ReadDate + "'";
                   ShowObisValueDetail value = new ShowObisValueDetail(Filter, CommonData.LanguagesID);
                   DatagridGeneralInfoElectrical.ItemsSource = null;
                   if (value._lstShowOBISValueDetail.Count>0)
                       DatagridGeneralInfoElectrical.ItemsSource = value._lstShowOBISValueDetail;
               }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);   
                CommonData.WriteLOG(ex);
            }
        }
        public void RefreshDatagridPowerConsumption()
        {
            try
            {
                DatagridConsumedWater.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate()
                {
                    string Filter = "   and Main.MeterID=" + CommonData.MeterID + " and Main.DateOfReceivedFromSource ='" + ReadDate + "'";
                    ShowConsumedActiveEnergy value = new ShowConsumedActiveEnergy(Filter);
                    DatagridPowerConsumption.ItemsSource = null;
                    DatagridPowerConsumption.ItemsSource = value._lstShowConsumedActiveEnergys;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }
        public void RefreshDatagridPowerConsumptionnew()
        {
            try
            {
                DatagridPowerConsumption.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate()
                {
                    string Filter = " and Main.MeterID=" + CommonData.MeterID.ToString();
                    ShowConsumedactiveenergypivot value = new ShowConsumedactiveenergypivot(Filter);
                    DatagridPowerConsumption.ItemsSource = null;
                    if (value.Lstpower.ListOfTariff.Count>0)
                     DatagridPowerConsumption.ItemsSource = value.Lstpower.ListOfTariff;

                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); 
                CommonData.WriteLOG(ex);
            }
        }
        public void RefreshDataGridConsumedWater207()
        {
            try
            {

                //ShowConsumedWater207 value = new ShowConsumedWater207(CommonData.SelectedMeterID);
                //DatagridConsumedWater207.ItemsSource = null;
                //GroupedCustomers = new ListCollectionView(value._lstShowConsumedWater207s);
                //GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("ReadDate"));
                //if (value._lstShowConsumedWater207s.Count>0)
                // DatagridConsumedWater207.ItemsSource = GroupedCustomers;

            }
            catch (Exception)
            {

                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void MeterNumberGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MeterSelectedChange();
        }
        public void MeterSelectedChange()
        {
            try
            {
                SelectedMeter selectmeter = new SelectedMeter();
                if (MeterNumberGrid.SelectedItem != null)
                {
                    selectmeter = (SelectedMeter)MeterNumberGrid.SelectedItem;
                    CommonData.MeterID = selectmeter.MeterId;
                    CommonData.SelectedMeterID = selectmeter.MeterId;
                    CommonData.SelectedMeterNumber = selectmeter.MeterNumber.ToString();
                    ReadDate = selectmeter.ReadDate;
                    //HeaderID=selectmeter.
                    txtMeterNumber.Text = selectmeter.MeterNumber;
                    txtReadDate.Text = selectmeter.ReadDate;
                    if (CommonData.SelectedMeterNumber.StartsWith("207"))
                    {
                        Meter207Status();
                        changelstStatusposition(true);
                    }
                    else
                    {
                        changelstStatusposition(false);
                    }
                    refreshs();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
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
        private void MeterNumberGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MeterSelectedChange();
        }

        private void btnS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _303 Obj303 = new _303();
                _207 Obj207 = new _207();
                List<Status_Result> List = new List<Status_Result>();


                if (selectedButton != null)
                    selectedButton.Background = Brushes.SkyBlue;
                string value = "";
                Button btn = sender as Button;
                selectedButton = btn;
                btn.Background = Brushes.LimeGreen;
                string btnName = btn.Name.Replace("btnS_", "");
                switch (btnName)
                {
                    case "1":
                        try
                        {
                            string Value = "", Value1 = "", Value2 = "", Value3 = "";
                            Value1 = RefreshDatagridMeterStatus("0000603F00FF");
                            Value2 = RefreshDatagridMeterStatus("0000603F01FF");
                            Value3 = RefreshDatagridMeterStatus("0000603F02FF");
                            List = Obj303.PerformanceMeteroncreditevents(Value1.PadLeft(8, '0'), Value2.PadLeft(8, '0'), Value3.PadLeft(8, '0'), CommonData.LanguageName);
                            //Value = RefreshDatagrid_303("0000600A02FF");
                            //List = Obj303.StatuseGeneralRegister2(Value.PadLeft(8, '0'), CommonData.LanguageName, 2);

                        }
                        catch { }
                        break;

                    case "2":
                        try
                        {
                            string Value = "";
                            //Value = RefreshDatagrid_303("0000600302FF");
                            //List = Obj303.statusRegister("0000600302FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            Value = RefreshDatagridMeterStatus("0000600A03FF");
                            if (Value!="")
                                List = Obj303.StatuseGeneralRegister3(Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }

                        }
                        catch { }
                        break;

                    case "3":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600A01FF");
                            if(Value!="")
                                List = Obj303.statusRegister("0000600A01FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "4":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600A02FF");
                            if(Value!="")
                             List = Obj303.statusRegister("0000600A02FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "5":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600A03FF");
                            if(Value!="")
                             List = Obj303.statusRegister("0000600A03FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "6":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600A04FF");
                            if(Value!="")
                                List = Obj303.statusRegister("0000600A04FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "7":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600A05FF");
                            if(Value!="")
                                List = Obj303.statusRegister("0000600A05FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "8":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600A01FF");
                            if(Value!="")
                             List = Obj303.statusRegister("0000600A01FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "9": try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600A01FF");
                        if(Value!="")
                            List = Obj303.statusRegister("0000600A01FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        else
                        {
                            Status s = new Status();

                            Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                            List.Add(sr);

                        }
                        }
                        catch { }
                        break;

                    case "10":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600A01FF");
                            List = Obj303.statusRegister("0000600A01FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        }
                        catch { }
                        break;

                    case "11":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000616100FF");
                            if(Value!="")
                                List = Obj303.statusRegister("0000616100FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "12": try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000616102FF");
                            if(Value!="")
                                List = Obj303.statusRegister("0000616102FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "13":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0100600A03FF");
                            if(Value!="")
                                List = Obj303.statusRegister("0100600A03FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "14":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0100600501FF");
                            if(Value!="")
                                List = Obj303.statusRegister("0100600501FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "15":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0100600A00FF");
                            if(Value!="")
                                List = Obj303.statusRegister("0100600A00FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }
                        }
                        catch { }
                        break;

                    case "16":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0100600A01FF");
                            if(Value!="")
                             List = Obj303.statusRegister("0100600A01FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
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
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0100616101FF");
                            if(Value!="")
                                List = Obj303.statusRegister("0100616101FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
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
                            //Value = RefreshDatagrid_303("0802606101FF");
                            //List = Obj303.statusRegister("0802606101FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                            Value = RefreshDatagridMeterStatus("0000600A02FF");
                            if(Value!="")
                                List = Obj303.StatuseGeneralRegister2(Value.PadLeft(8, '0'), CommonData.LanguageName, 3);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }

                        }
                        catch { }
                        break;

                    case "19":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000600303FF");
                            if(Value!="")
                             List = Obj303.statusRegister("0000600303FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
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
                            if(Value!="")
                             List = Obj303.StatuseGeneralRegister2(Value.PadLeft(8, '0'), CommonData.LanguageName, 1);
                            else
                            {
                                Status s = new Status();

                                Status_Result sr = new Status_Result(s, CommonData.mainwindow.tm.TranslateofMessage.Message80);
                                List.Add(sr);

                            }

                        }
                        catch { }
                        break;

                    case "21":
                        try
                        {
                            string Value = "";
                            Value = RefreshDatagridMeterStatus("0000603E04FF");
                            if(Value!="")
                            {
                                List = Obj303.statusRegister("0000603E04FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                                foreach (var item in List)
                                {
                                    item.IsStatuseTrue = Status.dontCare;
                                }

                            }
                            //List = Obj303.statusRegister("0000603E04FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
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
                            string Value1 = "", Value2 = "";
                            Value1 = RefreshDatagridMeterStatus("0802606101FF");
                            Value2 = RefreshDatagridMeterStatus("0000603E01FF");
                            Value1 = Value1.Trim();
                            List = Obj207.StatusRegister207_0802606101FF(Value2,Value1, CommonData.LanguageName);
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
        public string RefreshDatagridMeterStatus(string OBIS)
        {
            try
            {
                string Value = "";
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                string Filter = " and Header.MeterID=" + CommonData.MeterID + " and ReadDate='" + ReadDate + "'  and OBISs.OBIS='" + OBIS + "'";
                ShowObisValueDetail EGeneralvalue = new ShowObisValueDetail(Filter, CommonData.LanguagesID);
                foreach (ShowOBISValueDetail_Result item in Bank.ShowOBISValueDetail(Filter, CommonData.LanguagesID, CommonData.UserID))
                    Value = item.Value;
                Bank.Database.Connection.Close();
                return Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                return "";
            }
        }
        public void changelist(List<Status_Result> List)
        {
            try
            {
                lstStatus.Items.Clear();
                foreach (var item in List)
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
                        Label Lb1 = new Label();
                        Lb1.Content = item.Description;
                        lstStatus.Items.Add(Lb1);

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
                dlg.FileName = selectedtab + "(" + txtMeterNumber.Text + ")"; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = " (.xlsx)|*.xlsx"; // Filter files by extension 
                Nullable<bool> result = dlg.ShowDialog();
                string FilePath = dlg.FileName;
                if (result.Value)
                {
                    if (tabControlMain.SelectedItem == tabitemPowerConsumption)
                    {
                        List<ShowConsumedatariffctiveenergypivot_Result> sublist = new List<ShowConsumedatariffctiveenergypivot_Result>();
                        object[] header = { "ConsumedDate", "ACtiveEnergy", "ReACtiveEnergy", "CReactiveEnergy", "NumberofNominalDemandViolation", "MaxDemand", "RealDemand", "lstTariff" };
                        object[] headerText = { "ConsumedDate", "ACtiveEnergy", "ReACtiveEnergy", "CReactiveEnergy", "NumberofNominalDemandViolation", "MaxDemand", "RealDemand" };
                        object[] SubheaderText = { "Tariff1", "Tariff2", "Tariff3", "Tariff4", "Tariff5", "Tariff6", "Tariff7", "Tariff8", "Tariff9", "Tariff10", "Tariff11", "Tariff12", "Tariff13", "Tariff14", "Tariff15", "Tariff16", "Tariff17", "Tariff18", "Tariff19", "Tariff20", "Tariff21", "Tariff22", "Tariff23", "Tariff24", "Tariff25", "Tariff26" };
                        object[] Subheader = { "description", "t1", "t2", "t3", "t4", "t5", "t6", "t7", "t8", "t9", "t10", "t11", "t12", "t13", "t14", "t15", "t16", "t17", "t18", "t19", "t20", "t21", "t22", "t23", "t24", "t25", "t26" };
                        ExcelSerializernew<Consumedpower, ShowConsumedatariffctiveenergypivot_Result, List<Consumedpower>> s = new ExcelSerializernew<Consumedpower, ShowConsumedatariffctiveenergypivot_Result, List<Consumedpower>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridPowerConsumption.ItemsSource);
                        s.header = header;
                        s.SubheaderText = SubheaderText;
                        s.headerText = headerText;
                        s.Subheader = Subheader;
                        s.CreateWorkSheet(FilePath, "Power Consumption", txtMeterNumber.Text, (List<Consumedpower>)view.SourceCollection, txtReadDate.Text, true,"HHU");
                    }
                    else if (tabControlMain.SelectedItem == tabitemConsumedWater)
                    {
                        object[] header = { "ConsumedDate", "w", "WT"};
                        object[] headerText = { "ConsumedDate", "Consumed Water", "Total Consumed Water"};
                        ExcelSerializernew<ShowConsumedWaterPivot_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowConsumedWaterPivot_Result>> s = new ExcelSerializernew<ShowConsumedWaterPivot_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowConsumedWaterPivot_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridConsumedWater.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(FilePath, "Water Consumption", txtMeterNumber.Text, (List<ShowConsumedWaterPivot_Result>)view.SourceCollection, txtReadDate.Text, false, "HHU");
                    }
                    else if (tabControlMain.SelectedItem == tabitemgeneralWater)
                    {
                        object[] header = { "ObisDesc", "Value", "OBISUnitDesc" };
                        object[] headerText = { "Title", "Value", "Unit" };
                        ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>> s = new ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridgeneralWater.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(FilePath, "General Information water", txtMeterNumber.Text, (List<ShowOBISValueDetail_Result>)view.SourceCollection, txtReadDate.Text, false, "HHU");
                    }
                    else if (tabControlMain.SelectedItem == tabitemGeneralInfoElectrical)
                    {
                        object[] header = { "ObisDesc", "Value", "OBISUnitDesc" };
                        object[] headerText = { "Title", "Value", "Unit" };
                        ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>> s = new ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridGeneralInfoElectrical.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(FilePath, "General information Power", txtMeterNumber.Text, (List<ShowOBISValueDetail_Result>)view.SourceCollection, txtReadDate.Text, false, "HHU");
                    }
                    else if (tabControlMain.SelectedItem == tabitemGeneral)
                    {
                        object[] header = { "ObisDesc", "Value", "OBISUnitDesc" };
                        object[] headerText = { "Title", "Value", "Unit" };
                        ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>> s = new ExcelSerializernew<ShowOBISValueDetail_Result, ShowConsumedatariffctiveenergypivot_Result, List<ShowOBISValueDetail_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(DatagridGeneral.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(FilePath, "Meters General Information", txtMeterNumber.Text, (List<ShowOBISValueDetail_Result>)view.SourceCollection, txtReadDate.Text, false, "HHU");
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
            Button btn = sender as Button;
            btn.Background = Brushes.Orange;
            if (selectedButton != null)
                selectedButton.Background = Brushes.LimeGreen;
        }

        private void BtnStatus_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                btn.Background = Brushes.SkyBlue;
                if (selectedButton != null)
                    selectedButton.Background = Brushes.LimeGreen;
            }
            catch { }
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
            }
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

        private void MeterNumberGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcount.Content =rowcount+""+ MeterNumberGrid.Items.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        
    }
}
