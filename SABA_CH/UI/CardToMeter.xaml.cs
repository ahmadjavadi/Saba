using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using SABA_CH.Global;
using Application = Microsoft.Office.Interop.Excel.Application;
using Window = System.Windows.Window;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for CardToMeter.xaml
    /// </summary>
    public partial class CardToMeter : System.Windows.Window
    {
        public ShowTranslateofLable Tr;
        public readonly int WindowId = 15;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
    
       
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
        public CardToMeter()
        {
            InitializeComponent();
            Tr = CommonData.translateWindow(WindowId);
            //MGrid.FlowDirection = CommonData.FlowDirection;          
         
           
                RefreshCardToMeterGrid();
                TranslateGrids();
          
        }
        private void TranslateGrids()
        {
            try
            {
                CardToMeterGrid.Columns[0].Header = Tr.TranslateofLable.Object4;
                CardToMeterGrid.Columns[1].Header = Tr.TranslateofLable.Object7;
                CardToMeterGrid.Columns[2].Header = Tr.TranslateofLable.Object8;
                CardToMeterGrid.Columns[3].Header = Tr.TranslateofLable.Object10;
                CardToMeterGrid.Columns[4].Header = Tr.TranslateofLable.Object9;
                CardToMeterGrid.Columns[5].Header = Tr.TranslateofLable.Object5;
                CardToMeterGrid.Columns[6].Header = Tr.TranslateofLable.Object6;
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
       
        public void RefreshCardToMeterGrid()
        {
            try
            {
                string filter = " and Main.MeterID=" + CommonData.SelectedMeterID;
                ShowCardTOMeter showcardTometer = new ShowCardTOMeter(filter);
                //CardToMeterGrid.ItemsSource = null;
                CardToMeterGrid.ItemsSource = showcardTometer._lstShowCardTOMeter;

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
                RefreshCardToMeterGrid();
                this.Title = CommonData.mainwindow.translateWindowName.TranslateofLable.Object15 + " " + CommonData.SelectedMeterNumber;
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
                dlg.FileName = "Card " + CommonData.SelectedMeterNumber; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = " (.xlsx)|*.xlsx"; // Filter files by extension 
                Nullable<bool> result = dlg.ShowDialog();
                string filePath = dlg.FileName;
                if (result != null && result.Value)
                {
                    ExportTpExcel(filePath);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void ExportTpExcel(string filePath)
        {
            try
            {
                Application myExcel = new Application();
                object missing = Type.Missing;
                Workbook wb = myExcel.Workbooks.Add(missing);
                Worksheet ws = (Worksheet)wb.ActiveSheet;
                Range rng = ws.get_Range("A1", missing);

                myExcel.Visible = false;
                // Insert columns' name
                ws.Cells[1, 1].value = "    شماره کنتور   ";
                ws.Cells[1, 1].Columns.AutoFit();
                ws.Cells[1, 2].value = " نام مشترک ";
                ws.Cells[1, 2].Columns.AutoFit();
                ws.Cells[1, 3].value = "شماره اشتراک آب";
                ws.Cells[1, 3].Columns.AutoFit();
                ws.Cells[1, 4].value = "شماره پرونده";
                ws.Cells[1, 4].Columns.AutoFit();
                ws.Cells[1, 5].value = "  شماره کارت   ";
                ws.Cells[1, 5].Columns.AutoFit();
                ws.Cells[1, 6].value = "تاریخ تخصیص کارت به کنتور";
                ws.Cells[1, 6].Columns.AutoFit();
                ws.Cells[1, 7].value = "  کاربر  ";
                ws.Cells[1, 7].Columns.AutoFit();
                ws.Rows[1].Interior.Color = ColorTranslator.ToOle(Color.Gainsboro);

                string filter = " and Main.MeterID=" + CommonData.SelectedMeterID;
                ShowCardTOMeter showcardTometer = new ShowCardTOMeter(filter);

                List<ShowCardsToMetersDetailes> list_ShowCardsTOMetersDetailes = new List<ShowCardsToMetersDetailes>();
                int rowNumber = 2;

                foreach (var item in showcardTometer._lstShowCardTOMeter)
                {
                    if ((rowNumber % 2) == 1)
                        ws.Rows[rowNumber].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                    else
                        ws.Rows[rowNumber].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);
                    //for (int i = 1; i < 8; i++)
                    //    setBorderOfCells(RowNumber,i, ws);
                    ws.Cells[rowNumber, 1].NumberFormat = "@";
                    ws.Cells[rowNumber, 1].value = item.MeterNumber;
                    ws.Cells[rowNumber, 2].value = item.CustomerName;
                    ws.Cells[rowNumber, 3].value = item.WatersubscriptionNumber;
                    ws.Cells[rowNumber, 4].value = item.DossierNumber;
                    ws.Cells[rowNumber, 5].value = item.CardNumber;
                    ws.Cells[rowNumber, 6].value = item.SetDate;
                    ws.Cells[rowNumber, 7].value = item.UserName;
                    rowNumber++;
                }

                // Set ws for culomn
                ws.UsedRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                ws.UsedRange.Borders.Weight = XlBorderWeight.xlMedium;
                ws.UsedRange.Borders.LineStyle = XlLineStyle.xlContinuous;
                ws.UsedRange.Columns.AutoFit();
                ws.Rows[1].font.bold = true;
                wb.SaveAs(filePath, AccessMode: XlSaveAsAccessMode.xlShared);
                myExcel.Application.Workbooks.Open(filePath, 0, false, 5, "", "", false,
                XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);
               // MessageBox.Show("گزارش در آدرس تعیین شده ذخیره گردید");
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
    }
}
