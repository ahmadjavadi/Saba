using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Globalization;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;
using SABA_CH.UI;
using SABA_CH.VEEClasses;
using Application = Microsoft.Office.Interop.Excel.Application;
using Button = System.Windows.Controls.Button;
using Point = System.Windows.Point;
using Window = System.Windows.Window;

namespace SABA_CH
{
    /// <summary>
    /// Interaction logic for Meters.xaml
    /// </summary>
    public partial class Meters : System.Windows.Window
    {
        public decimal? MeterSoftversionToDeviceModelId = 1000000, MeterDeviceModelId = 1000000, ModemSoftversionToDeviceModelId = 1000000, ModemDeviceModelId = 1000000, LocationId = 1000000, meterID=1000000,CustomerId,ModemId;
        public ShowDeviceModel_Result SelectedModem = null;
        public ShowDeviceModel_Result SelectedMeter = null;
        public ShowLocations_Result SelectedLocations = null;
        public bool IsEditing;
        //public int GroupType = 0;
        public ShowSoftversionToDeviceModel_Result SoftversionOfModem = null;
        public ShowSoftversionToDeviceModel_Result SoftversionOfMeter = null;
        public List<MeterToGr> LstMeterTogroup = null;
        public ShowButtonAccess_Result ButtonAccess = null;
        public int SelectedRow;
        public ShowTranslateofLable Tr = null;
        public ObjectParameter MeterId = new ObjectParameter("MeterID", 1000000);
        public ObjectParameter Result = new ObjectParameter("Result", 1000000);
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public ShowButtonAccess Us = null;
        public readonly int WindowId = 3;
        public bool IsNew;
        public int ShowType;
        public bool Isnew = false;
        public string NewMeterNumber = "";
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }       

