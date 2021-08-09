using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;
using Application = Microsoft.Office.Interop.Excel.Application;
using Window = System.Windows.Window;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for CardsToMeters.xaml
    /// </summary>
    public partial class CardsToMeters : System.Windows.Window
    {
        public ShowTranslateofLable Tr;
        public readonly int WindowId = 27;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        ShowALLCardsTOMeters_Result _selectedCard;
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
        public CardsToMeters()
        {
            Tr = CommonData.translateWindow(15);
            //DGAllMetersCards.FlowDirection = CommonData.FlowDirection;
            //DGDetailesMeterCard.FlowDirection = CommonData.FlowDirection;
            InitializeComponent();
            RefreshCardsToMetersGrid();
            TranslateGrids();

        }
        private void TranslateGrids()
        {
            try
            {
                //DGAllMetersCards.Columns[0].Header = Tr.TranslateofLable.Object1;
                //DGAllMetersCards.Columns[1].Header = Tr.TranslateofLable.Object2;
                //DGAllMetersCards.Columns[2].Header = Tr.TranslateofLable.Object9;
                //DGAllMetersCards.Columns[3].Header = Tr.TranslateofLable.Object7;

                //DGDetailesMeterCard.Columns[0].Header = Tr.TranslateofLable.Object6;
                //DGDetailesMeterCard.Columns[1].Header = Tr.TranslateofLable.Object5;
                //DGDetailesMeterCard.Columns[2].Header = Tr.TranslateofLable.Object4;
               
                //DGDetailesMeterCard.Columns[4].Header = tr.TranslateofLable.Object6;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

      
        public void RefreshCardsToMetersGrid()
        {
            try
            {
               
                ShowAllCardsToMeters showcardTometer = new ShowAllCardsToMeters("");
               List<ShowALLCardsTOMeters_Result> list_ShowCardsTOMeters = new List<ShowALLCardsTOMeters_Result>();
                if(showcardTometer._lstShowAllCardsTOMeters.Count!=0)
                  DGAllMetersCards.ItemsSource = showcardTometer._lstShowAllCardsTOMeters;

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
                _tabCtrl.SelectedItem = _tabPag;
                if (!_tabCtrl.IsVisible)
                {
                    _tabCtrl.Visibility = Visibility.Visible;
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
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

      

        private void DGAllMetersCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DGAllMetersCards.SelectedItem != null)
                {
                   _selectedCard =(ShowALLCardsTOMeters_Result) DGAllMetersCards.SelectedItem;

                    //decimal MeterNumber = DGAllMetersCards.SelectedItem.ToString(); //SelectedCard.MeterID;
                    string filter = " and Main.MeterID=" + _selectedCard.MeterID;
                    ShowCardTOMeter showcardTometer = new ShowCardTOMeter(filter);

                    List<ShowCardsToMetersDetailes> list_ShowCardsTOMetersDetailes = new List<ShowCardsToMetersDetailes>();

                    foreach (var item in showcardTometer._lstShowCardTOMeter)
                    {
                        ShowCardsToMetersDetailes oshow = new ShowCardsToMetersDetailes();
                        oshow.MeterId = item.MeterID;
                        oshow.CardNumber = item.CardNumber;                      
                        oshow.SetDate = item.SetDate;
                        oshow.UserName = item.UserName;
                        oshow.MeterNumber = item.MeterNumber;
                        list_ShowCardsTOMetersDetailes.Add(oshow);

                       
                    }
                    
                    DGDetailesMeterCard.ItemsSource = list_ShowCardsTOMetersDetailes;
                   
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "Cards for All Meters "; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = " (.xlsx)|*.xlsx"; // Filter files by extension 
                Nullable<bool> result = dlg.ShowDialog();
                string filePath = dlg.FileName;
                if (result != null && result.Value)
                {
                    ExportToExcel(filePath);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        public void ExportToExcel(string filePath)
        {
            try
            {
                CreateExcel(filePath);
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        public void CreateExcel(string filePath)
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
                ws.Cells[1, 7].value =  "  کاربر  ";
                ws.Cells[1, 7].Columns.AutoFit();
                ws.Rows[1].Interior.Color = ColorTranslator.ToOle(Color.Gainsboro); // "#FFBFF4B6";
                //ws.Rows[1].font.size = 12;
                //ws.Rows[1].font.bold = true;
                //for (int i = 1; i < 8; i++)
                //    setBorderOfCells(1, i, ws);
                // Fill All Rows

                ShowCardTOMeter showcardTometer = new ShowCardTOMeter("");

                List<ShowCardsToMetersDetailes> list_ShowCardsTOMetersDetailes = new List<ShowCardsToMetersDetailes>();
                int RowNumber = 2;
             
                foreach (var item in showcardTometer._lstShowCardTOMeter)
                {
                    if ((RowNumber % 2) == 1)
                        ws.Rows[RowNumber].Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                    else                      
                        ws.Rows[RowNumber].Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);
                    //for (int i = 1; i < 8; i++)
                    //    setBorderOfCells(RowNumber,i, ws);
                    ws.Cells[RowNumber, 1].NumberFormat = "@";
                    ws.Cells[RowNumber, 1].value = item.MeterNumber;
                    ws.Cells[RowNumber, 2].value = item.CustomerName;
                    ws.Cells[RowNumber, 3].value = item.WatersubscriptionNumber;
                    ws.Cells[RowNumber, 4].value = item.DossierNumber;
                    ws.Cells[RowNumber, 5].value = item.CardNumber;
                    ws.Cells[RowNumber, 6].value = item.SetDate;
                    ws.Cells[RowNumber, 7].value = item.UserName;
                    RowNumber++;
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
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
            
        }
        void SetBorderOfCells(int row, int column, Worksheet ws)
        {           
                ws.Cells[row, column].Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThin;         
                ws.Cells[row, column].Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;            
                ws.Cells[row, column].Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;            
                ws.Cells[row, column].Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThin;

       
            //    ws.Cells[row, column].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

        }


    }

    public partial class ShowCardsToMeters
    {
        public decimal MeterId { get; set; }   
        public string MeterNumber { get; set; }      
        public string WatersubscriptionNumber { get; set; }
        public string DossierNumber { get; set; }
        public string CustomerName { get; set; }
    }

    public partial class ShowCardsToMetersDetailes
    {
        public decimal MeterId { get; set; }
        public string CardNumber { get; set; }       
        public string SetDate { get; set; }
        public string UserName { get; set; } 
        public string MeterNumber { get; set; }
       
      
    }

}
