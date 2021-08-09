using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using MeterStatus;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;
using Action = System.Action;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;
using Window = System.Windows.Window;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for ExportToExcel.xaml
    /// </summary>
    public partial class ExportToExcel : System.Windows.Window
    {
        public List<SelectedMeter> lstSelectedMeter = null;
        public ShowReports_Result selectedreport;
        public bool IsNew = false;
        public readonly int windowID = 17;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public DataTable ExcelDT = new DataTable();
        bool ISCul2Report = false;
        public List<DataTable> ListofReportingMeters;
        //public Thread threadReport;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        List<string> MeterID;
        List<decimal?> CustomerIds;
        string[] ReadDate;
        List<string> SourceTypeName;
        List<string> MeterNumber;
        List<string> MaxReadDate;
        string[] OBISType;
        string[] SheetsName;
        public string OBISs = "";
        public TabControl Tab { set { tabCtrl = value; } }
        public ShowTranslateofLable tr = null;
        public string startdate = "";
        public string enddate = "";
        public ExportToExcel()
        {
            InitializeComponent();
            tr = CommonData.translateWindow(windowID);
            TranslateLabels();
            changeFlowDirection();
            if (CommonData.LanguagesID == 2)
            {
                //PleaseWateLabel.Visibility = Visibility.Hidden;
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
                //PleaseWateLabel.Visibility = Visibility.Hidden;
                datePickerEnden.Visibility = Visibility.Visible;
                datePickerStarten.Visibility = Visibility.Visible;
                datePickerEnd.Visibility = Visibility.Hidden;
                datePickerStart.Visibility = Visibility.Hidden;
                datePickerStarten.SelectedDate = DateTime.Now;
                datePickerEnden.SelectedDate = DateTime.Now;
            }

            RefreshGridMain();
            //changeHeaderMeterGrid();
            //RefreshMeters();
            GetMeterNumberForExcelFile();

            SetDate();

            if (MeterNumber.Count == 1)
            {
                Title = $"ارسال اطلاعات کنتور {MeterNumber[0]} به Excel"; 
                ObjectParameter MinReadDate = new ObjectParameter("MinReadDate", "");
                SQLSPS.ShowMinReadDate(Convert.ToDecimal(MeterID[0]), MinReadDate);
                if (MinReadDate.Value != "")
                {
                    if (CommonData.LanguagesID == 2)
                    {

                        if (MinReadDate.Value.ToString().Length >= 8)
                        {
                            //  datePickerStart.Text = MinReadDate.Value.ToString().Substring(0, 10);
                            if (MinReadDate.Value.ToString().Contains(" "))
                                datePickerStart.Text = MinReadDate.Value.ToString().Substring(0, MinReadDate.Value.ToString().IndexOf(' '));
                            else
                                datePickerStart.Text = MinReadDate.Value.ToString().Substring(0, 10);
                        }
                    }
                    else
                    {
                        DateTime dt = new DateTime();
                        datePickerStarten.Text = dt.ToString();
                    }
                    SetDate();
                }
                string filter = "  and Main.MeterID=" + MeterID[0].ToString();
                // Add One Day
                string Add1Day = datePickerEnd.DisplayDate.AddDays(1).ToString();
                Add1Day = Add1Day.Substring(Add1Day.LastIndexOf('/') + 1);

                var en = RsaDateTime.PersianDateTime.ConvertToGeorgianDateTime(enddate).AddDays(1);
                //string EndDate = enddate.ToString().Remove(8, 2);
                string EndDate = en.ToPersianDate();
                //
                if (CommonData.LanguagesID == 2)
                    filter = filter + " and Main.ReadDate Between '" + startdate.ToString().Substring(0, 10) + "' and '" + EndDate + "'";
                RefreshSelectedMeters(filter);
            }
            changeHeaderReadDateGrid();
        }
        public void TranslateLabels()
        {
            try
            {
                GridMain.Columns[0].Header = tr.TranslateofLable.Object1;
                MainGrid.DataContext = tr.TranslateofLable;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void changeFlowDirection()
        {
            try
            {
                //MainGrid.FlowDirection = CommonData.FlowDirection;
                //GridMain.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void RefreshGridMain()
        {
            try
            {
                GridMain.ItemsSource = null;
                ShowReports showreports = new ShowReports("");
                GridMain.ItemsSource = showreports._lstShowReports;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
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
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {

                tabCtrl.Items.Remove(tabPag);
                if (!tabCtrl.HasItems)
                {

                    tabCtrl.Visibility = Visibility.Hidden;

                }
                ClassControl.OpenWin[windowID] = false;
                CommonData.mainwindow.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void RefreshSelectedMeters(string filter)
        {
            try
            {
                ShowObisValueHeader OBISValueDetail = new ShowObisValueHeader(filter);
                ReadDateGrid.ItemsSource = null;
                ReadDateGrid.ItemsSource = OBISValueDetail._lstShowOBISValueHeader;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void GridMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (GridMain.SelectedItem != null)
                {
                    selectedreport = (ShowReports_Result)GridMain.SelectedItem;

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (selectedreport != null)
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.FileName = "Report"+DateTime.Now.ToPersianString1(); // Default file name
                    dlg.DefaultExt = ".xlsx"; // Default file extension
                    dlg.Filter = " (.xlsx)|*.xlsx"; // Filter files by extension 
                    Nullable<bool> result = dlg.ShowDialog();
                    if (result == true)
                    {

                        string FilePath = dlg.FileName;
                        GetReportWarning();

                        if (ReadDateGrid.SelectedIndex == -1)
                        {
                            Thread threadReport = new Thread(() => ExportDataToExcel(FilePath));
                            threadReport.IsBackground = true;
                            threadReport.Start();

                        }

                        else
                        {
                            Thread threadReport1 = new Thread(() => ExportDataToExcelwithReadOutDate(FilePath));
                            threadReport1.IsBackground = true;
                            threadReport1.Start();

                        }
                    }
                }
                else
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message21);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        //public void WaitingtoReport()
        //{
        //    try
        //    {
        //        CommonData.mainwindow.changeProgressBarTag("لطفا منتظر بمانید");
        //        CommonData.mainwindow.changeProgressBarValue(200);

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
        //    }
        //}

        //+++++++++++++++++++++++++++++++++++++++++      // new thread for get news to users to wait
        public void GetReportWarning()
        {
            //Thread th = new Thread(getDataFromCard(type));
            Thread th = new Thread(delegate () { WriteOnprogress(); });
            th.SetApartmentState(ApartmentState.STA);
            //th.IsBackground = true;
            th.Start();
        }

        public void WriteOnprogress()
        {
            CommonData.mainwindow.changeProgressBarValue(0);
            CommonData.mainwindow.changeProgressBarTag("");
            CommonData.mainwindow.changeProgressBarValue(1);
            CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);

        }

        // +++++++++++++++++++++++++++++++++++++++++

        public void ExportDataToExcel(string FilePath)
        {
            try
            {

                Application MyExcel = new Application();
                if (MyExcel == null)
                {
                    MessageBox.Show("Excel is not properly installed!!");
                    return;
                }
                ISCul2Report = false;
                bool IsDateTime = false;

                ExcelDT.Clear();
                ListofReportingMeters = new List<DataTable>();
                ListofReportingMeters.Clear();

                object missing = Type.Missing;
                Workbook wb = MyExcel.Workbooks.Add(missing);
                Worksheet ws = (Worksheet)wb.ActiveSheet;

                Range rng = ws.get_Range("A1", missing); ;

                MyExcel.Visible = false;
                ws = (Worksheet)wb.Sheets[1];

                MyExcel.DefaultSheetDirection = (int)Constants.xlRTL;

                // fill Row 1
                bool IsCustomerAdd = false;
               // string ISOBISIDS = GetOBIssInOBIStype();
                string OBISIDS =   GetOBIssInOBIStype();
                for (int d = 0; d < OBISType.Length; d++)
                {
                    if (OBISType[d] == "12")
                        IsCustomerAdd = true;
                }

                int FillColumns = 1;
                string Exception = "";
                if (selectedreport.ReportID == 2)
                    Exception = " and Main.OBISID<>81";

                string Filter = " and Main.ReportID= " + selectedreport.ReportID + "and Main.OBIstypeid !=12" + Exception + " order by obis ASC";


                ShowObisToExcel ShowOBISToExcel = new ShowObisToExcel(Filter);
                ExcelDT.Columns.Clear();
                try
                {
                    if (IsCustomerAdd)
                    {
                        ExcelDT.Columns.Add("نام مشترک");
                        ExcelDT.Columns.Add("نام خانوادگی");
                        ExcelDT.Columns.Add("شماره پرونده");
                        ExcelDT.Columns.Add("نوع مصرف");
                        ExcelDT.Columns.Add("دشت");
                        ExcelDT.Columns.Add("شماره کنتور");
                    }
                    ExcelDT.Columns.Add("منبع قرائت");
                    foreach (var itemC in ShowOBISToExcel._lstShowObisToExcel)
                    {
                        try
                        {
                            if (!itemC.ObisFarsiDesc.Contains("status") && !itemC.ObisFarsiDesc.Contains("credit") && !itemC.ObisFarsiDesc.Contains("شماره کنتور"))
                                ExcelDT.Columns.Add(itemC.ObisFarsiDesc);
                        }
                        catch { }
                    }

                    ListofReportingMeters.Add(ExcelDT);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                CommonData.mainwindow.changeProgressBarValue(2);

                int RowNum = 0, ColNumber = FillColumns;
               
                GetMeterNumberForExcelFile();
                rng = ws.UsedRange;
                ShowConsumedWaterPivot value;
                ShowCustomerReport CustomerReport;
                ShowReportObisValueDetail det;
                // get data from database and insert to Excel
                string Meternames = "", MeterIds = "";


                //List<ShowConsumedWaterPivot_Result> lst8 = new List<ShowConsumedWaterPivot_Result>();
                List<ShowReportOBISValueDetails_Result> lstothers = new List<ShowReportOBISValueDetails_Result>();
                int numOthers = 0;
                List<ShowCustomerReport_Result> lstCustomer = new List<ShowCustomerReport_Result>();
                for (int M = 0; M < MeterNumber.Count; M++)   // count of Meters
                {
                    Meternames = Meternames + MeterNumber[M].ToString() + ",";
                    MeterIds = MeterIds + MeterID[M].ToString() + ",";
                    
                }
                MeterIds = MeterIds.Remove(MeterIds.Length - 1);
                Meternames = Meternames.Remove(Meternames.Length - 1);
                IsDateTime = true;
                //DataRow DR = ExcelDT.NewRow();
                // RowNum++;
                ColNumber = 1;

                for (int cul = 0; cul < OBISType.Length; cul++)
                {
                    if (OBISType[cul] == "12")
                    {
                        string filter = " and Meter.MeterID in (" + MeterIds + ")";
                        CustomerReport = new ShowCustomerReport(filter);
                        for (int g = 0; g < CustomerReport._lstShowCustomerReport.Count; g++)
                        {
                            lstCustomer.Insert(g, CustomerReport._lstShowCustomerReport[g]);
                        }
                    }
                    //ISCustomerReport = true;


                    else if (OBISType[cul] == "2")
                        ISCul2Report = true;


                    else if (OBISType[cul] != "2" && OBISType[cul] != "8" && OBISType[cul] != "12")
                    {

                        Filter = "  and (charindex('\"" + OBISType[cul] + "\"',obiss.type)>0) and Header.MeterID in (" + MeterIds + ")" +
                         " and OBISs.OBISID in (" + OBISIDS + " ) and Header.OBISValueHeaderID IN (Select Max(OBISValueHeaderID) From OBISValueHeader Where MeterID in ("
                        + MeterIds + ") GROUP BY MeterID ) ";
                        det = new ShowReportObisValueDetail(Filter, CommonData.LanguagesID);

                        for (int k = 0; k < det._lstShowReportOBISValueDetail.Count; k++)
                        {
                            lstothers.Insert(numOthers, det._lstShowReportOBISValueDetail[k]);
                            numOthers++;
                        }

                    }

                    CommonData.mainwindow.changeProgressBarValue(2);


                }

                bool showConsumedWater = false;
                foreach (var item in OBISType)
                {
                    if (item.Contains("8"))
                        showConsumedWater = true;
                }


                ////ExcelDT.Rows.Add(DR);
                FillExcelForAllSelectedMeters(lstothers, showConsumedWater, lstCustomer, OBISIDS, "Max", null);

                for (int i = 0; i < ExcelDT.Columns.Count; i++)
                {
                    ws.Cells[1, (i + 1)] = ExcelDT.Columns[i].ColumnName;

                }

                for (int i = 0; i < ExcelDT.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < ExcelDT.Columns.Count; j++)
                    {
                        ws.Cells[(i + 2), (j + 1)] = ExcelDT.Rows[i][j];
                        ws.Cells[(i + 2), (j + 1)].Style.Numberformat = "@";

                    }
                    if ((i + 1) > 0 && ((i + 1) % 2) != 0)
                        ws.Rows[(i + 2)].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                    else
                        ws.Rows[(i + 2)].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);
                    rng = ws.UsedRange;
                    //rng.BorderAround(Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin);
                }

                ExcelDT.Clear();

                // Colect ws border and culomn size
                ws.UsedRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ws.UsedRange.Borders.Weight = XlBorderWeight.xlMedium;
                ws.UsedRange.Borders.LineStyle = XlLineStyle.xlContinuous;
                ws.UsedRange.Columns.AutoFit();
                ws.Rows[1].font.bold = true;
                ws.Rows[1].Interior.Color = ColorTranslator.ToOle(Color.Gainsboro);
                //freez first column
                ws.Application.ActiveWindow.SplitRow = 1;
                ws.Application.ActiveWindow.FreezePanes = true;

                //wb.SaveAs(filePath, AccessMode: Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared);
                wb.SaveAs(FilePath, XlFileFormat.xlWorkbookDefault, missing, missing, missing, missing,
                      XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                wb.Close(true, missing, missing);

                MyExcel.Quit();

                MessageBox.Show("گزارش به صورت فایل اکسل در آدرس تعیین شده ذخیره شد");
            }
            catch (Exception ex)
            {
                //PleaseWateLabel.Visibility = Visibility.Hidden;
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
            //PleaseWateLabel.Visibility = Visibility.Hidden;
            CommonData.mainwindow.changeProgressBarTag("");
            CommonData.mainwindow.changeProgressBarValue(0);
        }

        void FillExcelForAllSelectedMeters(List<ShowReportOBISValueDetails_Result> det, bool showConsumedWater,
               List<ShowCustomerReport_Result> Customer, string obisIDs, string readDate, List<ShowVeeConsumedWater_Result> lst8)
        {
            try
            {
                GetMeterNumberForExcelFile();
                string Readdatestring = "";
                for (int i = 0; i < MeterID.Count; i++)
                {
                    try
                    {
                        DataRow DR = ExcelDT.NewRow();
                        DataRow DR1 = null;
                        if (ISCul2Report)
                        {
                            if (readDate == "Max")
                                Readdatestring = "  and Header.ReadDate=(Select Max(ReadDate) From OBISValueHeader where MeterID=" + MeterID[i] + ")";
                            else
                                Readdatestring = "  and Header.ReadDate in (Select ReadDate From OBISValueHeader where MeterID=" + MeterID[i] + " and ReadDate='" + readDate + "')";

                            MeetrsStatus meterstatus = CreateListOfStatusValue(OBISs, MeterID[i], MeterNumber[i], Readdatestring);
                            foreach (Statusvalue item in meterstatus.MeetrsList)
                            {
                                for (int subList = 0; subList < item.List.Count; subList++)
                                {
                                    if (string.IsNullOrEmpty(item.List[subList].Description))
                                        continue;
                                    string Value = "";
                                    try
                                    {

                                        if (item.List[subList].IsStatuseTrue.ToString().ToUpper().Contains("DONTCARE"))
                                            Value = "NULL";
                                        else if (item.List[subList].IsStatuseTrue.ToString().ToUpper().Contains("TRUE"))
                                            Value = "فعال";
                                        else if (item.List[subList].IsStatuseTrue.ToString().ToUpper().Contains("FALSE"))
                                            Value = "غیرفعال";

                                        DR[item.List[subList].Description] = Value;
                                    }
                                    catch
                                    {
                                        ExcelDT.Columns.Add(item.List[subList].Description);
                                        if (item.List[subList].IsStatuseTrue.ToString().ToUpper().Contains("DONTCARE"))
                                            Value = "NULL";
                                        else if (item.List[subList].IsStatuseTrue.ToString().ToUpper().Contains("TRUE"))
                                            Value = "فعال";
                                        else if (item.List[subList].IsStatuseTrue.ToString().ToUpper().Contains("FALSE"))
                                            Value = "غیرفعال";
                                        DR[item.List[subList].Description] = Value;

                                    }
                                }

                            }

                        }

                        foreach (var item in det)
                            if (item.MeterID.ToString() == MeterID[i])
                            {
                                DR[item.ObisDesc] = (item.Value + item.OBISUnitDesc).ToString();
                                DR["منبع قرائت"] = item.SourceTypeName.ToString();

                            }
                        int Monthes = 0;

                        if (showConsumedWater)
                        {
                            lst8 = new List<ShowVeeConsumedWater_Result>();
                            ShowVeeConsumedWater value1 = new ShowVeeConsumedWater(Convert.ToDecimal(MeterID[i]));
                            for (int j = 0; j < value1._lstShowVeeConsumedWater.Count; j++)
                            {
                                lst8.Insert(j, value1._lstShowVeeConsumedWater[j]);
                            }

                            if (lst8 != null)
                            {
                                DR1 = ExcelDT.NewRow();

                                foreach (var item1 in lst8)
                                {

                                    Monthes++;
                                    //int NumberMonth = value._lstShowConsumedWaterPivot.Count;
                                    if (Monthes > 24)
                                        break;
                                    //Monthes = 22;
                                    //for (int i = 0; i < NumberMonth; i++)
                                    //{
                                    try
                                    {
                                        DR["آب مصرفي دوره" + Monthes.ToString()] = item1.ConsumedDate;
                                        DR["آب مصرفي کل تا دوره" + Monthes.ToString()] = item1.ConsumedDate;
                                        DR["ساعت كاركرد تا دوره" + Monthes.ToString()] = item1.ConsumedDate;

                                        DR1["آب مصرفي دوره" + Monthes.ToString()] = item1.MonthlyConsumption;
                                        DR1["آب مصرفي کل تا دوره" + Monthes.ToString()] = item1.TotalConsumption;

                                        DR1["ساعت كاركرد تا دوره" + Monthes.ToString()] = item1.PumpWorkingHour;
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                        string filter = " and Meter.MeterID in (" + MeterID[i] + ")";
                        ShowCustomerReport customerReport = new ShowCustomerReport(filter);
                        if (customerReport._lstShowCustomerReport.Count > 0)
                        {
                            var item2 = customerReport._lstShowCustomerReport[0];
                            DR["نام مشترک"] = item2.CustomerName;
                            DR["نام خانوادگی"] = item2.Customerfamily;
                            DR["شماره پرونده"] = item2.DossierNumber;
                            DR["دشت"] = item2.PlainName;
                            DR["شماره کنتور"] = MeterNumber[i];
                            ExcelDT.Columns.Add("");
                            switch (item2.TypeOfUse)
                            {
                                case 0:
                                    DR["نوع مصرف"] = "کشاورزی";
                                    break;
                                case 1:
                                    DR["نوع مصرف"] = "صنعتی";
                                    break;
                                case 2:
                                    DR["نوع مصرف"] = "شرب";
                                    break;
                                default:
                                    DR["نوع مصرف"] = "غیره";
                                    break;
                            }
                        }

                        ExcelDT.Rows.Add(DR);
                        if (DR1 != null)
                            ExcelDT.Rows.Add(DR1);
                    }
                    catch (Exception ex)
                    {
 
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
            }
        }




        public MeetrsStatus CreateListOfStatusValue(string OBISs, string MeterID, string MeterNumber, string ReadDateString)
        {
            MeetrsStatus MS = new MeetrsStatus();
            try
            {
                string Value1 = "";
                string Value2 = "";
                string Value3 = "";
                string[] OBIDS;
                OBIDS = OBISs.Split(',');
                _303 Obj303 = new _303();
                List<Status_Result> List = new List<Status_Result>();
                // for(int i = 0; i < MeterNumber.Count; i++)               
                //{ 
                if (!MeterNumber.StartsWith("207"))
                {
                    if (OBISs.Contains("0000603F00FF") || OBISs.Contains("0000603F01FF") || OBISs.Contains("0000603F02FF"))
                    {
                        try
                        {
                            Obj303 = new _303();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();
                            //string filter = " and Header.MeterID in (" + MeterID + ")  and OBISs.OBIS in ('0000603F00FF','0000603F01FF','0000603F02FF') " + ReadDateString;

                            //ShowReportOBISValueDetail reportValueDetailes = new ShowReportOBISValueDetail(filter,CommonData.LanguagesID);
                            Value1 = RefreshDatagridMeterStatus("0000603F00FF", MeterID, ReadDateString);
                            Value2 = RefreshDatagridMeterStatus("0000603F01FF", MeterID, ReadDateString);
                            Value3 = RefreshDatagridMeterStatus("0000603F02FF", MeterID, ReadDateString);

                            List = Obj303.PerformanceMeteroncreditevents(Value1.PadLeft(8, '0'), Value2.PadLeft(8, '0'), Value3.PadLeft(8, '0'), CommonData.LanguageName);
                            sv.StatusDesc = tr.TranslateofLable.Object49;
                            sv.List = List;
                            if (sv.List.Count > 1)
                                // if(sv.List[0].IsStatuseTrue!=Status.False || sv.List[1].IsStatuseTrue != Status.False)
                                MS.MeetrsList.Add(sv);
                        }
                        catch { }
                    }
                    if (OBISs.Contains("0000600A03FF"))
                    {
                        try
                        {
                            string Value = "";
                            Obj303 = new _303();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();
                            Value = RefreshDatagridMeterStatus("0000600A03FF", MeterID, ReadDateString);
                            if (Value != "")
                            {
                                List = Obj303.StatuseGeneralRegister3(Value.PadLeft(8, '0'), CommonData.LanguageName);
                                sv.StatusDesc = tr.TranslateofLable.Object48;
                                sv.List = List;

                                MS.MeetrsList.Add(sv);
                            }
                        }
                        catch { }
                    }
                    if (OBISs.Contains("0000600A02FF"))
                    {
                        try
                        {
                            string Value = "";
                            Obj303 = new _303();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();
                            Value = RefreshDatagridMeterStatus("0000600A02FF", MeterID, ReadDateString);
                            if (Value != "")
                            {
                                List = Obj303.StatuseGeneralRegister2(Value.PadLeft(8, '0'), CommonData.LanguageName, 3);
                                sv.StatusDesc = tr.TranslateofLable.Object47;
                                sv.List = List;
                                MS.MeetrsList.Add(sv);
                            }
                        }
                        catch
                        {
                        }


                    }
                    if (OBISs.Contains("0000600A02FF"))
                    {
                        try
                        {
                            string Value = "";
                            Obj303 = new _303();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();
                            Value = RefreshDatagridMeterStatus("0000600A02FF", MeterID, ReadDateString);
                            if (Value != "")
                            {
                                List = Obj303.StatuseGeneralRegister2(Value.PadLeft(8, '0'), CommonData.LanguageName, 1);
                                sv.StatusDesc = tr.TranslateofLable.Object46;
                                sv.List = List;
                                MS.MeetrsList.Add(sv);
                            }
                        }
                        catch { }
                    }
                    if (OBISs.Contains("0000603E04FF"))
                    {
                        try
                        {
                            string Value = "";
                            Obj303 = new _303();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();
                            Value = RefreshDatagridMeterStatus("0000603E04FF", MeterID, ReadDateString);
                            if (Value != "")
                            {
                                List = Obj303.statusRegister("0000603E04FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                                sv.StatusDesc = tr.TranslateofLable.Object50;
                                sv.List = List;
                                MS.MeetrsList.Add(sv);
                            }
                        }
                        catch { }
                    }
                }
                if (MeterNumber.StartsWith("207"))

                    if (OBISs.Contains("0802606101FF"))
                    {
                        try
                        {
                            string Value11 = "", Value21 = "";
                            _207 Obj207 = new _207();
                            List = new List<Status_Result>();
                            Statusvalue sv = new Statusvalue();

                            Value11 = RefreshDatagridMeterStatus("0802606101FF", MeterID, ReadDateString);
                            Value21 = RefreshDatagridMeterStatus("0000603E01FF", MeterID, ReadDateString);
                            Value11 = Value11.Trim();
                            List = Obj207.StatusRegister207_0802606101FF(Value21, Value11, CommonData.LanguageName);
                            sv.StatusDesc = tr.TranslateofLable.Object50;
                            sv.List = List;
                            MS.MeetrsList.Add(sv);
                        }
                        catch { }
                    }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
            return MS;
        }

        public void ExportDataToExcelwithReadOutDate(string filePath)
        {
            try
            {
                //PleaseWateLabel.Visibility = Visibility.Visible;

                Application myExcel = new Application();
                if (myExcel == null)
                {
                    MessageBox.Show("Excel is not properly installed!!");
                    return;
                }
                GetReadoutDateForExcelFile();  // fill ReadDate
                bool IsDateTime = false;
                object missing = Type.Missing;
                Workbook wb = myExcel.Workbooks.Add(missing);
                Worksheet ws = (Worksheet)wb.ActiveSheet;
                Range rng = ws.get_Range("A1", missing); ;
                //rng.Style.VerticalAlignment = VerticalAlignment.Center;
                myExcel.Visible = false;
                ws = (Worksheet)wb.Sheets[1]; // Explicit cast is not required here



                // Change Direction Excel
                myExcel.DefaultSheetDirection = (int)Constants.xlRTL;//.xlLTR; //or xlRTL

                // fill Row 1
                bool IsCustomerAdd = false;
                string ISOBISIDS = GetOBIssInOBIStype();
                for (int d = 0; d < OBISType.Length; d++)
                {
                    if (OBISType[d] == "12")
                        IsCustomerAdd = true;
                }

                int FillColumns = 1;
                string Exception = "";
                if (selectedreport.ReportID == 2)
                    Exception = " and Main.OBISID<>81";

                string Filter = " and Main.ReportID= " + selectedreport.ReportID + "and Main.OBIstypeid !=12" + Exception + " order by obis ASC";
                ShowObisToExcel ShowOBISToExcel = new ShowObisToExcel(Filter);
                ExcelDT.Columns.Clear();
                try
                {
                    if (IsCustomerAdd)
                    {
                        ExcelDT.Columns.Add("نام مشترک");
                        ExcelDT.Columns.Add("نام خانوادگی");
                        ExcelDT.Columns.Add("شماره پرونده");
                        ExcelDT.Columns.Add("نوع مصرف");
                        ExcelDT.Columns.Add("دشت");
                        ExcelDT.Columns.Add("شماره کنتور");
                    }
                    ExcelDT.Columns.Add("منبع قرائت");
                    foreach (var itemC in ShowOBISToExcel._lstShowObisToExcel)
                    {
                        try
                        {
                            if (!itemC.ObisFarsiDesc.Contains("status") && !itemC.ObisFarsiDesc.Contains("credit" ) && !itemC.ObisFarsiDesc.Contains("شماره کنتور") )
                                
                                ExcelDT.Columns.Add(itemC.ObisFarsiDesc);
                        }
                        catch { }
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString().ToString());
                    //PleaseWateLabel.Visibility = Visibility.Hidden;
                }
                CommonData.mainwindow.changeProgressBarValue(2);
                ShowConsumedWaterPivot value;
                ShowCustomerReport CustomerReport;
                ShowReportObisValueDetail det;
                // get data from database and insert to Excel               
                List<ShowVeeConsumedWater_Result> lst8 = new List<ShowVeeConsumedWater_Result>();
                List<ShowReportOBISValueDetails_Result> lstothers = new List<ShowReportOBISValueDetails_Result>();
                int numOthers = 0;
                List<ShowCustomerReport_Result> lstCustomer = new List<ShowCustomerReport_Result>();
                int RowNum = 0, ColNumber = FillColumns;
                string OBISIDS = GetOBIssInOBIStype();
                GetMeterNumberForExcelFile();
                string Meterid = MeterID[0];
                string Meternumber = MeterNumber[0];
                int[] usedRows = new int[OBISType.Length];
                //GetReadoutDateForExcelFile();  // fill ReadDate
                rng = ws.UsedRange;


                

                for (int ReadM = 0; ReadM < ReadDate.Length; ReadM++)
                {
                    var showVeeConsumedWater = false;
                    IsDateTime = true;
                    RowNum++;
                    if (ReadM > 0 && (ReadM % 2) != 0)
                        ws.Rows[RowNum].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                    else
                        ws.Rows[RowNum].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);


                    for (int cul = 0; cul < OBISType.Length; cul++)
                    {
                        if (OBISType[cul] == "12")
                        {
                            string filter = " and Meter.MeterID = " + Meterid;
                            CustomerReport = new ShowCustomerReport(filter);
                            for (int g = 0; g < CustomerReport._lstShowCustomerReport.Count; g++)
                            {
                                lstCustomer.Insert(g, CustomerReport._lstShowCustomerReport[g]);
                            }
                        }

                        else if (OBISType[cul] == "8")
                        {
                            showVeeConsumedWater = false;
                            // آب مصرفی

                            // Filter = " and Main.MeterID=" + Meterid + " and OBISS.obisid in ( " + OBISIDS + " )";
                            // value = new ShowConsumedWaterPivot(Filter, "");
                            //for (int j = 0; j < value._lstShowConsumedWaterPivot.Count; j++)
                            //{
                            //    lst8.Insert(j, value._[j]);
                            //}


                            if (lst8.Count < 1)
                            {
                                showVeeConsumedWater = true;
                                ShowVeeConsumedWater value1 = new ShowVeeConsumedWater(Convert.ToDecimal( MeterID[0]));
                                for (int j = 0; j < value1._lstShowVeeConsumedWater.Count; j++)
                                {
                                    lst8.Insert(j, value1._lstShowVeeConsumedWater[j]);
                                }

                            }


                        }
                        else if (OBISType[cul] == "2")
                            ISCul2Report = true;


                        else if (OBISType[cul] != "8" && OBISType[cul] != "2" && OBISType[cul] != "12")
                        {
                            Filter = "  and (charindex('\"" + OBISType[cul] + "\"',obiss.type)>0) and Header.MeterID= " + Meterid + " and OBISs.OBISID in (" + OBISIDS + " ) and Header.ReadDate='" + ReadDate[ReadM] + "'";
                            det = new ShowReportObisValueDetail(Filter, CommonData.LanguagesID);

                            for (int i = 0; i < det._lstShowReportOBISValueDetail.Count; i++)
                            {
                                lstothers.Insert(numOthers, det._lstShowReportOBISValueDetail[i]);
                                numOthers++;

                            }


                        }

                    }
                    CommonData.mainwindow.changeProgressBarValue(2);
                    FillExcelForAllSelectedMeters(lstothers, showVeeConsumedWater, lstCustomer, OBISIDS, ReadDate[ReadM], lst8);

                    /* ahmad
                     for (int i = 0; i < ExcelDT.Columns.Count; i++)
                    {
                        ws.Cells[1, (i + 1)] = ExcelDT.Columns[i].ColumnName;
                        // setBorderOfCells(1, (i + 1), ws);
                        //ws.Cells[1, (i + 1)].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gainsboro);
                        //ws.Cells[1, (i + 1)].font.bold = true;
                        // ws.Columns.AutoFit();
                    }

                    for (int i = 0; i < ExcelDT.Rows.Count; i++)
                    {
                        // to do: format datetime values before printing
                        for (int j = 0; j < ExcelDT.Columns.Count; j++)
                        {
                            ws.Cells[(i + 2), (j + 1)] = ExcelDT.Rows[i][j];
                            var s = ExcelDT.Rows[i][j].ToString();

                            ws.Cells[(i + 2), (j + 1)].Style.Numberformat = "@";

                        }
                        if ((i + 1) > 0 && ((i + 1) % 2) != 0)
                            ws.Rows[(i + 2)].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                        else
                            ws.Rows[(i + 2)].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);
                        rng = ws.UsedRange;
                        // rng.BorderAround(Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin);
                    }
                     */


                }
                for (int i = 0; i < ExcelDT.Columns.Count; i++)
                {
                    ws.Cells[1, (i + 1)] = ExcelDT.Columns[i].ColumnName;
                    // setBorderOfCells(1, (i + 1), ws);
                    //ws.Cells[1, (i + 1)].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gainsboro);
                    //ws.Cells[1, (i + 1)].font.bold = true;
                    // ws.Columns.AutoFit();
                }

                for (int i = 0; i < ExcelDT.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < ExcelDT.Columns.Count; j++)
                    {
                        ws.Cells[(i + 2), (j + 1)] = ExcelDT.Rows[i][j];
                        var s = ExcelDT.Rows[i][j].ToString();

                        ws.Cells[(i + 2), (j + 1)].Style.Numberformat = "@";

                    }
                    if ((i + 1) > 0 && ((i + 1) % 2) != 0)
                        ws.Rows[(i + 2)].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                    else
                        ws.Rows[(i + 2)].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);
                    rng = ws.UsedRange;
                    // rng.BorderAround(Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin);
                }
                ExcelDT.Clear();

                // set Culomn ws 
                ws.UsedRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ws.UsedRange.Borders.Weight = XlBorderWeight.xlMedium;
                ws.UsedRange.Borders.LineStyle = XlLineStyle.xlContinuous;
                ws.UsedRange.Columns.AutoFit();
                ws.Rows[1].font.bold = true;
                ws.Rows[1].Interior.Color = ColorTranslator.ToOle(Color.Gainsboro);
                //freez first column
                ws.Application.ActiveWindow.SplitRow = 1;
                ws.Application.ActiveWindow.FreezePanes = true;

                //wb.SaveAs(filePath, AccessMode: Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared);
                wb.SaveAs(filePath, XlFileFormat.xlWorkbookDefault, missing, missing, missing, missing,
                      XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                wb.Close(true, missing, missing);

                myExcel.Quit();

                //MyExcel.Application.Workbooks.Open(filePath, 0, false, 5, "", "", false,
                //Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);
                //System.Diagnostics.Process.Start(filePath);
                MessageBox.Show("گزارش به صورت فایل اکسل در آدرس تعیین شده ذخیره شد");

            }
            catch (Exception ex)
            {
                //PleaseWateLabel.Visibility = Visibility.Hidden;
                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
            //PleaseWateLabel.Visibility = Visibility.Hidden;
            CommonData.mainwindow.changeProgressBarTag("");
            CommonData.mainwindow.changeProgressBarValue(0);
        }



        public string RefreshDatagridMeterStatus(string OBIS, string MeterID, string ReaDateString)
        {
            try
            {
                string Value = "";
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                string Filter = " and Header.MeterID=" + MeterID + "  and OBISs.OBIS='" + OBIS + "' " + ReaDateString;
                ShowObisValueDetail EGeneralvalue = new ShowObisValueDetail(Filter, CommonData.LanguagesID);
                foreach (ShowOBISValueDetail_Result item in Bank.ShowOBISValueDetail(Filter, CommonData.LanguagesID, CommonData.UserID))
                    Value = item.Value;
                Bank.Database.Connection.Close();
                return Value;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                CommonData.WriteLOG(ex);
                return "";
            }
        }
        private string GetOBIssInOBIStype()
        {
            try
            {
                OBISs = "";
                string Filter = " and Main.ReportID= " + selectedreport.ReportID;
                ShowObisTypeToReport OBISTypeToReport = new ShowObisTypeToReport(Filter);
                ShowObisToReport OBISToReport = new ShowObisToReport(Filter);
                OBISType = new string[OBISTypeToReport._lstShowOBISTypeToReport.Count];
                SheetsName = new string[OBISTypeToReport._lstShowOBISTypeToReport.Count];
                string OBISIDs = "";
                bool addCustomerInfo = false;
                for (int i = 0; i < OBISTypeToReport._lstShowOBISTypeToReport.Count; i++)
                {
                    ShowOBISTypeToReport_Result item = new ShowOBISTypeToReport_Result();
                    item = OBISTypeToReport._lstShowOBISTypeToReport[i];
                    OBISType[i] = item.OBISTypeID.ToString();
                    if (OBISType[i] == "12")
                        addCustomerInfo = true;
                    //  SheetsName[i] = item.OBISTypeDesc.ToString();
                }
                if (!addCustomerInfo)
                { var obistypeTemp = new string[OBISType.Length + 1];
                    OBISType.CopyTo(obistypeTemp, 0);
                    obistypeTemp[obistypeTemp.Length - 1] = "12";
                    OBISType = obistypeTemp;
                }
                for (int i = 0; i < OBISToReport._lstShowOBISToReport.Count; i++)
                {
                    ShowOBISToReport_Result item = new ShowOBISToReport_Result();
                    item = OBISToReport._lstShowOBISToReport[i];
                    OBISIDs = OBISIDs + item.OBISID.ToString() + ",";
                    OBISs = OBISs + item.Obis.ToString() + ",";
                }
                
                if (OBISIDs.Length > 1)
                {
                    OBISIDs = OBISIDs.Substring(0, OBISIDs.Length - 1);

                }
                if (OBISs.Length > 1)
                {
                    OBISs = OBISs.Substring(0, OBISs.Length - 1);

                }
                //if (OBISIDs.Contains(",81"))
                //{
                //    OBISIDs = OBISIDs.Replace(",81", "");
                //}
                return OBISIDs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
                return "";
            }
        }
        public void GetMeterNumberForExcelFile()
        {
            try
            {
                MeterID = new List<string>();
                CustomerIds = new List<decimal?>();
                MeterNumber = new List<string>();
                MaxReadDate = new List<string>();
                SourceTypeName = new List<string>();
                if (CommonData.MainList.Count > 0)//.mainwindow.MeterGrid.Items.Count > 0)
                {
                    for (int i = 0; i < CommonData.MainList.Count; i++)//CommonData.mainwindow.MeterGrid.Items.Count; i++)
                    {
                        SelectedMeter selectedMeter = new SelectedMeter();
                        selectedMeter = (SelectedMeter)CommonData.MainList[i];//CommonData.mainwindow.MeterGrid.Items[i];
                        if (selectedMeter.Isvisable)
                        {
                            if (selectedMeter.CustomerId == null || selectedMeter.CustomerId == 0)
                            {
                                selectedMeter.CustomerId = 0;
                            }

                            MeterID.Add(selectedMeter.MeterId.ToString());
                            CustomerIds.Add(selectedMeter.CustomerId);
                            MeterNumber.Add(selectedMeter.MeterNumber.ToString());
                            SourceTypeName.Add(selectedMeter.SourceTypeName.ToString());
                            ObjectParameter MAXReadDate = new ObjectParameter("MaxReadDate", "");
                            SQLSPS.ShowMaxReadDate(selectedMeter.MeterId, MAXReadDate);
                            MaxReadDate.Add(MAXReadDate.Value.ToString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void GetReadoutDateForExcelFile()
        {
            ReadDateGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                 delegate ()
                 {


                     if (ReadDateGrid.SelectedItems.Count > 0)
                     {
                         ReadDate = new string[ReadDateGrid.SelectedItems.Count];
                         SourceTypeName = new List<string>();
                         for (int i = 0; i < ReadDateGrid.SelectedItems.Count; i++)
                         {
                             ShowOBISValueHeader_Result selectedReadDate = new ShowOBISValueHeader_Result();
                             selectedReadDate = (ShowOBISValueHeader_Result)ReadDateGrid.SelectedItems[i];
                             ReadDate[i] = selectedReadDate.ReadDate.ToString();
                             SourceTypeName.Add(selectedReadDate.SourceTypeName.ToString());

                         }
                     }

                 }));
        }
        public void GetDataForExcelFilewithmultiReadOut(string filepath)
        {
            try
            {
                string MeterID = null;
                string MeterNumber = null;
                string[] ReadDate = null;
                if (CommonData.mainwindow.MeterGrid.SelectedItems.Count > 0)
                {
                    ReadDate = new string[ReadDateGrid.SelectedItems.Count];
                    SelectedMeter selectedMeter = new SelectedMeter();
                    selectedMeter = (SelectedMeter)CommonData.mainwindow.MeterGrid.SelectedItem;
                    MeterID = selectedMeter.MeterId.ToString();
                    MeterNumber = selectedMeter.MeterNumber.ToString();
                    for (int i = 0; i < ReadDateGrid.SelectedItems.Count; i++)
                    {

                        ShowOBISValueHeader_Result SelectedReadout = new ShowOBISValueHeader_Result();
                        SelectedReadout = (ShowOBISValueHeader_Result)ReadDateGrid.SelectedItems[i];
                        ReadDate[i] = SelectedReadout.ReadDate.ToString();

                    }
                    string Filter = " and Main.ReportID= " + selectedreport.ReportID;
                    ShowObisTypeToReport OBISTypeToReport = new ShowObisTypeToReport(Filter);
                    ShowObisToReport OBISToReport = new ShowObisToReport(Filter);
                    string[] OBISType = new string[OBISTypeToReport._lstShowOBISTypeToReport.Count];
                    string[] SheetsName = new string[OBISTypeToReport._lstShowOBISTypeToReport.Count];
                    string OBISIDs = "";
                    for (int i = 0; i < OBISTypeToReport._lstShowOBISTypeToReport.Count; i++)
                    {
                        ShowOBISTypeToReport_Result item = new ShowOBISTypeToReport_Result();
                        item = OBISTypeToReport._lstShowOBISTypeToReport[i];
                        OBISType[i] = item.OBISTypeID.ToString();
                        SheetsName[i] = item.OBISTypeDesc.ToString();
                    }
                    for (int i = 0; i < OBISToReport._lstShowOBISToReport.Count; i++)
                    {
                        ShowOBISToReport_Result item = new ShowOBISToReport_Result();
                        item = OBISToReport._lstShowOBISToReport[i];
                        OBISIDs = OBISIDs + item.OBISID.ToString() + ",";
                    }
                    OBISIDs = OBISIDs.Substring(0, OBISIDs.Length - 1);
                    ExcelSerializer EXserializer = new ExcelSerializer();
                    EXserializer.CopyWorkSheetConsumedwithReadDate(filepath, SheetsName, OBISType, MeterNumber, "", MeterID, OBISIDs, ReadDate);

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }

        }

        public void changeHeaderReadDateGrid()
        {
            try
            {
                ReadDateGrid.Columns[0].Header = tr.TranslateofLable.Object6.ToString();
                ReadDateGrid.Columns[1].Header = tr.TranslateofLable.Object7.ToString();
                ReadDateGrid.Columns[2].Header = tr.TranslateofLable.Object8.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
        }
        private void MeterGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string Filter = "";
                SelectedMeter selectedmeter = new SelectedMeter();
                selectedmeter = (SelectedMeter)CommonData.mainwindow.MeterGrid.SelectedItem;
                Filter = " and Main.MeterID=" + selectedmeter.MeterId.ToString();
                RefreshSelectedMeters(Filter);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void MeterGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ReadDateGrid.SelectedIndex = -1;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonData.mainwindow.Excel(sender, null);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }

        }

        private void GridMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                CommonData.mainwindow.Excel(sender, null);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MeterNumber.Count == 0)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message58);
                    this.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void datePickerStart_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            SetDate();
            if (MeterNumber.Count == 1)
            {
                // Add One Day
                string Add1Day = datePickerEnd.DisplayDate.AddDays(1).ToString();
                Add1Day = Add1Day.Substring(Add1Day.LastIndexOf('/') + 1);
                string EndDate = enddate.ToString().Remove(8, 2);
                EndDate = EndDate + Add1Day;
                //
                string filter = "  and Main.MeterID=" + MeterID[0].ToString();
                if (CommonData.LanguagesID == 2)
                    filter = filter + " and Main.ReadDate Between '" + startdate.ToString().Substring(0, 10) + "' and '" + enddate.ToString().Substring(0, 10) + "'";
                RefreshSelectedMeters(filter);
            }
        }
        public void SetDate()
        {
            try
            {
                if (CommonData.LanguagesID == 2)
                {
                    startdate = datePickerStart.Text;
                    //datePickerEnd.DisplayDate.AddDays(1);
                    enddate = datePickerEnd.Text;
                    int day = Convert.ToInt32(startdate.Substring(startdate.LastIndexOf('/') + 1, startdate.Length - startdate.LastIndexOf('/') - 1));
                    int Month = Convert.ToInt32(startdate.Substring(startdate.IndexOf('/') + 1, startdate.LastIndexOf('/') - startdate.IndexOf('/') - 1));
                    if (day < 10 && startdate.Length < 10)

                        startdate = startdate.Substring(0, startdate.Length - 1) + "0" + day;

                    if (Month < 10 && startdate.Length < 10)
                        startdate = startdate.Substring(0, startdate.IndexOf('/') + 1) + "0" + startdate.Substring(startdate.IndexOf('/') + 1, startdate.Length - startdate.IndexOf('/') - 1);
                    day = Convert.ToInt32(enddate.Substring(enddate.LastIndexOf('/') + 1, enddate.Length - enddate.LastIndexOf('/') - 1)) + 1;
                    Month = Convert.ToInt32(enddate.Substring(enddate.IndexOf('/') + 1, enddate.LastIndexOf('/') - enddate.IndexOf('/') - 1));
                    if (day < 10)

                        enddate = enddate.Substring(0, enddate.Length - 1) + "0" + day;

                    if (Month < 10)
                        enddate = enddate.Substring(0, enddate.IndexOf('/') + 1) + "0" + enddate.Substring(enddate.IndexOf('/') + 1, enddate.Length - enddate.IndexOf('/') - 1);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void BtnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadDateGrid.SelectAll();
                //if (ReadDateGrid.Items.Count > 0)
                //{
                //    for (int i = 0; i < ReadDateGrid.Items.Count; i++)
                //    { 
                //       DataGridRow row = (DataGridRow)ReadDateGrid.ItemContainerGenerator.ContainerFromIndex(i);
                //        if (row != null)
                //            row.IsSelected = true;
                //        else
                //        {

                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void datePickerStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            datePickerStart.Focus();
            datePickerStart.Focus();
        }

        private void datePickerEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            datePickerEnd.Focus();
            datePickerEnd.Focus();
        }
    }
}
