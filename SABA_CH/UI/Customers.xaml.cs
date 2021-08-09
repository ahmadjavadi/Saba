using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
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
    /// Interaction logic for Customers.xaml
    /// </summary>
    [SuppressMessage("ReSharper", "RedundantAssignment")]
    public partial class Customers : System.Windows.Window
    {
        public readonly int windowID = 4;
        private TabControl tabCtrl;
        private TabItem tabPag;
        private int AddCustomer = 0;

        private bool Isfirsttime = true;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }

        public ObjectParameter Result = new ObjectParameter("Result", 1000000);

        public ObjectParameter CustomerID = new ObjectParameter("CustomerID",
            1000000);

        public ObjectParameter LocationID = new ObjectParameter("LocationID",
            1000000);

        public ObjectParameter officeID = new ObjectParameter("officeID",
            1000000);

        public ObjectParameter CityID = new ObjectParameter("CityID", 1000000);
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public ShowButtonAccess_Result us = null;

        public TabControl Tab
        {
            set { tabCtrl = value; }
        }

        public ShowTranslateofLable tr = null;
        public ShowCountries_Result SelectedCountry = null;
        public ShowProvinces_Result SelectedProvinces = null;
        public ShowPlains_Result SelectedPlain = null;
        public ShowAreas_Result SelectedArea = null;
        public ShowCatchments_Result SelectedCatchments = null;
        public ShowCustomers_Result SelectedCustomer = null;
        public bool ISEditing = false;
        public bool IsClose = true;
        public bool IsNew = false;

        public decimal? cityID = 1000000,
            PlainID = null,
            AreaID = null,
            CatchmentID = null,
            CountryID = 1000000,
            ProvinceID = 1000000,
            customerID,
            OfficeID = 1000000,
            EOfficeID = 1000000,
            SubOfficeID = 1000000,
            EsubOfficeID = 1000000;

        public string CustomerName = "", NationCode = "";

        public Customers()
        {

            InitializeComponent();
            tr = CommonData.translateWindow(windowID);
            CommonData.ChangeFlowDirection(MGrid);
            CommonData.ChangeFlowDirection(GridCustomer);

            gridexpanderCustomerl.DataContext = tr.TranslateofLable;
            gridexpanderwaterl.DataContext = tr.TranslateofLable;
            gridEsubscl.DataContext = tr.TranslateofLable;
            GridCustomer.DataContext = tr.TranslateofLable;
            RefreshDataGrid("");
            RefreshcmbCountry();
            RefreshcmbPlain("");
            RefreshcmbCatchment();
            TranslateDataGrid();
            RefreshcmbOffice();
            RefreshcmbEOffice();
            ChangeFlowdirection();
            refreshcmbWelldepth();
            refreshcmbTypeOfUse();
            Focus();
            us = CommonData.ShowButtonBinding("", windowID);
            if (us.CanInsert)
                ToolStripButtonNew_Click(null, null);
            
        }

        public void refreshcmbTypeOfUse()
        {
            try
            {
                List<string> lstTypeOfUse = new List<string>();
                lstTypeOfUse.Add(tr.TranslateofLable.Object34);
                lstTypeOfUse.Add(tr.TranslateofLable.Object33);
                lstTypeOfUse.Add(tr.TranslateofLable.Object35);
                lstTypeOfUse.Add(tr.TranslateofLable.Object36);
                cmbTypeOfUse.ItemsSource = lstTypeOfUse;
                cmbTypeOfUse.SelectedIndex = 0;
               
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void refreshcmbWelldepth()
        {
            try
            {
                if (CommonData.LanguagesID == 2)
                {
                    cmbWelldepth.Items.Add("عمیق");
                    cmbWelldepth.Items.Add("نیمه عمیق");
                }
                else
                {
                    cmbWelldepth.Items.Add("deep");
                    cmbWelldepth.Items.Add("Semi-deep");


                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void ChangeFlowdirection()
        {
            try
            {
                //GridCustomer.FlowDirection = CommonData.FlowDirection;
                //MainScrollviewer.FlowDirection = CommonData.FlowDirection;
                //GridMain.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void TranslateDataGrid()
        {
            try
            {
                //GridMain.Columns[0].Header = tr.TranslateofLable.Object1;
                //GridMain.Columns[1].Header = tr.TranslateofLable.Object2;
                //GridMain.Columns[2].Header = tr.TranslateofLable.Object19;
                //GridMain.Columns[3].Header = tr.TranslateofLable.Object6;
                //GridMain.Columns[4].Header = tr.TranslateofLable.Object3;
                //GridMain.Columns[5].Header = tr.TranslateofLable.Object4;
                //GridMain.Columns[6].Header = tr.TranslateofLable.Object5;
                //GridMain.Columns[7].Header = tr.TranslateofLable.Object8;
                //GridMain.Columns[8].Header = tr.TranslateofLable.Object9;
                //GridMain.Columns[9].Header = tr.TranslateofLable.Object10;
                //GridMain.Columns[10].Header = tr.TranslateofLable.Object11;
                //GridMain.Columns[11].Header = tr.TranslateofLable.Object12;
                //GridMain.Columns[12].Header = tr.TranslateofLable.Object13;
                ////GridMain.Columns[13].Header = tr.TranslateofLable.Object21;
                ////GridMain.Columns[14].Header = tr.TranslateofLable.Object22;
                ////GridMain.Columns[14].Header = tr.TranslateofLable.Object24;
                //GridMain.Columns[13].Header = tr.TranslateofLable.Object23;
                //GridMain.Columns[14].Header = tr.TranslateofLable.Object20;
                expwaterinfo.Header = tr.TranslateofLable.Object31;
                expEinfo.Header = tr.TranslateofLable.Object30;
                expGeneralinfo.Header = tr.TranslateofLable.Object32;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void RefreshDataGrid(string Filter)
        {
            try
            {

                var a = DataBaseManager.Instance.GetAllCustomer();
                IsNew = false;
                //GridMain.ItemsSource = null;

                ShowCustomers Customers = new ShowCustomers(Filter);
                GridMain.ItemsSource = Customers._lstShowCustomers;
                if (GridMain.Items.Count > 0)
                    GridMain.SelectedIndex = GridMain.Items.Count - 1;
                //GridCustomer.DataContext = Customers;    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void RefreshcmbCountry()
        {
            try
            {
                ShowCountries Countries = new ShowCountries("");
                cmbCountry.ItemsSource = Countries.CollectShowCountries;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void RefreshcmbProvince()
        {
            try
            {
                string filter = "";
                if (SelectedCountry != null)
                    filter = " and CountryID=" + SelectedCountry.CountryID.ToString();
                ShowProvinces Province = new ShowProvinces(filter);
                cmbProvince.ItemsSource = Province.CollectShowProvinces;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void RefreshcmbPlain(string filter)
        {
            try
            {              
                ShowPlains plains = new ShowPlains(filter);
                cmbPlain.ItemsSource = plains.CollectShowPlains;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void RefreshcmbArea()
        {
            try
            {
                string Filter = "";
                ShowAreas plains = new ShowAreas(Filter);
                //cmbAreas.ItemsSource = Plains.CollectShowAreas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void RefreshcmbCatchment()
        {
            try
            {

                ShowCatchments catchments = new ShowCatchments(" and usertogroup.userid= " + CommonData.UserID);
                cmbCatchment.ItemsSource = catchments._lstShowCatchments;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void cmbLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCountry.SelectedItem != null)
                {
                    SelectedCountry = (ShowCountries_Result)cmbCountry.SelectedItem;
                    RefreshcmbProvince();
                    if (SelectedCountry != null)
                        CountryID = SelectedCountry.CountryID;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", windowID);
                toolBar1.DataContext = us;
                if (us != null)
                    toolBar1.DataContext = us;
                tabCtrl.SelectedItem = tabPag;
                if (!tabCtrl.IsVisible)
                {

                    tabCtrl.Visibility = Visibility.Visible;

                }
                Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                if (!IsEditing())
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
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }

        }

        public bool IsEditing()
        {
            try
            {
                if (ISEditing)
                {
                    MessageBoxResult res = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message5,
                        CommonData.mainwindow.tm.TranslateofMessage.Message11, MessageBoxButton.YesNo);
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

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
                return false;
            }
        }

        public void RefreshcmbOffice()
        {
            try
            {
                string filter = "";
                string p = CommonData.ProvinceCode;
                int value = Int32.Parse(p, NumberStyles.HexNumber);
                value = value / 1000;
                if (value == 8)
                    value = 1;
                else if (value == 1)
                    value = 8;

                if (value != 0)
                    filter = " and officeid=" + value ;

                
                 ShowOffice showoffice = new ShowOffice(filter);
                cmbOffice.ItemsSource = null;
                cmbOffice.ItemsSource = showoffice._lstShowOffice;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void RefreshcmbWSubOffice(string filter)
        {
            try
            {
                ShowSubOffice showoffice = new ShowSubOffice(filter);
                cmbWSubOffice.ItemsSource = null;
                cmbWSubOffice.ItemsSource = showoffice._lstShowSubOffices;

                if (showoffice._lstShowSubOffices.Count > 0)
                    cmbWSubOffice.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void RefreshcmbEOffice()
        {
            try
            {
                string filter = "";
                string p = CommonData.ProvinceCode;
                int value = Int32.Parse(p, NumberStyles.HexNumber);
                value = value / 1000;
                if (value == 8)
                    value = 1;
                else if (value == 1)
                    value = 8;

                if (value != 0)
                    filter = " and EOfficeID=" + value;

                ShowEOffice showoffice = new ShowEOffice(filter);
                cmbEOffice.ItemsSource = null;
                cmbEOffice.ItemsSource = showoffice._lstShowEOffices;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void RefreshcmbSubOfficeE(string filter)
        {
            try
            {
                ShowESubOffice showoffice = new ShowESubOffice(filter);
                cmbSubOfficeE.ItemsSource = null;
                cmbSubOfficeE.ItemsSource = showoffice._lstShowESubOffices;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Grid grid = sender as Grid;
                if (grid != null) grid.DataContext = tr.TranslateofLable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsEditing())
                    return;
                IsNew = true;
                Isfirsttime = true;
                CommonData.New(this.gridexpanderCustomer);
                CommonData.New(this.gridexpanderwater);
                CommonData.New(this.gridEsubsc);
                PlainID = null;
                CatchmentID = null;
                AreaID = null;
                ProvinceID = null;
                CityID = null;
                if (CommonData.LanguagesID == 2)
                {
                    cmbCountry.SelectedIndex = 79;
                }
                else
                    cmbCountry.SelectedIndex = 0;

                cmbCatchment.SelectedIndex = -1;

                cmbPlain.SelectedIndex = -1;

                if (cmbOffice.Items.Count == 0)
                    cmbOffice.SelectedIndex = -1;
                else
                    cmbOffice.SelectedIndex = 0;

                if (cmbWSubOffice.Items.Count == 0)
                    cmbWSubOffice.SelectedIndex = -1;
                else
                    cmbWSubOffice.SelectedIndex = 0;

                cmbSubOfficeE.SelectedIndex = -1;
                cmbProvince.SelectedIndex = -1;
                cmbEOffice.SelectedIndex = -1;
                cmbTypeOfUse.SelectedIndex = 0;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // If name & FamilyName & Document Number is null don't let Save
                if (txtWellAddress.Text.Trim() == "" || txtCustomerName.Text.Trim() == "" || txtCustomerfamily.Text.Trim() == "" || txtdossier.Text.Trim() == "" || cmbProvince.SelectedIndex==-1 ||
                     txtCity.Text.Trim()=="" || txtFlowindossier.Text.Trim()=="" || txtWsubscNumber.Text.Trim()=="" || txtEsubscNumber.Text.Trim()=="" || cmbPlain.SelectedIndex==-1)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message26);
                    return;
                }

                //
                ISEditing = false;
                Result = new ObjectParameter("Result", 1000000);
                CustomerID = new ObjectParameter("CustomerID", 1000000);
                LocationID = new ObjectParameter("LocationID", 1000000);
                CityID = new ObjectParameter("CityID", 1000000);
                officeID = new ObjectParameter("officeID", 1000000);
                ErrMsg = new ObjectParameter("ErrMsg", "");
                if (PlainID == 0)
                    PlainID = null;
                if (CatchmentID == 0)
                    CatchmentID = null;
                if (AreaID == 0)
                    AreaID = null;
                if (ProvinceID == 0)
                    ProvinceID = null;
                if (IsNew)
                {

                    SQLSPS.InsCities("", txtCity.Text, CountryID, ProvinceID, CityID, Result, ErrMsg);
                    if (ErrMsg.Value.ToString() == "")
                        SQLSPS.InsLocations(PlainID, CatchmentID, AreaID, Convert.ToDecimal(CityID.Value), LocationID,
                            Result, ErrMsg);
                    else
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                        return;
                    }
                    if (ErrMsg.Value.ToString() == "")
                    {
                        if (txtNationalCode.Text == "")
                            txtNationalCode.Text = "0";
                        if (txtFlowindossier.Text == "")
                            txtFlowindossier.Text = "0";
                        //if (txtDiameterofpipe.Text=="")
                        //    txtDiameterofpipe.Text = "0";
                        if (cmbWelldepth.Text == "")
                            cmbWelldepth.SelectedIndex = 0;
                        //OfficeID = 1000000, EOfficeID = 1000000, SubOfficeID = 1000000, EsubOfficeID = 1000000
                        if (EOfficeID == 1000000 || EOfficeID == 0)
                            EOfficeID = null;
                        if (OfficeID == 1000000 || OfficeID == 0)
                            OfficeID = null;
                        if (SubOfficeID == 1000000 || SubOfficeID == 0)
                            SubOfficeID = null;
                        if (EsubOfficeID == 1000000 || EsubOfficeID == 0)
                            EsubOfficeID = null;
                        SQLSPS.InsCustomers(txtCustomerName.Text, txtCustomerfamily.Text, txtTel.Text, txtAddress.Text,
                            txtWsubscNumber.Text, txtEsubscNumber.Text, Convert.ToDecimal(LocationID.Value),
                            Convert.ToDecimal(txtNationalCode.Text), txtMobile.Text, txtLongitude.Text, txtLatitude.Text,
                            txtPostCode.Text, OfficeID, EOfficeID, SubOfficeID, EsubOfficeID, txtdossier.Text,
                            txtFlowindossier.Text, "0", cmbWelldepth.SelectedIndex.ToString(), "", txtWellAddress.Text, "",
                            cmbTypeOfUse.SelectedIndex, CustomerID, Result, ErrMsg);
                        if (ErrMsg.Value.ToString() != "")
                            MessageBox.Show(ErrMsg.Value.ToString());
                        else
                            AddCustomer++;
                    }
                    else
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                        return;
                    }

                }
                else
                {

                    SQLSPS.InsCities("", txtCity.Text, CountryID, ProvinceID, CityID, Result, ErrMsg);
                    if (ErrMsg.Value.ToString() == "")
                    {
                        if (PlainID == 0)
                            PlainID = null;
                        if (CatchmentID == 0)
                            CatchmentID = null;
                        if (AreaID == 0)
                            AreaID = null;
                        SQLSPS.InsLocations(PlainID, CatchmentID, AreaID, Convert.ToDecimal(CityID.Value), LocationID,
                            Result, ErrMsg);
                    }
                    else
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                        return;
                    }
                    if (ErrMsg.Value.ToString() == "")
                    {
                        if (txtNationalCode.Text == "")
                            txtNationalCode.Text = "0";
                        if (txtFlowindossier.Text == "")
                            txtFlowindossier.Text = "0";
                        //if (txtDiameterofpipe.Text == "")
                        //    txtDiameterofpipe.Text = "0";
                        if (cmbWelldepth.Text == "")
                            cmbWelldepth.SelectedIndex = 0;
                        if (EOfficeID == 1000000 || EOfficeID == 0)
                            EOfficeID = null;
                        if (OfficeID == 1000000 || OfficeID == 0)
                            OfficeID = null;
                        if (SubOfficeID == 1000000 || SubOfficeID == 0)
                            SubOfficeID = null;
                        if (EsubOfficeID == 1000000 || EsubOfficeID == 0)
                            EsubOfficeID = null;
                        SQLSPS.UpdCustomers(txtCustomerName.Text, txtCustomerfamily.Text, txtTel.Text, txtAddress.Text,
                            txtWsubscNumber.Text, txtEsubscNumber.Text, Convert.ToDecimal(LocationID.Value),
                            Convert.ToDecimal(txtNationalCode.Text), txtMobile.Text, txtLongitude.Text, txtLatitude.Text,
                            txtPostCode.Text, OfficeID, EOfficeID, SubOfficeID, EsubOfficeID, txtdossier.Text,
                            txtFlowindossier.Text, 0, cmbWelldepth.SelectedIndex, "", txtWellAddress.Text, "",
                            cmbTypeOfUse.SelectedIndex, customerID, Result, ErrMsg);
                        if (ErrMsg.Value.ToString() != "")
                            MessageBox.Show(ErrMsg.Value.ToString());
                    }
                    else
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                        return;
                    }

                }
                RefreshDataGrid("");
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message7);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("Error");
            }
        }

        private void ToolStripButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (customerID == null)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message2);
                    return;
                }
                ISEditing = false;
                MessageBoxResult res = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message1,
                    CommonData.mainwindow.tm.TranslateofMessage.Message11, MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    ShowMeter Meters = new ShowMeter(" and Main.CustomerID=" + customerID.ToString());
                    if (Meters._lstShowMeters.Count > 0)
                    {
                        MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message75);
                        return;
                    }
                    Result = new ObjectParameter("Result", 1000000);
                    ErrMsg = new ObjectParameter("ErrMsg", "");
                    SQLSPS.DelCustomer(customerID, Result, ErrMsg);
                    if (ErrMsg.Value.ToString() != "")
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                    }
                    RefreshDataGrid("");
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message7);
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("Error");
            }
        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshDataGrid("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
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
                    ReadCustomerFromExcel(filename);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        public void ReadCustomerFromExcel(string path)
        {
            int i=0;
            string filename = "";
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "Import Customer From Excel",
                Filter = ".txt|*.txt"
            };
            if (sfd.ShowDialog() == true)
            {
                filename = sfd.FileName;
            }
            AddCustomer = 0;
            string errorData = "";
            ShowCustomers customers = null;
            Application App = new Application();
            Workbook workbook = (Workbook)App.Workbooks.Add(Missing.Value);
            Worksheet worksheet;
            try
            {
                workbook = App.Workbooks.Open(path, 0, false, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                // Get first Worksheet

                worksheet = (Worksheet)workbook.Sheets.get_Item(1);
                // Setting cell values
                Range range = worksheet.UsedRange;
                int c = range.Rows.Count;

                ShowCustomers_Result newCustomer = new ShowCustomers_Result();
                ShowCities_Result newCity = new ShowCities_Result();
                ShowCountries_Result newCountry = new ShowCountries_Result();

                ShowCatchments_Result newCatchment = new ShowCatchments_Result();
                ShowPlains_Result newPlain = new ShowPlains_Result();
                ShowProvinces_Result newProvince = new ShowProvinces_Result();
                ShowOffice_Result newOffice = new ShowOffice_Result();
                ShowEOffice_Result newEOffice = new ShowEOffice_Result();
                ShowSubOffice_Result newsubOffice = new ShowSubOffice_Result();
                ShowESubOffice_Result newEsubOffice = new ShowESubOffice_Result();
             

                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);
                CommonData.mainwindow.changeProgressBar_MaximumValue(c);

      
           #region rowExcel
                for ( i = 2; i <= c; i++)
                {
                    Result = new ObjectParameter("Result", 1000000);
                    ErrMsg = new ObjectParameter("ErrMsg", "");


                    ObjectParameter CityID = new ObjectParameter("CityID", 1000000);
                    ObjectParameter LocationID = new ObjectParameter("LocationID", 1000000);
                    int typeOfUse = 0;
                    newCustomer = new ShowCustomers_Result();
                    newCity = new ShowCities_Result();
                    newCountry = new ShowCountries_Result();

                    newCatchment = new ShowCatchments_Result();
                    newPlain = new ShowPlains_Result();
                    newProvince = new ShowProvinces_Result();
                    newOffice = new ShowOffice_Result();
                    newEOffice = new ShowEOffice_Result();
                    newsubOffice = new ShowSubOffice_Result();
                    newEsubOffice = new ShowESubOffice_Result();


                    string meterNo;
                    if (CheckMeterNo(worksheet, i, filename, out meterNo)) continue;
                    string message = "";
                    if (CheckCustomerNam(newCustomer, worksheet, i, message, filename, ref customers, ref meterNo, ref errorData)) continue;

                    if (CheckCustomerFamily(newCustomer, worksheet, i, meterNo, message, filename)) continue;


                    newCustomer.FatherName =  Convert.ToString((((Range) worksheet.Cells[i, "D"]).Value));
                    newCustomer.NationalCode = Convert.ToDecimal((((Range) worksheet.Cells[i, "E"]).Value));
                    newCustomer.MobileNumber = Convert.ToString((((Range) worksheet.Cells[i, "F"]).Value));
                    if (newCustomer.MobileNumber == null) newCustomer.MobileNumber = "";
                    newCustomer.CustomerTel =  Convert.ToString((((Range) worksheet.Cells[i, "G"]).Value));
                    newCustomer.CustomerAddress =  Convert.ToString((((Range) worksheet.Cells[i, "H"]).Value));
                    if (newCustomer.CustomerAddress == null) newCustomer.CustomerAddress = "";
                    newCustomer.PostCode =   Convert.ToString((((Range) worksheet.Cells[i, "I"]).Value));
                    newCustomer.WatersubscriptionNumber =  Convert.ToString((((Range) worksheet.Cells[i, "J"]).Value));
                    newCustomer.ElecsubscriptionNumber =  Convert.ToString((((Range) worksheet.Cells[i, "K"]).Value));

                    if (CheckSubscriberNumber(newCustomer, meterNo, message, i, filename)) continue;

                    if (CheckLocationInfo(newCountry, worksheet, i, newProvince, newCity, meterNo, message, filename)) continue;

                    if (CheckPlaneInfo(newPlain, worksheet, i, meterNo, message, filename)) continue;

                    if (CheckDossierInfo(newCatchment, worksheet, i, newCustomer, newOffice, newEOffice, newsubOffice, newEsubOffice, meterNo, message, filename)) continue;


                    ShowCountries countries = new ShowCountries("");

                    foreach (var item in countries._lstShowCountries)
                    {
                        if(newCountry.CountryName != null)
                        if (newCountry.CountryName.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی").Equals(item.CountryName.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی")) ||
                            newCountry.CountryName== "ایران" && item.CountryName.ToUpper()== "IRAN")
                        {
                            newCountry.CountryID = item.CountryID;
                            break;
                        }
                    }
                    ShowProvinces province = new ShowProvinces("");
                    foreach (var item in province._lstShowProvinces)
                    {
                        if(newProvince.ProvinceName!=null)
                        if (newProvince.ProvinceName.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی").Equals(item.ProvinceName.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی")))
                        {
                            newProvince.ProvinceID = item.ProvinceID;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(newPlain.PlainName))
                    {
                        newPlain.PlainName = newPlain.PlainName.TrimEnd();
                        newPlain.PlainName = newPlain.PlainName.TrimStart();
                    }
                    if (string.IsNullOrEmpty(newPlain.PlainName))
                    {
                        message += " لطفا دشت مشترک سطر";
                        message += i.ToString() + " ";
                        message += "را وارد نمایید" + "\r\n";
                        SaveErrorLog(filename,  message);
                        continue;
                    }
                    ShowPlains plain = new ShowPlains("");

                    foreach (var item in plain._lstShowPlains)
                    {
                        if (newPlain.PlainName != null)
                        {
                            if (newPlain.PlainName.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی").Equals(item.PlainName.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی")))
                            {
                                newPlain.PlainID = item.PlainID;
                                break;
                            }
                        }
                     
                    }
                   

                        ShowCatchments catchment = new ShowCatchments("");
                    foreach (var item in catchment._lstShowCatchments)
                    {
                        if(newCatchment.CatchmentName!=null)
                         if (item.CatchmentName.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی").Equals(newCatchment.CatchmentName.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی")))
                         {
                            newCatchment.CatchmentID = item.CatchmentID;
                            break;
                         }
                    }
                    if (!string.IsNullOrEmpty(newOffice.OfficeDesc))
                    {
                        newOffice.OfficeDesc = newOffice.OfficeDesc.TrimEnd();
                        newOffice.OfficeDesc = newOffice.OfficeDesc.TrimStart();
                    }
                    if (string.IsNullOrEmpty(newOffice.OfficeDesc))
                    {
                        message += " لطفا اداره آب منطقه ای مشترک سطر";
                        message += i.ToString() + " ";
                        message += "را وارد نمایید" + "\r\n";
                        SaveErrorLog(filename,  message);
                        continue;
                    }
                    // Select correct office
                    string p = CommonData.ProvinceCode;
                    string filter = "";
                    try
                    {
                        int value = Int32.Parse(p, NumberStyles.HexNumber);
                        value = value / 1000;
                        if (value == 8)
                            value = 1;
                        else if (value == 1)
                            value = 8;

                        if (value != 0)
                            filter = "and OfficeID in (0," + value + ")";
                    }
                    catch
                    {
                        filter = "";
                    }

                    //

                    ShowOffice office = new ShowOffice(filter);

                    foreach (var item in office._lstShowOffice)
                    {
                        if (newOffice.OfficeDesc != null)
                        {
                            if (newOffice.OfficeDesc.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی").Equals(item.OfficeDesc.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی")))
                            {
                                newOffice.OfficeID = item.OfficeID;
                                break;
                            }
                        }                       
                    }
                    if (newOffice.OfficeID == 0)
                    {
                        message += " لطفااطلاعات اداره آب منطقه ای مشترک سطر";
                        message += i.ToString() + " ";
                        message += "رادرست وارد نمایید" + "\r\n";
                        SaveErrorLog(filename,  message);
                        continue;
                    }

                    ShowEOffice eOffice = new ShowEOffice("");
                    foreach (var item in eOffice._lstShowEOffices)
                    {
                       if(newEOffice.OfficeDesc!=null)
                        if (newEOffice.OfficeDesc.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی").Equals(item.OfficeDesc.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی")))
                        {
                            newEOffice.OfficeID = item.OfficeID;
                            break;
                        }
                    }

                    ShowESubOffice esubOffice = new ShowESubOffice("");
                    foreach (var item in esubOffice._lstShowESubOffices)
                    {
                        if(newEsubOffice.ESubOfficeDesc!=null)
                         if (newEsubOffice.ESubOfficeDesc.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی").Equals(item.ESubOfficeDesc.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی")))
                         {
                            newEsubOffice.ESubOfficeID = item.ESubOfficeID;
                            break;
                         }
                    }


                    ShowSubOffice subOffice = new ShowSubOffice("");
                    foreach (var item in subOffice._lstShowSubOffices)
                    {
                        if (newsubOffice.SubOfficeDesc != null)
                        {
                            if (newsubOffice.SubOfficeDesc.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی").Equals(item.SubOfficeDesc.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی")))
                            {
                                newsubOffice.SubOfficeID = item.SubOfficeID;
                                break;
                            }
                        }
                        
                    }

                    if (!string.IsNullOrEmpty(newsubOffice.SubOfficeDesc))
                    {
                        newsubOffice.SubOfficeDesc = newsubOffice.SubOfficeDesc.TrimEnd();
                        newsubOffice.SubOfficeDesc = newsubOffice.SubOfficeDesc.TrimStart();
                    }
                    if (string.IsNullOrEmpty(newsubOffice.SubOfficeDesc))
                    {
                        message += " لطفا امور آب مشترک سطر";
                        message += i.ToString() + " ";
                        message += "را وارد نمایید" + "\r\n";
                        SaveErrorLog(filename,  message);
                        continue;
                    }

                    ShowCities cities = new ShowCities("");
                    foreach (var item in cities._lstShowCities)
                    {
                        if (newCity.CityName != null)
                        {
                            if (newCity.CityName.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی").Equals(item.CityName.Replace(" ", "").Replace("ك", "ک").Replace("ي", "ی")))
                            {
                                newCity.CityID = item.CityID;
                                break;
                            }
                        }
                        
                    }

                    if (!string.IsNullOrEmpty(newCity.CityName))
                    {
                        newCity.CityName = newCity.CityName.TrimEnd();
                        newCity.CityName = newCity.CityName.TrimStart();
                    }
                    if (string .IsNullOrEmpty(newCity.CityName))
                    {
                        message += " لطفا شهر مشترک سطر";
                        message += i.ToString() + " ";
                        message += "را وارد نمایید" + "\r\n";
                        SaveErrorLog(filename,  message);
                        continue;
                    }

                    ShowLocations locations = new ShowLocations("");
                    this.LocationID.Value = "1000000";
                    foreach (var item in locations._lstShowLocations)
                    {
                        if (newCatchment.CatchmentName != null)
                        {
                            try
                            {
                                if (item.CityName.Equals(newCity.CityName) && item.CatchmentName.Trim().Equals(newCatchment.CatchmentName.Trim()) &&
                              item.ProvinceName.Trim().Equals(newProvince.ProvinceName.Trim()) && item.PlainName.Trim().Equals(newPlain.PlainName.Trim()))
                            {
                                this.LocationID.Value = item.LocationID;
                                break;
                            }
                            else                            
                              this.LocationID.Value = "1000000";

                            }
                            catch (Exception ex)
                            {

                               MessageBox.Show("");
                            }
                        }
                     
                    }

                    if (newEOffice.OfficeID == 1000000 || newEOffice.OfficeID == 0)
                        EOfficeID = null;
                    else
                        EOfficeID = newEOffice.OfficeID;

                    if (newOffice.OfficeID == 1000000 || newOffice.OfficeID == 0)
                        OfficeID = null;
                    else
                        OfficeID = newOffice.OfficeID;

                    if (newsubOffice.SubOfficeID == 1000000 || newsubOffice.SubOfficeID == 0)
                        SubOfficeID = null;
                    else
                        SubOfficeID = newsubOffice.SubOfficeID;

                    if (newEsubOffice.ESubOfficeID == 1000000 || newEsubOffice.ESubOfficeID == 0)
                        EsubOfficeID = null;
                    else
                        EsubOfficeID = newEsubOffice.ESubOfficeID;

                    if (newProvince.ProvinceID == 1000000 || newProvince.ProvinceID == 0)
                        ProvinceID = null;
                    else
                        ProvinceID = newProvince.ProvinceID;

                    if (newCatchment.CatchmentID == 1000000 || newCatchment.CatchmentID == 0)
                        CatchmentID = null;
                    else
                        CatchmentID = newCatchment.CatchmentID;

                    if (newPlain.PlainID == 1000000 || newPlain.PlainID == 0)
                        PlainID = null;
                    else
                        PlainID = newPlain.PlainID;

                    //if (newCity.CityID == 1000000 || newCity.CityID == 0)
                    //{
                        SQLSPS.InsCities("", newCity.CityName, CountryID, ProvinceID, CityID, Result, ErrMsg);
                        cityID =NumericConverter.DecimalConverter(CityID.Value.ToString());
                    //}
                    //else
                        //cityID = newCity.CityID;

                    ShowMeter_Result showMeterResult = SQLSPS.ShowMeter("and Main.MeterNumber='" + meterNo + "'");
                    
                    if (this.LocationID.Value.ToString() == "0" || this.LocationID.Value.ToString() == "1000000")
                        SQLSPS.InsLocations(PlainID, CatchmentID, null, this.cityID, this.LocationID, Result, ErrMsg);

                    decimal customerId = HasCustomer(newCustomer, customers);

                    if (customerId > -1)
                    {
                        ShowMeterToCustomer_Result smtr =  SQLSPS.ShowMeterToCustomer(" and Main.CustomerID="+ customerId);
                        if (smtr != null)
                        {
                            if (smtr.MeterNumber != meterNo)
                            {
                                message = " به مشترک";
                                message +=" " + newCustomer.CustomerName + " " + newCustomer.Customerfamily;
                                message += "  در سطر";
                                message += i.ToString() + " ";
                                message += " کنتور شماره ";
                                message += smtr.MeterNumber + " ";
                                message += " اختصاص داده شده است " + "\r\n";
                                SaveErrorLog(filename,  message);
                                continue;
                            }
                            else if (smtr.MeterNumber == meterNo)
                            {
                                message = "  مشترک";
                                message += " " + newCustomer.CustomerName + " " + newCustomer.Customerfamily ;
                                message += " با کنتور شماره  ";
                                message += smtr.MeterNumber + " ";
                                message +=  "  در سطر";
                                message += i.ToString() + " ";
                                message += " تکراری می باشد " + "\r\n";
                                SaveErrorLog(filename,  message);
                                continue;
                            }
                        }
                        else
                        {
                            if (showMeterResult.MeterNumber.Equals(meterNo))
                            {
                                if (showMeterResult.CustomerID < 1)
                                {
                                    if (meterNo.StartsWith("207"))
                                        showMeterResult.SoftversionToDeviceModelID = 2;
                                    SQLSPS.UpdMeter(meterNo, showMeterResult.DeviceModelID, showMeterResult.IsDirect,
                                        showMeterResult.SoftversionToDeviceModelID, showMeterResult.ModemID, customerId,
                                        showMeterResult.Valid, showMeterResult.MeterID, ErrMsg, Result);
                                }
                                else
                                {
                                    message = "  در سطر";
                                    message += i.ToString() + " ";
                                    message = " کنتور شماره ";
                                    message += meterNo + " ";
                                    message += " به مشترک ";
                                    message += newCustomer.CustomerName + " " + newCustomer.Customerfamily + " " + newCustomer.WatersubscriptionNumber;
                                    message += " اختصاص داده شده است " + "\r\n";
                                    SaveErrorLog(filename,  message);
                                    continue;
                                }
                            }
                        
                            else
                            {
                                SaveMeter(meterNo, customerId.ToString());
                            }
                        }
                        
                    }
                    else
                    {
                        if (showMeterResult.MeterNumber != meterNo)
                        {
                            ShowMeterToCustomer_Result smtr =
                                SQLSPS.ShowMeterToCustomer(" and Main.MeterNumber='" + meterNo + "'");
                            CustomerID.Value = 1000000;
                            SQLSPS.InsCustomers(newCustomer.CustomerName, newCustomer.Customerfamily,
                                newCustomer.CustomerTel,
                                newCustomer.CustomerAddress, newCustomer.WatersubscriptionNumber,
                                newCustomer.ElecsubscriptionNumber, Convert.ToDecimal(this.LocationID.Value),
                                newCustomer.NationalCode,
                                newCustomer.MobileNumber, newCustomer.Longitude, newCustomer.Latitude,
                                newCustomer.PostCode,
                                OfficeID, EOfficeID, SubOfficeID, EsubOfficeID, newCustomer.DossierNumber,
                                newCustomer.Flowindossier, newCustomer.Diameterofpipe, newCustomer.Welldepth,
                                newCustomer.WellLicense, newCustomer.WellAddress, newCustomer.FatherName,
                                newCustomer.TypeOfUse,
                                CustomerID, Result,
                                ErrMsg);
                            
                            SaveMeter(meterNo, CustomerID.Value.ToString());
                            AddCustomer++;
                        }
                        else
                        {
                            if (showMeterResult.CustomerID < 1)
                            {
                                SQLSPS.InsCustomers(newCustomer.CustomerName, newCustomer.Customerfamily,
                             newCustomer.CustomerTel,
                             newCustomer.CustomerAddress, newCustomer.WatersubscriptionNumber,
                             newCustomer.ElecsubscriptionNumber, Convert.ToDecimal(this.LocationID.Value),
                             newCustomer.NationalCode,
                             newCustomer.MobileNumber, newCustomer.Longitude, newCustomer.Latitude,
                             newCustomer.PostCode,
                             OfficeID, EOfficeID, SubOfficeID, EsubOfficeID, newCustomer.DossierNumber,
                             newCustomer.Flowindossier, newCustomer.Diameterofpipe, newCustomer.Welldepth,
                             newCustomer.WellLicense, newCustomer.WellAddress, newCustomer.FatherName,
                             newCustomer.TypeOfUse,
                             CustomerID, Result,
                             ErrMsg);
                                SQLSPS.UpdMeter(meterNo, showMeterResult.DeviceModelID, showMeterResult.IsDirect,
                                    showMeterResult.SoftversionToDeviceModelID, showMeterResult.ModemID, Convert.ToDecimal(CustomerID.Value.ToString()),
                                    showMeterResult.Valid, showMeterResult.MeterID, ErrMsg, Result);
                                AddCustomer++;
                            }
                            else
                            {
                                message = "  در سطر";
                                message += i + " ";
                                message = " کنتور شماره ";
                                message += meterNo + " ";
                                message += " به مشترک ";
                                message += newCustomer.CustomerName + " " + newCustomer.WatersubscriptionNumber;
                                message += " اختصاص داده شده است " + "\r\n";
                                SaveErrorLog(filename,  message);
                                continue;
                            }

                        }
                    }
                    
                    CommonData.mainwindow.changeProgressBarValue(1);
                }
