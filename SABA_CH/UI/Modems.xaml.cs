using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for Modems.xaml
    /// </summary>
    public partial class Modems : System.Windows.Window
    {
        public ShowDeviceModel_Result SelectedModemtype = null;
        public ShowModem_Result SelectedModem = null;
        public bool IsClose = true;
        public bool ISEditing = false;
        public ShowSoftversionToDeviceModel_Result SelectedVersion = null;
        public ShowButtonAccess_Result us = null;
        public  decimal? modemID = 1000000;
        public  decimal?   ModemSoftversionToDeviceModelID = 1000000, ModemDeviceModelID = 1000000, LocationID = 1000000;
        public string modemNumber = "", simNumber = "";
        public ShowTranslateofLable tr = null;
        public readonly int windowID = 2;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public bool IsNew = false;
        public string MeterNumber = "";
        public bool Isduplicate = false;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }        
        public ObjectParameter Result = new ObjectParameter("Result", 1000000);
        public ObjectParameter ModemID = new ObjectParameter("ModemID", 1000000);
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public TabControl Tab { set { tabCtrl = value; } }
        public Modems()
        {
            InitializeComponent();
            tr = CommonData.translateWindow(windowID);
            GridLabel.DataContext = tr.TranslateofLable;            
            MGrid.DataContext = tr.TranslateofLable;
            CommonData.ChangeFlowDirection(MGrid);
            Refresh();
            
            CommonData.ChangeFlowDirection(MGrid);
            TranslateDataGrid();
            
        }
        public void TranslateDataGrid()
        {
            try
            {
                MainGrid.Columns[0].Header = tr.TranslateofLable.Object1;
                MainGrid.Columns[1].Header = tr.TranslateofLable.Object3;
                MainGrid.Columns[2].Header = tr.TranslateofLable.Object2;
                MainGrid.Columns[3].Header = tr.TranslateofLable.Object5;
                MainGrid.Columns[4].Header = tr.TranslateofLable.Object4;
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
                //MainGrid.ItemsSource = null;
                ShowModem modem = new ShowModem("");
                MainGrid.ItemsSource = modem._lstShowModem;
                if (MainGrid.Items.Count > 0)
                    MainGrid.SelectedIndex = MainGrid.Items.Count - 1;

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
                SelectedModem = null;
                CommonData.New(GridDown);
                GridDown.DataContext = null;
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
                ErrMsg = new ObjectParameter("ErrMsg", "");
                if (txtModemNumber.Text=="" ||txtSimNumber.Text=="")
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message54);
                    return;
                }
                try
                {
                    ulong.Parse(txtSimNumber.Text);
                }
                catch
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message26);
                    return;

                }
                if (IsNew)
                {
                    ModemID = new ObjectParameter("ModemID", 1000000);
                    SQLSPS.Insmodem(txtModemNumber.Text, txtSimNumber.Text, ModemSoftversionToDeviceModelID, ModemDeviceModelID, ModemID, Result, ErrMsg);
                    modemID = Convert.ToDecimal(ModemID.Value);
                    IsNew = false;
                }
                else if (!IsNew)
                    SQLSPS.Updmodem(txtModemNumber.Text, txtSimNumber.Text, ModemSoftversionToDeviceModelID, ModemDeviceModelID, modemID, Result, ErrMsg);

                if (ErrMsg.Value.ToString() != "")
                {
                    MessageBox.Show(ErrMsg.Value.ToString());
                    return;
                }
                else
                {
                    Refresh();

                }
                MessageBox.Show("ذخیره داده به درستی انجام شد");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در ذخیره داده");
            }
        }

        private void ToolStripButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (modemID == null)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message2);
                    return;
                }
                MessageBoxResult res = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message1, "Warning", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    Result = new ObjectParameter("Result", 1000000);
                    ErrMsg = new ObjectParameter("ErrMsg", "");
                    SQLSPS.DelModem(modemID, Result, ErrMsg);
                    CommonData.New(GridDown);
                    if (ErrMsg.Value.ToString() != "")
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                    }
                    Refresh();
                }
                MessageBox.Show("عملیات به درستی انجام شد");//CommonData.mainwindow.tm.TranslateofMessage.Message91);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در انجام عملیات");//CommonData.mainwindow.tm.TranslateofMessage.Message92);
            }
        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void ToolStripButtonImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                //this.DataContext = CommonData.setting;
                //MainGrid.DataContext = CommonData.setting;
                toolBar1.DataContext = CommonData.ShowButtonBinding("", windowID);
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
                if (Isduplicate)
                {
                    modemNumber="";
                    simNumber ="";
                    modemID = null;
                }
                
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
                IsNew = false;
                Isduplicate = false;
                SelectedModem = (ShowModem_Result)MainGrid.SelectedItem;
                if (SelectedModem != null)
                {
                             
                    IsClose = false;
                    modemID = SelectedModem.ModemID;
                    modemNumber = SelectedModem.ModemNumber.ToString(); 
                    string filter = "  and Main.Valid=1 and Main.MeterNumber!= '" + MeterNumber + "' and Main.ModemID=" + modemID.ToString();
                    ShowMeter meters = new ShowMeter(filter);
                    if (!meters.CollectShowMeters.IsEmpty)
                    {
                        
                        Isduplicate = true;
                        
                    }
                    modemID = SelectedModem.ModemID;
                    modemNumber = SelectedModem.ModemNumber.ToString();               
                    simNumber = SelectedModem.SimNumber.ToString();
                    cmbModemManufacturerName.Text = SelectedModem.ManufacturerName.ToString();
                    cmbModemSoftware.Text = SelectedModem.Softversion.ToString();
                    txtModemNumber.Text = SelectedModem.ModemNumber.ToString();
                    txtSimNumber.Text = SelectedModem.SimNumber.ToString();
                    //GridDown.DataContext = SelectedModem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void cmbModemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbModemType.SelectedItem != null)
                {
                    SelectedModemtype = (ShowDeviceModel_Result)cmbModemType.SelectedItem;
                    ModemDeviceModelID = Convert.ToDecimal(SelectedModemtype.DeviceModeleID);

                    string filter = " and DeviceType.DeviceTypeID=2 " + " and Main.DeviceModelID=" + SelectedModemtype.DeviceModeleID.ToString();
                    ShowSoftversionToDeviceModel MeterSoftversion = new ShowSoftversionToDeviceModel(filter);
                    cmbModemSoftware.ItemsSource = MeterSoftversion.CollectShowSoftversionToDeviceModel;
                    if (cmbModemSoftware.Items.Count > 0)
                        cmbModemSoftware.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void cmbModemSoftware_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectedVersion = (ShowSoftversionToDeviceModel_Result)cmbModemSoftware.SelectedItem;
                if (SelectedVersion != null)
                    ModemSoftversionToDeviceModelID = SelectedVersion.SoftversionToDeviceModelID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void gr_Loaded(object sender, RoutedEventArgs e)
        {
          
            (sender as Grid).DataContext = tr.TranslateofLable;
        }

        private void cmbModemManufacturerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                 
                if (cmbModemManufacturerName.SelectedItem != null)
                {
                    if (cmbModemManufacturerName.SelectedValue.ToString() != "")
                    {
                        ShowDeviceModel MeterModel = new ShowDeviceModel(" and DeviceType.DeviceTypeID=2 and Main.ManufacturerName='" + cmbModemManufacturerName.SelectedValue.ToString() + "'");
                        cmbModemType.ItemsSource = MeterModel.CollectShowDeviceModel;
                        cmbModemType.SelectedIndex = 0;
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void MainGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                IsNew = false;
                Isduplicate = false;
                SelectedModem = (ShowModem_Result)MainGrid.SelectedItem;
                if (SelectedModem != null)
                {
                    modemID = SelectedModem.ModemID;
                    modemNumber = SelectedModem.ModemNumber.ToString();
                    simNumber = SelectedModem.SimNumber.ToString();
                    GridDown.DataContext = SelectedModem;
                }
                if (SelectedModem!=null)
	            {
                    IsClose = false;
		            modemID=SelectedModem.ModemID;
                    string filter = "  and Main.Valid=1 and Main.MeterNumber!= '" + MeterNumber + "' and Main.ModemID="+modemID.ToString();
                    ShowMeter meters = new ShowMeter(filter);
                    if (!meters.CollectShowMeters.IsEmpty)
                    {
                        MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message6);
                        Isduplicate = true;
                        return;
                    }
                    modemNumber=SelectedModem.ModemNumber.ToString();
                    simNumber = SelectedModem.SimNumber.ToString();
                    this.Close();
	            }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (IsClose)
                {
                     modemID=null;
                     modemNumber="";
                     simNumber = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void txtSimNumber_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (IsNew)
                    GridDown.DataContext = null;
                ISEditing = true;
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
                    MessageBoxResult res = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message5, CommonData.mainwindow.tm.TranslateofMessage.Message11, MessageBoxButton.YesNo);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            us = CommonData.ShowButtonBinding("", windowID);
            if (us.CanInsert)
                ToolStripButtonNew_Click(null, null);
           
        }

        private void MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //IsNew = false;
                //Isduplicate = false;
                //SelectedModem = (ShowModem_Result)MainGrid.SelectedItem;
                //if (SelectedModem != null)
                //{
                //    modemID = SelectedModem.ModemID;
                //    modemNumber = SelectedModem.ModemNumber.ToString();
                //    simNumber = SelectedModem.SimNumber.ToString();
                //    GridDown.DataContext = SelectedModem;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void MainGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcount.Content =rowcount +"="+MainGrid.Items.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
    }
}
