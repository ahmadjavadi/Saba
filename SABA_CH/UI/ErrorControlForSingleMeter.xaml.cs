using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
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
    /// Interaction logic for ErrorControlForSingleMeter.xaml
    /// </summary>
    public partial class ErrorControlForSingleMeter : System.Windows.Window
    {
        public string Startdate = "";
        public string Enddate = "";
        public ShowTranslateofLable tr = null;
        string Filter = "";
        public ShowButtonAccess Us = null;
        public readonly int WindowId = 44;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
        public ErrorControlForSingleMeter()
        {
            tr = CommonData.translateWindow(WindowId);
            InitializeComponent();
            this.Title = this.Title + " " + CommonData.MeterNumber;
            ChangeFlowDirection();
            if (CommonData.LanguagesID == 2)
            {
                datePickerEnd.Visibility = Visibility.Visible;
                datePickerStart.Visibility = Visibility.Visible;
                datePickerEnden.Visibility = Visibility.Hidden;
                datePickerStarten.Visibility = Visibility.Hidden;
                var s = datePickerStart.SelectedDate.AddDays(-365);
                datePickerStart.SelectedDate = s;
                //s =s.AddDays(-365);
                //int year = datePickerStart.SelectedDate.Year - 1;
                //string start = datePickerStart.Text;
                //start = start.Replace(datePickerStart.SelectedDate.Year.ToString(), year.ToString());
                datePickerStart.Text = s.ToString();
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
            ObjectParameter minReadDate = new ObjectParameter("MinReadDate", "");
            SQLSPS.ShowMinReadDate(CommonData.SelectedMeterID, minReadDate);
            if (!string.IsNullOrEmpty( minReadDate.Value .ToString()))
                if (CommonData.LanguagesID == 2)
                {
                    if (minReadDate.Value.ToString().Contains(" "))
                        datePickerStart.Text = minReadDate.Value.ToString().Substring(0, minReadDate.Value.ToString().IndexOf(" "));
                    else
                        datePickerStart.Text = minReadDate.Value.ToString();
                }
                else
                {
                    DateTime dt = new DateTime();
                    datePickerStarten.Text = dt.ToString();
                }
            GridMain.DataContext = tr.TranslateofLable;
            Translate();
            

            SetDate();           
            Refresh();
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

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
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
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        public void SetDate()
        {
            try
            {
               
                //   var s = datePickerStart.SelectedDate;
                //datePickerStart.Text = s.ToString();
                //if (Startdate.Contains(" "))
                //    Startdate = Startdate.Substring(0, Startdate.IndexOf(" "));
                //Enddate = datePickerEnd.Text;
                //if (Enddate.Contains(" "))
                //    Enddate = Startdate.Substring(0, Startdate.IndexOf(" "));
                //int day = Convert.ToInt32(Startdate.Substring(Startdate.LastIndexOf('/') + 1, Startdate.Length - Startdate.LastIndexOf('/') - 1));
                //int month = Convert.ToInt32(Startdate.Substring(Startdate.IndexOf('/') + 1, Startdate.LastIndexOf('/') - Startdate.IndexOf('/') - 1));
                //if (day < 10)

                //    Startdate = Startdate.Substring(0, Startdate.Length - 1) + "0" + day;

                //if (month < 10)
                //    Startdate = Startdate.Substring(0, Startdate.IndexOf('/') + 1) + "0" + Startdate.Substring(Startdate.IndexOf('/') + 1, Startdate.Length - Startdate.IndexOf('/') - 1);
                //day = Convert.ToInt32(Enddate.Substring(Enddate.LastIndexOf('/') + 1, Enddate.Length - Enddate.LastIndexOf('/') - 1));
                //month = Convert.ToInt32(Enddate.Substring(Enddate.IndexOf('/') + 1, Enddate.LastIndexOf('/') - Enddate.IndexOf('/') - 1));
                //if (day < 10)

                //    Enddate = Enddate.Substring(0, Enddate.Length - 1) + "0" + day;

                //if (month < 10)
                //    Enddate = Enddate.Substring(0, Enddate.IndexOf('/') + 1) + "0" + Enddate.Substring(Enddate.IndexOf('/') + 1, Enddate.Length - Enddate.IndexOf('/') - 1);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void Translate()
        {
            try
            {
                DatagridGeneral.Columns[0].Header = tr.TranslateofLable.Object1;
                DatagridGeneral.Columns[1].Header = tr.TranslateofLable.Object3;
                DatagridGeneral.Columns[2].Header = tr.TranslateofLable.Object4;
                DatagridGeneral.Columns[3].Header = tr.TranslateofLable.Object5;
                
                DatagridGeneral.Columns[4].Header = tr.TranslateofLable.Object6;
                DatagridGeneral.Columns[5].Header = tr.TranslateofLable.Object7;
                DatagridGeneral.Columns[6].Header = tr.TranslateofLable.Object8;
                DatagridGeneral.Columns[7].Header = tr.TranslateofLable.Object9;
                DatagridGeneral.Columns[8].Header = tr.TranslateofLable.Object11;
                DatagridGeneral.Columns[9].Header = tr.TranslateofLable.Object12;
                DatagridGeneral.Columns[10].Header = tr.TranslateofLable.Object13;
                DatagridGeneral.Columns[11].Header = tr.TranslateofLable.Object10;
                DatagridGeneral.Columns[12].Header = tr.TranslateofLable.Object19;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void Refresh()
        {
            try
            {
                Startdate = datePickerStart.Text;
                Enddate = datePickerEnd.Text;
                string filter = "";
                if (CommonData.selectedGroup.GroupID != -1)
                    filter = "  and Main.MeterID=" + CommonData.SelectedMeterID.ToString();
                
                if (CommonData.LanguagesID == 2)
                    filter = filter + " and (( substring(Header.ReadDate,1,10)  between '" + Startdate + "' and '" + Enddate + "') or ( substring(Header.ReadDate,1,10)  between '" + Startdate.Replace("/0","/") + "' and '" + Enddate.Replace("/0", "/") + "'))";
                else
                    filter = filter + " and substring(Header.ReadDate,1,10) between '" + datePickerStart.Text + "' and '" + datePickerEnd.Text + "'";
                
                
                ShowErrorControlReportNew errcontrol = new ShowErrorControlReportNew(filter, Filter);
                DatagridGeneral.ItemsSource = null;
                DatagridGeneral.ItemsSource = errcontrol._lstshowErrorControlReportNew;
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

        private void ToolStripButtonExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "Meter Error" + "(" + CommonData.MeterNumber + ")"; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = " (.xlsx)|*.xlsx"; // Filter files by extension 
                Nullable<bool> result = dlg.ShowDialog();
                string filePath = dlg.FileName;
                if (result.Value)
                {
                    object[] header = { "MeterNumber", "ReadDate", "SourceTypeName", "Disconnect", "ExpiryDate", "Overdrafts", "HasCurrentWithDisconnect", "BaRand", "OpenMeterDoor", "OpenTerminalDoor", "HasCurrentWithoutVolt", "MeterError" };
                    object[] headerText = { tr.TranslateofLable.Object1, tr.TranslateofLable.Object3, tr.TranslateofLable.Object4, tr.TranslateofLable.Object5, tr.TranslateofLable.Object6, tr.TranslateofLable.Object7, tr.TranslateofLable.Object8, tr.TranslateofLable.Object9, tr.TranslateofLable.Object11, tr.TranslateofLable.Object12, tr.TranslateofLable.Object13, tr.TranslateofLable.Object19 };
                    ExcelSerializernew<showErrorControlReportNew_Result, showErrorControlReportNew_Result, List<showErrorControlReportNew_Result>> s = new ExcelSerializernew<showErrorControlReportNew_Result, showErrorControlReportNew_Result, List<showErrorControlReportNew_Result>>();
                    ICollectionView view = CollectionViewSource.GetDefaultView(DatagridGeneral.ItemsSource);
                    s.header = header;
                    s.headerText = headerText;
                    s.CreateWorkSheet(filePath, "Meters Error", CommonData.MeterNumber, (List<showErrorControlReportNew_Result>)view.SourceCollection, "", false,"");
                }
            }
            catch { }
        }

        private void datePickerStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }
        private void cb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

        }

        private void datePickerEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            datePickerEnd.Focus();
          
            datePickerEnd.Focus();
        }

        private void datePickerStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            datePickerStart.Focus();
     
            datePickerStart.Focus();
           
        }

        
    }
}
