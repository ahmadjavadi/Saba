using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;
using Application = Microsoft.Office.Interop.Excel.Application;
using Window = System.Windows.Window;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for TypeandSoftware.xaml
    /// </summary>
    public partial class DeviceModel : System.Windows.Window
    {
        public readonly int windowID = 9;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public ObjectParameter Result = new ObjectParameter("Result", 1000000);
        public ObjectParameter DeviceModelID = new ObjectParameter("DeviceModelID", 1000000);
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public ShowButtonAccess_Result us = null;
        public TabControl Tab { set { tabCtrl = value; } }
        public ShowTranslateofLable tr = null;
        public ShowDeviceModelandSoftversion_Result SelectedRow = null;
        public ShowCountries_Result SelectedCountry = null;
        public bool IsNew = false;
        public bool ISEditing = false;
        public decimal? deviceModelID = null, deviceTypeID=null;
        public DeviceModel()
        {
            InitializeComponent();
            tr = CommonData.translateWindow(9);
            //GridLabel.DataContext = tr.TranslateofLable;
            RefreshDataGrid();
            ChangeFlowDirection();
            TranslateDataGrid();
            RefreshcmbDeviceType();

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", windowID);
                toolBar1.DataContext = us;
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
                if (!ISEDiting())
                {
                    e.Cancel = true;
                    return;
                }
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
        public bool ISEDiting()
        {
            try
            {
                if (ISEditing)
                {
                    MessageBoxResult res = MessageBox.Show("تغییرات قبلی شما ذخیره نشده است آیا مایل به ادامه کار هستید", "اخطار", MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.No)
                        return false;
                    else
                    {
                        ISEditing = false;
                        return true;
                    }
                }
                else
                    return true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                return false;
            }
        }
        private void ToolStripButtonNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ISEDiting())
                    return;
                IsNew = true;
                CommonData.New(this.MGrid);
                //GridDown.DataContext = null;
                deviceModelID = null;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ISEditing = false;
                Result = new ObjectParameter("Result", 1000000);
                DeviceModelID = new ObjectParameter("DeviceModelID", 1000000);
                ErrMsg = new ObjectParameter("ErrMsg", "");
                //if (IsNew)
                //    SQLSPS.InsDeviceModel(txtModelName.Text, txtManufacturer.Text, deviceTypeID, DeviceModelID, Result, ErrMsg);
                //else
                //    SQLSPS.UPDDeviceModel(txtModelName.Text, txtManufacturer.Text, deviceTypeID, deviceModelID, Result, ErrMsg);
                if (ErrMsg.Value.ToString() != "")
                    MessageBox.Show(ErrMsg.Value.ToString());
                RefreshDataGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (deviceModelID == null)
                {
                    MessageBox.Show("هیچ رکوردی برای حذف انتخاب نشده است");
                    return;
                }
                MessageBoxResult res = MessageBox.Show("آیا مایل به حذف رکورد انتخاب شده هستید", "اخطار", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    Result = new ObjectParameter("Result", 1000000);
                    ErrMsg = new ObjectParameter("ErrMsg", "");
                    SQLSPS.DelDeviceModel(deviceModelID, Result, ErrMsg);
                    if (ErrMsg.Value.ToString() != "")
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                    }
                    RefreshDataGrid();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "EXCEL Files (*.xls)|*.xlsx|ALL Files (*.*)|*.*"; 
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    // Open document 
                    string filename = dlg.FileName;
                    Read_Excell(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void Read_Excell(string _path)
        {
                ObjectParameter FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                Result = new ObjectParameter("Result", 1000000);
                ObjectParameter DeviceTypeID = new ObjectParameter("DeviceTypeID", 1000000);
                DeviceModelID = new ObjectParameter("DeviceModelID", 1000000);
                ErrMsg = new ObjectParameter("ErrMsg", "");
                ObjectParameter SoftversionToDeviceModelID =new ObjectParameter("SoftversionToDeviceModelID",1000000);
                ObjectParameter ReturnUnitConvertType =new ObjectParameter("ReturnUnitConvertType",1000000);

                Application excelApp = new Application();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Workbook workbook = (Workbook)excelApp.Workbooks.Add(Missing.Value);

                Worksheet worksheet;

                try
                {

                    // Opening excel file
                    
                    workbook = excelApp.Workbooks.Open(_path, 0, false, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                    // Get first Worksheet

                    worksheet = (Worksheet)workbook.Sheets.get_Item(1);



                    // Setting cell values
                    Range range = worksheet.UsedRange;
                    int c = range.Rows.Count;

                    ShowDeviceModelandSoftversion_Result newmodel = new ShowDeviceModelandSoftversion_Result();
                    newmodel.DeviceName = ((Range)worksheet.Cells[2, "A"]).Value;
                    
                    newmodel.DeviceModelName =Convert.ToString(((Range)worksheet.Cells[2, "B"]).Value);
                    newmodel.ManufacturerName = Convert.ToString(((Range)worksheet.Cells[2, "C"]).Value);
                    newmodel.Softversion = Convert.ToString(((Range)worksheet.Cells[2, "D"]).Value);
                    
                    SQLSPS.InsDeviceModel(newmodel.DeviceModelName, newmodel.ManufacturerName, newmodel.DeviceName, newmodel.MessageVersion,DeviceTypeID, DeviceModelID, Result, ErrMsg);
                    if (ErrMsg.Value.ToString()=="")
                    {
                        SQLSPS.InsSoftversionToDeviceModel(newmodel.Softversion, Convert.ToDecimal(DeviceModelID.Value), SoftversionToDeviceModelID, Result, ErrMsg);
                    }
                    if (ErrMsg.Value=="")
                    {
                        for (int i = 4; i <= c; i++)
                            if (Convert.ToString(((Range)worksheet.Cells[i, "A"]).Value) != "" & Convert.ToString(((Range)worksheet.Cells[i, "A"]).Value) != "OBISCODE")
                            {
                                FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                                Result = new ObjectParameter("Result", 1000000);
                                ObjectParameter OBISID = new ObjectParameter("OBISID", 1000000);
                                ErrMsg = new ObjectParameter("ErrMsg", "");
                                ObjectParameter ReturnOBISType = new ObjectParameter("ReturnOBISType", "");
                                ShowOBISs_Result OBIDCODE = new ShowOBISs_Result();
                                OBIDCODE.Obis = Convert.ToString((((Range)worksheet.Cells[i, "A"]).Value));
                                OBIDCODE.ObisCode= Convert.ToString((((Range)worksheet.Cells[i, "B"]).Value));
                                OBIDCODE.ObisFarsiDesc = Convert.ToString((((Range)worksheet.Cells[i, "C"]).Value));
                                OBIDCODE.ObisLatinDesc = Convert.ToString((((Range)worksheet.Cells[i, "D"]).Value));
                                OBIDCODE.ObisArabicDesc = Convert.ToString((((Range)worksheet.Cells[i, "E"]).Value));
                                OBIDCODE.DeviceTypeID = Convert.ToDecimal(DeviceTypeID.Value);//Convert.ToDecimal((((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[i, "F"]).Value));
                                OBIDCODE.OBISUnitDesc = Convert.ToString((((Range)worksheet.Cells[i, "F"]).Value));
                                OBIDCODE.ObisTypeID = Convert.ToDecimal((((Range)worksheet.Cells[i, "G"]).Value));
                                OBIDCODE.Format = Convert.ToString((((Range)worksheet.Cells[i, "H"]).Value));                                
                                OBIDCODE.ClassID = Convert.ToInt32((((Range)worksheet.Cells[i, "I"]).Value));
                                OBIDCODE.CardFormatType = Convert.ToString((((Range)worksheet.Cells[i, "J"]).Value));
                                OBIDCODE.HHuFormatType = Convert.ToString((((Range)worksheet.Cells[i, "K"]).Value));
                                SQLSPS.INSOBISs(OBIDCODE.ObisCode, OBIDCODE.Obis, OBIDCODE.ObisFarsiDesc, OBIDCODE.ObisLatinDesc, OBIDCODE.ObisArabicDesc, OBIDCODE.DeviceTypeID, OBIDCODE.OBISUnitDesc, OBIDCODE.ObisTypeID, OBIDCODE.Format, OBIDCODE.ClassID, OBIDCODE.CardFormatType, OBIDCODE.HHuFormatType, "", FixedOBISCode,ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMsg);
                                if (ErrMsg.Value == "")
                                    SQLSPS.InsobisToSoftversion(Convert.ToDecimal(OBISID.Value), Convert.ToDecimal(SoftversionToDeviceModelID.Value), Result, ErrMsg);

                            }
                    }
                   
                    workbook.Close(0, 0, 0);
                }
                catch(Exception ex)
                {
                    //workbook.SaveAs(_path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    workbook.Close(0, 0, 0);
                    MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                }
                
            }
        
        public void RefreshDataGrid()
        {
            try
            {
                ShowDeviceModelandSoftversion devicemodel = new ShowDeviceModelandSoftversion("");
                MainGrid.ItemsSource = null;
                MainGrid.ItemsSource = devicemodel._lstShowDeviceModelandSoftversion;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void TranslateDataGrid()
        {
            try
            {
                MainGrid.Columns[0].Header = tr.TranslateofLable.Object2;
                MainGrid.Columns[1].Header = tr.TranslateofLable.Object1;
                MainGrid.Columns[2].Header = tr.TranslateofLable.Object3;
                MainGrid.Columns[3].Header = tr.TranslateofLable.Object4;

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
                //MGrid.FlowDirection = CommonData.FlowDirection;
                //MainGrid.FlowDirection = CommonData.FlowDirection;
                //GridLabel.FlowDirection = CommonData.FlowDirection;
                //GridDown.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void MainGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!ISEDiting())
                    return;
                IsNew = false;
                if (MainGrid.SelectedItem!=null)
                {
                    SelectedRow = (ShowDeviceModelandSoftversion_Result)MainGrid.SelectedItem;
                    deviceModelID = SelectedRow.DeviceModeleID;
                    //GridDown.DataContext = SelectedRow;
                    //cmbDeviceType.Text = SelectedRow.DeviceName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshcmbDeviceType()
        {
            try
            {
                ShowDeviceType devicetype = new ShowDeviceType("");
                //cmbDeviceType.ItemsSource = devicetype.CollectShowDeviceType;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void cmbDeviceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //if (cmbDeviceType.SelectedItem!=null)
                //{
                    
                //    ShowDeviceType_Result  DeviceType  =(ShowDeviceType_Result)cmbDeviceType.SelectedItem;
                //    deviceTypeID = DeviceType.DeviceTypeID;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void txtModelName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                //if (IsNew)
                //    GridDown.DataContext = null;
                ISEditing = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

    }
}