#endregion rowExcel
                workbook.Close(Missing.Value, Missing.Value, Missing.Value);
                RefreshDataGrid("");
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
               
                MessageBox.Show( AddCustomer+"مشترک، به لیست مشترکین اضافه شد ");
                CommonData.mainwindow.RefreshSelectedMeters("");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(i.ToString());
                workbook.Close(Missing.Value, Missing.Value, Missing.Value);
                MessageBox.Show("اشکال در بارگیری اطلاعات مشترک. لطفا اطلاعات مشترک را کامل نمایید سپس دوباره اقدام نمایید"+"\r\n");
                //MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
            }
        }

        private bool CheckDossierInfo(ShowCatchments_Result newCatchment, Worksheet worksheet, int i,
            ShowCustomers_Result newCustomer, ShowOffice_Result newOffice, ShowEOffice_Result newEOffice,
            ShowSubOffice_Result newsubOffice, ShowESubOffice_Result newEsubOffice, string meterNo, string message,
            string filename)
        {
            newCatchment.CatchmentName = Convert.ToString((((Range) worksheet.Cells[i, "P"]).Value));

            newCustomer.Longitude = Convert.ToString((((Range) worksheet.Cells[i, "Q"]).Value));
            newCustomer.Latitude = Convert.ToString((((Range) worksheet.Cells[i, "R"]).Value));

            newOffice.OfficeDesc = Convert.ToString((((Range) worksheet.Cells[i, "S"]).Value));
            newEOffice.OfficeDesc = Convert.ToString((((Range) worksheet.Cells[i, "T"]).Value));
            newsubOffice.SubOfficeDesc = Convert.ToString((((Range) worksheet.Cells[i, "U"]).Value));
            newEsubOffice.ESubOfficeDesc = Convert.ToString((((Range) worksheet.Cells[i, "V"]).Value));


            newCustomer.DossierNumber = Convert.ToString((((Range) worksheet.Cells[i, "W"]).Value));
            newCustomer.Flowindossier = Convert.ToString((((Range) worksheet.Cells[i, "X"]).Value));
            if (!string.IsNullOrEmpty(newCustomer.DossierNumber))
            {
                newCustomer.DossierNumber = newCustomer.DossierNumber.TrimEnd();
                newCustomer.DossierNumber = newCustomer.DossierNumber.TrimStart();
            }
            if (!string.IsNullOrEmpty(newCustomer.Flowindossier))
            {
                newCustomer.Flowindossier = newCustomer.Flowindossier.TrimEnd();
                newCustomer.Flowindossier = newCustomer.Flowindossier.TrimStart();
            }
            if ((string.IsNullOrEmpty(newCustomer.DossierNumber) || string.IsNullOrEmpty(newCustomer.Flowindossier)) &&
                !(string.IsNullOrEmpty(meterNo)))
            {
                message += " لطفا دبی پروانه و شماره پرونده مشترک سطر";
                message += i.ToString() + " ";
                message += "را وارد نمایید" + "\r\n";
                SaveErrorLog(filename, message);
                return true;
            }
            newCustomer.Diameterofpipe = Convert.ToString((((Range) worksheet.Cells[i, "Y"]).Value));
            newCustomer.Welldepth = Convert.ToString((((Range) worksheet.Cells[i, "Z"]).Value));
            newCustomer.WellLicense = Convert.ToString((((Range) worksheet.Cells[i, "AA"]).Value));
            newCustomer.WellAddress = Convert.ToString((((Range) worksheet.Cells[i, "AB"]).Value));
            if (!string.IsNullOrEmpty(newCustomer.WellAddress))
            {
                newCustomer.WellAddress = newCustomer.WellAddress.TrimEnd();
                newCustomer.WellAddress = newCustomer.WellAddress.TrimStart();
            }
            if (string.IsNullOrEmpty(newCustomer.WellAddress) && !(string.IsNullOrEmpty(meterNo)))
            {
                message += " لطفا آدرس چاه مشترک سطر";
                message += i.ToString() + " ";
                message += "را وارد نمایید" + "\r\n";
                SaveErrorLog(filename, message);
                return true;
            }

            newCustomer.Welldepth = "1";
            string val = Convert.ToString(((Range) worksheet.Cells[i, "AC"]).Value);
            if (val != null)
            {
                if (val.Equals("عمیق"))
                {
                    newCustomer.Welldepth = "0";
                }
            }
            newCustomer.TypeOfUse = 3;
            val = Convert.ToString(((Range) worksheet.Cells[i, "AD"]).Value);
            switch (val)
            {
                case "کشاورزی":
                    newCustomer.TypeOfUse = 0;
                    break;
                case "صنعتی":
                    newCustomer.TypeOfUse = 1;
                    break;
                case "شرب":
                    newCustomer.TypeOfUse = 2;
                    break;
            }
            return false;
        }

        private bool CheckPlaneInfo(ShowPlains_Result newPlain, Worksheet worksheet, int i, string meterNo, string message,
            string filename)
        {
            newPlain.PlainName = Convert.ToString((((Range) worksheet.Cells[i, "O"]).Value));
            if (!string.IsNullOrEmpty(newPlain.PlainName))
            {
                newPlain.PlainName = newPlain.PlainName.TrimEnd();
                newPlain.PlainName = newPlain.PlainName.TrimStart();
            }
            if (string.IsNullOrEmpty(newPlain.PlainName) && !(string.IsNullOrEmpty(meterNo)))
            {
                message += " لطفا نام دشت سطر";
                message += i.ToString() + " ";
                message += "را وارد نمایید" + "\r\n";
                SaveErrorLog(filename, message);
                return true;
            }
            return false;
        }

        private bool CheckLocationInfo(ShowCountries_Result newCountry, Worksheet worksheet, int i,
            ShowProvinces_Result newProvince, ShowCities_Result newCity, string meterNo, string message, string filename)
        {
            newCountry.CountryName = Convert.ToString((((Range) worksheet.Cells[i, "L"]).Value));

            newProvince.ProvinceName = Convert.ToString((((Range) worksheet.Cells[i, "M"]).Value));

            newCity.CityName = Convert.ToString((((Range) worksheet.Cells[i, "N"]).Value));
            if (!string.IsNullOrEmpty(newCity.CityName))
            {
                newCity.CityName = newCity.CityName.TrimEnd();
                newCity.CityName = newCity.CityName.TrimStart();
            }
            if (string.IsNullOrEmpty(newCity.CityName) && !(string.IsNullOrEmpty(meterNo)))
            {
                message += " لطفا شهر مشترک سطر";
                message += i.ToString() + " ";
                message += "را وارد نمایید" + "\r\n";
                SaveErrorLog(filename, message);
                return true;
            }
            return false;
        }

        private bool CheckSubscriberNumber(ShowCustomers_Result newCustomer, string meterNo, string message, int i,
            string filename)
        {
            if (!string.IsNullOrEmpty(newCustomer.WatersubscriptionNumber))
            {
                newCustomer.WatersubscriptionNumber = newCustomer.WatersubscriptionNumber.TrimEnd();
                newCustomer.WatersubscriptionNumber = newCustomer.WatersubscriptionNumber.TrimStart();
            }
            if (!string.IsNullOrEmpty(newCustomer.ElecsubscriptionNumber))
            {
                newCustomer.ElecsubscriptionNumber = newCustomer.ElecsubscriptionNumber.TrimEnd();
                newCustomer.ElecsubscriptionNumber = newCustomer.ElecsubscriptionNumber.TrimStart();
            }
            if ((string.IsNullOrEmpty(newCustomer.WatersubscriptionNumber) ||
                 string.IsNullOrEmpty(newCustomer.ElecsubscriptionNumber)) && !(string.IsNullOrEmpty(meterNo)))
            {
                message += " لطفا شماره اشتراک  آب و برق مشترک سطر";
                message += i.ToString() + " ";
                message += "را وارد نمایید" + "\r\n";
                SaveErrorLog(filename, message);
                return true;
            }

            if (string.IsNullOrEmpty(newCustomer.CustomerName) && string.IsNullOrEmpty(newCustomer.Customerfamily) &&
                string.IsNullOrEmpty(meterNo) && string.IsNullOrEmpty(newCustomer.WatersubscriptionNumber) &&
                string.IsNullOrEmpty(newCustomer.ElecsubscriptionNumber))
                return true;
            return false;
        }

        private bool CheckCustomerFamily(ShowCustomers_Result newCustomer, Worksheet worksheet, int i, string meterNo,
            string message, string filename)
        {
            newCustomer.Customerfamily = Convert.ToString((((Range) worksheet.Cells[i, "C"]).Value));
            if (!string.IsNullOrEmpty(newCustomer.Customerfamily))
            {
                newCustomer.Customerfamily = newCustomer.Customerfamily.TrimEnd();
                newCustomer.Customerfamily = newCustomer.Customerfamily.TrimStart();
            }

            if (string.IsNullOrEmpty(newCustomer.Customerfamily) && !(string.IsNullOrEmpty(meterNo)))
            {
                message += "لطفا نام خانوادگی مشترک سطر";
                message += i.ToString() + " ";
                message += "را وارد نمایید" + "\r\n";
                SaveErrorLog(filename, message);
                return true;
            }
            return false;
        }

        private bool CheckCustomerNam(ShowCustomers_Result newCustomer, Worksheet worksheet, int i, string message,
            string filename, ref ShowCustomers customers, ref string meterNo, ref string errorData)
        {
            customers = new ShowCustomers("");
            newCustomer.CustomerName = Convert.ToString((((Range) worksheet.Cells[i, "B"]).Value));


            if (!string.IsNullOrEmpty(meterNo))
            {
                meterNo = meterNo.TrimEnd();
                meterNo = meterNo.TrimStart();
            }

            if (string.IsNullOrEmpty(meterNo) && !(string.IsNullOrEmpty(newCustomer.CustomerName)))
            {
                message += " لطفا نام یا شماره کنتور مشترک سطر";
                message += i.ToString() + " ";
                message += "را وارد نمایید" + "\r\n";
                SaveErrorLog(filename, message);

                return true;
            }
            if (!string.IsNullOrEmpty(newCustomer.CustomerName))
            {
                newCustomer.CustomerName = newCustomer.CustomerName.TrimEnd();
                newCustomer.CustomerName = newCustomer.CustomerName.TrimStart();
            }
            if (string.IsNullOrEmpty(newCustomer.CustomerName) && !(string.IsNullOrEmpty(meterNo)))
            {
                message += " لطفا نام یا شماره کنتور مشترک سطر";
                message += i.ToString() + " ";
                message += "را وارد نمایید" + "\r\n";
                errorData += message;

                return true;
            }
            return false;
        }

        private bool CheckMeterNo(Worksheet worksheet, int i, string filename, out string meterNo)
        {
            meterNo = Convert.ToString((((Range) worksheet.Cells[i, "A"]).Value));

            if (!string.IsNullOrEmpty(meterNo))
            {
                meterNo = meterNo.Trim();
                if (meterNo.StartsWith("19"))
                {
                    if (!(meterNo.Length == 13 || meterNo.Length == 11))
                    {
                        SaveErrorLog(filename, "Invalid Meter Number: " + meterNo + "\r\n");
                        return true;
                    }
                }
                else if (meterNo.StartsWith("207"))
                {
                    if (meterNo.Length != 8)
                    {
                        SaveErrorLog(filename, "Invalid Meter Number: " + meterNo + "\r\n");
                        return true;
                    }
                }
                else
                {
                    SaveErrorLog(filename, "Invalid Meter Number: " + meterNo + "\r\n");
                    return true;
                }
            }
            return false;
        }

        void SaveErrorLog( string filename, string message)
        {
            try
            {
                if(!string .IsNullOrEmpty(filename))
                    File.AppendAllText(filename, message);
            }
            catch (Exception)
            {
                    
            }
            
        }


        public decimal HasCustomer(ShowCustomers_Result customer, ShowCustomers customerList)
        {
            foreach (var item in customerList._lstShowCustomers)
            {
                if (item.CustomerName.Equals(customer.CustomerName)
                    && item.Customerfamily.Equals(customer.Customerfamily)
                    //&& item.FatherName.Equals(customer.FatherName)
                    && item.ElecsubscriptionNumber.Equals(customer.ElecsubscriptionNumber)
                    && item.WatersubscriptionNumber.Equals(customer.WatersubscriptionNumber)
                   // && item.WellLicense.Equals(customer.WellLicense)
                   // && item.WellAddress.Equals(customer.WellAddress)
                    )
                    return item.CustomerID;
            }
            return -1000;
        }

        public string SaveMeter(string meterNumber, string customerId)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter errMsg = new ObjectParameter("ErrMSG", "");
                ObjectParameter meterId = new ObjectParameter("MeterID", 1000000000000);

                SQLSPS.InsMeter(meterNumber, null, true, null, null, null, Convert.ToDecimal(customerId), 1, 1, true, meterId, errMsg, result);

                return meterId.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
                return null;
            }
        }

        private void cmbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbProvince.SelectedItem != null)
                {
                    SelectedProvinces = (ShowProvinces_Result)cmbProvince.SelectedItem;
                    ProvinceID = SelectedProvinces.ProvinceID;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }

        }

        private void cmbPlain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbPlain.SelectedItem != null)
                {
                    SelectedPlain = (ShowPlains_Result)cmbPlain.SelectedItem;
                    PlainID = SelectedPlain.PlainID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void cmbCatchment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCatchment.SelectedItem != null)
                {
                    SelectedCatchments = (ShowCatchments_Result)cmbCatchment.SelectedItem;
                    CatchmentID = SelectedCatchments.CatchmentID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void txtCustomerfamily_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtNationalCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                foreach (char item in e.Text)
                    if (!Char.IsDigit(item) && !Char.IsControl(item))
                        e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void GridMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!IsEditing())
                    return;
                if (GridMain.SelectedItem != null)
                {
                    SelectedCustomer = (ShowCustomers_Result)GridMain.SelectedItem;
                    GridCustomer.DataContext = SelectedCustomer;
                    customerID = SelectedCustomer.CustomerID;

                    foreach (ShowCatchments_Result item in cmbCatchment.Items)
                    {
                        if (item.CatchmentName == SelectedCustomer.CatchmentName)
                            cmbCatchment.SelectedItem = item;
                    }
                    CatchmentID = SelectedCustomer.CatchmentID;
                    cmbCatchment.Text = SelectedCustomer.CatchmentName;


                    foreach (ShowCountries_Result item in cmbCountry.Items)
                    {
                        if (item.CountryName == SelectedCustomer.CountryName)
                            cmbCountry.SelectedItem = item;
                    }
                    cmbCountry.Text = SelectedCustomer.CountryName;
                    CountryID = SelectedCustomer.CountryID;

                    foreach (ShowPlains_Result item in cmbPlain.Items)
                    {
                        if (item.PlainName == SelectedCustomer.PlainName)
                            cmbPlain.SelectedItem = item;
                    }
                    PlainID = SelectedCustomer.PlainID;
                    cmbPlain.Text = SelectedCustomer.PlainName;

                    foreach (ShowProvinces_Result item in cmbProvince.Items)
                    {
                        if (item.ProvinceName == SelectedCustomer.ProvinceName)
                            cmbProvince.SelectedItem = item;
                    }
                    ProvinceID = SelectedCustomer.ProvinceID;
                    cmbProvince.Text = SelectedCustomer.ProvinceName;

                    foreach (ShowOffice_Result item in cmbOffice.Items)
                    {
                        if (item.OfficeDesc == SelectedCustomer.OfficeDesc)
                            cmbOffice.SelectedItem = item;
                    }
                    cmbOffice.Text = SelectedCustomer.OfficeDesc;
                    OfficeID = SelectedCustomer.OfficeID;

                    foreach (ShowSubOffice_Result item in cmbWSubOffice.Items)
                    {
                        if (item.SubOfficeDesc == SelectedCustomer.SubOfficeDesc)
                            cmbWSubOffice.SelectedItem = item;
                    }
                    SubOfficeID = SelectedCustomer.WSubofficeID;
                    cmbWSubOffice.Text = SelectedCustomer.SubOfficeDesc;

                    foreach (ShowESubOffice_Result item in cmbSubOfficeE.Items)
                    {
                        if (item.ESubOfficeDesc == SelectedCustomer.ESubOfficeDesc)
                            cmbSubOfficeE.SelectedItem = item;
                    }
                    //EsubOfficeID = SelectedCustomer.ESubofficeID;
                    //cmbSubOfficeE.Text = SelectedCustomer.ESubOfficeDesc;



                    foreach (ShowEOffice_Result item in cmbEOffice.Items)
                    {
                        if (item.OfficeDesc == SelectedCustomer.EOfficeDesc)
                            cmbEOffice.SelectedItem = item;
                    }
                    EOfficeID = SelectedCustomer.EofficeID;
                    cmbEOffice.Text = SelectedCustomer.EOfficeDesc;

                    EsubOfficeID = SelectedCustomer.ESubofficeID;
                    cmbSubOfficeE.Text = SelectedCustomer.ESubOfficeDesc;


                    txtAddress.Text = SelectedCustomer.CustomerAddress;
                    txtCity.Text = SelectedCustomer.CityName;
                    txtCustomerfamily.Text = SelectedCustomer.Customerfamily;
                    txtCustomerName.Text = SelectedCustomer.CustomerName;
                    //txtDiameterofpipe.Text = SelectedCustomer.Diameterofpipe.ToString();
                    txtdossier.Text = SelectedCustomer.DossierNumber;
                    txtEsubscNumber.Text = SelectedCustomer.ElecsubscriptionNumber;
                    //txtfatherName.Text = SelectedCustomer.FatherName;
                    txtFlowindossier.Text = SelectedCustomer.Flowindossier;
                    txtLatitude.Text = SelectedCustomer.Latitude;
                    txtLongitude.Text = SelectedCustomer.Longitude;
                    txtMobile.Text = SelectedCustomer.MobileNumber;
                    txtNationalCode.Text = SelectedCustomer.NationalCode.ToString();
                    txtPostCode.Text = SelectedCustomer.PostCode;
                    txtTel.Text = SelectedCustomer.CustomerTel;
                    txtWellAddress.Text = SelectedCustomer.WellAddress;
                    cmbWelldepth.SelectedIndex = NumericConverter.IntConverter(SelectedCustomer.Welldepth);
                    cmbTypeOfUse.SelectedIndex = SelectedCustomer.TypeOfUse;
                    //txtWellLicense.Text = SelectedCustomer.WellLicense;
                    txtWsubscNumber.Text = SelectedCustomer.WatersubscriptionNumber;
                    IsNew = false;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }



        private void txtCustomerfamily_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (IsNew && !Isfirsttime)
                {
                    GridCustomer.DataContext = null;
                    Isfirsttime = false;
                }
                ISEditing = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void GridMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    if (!ISEDiting())
            //        return;
            //    if (GridMain.SelectedItem != null)
            //    {
            //        SelectedCustomer = (ShowCustomers_Result)GridMain.SelectedItem;
            //        GridCustomer.DataContext = SelectedCustomer;
            //        customerID = SelectedCustomer.CustomerID;
            //        cmbCatchment.Text = SelectedCustomer.CatchmentName;
            //        CatchmentID = SelectedCustomer.CatchmentID;
            //        PlainID = SelectedCustomer.PlainID;
            //        cmbCountry.Text = SelectedCustomer.CountryName;
            //        CountryID = SelectedCustomer.CountryID;
            //        cmbPlain.Text = SelectedCustomer.PlainName;
            //        cmbProvince.Text = SelectedCustomer.ProvinceName;
            //        cmbOffice.Text = SelectedCustomer.OfficeDesc;
            //        OfficeID = SelectedCustomer.OfficeID;
            //        SubOfficeID = SelectedCustomer.WSubofficeID;
            //        EOfficeID = SelectedCustomer.EofficeID;
            //        EsubOfficeID = SelectedCustomer.ESubofficeID;
            //        txtAddress.Text = SelectedCustomer.CustomerAddress;
            //        txtCity.Text = SelectedCustomer.CityName;
            //        txtCustomerfamily.Text = SelectedCustomer.Customerfamily;
            //        txtCustomerName.Text = SelectedCustomer.CustomerName;
            //        txtDiameterofpipe.Text = SelectedCustomer.Diameterofpipe.ToString();
            //        txtdossier.Text = SelectedCustomer.DossierNumber;
            //        txtEsubscNumber.Text = SelectedCustomer.ElecsubscriptionNumber.ToString();
            //        txtfatherName.Text = SelectedCustomer.FatherName;
            //        txtFlowindossier.Text = SelectedCustomer.Flowindossier.ToString();
            //        txtLatitude.Text = SelectedCustomer.Latitude;
            //        txtLongitude.Text = SelectedCustomer.Longitude;
            //        txtMobile.Text = SelectedCustomer.MobileNumber;
            //        txtNationalCode.Text = SelectedCustomer.NationalCode.ToString();
            //        txtPostCode.Text = SelectedCustomer.PostCode;
            //        txtTel.Text = SelectedCustomer.CustomerTel;
            //        txtWellAddress.Text = SelectedCustomer.WellAddress;
            //        txtWelldepth.Text = SelectedCustomer.Welldepth.ToString();
            //        txtWellLicense.Text = SelectedCustomer.WellLicense;
            //        txtWsubscNumber.Text = SelectedCustomer.WatersubscriptionNumber;
            //        IsNew = false;
            //        this.Focus();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            //}
            GridMain_SelectionChanged(sender, null);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (IsClose)
                {
                    customerID = null;
                    CustomerName = "";
                    NationCode = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void cmbOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbOffice.SelectedItem != null)
                {
                    var showoffice = (ShowOffice_Result)cmbOffice.SelectedItem;
                    OfficeID = showoffice.OfficeID;
                    //  1.0.7.7
                    string filter = " and OfficeID=" + OfficeID+ " and usertogroup.userid= "+CommonData.UserID;
                    RefreshcmbWSubOffice(filter);
                    RefreshcmbPlain(" and ProvinceID ="+ OfficeID + " and usertogroup.userid= " + CommonData.UserID);

                }
                else
                    OfficeID = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void cmbWSubOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbWSubOffice.SelectedItem != null)
                {
                    var showoffice = (ShowSubOffice_Result)cmbWSubOffice.SelectedItem;
                    SubOfficeID = showoffice.SubOfficeID;
                }
                else
                    SubOfficeID = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void cmbEOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbEOffice.SelectedItem != null)
                {
                    var showoffice = (ShowEOffice_Result)cmbEOffice.SelectedItem;
                    EOfficeID = showoffice.OfficeID;
                    string filter = " and OfficeID=" + EOfficeID;
                    RefreshcmbSubOfficeE(filter);

                }
                else
                    EOfficeID = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void cmbSubOfficeE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbSubOfficeE.SelectedItem != null)
                {
                    var showoffice = (ShowESubOffice_Result)cmbSubOfficeE.SelectedItem;
                    EsubOfficeID = showoffice.ESubOfficeID;
                }
                else
                    EsubOfficeID = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (us.CanInsert)
                ToolStripButtonNew_Click(null, null);
        }

        private void GridMain_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcount.Content = rowcount + "=" + GridMain.Items.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }
    }
}