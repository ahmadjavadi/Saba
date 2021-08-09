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
    /// Interaction logic for WaterManagementReport.xaml
    /// </summary>
    public partial class WaterManagementReport : System.Windows.Window
    {
        public ShowTranslateofLable tr = null;
        string startdate ="";
        string enddate = ""; 
        public ShowButtonAccess us = null;
        public readonly int windowID = 18;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
        public ICollectionView GroupedCustomers { get; private set; }
        
        public WaterManagementReport()
        {
            InitializeComponent();
            tr = CommonData.translateWindow(windowID);
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
                 
                int year= datePickerStart.SelectedDate.Year-1;
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
            setchart();
        }
        public void SetDate()
        {
            try
            {
                startdate = datePickerStart.Text;
                enddate = datePickerEnd.Text;
                int day = Convert.ToInt32(startdate.Substring(startdate.LastIndexOf('/')+1, startdate.Length - startdate.LastIndexOf('/')-1));
                int Month = Convert.ToInt32(startdate.Substring(startdate.IndexOf('/')+1, startdate.LastIndexOf('/') - startdate.IndexOf('/')-1));
                if (day < 10)

                    startdate = startdate.Substring(0, startdate.Length - 1) + "0" + day;
                
                if (Month < 10)
                    startdate = startdate.Substring(0, startdate.IndexOf('/')+1) + "0" + startdate.Substring(startdate.IndexOf('/') + 1, startdate.Length - startdate.IndexOf('/') - 1);
                day = Convert.ToInt32(enddate.Substring(enddate.LastIndexOf('/') + 1, enddate.Length - enddate.LastIndexOf('/') - 1));
                Month = Convert.ToInt32(enddate.Substring(enddate.IndexOf('/') + 1, enddate.LastIndexOf('/') - enddate.IndexOf('/') - 1));
                if (day < 10)

                    enddate = enddate.Substring(0, enddate.Length - 1) + "0" + day;

                if (Month < 10)
                    enddate = enddate.Substring(0, enddate.IndexOf('/') + 1) + "0" + enddate.Substring(enddate.IndexOf('/') + 1, enddate.Length - enddate.IndexOf('/') - 1);

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void setchart()
        {
            try
            {
                string Filter = "";
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);

                
                //if (CommonData.selectedGroup.GroupID != -1)
                //    Filter = " and MTG.groupid=" + CommonData.selectedGroup.GroupID + " and mtg.grouptype= " + CommonData.selectedGroup.GroupType;

                if (CommonData.LanguagesID == 2)
                    Filter = Filter + "  and ConsumedDate Between '" + startdate + "' and '" + enddate + "'";
                else
                    Filter = Filter + "  and ConsumedDate Between '" + datePickerStarten.Text + "' and '" + datePickerEnd.Text + "'";
                ShowManagmentConsumedWaterPivot value = new ShowManagmentConsumedWaterPivot("",Filter);
                //chart1.DataSource = value._lstShowManagmentConsumedWaterPivot;
                GridSumGroup.ItemsSource = value._lstShowManagmentConsumedWaterPivot;
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag("");

            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag("");
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void Translate()
        {
            try
            {
                this.DataContext = tr.TranslateofLable;
                ReportTabItem.Header = tr.TranslateofLable.Object1;
                TabItemSum.Header = tr.TranslateofLable.Object13;
                ChartTabItem.Header = tr.TranslateofLable.Object2;
                GridMain.Columns[0].Header = tr.TranslateofLable.Object9;
                GridMain.Columns[1].Header = tr.TranslateofLable.Object10;
                GridMain.Columns[2].Header = tr.TranslateofLable.Object11;
                //GridMain.Columns[3].Header = tr.TranslateofLable.Object12;

                GridSumGroup.Columns[0].Header = tr.TranslateofLable.Object9;
                GridSumGroup.Columns[1].Header = tr.TranslateofLable.Object10;
                GridSumGroup.Columns[2].Header = tr.TranslateofLable.Object11;
                //GridSumGroup.Columns[3].Header = tr.TranslateofLable.Object12;

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

                tabCtrl.SelectedItem = tabPag;
                if (!tabCtrl.IsVisible)
                {

                    tabCtrl.Visibility = Visibility.Visible;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        public void RefreshDatagrid()
        {
            try
            {
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);

                string Filter = "";
                //if (CommonData.selectedGroup.GroupID != -1)
                //    Filter = " and MTG.groupid=" + CommonData.selectedGroup.GroupID + " and mtg.grouptype= " + CommonData.selectedGroup.GroupType;

                if (CommonData.LanguagesID == 2)
                    Filter = Filter + "  and ConsumedDate Between '" + startdate + "' and '" + enddate + "'";
                else
                    Filter = Filter + "  and ConsumedDate Between '" + datePickerStarten.Text + "' and '" + datePickerEnd.Text + "'";
               
                ShowConsumedWaterPivot value = new ShowConsumedWaterPivot("",Filter);
                GroupedCustomers = new ListCollectionView(value._lstShowConsumedWaterPivot);
                GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("MeterNumber"));
                GridMain.ItemsSource = GroupedCustomers;
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag("");


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag("");
            }
        }


        private void datePickerStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDate();
            RefreshDatagrid();
            setchart();
        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            SetDate();
            RefreshDatagrid();
            setchart();
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
                string FilePath = dlg.FileName;
                if (result.Value)
                {
                    if (TabControlMain.SelectedItem == TabItemSum)
                    {

                        object[] header = { "ConsumedDate", "W", "WT"};
                        object[] headerText = { "ConsumedDate", "Water consumption", "Total Water consumption" };

                        ExcelSerializernew<ShowManagmentConsumedWaterPivot_Result, ShowManagmentConsumedWaterPivot_Result, List<ShowManagmentConsumedWaterPivot_Result>> s = new ExcelSerializernew<ShowManagmentConsumedWaterPivot_Result, ShowManagmentConsumedWaterPivot_Result, List<ShowManagmentConsumedWaterPivot_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(GridSumGroup.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(FilePath, "Water Consumption", "", (List<ShowManagmentConsumedWaterPivot_Result>)view.SourceCollection, "", false,"");
                    }
                    else if (TabControlMain.SelectedItem == ReportTabItem)
                    {
                        object[] header = { "MeterNumber", "ConsumedDate", "w", "WT" };
                        object[] headerText = { "MeterNumber", "ConsumedDate", "Water consumption", "Total Water consumption" };

                        ExcelSerializernew<ShowConsumedWaterPivot_Result, ShowConsumedWaterPivot_Result, List<ShowConsumedWaterPivot_Result>> s = new ExcelSerializernew<ShowConsumedWaterPivot_Result, ShowConsumedWaterPivot_Result, List<ShowConsumedWaterPivot_Result>>();
                        ICollectionView view = CollectionViewSource.GetDefaultView(GridMain.ItemsSource);
                        s.header = header;
                        s.headerText = headerText;
                        s.CreateWorkSheet(FilePath, "Water Consumption", "", (List<ShowConsumedWaterPivot_Result>)view.SourceCollection, "", false,"");
                    }
                }
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

        private void datePickerEnd_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            SetDate();
            RefreshDatagrid();
            setchart();
        }

        private void datePickerStart_SelectedDateChanged_1(object sender, RoutedEventArgs e)
        {
            SetDate();
            RefreshDatagrid();
            setchart();
        }


    }
}
