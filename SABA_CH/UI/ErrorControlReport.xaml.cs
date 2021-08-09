using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for ErrorControlReport.xaml
    /// </summary>
    public partial class ErrorControlReport : System.Windows.Window
    {
        public string Startdate = "";
        public string Enddate = "";
        public ShowTranslateofLable tr = null;
        string _filter = "";
        public ShowButtonAccess Us = null;
        public readonly int WindowId = 23;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
        public ErrorControlReport()
        {
            InitializeComponent();
            
            ChangeFlowDirection();
            tr = CommonData.translateWindow(WindowId);
            GridMain.DataContext = tr.TranslateofLable;
            Translate();
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
            chkor.IsChecked = true;
            CheckedAll();
            Refresh();
        }
        public void SetDate()
        {
            try
            {
                Startdate = datePickerStart.Text;
                Enddate = datePickerEnd.Text;
                int day = Convert.ToInt32(Startdate.Substring(Startdate.LastIndexOf('/') + 1, Startdate.Length - Startdate.LastIndexOf('/') - 1));
                int Month = Convert.ToInt32(Startdate.Substring(Startdate.IndexOf('/') + 1, Startdate.LastIndexOf('/') - Startdate.IndexOf('/') - 1));
                if (day < 10)

                    Startdate = Startdate.Substring(0, Startdate.Length - 1) + "0" + day;

                if (Month < 10)
                    Startdate = Startdate.Substring(0, Startdate.IndexOf('/') + 1) + "0" + Startdate.Substring(Startdate.IndexOf('/') + 1, Startdate.Length - Startdate.IndexOf('/') - 1);
                day = Convert.ToInt32(Enddate.Substring(Enddate.LastIndexOf('/') + 1, Enddate.Length - Enddate.LastIndexOf('/') - 1));
                Month = Convert.ToInt32(Enddate.Substring(Enddate.IndexOf('/') + 1, Enddate.LastIndexOf('/') - Enddate.IndexOf('/') - 1));
                if (day < 10)

                    Enddate = Enddate.Substring(0, Enddate.Length - 1) + "0" + day;

                if (Month < 10)
                    Enddate = Enddate.Substring(0, Enddate.IndexOf('/') + 1) + "0" + Enddate.Substring(Enddate.IndexOf('/') + 1, Enddate.Length - Enddate.IndexOf('/') - 1);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        private void Translate()
        {
            try
            {
                DatagridGeneral.Columns[0].Header = tr.TranslateofLable.Object1;
                DatagridGeneral.Columns[1].Header = tr.TranslateofLable.Object3;
                //DatagridGeneral.Columns[2].Header = tr.TranslateofLable.Object4;
                DatagridGeneral.Columns[2].Header = tr.TranslateofLable.Object5;
                DatagridGeneral.Columns[3].Header = tr.TranslateofLable.Object6;
                DatagridGeneral.Columns[4].Header = tr.TranslateofLable.Object7;
                DatagridGeneral.Columns[5].Header = tr.TranslateofLable.Object8;
                DatagridGeneral.Columns[6].Header = tr.TranslateofLable.Object9;
                DatagridGeneral.Columns[7].Header = tr.TranslateofLable.Object11;
                DatagridGeneral.Columns[8].Header = tr.TranslateofLable.Object12;
                DatagridGeneral.Columns[9].Header = tr.TranslateofLable.Object13;
                DatagridGeneral.Columns[10].Header = tr.TranslateofLable.Object10;
                DatagridGeneral.Columns[11].Header = tr.TranslateofLable.Object19; 
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        private void ChangeFlowDirection()
        {
            try
            {
                //GridMain.FlowDirection = CommonData.FlowDirection;
                //DatagridGeneral.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            _tabCtrl.SelectedItem = _tabPag;
            if (!_tabCtrl.IsVisible)
            {

                _tabCtrl.Visibility = Visibility.Visible;

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
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }

        }
        public void CheckedAll()
        {
            try
            {
                CheckBox chk;
                foreach (var item in gridchk.Children)
                {
                    if (item is CheckBox)
                    {
                        chk = (item as CheckBox);
                        chk.IsChecked = true;
                    }
                }
                chkor.IsChecked = true;
                chkDate.IsChecked = false;
            }

            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public void Refresh()
        {
            try
            {
                string filter = "";                
                if (CommonData.selectedGroup.GroupID != -1)
                    filter = "  and MTG.GroupID=" + CommonData.selectedGroup.GroupID + " and MTG.GroupType=" + CommonData.selectedGroup.GroupType;
                if (chkDate.IsChecked == true)
                {
                    if (CommonData.LanguagesID == 2)
                        filter = filter + " and substring(Header.ReadDate,1,10)  between '" + Startdate + "' and '" + Enddate + "'";
                    else
                        filter = filter + " and substring(Header.ReadDate,1,10) between '" + datePickerStart.Text + "' and '" + datePickerEnd.Text + "'";
                }
                else
                {
                    filter = " and Header.ReadDate=(SELECT MAX(ReadDate) from OBISValueHeader h where h.meterID=Header.MeterID) ";
                }
                if (_filter.Substring(1, 2) == "or")
                    _filter = " and" + _filter.Substring(3, _filter.Length - 3);
                ShowErrorControlReportNew errcontrol = new ShowErrorControlReportNew(filter, _filter);
                DatagridGeneral.ItemsSource=null;
                DatagridGeneral.ItemsSource=errcontrol._lstshowErrorControlReportNew;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            SetDate();
            Refresh();
        }

        private void chkDisconnect_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = "";
                string s = "";
                name = (sender as CheckBox).Name;
                name = name.Replace("chk", "");
                if (_filter == "")
                    s = " and ";
                else if(chkand.IsChecked==true)
                    s = " and ";
                else if (chkor.IsChecked == true)
                    s = " or ";
                s = s + name + "=1 ";
                if (!_filter.Contains(s))
                    _filter = _filter + s;

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void chkDisconnect_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = "";
                name = (sender as CheckBox).Name;
                name = name.Replace("chk", "");
                _filter = _filter .Replace(" or " + name + "=1 ","");
                _filter = _filter.Replace(" and " + name + "=1 ", "");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void chkor_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                chkand.IsChecked = false;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void chkand_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                chkor.IsChecked = false;
                _filter = _filter.Replace(" or ", " and ");
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void chkand_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if(chkor.IsChecked==false)
                    chkor.IsChecked = true;
                if (_filter.Length > 4)
                {
                    string s = _filter.Substring(4, _filter.Length - 4);
                    s = s.Replace(" and ", " or ");
                    _filter = _filter.Substring(0, 4) + s;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void chkor_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkand.IsChecked == false)
                    chkand.IsChecked = true;
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void datePickerStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void cb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
               
            }
            catch (Exception)
            {
                
                throw;
            }
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
                    object[] header = { "MeterNumber", "ReadDate", "Disconnect", "ExpiryDate", "Overdrafts", "HasCurrentWithDisconnect", "BaRand", "OpenMeterDoor", "OpenTerminalDoor", "HasCurrentWithoutVolt", "MeterError" };
                    object[] headerText = { tr.TranslateofLable.Object1, tr.TranslateofLable.Object3, tr.TranslateofLable.Object5, tr.TranslateofLable.Object6, tr.TranslateofLable.Object7, tr.TranslateofLable.Object8, tr.TranslateofLable.Object9, tr.TranslateofLable.Object11, tr.TranslateofLable.Object12, tr.TranslateofLable.Object13, tr.TranslateofLable.Object19 };
                    ExcelSerializernew<showErrorControlReportNew_Result, showErrorControlReportNew_Result, List<showErrorControlReportNew_Result>> s = new ExcelSerializernew<showErrorControlReportNew_Result, showErrorControlReportNew_Result, List<showErrorControlReportNew_Result>>();
                    ICollectionView view = CollectionViewSource.GetDefaultView(DatagridGeneral.ItemsSource);
                    s.header = header;
                    s.headerText = headerText;
                    s.CreateWorkSheet(FilePath, "Meters Error", "", (List<showErrorControlReportNew_Result>)view.SourceCollection, "", false,"");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
    }
}
