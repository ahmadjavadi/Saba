using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for PowerManagmentReport.xaml
    /// </summary>
    public partial class PowerManagmentReport : System.Windows.Window
    {
        public ShowTranslateofLable Tr = null;
        public string Startdate = "";
        public string Enddate = "";
        public ShowButtonAccess Us = null;
        public readonly int WindowId = 24;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public ICollectionView GroupedCustomers { get; private set; }
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
        public PowerManagmentReport()
        {
            InitializeComponent();
            Tr = CommonData.translateWindow(WindowId);
            Translate();
            ChangeFlowDirection();
            if (CommonData.LanguagesID == 2)
            {
                datePickerEnd.Visibility = Visibility.Visible;
                datePickerStart.Visibility = Visibility.Visible;
                datePickerEnden.Visibility = Visibility.Hidden;
                datePickerStarten.Visibility = Visibility.Hidden;
                Thread.CurrentThread.CurrentCulture = new CultureInfo(1065);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                int year = datePickerStart.SelectedDate.Year - 1;
                string start = datePickerStart.Text;
                start = start.Replace(datePickerStart.SelectedDate.Year.ToString(), year.ToString());
                datePickerStart.Text = start;
            }
            else
            {
                datePickerEnden.Visibility = Visibility.Visible;
                datePickerStarten.Visibility = Visibility.Visible;
                datePickerEnd.Visibility = Visibility.Hidden;
                datePickerStart.Visibility = Visibility.Hidden;
                datePickerStarten.SelectedDate = DateTime.Now;
                datePickerEnden.SelectedDate = DateTime.Now;
            }

            SetDate();
            RefreshDatagrid();
            Setchart();
            //RefreshDatagridConsumedWater();
            
        }
        public void Translate()
        {
            try
            {
                this.DataContext = Tr.TranslateofLable;
                ReportTabItem.Header = Tr.TranslateofLable.Object1;
                ChartTabItem.Header = Tr.TranslateofLable.Object2;
                TranslateHeaderOfDatagridPowerConsumption();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void SetDate()
        {
            try
            {
                Startdate = datePickerStart.Text;
                Enddate = datePickerEnd.Text;
                int day = Convert.ToInt32(Startdate.Substring(Startdate.LastIndexOf('/') + 1, Startdate.Length - Startdate.LastIndexOf('/') - 1));
                int month = Convert.ToInt32(Startdate.Substring(Startdate.IndexOf('/') + 1, Startdate.LastIndexOf('/') - Startdate.IndexOf('/') - 1));
                if (day < 10)

                    Startdate = Startdate.Substring(0, Startdate.Length - 1) + "0" + day;

                if (month < 10)
                    Startdate = Startdate.Substring(0, Startdate.IndexOf('/') + 1) + "0" + Startdate.Substring(Startdate.IndexOf('/') + 1, Startdate.Length - Startdate.IndexOf('/') - 1);
                day = Convert.ToInt32(Enddate.Substring(Enddate.LastIndexOf('/') + 1, Enddate.Length - Enddate.LastIndexOf('/') - 1));
                month = Convert.ToInt32(Enddate.Substring(Enddate.IndexOf('/') + 1, Enddate.LastIndexOf('/') - Enddate.IndexOf('/') - 1));
                if (day < 10)

                    Enddate = Enddate.Substring(0, Enddate.Length - 1) + "0" + day;

                if (month < 10)
                    Enddate = Enddate.Substring(0, Enddate.IndexOf('/') + 1) + "0" + Enddate.Substring(Enddate.IndexOf('/') + 1, Enddate.Length - Enddate.IndexOf('/') - 1);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void Setchart()
        {
            try
            {
                string filter= "";
                if(CommonData.selectedGroup.GroupID!=-1)
                    filter = " and MTG.groupid=" + CommonData.selectedGroup.GroupID + " and mtg.grouptype= " + CommonData.selectedGroup.GroupType;
                if (CommonData.LanguagesID == 2)
                    filter = filter + "  and Main.ConsumedDate Between '" + Startdate + "' and '" + Enddate  + "'";
                else
                    filter = filter + "  and Main.ConsumedDate Between '" + datePickerStarten.Text + "' and '" + datePickerEnden.Text.ToString() + "'";

                ShowManagmentConsumedactiveenergypivot value = new ShowManagmentConsumedactiveenergypivot(filter);
                //chart1.DataSource = value._lstShowManagmentConsumedactiveenergypivot;
                GridSumConsum.ItemsSource = value._lstShowManagmentConsumedactiveenergypivot;
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
                GridMain.Columns[0].Header = Tr.TranslateofLable.Object65;
                GridMain.Columns[1].Header = Tr.TranslateofLable.Object46;
                GridMain.Columns[2].Header = Tr.TranslateofLable.Object47;
                GridMain.Columns[3].Header = Tr.TranslateofLable.Object48;
                GridMain.Columns[4].Header = Tr.TranslateofLable.Object49;
                GridMain.Columns[5].Header = Tr.TranslateofLable.Object50;
                GridMain.Columns[6].Header = Tr.TranslateofLable.Object51;
                GridMain.Columns[7].Header = Tr.TranslateofLable.Object52;

                GridSumConsum.Columns[0].Header = Tr.TranslateofLable.Object46;
                GridSumConsum.Columns[1].Header = Tr.TranslateofLable.Object47;
                GridSumConsum.Columns[2].Header = Tr.TranslateofLable.Object48;
                GridSumConsum.Columns[3].Header = Tr.TranslateofLable.Object49;
                GridSumConsum.Columns[4].Header = Tr.TranslateofLable.Object50;
                GridSumConsum.Columns[5].Header = Tr.TranslateofLable.Object51;
                GridSumConsum.Columns[6].Header = Tr.TranslateofLable.Object52;
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
                //MainGrid.FlowDirection = CommonData.FlowDirection;
                //GridMain.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
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
        public void RefreshDatagrid()
        {
            try
            {
                string filter = "";
                if (CommonData.selectedGroup.GroupID != -1)
                    filter = " and MTG.groupid=" + CommonData.selectedGroup.GroupID + " and mtg.grouptype= " + CommonData.selectedGroup.GroupType;

                if (CommonData.LanguagesID == 2)
                    filter = filter+"  and Main.ConsumedDate Between '" + Startdate + "' and '" +Enddate + "'";
                else
                    filter =filter+ "  and Main.ConsumedDate Between '" + datePickerStarten.Text + "' and '" + datePickerEnden.Text + "'";
                ShowConsumedactiveenergypivotManage value = new ShowConsumedactiveenergypivotManage(filter);
                GroupedCustomers = new ListCollectionView(value.Lstpower.ListOfTariff);
                GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("MeterNumber"));
                GridMain.ItemsSource =  value.Lstpower.ListOfTariff;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        
        private void datePickerStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDate();
            RefreshDatagrid();
            Setchart();
        }

        private void ToolStripButtonExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                //dlg.FileName = selectedtab + "(" + txtMeterNumber.Text + ")"; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = " (.xlsx)|*.xlsx"; // Filter files by extension 
                Nullable<bool> result = dlg.ShowDialog();
                string filePath = dlg.FileName;
                if (result.Value)
                {
                    if (TabControlMain.SelectedItem == TabItemSum)
                    {
                        
                        object[] header = { "ConsumedDate", "ActiveEnergy", "ReActiveEnergy", "CREActiveEnergy", "NumberofNominalDemandViolation", "MaxDemand", "RealDemand" };
                        object[] headerText = { "ConsumedDate", "ActiveEnergy", "ReActiveEnergy", "CREActiveEnergy", "NumberofNominalDemandViolation", "MaxDemand", "RealDemand" };

                        ExcelSerializernew<ShowManagmentConsumedactiveenergypivot_Result, ShowManagmentConsumedactiveenergypivot_Result, List<ShowManagmentConsumedactiveenergypivot_Result>> s = new ExcelSerializernew<ShowManagmentConsumedactiveenergypivot_Result, ShowManagmentConsumedactiveenergypivot_Result, List<ShowManagmentConsumedactiveenergypivot_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(GridSumConsum.ItemsSource);
                        s.header = header;                        
                        s.headerText = headerText;
                        s.CreateWorkSheet(filePath, "Power Consumption", "", (List<ShowManagmentConsumedactiveenergypivot_Result>)view.SourceCollection, "", false,"");
                    }
                    else if (TabControlMain.SelectedItem == ReportTabItem)
                    {
                        object[] header = { "MeterNumber", "ConsumedDate", "ACtiveEnergy", "ReACtiveEnergy", "CReactiveEnergy", "NumberofNominalDemandViolation", "MaxDemand", "RealDemand" };
                        object[] headerText = { "MeterNumber", "ConsumedDate", "ActiveEnergy", "ReActiveEnergy", "CREActiveEnergy", "NumberofNominalDemandViolation", "MaxDemand", "RealDemand" };

                        ExcelSerializernew<Consumedpower, Consumedpower, List<Consumedpower>> s = new ExcelSerializernew<Consumedpower, Consumedpower, List<Consumedpower>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(GridMain.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(filePath, "Power Consumption", "", (List<Consumedpower>)view.SourceCollection, "",false,"");
                    }
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            SetDate();
            RefreshDatagrid();
            Setchart();
        }

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
                diagram1.Width = chart1.Width;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

    }
}