        public TabControl Tab { set { _tabCtrl = value; } }
        public ShowMeter_Result Meter;
        public Meters(int type,string newMeterNumber,bool isNew,decimal? meterId)
        {
            try
            {
                ShowType = type;
                InitializeComponent();
                
                RefreshDataGrid("");
                meterID = meterId;
                Tr = CommonData.translateWindow(WindowId);
                CommonData.ChangeFlowDirection(MGrid);
                CommonData.ChangeFlowDirection(Griddown);
                GridLabel.DataContext = Tr.TranslateofLable;
                this.DataContext = Tr.TranslateofLable;
                TranslateDataGrid();
                TranslateMeterToGroup();
                ShowType = type;
                this.NewMeterNumber = newMeterNumber;
                Isnew = isNew;               
                //chkInDirect. false;
                //chkDirect.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public void TranslateDataGrid()
        {
            try
            {
                GridMain.Columns[0].Header = Tr.TranslateofLable.Object4;
                GridMain.Columns[1].Header = Tr.TranslateofLable.Object1;
                GridMain.Columns[2].Header = Tr.TranslateofLable.Object2;
                //GridMain.Columns[3].Header = tr.TranslateofLable.Object3;
                GridMain.Columns[3].Header = Tr.TranslateofLable.Object5;
                GridMain.Columns[4].Header = Tr.TranslateofLable.Object6;
                GridMain.Columns[5].Header = Tr.TranslateofLable.Object7;
                GridMain.Columns[6].Header = Tr.TranslateofLable.Object11;
                GridMain.Columns[7].Header = Tr.TranslateofLable.Object10;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public void TranslateMeterToGroup()
        {
            try
            {
                MeterToGroup.Columns[0].Header = Tr.TranslateofLable.Object12;
                MeterToGroup.Columns[1].Header = Tr.TranslateofLable.Object13;
                ToolStripButtonImport.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public  void RefreshDataGrid(string filter)
        {
            try
            {
               // IsNew = false;
                //GridMain.ItemsSource = null;
                ShowMeter meters = new ShowMeter(filter);
                GridMain.ItemsSource = meters._lstShowMeters;
                Griddown.DataContext = meters;
                int i =-1;
                if (GridMain.Items.Count > 0)
                {
                    foreach (ShowMeter_Result item in GridMain.Items)
	                {
                        if (meterID != 1000000 || meterID!=null)
                            if (item.MeterID == meterID)
                            {
                                GridMain.SelectedItem = item;
                                break;
                            }
                        }
	                }
                    
                else if (GridMain.Items.Count==0)
                {
                    ToolStripButtonNew_Click(null,null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
            
        }
        
        private void ToolStripButtonNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsNew = true;
                CommonData.New(GridMeter);
                CustomerId = 10000000;
                ModemId = 10000000;
                meterID = 10000000;
                RefreshMeterToGroup();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonSave_Click(object sender, RoutedEventArgs e)
        {
            ObjectParameter modemId = new ObjectParameter("ModemID", 1000000);
            ObjectParameter customerId = new ObjectParameter("CustomerID", 1000000);
           
            try
            {

                // I insert for full MeterDeviceModelID

                if (txtMeterNumber.Text.Length > 7)
                {
                    string modemTypeNumber = txtMeterNumber.Text.Substring(0, 3);
                    if (modemTypeNumber == "207")
                    {
                        //MeterSoftversionToDeviceModelId = 2;
                        MeterDeviceModelId = 3;
                    }
                    else
                    {
                        MeterDeviceModelId = 1;
                        //MeterSoftversionToDeviceModelId = 21;
                    }
                    if (CommonData.MeterNumber == txtMeterNumber.Text)
                    {
                        if (txtMeterNumber.Text == CommonData.CardMeterNumber)
                        {
                            
                            switch (CommonData.SoftwareVersion)
                            {
                                case "RSASEWM303L0D3051301": MeterSoftversionToDeviceModelId = 9; break;
                                case "RSASEWM303L0D3072701": MeterSoftversionToDeviceModelId = 10; break;
                                case "RSASEWM303L0D3082701": MeterSoftversionToDeviceModelId = 11; break;
                                case "RSASEWM303L0I3082701": MeterSoftversionToDeviceModelId = 12; break;
                                case "RSASEWM303L0I3072701": MeterSoftversionToDeviceModelId = 13; break;
                                case "RSASEWM303L0I3051301": MeterSoftversionToDeviceModelId = 14; break;
                                case "RSASEWM303L0I3071702": MeterSoftversionToDeviceModelId = 15; break;
                                case "RSASEWM303L0I3093002": MeterSoftversionToDeviceModelId = 16; break;
                                case "RSASEWM303L0D3093002": MeterSoftversionToDeviceModelId = 17; break;
                                case "RSASEWM303L0I4120903": MeterSoftversionToDeviceModelId = 18; break;
                                case "RSASEWM303L0D4120903": MeterSoftversionToDeviceModelId = 19; break;
                                case "RSASEWM303L0D6112104": MeterSoftversionToDeviceModelId = 20; break;
                                case "RSASEWM303L0I6112104": MeterSoftversionToDeviceModelId = 21; break;
                                default:
                                    MeterSoftversionToDeviceModelId = 21;
                                    break;
                            }                                
                        };
                    }
                }               
                //
                bool meterType;
                if (chkDirect.IsChecked == true)
                    meterType = true; 
                else
                    meterType = false;
                int len = txtMeterNumber.Text.Length;
                if (len > 6)
                    if (txtMeterNumber.Text.Substring(len - 6, 6) == "000000")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message52);
                        return;
                    }
                if (txtMeterNumber.Text=="")
                {
                    //CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message79);
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message79);
                    return;
                }
               
                string customerAdd = CheckMeterForCustomerandGroup(txtMeterNumber.Text);
                string customerWrite = txtCustomerName.Text.Trim();
                if ((customerWrite.Length<1 && customerAdd.Contains("True")) || (customerAdd.Contains("Customer") && customerWrite.Length < 1))
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message93);
                    return;
                }

                IsEditing = false;
                if (IsNew)
                {
                    Result = new ObjectParameter("Result", 1000000);
                    ErrMsg = new ObjectParameter("ErrMsg", "");
                    if (txtModemNumber.Text == "" && txtsimNumber.Text == "")
                        ModemId = null;
                    if (txtCustomerName.Text == "")
                        CustomerId = null;
                    if (MeterDeviceModelId == 1000000)
                        MeterDeviceModelId = null;

                    bool isDirect = txtMeterNumber.Text.StartsWith("193");
                    if (txtMeterNumber.Text.StartsWith("19") && meterType != isDirect)
                        meterType = isDirect;

                    SQLSPS.InsMeter(txtMeterNumber.Text, MeterDeviceModelId, meterType,null,null, ModemId,CustomerId, 1,1,true, MeterId, ErrMsg, Result);
                    if (ErrMsg.Value.ToString()!="")
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                        return;
                    }
                    meterID = Convert.ToDecimal(MeterId.Value);
                    SaveMeterToGroup(Convert.ToDecimal(MeterId.Value));
                    //if (ShowType == 1)
                    //{
                    SQLSPS.InsMeterToGroup(Convert.ToDecimal(MeterId.Value), 1, 1, Result, ErrMsg);
                    //}
                    if (CustomerId != null)
                    {
                        SQLSPS.InsMeterToCustomer(Convert.ToDecimal(MeterId.Value), CustomerId, true, DateTime.Now.ToPersianString(), Result, ErrMsg);                        
                        RefreshDataGrid("");
                    }
                        

                }
                else if (!IsNew)
                {
                    Result = new ObjectParameter("Result", 1000000);
                    ErrMsg = new ObjectParameter("ErrMsg", "");
                    if (ModemId == 0)
                        ModemId = null;
                    if (CustomerId == 0)
                        CustomerId = null;
                    if (MeterDeviceModelId == 1000000)
                        MeterDeviceModelId = null;

                    bool isDirect = txtMeterNumber.Text.StartsWith("193");
                    if (txtMeterNumber.Text.StartsWith("19") && meterType != isDirect)
                        meterType = isDirect;

                    SaveMeterToGroup(Convert.ToDecimal(MeterId.Value));
                    

                    //SQLSPS.DelMeterToCustomer(meterID, customerID, Result, ErrMsg);
                    //if (customerID != null)
                    //  SQLSPS.InsMeterToCustomer(meterID, customerID, true, DateTime.Now.ToPersianString(), Result, ErrMsg);
                    if (ErrMsg.Value.ToString() != "")
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                    } 
                }

                if (txtMeterNumber.Text.StartsWith("207"))
                    MeterSoftversionToDeviceModelId = 2;
                SQLSPS.UpdMeter(txtMeterNumber.Text, MeterDeviceModelId, meterType, MeterSoftversionToDeviceModelId, ModemId, CustomerId, true, meterID, ErrMsg, Result);

                ShowMeter_Result MeterInfo = SQLSPS.ShowMeter(" and Main.Valid=1 and Main.MeterNumber='" + txtMeterNumber.Text + "'");

                VeeMeterData vee = new VeeMeterData();
                //if (txtMeterNumber.Text.StartsWith("207"))
                //{
                //    vee.Vee207data(Convert.ToDecimal(meterID), txtMeterNumber.Text, CustomerId);
                //}
                //else
                //    vee.Vee303data_NewCustomer(Convert.ToDecimal(meterID), txtMeterNumber.Text, CustomerId, MeterInfo.Softversion);

                IsNew = false;
                string filter = "";
        
                if (CommonData.mainwindow.SelectedGroup.GroupID != -1)
                    filter = "and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + CommonData.mainwindow.SelectedGroup.GroupID + "and  GroupType=" + CommonData.mainwindow.SelectedGroup.GroupType + ") ";

                RefreshDataGrid("");

                if (CommonData.mainwindow. SelectedGroup.GroupID != -1)
                    filter = "and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + CommonData.mainwindow.SelectedGroup.GroupID + "and  GroupType=" + CommonData.mainwindow.SelectedGroup.GroupType + ") ";
                CommonData.mainwindow.RefreshSelectedMeters(filter);

                MessageBox.Show("ذخیره به درستی انجام شد");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در ذخیره داده");//CommonData.mainwindow.tm.TranslateofMessage.Message92);
            }
        }
       
