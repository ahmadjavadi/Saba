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
    /// Interaction logic for negativeCreditReport.xaml
    /// </summary>
    public partial class negativeCreditReport : System.Windows.Window
    {
        public readonly int windowID = 21;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
        public ShowTranslateofLable tr = null;
        public ShowGroups_Result selectedGroup {get;set;}
        public ShowCustomers_Result selectedCustomer { get; set; }
        public string startdate = "";
        public string enddate = "";
        public string MeterNumber = "";
        public negativeCreditReport()
        {
            InitializeComponent();
            if (CommonData.LanguagesID == 2)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(1065);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                datePickerEnd.Visibility = Visibility.Visible;
                datePickerStart.Visibility = Visibility.Visible;
                datePickerEnden.Visibility = Visibility.Hidden;
                datePickerStarten.Visibility = Visibility.Hidden;
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
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                datePickerStarten.SelectedDate = DateTime.Now;
                datePickerEnden.SelectedDate = DateTime.Now;
            }
            tr = CommonData.translateWindow(windowID);
            MainGrid.DataContext = tr.TranslateofLable;
            changeFlowDirection();
            TranslateGridMeter();
            chkDate.IsChecked = false;
            datePickerStart.IsEnabled = false;
            datePickerEnd.IsEnabled = false;
            SetDate();
            refresh();

        }
        public void SetDate()
        {
            try
            {
                startdate = datePickerStart.Text;
                enddate = datePickerEnd.Text;
                int day = Convert.ToInt32(startdate.Substring(startdate.LastIndexOf('/') + 1, startdate.Length - startdate.LastIndexOf('/') - 1));
                int Month = Convert.ToInt32(startdate.Substring(startdate.IndexOf('/') + 1, startdate.LastIndexOf('/') - startdate.IndexOf('/') - 1));
                if (day < 10)

                    startdate = startdate.Substring(0, startdate.Length - 1) + "0" + day;

                if (Month < 10)
                    startdate = startdate.Substring(0, startdate.IndexOf('/') + 1) + "0" + startdate.Substring(startdate.IndexOf('/') + 1, startdate.Length - startdate.IndexOf('/') - 1);
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
        public void refresh()
        {
            try
            {
                string Filter = "";
                if (CommonData.selectedGroup.GroupID != -1)
                    Filter = "  and MTG.GroupID=" + CommonData.selectedGroup.GroupID + "  and MTG.GroupType=" + CommonData.selectedGroup.GroupType;
                else
                    Filter = " ";
                
                if (CommonData.selectedGroup.GroupID != -1)
                    Filter = "  and MTG.GroupID=" + CommonData.selectedGroup.GroupID + " and MTG.GroupType=" + CommonData.selectedGroup.GroupType;
                if (chkDate.IsChecked == true)
                {
                    if (CommonData.LanguagesID == 2)
                        Filter = Filter + " and m.ReadDate between '" + startdate + "' and '" + enddate + "'";
                    else
                        Filter = Filter + " and m.ReadDate between '" + datePickerStarten.Text + "' and '" + datePickerEnden.Text + "'";
                }
                else
                {
                    Filter = " and m.ReadDate=(Select Max(ReadDate) From OBISValueHeader where OBISValueHeader.MeterID=m.MeterID)";
                }
                ShowNegativeCredit shownegativecredit = new ShowNegativeCredit(Filter);
                GridMeter.ItemsSource = null;
                GridMeter.ItemsSource = shownegativecredit._lstShowNegativeCredit;

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void changeFlowDirection()
        {
            try
            {
                //MainGrid.FlowDirection = CommonData.FlowDirection;
                
                //GridMeter.FlowDirection = CommonData.FlowDirection;
                
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        
        public void TranslateGridMeter()
        {
            try
            {
                GridMeter.Columns[0].Header = tr.TranslateofLable.Object8;
                GridMeter.Columns[1].Header = tr.TranslateofLable.Object9;
                GridMeter.Columns[2].Header = tr.TranslateofLable.Object7;
                GridMeter.Columns[3].Header = tr.TranslateofLable.Object10;
                GridMeter.Columns[4].Header = tr.TranslateofLable.Object11;
                GridMeter.Columns[5].Header = tr.TranslateofLable.Object12;
                GridMeter.Columns[6].Header = tr.TranslateofLable.Object13;
                GridMeter.Columns[7].Header = tr.TranslateofLable.Object14;
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

        private void GridMeter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MeterNumber.StartsWith("207"))
                {
                    CreditModes207.Visibility = Visibility.Visible;
                    CreditModes303.Visibility = Visibility.Hidden;
                }
                else
                {
                    CreditModes207.Visibility = Visibility.Hidden;
                    CreditModes303.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetDate();
                refresh();
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
                dlg.FileName = "Negative Credit"; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = " (.xlsx)|*.xlsx"; // Filter files by extension 
                Nullable<bool> result = dlg.ShowDialog();
                string FilePath = dlg.FileName;
                if (result.Value)
                {
                    object[] header = { "MeterNumber", "WatersubscriptionNumber", "DossierNumber", "Value", "debtValue", "ReadDate", "TotalcreditVal", "AnnuaCreditVal" };
                    object[] headerText = { tr.TranslateofLable.Object8, tr.TranslateofLable.Object9, tr.TranslateofLable.Object7, tr.TranslateofLable.Object10, tr.TranslateofLable.Object11, tr.TranslateofLable.Object12, tr.TranslateofLable.Object13, tr.TranslateofLable.Object14 };
                    ExcelSerializernew<ShowNegativeCredit_Result, ShowNegativeCredit_Result, List<ShowNegativeCredit_Result>> s = new ExcelSerializernew<ShowNegativeCredit_Result, ShowNegativeCredit_Result, List<ShowNegativeCredit_Result>>();
                    ICollectionView view = CollectionViewSource.GetDefaultView(GridMeter.ItemsSource);
                    s.header = header;
                    s.headerText = headerText;
                    s.CreateWorkSheet(FilePath, "Negative Credit", "", (List<ShowNegativeCredit_Result>)view.SourceCollection, "", false,"");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void chkDate_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                datePickerEnd.IsEnabled = true;
                datePickerStart.IsEnabled = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void chkDate_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                datePickerEnd.IsEnabled = false;
                datePickerStart.IsEnabled = false;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        
        private void datePickerEnd_SelectedDateChanged(object sender, RoutedEventArgs e)
        {

        }

        private void datePickerStart_SelectedDateChanged(object sender, RoutedEventArgs e)
        {

        }

    }
}
