using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;
using Application = Microsoft.Office.Interop.Excel.Application;
using Window = System.Windows.Window;

namespace SABA_CH
{
	/// <summary>
	/// Interaction logic for NewObis.xaml
	/// </summary>
	public partial class NewObis : System.Windows.Window
	{
        public ObjectParameter Obisid = new ObjectParameter("OBISID", 1000000);
        public ObjectParameter Result = new ObjectParameter("Result", 1000000);
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public ShowButtonAccess_Result Us = null;
        public ShowDeviceType_Result SelectedMeterType = null;
        public ShowSoftversionToDeviceModel_Result SelectedSoftversion = null;
        public decimal? obisid = 1000000;
	    public decimal? DeviceModelId = 1000000;
	    public decimal? SoftversionId=1000000;
	    public ShowOBISs_Result SelectedObiSs = null;
		public readonly int WindowId=11;
		private TabControl _tabCtrl;
        private TabItem _tabPag;
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
        public ShowTranslateofLable tr = null;
        public bool IsNew = false;
        public ShowOBISs_Result Selecteditem = null;
		public NewObis()
		{
			this.InitializeComponent();
            tr = CommonData.translateWindow(WindowId);
            ChangeFlowDirection();
            TranslateDataGrid();
            Refresh("");
            
		}
        public void ChangeFlowDirection()
        {
            try
            {
                //Griddown.FlowDirection = CommonData.FlowDirection;
                //GridLabel.FlowDirection = CommonData.FlowDirection;
                //GidMain.FlowDirection = CommonData.FlowDirection;
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
                GridLabel.DataContext = tr.TranslateofLable;
                //GidMain.Columns[0].Header = tr.TranslateofLable.Object1;
                GidMain.Columns[0].Header = tr.TranslateofLable.Object2;
                GidMain.Columns[1].Header = tr.TranslateofLable.Object3;
                GidMain.Columns[2].Header = tr.TranslateofLable.Object10;  
                GidMain.Columns[3].Header = tr.TranslateofLable.Object4;                
                //GidMain.Columns[4].Header = tr.TranslateofLable.Object6;
                GidMain.Columns[4].Header = tr.TranslateofLable.Object7;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        public void RefreshCmbUnitName(string filter )
        {
            try
            {
                
                ShowObisUnits units = new ShowObisUnits(filter);
                CmbUnitName.ItemsSource = units.CollectShowObisUnits;
                if (CmbUnitName.Items.Count>0)
                {
                    CmbUnitName.Text = Selecteditem.OBISUnitDesc;
                }

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        
        public void Refresh(string filter)
        {
            try
            {
                ShowOBISs refresh = new ShowOBISs(filter);
                GidMain.ItemsSource = refresh._lstShowOBISs;
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
                toolBar1.DataContext = Us;
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
                _tabCtrl.Items.Remove(_tabPag);
                if (!_tabCtrl.HasItems)
                {
                    _tabCtrl.Visibility = Visibility.Hidden;
                }
                ClassControl.OpenWin[11] = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
		}

        private void ToolStripButtonNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsNew = true;
                CommonData.New(this.Griddown);
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
                Result = new ObjectParameter("Result", 1000000);
                ErrMsg = new ObjectParameter("ErrMsg", "");
                decimal? unitIdForshow = 1000000;
                SQLSPS.UpdobiSs("", txtOBIS.Text, txtfarsidesc.Text, txtObisLatinDesc.Text, txtObisArabicDesc.Text, 1, CmbUnitName.Text.ToString(), 1, "", 1, "", "", obisid,unitIdForshow, (ObjectParameter)Result, (ObjectParameter)ErrMsg);
                //string Filter = " and SoftversionToDeviceModel.SoftversionToDeviceModelID=" + SelectedSoftversion.SoftversionToDeviceModelID.ToString();
                string Filter = "";
                Refresh(Filter);
                MessageBox.Show("ذخیره اطلاعات به درستی انجام شد");//CommonData.mainwindow.tm.TranslateofMessage.Message91);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در ذخیره اطلاعات");//CommonData.mainwindow.tm.TranslateofMessage.Message92);
            }
        }

        private void ToolStripButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (obisid==null)
                {
                    MessageBox.Show("هیچ رکوردی برای حذف انتخاب نشده است");
                    return;
                }
                MessageBoxResult res = MessageBox.Show("آیا مایل به حذف رکورد انتخاب شده هستید", "اخطار", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    SQLSPS.DelOBISs(obisid, Result, ErrMsg);
                    string filter = " and SoftversionToDeviceModel.SoftversionToDeviceModelID=" + SelectedSoftversion.SoftversionToDeviceModelID.ToString();
                    Refresh(filter);
                    CommonData.New(this.Griddown);
                }                
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
            ObjectParameter FixedOBISCode;
            Result = new ObjectParameter("Result", 1000000);
            ObjectParameter deviceModelId = new ObjectParameter("DeviceModelID", 1000000);
            ErrMsg = new ObjectParameter("ErrMsg", "");
            ObjectParameter deviceTypeId=new ObjectParameter("DeviceTypeID",1000000);
            ObjectParameter returnUnitConvertType=new ObjectParameter("ReturnUnitConvertType",1000000);
            ObjectParameter softversionToDeviceModelId = new ObjectParameter("SoftversionToDeviceModelID", 1000000);
            Application excelApp = new Application();
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
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

                ShowDeviceModel_Result newmodel = new ShowDeviceModel_Result();
                newmodel.DeviceName = ((Range)worksheet.Cells[2, "A"]).Value;

                newmodel.DeviceModelName = Convert.ToString(((Range)worksheet.Cells[2, "B"]).Value);
                newmodel.ManufacturerName = Convert.ToString(((Range)worksheet.Cells[2, "C"]).Value);
                //newmodel.Softversion = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[2, "D"]).Value);
                string Softversion = "";
                SQLSPS.InsDeviceModel(newmodel.DeviceModelName, newmodel.ManufacturerName, newmodel.DeviceName, newmodel.MessageVersion,deviceTypeId, deviceModelId, Result, ErrMsg);
                if (ErrMsg.Value.ToString() == "")
                {
                    //SQLSPS.INSSoftversionToDeviceModel(newmodel.Softversion, Convert.ToDecimal(DeviceModelID.Value), SoftversionToDeviceModelID, Result, ErrMsg);
                    SQLSPS.InsSoftversionToDeviceModel(Softversion, Convert.ToDecimal(deviceModelId.Value), softversionToDeviceModelId, Result, ErrMsg);
                }
                if (ErrMsg.Value == "")
                {
                    for (int i = 4; i <= c; i++)
                        if (Convert.ToString(((Range)worksheet.Cells[i, "A"]).Value) != "" & Convert.ToString(((Range)worksheet.Cells[i, "A"]).Value) != "OBISCODE")
                        {
                            FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                            Result = new ObjectParameter("Result", 1000000);
                            ObjectParameter OBISID = new ObjectParameter("OBISID", 1000000);
                            ErrMsg = new ObjectParameter("ErrMsg", "");
                            ObjectParameter returnObisType = new ObjectParameter("ReturnOBISType", "");
                            ShowOBISs_Result obidcode = new ShowOBISs_Result();
                            obidcode.Obis = Convert.ToString((((Range)worksheet.Cells[i, "A"]).Value));
                            obidcode.ObisCode = Convert.ToString((((Range)worksheet.Cells[i, "B"]).Value));
                            obidcode.ObisFarsiDesc = Convert.ToString((((Range)worksheet.Cells[i, "C"]).Value));
                            obidcode.ObisLatinDesc = Convert.ToString((((Range)worksheet.Cells[i, "D"]).Value));
                            obidcode.ObisArabicDesc = Convert.ToString((((Range)worksheet.Cells[i, "E"]).Value));
                            //OBIDCODE.DeviceTypeID = Convert.ToDecimal((((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[i, "F"]).Value));
                            obidcode.OBISUnitDesc = Convert.ToString((((Range)worksheet.Cells[i, "F"]).Value));
                            obidcode.ObisTypeID = Convert.ToDecimal((((Range)worksheet.Cells[i, "G"]).Value));
                            obidcode.Format = Convert.ToString((((Range)worksheet.Cells[i, "H"]).Value));                            
                            obidcode.CardFormatType = Convert.ToString((((Range)worksheet.Cells[i, "I"]).Value));
                            obidcode.HHuFormatType = Convert.ToString((((Range)worksheet.Cells[i, "J"]).Value));
                            SQLSPS.INSOBISs(obidcode.ObisCode, obidcode.Obis, obidcode.ObisFarsiDesc, obidcode.ObisLatinDesc, obidcode.ObisArabicDesc, Convert.ToDecimal(deviceTypeId.Value), obidcode.OBISUnitDesc, obidcode.ObisTypeID, obidcode.Format, obidcode.ClassID, obidcode.CardFormatType, obidcode.HHuFormatType, "",FixedOBISCode, returnUnitConvertType,returnObisType, OBISID, Result, ErrMsg);
                            if (ErrMsg.Value == "")
                                SQLSPS.InsobisToSoftversion(Convert.ToDecimal(OBISID.Value), Convert.ToDecimal(softversionToDeviceModelId.Value), Result, ErrMsg);
                        }
                }

                workbook.Close(0, 0, 0);
            }
            catch (Exception ex)
            {
                //workbook.SaveAs(_path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close(0, 0, 0);
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        

        private void GidMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (GidMain.SelectedItem!=null)
                {
                    Selecteditem = new ShowOBISs_Result();
                    Selecteditem = (ShowOBISs_Result)GidMain.SelectedItem;
                    obisid = Selecteditem.OBISID;
                    Griddown.DataContext = Selecteditem;
                    string Filter = " and Main.UnitGroupID=" + Selecteditem.UnitGroupID.ToString();
                    RefreshCmbUnitName(Filter);
                    
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
       
        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh("");
        }

       

        private void CmbUnitName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        
	}
}