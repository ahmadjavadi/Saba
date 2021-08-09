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
    /// Interaction logic for NominalDemandViolationReport.xaml
    /// </summary>
    public partial class NominalDemandViolationReport : System.Windows.Window
    {
        public string startdate = "";
        public string enddate = "";
        public readonly int windowID = 22;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
        public ShowTranslateofLable tr = null;
       
        public showFilterforNominalDemandReport_Result SelectedFilter = new showFilterforNominalDemandReport_Result();
        public NominalDemandViolationReport()
        {
            InitializeComponent();
            tr = CommonData.translateWindow(windowID);
            changeFlowDirection();
            txtMinNumber.Text = "10";
            txtpercent.Text = "10";
            chkDate.IsChecked = false;
            datePickerEnd.IsEnabled = false;
            datePickerStart.IsEnabled = false;
            chkpercent.IsChecked = true;
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
            TranslateGridMeter();
            SetDate();
            Refresh();

           
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
        private void Refresh()
        {
            try
            {
                string Filter="";
                if (chkMin.IsChecked == true)
                {
                    Filter = " and DemandMinuteValue >" + txtMinNumber.Text;
                }
                else if (chkpercent.IsChecked == true)
                {
                    Filter = " and RealDemandValue >(" + txtpercent.Text + "* NominalDemandValue)/100";
                }
                if (CommonData.selectedGroup.GroupID != -1)
                    Filter = Filter+" and MTG.groupid=" + CommonData.selectedGroup.GroupID + " and mtg.grouptype= " + CommonData.selectedGroup.GroupType;

                if (chkDate.IsChecked==false)
                {
                    Filter = Filter + " and n.ReadDate=(Select Max(ReadDate) From OBISValueHeader where OBISValueHeader.MeterID=n.MeterID)";
                }
                else
                    if(datePickerStart.SelectedDate!=null && datePickerEnd.SelectedDate!=null)
                    {
                        if(CommonData.LanguagesID==2)
                            Filter = Filter + " and n.ReadDate between '" + startdate + "' and '" + enddate + "'";
                        else
                            Filter = Filter + " and n.ReadDate between '" + datePickerStart.Text.ToString() + "' and '" + datePickerEnd.Text.ToString() + "'";
                    }

                ShowNominalDemandViolation NominalDemand = new ShowNominalDemandViolation(Filter);
                GridMeter.ItemsSource = NominalDemand._lstShowNominalDemandViolation;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void TranslateGridMeter()
        {
            try
            {
                GridFilter.DataContext = tr.TranslateofLable;
                GridMeter.Columns[0].Header = tr.TranslateofLable.Object1;
                GridMeter.Columns[1].Header = tr.TranslateofLable.Object2;
                GridMeter.Columns[2].Header = tr.TranslateofLable.Object6;
                GridMeter.Columns[3].Header = tr.TranslateofLable.Object7;
                GridMeter.Columns[4].Header = tr.TranslateofLable.Object8;
                GridMeter.Columns[5].Header = tr.TranslateofLable.Object9;
                GridMeter.Columns[6].Header = tr.TranslateofLable.Object10;
                GridMeter.Columns[7].Header = tr.TranslateofLable.Object11;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void changeFlowDirection()
        {
            try
            {
                //GridMain.FlowDirection = CommonData.FlowDirection;
                //GridMeter.FlowDirection = CommonData.FlowDirection;
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

        private void ToolStripButtonExportToExcel_Click(object sender, RoutedEventArgs e)
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
                    object[] header = { "MeterNumber", "ReadDate", "NominalDemandValue", "RealDemandValue", "DemandMinuteValue", "NumberDemandValue" };
                    object[] headerText = { "MeterNumber", "ReadDate", "NominalDemandValue", "RealDemandValue", "DemandMinuteValue", "NumberDemandValue" };
                    ExcelSerializernew<ShowNominalDemandViolation_Result, ShowNominalDemandViolation_Result, List<ShowNominalDemandViolation_Result>> s = new ExcelSerializernew<ShowNominalDemandViolation_Result, ShowNominalDemandViolation_Result, List<ShowNominalDemandViolation_Result>>();
                    ICollectionView view = CollectionViewSource.GetDefaultView(GridMeter.ItemsSource);
                    s.header = header;
                    s.headerText = headerText;
                    s.CreateWorkSheet(FilePath, "Power Consumption", "", (List<ShowNominalDemandViolation_Result>)view.SourceCollection, "", false,"");
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
            Refresh();
        }

        private void datePickerStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
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

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
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

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMinNumber.IsEnabled = true;
                txtpercent.IsEnabled = false;
                chkpercent.IsChecked = false;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void chkpercent_Checked(object sender, RoutedEventArgs e)
        {
             try
            {
                txtMinNumber.IsEnabled = false;
                txtpercent.IsEnabled = true;
                chkMin.IsChecked = false;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void chkpercent_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMinNumber.IsEnabled = true;
                txtpercent.IsEnabled = false;
                chkMin.IsChecked = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void chkMin_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMinNumber.IsEnabled = false;
                txtpercent.IsEnabled = true;
                chkpercent.IsChecked = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void datePickerStart_SelectedDateChanged(object sender, RoutedEventArgs e)
        {

        }

       

        
    }
}
