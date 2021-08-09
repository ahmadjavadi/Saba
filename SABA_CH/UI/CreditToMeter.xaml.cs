using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;
using Window = System.Windows.Window;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for CreditToMeter.xaml
    /// </summary>
    public partial class CreditToMeter : System.Windows.Window
    {     
       
        public ShowTranslateofLable tr = null;
        public readonly int windowID = 14;
        private TabControl tabCtrl;
        private TabItem tabPag;
        string startdate = "";
        string enddate = "";
        ShowCredit303 showCredit;
        bool IsOneMeter = false, IsRun = false;
        bool IsNecessary = false;
        bool IsnotNecessary = false;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
        public CreditToMeter()
        {
            InitializeComponent();
            tr = CommonData.translateWindow(windowID);
            CreditGrid1.DataContext = tr.TranslateofLable;
            DGAllMeres.ItemsSource = null;
            CreditGrid.ItemsSource = null;
            ChangeVisability();
            TranslateGrids();
            //MGrid.FlowDirection = CommonData.FlowDirection;           
            RefreshCreditGrid();
        
        }
        public void ChangeVisability()
        {
          
            if (this.Title.Contains("1") || this.Title.Contains("2"))
            {
                IsOneMeter = true;
                btnSearch.Visibility = Visibility.Hidden;
                GrBoxDate.IsEnabled = false;
                GrBoxKind.IsEnabled = false;
                datePickerStart.IsEnabled = false;
                datePickerEnd.IsEnabled = false;
                decimal GroupID = 0;
                if (CommonData.selectedGroup.GroupName != "همه گروه ها")
                    GroupID = CommonData.selectedGroup.GroupID;
                if (DGAllMeres.Items.Count == 0)
                {
                   // ShowAllCreditTokenWithNecessary showAllCreditToken = new ShowAllCreditTokenWithNecessary("01/01/1800", "12/12/4000",CommonData.UserID,GroupID, CommonData.selectedGroup.GroupType,false,false,false);
                    ShowAllCreditTokenWithNecessary_Result ShowOneCreditToken = new ShowAllCreditTokenWithNecessary_Result();
                bool IsFull = false;
                string Filter= " and Main.MeterID=" + CommonData.SelectedMeterID+" order by BuildDate desc";
                ShowToken showtoken = new ShowToken(Filter);
               
                    foreach (var item0 in showtoken._lstShowToken)
                    {
                        ShowOneCreditToken.CreditValue += item0.CreditValue;
                        ShowOneCreditToken.CustomerName = item0.CustomerName;
                        ShowOneCreditToken.MeterID = item0.MeterID;
                        ShowOneCreditToken.MeterNumber = item0.MeterNumber;
                        //ShowOneCreditToken.Value = item0.CreditValue.ToString();
                        ShowOneCreditToken.WatersubscriptionNumber = item0.WatersubscriptionNumber;
                        IsFull = true;
                        CreditGrid.ItemsSource = null;
                    }

                    if (IsFull)
                    {

                        //ShowConsumedWaterPivot consumedWaterobj = new ShowConsumedWaterPivot(" and main.meterid=" + CommonData.SelectedMeterID, "");

                        //if (consumedWaterobj._lstShowConsumedWaterPivot.Count != 0)
                        //    ShowOneCreditToken.Value = consumedWaterobj._lstShowConsumedWaterPivot[0].WT.ToString();
                        //else
                        //    ShowOneCreditToken.Value = "0";
                        ShowConsumedWater oconsumedWater = new ShowConsumedWater(" and main.meterid=" + CommonData.SelectedMeterID + " and obiss.obisid=83");
                        if (ShowOneCreditToken.Value == null)
                            ShowOneCreditToken.Value = "0";

                        foreach (var item in oconsumedWater._lstShowConsumedWaters)
                        {
                            if (NumericConverter.DoubleConverter(ShowOneCreditToken.Value) < NumericConverter.DoubleConverter(item.ConsumedWater))
                                ShowOneCreditToken.Value = item.ConsumedWater;
                        }
                        
                        DGAllMeres.Items.Add(ShowOneCreditToken);
                        if (DGAllMeres.Items != null)
                            DGAllMeres.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                IsOneMeter = false;
                txtSerialNumber207.Text = "";
                txtSerialNumber.Text = "";
                GrBoxDate.IsEnabled = true;
                GrBoxKind.IsEnabled = true;
                datePickerStart.IsEnabled = true;
                datePickerEnd.IsEnabled = true;
                btnSearch.Visibility = Visibility.Visible;
            }
            if (CommonData.SelectedMeterNumber.StartsWith("207"))
            {
                WaterCreditSettingGrid207.Visibility = Visibility.Visible;
                WaterCreditSettingGrid.Visibility = Visibility.Hidden;
                txtSerialNumber207.Text = CommonData.MeterNumber;
                txtEnd.Visibility = Visibility.Hidden;
                txtStart303.Visibility = Visibility.Hidden;
                txtstartTime.Visibility = Visibility.Hidden;
                txtendTime.Visibility = Visibility.Hidden;
            }
            else
            {
                WaterCreditSettingGrid207.Visibility = Visibility.Hidden;
                WaterCreditSettingGrid.Visibility = Visibility.Visible;
                txtSerialNumber.Text = CommonData.MeterNumber;
                txtEnd.Visibility = Visibility.Visible;
                txtStart303.Visibility = Visibility.Visible;
                txtstartTime.Visibility = Visibility.Visible;
                txtendTime.Visibility = Visibility.Visible;

            }
        }
        private void TranslateGrids()
        {
            try
            {
                //CreditGrid.Columns[0].Header = tr.TranslateofLable.Object4;               
                //CreditGrid.Columns[1].Header = tr.TranslateofLable.Object5;
                //CreditGrid.Columns[2].Header = tr.TranslateofLable.Object6;
                //CreditGrid.Columns[3].Header = tr.TranslateofLable.Object7;
                //CreditGrid.Columns[4].Header = tr.TranslateofLable.Object50;
                //CreditGrid.Columns[5].Header = tr.TranslateofLable.Object51;
                //DGAllMeres.Columns[0].Header = tr.TranslateofLable.Object1;
                //DGAllMeres.Columns[1].Header = tr.TranslateofLable.Object47;
                //DGAllMeres.Columns[2].Header = tr.TranslateofLable.Object2;
                //DGAllMeres.Columns[3].Header = tr.TranslateofLable.Object49;
                datePickerStarten.SelectedDate = DateTime.Now;
                datePickerEnden.SelectedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void CreditVaues()
        {


        }
        
        public void RefreshCreditGrid( )
        {
            try
            {

                 string Filter = " and Main.MeterID=" + CommonData.SelectedMeterID + "order by BuildDate desc";
                ShowToken showtoken = new ShowToken(Filter);
                //txtSerialNumber207.Text =CommonData.SelectedMeterNumber;

                CreditGrid.ItemsSource = showtoken._lstShowToken;

                if (showtoken._lstShowToken.Count > 0 && CommonData.MeterNumber.StartsWith("19"))
                    CreditGrid.SelectedIndex = 0;

                else if (showtoken._lstShowToken.Count > 0 && CommonData.MeterNumber.StartsWith("207"))
                    CreditGrid.SelectedIndex = 0;
                else
                    CleanCheckboxes();

                if (!CommonData.SelectedMeterNumber.StartsWith("207"))
                {
                    Filter = " and MeterID=" + CommonData.SelectedMeterID; // + " order by IDcredit desc" ;
                    showCredit = new ShowCredit303(Filter);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                CommonData.WriteLOG(ex);
            }
        }        
        private void CleanCheckboxes()
        {
            try
            {
                if (CommonData.SelectedMeterNumber.StartsWith("207"))
                {
                    txtSerialNumber207.Text = CommonData.MeterNumber;
                    txtWaterCredit207.Text = "";
                    txtEnd207.Text = "";
                    chk3207.IsChecked = false;
                    chk1207.IsChecked = false;
                    chk2207.IsChecked = false;
                    chk4207.IsChecked = false;
                }
                else
                {
                    txtSerialNumber.Text = CommonData.MeterNumber;
                    txtWaterCredit.Text = "";

                    txtEnd.Text = "";
                    txtendTime.Text = "";
                    txtstartTime.Text = "";
                    txtStart303.Text = "";
                    chk1.IsChecked = false;
                    chk2.IsChecked = false;
                    chk3.IsChecked = false;
                    chk4.IsChecked = false;
                    chk5.IsChecked = false;
                    chk10.IsChecked = false;
                    chk20.IsChecked = false;
                    chk30.IsChecked = false;
                    CreditDate.Text = "";
                    CreditDate.IsReadOnly = true;
                    // version4
                    chk10.IsChecked = false;
                    Active1.Visibility = Visibility.Hidden;
                    CreditDate.Visibility = Visibility.Hidden;
                    Active1.Visibility = Visibility.Hidden;
                    //
                    chk20.IsChecked = false;
                    Active2.Visibility = Visibility.Hidden;
                    chk30.IsChecked = false;
                    Active3.Visibility = Visibility.Hidden;
                    //

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
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
                RefreshCreditGrid();
                ChangeVisability();
                if (DGAllMeres.Items.Count<1)
                {
                    CleanCheckboxes();
                    CreditGrid.ItemsSource = null;
                    txtSerialNumber.Text = "";
                    txtSerialNumber207.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
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
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void CreditGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectGrid();
        }
        public void SelectGrid() 
        {
            try
            {
                DateTime newstartdate;
                DateTime newenddate;                
                ShowToken_Result selectedRow = new ShowToken_Result();
                ShowCredit303_Result credit303bool = new ShowCredit303_Result();
              
                string[] checkedv = new string[5];
                string checkedvalue = "";
               
             
                if (CreditGrid.SelectedItem != null)
                {
                    selectedRow = (ShowToken_Result)CreditGrid.SelectedItem;
                    //credit303bool =(ShowCredit303_Result)CreditGrid.SelectedItem;
                    //try
                    //{
                    //    credit303bool = (showCredit._lstShowCredit303)[CreditGrid.SelectedIndex];
                    //}
                    //catch { }

                    try
                    {
                        if (!CommonData.SelectedMeterNumber.StartsWith("207"))
                            newstartdate = Convert.ToDateTime(selectedRow.StartDate);                     
                        
                    }
                    catch
                    {
                        // DateTime date1 = Persia.Calendar.ConvertToGregorian(1384, 3, 18, DateType.Persian);
                        //newstartdate = DateTime.ParseExact(selectedRow.StartDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        PersianCalendar p = new PersianCalendar();
                        string[] d = selectedRow.StartDate.Split('/');
                        d[2] = d[2].Substring(0, 2);
                        newstartdate = p.ToDateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), 0, 0, 0, 0);
                        newstartdate = Convert.ToDateTime(newstartdate);
                    }

                    try
                    {
                        if(selectedRow.EndDate!="")
                        newenddate = Convert.ToDateTime(selectedRow.EndDate);
                        
                    }
                    catch 
                    {
                        //newenddate = DateTime.ParseExact(selectedRow.EndDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                        PersianCalendar p = new PersianCalendar();
                        string[] d = selectedRow.EndDate.Split('/');
                        d[2] = d[2].Substring(0, 2);
                        newenddate = p.ToDateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), 0, 0, 0, 0);
                        newenddate = Convert.ToDateTime(newenddate);
                    }


                    checkedvalue = selectedRow.CreditTransferModes;
                    checkedvalue = Convert.ToString(Convert.ToInt16(checkedvalue, 16), 2).PadLeft(5, '0');

                    for (int i = 0; i < 5; i++)
                    {
                        checkedv[i] = checkedvalue.Substring(i, 1);
                    }
                    if (CommonData.SelectedMeterNumber.StartsWith("207"))
                    {
                        txtSerialNumber207.Text = CommonData.MeterNumber;
                        txtWaterCredit207.Text = selectedRow.CreditValue.ToString();
                        txtEnd207.IsReadOnly = false; 
                        txtEnd207.Text = selectedRow.EndDate.ToString();
                        txtEnd207.IsReadOnly = true;
                        chk3207.IsChecked = Convert.ToBoolean(Convert.ToInt16(checkedv[1]));
                        chk1207.IsChecked = Convert.ToBoolean(Convert.ToInt16(checkedv[2]));
                        chk2207.IsChecked = Convert.ToBoolean(Convert.ToInt16(checkedv[3]));
                        chk4207.IsChecked = Convert.ToBoolean(Convert.ToInt16(checkedv[4]));
                        // umite all 
                    }
                    else
                    {
                        
                        

                        txtSerialNumber.Text = CommonData.MeterNumber;
                        txtWaterCredit.Text = selectedRow.CreditValue.ToString();

                        txtEnd.Text = selectedRow.EndDate.ToString().Substring(0, 10);
                        txtendTime.Text = selectedRow.endTime.ToString();
                        txtstartTime.Text = selectedRow.StartTime.ToString();
                        txtStart303.Text = selectedRow.StartDate.ToString().Substring(0, 10);
                        chk1.IsChecked = Convert.ToBoolean(Convert.ToInt16(checkedv[4]));
                        chk2.IsChecked = Convert.ToBoolean(Convert.ToInt16(checkedv[3]));
                        chk3.IsChecked = Convert.ToBoolean(Convert.ToInt16(checkedv[2]));
                        chk4.IsChecked = Convert.ToBoolean(Convert.ToInt16(checkedv[1]));
                        chk5.IsChecked = Convert.ToBoolean(Convert.ToInt16(checkedv[0]));

                        try
                        {

                            //credit303bool = (showCredit._lstShowCredit303)[CreditGrid.SelectedIndex];//[(showCredit._lstShowCredit303.Count) -1-CreditGrid.SelectedIndex];                          

                        //if (credit303bool.credit_Capability_Activation == 1)
                       if(selectedRow.credit_Capability_Activation==1)
                        {
                            chk10.IsChecked = true;
                            Active1.Visibility = Visibility.Visible;
                            CreditDate.Visibility = Visibility.Visible;
                            StartCredit.Visibility = Visibility.Visible;
                            Active1.IsChecked = true;
                            CreditDate.IsReadOnly = false;
                            CreditDate.Text = selectedRow.creditStartDate; //(Convert.ToDateTime(credit303bool.creditStartDate)).ToString();
                            CreditDate.IsReadOnly = true;
                  
                        }
                        else if (selectedRow.credit_Capability_Activation == -1)
                        {
                            chk10.IsChecked = true;
                            Active1.Visibility = Visibility.Visible;
                            CreditDate.Visibility = Visibility.Hidden;
                            StartCredit.Visibility = Visibility.Hidden;
                            Active1.IsChecked = false;
                        }
                        else if (selectedRow.credit_Capability_Activation == 0)
                        {
                            chk10.IsChecked = false;
                            Active1.Visibility = Visibility.Hidden;
                            CreditDate.Visibility = Visibility.Hidden;
                            Active1.Visibility = Visibility.Hidden;
                        }

                        if (selectedRow.disconnectivity_On_Negative_Credit == 1)
                        {
                            chk20.IsChecked = true;
                            Active2.Visibility = Visibility.Visible;
                            Active2.IsChecked = true;
                        }
                        else if (selectedRow.disconnectivity_On_Negative_Credit == -1)
                        {
                            chk20.IsChecked = true;
                            Active2.Visibility = Visibility.Visible;
                            Active2.IsChecked = false;
                        }
                        else if (selectedRow.disconnectivity_On_Negative_Credit == 0)
                        {
                            chk20.IsChecked = false;
                            Active2.Visibility = Visibility.Hidden;
                        }
                        //
                        if (selectedRow.disconnectivity_On_Expired_Credit == 1)
                        {
                            chk30.IsChecked = true;
                            Active3.Visibility = Visibility.Visible;
                            Active3.IsChecked = true;
                        }
                        else if (selectedRow.disconnectivity_On_Expired_Credit == -1)
                        {
                            chk30.IsChecked = true;
                            Active3.Visibility = Visibility.Visible;
                            Active3.IsChecked = false;
                        }
                        else if (selectedRow.disconnectivity_On_Expired_Credit == 0)
                        {
                            chk30.IsChecked = false;
                            Active3.Visibility = Visibility.Hidden;
                        }
                        }
                        catch
                        {
                            chk10.IsChecked = false;
                            Active1.Visibility = Visibility.Hidden;
                            CreditDate.Visibility = Visibility.Hidden;
                            Active1.Visibility = Visibility.Hidden;
                            chk20.IsChecked = false;
                            Active2.Visibility = Visibility.Hidden;
                            chk30.IsChecked = false;
                            Active3.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshCreditGrid();
            ChangeVisability();
        }

        private void ToolStripButtonExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "Credit "+DateTime.Now.ToPersianString1();// + CommonData.SelectedMeterNumber; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = " (.xlsx)|*.xlsx"; // Filter files by extension 
                Nullable<bool> result = dlg.ShowDialog();
                string FilePath = dlg.FileName;
                if (result.Value)
                {
                    CommonData.mainwindow.changeProgressBarTag("لطفا منتظر بمانید");
                    Thread thExcel = new Thread(() => CreateExcel(FilePath));
                    thExcel.IsBackground=true;
                    thExcel.Start();
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public void CreateExcel(string FilePath)
        {
            try
            {
                CommonData.mainwindow.changeProgressBarTag("لطفا منتظر بمانید");
                Application MyExcel = new Application();
              
                object missing = Type.Missing;
                Workbook wb = MyExcel.Workbooks.Add(missing);
                Worksheet ws = (Worksheet)wb.ActiveSheet;
                Range rng = ws.get_Range("A1", missing);
                DataTable DTExcel = new DataTable();
                DataTable DTWaterConsumed = new DataTable();
                DTExcel.Clear();
                //MyExcel.Visible = false;
                // Insert columns' name
               DTExcel.Columns.Add("  شماره کنتور  ");
                ws.Cells[1, 1].Columns.AutoFit();
                DTExcel.Columns.Add("  نام مشترک  ");
                ws.Cells[1, 2].Columns.AutoFit();
                DTExcel.Columns.Add("شماره اشتراک آب");
                ws.Cells[1, 3].Columns.AutoFit();
                DTExcel.Columns.Add("  تاریخ تخصیص  ");
                ws.Cells[1, 4].Columns.AutoFit();
                DTExcel.Columns.Add("تاریخ شروع اعتبار");
                DTExcel.Columns.Add("تاریخ پایان اعتبار");
                DTExcel.Columns.Add("مجموع اعتبارات تخصیص داده شده");
                ws.Cells[1, 5].Columns.AutoFit();
                DTExcel.Columns.Add("شرح اعتبارات تخصیص داده شده");
                ws.Cells[1, 6].Columns.AutoFit();
                ws.Cells[1, 7].Columns.AutoFit();
                DTExcel.Columns.Add(" آب مصرفی کل ");
                ws.Cells[1, 8].Columns.AutoFit();
                ws.Cells[1, 9].Columns.AutoFit();
                DTExcel.Columns.Add("  شماره کارت  ");
                ws.Cells[1, 10].Columns.AutoFit();
                DTExcel.Columns.Add(" کاربر ");
                ws.Cells[1, 11].Columns.AutoFit();
                ws.Rows[1].Interior.Color = ColorTranslator.ToOle(Color.Gainsboro); // "#FFBFF4B6";
             
                DTWaterConsumed.Columns.Add("  تاریخ  ");
                DTWaterConsumed.Columns.Add("آب مصرفی بازه");
                DTWaterConsumed.Columns.Add("آب مصرفی تجمعی");
                DTWaterConsumed.Columns.Add("ساعت کارکرد");
                // Fill All Rows
                int RowNumber = 2;
                int ins = 0;
                string NumberOfID = "";
              
                List<ShowAllCreditTokenWithNecessary_Result> list_showAllCreditToken = new List<ShowAllCreditTokenWithNecessary_Result>();

                 foreach (var item in DGAllMeres.Items)
                    {
                        list_showAllCreditToken.Add((ShowAllCreditTokenWithNecessary_Result)item);
                        NumberOfID = NumberOfID + list_showAllCreditToken[ins].MeterID + ",";
                        ins++;
                    }
         

                if (IsOneMeter)
                {

                    ws.Cells[RowNumber, 1].value = CommonData.SelectedMeterNumber;
                    ShowToken showToken = new ShowToken(" and Main.MeterID = " + CommonData.SelectedMeterID);
                    //ShowConsumedWater ConsumedWater = new ShowConsumedWater(" and main.meterid=" + CommonData.SelectedMeterID);
                    int r = 0;
                    foreach (var item in showToken._lstShowToken)
                    {
                        DataRow DR = DTExcel.NewRow();
                        if (r == 0)
                        {
                            DR["  شماره کنتور  "] = item.MeterNumber;
                            DR["  نام مشترک  "] = item.CustomerName;
                            DR["شماره اشتراک آب"] = item.WatersubscriptionNumber;
                            DR[" آب مصرفی کل "] = list_showAllCreditToken[0].Value;                          
                            DR["مجموع اعتبارات تخصیص داده شده"] = list_showAllCreditToken[0].CreditValue;
                            r++;
                        }
                     
                        DR["تاریخ شروع اعتبار"] = item.StartDate;
                        DR["تاریخ پایان اعتبار"] = item.EndDate;
                        DR["شرح اعتبارات تخصیص داده شده"] = item.CreditValue;
                        DR["  تاریخ تخصیص  "] = item.BuildDate;
                        DR[" کاربر "] = item.UserName;                   
                        DR["  شماره کارت  "] = item.CardNumber;

                        DTExcel.Rows.Add(DR);

                    } 
                     
                    ShowVeeConsumedWater value = new ShowVeeConsumedWater(CommonData.SelectedMeterID);
                    foreach (var item in value._lstShowVeeConsumedWater)                    
                    {
                        DataRow DR = DTWaterConsumed.NewRow();
                        DR["  تاریخ  "] = item.ConsumedDate;
                        DR["آب مصرفی بازه"] = item.MonthlyConsumption;
                        DR["آب مصرفی تجمعی"] = item.TotalConsumption;
                        DR["ساعت کارکرد"] = item.PumpWorkingHour;
                        DTWaterConsumed.Rows.Add(DR);
                    }
                    // Fill Excel
                    for (int i = 0; i < DTExcel.Columns.Count; i++)
                    {
                        ws.Cells[1, (i + 1)] = DTExcel.Columns[i].ColumnName;                   
                      
                        //ws.Rows[1].font.color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        ws.Columns.AutoFit();
                    }

                    for (int i = 0; i < DTExcel.Rows.Count; i++)
                    {
                        //ws.Columns[i].ColumnWidth = 18;

                        // to do: format datetime values before printing
                        for (int j = 0; j < DTExcel.Columns.Count; j++)
                        {
                            ws.Cells[(i + 2), (j + 1)] = DTExcel.Rows[i][j];
                            if (j < 2)
                            {
                                ws.Cells[(i + 2), (j + 1)].Numberformat = "#";
                                ws.Cells[(i + 2), (j + 1)].Columns.AutoFit();
                            }                      
                        }
                        if ((i + 1) > 0 && ((i + 1) % 2) != 0)
                            ws.Rows[(i + 2)].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                        else
                            ws.Rows[(i + 2)].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);
                        rng = ws.UsedRange;
                        rng.BorderAround(XlBorderWeight.xlThin);
                    }
                    ws.Cells[2, DTExcel.Columns.Count+1] = "  تاریخ  ";
                    ws.Cells[3, DTExcel.Columns.Count+1] = "آب مصرفی";
                    ws.Cells[4, DTExcel.Columns.Count+1] = "آب مصرفی تجمعی";
                    ws.Cells[5, DTExcel.Columns.Count+1] = "ساعت کارکرد";
                    int m = 0;
                    for (int g = (DTExcel.Columns.Count+2); g <(DTExcel.Columns.Count+ DTWaterConsumed.Rows.Count+2); g++)
                    {

                        ws.Cells[2, g] = DTWaterConsumed.Rows[m][0];
                        ws.Cells[3, g] = DTWaterConsumed.Rows[m][1];
                        ws.Cells[4, g] = DTWaterConsumed.Rows[m][2];
                        ws.Cells[5, g] = DTWaterConsumed.Rows[m][3];
                        m++;
                    }
                }
                else
                {
                    CommonData.mainwindow.changeProgressBarTag("لطفا منتظر بمانید");
                    ShowToken showToken;
                    //Fill  Columns   in Excel              
                    for (int i = 0; i < DTExcel.Columns.Count; i++)
                    {
                        ws.Cells[1, (i + 1)] = DTExcel.Columns[i].ColumnName;
                       // setBorderOfCells(1, (i + 1), ws);
                        ws.Cells[1, (i + 1)].Interior.Color = ColorTranslator.ToOle(Color.Gainsboro); // "#FFBFF4B6";
                       // ws.Rows[1].font.color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                       // ws.Columns.AutoFit();
                    }
                    if (NumberOfID.Length != 0)
                        NumberOfID = NumberOfID.Remove(NumberOfID.Length - 1);
                    else
                    {
                        MessageBox.Show("کنتوری برای گزارش گیری نمایش داده نشده است");
                        return;
                    }
                    int r= 0, RowN=0, Number=0;
                    for (int j = 0; j < DGAllMeres.Items.Count; j++)
                    {
                        DTExcel.Rows.Clear();
                        //ShowConsumedWater ConsumedWater = new ShowConsumedWater(" and main.meterid ="+ list_showAllCreditToken[j].MeterID);
                        showToken = new ShowToken(" and Main.MeterID = " +  list_showAllCreditToken[j].MeterID + " order by  MeterNumber");
                        if (showToken._lstShowToken.Count == 0)
                        {
                            DataRow DR = DTExcel.NewRow();
                                 DR["  شماره کنتور  "] = list_showAllCreditToken[j].MeterNumber;
                                 DR["  نام مشترک  "] = list_showAllCreditToken[j].CustomerName;
                                 DR["شماره اشتراک آب"] = list_showAllCreditToken[j].WatersubscriptionNumber;
                                 DR[" آب مصرفی کل "] = list_showAllCreditToken[j].Value;
                            DTExcel.Rows.Add(DR);
                            Number++;
                        }
                        r = 0;
                        foreach (var item in showToken._lstShowToken)
                        {

                            DataRow DR = DTExcel.NewRow();
                            if (r==0)//r==j)
                            {
                                DR["  شماره کنتور  "] = item.MeterNumber;
                                DR["  نام مشترک  "] = item.CustomerName;
                                DR["شماره اشتراک آب"] = item.WatersubscriptionNumber;
                                DR[" آب مصرفی کل "] = list_showAllCreditToken[j].Value;                            
                                DR["مجموع اعتبارات تخصیص داده شده"] = list_showAllCreditToken[j].CreditValue;
                                r++;
                            }                         
                            DR["تاریخ شروع اعتبار"] = item.StartDate;
                            DR["تاریخ پایان اعتبار"] = item.EndDate;
                            DR["شرح اعتبارات تخصیص داده شده"] = item.CreditValue;
                            DR["  تاریخ تخصیص  "] = item.BuildDate;
                            DR[" کاربر "] = item.UserName;                          
                            DR["  شماره کارت  "] = item.CardNumber;

                            DTExcel.Rows.Add(DR);
                            Number++;
                        }
                        DTWaterConsumed.Clear();
                        ShowVeeConsumedWater value = new ShowVeeConsumedWater(Convert.ToDecimal( list_showAllCreditToken[j].MeterID));
                        foreach (var item in value._lstShowVeeConsumedWater)
                        {
                            DataRow DR = DTWaterConsumed.NewRow();
                            DR["  تاریخ  "] = item.ConsumedDate;
                            DR["آب مصرفی بازه"] = item.MonthlyConsumption;
                            DR["آب مصرفی تجمعی"] = item.TotalConsumption;
                            DR["ساعت کارکرد"] = item.PumpWorkingHour;
                            DTWaterConsumed.Rows.Add(DR);
                        }
                        // Fill Excel
                       
                        for (int i = RowN; i < (DTExcel.Rows.Count+RowN); i++)
                        {
                            for (int k = 0; k < DTExcel.Columns.Count; k++)
                            {
                                ws.Cells[(i + 2), (k + 1)] = DTExcel.Rows[(i-RowN)][k];
                                if (k < 2)
                                {
                                    ws.Cells[(i + 2), (k + 1)].Numberformat = "#";
                                  //  ws.Cells[(i + 2), (k + 1)].Columns.AutoFit();
                                }
                            }
                           
                            rng = ws.UsedRange;
                            rng.BorderAround(XlBorderWeight.xlThin);
                        }

                        ws.Cells[RowN+2, DTExcel.Columns.Count + 1] = "  تاریخ  ";
                        ws.Cells[RowN+3, DTExcel.Columns.Count + 1] = "آب مصرفی";
                        ws.Cells[RowN+4, DTExcel.Columns.Count + 1] = "آب مصرفی تجمعی";
                        ws.Cells[RowN+5, DTExcel.Columns.Count + 1] = "ساعت کارکرد";
                        int m = 0;
                        for (int g = (DTExcel.Columns.Count + 2); g < (DTExcel.Columns.Count + DTWaterConsumed.Rows.Count + 2); g++)
                        {
                            ws.Cells[RowN+2, g] = DTWaterConsumed.Rows[m][0];
                            ws.Cells[RowN+3, g] = DTWaterConsumed.Rows[m][1];
                            ws.Cells[RowN+4, g] = DTWaterConsumed.Rows[m][2];
                            ws.Cells[RowN+5, g] = DTWaterConsumed.Rows[m][3];
                            m++;

                            if ((j + 1) > 0 && ((j + 1) % 2) != 0)
                            {
                                ws.Rows[(RowN + 2)].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                                ws.Rows[(RowN + 3)].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                                ws.Rows[(RowN + 4)].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                                ws.Rows[(RowN + 5)].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                            }
                            else
                            {
                                ws.Rows[(RowN + 2)].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);
                                ws.Rows[(RowN + 3)].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);
                                ws.Rows[(RowN + 4)].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);
                                ws.Rows[(RowN + 5)].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);
                            }
                        }
                        if (DTExcel.Rows.Count>4)//(Number-RowN)> 3)
                            RowN = DTExcel.Rows.Count + RowN;//RowN = +Number;
                        else
                            RowN = RowN+4;
                    }
                                   
                }
                // Set ws for culomn
                ws.UsedRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ws.UsedRange.Borders.Weight = XlBorderWeight.xlMedium;
                ws.UsedRange.Borders.LineStyle = XlLineStyle.xlContinuous;
                ws.UsedRange.Columns.AutoFit();
                ws.Rows[1].font.bold = true;

                //freez first column
                ws.Application.ActiveWindow.SplitRow = 1;
                ws.Application.ActiveWindow.FreezePanes = true;

                wb.SaveAs(FilePath, AccessMode: XlSaveAsAccessMode.xlShared);
                MyExcel.Application.Workbooks.Open(FilePath, 0, false, 5, "", "", false,
                XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);
                //System.Diagnostics.Process.Start(FilePath);
                CommonData.mainwindow.changeProgressBarTag("گزارش به صورت فایل اکسل در آدرس تعیین شده ذخیره شد");
                MessageBox.Show("گزارش به صورت فایل اکسل در آدرس تعیین شده ذخیره شد");
                CommonData.mainwindow.changeProgressBarTag(" ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }

        }

        void setBorderOfCells(int row, int column, Worksheet ws)
        {
            ws.Cells[row, column].Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThin;
            ws.Cells[row, column].Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
            ws.Cells[row, column].Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;
            ws.Cells[row, column].Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThin;


            //    ws.Cells[row, column].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        }

        private void CreditGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
            SelectGrid();
        }

        private void datePickerStart_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            SetDate();           
            //RefreshSelectedMeters(filter);
            
        }
        public void SetDate()
        {
            try
            {
                if (CommonData.LanguagesID == 2)
                {
                    
                    startdate = datePickerStart.Text;
                    enddate = datePickerEnd.Text;
                    int day = Convert.ToInt32(startdate.Substring(startdate.LastIndexOf('/') + 1, startdate.Length - startdate.LastIndexOf('/') - 1));
                    int Month = Convert.ToInt32(startdate.Substring(startdate.IndexOf('/') + 1, startdate.LastIndexOf('/') - startdate.IndexOf('/') - 1));
                    if (day < 10 && startdate.Length < 10)

                        startdate = startdate.Substring(0, startdate.Length - 1) + "0" + day;

                    if (Month < 10 && startdate.Length < 10)
                        startdate = startdate.Substring(0, startdate.IndexOf('/') + 1) + "0" + startdate.Substring(startdate.IndexOf('/') + 1, startdate.Length - startdate.IndexOf('/') - 1);
                    day = Convert.ToInt32(enddate.Substring(enddate.LastIndexOf('/') + 1, enddate.Length - enddate.LastIndexOf('/') - 1));
                    Month = Convert.ToInt32(enddate.Substring(enddate.IndexOf('/') + 1, enddate.LastIndexOf('/') - enddate.IndexOf('/') - 1));
                    if (day < 10)

                        enddate = enddate.Substring(0, enddate.Length - 1) + "0" + day;

                    if (Month < 10)
                        enddate = enddate.Substring(0, enddate.IndexOf('/') + 1) + "0" + enddate.Substring(enddate.IndexOf('/') + 1, enddate.Length - enddate.IndexOf('/') - 1);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }
        }
        private string ConvertPerDatetoGre(string datePickerDate)
        {
            try
            {
                
                string date = datePickerDate;
                String[] userDateParts = date.Split(new[] { "/" }, StringSplitOptions.None);
                int userYear = NumericConverter.IntConverter(userDateParts[0]);
                int userMonth = NumericConverter.IntConverter(userDateParts[1]);
                int userDay = NumericConverter.IntConverter(userDateParts[2]);
                DateTime userDate = new DateTime(userYear, userMonth, userDay,new PersianCalendar());
                //userDate = userDate.AddDays(userDay - 1);
                return userDate.ToString(CultureInfo.InvariantCulture);
                //return userDate.ToString("MM/dd/yyyy HH:mm:ss");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }
            return "";
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DGAllMeres.ItemsSource = null;
                DGAllMeres.Items.Refresh();              
                CreditGrid.ItemsSource = null;
                CleanCheckboxes();
               
                bool WithCredit = false;
                // get the time
                //SetDate();
                string SDate = "", EDate = "";
             
                SDate = ConvertPerDatetoGre(datePickerStart.SelectedDate.AddDays(-1).ToString());
                EDate = ConvertPerDatetoGre(datePickerEnd.SelectedDate.AddDays(1).ToString());
                // get With credit or not
                if (RdbtnWithCredit.IsChecked == true)
                    WithCredit = true;
                else if (RdbtnWithOutCredit.IsChecked == true)
                    WithCredit = false;
                //Get info & fill DataGrid
                decimal GroupID = 0;
                if (CommonData.selectedGroup.GroupName != "همه گروه ها")
                    GroupID = CommonData.selectedGroup.GroupID;

                if ((!IsNecessary && !IsnotNecessary) ||(IsNecessary && IsnotNecessary))
                {
                    ShowAllCreditTokenWithNecessary showAllCreditToken = new ShowAllCreditTokenWithNecessary(SDate, EDate, CommonData.UserID, GroupID, CommonData.selectedGroup.GroupType,false, WithCredit,false);
                List<ShowAllCreditTokenWithNecessary_Result> _lstShowSomCreditToken = new List<ShowAllCreditTokenWithNecessary_Result>();
                bool IsRepeated = false;
                int Num = 0;

                    for (int i = 0; i < showAllCreditToken._lstShowAllCreditTokenWithNecessary.Count; i++)
                    {
                        var t1 = showAllCreditToken._lstShowAllCreditTokenWithNecessary[i];
                        if (string.IsNullOrEmpty(t1.WatersubscriptionNumber))
                        {

                            for (int j = i+1; j < showAllCreditToken._lstShowAllCreditTokenWithNecessary.Count; j++)
                            {
                                var t2 = showAllCreditToken._lstShowAllCreditTokenWithNecessary[j];
                                if (t1.MeterID == t2.MeterID)
                                {
                                    if (!string.IsNullOrEmpty(t2.WatersubscriptionNumber))
                                        t1.WatersubscriptionNumber = t2.WatersubscriptionNumber;
                                }
                            }
                        }
                    }


                foreach (var item in showAllCreditToken._lstShowAllCreditTokenWithNecessary)
                {
                    for (int i = 0; i < _lstShowSomCreditToken.Count; i++)
                    {
                        if (_lstShowSomCreditToken[i].MeterID == item.MeterID)
                        {
                            IsRepeated = true;
                            Num = i;
                            break;
                        }
                    }
                    if (!IsRepeated)
                        _lstShowSomCreditToken.Add(item);
                    else if (IsRepeated)
                    {
                        _lstShowSomCreditToken[Num].CreditValue += item.CreditValue;
                        if (_lstShowSomCreditToken[Num].Value.ToString().Trim() == "") _lstShowSomCreditToken[Num].Value = "0";
                        if (item.Value.ToString().Trim() == "") item.Value = "0";
                        string old = _lstShowSomCreditToken[Num].Value.ToString();
                        string newvalue = item.Value.ToString();
                        if (NumericConverter.DoubleConverter(old) <NumericConverter.DoubleConverter(newvalue))
                            _lstShowSomCreditToken[Num].Value = item.Value;
                    }
                    IsRepeated = false;
                }

                if (showAllCreditToken._lstShowAllCreditTokenWithNecessary.Count != 0)
                {
                    DGAllMeres.ItemsSource = _lstShowSomCreditToken;
                    Lbcount.Content = _lstShowSomCreditToken.Count.ToString();
                    //DGAllMeres.ItemsSource = showAllCreditToken._lstShowAllCreditToken;

                }
            }
             else if((IsnotNecessary && !IsNecessary)||(!IsnotNecessary && IsNecessary))
                {
                    //ShowAllCreditToken showAllCreditToken = new ShowAllCreditToken(SDate, EDate, WithCredit, CommonData.UserID, GroupID, CommonData.selectedGroup.GroupType);
                    ShowAllCreditTokenWithNecessary showAllCreditTokenwithNecessary = new ShowAllCreditTokenWithNecessary(SDate, EDate, CommonData.UserID, GroupID, CommonData.selectedGroup.GroupType, IsNecessary,WithCredit,true);
                    List<ShowAllCreditTokenWithNecessary_Result> _lstShowSomCreditTokenwithNecessary = new List<ShowAllCreditTokenWithNecessary_Result>();
                    bool IsRepeated = false;
                    int Num = 0;

                    foreach (var item in showAllCreditTokenwithNecessary._lstShowAllCreditTokenWithNecessary)
                    {
                        for (int i = 0; i < _lstShowSomCreditTokenwithNecessary.Count; i++)
                        {
                            if (_lstShowSomCreditTokenwithNecessary[i].MeterID == item.MeterID)
                            {
                                IsRepeated = true;
                                Num = i;
                                break;
                            }
                        }
                        if (!IsRepeated)
                            _lstShowSomCreditTokenwithNecessary.Add(item);
                        else if (IsRepeated)
                        {
                            _lstShowSomCreditTokenwithNecessary[Num].CreditValue += item.CreditValue;
                            if (_lstShowSomCreditTokenwithNecessary[Num].Value.ToString().Trim() == "") _lstShowSomCreditTokenwithNecessary[Num].Value = "0";
                            if (item.Value.ToString().Trim() == "") item.Value = "0";
                            string old = _lstShowSomCreditTokenwithNecessary[Num].Value.ToString();
                            string newvalue = item.Value.ToString();
                            if (NumericConverter.DoubleConverter(old) <
                                NumericConverter.DoubleConverter(newvalue))
                                _lstShowSomCreditTokenwithNecessary[Num].Value = item.Value;
                        }
                        IsRepeated = false;
                    }

                    if (showAllCreditTokenwithNecessary._lstShowAllCreditTokenWithNecessary.Count != 0)
                    {
                        DGAllMeres.ItemsSource = _lstShowSomCreditTokenwithNecessary;
                        Lbcount.Content = _lstShowSomCreditTokenwithNecessary.Count.ToString();
                        //DGAllMeres.ItemsSource = showAllCreditToken._lstShowAllCreditToken;

                    }
                }
        
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error in DateTime");

            }

        }

        private void chekboxwithCredit_Checked(object sender, RoutedEventArgs e)
        {
            IsNecessary = true;
            //chekboxwithoutCredit.IsChecked = false;
            //IsnotNecessary = false;
        }

        private void chekboxwithCredit_Unchecked(object sender, RoutedEventArgs e)
        {
            IsNecessary = false;
        }

        private void chekboxwithoutCredit_Unchecked(object sender, RoutedEventArgs e)
        {
            IsnotNecessary = false;
            //chekboxwithCredit.IsChecked = false;
            //IsNecessary = false;
        }

        private void chekboxwithoutCredit_Checked(object sender, RoutedEventArgs e)
        {
            IsnotNecessary = true;
        }

        private void RdbtnWithOutCredit_Click(object sender, RoutedEventArgs e)
        {
            if (RdbtnWithOutCredit.IsChecked == true)
            {
                chekboxwithoutCredit.IsEnabled = false;
                chekboxwithCredit.IsEnabled = false;
                chekboxwithoutCredit.IsChecked = false;
                chekboxwithCredit.IsChecked = false;
                IsNecessary = false;
                IsnotNecessary = false;
            }
            else
            {
                chekboxwithoutCredit.IsEnabled = true;
                chekboxwithCredit.IsEnabled = true;
                if (chekboxwithoutCredit.IsChecked == true) IsnotNecessary = true;
                if (chekboxwithCredit.IsChecked == true) IsNecessary = true;
            }
        }

        private void RdbtnWithCredit_Click(object sender, RoutedEventArgs e)
        {
            if (RdbtnWithCredit.IsChecked==true)
            {
                chekboxwithCredit.IsEnabled = true;
                chekboxwithoutCredit.IsEnabled = true;
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

        private void DGAllMeres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                try
                {               
                  ShowAllCreditTokenWithNecessary_Result MeterID = (ShowAllCreditTokenWithNecessary_Result)DGAllMeres.SelectedItem;
                    if (MeterID != null)
                    {

                        CommonData.SelectedMeterID = MeterID.MeterID;

                        txtSerialNumber207.Text = MeterID.MeterNumber;
                        txtSerialNumber.Text = MeterID.MeterNumber;
                        CommonData.MeterNumber = MeterID.MeterNumber;
                        CommonData.SelectedMeterNumber = MeterID.MeterNumber;
                        RefreshCreditGrid();
                        ChangeVisability();
                        SelectGrid();

                    }
                }
                catch 
                {
                    ShowAllCreditTokenWithNecessary_Result MeterID = (ShowAllCreditTokenWithNecessary_Result)DGAllMeres.SelectedItem;
                    if (MeterID != null)
                    {

                        CommonData.SelectedMeterID = MeterID.MeterID;

                        txtSerialNumber207.Text = MeterID.MeterNumber;
                        txtSerialNumber.Text = MeterID.MeterNumber;
                        CommonData.MeterNumber = MeterID.MeterNumber;
                        CommonData.SelectedMeterNumber = MeterID.MeterNumber;
                        RefreshCreditGrid();
                        ChangeVisability();
                        SelectGrid();

                    }
                }
              

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }
        }
    }
}