        public void SaveMeterToGroup(decimal meterId)
        {
            try
            {
                Result = new ObjectParameter("Result", 1000000);
                ErrMsg = new ObjectParameter("ErrMsg", "");
                for (int i = 0; i < LstMeterTogroup.Count; i++)
                {
                    MeterToGr MTG = new MeterToGr();
                    MTG = LstMeterTogroup[i];
                    if (!Convert.ToBoolean(MTG.Isvisable))
                    {
                        SQLSPS.DelMeterToGroup(meterID, MTG.GroupId, MTG.GroupType, Result, ErrMsg);
                        if (ErrMsg.Value.ToString() != "")
                        {
                            MessageBox.Show(ErrMsg.Value.ToString());
                        }
                    }
                    if (Convert.ToBoolean(MTG.Isvisable))
                    {
                        SQLSPS.InsMeterToGroup(meterID, MTG.GroupId, MTG.GroupType, Result, ErrMsg);
                        if (ErrMsg.Value.ToString() != "")
                        {
                            MessageBox.Show(ErrMsg.Value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }

        }
        private void ToolStripButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (meterID==null)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message2);
                    return;
                }
                MessageBoxResult res = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message1, CommonData.mainwindow.tm.TranslateofMessage.Message11, MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    Result = new ObjectParameter("Result", 1000000);
                    ErrMsg = new ObjectParameter("ErrMsg", "");
                    SQLSPS.DelMeter(meterID, Result, ErrMsg);
                    if (ErrMsg.Value.ToString() != "")
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                    }
                    RefreshDataGrid("");
                    if ( GridMain.ItemsSource!=null)
                    {
                        GridMain.SelectedIndex = GridMain.Items.Count - 1;
                    }
                    string filter = "";
                    if (CommonData.mainwindow.SelectedGroup.GroupID != -1)
                        filter = "and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + CommonData.mainwindow.SelectedGroup.GroupID + "and  GroupType=" + CommonData.mainwindow.SelectedGroup.GroupType + ") ";

                    CommonData.mainwindow.RefreshSelectedMeters(filter);
                }
                MessageBox.Show("عملیات به درستی انجام شد");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در انجام عملیات");
            }
        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid("");
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
                    ReadMeterFromExcel(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void GridMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!IseDiting())
                    return;
                Meter = new ShowMeter_Result();

                IsNew = false;
                
                if (GridMain.SelectedItem != null)
                {
                    if (ShowType == 1)
                    {
                        meterID = CommonData.SelectedMeterID;
                        for (int i = 0; i < GridMain.Items.Count; i++)
                        {
                           ShowMeter_Result  newmeter = (ShowMeter_Result)GridMain.Items[i];
                           if (newmeter.MeterID == meterID)
                               Meter = newmeter;

                        }
                        
                    }
                    else
                        Meter = (ShowMeter_Result)GridMain.SelectedItem;
                    GridMeter.DataContext = Meter;
                    txtMeterNumber.Text = Meter.MeterNumber;
                    
                    CmbManufacturer.Text = Meter.ManufacturerName;
                    //CmbMeterType.Text = meter.DeviceModelName.ToString();
                    //CmbMeterSoftversion.Text = meter.Softversion.ToString();   
                    meterID = Meter.MeterID;
                    if (Meter.CustomerID == 0 && CustomerId > 0)
                    { }
                    else
                    {
                        CustomerId = Meter.CustomerID;
                    }
                    CommonData.CustomerId = CustomerId;
                    txtModemNumber.Text = Meter.ModemNumber;
                    txtsimNumber.Text = Meter.SimNumber;
                    txtCustomerName.Text = Meter.CustomerName;
                    txtNationalCode.Text = Meter.DossierNumber;
                    ModemId = Meter.ModemID;

                    
                    if (Meter.IsDirect)
                    {
                        chkDirect.IsChecked = true;
                        chkInDirect.IsChecked = false;
                    }
                    else
                    {
                        chkInDirect.IsChecked = true;
                        chkDirect.IsChecked = false;
                    }
                    RefreshMeterToGroup();

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        //public void ChangelstMeterTogroup(bool Isvisable, int i)
        //{
        //    try
        //    {
        //        if (lstMeterTogroup.Count > i && i >= 0)
        //        {
        //            MeterToGr UsG = new MeterToGr();
        //            UsG = lstMeterTogroup[i]; 
        //            UsG.Isvisable = Isvisable;
        //            lstMeterTogroup[i] = UsG;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
        //}
        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                ShowButtonAccess_Result ua= CommonData.ShowButtonBinding("", WindowId);
                toolBar1.DataContext = ua;                    
                _tabCtrl.SelectedItem = _tabPag;
                if (!_tabCtrl.IsVisible)
                {

                    _tabCtrl.Visibility = Visibility.Visible;
                }
                if (ShowType == 1)
                {
                    if (Isnew)
                    {
                        ToolStripButtonNew_Click(null, null);
                        txtMeterNumber.Text = NewMeterNumber;
                    }
                    else
                    {
                        txtMeterNumber.Text = NewMeterNumber;
                        meterID = CommonData.SelectedMeterID;
                        GridMain_SelectionChanged(null, null);
                        if (NewMeterNumber.Length > 6)
                            if (NewMeterNumber.Substring(2, 1) == "4")
                            {
                                chkDirect.IsChecked = false;
                                chkInDirect.IsChecked = true;
                            }
                            else if (NewMeterNumber.Substring(2, 1) == "3")
                            {
                                chkInDirect.IsChecked = false;
                                chkDirect.IsChecked = true;
                            }
                    }
                }
                if (ShowType == 0)
                {
                    //if (isnew)
                    if(ua.CanInsert)
                     ToolStripButtonNew_Click(null, null);
                }
                ShowType = 100;
                
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
                if (!IseDiting())
                {
                    e.Cancel = true;
                    return;
                }
                _tabCtrl.Items.Remove(_tabPag);
                if (!_tabCtrl.HasItems)
                {

                    _tabCtrl.Visibility = Visibility.Hidden;

                }
                ClassControl.OpenWin[WindowId] = false;
                CommonData.mainwindow.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        //private void cmbModemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //if (cmbModemType.SelectedItem!=null)
        //    //{
        //    //    SelectedModem = (ShowDeviceModel_Result)cmbModemType.SelectedItem;
        //    //    ModemDeviceModelID = Convert.ToDecimal(SelectedModem.DeviceModeleID);
        //    //    txtModemManufacturerName.Text = SelectedModem.ManufacturerName.ToString();
        //    //    string filter = " and DeviceType.DeviceTypeID=2 " + " and Main.DeviceModelID=" + SelectedModem.DeviceModeleID.ToString();
        //    //    ShowSoftversionToDeviceModel ModemSoftversion = new ShowSoftversionToDeviceModel(filter);
        //    //    cmbModemSoftware.ItemsSource = ModemSoftversion.CollectShowSoftversionToDeviceModel;
        //    //    if (cmbModemSoftware.Items.Count > 0)
        //    //        cmbModemSoftware.SelectedIndex = 0;
        //    //}

        //}

        //private void CmbMeterSoftversion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        //if (CmbMeterSoftversion.SelectedItem != null)
        //        //{
        //        //    SoftversionOfMeter = (ShowSoftversionToDeviceModel_Result)CmbMeterSoftversion.SelectedItem;
        //        //    MeterSoftversionToDeviceModelID = SoftversionOfMeter.SoftversionToDeviceModelID;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
            
        //}

        //private void CmbMeterType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (CmbMeterType.SelectedItem != null)
        //        {
        //            SelectedMeter = (ShowDeviceModel_Result)CmbMeterType.SelectedItem;
        //            MeterDeviceModelID = Convert.ToDecimal(SelectedMeter.DeviceModeleID);

        //            string filter = " and DeviceType.DeviceTypeID=1 " + " and Main.DeviceModelID=" + SelectedMeter.DeviceModeleID.ToString();
        //            ShowSoftversionToDeviceModel MeterSoftversion = new ShowSoftversionToDeviceModel(filter);
        //            //CmbMeterSoftversion.ItemsSource = MeterSoftversion.CollectShowSoftversionToDeviceModel;
        //            //if (CmbMeterSoftversion.Items.Count > 0)
        //            //    CmbMeterSoftversion.SelectedIndex = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
        //}
        //private void cmbModemSoftware_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //if (cmbModemSoftware.SelectedItem!=null)
        //    //{
        //    //    SoftversionOfModem = (ShowSoftversionToDeviceModel_Result)cmbModemSoftware.SelectedItem;
        //    //    ModemSoftversionToDeviceModelID = Convert.ToDecimal(SoftversionOfModem.SoftversionToDeviceModelID);

        //    //}
        //}

        //private void cmbLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //if (cmbLocation.SelectedItem!=null)
        //    //{
        //    //    SelectedLocations = (ShowLocations_Result)cmbLocation.SelectedItem;
        //    //    LocationID = Convert.ToDecimal(SelectedLocations.LocationID);
        //    //}
        //}

        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                (sender as Grid).DataContext = Tr.TranslateofLable;
                //(sender as Grid).FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void bnModems_Click(object sender, RoutedEventArgs e)
        {          
            try
            {
                ButtonAccess = CommonData.ShowButtonBinding("", 2);
                if (ButtonAccess.CanShow)
                {
                    Modems m = new Modems();
                    m.Title = CommonData.mainwindow.translateWindowName.TranslateofLable.Object2;
                    m.Tab = CommonData.mainwindow.tabControl1;
                    ClosableTab theTabItem = new ClosableTab();
                    theTabItem.Title = m.Title;
                    theTabItem.windowname = m;
                    if (IsNew)
                        m.MeterNumber = txtMeterNumber.Text;
                    else if (Meter.MeterNumber != null)
                        m.MeterNumber = Meter.MeterNumber;
                    else
                        m.MeterNumber = "";
                    CommonData.mainwindow.tabControl1.Items.Add(theTabItem);
                    theTabItem.Focus();
                    m.TabPag = theTabItem;
                    m.Owner = CommonData.mainwindow;
                    m.ShowDialog();
                    if (m.modemID != this.ModemId && m.modemID !=null)
                        IsEditing = true;
                    this.ModemId = m.modemID;
                    txtModemNumber.Text = m.modemNumber;
                    txtsimNumber.Text = m.simNumber;
                    
                }
                
                else
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message55);
                    return;
                }
                this.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }

        }
        //public void Refreshfrmselect()
        //{
        //    try
        //    {
        //        //string filter = "";

        //        //if (CommonData.list.Count > 0)
        //        //    CommonData.list.RemoveRange(0, CommonData.list.Count);
        //        //SabaNewEntities Bank = new SabaNewEntities();

        //        //foreach (ShowModem_Result item in Bank.ShowModem(filter) )
        //        //{
        //        //    frmselectlist New = new frmselectlist();
        //        //    New.ID = Convert.ToDecimal(item.ModemID.ToString());
        //        //    New.Code = item.ModemNumber.ToString();
        //        //    New.Desc = item.SimNumber.ToString();
        //        //    CommonData.list.Add(New);
        //        //}
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }

        //}
        public bool IseDiting()
        {
            try
            {
                if (IsEditing)
                {
                    MessageBoxResult res = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message5, CommonData.mainwindow.tm.TranslateofMessage.Message11, MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.No)
                        return false;
                    else
                    {
                        IsEditing = false;
                        return true;
                    }
                }
                else
                    return true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
                return false;
            }
        }
       
        public void RefreshMeterToGroup()
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();

                string p = CommonData.ProvinceCode;
                string filter = "";
                try
                {
                    int value = Int32.Parse(p, NumberStyles.HexNumber);
                    value = value / 1000;
                    if (value == 1)
                        value = 8;
                    else if (value == 8)
                        value = 1;

                    if (value != 0)
                        filter = " and ProvinceID in (0," + value + ")";
                }
                catch
                {
                    filter = "";
                }
                             
                LstMeterTogroup = new List<MeterToGr>();
                bank.Database.Connection.Open();
                //for the first row it should be null
                MeterToGr UsG;// = new MeterToGr();
                //UsG.MeterID =0;
                //UsG.GroupID = 0;
                //UsG.GroupName = "";
                //UsG.Isvisable = false;
                //UsG.GroupType = 0;
                //lstMeterTogroup.Add(UsG);

                //
                foreach (ShowMeterToGroup_Result item in bank.ShowMeterToGroup(filter, meterID, CommonData.UserID))
                {
                    UsG = new MeterToGr();
                    UsG.MeterId = item.MeterID;
                    UsG.GroupId = item.GID;
                    UsG.GroupName = item.GroupName;
                    UsG.Isvisable = Convert.ToBoolean(item.Isvisable);
                    UsG.GroupType = item.GroupType;
                    LstMeterTogroup.Add(UsG);
                }
                bank.Database.Connection.Close();
                bank.Dispose();
                MeterToGroup.ItemsSource = null;
                MeterToGroup.ItemsSource = LstMeterTogroup;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

      

        private void MeterToGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectedRow = MeterToGroup.SelectedIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

     
        private void Button_Click(object sender, RoutedEventArgs e)
        {


            Select slc = new Select();
            CommonData.selectedDetail = null;
            Point locationFromScreen = (sender as Button).PointToScreen(new Point(0, 0));
            PresentationSource source = PresentationSource.FromVisual(this);
            Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
            slc.Top = targetPoints.Y;
            slc.Left = targetPoints.X - slc.Width;
            fillFrmSelectDatagrid(slc);           
            TranslateFrmSelectDatagrid(slc);
            slc.Title = Tr.TranslateofLable.Object20;
            slc.ShowDialog();
            if (CommonData.selectedDetail!=null)
            {
                this.CustomerId = CommonData.selectedDetail.ID;
                txtCustomerName.Text = CommonData.selectedDetail.Desc;
                txtNationalCode.Text = CommonData.selectedDetail.Code;
            }

            RefreshMeterToGroupDataGrid("");
            this.Focus();
        }

        private void RefreshMeterToGroupDataGrid(string v)
        {
            try
            {


            bool isDirect = false , meterType=false;
            if (chkDirect.IsChecked == true)
                meterType = true;
            else
                meterType = false;
            if (txtMeterNumber.Text.StartsWith("193"))
                isDirect = true;
            if (txtMeterNumber.Text.StartsWith("19") && meterType != isDirect)
                meterType = isDirect;

            if (ModemId == 0)
                ModemId = null;
            if (CustomerId == 0)
                CustomerId = null;
            if (MeterDeviceModelId == 1000000)
                MeterDeviceModelId = null;
                //if (!IsNew)
                //{
                //    SaveMeterToGroup();
                //    SQLSPS.UPDMeter(txtMeterNumber.Text, MeterDeviceModelID, MeterType, null, modemID, CommonData.selectedDetail.ID, true, meterID, ErrMsg, Result);
                //    RefreshDataGrid("");
                //    SQLSPS.UPDMeter(txtMeterNumber.Text, MeterDeviceModelID, MeterType, null, modemID, null, true, meterID, ErrMsg, Result);
                //}
            IsEditing = true;

        }
            catch (Exception)
            {
            }
        }

        private void fillFrmSelectDatagrid(Select slc)
        {
            try
            {
                var lstCustomer = new List<SelectDetail>();
                string Filter = " and Main.CustomerID not in (Select Isnull(CustomerID,0) From Meter WHERE Valid=1)";
                ShowCustomers customers=new ShowCustomers(Filter);
                foreach (var item in customers._lstShowCustomers)
                {
                    SelectDetail SD=new SelectDetail();
                    SD.ID = item.CustomerID;
                    SD.Code = item.DossierNumber;
                    SD.Desc = item.CustomerName +" "+ item.Customerfamily;
                    SD.Desc2 = item.WatersubscriptionNumber;
                    SD.Desc3 = item.ElecsubscriptionNumber;
                    lstCustomer.Add(SD);
                }
                
                slc.GridMain.Columns[0].Visibility = Visibility.Hidden;
                slc.GridMain.ItemsSource = lstCustomer;

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        private void TranslateFrmSelectDatagrid(Select slc)
        {
            try
            {
                slc.GridMain.Columns[1].Header = Tr.TranslateofLable.Object21;
                slc.GridMain.Columns[2].Header = Tr.TranslateofLable.Object17;
                slc.GridMain.Columns[3].Header = Tr.TranslateofLable.Object18;
                slc.GridMain.Columns[4].Header = Tr.TranslateofLable.Object19;

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        private void CmbManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                if (CmbManufacturer.SelectedItem != null)
                {
                    if (CmbManufacturer.SelectedItem != "")
                    {
                        ShowDeviceModel meterModel = new ShowDeviceModel(" and DeviceType.DeviceTypeID=1 and Main.ManufacturerName='" + CmbManufacturer.SelectedItem + "'");
                        //CmbMeterType.ItemsSource = MeterModel.CollectShowDeviceModel;
                        //CmbMeterType.SelectedIndex = 0;
                    }
                    //ISEditing = true;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void txtMeterNumber_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (IsNew)
                    GridMeter.DataContext = null;
                IsEditing = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

       
        public void ReadMeterFromExcel(string path)
        {
            try
            {
                Application app = new Application();
                Workbook workbook;
                Worksheet worksheet;
                workbook = app.Workbooks.Open(path, 0, false, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                // Get first Worksheet

                worksheet = (Worksheet)workbook.Sheets.get_Item(1);



                // Setting cell values
                Range range = worksheet.UsedRange;
                int c = range.Rows.Count;

                ShowMeter_Result newmeter = new ShowMeter_Result();
                ShowModem_Result newmodem = new ShowModem_Result();
                ShowCustomers_Result newCustomer = new ShowCustomers_Result();
                ShowCities_Result newCity = new ShowCities_Result();
                ShowCountries_Result newCountry = new ShowCountries_Result();
                ShowAreas_Result newArea = new ShowAreas_Result();
                ShowCatchments_Result newCatchment = new ShowCatchments_Result();
                ShowPlains_Result newPlain = new ShowPlains_Result();
                ShowProvinces_Result newProvince = new ShowProvinces_Result();
                ShowOffice_Result newOffice = new ShowOffice_Result();
                for (int i = 2; i < c; i++)
                {
                    Result = new ObjectParameter("Result", 1000000);
                    ErrMsg = new ObjectParameter("ErrMsg", "");
                    MeterId = new ObjectParameter("ErrMsg", 1000000);
                    ObjectParameter modemId = new ObjectParameter("ModemID", 1000000);
                    ObjectParameter customerId = new ObjectParameter("CustomerID", 1000000);
                    ObjectParameter deviceModelId = new ObjectParameter("DeviceModelID", 1000000);
                    ObjectParameter deviceTypeId = new ObjectParameter("DeviceTypeID", 1000000);
                    ObjectParameter softversionToDeviceModelId = new ObjectParameter("SoftversionToDeviceModelID", 1000000);
                    ObjectParameter cityId = new ObjectParameter("CityID", 1000000);
                    ObjectParameter locationId = new ObjectParameter("LocationID", 1000000);
                    newmeter = new ShowMeter_Result();
                    newmodem = new ShowModem_Result();
                    newCustomer = new ShowCustomers_Result();
                    newCity = new ShowCities_Result();
                    newCountry = new ShowCountries_Result();
                    newArea = new ShowAreas_Result();
                    newCatchment = new ShowCatchments_Result();
                    newPlain = new ShowPlains_Result();
                    newProvince = new ShowProvinces_Result();
                    newOffice = new ShowOffice_Result();
                    newmeter.MeterNumber = Convert.ToString((((Range)worksheet.Cells[i, "A"]).Value));
                    newmeter.ManufacturerName = Convert.ToString((((Range)worksheet.Cells[i, "B"]).Value));
                    newmeter.DeviceModelName = Convert.ToString((((Range)worksheet.Cells[i, "C"]).Value));
                    newmeter.MessageVersion = Convert.ToString((((Range)worksheet.Cells[i, "D"]).Value));
                    newmeter.Softversion = Convert.ToString((((Range)worksheet.Cells[i, "E"]).Value));
                    newmeter.IsDirect = Convert.ToString((((Range)worksheet.Cells[i, "F"]).Value));
                    newmodem.ModemNumber = Convert.ToString((((Range)worksheet.Cells[i, "G"]).Value));
                    newmodem.SimNumber = Convert.ToString((((Range)worksheet.Cells[i, "H"]).Value));
                    newmodem.DeviceModelName = Convert.ToString((((Range)worksheet.Cells[i, "I"]).Value));
                    newmodem.ManufacturerName = Convert.ToString((((Range)worksheet.Cells[i, "J"]).Value));
                    newmodem.Softversion = Convert.ToString((((Range)worksheet.Cells[i, "K"]).Value));
                    newmodem.MessageVersion = Convert.ToString((((Range)worksheet.Cells[i, "L"]).Value));

                    SQLSPS.InsDeviceModel(newmodem.DeviceModelName, newmodem.ManufacturerName, "Modem", newmodem.MessageVersion, deviceTypeId, deviceModelId, Result, ErrMsg);
                    SQLSPS.InsSoftversionToDeviceModel(newmodem.Softversion, Convert.ToDecimal(deviceModelId.Value), softversionToDeviceModelId, Result, ErrMsg);

                    deviceModelId = new ObjectParameter("DeviceModelID", 1000000);
                    softversionToDeviceModelId = new ObjectParameter("SoftversionToDeviceModelID", 1000000);

                    SQLSPS.InsDeviceModel(newmeter.DeviceModelName, newmeter.ManufacturerName, "Meter", newmeter.MessageVersion, deviceTypeId, deviceModelId, Result, ErrMsg);
                    SQLSPS.InsSoftversionToDeviceModel(newmeter.Softversion, Convert.ToDecimal(deviceModelId.Value), softversionToDeviceModelId, Result, ErrMsg);
                    SQLSPS.InsMeter(newmeter.MeterNumber, Convert.ToDecimal(deviceModelId.Value), newmeter.IsDirect,null, Convert.ToDecimal(softversionToDeviceModelId.Value), Convert.ToDecimal(modemId.Value), Convert.ToDecimal(CustomerId.Value), 1,1, true, MeterId, ErrMsg, Result);

                    newCountry.CountryName = Convert.ToString((((Range)worksheet.Cells[i, "M"]).Value));
                    newProvince.ProvinceName = Convert.ToString((((Range)worksheet.Cells[i, "N"]).Value));
                    newPlain.PlainName = Convert.ToString((((Range)worksheet.Cells[i, "O"]).Value));
                    newCatchment.CatchmentName = Convert.ToString((((Range)worksheet.Cells[i, "P"]).Value));
                    newCity.CityName = Convert.ToString((((Range)worksheet.Cells[i, "Q"]).Value));
                    newArea.AreaName = Convert.ToString((((Range)worksheet.Cells[i, "R"]).Value));
                    newOffice = Convert.ToString((((Range)worksheet.Cells[i, "S"]).Value));
                    newCustomer.CustomerName = Convert.ToString((((Range)worksheet.Cells[i, "T"]).Value));
                    newCustomer.Customerfamily = Convert.ToString((((Range)worksheet.Cells[i, "U"]).Value));
                    newCustomer.CustomerTel = Convert.ToString((((Range)worksheet.Cells[i, "V"]).Value));
                    newCustomer.CustomerAddress = Convert.ToString((((Range)worksheet.Cells[i, "W"]).Value));
                    newCustomer.NationalCode = Convert.ToString((((Range)worksheet.Cells[i, "X"]).Value));
                    newCustomer.MobileNumber = Convert.ToString((((Range)worksheet.Cells[i, "Y"]).Value));
                    newCustomer.PostCode = Convert.ToString((((Range)worksheet.Cells[i, "Z"]).Value));
                    newCustomer.WatersubscriptionNumber = Convert.ToString((((Range)worksheet.Cells[i, "AA"]).Value));
                    newCustomer.ElecsubscriptionNumber = Convert.ToString((((Range)worksheet.Cells[i, "AB"]).Value));
                    newCustomer.Longitude = Convert.ToString((((Range)worksheet.Cells[i, "AC"]).Value));
                    newCustomer.Latitude = Convert.ToString((((Range)worksheet.Cells[i, "AD"]).Value));

                    ShowCountries countries = new ShowCountries("  and CountryName='" + newCountry.CountryName + "'");
                    foreach (var item in countries._lstShowCountries)
                    {
                        newCountry.CountryID = item.CountryID;
                    }
                    ShowProvinces province = new ShowProvinces("  and ProvinceName='" + newProvince.ProvinceName + "'");
                    foreach (var item in province._lstShowProvinces)
                    {
                        newProvince.ProvinceID = item.ProvinceID;
                    }
                    ShowPlains plain = new ShowPlains(" and PlainName='" + newPlain.PlainName + "'");
                    foreach (var item in plain._lstShowPlains)
                    {
                        newPlain.PlainID = item.PlainID;
                    }

                    ShowCatchments catchment = new ShowCatchments(" and CatchmentName='" + newCatchment.CatchmentName + "'");
                    foreach (var item in catchment._lstShowCatchments)
                    {
                        newCatchment.CatchmentID = item.CatchmentID;
                    }

                    ShowAreas area = new ShowAreas(" and AreaName='" + newArea.AreaName + "'");
                    foreach (var item in area._lstShowAreas)
                    {
                        newArea.AreaID = item.AreaID;
                    }

                    ShowOffice office = new ShowOffice(" and OfficeDesc='" + newOffice.OfficeDesc + "'");
                    foreach (var item in office._lstShowOffice)
                    {
                        newOffice.OfficeID = item.OfficeID;
                    }

                    SQLSPS.InsCities("", newCity.CityName, newCountry.CountryID, newProvince.ProvinceID, cityId, Result, ErrMsg);
                    SQLSPS.InsLocations(newPlain.PlainID, newCatchment.CatchmentID, newArea.AreaID, Convert.ToDecimal(cityId.Value), locationId, Result, ErrMsg);

                    SQLSPS.InsCustomers(newCustomer.CustomerName, newCustomer.Customerfamily, newCustomer.CustomerTel, newCustomer.CustomerAddress, newCustomer.WatersubscriptionNumber, newCustomer.ElecsubscriptionNumber, Convert.ToDecimal(locationId.Value), newCustomer.NationalCode,
                                        newCustomer.MobileNumber, newCustomer.Longitude, newCustomer.Latitude, newCustomer.PostCode, newOffice.OfficeID, null, null, null, newCustomer.DossierNumber, newCustomer.Flowindossier, newCustomer.Diameterofpipe, "1", newCustomer.WellLicense, newCustomer.WellAddress, newCustomer.FatherName,1, customerId, Result, ErrMsg);

                    SQLSPS.InsMeterToCustomer(Convert.ToDecimal(MeterId.Value), Convert.ToDecimal(customerId.Value), true, DateTime.Now.ToPersianString(), Result, ErrMsg);
                    
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void txtMeterNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (GridMain.Items.Count == 0)
                    IsNew = true;

                if (txtMeterNumber.Text.StartsWith("207"))
                {
                    CmbManufacturer.SelectedIndex = 0;

                    ShowDeviceModel meterModel = new ShowDeviceModel(" and DeviceType.DeviceTypeID=1 and Main.DeviceModelName ='207'");
                    //CmbMeterType.ItemsSource = MeterModel._lstShowDeviceModel;
                    //CmbMeterType.SelectedIndex = CmbMeterType.Items.Count - 1;

                }
                else if (txtMeterNumber.Text.StartsWith("19"))
                {                    
                     
                    ShowDeviceModel meterModel = new ShowDeviceModel(" and DeviceType.DeviceTypeID=1 and Main.DeviceModelName ='303'");
                    //CmbMeterType.ItemsSource = MeterModel.CollectShowDeviceModel;
                    //CmbMeterType.SelectedIndex = CmbMeterType.Items.Count - 1;

                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void chkDirect_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                chkInDirect.IsChecked = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void chkDirect_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if(chkInDirect!= null)
                    chkInDirect.IsChecked = true;
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        //private void CustomerDel_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (customerID != null)
        //        {
        //            txtCustomerName.Text = "";
        //            txtNationalCode.Text = "";
        //        }
        //           //SQLSPS.DelMeterToCustomer(meterID, customerID, Result, ErrMsg);
        //        //RefreshDataGrid("");

        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
        //    }
        //}

        private void chkInDirect_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                chkDirect.IsChecked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

     
        private void chkInDirect_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
            
                chkDirect.IsChecked = false;
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void MNUDelModemNumber_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ModemId =null;
                txtModemNumber.Text = "";
                txtsimNumber.Text = "";
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void MNUDelCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.CustomerId = null;
                txtCustomerName.Text = "";
                txtNationalCode.Text = "";
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void GridMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GridMain_SelectionChanged(sender, null);
        }

        private void GridMain_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcount.Content =rowcount+"="+ GridMain.Items.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void MeterToGroup_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                     rowcount= CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcountgroup.Content =rowcount+"="+ (MeterToGroup.Items.Count-1);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
            
        }

        public string CheckMeterForCustomerandGroup(string meternumber)
        {
            try
            {
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                ObjectParameter IsAllow = new ObjectParameter("IsAllow", false);
                ObjectParameter CustomerID = new ObjectParameter("CustomerID", 10000000000000000);
                ObjectParameter GroupID = new ObjectParameter("GroupID", 10000000000000000);


                ShowMeter_Result Meter = new ShowMeter_Result();
                Meter = SQLSPS.ShowMeter(" and Main.MeterNumber='" + meternumber + "'");
                if (Meter == null) Meter.MeterID = 0;

                SQLSPS.ShowCustomerIDMeterID(Meter.MeterID, IsAllow, CustomerID, GroupID, Result, ErrMSG);
                //if (Convert.ToBoolean(IsAllow.Value))
                //{
                    if (NumericConverter.IntConverter(CustomerID.Value.ToString()) != 0)
                    {
                        if (NumericConverter.IntConverter(GroupID.Value.ToString()) != 0)
                            return "True";
                        else
                            return "NO Group";
                    }
                    else
                    {
                        if (NumericConverter.IntConverter(GroupID.Value.ToString()) != 0)
                            return "NO Customer";
                        else
                            return "NO Group and Customer";
                    }
                //}
                //else
                //    return "NO Allowed";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
                return "False";
            }
        }
        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentRowIndex = MeterToGroup.Items.IndexOf(MeterToGroup.CurrentItem);

            if (currentRowIndex>=0)
            {
                MeterToGr MG = new MeterToGr();
                MG = LstMeterTogroup[currentRowIndex];
                MG.Isvisable = !MG.Isvisable;
                LstMeterTogroup[currentRowIndex] = MG;
            }
        
        }

      
        //private void Isvisable_Checked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (SelectedRow == 0)
        //            return;
        //        ChangelstMeterTogroup(true, SelectedRow);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
        //    }
        //}
        //private void Isvisable_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ChangelstMeterTogroup(false, SelectedRow);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
        //    }
        //}

    }
}
