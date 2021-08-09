
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;
using Card;
using DLMS.DTOEntities;

using HandHeldReader;
using HHU303.DTOEntities;
using SABA_CH.DataBase;
using SABA_CH.Global;
using SABA_CH.UI;
using VEE;
using Application = System.Windows.Application;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using MessageBoxOptions = System.Windows.MessageBoxOptions;
using OBISObject = HandHeldReader.OBISObject;
using Orientation = System.Windows.Controls.Orientation;
using ProgressBar = System.Windows.Controls.ProgressBar;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Setting = SABA_CH.UI.Setting;
using Settings = SABA_CH.Properties.Settings;
using ShowConsumedWaterForVee_Result = VEE.ShowConsumedWaterForVee_Result;
using ShowVEEConsumedWaterForVEE_Result = VEE.ShowVEEConsumedWaterForVEE_Result;
using RSA.SmartCard.Framework;
using RsaDateTime;
using System.Drawing;
using HHU303;
using SABA_CH.Views;
using VEE.WaterData; 
using System.Linq;
using SABA_CH.VEEClasses;
using DevExpress.Charts.Native;
using System.Configuration;
using CardToMeter = SABA_CH.UI.CardToMeter;

namespace SABA_CH
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : System.Windows.Window
    {
        public List<OBISObject> objectList { get; set; }
        public List<MeterData> objectList1 { get; set; }
        private string[] CurveList;

        #region CardIfo
        DateTime cardDetectionOldTime = new DateTime();
        const string DefaultReader = "Gemplus USB Smart Card Reader 0";

        public bool ReadFromCard = false;
        public bool ReadFromHHU = false;

        #endregion CardIfo

        //public Card card = null;
        public List<SelectedMeter> lstSelectedMeter = null;
        public ShowTranslateofLable tr = null;
        public ShowTranslateofLable translateWindowName = null;
        public int SelectedRow = 0;
        System.Windows.Window mainWindow = Application.Current.MainWindow;
        public List<SelectedMeter> MeterList { get; set; }
        public ShowLanguage Language = null;
        public static int progressBarValue;
        public ShowButtonAccess_Result us = null;
        public ShowGroups_Result SelectedGroup = null;
        public ShowTranslateofMessage tm = null;
        public Thread th;
        public new SabaNewEntities Bank;
        public bool SelectAll = false;
        public CardBase m_iCard = null;
        public bool ISchecked = false;
        List<string> MeterID, MeterNumber;
  

        DateTime ExpirationDate = new DateTime(2017, 01, 01);

        ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
        ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
        ObjectParameter ID = new ObjectParameter("ID", 10000000000000000);

        public MainWindow()
        {
            WaitingForm oWaitingForm =null ;

            try
            {
                SabaNewEntities.DataSourceName = "SabaCandH"; ;
                System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                 
                foreach (Process proc in Process.GetProcesses())
                    if (proc.ProcessName.ToUpper().Equals("SABA") && Process.GetCurrentProcess().Id != proc.Id)
                    {
                        //System.Windows.Forms.MessageBox.Show(proc.ProcessName.ToUpper());
                        proc.Kill();
                        break;
                    }

                // Application.Run(...);
                InitializeComponent();


                CommonData.getResponserFromDangle = false;
                // ahmad run without dongle 
                //CommonData.getResponserFromDangle = true;

                changeProgressBarValue(0);
                CommonData.mainwindow = this;
                changeMainPageGrid(false);
                ClassControl.Init();


                Login login = new Login();
                login.ShowDialog();

                System.Threading.Tasks.Task.Delay(3000);

                oWaitingForm = new WaitingForm();
                oWaitingForm.Show();
                oWaitingForm.Close();

            }
            catch (Exception ex)
            {
                if (ex.ToString().ToLower().Contains("sql"))
                {
                    System.Windows.Forms.MessageBox.Show("خطا در برقراری ارتباط با پایگاه داده");
                    Environment.Exit(1);
                    return;
                }
                else
                {
                    Login login = new Login();
                    login.ShowDialog();
                    Application.Current.Run(login);
                }
            }

            

            CommonData.GetShutdownInterval = Properties.Settings.Default.GetShutdownInterval;

            tr = CommonData.translateWindow(1);
            tm = CommonData.translateMessage();
            translateWindowName = CommonData.translateWindow(100);
            ChangeFlowdirection();
            grid.DataContext = tr;
            Gridmn.DataContext = tr.TranslateofLable;
            changeHeaderMeterGrid();

            


            this.Owner = mainWindow;
            //ahmad dont check dongle
            
            //ahmad 
            //CommonData.Citycode = "00000001";

            //ahmad 
            //grid.IsEnabled = true;
            //ahmad

            

            changeProgressBarTag(tm.TranslateofMessage.Message41);
            //RefreshSelectedMeters("");                
            lblUser.Content = CommonData.UserName;
            CommonData.mainwindow = this;
            this.Title = tm.TranslateofMessage.Message60;

            SelectICard();

            ActiveSoftwareWindow.Instance.CheckDangle();
        }

        void InitiateNewDataBase()
        {
            WaitingForm oWaitingForm = null;

            try
            {

                System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

               


                CommonData.getResponserFromDangle = false;
                // ahmad run without dongle 
                //CommonData.getResponserFromDangle = true;

                changeProgressBarValue(0);
                CommonData.mainwindow = this;
                changeMainPageGrid(false);
                ClassControl.Init();


                Login login = new Login();
                login.ShowDialog();

                System.Threading.Tasks.Task.Delay(3000);

                oWaitingForm = new WaitingForm();
                oWaitingForm.Show();
                oWaitingForm.Close();

            }
            catch (Exception ex)
            {
                if (ex.ToString().ToLower().Contains("sql"))
                {
                    System.Windows.Forms.MessageBox.Show("خطا در برقراری ارتباط با پایگاه داده");
                    Environment.Exit(1);
                    return;
                }
                else
                {
                    Login login = new Login();
                    login.ShowDialog();
                    Application.Current.Run(login);
                }
            }



            CommonData.GetShutdownInterval = Properties.Settings.Default.GetShutdownInterval;

            tr = CommonData.translateWindow(1);
            tm = CommonData.translateMessage();
            translateWindowName = CommonData.translateWindow(100);
            ChangeFlowdirection();
            grid.DataContext = tr;
            Gridmn.DataContext = tr.TranslateofLable;
            changeHeaderMeterGrid();




            this.Owner = mainWindow;
            //ahmad dont check dongle

            //ahmad 
            //CommonData.Citycode = "00000001";

            //ahmad 
            //grid.IsEnabled = true;
            //ahmad



            changeProgressBarTag(tm.TranslateofMessage.Message41);
            //RefreshSelectedMeters("");                
            lblUser.Content = CommonData.UserName;
            CommonData.mainwindow = this;
            this.Title = tm.TranslateofMessage.Message60;

            SelectICard();

            ActiveSoftwareWindow.Instance.CheckDangle();
        }

        internal void ChangeEnable(bool isEnabled, string text)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
      new Action(
          delegate ()
          {
              IsEnabled = isEnabled;
              changeProgressBarTag(text);

          }));
        }

        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(e.ToString());
            Exception ex = new Exception(e.ToString());
            CommonData.WriteLOG(ex);
            Application.Current.Shutdown();
            Application.Current.Run();
        }

        public void changeMainPageGrid(bool enable)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
               delegate ()
               {
                   grid.IsEnabled = enable;
               }));
        }

        public bool CheckingSoftwearVersion()
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                //string VersionSoftwear = AppDomain.CurrentDomain.DomainManager.EntryAssembly.GetName().Version.ToString();
                Bank.Database.Connection.Open();
                string Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                // Getting the version:
                CommonData.ProgramVersion = Assembly.GetEntryAssembly().GetName().Version.ToString(); //AppDomain.CurrentDomain.DomainManager.EntryAssembly.GetName().Version.ToString();
                Bank.InsChangeDB(Date, CommonData.ProgramVersion, ID, Result, ErrMSG);
                if (Int64.Parse(ID.Value.ToString()) > 1000)
                {
                    CommonData.mainwindow.changeProgressBarValue(1);
                    CommonData.mainwindow.changeProgressBarTag("Data Restored");
                    Bank.Database.Connection.Close();
                    Bank.Dispose();
                    return false;
                }
                else
                {
                    CommonData.mainwindow.changeProgressBarValue(1);
                    CommonData.mainwindow.changeProgressBarTag("Data Restored");
                    Bank.Database.Connection.Close();
                    Bank.Dispose();
                    return true;
                }

            }
            catch (Exception ex)
            {
                BackupDatabase("SabacandH");
                SabaNewEntities Bank = new SabaNewEntities();
             
                foreach (string command in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.sql"))
                {
                    if (command.Contains("SQL6") || command.Contains("SQL7"))
                    {
                        // MessageBox.Show("001");
                        ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 40000;

                        string script = File.ReadAllText(command);
                     
                        string s = Bank.Database.Connection.ConnectionString;
                        SqlConnection conn = new SqlConnection(s);
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(script, conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        conn.Dispose();
                        //}

                    }

                }
                CommonData.mainwindow.changeProgressBarValue(1);
                CommonData.mainwindow.changeProgressBarTag("Data Restored");
                Bank.Database.Connection.Close();
                Bank.Dispose();
                return false;
            }
        }

       
        public bool TrialCheckedIsValid()
        {
            try
            {
                SetExpireDate();

                if (Settings.Default.LastDate == string.Empty)
                    SetLastDate();
                if (about != null)
                    about.ShowExpiredDate();

                DateTime last =RsaDateTime.PersianDateTime.ConvertToGeorgianDateTime1(Settings.Default.LastDate);
                
                if (ExpirationDate < DateTime.Now)
                {
                    CommonData.IsExpire = true;
                    ChangeEnable(false, "زمان استفاده از این نرم افزار سپری شده است.در صورت تمایل کد فعال سازی جدید وارد کنید.");
                    var r1 = MessageBox.Show("زمان استفاده از این نرم افزار سپری شده است. در صورت تمایل کد فعال سازی جدید وارد کنید.", "هشدار اتمام اعتبار", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (r1 == MessageBoxResult.Yes)
                    {
                        ActiveSoftwareWindow.Instance.ShowDialogForm();
                        return true; 
                    }
                    
                    return false;
                }
                if (last > DateTime.Now && CommonData.UserName != "Ansari")
                {
                    System.Windows.Forms.MessageBox.Show("لطفا  ساعت کامپیوتر را تنظیم کرده و سپس برنامه را اجرا کنید");
                    return false;
                }
                if (last < ExpirationDate )
                {
                    if (CheckExpirationDate())
                    {
                        CommonData.IsExpire = false;
                    }
                    else
                    {
                       
                        CommonData.IsExpire = true;
                        var x = MessageBox.Show("زمان استفاده از این نرم افزار  کمتر از 10 روز باقی مانده است . در صورت تمایل کد فعال سازی جدید وارد کنید.", "هشدار اتمام اعتبار", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (x == MessageBoxResult.Yes)
                        {
                            ActiveSoftwareWindow.Instance.ShowDialogForm();
                            return true;
                        }
                        CommonData.IsExpire = false;
                        return true; ;
                    }
                    SetLastDate();
                    if (DateTime.Now < ExpirationDate)
                        return true;
                  
                }
            }
            catch (Exception)
            {
            }
            SetLastDate();
            
            return false;
        }

        private void SetExpireDate()
        {
            try
            {
                
                string temp = DateTime.Now.ToPersianString().Substring(0, 2);
                if (CommonData.ExpireDate.StartsWith("00") && (CommonData.ExpireDate.Length == 8))
                {
                    CommonData.ExpireDate = CommonData.ExpireDate.Substring(2, CommonData.ExpireDate.Length - 2);
                }
                if (CommonData.ExpireDate.Length >= 6)
                {
                    var yy = Convert.ToInt32(CommonData.ExpireDate.Substring(0, 2));
                    var year = Convert.ToInt32(14) * 100 + Convert.ToInt32(CommonData.ExpireDate.Substring(0, 2));
                    if (yy > 90)
                    {
                        year = Convert.ToInt32(13) * 100 + Convert.ToInt32(CommonData.ExpireDate.Substring(0, 2));
                    }
                        
                    int month = Convert.ToInt32(CommonData.ExpireDate.Substring(2, 2));
                    int day = Convert.ToInt32(CommonData.ExpireDate.Substring(4, 2));
                    ExpirationDate = PersianDateTime.ConvertToGeorgianDateTime(year, month, day, 0, 0, 0);
                }
                else
                {
                    CommonData.ExpireDate = CommonData.ExpireDate.PadLeft(6, '0');
                    var yy = Convert.ToInt32(CommonData.ExpireDate.Substring(0, 2));
                    var year = Convert.ToInt32(14) * 100 + Convert.ToInt32(CommonData.ExpireDate.Substring(0, 2));
                    if (yy > 90)
                    {
                        year = Convert.ToInt32(13) * 100 + Convert.ToInt32(CommonData.ExpireDate.Substring(0, 2));
                    }
                    
                    int month = Convert.ToInt32(CommonData.ExpireDate.Substring(2, 2));
                    int day = Convert.ToInt32(CommonData.ExpireDate.Substring(4, 2));
                    ExpirationDate = PersianDateTime.ConvertToGeorgianDateTime(year, month, day, 0, 0, 0);

                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
            }
        }

        private bool CheckExpirationDate()
        {
            try
            {
                if (ExpirationDate.Year == DateTime.Now.Year)
                {
                    int Month = (ExpirationDate.Month) - (DateTime.Now.Month);
                    if (Month < 2)
                    {
                        int Day = (Month * 30) + ExpirationDate.Day;
                        if ((Day - DateTime.Now.Day) < 11)
                        {
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                        return true;
                }
                else
                    return true;
            }
            catch (Exception)
            {

                return false; 
            }
            return false;
        }
        private void SetLastDate()
        {
            Settings.Default.LastDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); 

            Settings.Default.Save();
        }
        
        public void changeHeaderMeterGrid()
        {
            try
            {
                MeterGrid.Columns[1].Header = tr.TranslateofLable.Object49.ToString();
                MeterGrid.Columns[2].Header = tr.TranslateofLable.Object50.ToString();
                MeterGrid.Columns[3].Header = tr.TranslateofLable.Object45.ToString();
                MeterGrid.Columns[0].Header = tr.TranslateofLable.Object61.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void RefreshCmbGroups()
        {
            int Index = 0;
            try
            {

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
                        filter = "and m.ProvinceID in (0," + value + ")";
                }
                catch
                {
                    filter = "";
                }

                ShowGroups Groups = new ShowGroups(filter, 1, CommonData.LanguagesID);
                //CmbGroups.ItemsSource = Groups.CollectShowGroups;
                //if (CmbGroups.Items != null)
                //{
                //    CmbGroups.SelectedIndex = 0;
                //}
                if (CommonData.UserID == 1)
                {
                    for (int i = 0; i < Groups._lstShowGroups.Count; i++)
                    {
                        if (Groups._lstShowGroups[i].GroupName == "گروه جامع")
                            Index = i;
                        if (Groups._lstShowGroups[i].GroupName == "همه گروه ها")
                        {
                            Groups._lstShowGroups.Remove(Groups._lstShowGroups[i]);
                            i--;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Groups._lstShowGroups.Count; i++)
                    {
                        if (Groups._lstShowGroups[i].GroupName == "همه گروه ها")
                            break;
                        if (Groups._lstShowGroups[i].GroupName != "همه گروه ها" && i == (Groups._lstShowGroups.Count - 1))
                        {
                            ShowGroups_Result _All = new ShowGroups_Result();
                            _All.GroupID = Groups._lstShowGroups.Count + 1;
                            _All.GroupName = "همه گروه ها";
                            Groups._lstShowGroups.Add(_All);
                        }
                    }
                }

                CmbGroups.ItemsSource = Groups.CollectShowGroups;
                if (CmbGroups.Items != null)
                {
                    CmbGroups.SelectedIndex = Index;
                }

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        ShowDataBasesInfo _dataBasesInfo;
        public void RefreshCmbDataBaseInfo()
        {
            int Index = 0;
            try
            {
                _dataBasesInfo = new ShowDataBasesInfo("");
                if (_dataBasesInfo == null)
                {
                    CmbDataBaseInfo.Visibility = Visibility.Hidden;
                    lblDataBaseInfo.Visibility = Visibility.Hidden;
                }
                else if (_dataBasesInfo._lstShowDataBasesInfo == null)
                {
                    CmbDataBaseInfo.Visibility = Visibility.Hidden;
                    lblDataBaseInfo.Visibility = Visibility.Hidden;
                }
                else if (_dataBasesInfo._lstShowDataBasesInfo.Count == 0)
                {
                    CmbDataBaseInfo.Visibility = Visibility.Hidden;
                    lblDataBaseInfo.Visibility = Visibility.Hidden;
                }
                else
                {
                    List<string> names = new List<string>();
                    foreach (var item in _dataBasesInfo._lstShowDataBasesInfo)
                    {
                        if (item.Name.ToLower().Equals("local"))
                            names.Insert(0, item.Name);
                        else
                            names.Add(item.Name);
                    }


                    CmbDataBaseInfo.ItemsSource = names;
                    if (CmbDataBaseInfo.Items != null)
                    {
                        CmbDataBaseInfo.SelectedIndex = Index;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                CmbDataBaseInfo.Visibility = Visibility.Hidden;
                lblDataBaseInfo.Visibility = Visibility.Hidden;

            }
            firstLogin = false;
        }
        public void ChangeFlowdirection()
        {
            MenuItem mnuitem = null, mnusubitem;

            //menu.FlowDirection = CommonData.FlowDirection;
            foreach (var item in menu.Items)
            {
                mnuitem = (MenuItem)item;
                //mnuitem.FlowDirection = CommonData.FlowDirection;
                if (mnuitem.Items.Count > 0)
                {
                    foreach (var itemsub in mnuitem.Items)
                    {
                        mnusubitem = (MenuItem)itemsub;
                        //mnusubitem.FlowDirection = CommonData.FlowDirection;
                    }
                }
            }
            //mnudown.FlowDirection = CommonData.FlowDirection;
            //grid.FlowDirection = CommonData.FlowDirection;
            //Gridmn.FlowDirection = CommonData.FlowDirection;
            //grid_image.FlowDirection = CommonData.FlowDirection;
            //MeterGrid.FlowDirection = CommonData.FlowDirection;
        }
        private void Fortest(object sender, RoutedEventArgs e)
        {

        }
        private void WaterReport(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 18);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[18])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[18] = true;
                        WaterManagementReport WatermanageReport = new WaterManagementReport();
                        WatermanageReport.Title = translateWindowName.TranslateofLable.Object18;
                        WatermanageReport.Owner = this;
                        WatermanageReport.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = WatermanageReport.Title;
                        theTabItem.windowname = WatermanageReport;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        WatermanageReport.TabPag = theTabItem;
                        WatermanageReport.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        private void WaterPublicInformation(object sender, RoutedEventArgs e)
        {            
            try
            {
                us = CommonData.ShowButtonBinding("", 30);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[34])
                    {
                        string Result = CheckMeterForCustomerandGroup(CommonData.MeterNumber);
                        if (Result == "True")
                        {
                            CommonData.mainwindow.changeProgressBarTag("");
                            CommonData.mainwindow.changeProgressBarValue(0);
                            ClassControl.OpenWin[34] = true;
                            ShowCardData Water = new ShowCardData();
                            Water.Title = translateWindowName.TranslateofLable.Object6;
                            Water.Owner = this;
                            Water.Tab = tabControl1;
                            ClosableTab theTabItem = new ClosableTab();
                            theTabItem.Title = Water.Title;
                            theTabItem.windowname = Water;
                            tabControl1.Items.Add(theTabItem);
                            theTabItem.Focus();
                            Water.TabPag = theTabItem;
                            Water.tabControlMain.SelectedItem = Water.tabitemgeneralWater;
                            Water.Show();
                            CommonData.showmeterdata = Water;
                        }
                        else if (Result == "NO Group")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message94, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                        else if (Result == "NO Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message93, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                        else if (Result == "NO Group and Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message96, "",
                                MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void MonhanyInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 31);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[34])
                    {
                        string result = CheckMeterForCustomerandGroup(CommonData.MeterNumber);
                        if (result == "True")
                        {
                            CommonData.mainwindow.changeProgressBarTag("");
                            CommonData.mainwindow.changeProgressBarValue(0);
                            ClassControl.OpenWin[34] = true;
                            ShowCardData Water = new ShowCardData();
                            Water.Title = translateWindowName.TranslateofLable.Object6;
                            Water.Owner = this;
                            Water.Tab = tabControl1;
                            ClosableTab theTabItem = new ClosableTab();
                            theTabItem.Title = Water.Title;
                            theTabItem.windowname = Water;
                            tabControl1.Items.Add(theTabItem);
                            theTabItem.Focus();
                            Water.TabPag = theTabItem;
                            //Water.tabControlMain.SelectedItem = Water.tabite;
                            Water.Show();
                            CommonData.showmeterdata = Water;
                        }
                        else if (result == "NO Group")
                        {
                            MessageBoxResult mbResult = MessageBox.Show(tm.TranslateofMessage.Message94, "", MessageBoxButton.OK);
                            if (mbResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                        else if (result == "NO Customer")
                        {
                            MessageBoxResult mbResult = MessageBox.Show(tm.TranslateofMessage.Message93, "", MessageBoxButton.OK);
                            if (mbResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                        else if (result == "NO Group and Customer")
                        {
                            MessageBoxResult mbResult = MessageBox.Show(tm.TranslateofMessage.Message96, "",
                                MessageBoxButton.OK);
                            if (mbResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void UseWaterInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 32);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[34])
                    {
                        string Result = CheckMeterForCustomerandGroup(CommonData.MeterNumber);
                        if (Result == "True")
                        {
                            CommonData.mainwindow.changeProgressBarTag("");
                            CommonData.mainwindow.changeProgressBarValue(0);
                            ClassControl.OpenWin[34] = true;
                            ShowCardData Water = new ShowCardData();
                            Water.Title = translateWindowName.TranslateofLable.Object6;
                            Water.Owner = this;
                            Water.Tab = tabControl1;
                            ClosableTab theTabItem = new ClosableTab();
                            theTabItem.Title = Water.Title;
                            theTabItem.windowname = Water;
                            tabControl1.Items.Add(theTabItem);
                            theTabItem.Focus();
                            Water.TabPag = theTabItem;
                            Water.tabControlMain.SelectedItem = Water.tabitemConsumedWater;
                            Water.Show();
                            CommonData.showmeterdata = Water;
                        }
                        else if (Result == "NO Group")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message94, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                        else if (Result == "NO Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message93, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                        else if (Result == "NO Group and Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message96, "",
                                MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ShowWater(object sender, RoutedEventArgs e)
        {

        }

        private void ShowCredit(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 14);
                if (us.CanShow)
                {
                    GetMeterNumberForCredite();
                    if (!ClassControl.OpenWin[14])
                    {

                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[14] = true;
                        CreditToMeter w5 = new CreditToMeter();
                        if (MeterNumber.Count > 0)
                            w5.Title = translateWindowName.TranslateofLable.Object14 + CommonData.SelectedMeterNumber;
                        else
                            w5.Title = translateWindowName.TranslateofLable.Object14;

                        w5.Owner = this;
                        w5.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = w5.Title;
                        theTabItem.windowname = w5;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        w5.TabPag = theTabItem;
                        w5.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }

        }

        private void ElectricityIfo(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 33);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[34])
                    {
                        string Result = CheckMeterForCustomerandGroup(CommonData.MeterNumber);
                        if (Result == "True")
                        {
                            CommonData.mainwindow.changeProgressBarTag("");
                            CommonData.mainwindow.changeProgressBarValue(0);
                            ClassControl.OpenWin[34] = true;
                            ShowCardData Water = new ShowCardData();
                            Water.Title = translateWindowName.TranslateofLable.Object6;
                            Water.Owner = this;
                            Water.Tab = tabControl1;
                            ClosableTab theTabItem = new ClosableTab();
                            theTabItem.Title = Water.Title;
                            theTabItem.windowname = Water;
                            tabControl1.Items.Add(theTabItem);
                            theTabItem.Focus();
                            Water.TabPag = theTabItem;
                            Water.tabControlMain.SelectedItem = Water.tabitemGeneralInfoElectrical;
                            Water.Show();
                            CommonData.showmeterdata = Water;
                        }
                        else if (Result == "NO Group")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message94, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                        else if (Result == "NO Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message93, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                        else if (Result == "NO Group and Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message96, "",
                                MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void UseElectricity(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 40);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[34])
                    {
                        string Result = CheckMeterForCustomerandGroup(CommonData.MeterNumber);
                        if (Result == "True")
                        {
                            CommonData.mainwindow.changeProgressBarTag("");
                            CommonData.mainwindow.changeProgressBarValue(0);
                            ClassControl.OpenWin[34] = true;
                            ShowCardData Water = new ShowCardData();
                            Water.Title = translateWindowName.TranslateofLable.Object6;
                            Water.Owner = this;
                            Water.Tab = tabControl1;
                            ClosableTab theTabItem = new ClosableTab();
                            theTabItem.Title = Water.Title;
                            theTabItem.windowname = Water;
                            tabControl1.Items.Add(theTabItem);
                            theTabItem.Focus();
                            Water.TabPag = theTabItem;
                            Water.tabControlMain.SelectedItem = Water.tabitemPowerConsumption;
                            Water.Show();
                            CommonData.showmeterdata = Water;
                        }
                        else if (Result == "NO Group")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message94, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                        else if (Result == "NO Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message93, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                        else if (Result == "NO Group and Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message96, "",
                                MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Tarefe(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 41);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[34])
                    {
                        string Result = CheckMeterForCustomerandGroup(CommonData.MeterNumber);
                        if (Result == "True")
                        {
                            CommonData.mainwindow.changeProgressBarTag("");
                            CommonData.mainwindow.changeProgressBarValue(0);
                            ClassControl.OpenWin[34] = true;
                            ShowCardData Water = new ShowCardData();
                            Water.Title = translateWindowName.TranslateofLable.Object6;
                            Water.Owner = this;
                            Water.Tab = tabControl1;
                            ClosableTab theTabItem = new ClosableTab();
                            theTabItem.Title = Water.Title;
                            theTabItem.windowname = Water;
                            tabControl1.Items.Add(theTabItem);
                            theTabItem.Focus();
                            Water.TabPag = theTabItem;
                            Water.tabControlMain.SelectedIndex = 10;
                            Water.Show();
                            CommonData.showmeterdata = Water;
                        }
                        else if (Result == "NO Group")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message94, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                        else if (Result == "NO Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message93, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                        else if (Result == "NO Group and Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message96, "",
                                MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }



        private void WaterEvent(object sender, RoutedEventArgs e)
        {

        }

        private void ElectronicEvent(object sender, RoutedEventArgs e)
        {

        }

        private void PublicEvent(object sender, RoutedEventArgs e)
        {

        }

        private void ShowCarts(object sender, RoutedEventArgs e)
        {
            us = CommonData.ShowButtonBinding("", 15);
            if (us.CanShow)
            {
                GetMeterNumberForCards();
                if (MeterNumber.Count > 0)
                {
                    if (!ClassControl.OpenWin[15])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[15] = true;
                        CardToMeter cardtometer = new CardToMeter();
                        cardtometer.Title = translateWindowName.TranslateofLable.Object15 + " " + CommonData.SelectedMeterNumber;
                        cardtometer.Owner = this;
                        cardtometer.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = cardtometer.Title;
                        theTabItem.windowname = cardtometer;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        cardtometer.TabPag = theTabItem;
                        cardtometer.Show();
                    }

                    else
                    {
                        MessageBox.Show(tm.TranslateofMessage.Message55);
                        return;
                    }
                }

                else
                {
                    if (!ClassControl.OpenWin[27])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[27] = true;
                        CardsToMeters cardstometers = new CardsToMeters();
                        cardstometers.Title = translateWindowName.TranslateofLable.Object15;
                        cardstometers.Owner = this;
                        cardstometers.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = cardstometers.Title;
                        theTabItem.windowname = cardstometers;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        cardstometers.TabPag = theTabItem;
                        cardstometers.Show();
                    }
                    else
                    {
                        MessageBox.Show(tm.TranslateofMessage.Message55);
                        return;
                    }
                }
            }
        }
        public void GetMeterNumberForCards()
        {
            try
            {
                MeterID = new List<string>();
                MeterNumber = new List<string>();
                if (CommonData.mainwindow.MeterGrid.Items.Count > 0)
                {
                    for (int i = 0; i < CommonData.mainwindow.MeterGrid.Items.Count; i++)
                    {
                        SelectedMeter selectedMeter = new SelectedMeter();
                        selectedMeter = (SelectedMeter)CommonData.mainwindow.MeterGrid.Items[i];
                        if (selectedMeter.Isvisable)
                        {
                            MeterID.Add(selectedMeter.MeterId.ToString());
                            MeterNumber.Add(selectedMeter.MeterNumber);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void GetMeterNumberForCredite()
        {
            try
            {
                MeterID = new List<string>();
                MeterNumber = new List<string>();
                if (CommonData.mainwindow.MeterGrid.Items.Count > 0)
                {
                    for (int i = 0; i < CommonData.mainwindow.MeterGrid.Items.Count; i++)
                    {
                        SelectedMeter selectedMeter = new SelectedMeter();
                        selectedMeter = (SelectedMeter)CommonData.mainwindow.MeterGrid.Items[i];
                        if (selectedMeter.Isvisable)
                        {
                            MeterID.Add(selectedMeter.MeterId.ToString());
                            MeterNumber.Add(selectedMeter.MeterNumber);
                            CommonData.SelectedMeterID = selectedMeter.MeterId;
                            CommonData.SelectedMeterNumber = selectedMeter.MeterNumber;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void GetInfoCart(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 20);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[20])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[20] = true;
                        ImportDataFromSQLCe import = new ImportDataFromSQLCe();
                        import.Title = translateWindowName.TranslateofLable.Object20;
                        import.Owner = this;
                        import.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = import.Title;
                        theTabItem.windowname = import;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        import.TabPag = theTabItem;
                        import.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void SABAPC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 48);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[48])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[48] = true;
                        ImportFromSABAPC import = new ImportFromSABAPC();
                        import.Title = translateWindowName.TranslateofLable.Object48;
                        import.Owner = this;
                        import.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = import.Title;
                        theTabItem.windowname = import;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        import.TabPag = theTabItem;
                        import.Show();
                        
                    }
                  
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }

        }
        public void ReadHHU()
        {
            try
            {
                MeterList = new List<SelectedMeter>();
                changeProgressBarValue(0);
                changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);


                ObjectParameter result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter errMsg = new ObjectParameter("ErrMSG", "");

                HHUReadOut hhureadout = HandHeldReader.HandHeldReader.PortableDeviceReader();

               th = new Thread(delegate () { getDataFromHHU(hhureadout); });
                th.SetApartmentState(ApartmentState.STA);
                th.IsBackground = true;
                th.Start();

            }
            catch (Exception ex)
            {
                //th.Suspend();
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }

        }
        public void ReadHhuFile()
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();


                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var data303 = HHU303.HHU303.ReadHHuFile(fbd.SelectedPath);
                    var data207 = HandHeldReader.HandHeldReader.ReadFromHhuFile(fbd.SelectedPath);
                    if (data207.ReadOutList != null && data207.ReadOutList.Count > 0)
                        getDataFromHHUFile(data207);
                    else if (data303.hhureadout != null && data303.hhureadout.Count > 0)
                        getDataFromHHU303(data303);
                    else
                    {
                        CommonData.mainwindow.changeProgressBarTag("اطلاعات معتبری برای ذخیره سازی یافت نشد");
                    }
                }

            }
            catch (Exception ex)
            {
                //th.Suspend();
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }

        }
        public void ReadHhu303()
        {
            try
            {
                MeterList = new List<SelectedMeter>();
                changeProgressBarValue(0);
                changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);

                ObjectParameter result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter errMsg = new ObjectParameter("ErrMSG", "");

                HHUReadout data = HHU303.HHU303.PortableDeviceReader();

                th = new Thread(delegate () { getDataFromHHU303(data); });
                th.SetApartmentState(ApartmentState.STA);
                th.IsBackground = true;
                th.Start();

            }
            catch (Exception ex)
            {
                //th.Suspend();
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }

        }


        public void getDataFromHHU303(HHUReadout hhureadout)
        {
            try
            {
                MeterList = null;
                int curveIndex = 1;
                var softwareVersion = "";
                bool read207 = false;
                /////
                if (hhureadout != null)
                {
                    if (hhureadout.ErrorMessage == "-1")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message33);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorMessage == "-2")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message72);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorMessage == "-3")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message73);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }

                    else if (hhureadout.ErrorMessage == "-5" || hhureadout.hhureadout.Count<1)
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message86);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorMessage == "" && hhureadout.hhureadout != null)
                    {

                        changeProgressBar_MaximumValue(hhureadout.hhureadout.Count * 5);
                         
                        for (int i = 0; i < hhureadout.hhureadout.Count; i++)
                        {
                            if (hhureadout.hhureadout[i].meterNo != "")
                            {

                                 
                                foreach (var item in hhureadout.hhureadout[i].readoutList[0].MeterObjects)
                                {
                                    if (item.LogicalName == "0100000200FF" && item.AttributeID == 2 && item.ClaseID == 1)
                                    {
                                        if (!item.ValueString.StartsWith("RSA"))
                                        {
                                            softwareVersion = CommonData.ReverceASCIIStringToString(item.ValueString); break;
                                        }
                                        else
                                        {
                                            softwareVersion = item.ValueString; break;
                                        }
                                    }
                                }
                                if (softwareVersion.StartsWith("RSAS") && Convert.ToInt16(softwareVersion.Substring(softwareVersion.Length - 1)) > 4)
                                {
                                    softwareVersion = "RSASEWM303L0D6112104";
                                }
                                var meterNo = hhureadout.hhureadout[i].meterNo;
                                var meterId = AddMeterToList(meterNo, "", softwareVersion,out var  customerId);
                                if (hhureadout.hhureadout[i].meterNo.StartsWith("207"))
                                    read207 = true;
                                else
                                    read207 = false;

                                CommonData.counter = i;

                                for (int j = 0; j < hhureadout.hhureadout[i].readoutList.Count; j++)
                                {
                                    var result = new ObjectParameter("Result", 10000000000000000);
                                    
                                    var errMsg = new ObjectParameter("ErrMSG", "");
                                    ObjectParameter OBISValueHeaderID = new ObjectParameter("OBISValueHeaderID", 1000000000000000);
                                    var clockValue = CommonData.ByteArrayStringToDateTime(hhureadout.hhureadout[i].readoutList[j].MeterObjects[1].ValueString).ToString();
                                    
                                    SQLSPS.InsobisValueHeader(RsaDateTime.PersianDateTime.ConvertToPersianDateTime(clockValue), 1, CommonData.UserID, hhureadout.hhureadout[i].readoutList[j].ReadDate, meterId, 3, OBISValueHeaderID, Result, ErrMSG);
                                    curveIndex = 1;
                                    #region MyRegion
                                    foreach (var item in hhureadout.hhureadout[i].readoutList[j].MeterObjects)
                                    {
                                        if (item.LogicalName == "0802802800FF")
                                        {
                                            /*
                                             CurveList[0]  Curve- Code
                                             CurveList[1]  Curve-Flow 0
                                             CurveList[2]  Curve-Flow 1
                                             CurveList[3]  Curve-Flow 2
                                             CurveList[4]  Curve-Flow 3
                                             CurveList[5]  Curve-Flow 4
                                             CurveList[6]  Curve-Flow 5
                                             CurveList[7]  Curve-Power 0
                                             CurveList[8]  Curve-Power 1
                                             CurveList[9]  Curve-Power 2
                                             CurveList[10] Curve-Power 3
                                             CurveList[11] Curve-Power 4
                                             CurveList[12] Curve-Power 5
                                             CurveList[13] Curve- NoLoad Power
                                             CurveList[14] Curve- CalibrationFlow
                                             CurveList[15] Curve- CalibrationPower
                                             */
                                            switch (item.AttributeID)
                                            {
                                                case 1: CurveList = new string[16]; continue;
                                                case 2: CurveList[0] = item.ValueString;continue;//curve code
                                                case 3:  continue;// curve status
                                                case 4: CurveList[13] =(Convert.ToDecimal( item.ValueString)/1000).ToString(); continue;// no load active power
                                                case 5: continue; // no load rective power
                                                case 6:// flow points
                                                    if (item.Level == 2)
                                                    {
                                                        if (curveIndex>6) continue;
                                                        CurveList[curveIndex++] = (Convert.ToDecimal(item.ValueString) / 1000).ToString(); continue;
                                                    }
                                                    else
                                                    { continue; }
                                                    
                                                case 7://acrive power points
                                                    if (item.Level == 2)
                                                    {
                                                        if (curveIndex > 12) continue;
                                                        CurveList[curveIndex++] = (Convert.ToDecimal(item.ValueString) / 1000).ToString(); continue;
                                                    }
                                                    else
                                                    { continue; }
                                                case 8: // reactive power info
                                                    //CurveList[0] = item.ValueString;
                                                    continue;
                                                case 9: CurveList[15] = (Convert.ToDecimal(item.ValueString) / 1000).ToString(); continue;//calibration power
                                                case 10: CurveList[14] = (Convert.ToDecimal(item.ValueString) / 1000).ToString();
                                                    bool fillCurveInfo = true;
                                                    foreach (var curve in CurveList)
                                                    {
                                                        if (string.IsNullOrEmpty(curve))
                                                        {
                                                            fillCurveInfo = false;
                                                        }
                                                    }
                                                    if (fillCurveInfo)
                                                    {
                                                        var Result1 = new ObjectParameter("Result", 10000000000000000); 
                                                        var ErrMSG1 = new ObjectParameter("ErrMSG", "");
                                                        SQLSPS.InsCurve(Convert.ToDecimal(meterId), Convert.ToDecimal(OBISValueHeaderID.Value), CurveList[0], CurveList[1], CurveList[2], CurveList[3], CurveList[4]
                                                                               , CurveList[5], CurveList[6], CurveList[7], CurveList[8], CurveList[9], CurveList[10], CurveList[11],
                                                                               CurveList[12], CurveList[13], CurveList[14], CurveList[15], Result1, ErrMSG1);
                                                    }

                                                    continue;// calibrataion flow
                                                default:
                                                    break;
                                            }

                                            continue;
                                        }

                                        if (item.AttributeID != 2)
                                        {
                                            continue;
                                        }
                                        var valueString = CommonData.ConvertValue(item);

                                        var name = Card.Classes.CardDataList.GetObjectName(item.LogicalName);
//                                        System.IO.File.AppendAllText(@"d:\obisstring.txt", item.LogicalName + "	" + name+"\r\n");
                                        var FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                                        var  Result = new ObjectParameter("Result", 10000000000000000);
                                        var  OBISID = new ObjectParameter("OBISID", 10000000000000000);
                                        var  ErrMSG = new ObjectParameter("ErrMSG", "");
                                        var  ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", 1000000000000000);
                                        var  ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);
                                        SQLSPS.INSOBISs("", item.LogicalName, "", "", "", 1, "", null, "", 1, "", "", "", FixedOBISCode, ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMSG);
 
                                        if (FixedOBISCode.Value.ToString() != "")
                                        {
                                             
                                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 900 && Convert.ToInt32(FixedOBISCode.Value.ToString()) < 925)
                                            {
                                                SaveConsumedActiveEnergy(clockValue, meterId, FixedOBISCode, OBISID, OBISValueHeaderID,
                                                    valueString);
                                            }
                                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 800 && Convert.ToInt32(FixedOBISCode.Value.ToString()) < 825)
                                            {
                                               //SaveConsumedWater(meterNo, clockValue, meterId, softwareVersion, FixedOBISCode, OBISID, OBISValueHeaderID, valueString,
                                               //      objectList[j].dateTime, objectList[j].code);
                                            }
                                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 825 && Convert.ToInt32(FixedOBISCode.Value.ToString()) < 863)
                                            {
                                              //  SaveConsumedWater(meterNo, clockValue, CommonData.SelectedMeterID, softwareVersion, FixedOBISCode, OBISID, OBISValueHeaderID, 
                                               //     objectList[j].value.ToString(), objectList[j].dateTime, objectList[j].code);
                                            }
                                        }
                                        Result = new ObjectParameter("Result", 10000000000000000);
                                       var ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                                        ErrMSG = new ObjectParameter("ErrMSG", "");
                                        SQLSPS.InsobisValueDetail(Convert.ToDecimal(OBISValueHeaderID.Value), softwareVersion, Convert.ToDecimal(OBISID.Value), valueString, null, "", ObisValueID, Result, ErrMSG);
                                        if (CommonData.mainwindow != null)
                                        {
                                            CommonData.mainwindow.changeProgressBarTag("ذخیره ...");
                                            
                                        }
                                    }


                                    #endregion
                                    // CommonData.mainwindow.changeProgressBarValue(1);
                                }
                                 
                            }

                            CommonData.counter = i;

                            changeProgressBarValue(i);
                        }
                        ////
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        VeeListoFMeterFromHHU();
                        string fltr = "";
                        if (SelectedGroup.GroupID != -1)
                            fltr = "and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + SelectedGroup.GroupID + "and  GroupType=" + SelectedGroup.GroupType + ") ";

                        RefreshSelectedMeters(fltr);
                         
                         
                         
                    }
                }

                if (hhureadout.hhureadout == null)
                {
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message33);
                    ReadFromHHU = true;
                    CommonData.IsCompleteReadOut = true;
                    return;
                }
                if (CommonData.mainwindow != null && hhureadout.hhureadout != null)
                {

                    CommonData.mainwindow.changeProgressBarValue(1000);
                    Thread.Sleep(1000);
                    CommonData.mainwindow.changeProgressBarTag("اطلاعات با موفقیت ذخیره شد");
                }
                 
                else
                {
                    MessageBox.Show("لطفا HHU  را به سیستم متصل نمایید");
                }

            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag("");
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        
        public void getDataFromHHU(HHUReadOut hhureadout)
        {
            try
            {
                MeterList = null;
                bool read207 = false;
                if (hhureadout != null)
                {
                    if (hhureadout.ErrorCode == "-1")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message33);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "-2")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message72);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "-6")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message73);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "-4")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message74);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "-5")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message86);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "-3")
                    {
                        CommonData.mainwindow.changeProgressBarTag("خطا در تخلیه اطلاعات ");
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "0" && hhureadout.ReadOutList != null)
                    {

                        var stringwriter = new StringWriter();
                        XmlSerializer myserializer = new XmlSerializer(hhureadout.ReadOutList.GetType());
                        myserializer.Serialize(stringwriter, hhureadout.ReadOutList);
                        string xmlMessage = stringwriter.ToString();

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(xmlMessage);
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        // Save the document to a file and auto-indent the output.
                        XmlWriter writer = XmlWriter.Create("data.xml", settings);
                        doc.Save(writer);
                        writer.Close();
                        string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\data.xml";
                        //SQLSPS.InsReadValueFromHHUXML(path, CommonData.UserID, CommonData.LanguagesID, Result, ErrMSG);
                        //}
                        //if (hhureadout != null)
                        if (HandHeldReader.HandHeldReader.FindHhuReadOut)
                        {
                            if(hhureadout.ReadOutList.Count>100)
                                changeProgressBarValue(hhureadout.ReadOutList.Count);
                            changeProgressBar_MaximumValue(hhureadout.ReadOutList.Count * 35);
                          
                            for (int i = 0; i < hhureadout.ReadOutList.Count; i+=1)
                            {
                                SaveHhu207ReadOutData(hhureadout.ReadOutList[i]);
                                //Thread th = new Thread(delegate()
                                //    {
                                //        SaveHHU207ReadOutData(hhureadout.ReadOutList[i]);
                                //    });
                                //    //th.SetApartmentState(ApartmentState.STA);
                                //    th.IsBackground = true;
                                //    th.Start();


                            
                                CommonData.counter = i;
                                    changeProgressBarValue(10);
                                    //changeProgressBarTag(hhureadout.ReadOutList[i].SerialNumber +"    "+ CommonData.mainwindow.tm.TranslateofMessage.Message98);
                                    changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message98);

                            }
                            ReadFromHHU = true;
                            CommonData.IsCompleteReadOut = true;
                          
                            VeeListoFMeterFromHHU();
                          
                            string fltr = "";
                            if (SelectedGroup.GroupID != -1)
                                fltr = "and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + SelectedGroup.GroupID + "and  GroupType=" + SelectedGroup.GroupType + ") ";

                            RefreshSelectedMeters(fltr);
                            changeProgressBarTag("");
                            changeProgressBarValue(0);

                            //ShowHHuForm();

                        }

                    }
                }
                if (hhureadout.ReadOutList == null)
                {
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message33);
                    ReadFromHHU = true;
                    CommonData.IsCompleteReadOut = true;
                    return;
                }
                if (CommonData.mainwindow != null && hhureadout.ReadOutList != null)
                {

                    CommonData.mainwindow.changeProgressBarValue(hhureadout.ReadOutList.Count * 2);
                    CommonData.mainwindow.changeProgressBarTag("");
                    //CommonData.carddataReceive.refreshs();
                }
            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag("");
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void getDataFromHHUFile(HHUReadOut hhureadout)
        {
            try
            {
                bool read207 = false;
                if (hhureadout != null)
                {
                    if (hhureadout.ErrorCode == "-1")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message33);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "-2")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message72);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "-6")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message73);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "-4")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message74);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "-5")
                    {
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message86);
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "-3")
                    {
                        CommonData.mainwindow.changeProgressBarTag("خطا در تخلیه اطلاعات ");
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;
                        return;
                    }
                    else if (hhureadout.ErrorCode == "0" && hhureadout.ReadOutList != null)
                    {
                        if (hhureadout.ReadOutList.Count > 100)
                            changeProgressBarValue(hhureadout.ReadOutList.Count);

                        changeProgressBar_MaximumValue(hhureadout.ReadOutList.Count * 35);
                        MeterList = null;
                        for (int i = 0; i < hhureadout.ReadOutList.Count; i += 1)
                        {
                            SaveHhu207ReadOutData(hhureadout.ReadOutList[i]); 

                            CommonData.counter = i;
                            changeProgressBarValue(10);
                            //changeProgressBarTag(hhureadout.ReadOutList[i].SerialNumber +"    "+ CommonData.mainwindow.tm.TranslateofMessage.Message98);
                            changeProgressBarTag("ذخیره سازی اطلاعات ... ");

                        }
                        ReadFromHHU = true;
                        CommonData.IsCompleteReadOut = true;

                        VeeListoFMeterFromHHU();

                        string fltr = "";
                        if (SelectedGroup.GroupID != -1)
                            fltr = "and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + SelectedGroup.GroupID + "and  GroupType=" + SelectedGroup.GroupType + ") ";

                        RefreshSelectedMeters(fltr);
                         
                        

                    }
                }
                if (hhureadout.ReadOutList == null)
                {
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message33);
                    ReadFromHHU = true;
                    CommonData.IsCompleteReadOut = true;
                    return;
                }
                if (CommonData.mainwindow != null && hhureadout.ReadOutList != null && hhureadout.ReadOutList.Count > 0)
                {
                    Thread.Sleep(1000);
                    CommonData.mainwindow.changeProgressBarValue(1000);
                    Thread.Sleep(1000);
                    CommonData.mainwindow.changeProgressBarTag("اطلاعات با موفقیت ذخیره شد");
                    

                }
            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag("");
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        private void SaveHhu207ReadOutData(ReadOut readout)
        {
            if (!readout.SerialNumber.StartsWith("207"))
                return;
            decimal meterId = AddMeterToList(readout.SerialNumber,
                readout.ReadOutDateTime,
                readout.SoftwareVersion, out var customerId);
            SaveINDBReadValueFromHHU(readout.SerialNumber, meterId, readout.ReadOutDateTime,
                readout.SoftwareVersion,
                readout.OBISObjectList,customerId);
        }

        


        public decimal AddMeterToList(string serialNumber, string readOutDateTime, string softwareVersion,out decimal? customerId)
        {
            customerId = -1;
            try
            {
                //SabaNewEntities
               // Bank = new SabaNewEntities();
                //Bank.Database.Connection.Open();
                ObjectParameter result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter errMsg = new ObjectParameter("ErrMSG", "");
                ObjectParameter meterId = new ObjectParameter("MeterID", 100000000000000);
                SelectedMeter newmeter = new SelectedMeter();
               
                SQLSPS.InsMeter(serialNumber, null, true, softwareVersion, null, null, customerId, SelectedGroup.GroupID, SelectedGroup.GroupType, true, meterId, errMsg, result);
                
                newmeter.MeterNumber = serialNumber;
                ShowMeter_Result MeterInfo = SQLSPS.ShowMeter(" and Main.Valid=1 and Main.MeterNumber='" + serialNumber + "'");
                customerId = MeterInfo.CustomerID;
                meterId .Value= MeterInfo.MeterID;
                newmeter.ReadDate = readOutDateTime;
                if (CommonData.LanguagesID== 2)
                    newmeter.ReadDate  =PersianDateTime.ConvertToPersianDateTime(readOutDateTime);
                
                newmeter.MeterId = Convert.ToDecimal(meterId.Value);
                if (CommonData.AccessUserToDefultGroup)
                { bool IsExistsInList = false;
                    if (MeterList == null)
                        MeterList = new List<SelectedMeter>();
                    foreach (var item in MeterList)
                    {
                        if (item.MeterNumber == serialNumber) { IsExistsInList = true; break; }
                    }
                    if(!IsExistsInList)
                        MeterList.Add(newmeter);
                }
                //Bank.Database.Connection.Close();
                return Convert.ToDecimal(meterId.Value);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public void ShowHHuForm()
        {
            try
            {
                //while (!ReadFromHHU) ;
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                               new Action(
                                   delegate
                                   {
                                       if (CommonData.IsCompleteReadOut && MeterList != null)
                                       {
                                           HHUInfoReceive getdatafromhhu = new HHUInfoReceive(MeterList);
                                           getdatafromhhu.Title = CommonData.mainwindow.translateWindowName.TranslateofLable.Object19;
                                           getdatafromhhu.Owner = this;
                                           getdatafromhhu.Tab = CommonData.mainwindow.tabControl1;
                                           ClosableTab theTabItem = new ClosableTab();
                                           theTabItem.Title = getdatafromhhu.Title;
                                           theTabItem.windowname = getdatafromhhu;
                                           AddTabItemsssss(theTabItem);
                                           theTabItem.Focus();
                                           getdatafromhhu.TabPag = theTabItem;
                                           getdatafromhhu.Show();

                                       }
                                   }
                ));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void AddTabItemsssss(object theTabItem)
        {
            try
            {
                tabControl1.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
               delegate ()
               {
                   tabControl1.Items.Add(theTabItem);
               }));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        private void hhu(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 19);
                if (us.CanShow)
                {
                    if (CommonData.HHUDataReceive != null)
                    {
                        CommonData.HHUDataReceive.Close();
                    }
                    ReadFromHHU = false;
                    changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);
                    ReadHHU();

                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void Hhu303(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 19);
                if (us.CanShow)
                {
                    if (CommonData.HHUDataReceive != null)
                    {
                        CommonData.HHUDataReceive.Close();
                    }
                    ReadFromHHU = false;
                    ReadHhu303();

                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void Excel(object sender, RoutedEventArgs e)
        {

            us = CommonData.ShowButtonBinding("", 16);
            if (us.CanShow)
            {
                if (!ClassControl.OpenWin[16])
                {
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(0);
                    ClassControl.OpenWin[16] = true;
                    SelectDataTypeForExportToExcel exportToExcel = new SelectDataTypeForExportToExcel();
                    exportToExcel.Title = translateWindowName.TranslateofLable.Object16;
                    exportToExcel.Owner = this;
                    exportToExcel.Tab = tabControl1;
                    ClosableTab theTabItem = new ClosableTab();
                    theTabItem.Title = exportToExcel.Title;
                    theTabItem.windowname = exportToExcel;
                    tabControl1.Items.Add(theTabItem);
                    theTabItem.Focus();
                    exportToExcel.TabPag = theTabItem;
                    exportToExcel.Show();
                }
            }
            else
            {
                MessageBox.Show(tm.TranslateofMessage.Message55);
                return;
            }
        }


        private void SABAExit(object sender, RoutedEventArgs e)
        {
        }

        public void JoinInGroup(object sender, RoutedEventArgs e)
        {

            us = CommonData.ShowButtonBinding("", 8);
            if (us.CanShow)
            {
                if (!ClassControl.OpenWin[8])
                {
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(0);
                    ClassControl.OpenWin[8] = true;
                    UI.UserToGroup usertogroup = new UI.UserToGroup();
                    usertogroup.Title = translateWindowName.TranslateofLable.Object8;
                    usertogroup.Owner = this;
                    usertogroup.Tab = tabControl1;
                    ClosableTab theTabItem = new ClosableTab();
                    theTabItem.Title = usertogroup.Title;
                    theTabItem.windowname = usertogroup;
                    tabControl1.Items.Add(theTabItem);
                    theTabItem.Focus();
                    usertogroup.TabPag = theTabItem;
                    usertogroup.Show();
                    usertogroup.Refresh();
                }
            }
            else
            {
                MessageBox.Show(tm.TranslateofMessage.Message55);
                return;
            }
        }

        private void NewGroup(object sender, RoutedEventArgs e)
        {

            us = CommonData.ShowButtonBinding("", 10);
            if (us.CanShow)
            {
                if (!ClassControl.OpenWin[10])
                {
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(0);
                    ClassControl.OpenWin[10] = true;
                    NewGroup G2 = new NewGroup();
                    G2.Title = translateWindowName.TranslateofLable.Object10;
                    G2.Owner = this;
                    G2.Tab = tabControl1;
                    ClosableTab theTabItem = new ClosableTab();
                    theTabItem.Title = G2.Title;
                    theTabItem.windowname = G2;
                    tabControl1.Items.Add(theTabItem);
                    theTabItem.Focus();
                    G2.TabPag = theTabItem;
                    G2.Show();
                }
            }
            else
            {
                MessageBox.Show(tm.TranslateofMessage.Message55);
                return;
            }
        }

        private void OmitGroup(object sender, RoutedEventArgs e)
        {

        }

        private void EditGroup(object sender, RoutedEventArgs e)
        {

        }

        private void SetGroup(object sender, RoutedEventArgs e)
        {

        }

        private void ManageUsers(object sender, RoutedEventArgs e)
        {
            if (!ClassControl.OpenWin[8])
            {
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
                ClassControl.OpenWin[8] = true;
                SetPermissionsforUser permission = new SetPermissionsforUser();
                permission.Owner = this;
                permission.Tab = tabControl1;
                ClosableTab theTabItem = new ClosableTab();
                theTabItem.Title = permission.Title;
                theTabItem.windowname = permission;
                tabControl1.Items.Add(theTabItem);
                theTabItem.Focus();
                permission.TabPag = theTabItem;
                permission.Show();
            }
        }

        private void SetShowInfo(object sender, RoutedEventArgs e)
        {
            if (!ClassControl.OpenWin[6])
            {
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
                ClassControl.OpenWin[6] = true;
                InfoShowRegulation T2 = new InfoShowRegulation();
                T2.Owner = this;
                T2.Tab = tabControl1;
                T2.Title = translateWindowName.TranslateofLable.Object6;
                ClosableTab theTabItem = new ClosableTab();
                theTabItem.Title = T2.Title;
                theTabItem.windowname = T2;
                tabControl1.Items.Add(theTabItem);
                theTabItem.Focus();
                T2.TabPag = theTabItem;
                T2.Show();
            }
        }

        private void PishF(object sender, RoutedEventArgs e)
        {

        }

        private void SecurityCode(object sender, RoutedEventArgs e)
        {

        }

        public void ReadCard(int type)
        {
            //Thread th = new Thread(getDataFromCard(type));
            Thread th = new Thread(delegate () { GetDataFromCard(type); });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        public void GetDataFromCard(int type)
        {
            try
            {
                bool IS207 = false;
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag("");
                changeProgressBarValue(0);
                changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);
                CardManager cm = new CardManager();
                CardReadOut cr = cm.getCardData();

                //XmlSerializer deserializer = new XmlSerializer(typeof(Card.CardReadOut));
                //TextReader reader = new StreamReader(@"D:\SABA\V1.0.5.6\SABA_CH\SABA_CH\bin\Release\test.xml");
                //object obj = deserializer.Deserialize(reader);
                //Card.CardReadOut XmlData = (Card.CardReadOut)obj;
                //cr = XmlData;

                ////new
                //var stringwriter = new System.IO.StringWriter();
                //XmlSerializer myserializer = new XmlSerializer(cr.GetType());
                //myserializer.Serialize(stringwriter,cr);
                //string xmlMessage = stringwriter.ToString();

                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(xmlMessage);

                //XmlWriterSettings settings = new XmlWriterSettings();
                //settings.Indent = true;
                //// Save the document to a file and auto-indent the output.
                //XmlWriter writer = XmlWriter.Create("carddata.xml", settings);
                //doc.Save(writer);
                //writer.Close();
                ////new
                if (cr.ErrorMessage.MeterErrorMessage.Count == 0)
                    if (cr.ErrorMessage.CardErrorMessage.Count == 0)
                    {
                        changeProgressBarValue(0);
                        changeProgressBarTag("");
                        ulong city = Convert.ToUInt64(cr.Card_CityCode);
                        string carccitycode = city.ToString("X8");

                        if (!EvaluationOfCityCardCode(carccitycode))
                        {
                            CommonData.getDataFromCard = false;
                            return;
                        }
                        CommonData.MeterNumber = cr.SerialNumber;
                        CommonData.CardMeterNumber = cr.SerialNumber;
                        CommonData.SelectedMeterNumber = cr.SerialNumber;
                        CommonData.SelectedMeterNumber = cr.SerialNumber;
                        CommonData.SoftwareVersion = cr.SoftwareVersion;
                        //if (cr.SerialNumber.StartsWith("207"))
                        //    CommonData.ClockValue = cr.ReadOutDateTime.Substring(0, cr.ReadOutDateTime.IndexOf(" "));
                        //else
                        CommonData.ClockValue = cr.ReadOutDateTime;
                        CommonData.CardNumber = cr.CardNumber;
                        if (CommonData.MeterNumber.StartsWith("207"))
                            IS207 = true;
                        int len = CommonData.MeterNumber.Length;
                        if (len > 6)
                            if (CommonData.MeterNumber.Substring(len - 6, 6) == "000000")
                            {
                                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message52);
                                return;
                            }


                        
                        int m = SaveINDBReadValueFromCard(CommonData.CardMeterNumber, CommonData.ClockValue, CommonData.SoftwareVersion, cr.CardData.ObjectList, IS207, 1000, cr.CardNumber, cr.SoftwareVersion);
                        ReadFromCard = true;
                        
                        CommonData.IsCompleteReadOut = true;
                        if (m == 0)
                        {
                            ShowCardDate(type);
                            string Filter = "";
                            if (SelectedGroup.GroupID != -1)
                                Filter = "and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + SelectedGroup.GroupID + "and  GroupType=" + SelectedGroup.GroupType + ") ";
                            RefreshSelectedMeters(Filter);

                        }
                    }
                    else
                    {

                        //CommonData.mainwindow.changeProgressBarTag(cr.ErrorMessage.MeterErrorMessage[cr.ErrorMessage.MeterErrorMessage.Count - 1].message);
                        CommonData.mainwindow.changeProgressBarTag(cr.ErrorMessage.CardErrorMessage[cr.ErrorMessage.CardErrorMessage.Count - 1].message);
                        ReadFromCard = true;
                    }
                else
                {
                    
                    CommonData.mainwindow.changeProgressBarTag(cr.ErrorMessage.MeterErrorMessage[cr.ErrorMessage.MeterErrorMessage.Count - 1].message);
                    Thread.Sleep(50);
                    GetProgressBarBackGround(true);
                    ReadFromCard = true;
                }
            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag("");
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        bool EvaluationOfCityCardCode(string cityCode)
        {

            CityCardCodeStatus status = CheckCityCardCode(cityCode);
            CardManager cardManager = new CardManager();
            switch (status)
            {
                case CityCardCodeStatus.InvalidProvinceCode:
                    #region InvalidProvinceCode
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message42);
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message42);
                    CommonData.mainwindow.changeProgressBarValue(0);
                    CommonData.getDataFromCard = false;
                    return false;
                #endregion InvalidProvinceCode

                case CityCardCodeStatus.InvalidCityCode:
                    #region InvalidCityCode
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message43);
                    CommonData.mainwindow.changeProgressBarValue(0);
                    CommonData.getDataFromCard = false;
                    return false;
                #endregion InvalidCityCode

                case CityCardCodeStatus.Error:
                    break;

                case CityCardCodeStatus.UnKownCity:
                    #region UnKownCity


                    if (!CommonData.Citycode.EndsWith("00FF"))
                    {
                        string message = CommonData.mainwindow.tm.TranslateofMessage.Message44 + "\r\n";
                        message += CommonData.mainwindow.tm.TranslateofMessage.Message46 + "\r\n";
                        message += "OK" + "\r\n";
                        message += CommonData.mainwindow.tm.TranslateofMessage.Message47;
                        if (MessageBox.Show(message, CommonData.mainwindow.tm.TranslateofMessage.Message45, MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign) == MessageBoxResult.OK)
                        {

                            CardManager cm = new CardManager();
                            cm.WriteCityCodeOncard(CommonData.Citycode);
                            return true;
                        }
                    }
                    else
                    {
                        string message = CommonData.mainwindow.tm.TranslateofMessage.Message67 + "\r\n";
                        message += CommonData.mainwindow.tm.TranslateofMessage.Message48;
                        message += CommonData.mainwindow.tm.TranslateofMessage.Message47;
                        if (MessageBox.Show(message, CommonData.mainwindow.tm.TranslateofMessage.Message49, MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign) == MessageBoxResult.OK)
                        {
                            CardManager cm = new CardManager();
                            cm.WriteCityCodeOncard(CommonData.Citycode);
                            return true;
                        }
                    }
                    CommonData.mainwindow.changeProgressBarValue(0);
                    break;
                #endregion UnKownCity

                case CityCardCodeStatus.Success:
                    return true;
            }

            return false;
        }

        private CityCardCodeStatus CheckCityCardCode(string cityCode)
        {
            try
            {
                if (cityCode.Equals(CommonData.Citycode))
                    return CityCardCodeStatus.Success;

                else if (cityCode.Equals("00000000"))
                    return CityCardCodeStatus.UnKownCity;

                else if (!cityCode.Substring(0, 4).Equals(CommonData.Citycode.Substring(0, 4)))
                    return CityCardCodeStatus.InvalidProvinceCode;

                if (CommonData.Citycode.Substring(4, 4).Equals("00FF"))
                    return CityCardCodeStatus.Success;

                else if (cityCode.Substring(4, 4).Equals("00FF"))
                    return CityCardCodeStatus.UnKownCity;
                else
                    return CityCardCodeStatus.InvalidCityCode;
            }
            catch (Exception EX)
            {
                // MessageBox.Show(ex.ToString(), "CheckCityCardCode");
                return CityCardCodeStatus.Error;
            }
        }
        List<ShowVeeConsumedWater_Result> waterConsumptionList;
        public int SaveINDBReadValueFromCard(string meterNo, string clockValue,string softwareVersion, List<Card.OBISObject> objectList, bool read207, int max, string CardNumber, string Sofversion)
        {
            ObjectParameter OBISValueHeaderID = new ObjectParameter("OBISValueHeaderID", 1000000000000000);
            try
            {
                bool IsDuplicateData = false;
                ObjectParameter FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                ObjectParameter ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", "");
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter OBISID = new ObjectParameter("OBISID", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                ObjectParameter IsExits = new ObjectParameter("IsExits", false);
                ObjectParameter ObisValueID = new ObjectParameter("ObisValueID", 1000000000000000);
                ObjectParameter ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);
                ObjectParameter MeterID = new ObjectParameter("MeterID", 100000000000000);
                CommonData.OBISValueHeaderID = Convert.ToDecimal(OBISValueHeaderID.Value);

                string totalWater = "";
                string pumpWorkingTime = "";
                string currentConsumption = "";

                SQLSPS.ShowMeterNumber(CommonData.CardMeterNumber, IsExits, Result, ErrMSG);
                decimal? MeterSoftversionToDeviceModelId = 1000000;

                if (CommonData.correctReading)
                {
                    bool isdirect = false;
                    if (meterNo.StartsWith("193"))
                        isdirect = true;
                    if (meterNo.StartsWith("207"))
                    {
                        MeterSoftversionToDeviceModelId = 2;
                    }
                    else
                    {
                        switch (softwareVersion)
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
                        };
                    }

                    decimal? Customerid = -1;
                    if (Convert.ToBoolean(IsExits.Value))
                    {
                        ShowMeter_Result MeterInfo = SQLSPS.ShowMeter(" and Main.Valid=1 and Main.MeterNumber='" + CommonData.CardMeterNumber + "'");
                        Customerid = MeterInfo.CustomerID;
                        CommonData.CardMeterID = MeterInfo.MeterID;
                        CommonData.MeterID = MeterInfo.MeterID;
                        CommonData.CardCustomerId = MeterInfo.CustomerID;
                        SQLSPS.UpdMeter(meterNo, MeterInfo.DeviceModelID, isdirect, MeterSoftversionToDeviceModelId, MeterInfo.ModemID, MeterInfo.CustomerID, true, MeterInfo.MeterID, ErrMSG, Result);

                    }                   
                    else
                        Customerid = null;
                    
                    //if (Convert.ToBoolean(IsExits.Value) == true  && CommonData.UserID != 1)
                    //{
                    //    MessageBox.Show("شما مجاز به دیدن اطلاعات این کنتور نیستید");
                    //    return 9;
                    //}

                    if (Customerid < 1 || Customerid == null)
                    {
                        SQLSPS.ShowMeterNumber(CommonData.CardMeterNumber, IsExits, Result, ErrMSG);
                        if (Convert.ToBoolean(IsExits.Value))
                        {
                            ShowMeter_Result MeterInfo = SQLSPS.ShowMeter(" and Main.Valid=1 and Main.MeterNumber='" + CommonData.CardMeterNumber + "'");
                            Customerid = MeterInfo.CustomerID;
                            CommonData.CardMeterID = MeterInfo.MeterID;
                            CommonData.CardCustomerId = MeterInfo.CustomerID;
                            SQLSPS.UpdMeter(meterNo, MeterInfo.DeviceModelID, isdirect, MeterSoftversionToDeviceModelId, MeterInfo.ModemID, MeterInfo.CustomerID, true, MeterInfo.MeterID, ErrMSG, Result);

                        }
                        else
                        {
                            SQLSPS.InsMeter(CommonData.CardMeterNumber, null, true, Sofversion, null, null, Customerid, CommonData.selectedGroup.GroupID, CommonData.selectedGroup.GroupType, true, MeterID, ErrMSG, Result);
                            CommonData.CardMeterID = Convert.ToDecimal(MeterID.Value);

                            if (Customerid == null || Customerid < 1)
                            {
                                MessageBoxResult MBResult = MessageBox.Show("لطفا اطلاعات مشترک کنتور قرائت شده را وارد کنید و مجدداد دریافت  اطلاعات از کارت را بزنید", "", MessageBoxButton.OKCancel);
                                if (MBResult == MessageBoxResult.OK)
                                {
                                    CommonData.mainwindow.ShowMeters(1, meterNo, true);
                                }

                                return -1;
                            }
                        }
                    }


                    // دریافت بازه خاموشی از کاربر
                    CommonData.MeterShutdownStartDate = null;
                    CommonData.MeterShutdownEndDate = null;
                    CommonData.MeterShutdownMonths = 0;
                    CommonData.WasTheMeterOff = null;
                    //if(CommonData.GetShutdownInterval && CommonData.CardMeterNumber .StartsWith("207"))
                    //{
                    //    MeterShutdownInterval meterInterval = new MeterShutdownInterval();
                    //    meterInterval.ShowDialog();
                    //}

                    

                   
                    if (CommonData.LanguagesID == 2)
                    {
                        CommonData.ClockValue =PersianDateTime.ConvertToPersianDateTime(CommonData.ClockValue);
                        clockValue = CommonData.ClockValue;
                    }
                    if (!Convert.ToBoolean(SQLSPS.ShowClockObisValue(CommonData.ClockValue, (CommonData.CardMeterID))))
                    {

                        Result = new ObjectParameter("Result", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        OBISValueHeaderID = new ObjectParameter("OBISValueHeaderID", 1000000000000000);
                        string transferDate = "";
                        if (CommonData.LanguagesID == 2)
                        {
                            transferDate = DateTime.Now.ToPersianString();

                        }
                        else
                            transferDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Substring(0, 10);
                        SQLSPS.InsobisValueHeader(clockValue, 1, CommonData.UserID, transferDate, Convert.ToDecimal(CommonData.CardMeterID), 2, OBISValueHeaderID, Result, ErrMSG);
                        Meter_Consumption_Data303.ConsumedWaterdata = new List<VEE.VeeConsumedWater>();
                       CurveList =new string[16];
                         waterConsumptionList = new List<ShowVeeConsumedWater_Result>();
                        for (int j = 0; j < objectList.Count; j++)
                        {
                            //if (objectList[j].code.Contains("0000603E05FF") || objectList[j].code.Contains("0000603D02FF"))
                            //    transferDate = transferDate;
                            if (objectList[j].code.Contains("0802010000FF"))
                            {
                                totalWater = objectList[j].value;
                            }
                            if (objectList[j].code.Contains("0802010100FF"))
                            {
                                currentConsumption = objectList[j].value;
                            }
                            if (objectList[j].code.Contains("0802606202FF"))
                            {
                                pumpWorkingTime = objectList[j].value;
                            }
                            if (objectList[j].code.ToLower().Contains("0802802800ff_"))//"08026062"))
                            {
                                SaveInCurveTable(objectList[j].code, objectList[j].value, CommonData.CardMeterID.ToString(), OBISValueHeaderID.Value.ToString());
                             }
                            
                            Result = new ObjectParameter("Result", 10000000000000000);
                            OBISID = new ObjectParameter("OBISID", 10000000000000000);
                            ErrMSG = new ObjectParameter("ErrMSG", "");
                            ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", 1000000000000000);
                            ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);
                            FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                            SQLSPS.INSOBISs("", objectList[j].code, "", "", "", 1, "", null, "", 1, "", "", "", FixedOBISCode, ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMSG);

                            
                            if (FixedOBISCode.Value.ToString() != "")
                            {                               
                                if (Convert.ToInt32(FixedOBISCode.Value.ToString()) == 500 && CommonData.LanguagesID == 2)
                                {
                                    objectList[j].value = SaveDateValue(objectList[j].value); 
                                }
                                if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 900 && Convert.ToInt32(FixedOBISCode.Value.ToString()) < 925)
                                {
                                    SaveConsumedActiveEnergy(clockValue,CommonData.CardMeterID, FixedOBISCode, OBISID, OBISValueHeaderID,
                                        objectList[j].value.ToString());
                                }
                                if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 800 && Convert.ToInt32(FixedOBISCode.Value.ToString()) < 825)
                                {
                                    if (!CommonData.CardMeterNumber.StartsWith("207") && !string.IsNullOrEmpty(softwareVersion))
                                        if (!objectList[j].code.EndsWith("FF") && softwareVersion.EndsWith("4") && objectList[j].dateTime != "1921/03/21 00:00:00")
                                        {
                                            if (objectList[j].code.Contains("0802010100"))//آب مصرفی ماه
                                            {
                                                AddToVeeConsumedWater(CommonData.CardCustomerId, CommonData.CardMeterID, objectList[j].dateTime, objectList[j].value.ToString(), "", "");
                                            }
                                            else if (objectList[j].code.Contains("0802010000"))//آب مصرفی کل  ماه
                                            {
                                                AddToVeeConsumedWater(CommonData.CardCustomerId, CommonData.CardMeterID, objectList[j].dateTime, "", objectList[j].value.ToString(), "");

                                            }
                                            else if (objectList[j].code.Contains("0802606202"))//كاركرد الكتروپمپ ماه
                                            {
                                                AddToVeeConsumedWater(CommonData.CardCustomerId, CommonData.CardMeterID, objectList[j].dateTime, "", "", objectList[j].value.ToString());
                                            }

                                        }
                                    
                                    SaveConsumedWater(meterNo,clockValue, CommonData.CardMeterID, softwareVersion, FixedOBISCode , OBISID, OBISValueHeaderID, objectList[j].value.ToString(), objectList[j].dateTime, objectList[j].code,Customerid);
                                }
                                if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 825 && Convert.ToInt32(FixedOBISCode.Value.ToString()) < 863)
                                { 
                                    SaveConsumedWater(meterNo, clockValue, CommonData.CardMeterID, softwareVersion, FixedOBISCode, OBISID, OBISValueHeaderID, objectList[j].value.ToString(), objectList[j].dateTime, objectList[j].code, Customerid);
                                }
                            }
                            Result = new ObjectParameter("Result", 10000000000000000);
                            ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                            ErrMSG = new ObjectParameter("ErrMSG", "");

                            if (objectList[j].value.Contains("/") && !objectList[j].value.Contains(" ") && CommonData.CardMeterNumber.StartsWith("19"))
                            {
                                objectList[j].value=objectList[j].value.Replace("/", ".");
                            }
                            SQLSPS.InsobisValueDetail(Convert.ToDecimal(OBISValueHeaderID.Value), CommonData.SoftwareVersion, Convert.ToDecimal(OBISID.Value), objectList[j].value, null, "", ObisValueID, Result, ErrMSG);
                            if (CommonData.mainwindow != null)
                            {
                                CommonData.mainwindow.changeProgressBarTag("ذخیره ...");
                                CommonData.mainwindow.changeProgressBarValue(1);
                            }
                        }
                        Result = new ObjectParameter("Result", 10000000000000000);
                        OBISID = new ObjectParameter("OBISID", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", 1000000000000000);
                        ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);

                        SQLSPS.INSOBISs("", "0000", "", "", "", 1, "", null, "", 1, "", "", "", FixedOBISCode, ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMSG);

                        Result = new ObjectParameter("Result", 10000000000000000);
                        ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        SQLSPS.InsobisValueDetail(Convert.ToDecimal(OBISValueHeaderID.Value), CommonData.SoftwareVersion, Convert.ToDecimal(OBISID.Value), DateTime.Now.ToPersianString(), CommonData.ClockValue, "", ObisValueID, Result, ErrMSG);
                        InsertCard(CardNumber, OBISValueHeaderID);

                        Result = new ObjectParameter("Result", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        if (meterNo.StartsWith("207"))
                            SQLSPS.InsCurve(Convert.ToDecimal(CommonData.CardMeterID), Convert.ToDecimal(OBISValueHeaderID.Value), CurveList[0], CurveList[1], CurveList[2], CurveList[3], CurveList[4]
                       , CurveList[5], CurveList[6], CurveList[7], CurveList[8], CurveList[9], CurveList[10], CurveList[11],
                       CurveList[12], CurveList[13], CurveList[14], CurveList[15], Result, ErrMSG);

                        if (CommonData.CardMeterNumber.StartsWith("207"))
                        {
                            VeeMeterData vee = new VeeMeterData();
                            vee.Vee207data(CommonData.CardMeterID, CommonData.CardMeterNumber, CommonData.CardCustomerId);
                        }
                        else
                        {
                            if(softwareVersion.EndsWith("4"))
                            {
                                SaveVeeConsumedWater(CommonData.CardCustomerId);
                            }
                            else
                                Vee303(CommonData.CardMeterID, CommonData.CardMeterNumber, CommonData.CardCustomerId, waterConsumptionList, softwareVersion,totalWater, currentConsumption,pumpWorkingTime,clockValue);
                        }
                    }
                    else
                    {
                        //MessageBox.Show("شماره کنتور قبلا قرائت شده");
                        return 0;
                    }
                    if (CommonData.mainwindow != null)
                    {

                        CommonData.mainwindow.changeProgressBarValue(max);
                        CommonData.mainwindow.changeProgressBarTag("");
                        //CommonData.carddataReceive.refreshs();
                    }


                }
               
               
                CommonData.OBISValueHeaderID = Convert.ToDecimal(OBISValueHeaderID.Value);
                return 0;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                return -1;

            }
        }

        private void Vee303(decimal? meterID, string meterNumber, decimal customerId, List<ShowVeeConsumedWater_Result> waterConsumptionList, string softwareVersion, string totalWater, string currentConsumption, string pumpWorkingTime, string clockValue)
        {
            var veeData = new List<ShowVeeConsumedWater_Result>();
            ShowVeeConsumedWater value1 = new ShowVeeConsumedWater(meterID);
            string s1 = softwareVersion[softwareVersion.Length - 1] + "";

            VeeMeterData vee = new VeeMeterData();
            vee.Vee303data(CommonData.CardMeterID, CommonData.CardMeterNumber, CommonData.CardCustomerId, softwareVersion);
        }

        void SaveVeeConsumedWater(decimal customerId)
        {
            ShowVeeConsumedWater consumedWater1 = new ShowVeeConsumedWater(CommonData.CardMeterID);
            var preConsumption = new List<VEE.VeeConsumedWater>();
            foreach (var item in consumedWater1._lstShowVeeConsumedWater)
            {
                preConsumption.Add
                    (new VEE.VeeConsumedWater()
                    {
                        ConsumedDate = item.ConsumedDate,
                        Flow = item.Flow,
                        IsValid = item.IsValid,
                        MeterId = item.MeterId,
                        CustomerId = item.CustomerId,
                        MonthlyConsumption = item.MonthlyConsumption,
                        TotalConsumption = item.TotalConsumption
                    });
            }
            if (preConsumption.Count > 0)
            {

                if (Meter_Consumption_Data303.ConsumedWaterdata[0].ConsumedDate.CompareTo(preConsumption[0].ConsumedDate) <= 0)
                {
                    return;
                }
            }
            else
            {
                if (Meter_Consumption_Data303.ConsumedWaterdata.Count>0)
                    Meter_Consumption_Data303.ConsumedWaterdata.Last().MonthlyConsumption = Meter_Consumption_Data303.ConsumedWaterdata.Last().TotalConsumption;

            }


            if (Meter_Consumption_Data303.ConsumedWaterdata.Count > 0)
            {
                //SaveToExcell(Meter_Consumption_Data303.ConsumedWaterdata, meterNo);
                try
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("CustomerId");
                    dt.Columns.Add("MeterId");
                    dt.Columns.Add("ConsumedDate");
                    dt.Columns.Add("Flow");
                    dt.Columns.Add("MonthlyConsumption");
                    dt.Columns.Add("TotalConsumption");
                    dt.Columns.Add("IsValid");
                    dt.Columns.Add("PumpWorkingHour");
                    for (int i = 0; i < Meter_Consumption_Data303.ConsumedWaterdata.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["CustomerId"] = customerId;
                        dr["MeterId"] = CommonData.CardMeterID;
                        dr["ConsumedDate"] = Meter_Consumption_Data303.ConsumedWaterdata[i].ConsumedDate;
                        dr["Flow"] = "";
                        dr["MonthlyConsumption"] =ReplaceSlash (Meter_Consumption_Data303.ConsumedWaterdata[i].MonthlyConsumption);
                        dr["TotalConsumption"] = ReplaceSlash(Meter_Consumption_Data303.ConsumedWaterdata[i].TotalConsumption);
                        dr["IsValid"] = true;
                        if (Meter_Consumption_Data303.ConsumedWaterdata[i].Flow == "-1" || string.IsNullOrEmpty(Meter_Consumption_Data303.ConsumedWaterdata[i].Flow))
                                dr["PumpWorkingHour"] = "NA";
                        else
                            dr["PumpWorkingHour"] = ReplaceSlash(Meter_Consumption_Data303.ConsumedWaterdata[i].Flow);
                        dt.Rows.Add(dr);
                    }

                    SQLSPS.UPDVeeConsumedWater(dt);

                    //foreach (Process proc in Process.GetProcesses())
                    //    if (proc.ProcessName.ToUpper().Equals("EXCEL") && Process.GetbaProcess().Id != proc.Id)
                    //    {
                    //        //System.Windows.Forms.MessageBox.Show(proc.ProcessName.ToUpper());
                    //        proc.Kill();
                    //    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        
        void AddToVeeConsumedWater(Nullable<decimal> customerId, Nullable<decimal> meterId, string consumedDate,string  monthlyConsumption ,string totalConsumption,string pumpWorkingTime)
        {
            if(consumedDate.StartsWith("20"))
                consumedDate = RsaDateTime.PersianDateTime.ConvertToPersianDate(consumedDate);

            if (consumedDate .Contains( "1300/01/01"))
                return;

            var dt1 = consumedDate.Split(' ');
            if (dt1.Length > 1)
                consumedDate = dt1[0];


            var d1 = Meter_Consumption_Data303.ConsumedWaterdata.FirstOrDefault(x => x.ConsumedDate == consumedDate);

            if (d1 == null)
            {
                var d = new VEE.VeeConsumedWater() { ConsumedDate = consumedDate, MeterId = meterId, CustomerId = customerId ,IsValid=true};
                if (!string.IsNullOrEmpty(totalConsumption))
                {
                    d.TotalConsumption = ReplaceSlash(totalConsumption);
                }
                else if (!string.IsNullOrEmpty(monthlyConsumption))
                {
                    d.MonthlyConsumption = ReplaceSlash( monthlyConsumption);
                }
                else if (!string.IsNullOrEmpty(pumpWorkingTime))
                {
                    d.Flow = ReplaceSlash(pumpWorkingTime);
                }
                Meter_Consumption_Data303.ConsumedWaterdata.Add(d);
            }
            else
            {
                if (!string.IsNullOrEmpty(totalConsumption))
                {
                    d1.TotalConsumption = ReplaceSlash(totalConsumption);
                }
                else if (!string.IsNullOrEmpty(monthlyConsumption))
                {
                    d1.MonthlyConsumption = ReplaceSlash(monthlyConsumption);
                }
                else if (!string.IsNullOrEmpty(pumpWorkingTime))
                {
                    d1.Flow = ReplaceSlash(pumpWorkingTime);
                }
            }            
        }

        string ReplaceSlash(string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Replace("/", ".");
            return value;
        }

        public void SaveInCurveTable(string obissCode,string value,string meterID,string headerID)
        {
            try
            {
                
                    switch (obissCode)
                    {
                        case "0802802800FF_2.0.0":
                            CurveList[0] = value;
                            break;
                        case "0802802800FF_2.1.0":
                            CurveList[1] = value;
                            break;
                        case "0802802800FF_2.1.1":
                            CurveList[2] = value;
                            break;
                        case "0802802800FF_2.1.2":
                            CurveList[3] = value;
                            break;
                        case "0802802800FF_2.1.3":
                            CurveList[4] = value;
                            break;
                        case "0802802800FF_2.1.4":
                            CurveList[5] = value;
                            break;
                        case "0802802800FF_2.1.5":
                            CurveList[6] = value;
                            break;
                        case "0802802800FF_2.2.0":
                            CurveList[7] = value;
                            break;
                        case "0802802800FF_2.2.1":
                            CurveList[8] = value;
                            break;
                        case "0802802800FF_2.2.2":
                            CurveList[9] = value;
                            break;
                        case "0802802800FF_2.2.3":
                            CurveList[10] = value;
                            break;
                        case "0802802800FF_2.2.4":
                            CurveList[11] = value;
                            break;
                        case "0802802800FF_2.2.5":
                            CurveList[12] = value;
                            break;
                        case "0802802800FF_2.0.1":
                            CurveList[13] = value;
                            break;
                        case "0802802800FF_2.0.2":
                            CurveList[14] = value;
                            break;
                        case "0802802800FF_2.0.3":
                            CurveList[15] = value;
                            break;
                    }
              
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        public void InsertCard(string CardNumber, ObjectParameter OBISValueHeaderID)
        {
            try
            {
                ObjectParameter FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                ObjectParameter ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", "");
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter OBISID = new ObjectParameter("OBISID", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                ObjectParameter IsExits = new ObjectParameter("IsExits", false);
                ObjectParameter ObisValueID = new ObjectParameter("ObisValueID", 1000000000000000);
                ObjectParameter ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);

                SQLSPS.INSOBISs("", "FF02", "", "", "", 1, "", null, "", 1, "", "", "", FixedOBISCode, ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMSG);

                Result = new ObjectParameter("Result", 10000000000000000);
                ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                ErrMSG = new ObjectParameter("ErrMSG", "");
                SQLSPS.InsobisValueDetail(Convert.ToDecimal(OBISValueHeaderID.Value), CommonData.SoftwareVersion, Convert.ToDecimal(OBISID.Value), CardNumber, CardNumber, "", ObisValueID, Result, ErrMSG);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public string SaveDateValue(string OBISValue)
        {
            string NewValue = "";
            try
            {
                if (OBISValue.Length ==24 )
                {
                    DateTime cardDate =CommonData.ByteArrayStringToDateTime(OBISValue);
                    NewValue = cardDate.ToPersianString();
                }
                
                else if (OBISValue.Length > 10)
                {
                    if (OBISValue.StartsWith("14") || OBISValue.StartsWith("139"))
                        NewValue = OBISValue;
                    else
                    {
                        DateTime cardDate = Convert.ToDateTime(OBISValue);
                        NewValue = cardDate.ToPersianString(); 
                    }        
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
            return NewValue;
        }
        public void SaveConsumedActiveEnergy(string clockValue,decimal? meterId, ObjectParameter FixedOBISCode, ObjectParameter OBISID, ObjectParameter OBISValueHeaderID, string objectList)
        {
            try
            {
                
                DateTime pd = DateTime.Now;
                DateTime cardDate;
                int dif = Convert.ToInt32(FixedOBISCode.Value.ToString()) - 900;
               
                string[] str = clockValue.Split('/');
            
                if (CommonData.LanguagesID == 2)
                {                   
                    cardDate= PersianDateTime.ConvertToGeorgianDateTime(Convert.ToInt32(str[0]), Convert.ToInt32(str[1]), Convert.ToInt32(str[2].Substring(0, 2)));
                }
                else
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(Convert.ToInt32(str[2].Substring(0, 4)),
                        Convert.ToInt32(str[0]), Convert.ToInt32(str[1].Substring(0, 2)));
                }
                if (dif == 0)
                {
                    pd = cardDate;
                }
                else
                {                    
                    int day = cardDate.Day;
                    pd = cardDate.AddDays(-day + 1);
                    pd = pd.AddMonths(-dif + 1);
                }
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                SQLSPS.INSConsumedActiveEnergy(meterId, objectList, pd.ToPersianString(), DateTime.Now.ToPersianString(), Convert.ToDecimal(OBISID.Value), clockValue, Convert.ToDecimal(OBISValueHeaderID.Value), Result, ErrMSG);
            }

            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public string CreateConsumedActiveEnergyDate(int FixedOBISCode)
        {
            string ConsumedDate = "";
            try
            {
                DateTime pd = DateTime.Now;
                DateTime cardDate;
                int dif = FixedOBISCode - 900;
                
                string[] str = CommonData.ClockValue.Split('/');
                if (CommonData.LanguagesID == 2)
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(Convert.ToInt32(str[0]), Convert.ToInt32(str[1]), Convert.ToInt32(str[2].Substring(0, 2)));
                }
                else
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(Convert.ToInt32(str[2].Substring(0, 4)),
                        Convert.ToInt32(str[0]), Convert.ToInt32(str[1].Substring(0, 2)));
                }

                if (dif == 0)
                {
                    pd = cardDate;
                }
                else
                {
                    int day = cardDate.Day;
                    pd = cardDate.AddDays(-day + 1);
                    pd = pd.AddMonths(-dif + 1);
                }

              
               
                ConsumedDate = pd.ToPersianString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
            return ConsumedDate;
        }
        public void SaveConsumedWater(string meterNumber, string clockValue, decimal? meterId, string softwareVersion,
            ObjectParameter FixedOBISCode, ObjectParameter OBISID, ObjectParameter OBISValueHeaderID, string value, string dateTime, string obisCode, decimal? customerId)
        {
            try
            {
                if (meterNumber.StartsWith("207"))
                    SaveConsumedWater207(clockValue,meterId,FixedOBISCode, OBISID, OBISValueHeaderID, value);
                else
                    SaveConsumedWater303(clockValue,meterId, softwareVersion, FixedOBISCode, OBISID, OBISValueHeaderID, value, dateTime, obisCode,customerId);
            }
            catch (Exception ex)
            {

            }

        }
        public void SaveConsumedWater303(string clockValue, decimal? meterId, string softwareVersion, ObjectParameter FixedOBISCode, ObjectParameter OBISID, ObjectParameter OBISValueHeaderID, string value, string dateTime, string obisCode,decimal? customerId)
        {
            try
            {               
                DateTime cardDate;

                DateTime pden = new DateTime();
                
                int dif = Convert.ToInt32(FixedOBISCode.Value.ToString()) - 800;
                
                DateTime newdate = new DateTime();
                string[] str = clockValue.Split('/');
               
                CultureInfo provider = CultureInfo.InvariantCulture;
                provider = new CultureInfo("en-US");
                if (CommonData.LanguagesID == 2)
                {
                    int year = Convert.ToInt32(str[0]);
                    if (year > 50 && str[0].Length <= 2)
                        year = Convert.ToInt32(str[0]) + 1300;
                    else if (Convert.ToInt32(str[0]) < 50 && str[0].Length <= 2)
                        year = Convert.ToInt32(str[0]) + 1400;


                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(year, Convert.ToInt32(str[1]), Convert.ToInt32(str[2].Substring(0, 2)));
                    newdate = cardDate;                    
                }
                else
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(Convert.ToInt32(str[2].Substring(0, 4)),
                           Convert.ToInt32(str[0]), 
                           Convert.ToInt32(str[1].Substring(0, 2)));                    
                    newdate = cardDate;
                }
                if (dif == 0)
                {
                    pden = newdate;
                }
                else
                {
                    int day = newdate.Day;
                    pden = newdate.AddDays(20 - day);
                    //pden =Convert.ToDateTime(newdate.AddDays( 20 - day));
                    if (newdate.Day >= 20)
                        pden = Convert.ToDateTime(pden.AddMonths(-dif + 1));
                    else
                        pden = Convert.ToDateTime(pden.AddMonths(-dif));
                }
                string dt = pden.Year + "/" + pden.Month + "/" + pden.Day;
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");

                if (softwareVersion!=null && CommonData.SoftwareVersion.StartsWith("RSASEWM303") &&
                    NumericConverter.IntConverter(softwareVersion[softwareVersion.Length - 1].ToString()) > 3)
                {
                    // MessageBox.Show(value);
                    if (!string.IsNullOrEmpty(dateTime) && (dateTime != "0001/01/01 00:00:00"))
                    {
                        SQLSPS.INSConsumedWater(meterId, value, null, dateTime, clockValue,
                            Convert.ToDecimal(OBISID.Value), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            Convert.ToDecimal(OBISValueHeaderID.Value), Result, ErrMSG);
                        SabaNewEntities Bank = new SabaNewEntities();
                        Bank.Database.Connection.Open();
                        // ساعت کارکرد اضافه بشه
                        var w1 = new ShowVeeConsumedWater_Result() { ConsumedDate = RsaDateTime.PersianDateTime.ConvertToPersianDateTime(dateTime), MeterId = meterId, CustomerId = customerId, TotalConsumption = value };
                        if (obisCode.StartsWith("0802010000"))
                        {
                            if (!waterConsumptionList.Any(x=>x.ConsumedDate==w1.ConsumedDate))
                                waterConsumptionList.Add(w1);
                        }
                        else if (obisCode.StartsWith("0802606202"))
                        {
                            var w = waterConsumptionList.FirstOrDefault(x => x.ConsumedDate == w1.ConsumedDate);
                           // if (w != null) w.PumpWorkingHour = value;
                        }
                        else if (obisCode.StartsWith("0802020500"))
                        {
                            // flow
                        }
                        else if (obisCode.StartsWith("0802010100"))
                        {
                            //  Month Consumed Water
                            var w = waterConsumptionList.FirstOrDefault(x => x.ConsumedDate == w1.ConsumedDate);
                            if (w != null) w.MonthlyConsumption = value;
                        }
                        else
                        {

                        }
                    }
                    else
                    { 

                    }

                }
                else
                {
                    //MessageBox.Show(value);
                    SQLSPS.INSConsumedWater(meterId, value, null, dt, clockValue,
                        Convert.ToDecimal(OBISID.Value), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        Convert.ToDecimal(OBISValueHeaderID.Value), Result, ErrMSG);
                }

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }
        }
        public string CreateConsumedWaterDate303(int FixedOBISCode)
        {
            string ConsumedDate = "";
            try
            {
                DateTime pden = new DateTime();
                int dif = FixedOBISCode - 800;
               
                DateTime newdate = new DateTime();
                string[] str = CommonData.ClockValue.Split('/');
                if (CommonData.LanguagesID == 2)
                {
                    int year= Convert.ToInt32(str[0]);
                    
                    if (Convert.ToInt32(str[0]) > 50 && str[0].Length <= 2)
                        year = Convert.ToInt32(str[0]) + 1300;
                    else if (Convert.ToInt32(str[0]) < 50 && str[0].Length <= 2)
                        year = Convert.ToInt32(str[0]) + 1400;
                    newdate= PersianDateTime.ConvertToGeorgianDateTime(year, Convert.ToInt32(str[1]), Convert.ToInt32(str[2].Substring(0, 2))); ;
                }
                else
                {
                    newdate = PersianDateTime.ConvertToGeorgianDateTime(Convert.ToInt32(str[2].Substring(0, 4)),
                        Convert.ToInt32(str[0]),
                        Convert.ToInt32(str[1].Substring(0, 2))); ;
                }
                if (dif == 0)
                {
                    pden = newdate;
                }
                else
                {
                    int day = newdate.Day;
                    pden = Convert.ToDateTime(newdate.AddDays(20 - day));
                    if (newdate.Day >= 20)
                        pden = Convert.ToDateTime(pden.AddMonths(-dif + 1));
                    else
                        pden = Convert.ToDateTime(pden.AddMonths(-dif));
                }
                string dt = pden.Year + "/" + pden.Month + "/" + pden.Day;
                ConsumedDate = dt;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
            return ConsumedDate;
        }
        public void SaveConsumedWater207(string clockValue, decimal? meterId, ObjectParameter FixedOBISCode, ObjectParameter OBISID, ObjectParameter OBISValueHeaderID, string objectList)
        {
            try
            {

                DateTime pd = DateTime.Now;
              

                int dif = Convert.ToInt32(FixedOBISCode.Value.ToString()) - 800;
                DateTime cardDate = new DateTime();

                string[] str = clockValue.Split('/');

                if (CommonData.LanguagesID == 2)
                {                  
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(Convert.ToInt32(str[0]), Convert.ToInt32(str[1]), Convert.ToInt32(str[2].Substring(0, 2)));
                }
                else
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(Convert.ToInt32(str[2].Substring(0, 4)),
                           Convert.ToInt32(str[0]), Convert.ToInt32(str[1].Substring(0, 2)));                   
                }
                if (dif == 0)
                {
                    pd = cardDate;
                }
                else
                {
                    int day = cardDate.Day;
                    pd = cardDate.AddDays(-day + 1);
                    //if (day >= 2)
                    //    pd = (PersianDate)calender.AddMonths(pd, -dif+1 );
                    //else
                    //    pd = (PersianDate)calender.AddMonths(pd, -dif );
                    pd = pd.AddMonths( -dif + 1);

                }
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");

                SQLSPS.INSConsumedWater(meterId, objectList, null, pd.ToPersianString(), clockValue,
                    Convert.ToDecimal(OBISID.Value), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), Convert.ToDecimal(OBISValueHeaderID.Value),
                    Result, ErrMSG);
            }

            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);

            }
        }
        public string CreateConsumedWaterDate207(int FixedOBISCode)
        {
            string ConsumedDate = "";
            try
            {
                DateTime pd = DateTime.Now;
                

                int dif = FixedOBISCode - 800;
                DateTime cardDate = new DateTime();
                string[] str = CommonData.ClockValue.Split('/');
                if (CommonData.LanguagesID == 2)
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(
                        Convert.ToInt32(str[0]), 
                        Convert.ToInt32(str[1]), 
                        Convert.ToInt32(str[2].Substring(0, 2)));                    
                }
                else
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(
                        Convert.ToInt32(str[2].Substring(0, 4)),
                        Convert.ToInt32(str[0]), 
                        Convert.ToInt32(str[1].Substring(0, 2)));                    
                }
                if (dif == 0)
                {
                    pd = cardDate;
                }
                else
                {
                    int day = cardDate.Day;
                    pd = cardDate.AddDays(-day + 1);
                    //if (day >= 2)
                    //    pd = (PersianDate)calender.AddMonths(pd, -dif + 1);
                    //else
                    //    pd = (PersianDate)calender.AddMonths(pd, -dif);
                    pd = pd.AddMonths( -dif + 1);
                }
                ConsumedDate =pd.ToPersianString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
            return ConsumedDate;
        }
     
        public void SaveINDBReadValueFromHHU(string meterNumber,decimal meterId, string clockValue,string softwareVersion,OBISObjectsList objectlist,decimal? customerId)
        {
            try
            {
                //SabaNewEntities Bank = new SabaNewEntities();
                //Bank.Database.Connection.Open();
                objectList = new List<OBISObject>();
                objectList = objectlist.OBISObjectList;
                ObjectParameter FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                ObjectParameter ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", "");
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter OBISID = new ObjectParameter("OBISID", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                ObjectParameter IsExits = new ObjectParameter("IsExits", false);
                ObjectParameter ObisValueID = new ObjectParameter("ObisValueID", 1000000000000000);
                ObjectParameter ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);
                ObjectParameter OBISValueHeaderID = new ObjectParameter("OBISValueHeaderID", 1000000000000000);
                ObjectParameter MeterID = new ObjectParameter("MeterID", 100000000000000);
                SQLSPS.ShowMeterNumber(meterNumber, IsExits, Result, ErrMSG);
                if (CommonData.correctReading)
                {

                    if (!Convert.ToBoolean(SQLSPS.ShowClockObisValue(clockValue, (meterId))))
                    {
                        Result = new ObjectParameter("Result", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        OBISValueHeaderID = new ObjectParameter("OBISValueHeaderID", 1000000000000000);
                        string transferDate = "";
                        if (CommonData.LanguagesID == 2)
                        {
                            transferDate = DateTime.Now.ToPersianString();
                            clockValue = PersianDateTime.ConvertToPersianDateTime(clockValue);

                        }
                        else
                            transferDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Substring(0, 10);

                        SQLSPS.InsobisValueHeader(clockValue, 1, CommonData.UserID, transferDate, meterId, 3, OBISValueHeaderID, Result, ErrMSG);
                        if (ErrMSG.Value.ToString() != "")
                            MessageBox.Show("Line 2845 mw " + ErrMSG.Value.ToString());
                        CurveList=new string[16];

                        for (int j = 0; j < objectList.Count; j++)
                        {

                            Result = new ObjectParameter("Result", 10000000000000000);
                            OBISID = new ObjectParameter("OBISID", 10000000000000000);
                            ErrMSG = new ObjectParameter("ErrMSG", "");
                            ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", 1000000000000000);
                            ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);
                            SQLSPS.INSOBISs("", objectList[j].code, "", "", "", 1, "", null, "", 1, "", "", "", FixedOBISCode, ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMSG);

                             
                            if (objectList[j].code.ToLower().Contains("0802802800ff_"))
                            {
                                SaveInCurveTable( objectList[j].code, objectList[j].value, meterId.ToString(), OBISValueHeaderID.Value.ToString());
                            }
                            if (objectList[j].code.Contains("0100000200F"))
                            {
                                softwareVersion = objectList[j].value.ToString();
                            }


                            if (FixedOBISCode.Value.ToString() != "")
                            {
                                if (Convert.ToInt32(FixedOBISCode.Value.ToString()) == 500 && CommonData.LanguagesID == 2)
                                {
                                    objectList[j].value = SaveDateValue(objectList[j].value);
                                }
                                if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 900 && Convert.ToInt32(FixedOBISCode.Value.ToString()) < 925)
                                {
                                    SaveConsumedActiveEnergy(clockValue,meterId,FixedOBISCode, OBISID, OBISValueHeaderID, objectList[j].value.ToString());
                                }
                                if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 800 && Convert.ToInt32(FixedOBISCode.Value.ToString()) < 825)
                                {
                                    SaveConsumedWater(meterNumber, clockValue,meterId, softwareVersion, FixedOBISCode, OBISID, 
                                        OBISValueHeaderID, objectList[j].value.ToString(), "", "", customerId);
                                }
                            }
                            Result = new ObjectParameter("Result", 10000000000000000);
                            ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                            ErrMSG = new ObjectParameter("ErrMSG", "");
                            SQLSPS.InsobisValueDetail(Convert.ToDecimal(OBISValueHeaderID.Value), softwareVersion, Convert.ToDecimal(OBISID.Value), objectList[j].value, null, "",
                                ObisValueID, Result, ErrMSG);
                        }

                        Result = new ObjectParameter("Result", 10000000000000000);
                        OBISID = new ObjectParameter("OBISID", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", 1000000000000000);
                        ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);

                        SQLSPS.INSOBISs("", "0000", "", "", "", 1, "", null, "", 1, "", "", "", FixedOBISCode, ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMSG);

                        Result = new ObjectParameter("Result", 10000000000000000);
                        ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        SQLSPS.InsobisValueDetail(Convert.ToDecimal(OBISValueHeaderID.Value),softwareVersion, Convert.ToDecimal(OBISID.Value), DateTime.Now.ToPersianString(), clockValue, "", ObisValueID, Result, ErrMSG);

                        Result = new ObjectParameter("Result", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        if (meterNumber.StartsWith("207"))
                            SQLSPS.InsCurve(Convert.ToDecimal(meterId), Convert.ToDecimal(OBISValueHeaderID.Value), CurveList[0], CurveList[1], CurveList[2], CurveList[3], CurveList[4]
                       , CurveList[5], CurveList[6], CurveList[7], CurveList[8], CurveList[9], CurveList[10], CurveList[11],
                       CurveList[12], CurveList[13], CurveList[14], CurveList[15], Result, ErrMSG);
                    }
                    //if (CommonData.SelectedMeterNumber.StartsWith("207"))
                    //    CommonData.mainwindow.Vee207data(CommonData.SelectedMeterID);

                }

                CommonData.mainwindow.changeProgressBarValue(1);

            }
            catch (Exception ex)
            {

                //System.Windows.MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }


        public DataTable FillDataTable(OBISObjectsList objectlist, decimal? MeterID, decimal? UserID, string ReadDate, string transferDate)
        {
            DataTable dt = new DataTable();
            try
            {

                dt.Columns.Add("Code");
                dt.Columns.Add("Value");
                dt.Columns.Add("FixedOBISCode");
                dt.Columns.Add("ConsumedDate");
                dt.Columns.Add("SoftVersion");
                for (int j = 0; j < objectList.Count; j++)
                {
                    if (objectList[j].fixedCode != "")
                    {
                        if (Convert.ToInt32(objectList[j].fixedCode) >= 800 && Convert.ToInt32(objectList[j].fixedCode) < 825)
                        {
                            string ConsumedDate = "";
                            if (CommonData.MeterNumber.StartsWith("207"))
                                ConsumedDate = CreateConsumedWaterDate207(Convert.ToInt32(objectList[j].fixedCode));
                            else
                                ConsumedDate = CreateConsumedWaterDate303(Convert.ToInt32(objectList[j].fixedCode));

                            DataRow dr = dt.NewRow();

                            dr["Code"] = objectList[j].code.ToString();
                            dr["Value"] = objectList[j].value.ToString();
                            dr["FixedOBISCode"] = objectList[j].fixedCode.ToString();
                            dr["ConsumedDate"] = ConsumedDate;
                            dr["SoftVersion"] = CommonData.SoftwareVersion;
                            dt.Rows.Add(dr);

                        }
                        else if (Convert.ToInt32(objectList[j].fixedCode) >= 900 && Convert.ToInt32(objectList[j].fixedCode) < 925)
                        {
                            string ConsumedDate = "";
                            ConsumedDate = CreateConsumedActiveEnergyDate(Convert.ToInt32(objectList[j].fixedCode));
                            DataRow dr = dt.NewRow();
                            dr["Code"] = objectList[j].code.ToString();
                            dr["Value"] = objectList[j].value.ToString();
                            dr["FixedOBISCode"] = objectList[j].fixedCode.ToString();
                            dr["ConsumedDate"] = ConsumedDate;
                            dr["SoftVersion"] = CommonData.SoftwareVersion;
                            dt.Rows.Add(dr);

                        }
                        else if (Convert.ToInt32(objectList[j].fixedCode) == 500 && CommonData.LanguagesID == 2)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Code"] = objectList[j].code.ToString();
                            dr["Value"] = SaveDateValue(objectList[j].value);
                            dr["FixedOBISCode"] = 0;
                            dr["ConsumedDate"] = "";
                            dr["SoftVersion"] = CommonData.SoftwareVersion;
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["Code"] = objectList[j].code.ToString();
                        dr["Value"] = objectList[j].value.ToString();
                        dr["FixedOBISCode"] = 0;
                        dr["ConsumedDate"] = "";
                        dr["SoftVersion"] = CommonData.SoftwareVersion;
                        dt.Rows.Add(dr);

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
            return dt;
        }

        

        private void ResetCardInfoinCommonData()
        {
            try
            {
                CommonData.MeterID = 1000000000;
                CommonData.MeterNumber = "";
                CommonData.ClockValue = "";
                CommonData.CardID = 1000000000;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        private void getinfo(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 5);
                if (us.CanShow)
                {
                    if (CommonData.carddataReceive != null)
                    {
                        CommonData.carddataReceive.Close();
                    }
                    // Read card if Not in همه گروه ها
                    SelectedGroup = (ShowGroups_Result)CmbGroups.SelectedItem;
                    if (SelectedGroup.GroupName == "همه گروه ها")
                    {
                        MessageBox.Show("لطفا گروه دیگری را برای دریافت اطلاعات از کارت  انتخاب نمایید");
                        return;
                    }

                    ReadFromCard = false;
                    ReadCard(0);

                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }



        }
        public void ShowCardDate(int type)
        {
            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                               new Action(
                                   delegate ()
                                   {
                                       if (CommonData.IsCompleteReadOut)
                                       {
                                           string Result = CheckMeterForCustomerandGroup(CommonData.CardMeterNumber);
                                           if (Result == "True")
                                           {
                                               CardInfoReceive card = new CardInfoReceive();

                                               card.Title = translateWindowName.TranslateofLable.Object5;
                                               CommonData.carddataReceive = card;
                                               card.Owner = this;
                                               card.Tab = tabControl1;
                                               card.CardNumber = CommonData.CardNumber;
                                               ClosableTab theTabItem = new ClosableTab();
                                               theTabItem.Title = card.Title;
                                               theTabItem.windowname = card;
                                               tabControl1.Items.Add(theTabItem);
                                               theTabItem.Focus();
                                               card.TabPag = theTabItem;
                                               if (type == 1)
                                                   card.tabControlMain.SelectedItem = card.tabitemCredittocard;
                                               card.Show();
                                               card.BringIntoView();
                                           }

                                           else if (Result == "NO Group")
                                           {
                                               MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message94, "", MessageBoxButton.OK);
                                               if (MBResult == MessageBoxResult.OK)
                                               {
                                                   CommonData.mainwindow.ShowMeters(1, CommonData.CardMeterNumber, false);
                                               }

                                           }
                                           else if (Result == "NO Customer")
                                           {
                                               MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message93, "", MessageBoxButton.OK);
                                               if (MBResult == MessageBoxResult.OK)
                                               {
                                                   CommonData.mainwindow.ShowMeters(1, CommonData.CardMeterNumber, false);
                                               }
                                           }
                                           else if (Result == "NO Group and Customer")
                                           {
                                               MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message96, "", MessageBoxButton.OK);
                                               if (CommonData.SelectedMeterNumber.StartsWith("207"))
                                               {
                                                   VeeMeterData vee = new VeeMeterData();
                                                   vee.Vee207data(CommonData.SelectedMeterID, CommonData.CardMeterNumber, CommonData.CardCustomerId);
                                               }
                                               if (MBResult == MessageBoxResult.OK)
                                               {
                                                   CommonData.mainwindow.ShowMeters(1, CommonData.CardMeterNumber, false);
                                               }

                                           }
                                        

                                       }
                                   }
                ));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        private void Ekart(object sender, RoutedEventArgs e)
        {

        }

        private void creatKart(object sender, RoutedEventArgs e)
        {
            us = CommonData.ShowButtonBinding("", 47);
            if (us.CanShow)
            {
                if (!ClassControl.OpenWin[47])
                {
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(0);
                    ClassControl.OpenWin[24] = true;
                    IssuanceCard Ckart = new IssuanceCard();
                    Ckart.Owner = this;
                    Ckart.Tab = tabControl1;
                    ClosableTab theTabItem = new ClosableTab();
                    theTabItem.Title = Ckart.Title;
                    theTabItem.windowname = Ckart;
                    tabControl1.Items.Add(theTabItem);
                    theTabItem.Focus();
                    Ckart.TabPag = theTabItem;
                    Ckart.Show();
                }
            }
            else
            {
                MessageBox.Show(tm.TranslateofMessage.Message55);
                return;
            }
        }



        private void mdm(object sender, RoutedEventArgs e)
        {

        }

        private void excs(object sender, RoutedEventArgs e)
        {

        }

        private void billingexit(object sender, RoutedEventArgs e)
        {

        }

        private void sendsms(object sender, RoutedEventArgs e)
        {

        }


        private void ___MenuItem___OBIS_جديد__Click(object sender, RoutedEventArgs e)
        {


            try
            {
                us = CommonData.ShowButtonBinding("", 11);
                if (us.CanShow)
                {

                    if (!ClassControl.OpenWin[11])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[11] = true;
                        NewObis winmdm = new NewObis();
                        winmdm.Title = translateWindowName.TranslateofLable.Object11;
                        winmdm.Owner = this;
                        winmdm.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = winmdm.Title;
                        theTabItem.windowname = winmdm;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        winmdm.TabPag = theTabItem;
                        winmdm.Show();

                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }


        }



        //////
        /// پنجرهها

        private void SortWindows_Click(object sender, RoutedEventArgs e)
        {

            int i = 0; // How many windows is open.
            int j = 1; // sort from top.
            System.Windows.Window childwindow;
            double leftpoint = 0;
            double toppoint = 0;
            try
            {
                int row = 0;
                int column = 0;
                foreach (var item in tabControl1.Items)
                {
                    i++;
                    ClosableTab t = (ClosableTab)item;
                    childwindow = t.windowname;
                    childwindow.Width = 400;
                    childwindow.Height = 500;
                }
                foreach (var item in tabControl1.Items)
                {
                    ClosableTab t = (ClosableTab)item;
                    childwindow = t.windowname;
                    childwindow.WindowStartupLocation = WindowStartupLocation.Manual;

                    childwindow.Width = 400;
                    childwindow.Height = 500;
                    row++;
                    column++;
                    j++;

                    leftpoint = SystemParameters.WorkArea.Width - childwindow.Width - 550;
                    toppoint = SystemParameters.WorkArea.Height - childwindow.Height - 280;

                    toppoint = toppoint + (row * 10 + j * 12);
                    leftpoint = leftpoint + (column * 10);
                    childwindow.Left = leftpoint;
                    childwindow.Top = toppoint;
                    childwindow.Focus();
                }
            }

            catch
            {
                MessageBox.Show("Error In Cascade");
            }

        }
        /// <summary>
        /// ///////////// پنجرهها
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void side_by_side_Click(object sender, RoutedEventArgs e)
        {
            int i = 0; // Howmany windows is open.
            int j = 0;
            System.Windows.Window childwindow;

            try
            {

                foreach (var item in tabControl1.Items)
                {
                    i++;
                }
                if (i != 0)
                {
                    double maxheigh = (this.ActualHeight - 100);
                    double maxwidth = (this.ActualWidth - 200) / i;
                    foreach (var item in tabControl1.Items)
                    {
                        ClosableTab t = (ClosableTab)item;
                        childwindow = t.windowname;
                        childwindow.WindowStartupLocation = WindowStartupLocation.Manual;

                        if (maxheigh < childwindow.MinHeight)
                            maxheigh = childwindow.MinHeight;
                        if (maxwidth < childwindow.MinWidth)
                            maxwidth = childwindow.MinWidth;
                        childwindow.Height = maxheigh;
                        childwindow.Width = maxwidth;

                        Canvas.SetLeft(childwindow, j * maxwidth);
                        Canvas.SetTop(childwindow, this.ActualHeight - maxheigh);
                        j++;

                        if (((j + 1) * maxwidth) > (this.ActualWidth - 200))
                            j = 0;

                    }
                }
            }
            catch
            {
                MessageBox.Show("Error in Side by side");
            }
        }

        public void New_Meter_Click(object sender, RoutedEventArgs e)
        {
            ShowMeters(0, "", false);

        }
        public void ShowMeters(int type, string NewMeterNumber, bool IsNew)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 3);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[3])
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                               new Action(
                                   delegate ()
                                   {
                                       CommonData.mainwindow.changeProgressBarTag("");
                                       CommonData.mainwindow.changeProgressBarValue(0);
                                       ClassControl.OpenWin[3] = true;
                                       CommonData.SelectedMeterID = CommonData.MeterID;
                                       CommonData.SelectedMeterNumber = NewMeterNumber;
                                       Meters meter = new Meters(type, NewMeterNumber, IsNew,CommonData.MeterID);
                                       meter.Title = translateWindowName.TranslateofLable.Object3;
                                       meter.Owner = this;
                                       meter.Tab = tabControl1;
                                       ClosableTab theTabItem = new ClosableTab();
                                       theTabItem.Title = meter.Title;
                                       theTabItem.windowname = meter;
                                       tabControl1.Items.Add(theTabItem);
                                       theTabItem.Focus();
                                       meter.TabPag = theTabItem;
                                       //bool? x=  meter.ShowDialog();
                                       meter.Show();
                                   }));
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        private void omite_Meter_Click(object sender, RoutedEventArgs e)
        {
            if (!ClassControl.OpenWin[4])
            {
                ClassControl.OpenWin[4] = true;
                Modems G4 = new Modems();

                // G4.FlowDirection = FlowDirection.LeftToRight;
                //G4.GridDown.FlowDirection = CommonData.FlowDirection;

                G4.Owner = this;
                G4.Tab = tabControl1;
                ClosableTab theTabItem = new ClosableTab();
                theTabItem.Title = G4.Title;
                theTabItem.windowname = G4;
                tabControl1.Items.Add(theTabItem);
                theTabItem.Focus();
                G4.TabPag = theTabItem;
                G4.Show();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void sortwindow1_Click(object sender, RoutedEventArgs e)
        {

            int i = 0; // How many windows is open.
            int j = 1; // sort from top.
            System.Windows.Window childwindow;
            double leftpoint = 0;
            double toppoint = 0;

            try
            {
                int row = 0;
                int column = 0;
                foreach (var item in tabControl1.Items)
                {
                    i++;
                    ClosableTab t = (ClosableTab)item;
                    childwindow = t.windowname;
                    if (childwindow.Height == childwindow.MaxHeight && childwindow.Width == childwindow.MaxWidth)
                    {
                        childwindow.Height = childwindow.MinHeight;
                        childwindow.Width = childwindow.MinWidth;
                    }
                    childwindow.Width = 400;
                    childwindow.Height = 500;
                }
                foreach (var item in tabControl1.Items)
                {
                    ClosableTab t = (ClosableTab)item;
                    childwindow = t.windowname;
                    childwindow.WindowStartupLocation = WindowStartupLocation.Manual;

                    childwindow.Width = 400;
                    childwindow.Height = 500;
                    row++;
                    column++;
                    j++;

                    leftpoint = SystemParameters.WorkArea.Width - childwindow.Width - 550;
                    toppoint = SystemParameters.WorkArea.Height - childwindow.Height - 280;

                    toppoint = toppoint + (row * 10 + j * 12);
                    leftpoint = leftpoint + (column * 10);
                    childwindow.Left = leftpoint;
                    childwindow.Top = toppoint;
                    childwindow.Focus();
                }
            }

            catch
            {
                MessageBox.Show("Error In Cascade");
            }
        }


        private void sortwindow2_Click(object sender, RoutedEventArgs e)
        {
            int i = 0; // Howmany windows is open.
            int j = 0;
            System.Windows.Window childwindow;

            try
            {

                foreach (var item in tabControl1.Items)
                {
                    i++;
                }
                if (i != 0)
                {
                    double maxheigh = (this.ActualHeight - 100);
                    double maxwidth = (this.ActualWidth - 200) / i;
                    foreach (var item in tabControl1.Items)
                    {
                        ClosableTab t = (ClosableTab)item;
                        childwindow = t.windowname;
                        childwindow.WindowStartupLocation = WindowStartupLocation.Manual;

                        if (maxheigh < childwindow.MinHeight)
                            maxheigh = childwindow.MinHeight;
                        if (maxwidth < childwindow.MinWidth)
                            maxwidth = childwindow.MinWidth;
                        childwindow.Height = maxheigh;
                        childwindow.Width = maxwidth;

                        Canvas.SetLeft(childwindow, j * maxwidth);
                        Canvas.SetTop(childwindow, this.ActualHeight - maxheigh);
                        j++;

                        if (((j + 1) * maxwidth) > (this.ActualWidth - 200))
                            j = 0;

                    }
                }
            }
            catch
            {
                MessageBox.Show("Error in Side by side");
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                (sender as Grid).DataContext = tr.TranslateofLable;
                //(sender as Grid).FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void Isvisable_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ISchecked)
                    ChangelstMeter(true, CommonData.SelectedMeterID);
                ISchecked = false;
                MeterGrid.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void ChangelstMeter(bool IsVisable, decimal? i)
        {
            try
            {
                for (int j = 0; j < lstSelectedMeter.Count; j++)
                {
                    SelectedMeter UsG = new SelectedMeter();
                    UsG = lstSelectedMeter[j];
                    if (UsG.MeterId == i)
                    {
                        UsG.Isvisable = IsVisable;
                        lstSelectedMeter[j] = UsG;
                    }

                }
                //if (lstSelectedMeter.Count > i && i >= 0)
                //{
                //    SelectedMeter UsG = new SelectedMeter();
                //    UsG = lstSelectedMeter[i];
                //    UsG.Isvisable = IsVisable;
                //    lstSelectedMeter[i] = UsG;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        private void Isvisable_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ISchecked)
                    ChangelstMeter(false, CommonData.SelectedMeterID);
                ISchecked = false;
                MeterGrid.Focus();
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
                MeterGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                    delegate ()
                    {
                        try
                        {
                            //MeterGrid.ItemsSource = null;
                            SabaNewEntities Bank = new SabaNewEntities();
                            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 8000;
                            lstSelectedMeter = new List<SelectedMeter>();
                            Bank.Database.Connection.Open();
                            foreach (ShowMeter_Result item in Bank.ShowMeter(filter, CommonData.UserID))
                            {
                                if (item.WatersubscriptionNumber .Contains( "513364")|| item.WatersubscriptionNumber.Contains("513470")|| item.WatersubscriptionNumber.Contains("513306"))
                                {

                                }
                                SelectedMeter UsG = new SelectedMeter();
                                UsG.MeterId = item.MeterID;
                                UsG.MeterNumber = item.MeterNumber;
                                UsG.WatersubscriptionNumber = item.WatersubscriptionNumber;
                                UsG.ElecsubscriptionNumber = item.ElecsubscriptionNumber;
                                UsG.CustomerId = item.CustomerID;
                                UsG.HasError = item.HasError;
                                //UsG.SourceTypeName = item.SourceTypeName;
                                if (UsG.MeterNumber.StartsWith("207") || UsG.MeterNumber.StartsWith("19"))
                                    lstSelectedMeter.Add(UsG);

                                //if (UsG.MeterNumber.StartsWith("207"))
                                //{
                                //    VeeMeterData vee = new VeeMeterData();
                                //    vee.Vee207data(Convert.ToDecimal(item.MeterID), UsG.MeterNumber, UsG.CustomerId); 
                                //}
                            }
                           
                            Bank.Database.Connection.Close();
                            Bank.Dispose();
                            MeterGrid.ItemsSource = lstSelectedMeter;


                             
                            string rowcount = "";
                            if (CommonData.mainwindow != null)
                            {
                                rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                                changeProgressBarTag("");
                            }
                            lblcount.Content = rowcount + "=" + MeterGrid.Items.Count;
                        }
                        catch (Exception ex)
                        {
                            if (ex.ToString().ToLower().Contains("sql"))
                            {
                                System.Windows.Forms.MessageBox.Show("در حال حاضر امکان دسترسی به پایگاه داده مورد نظر امکان ندارد");
                                MeterGrid.ItemsSource = null;


                            }
                            else
                            {
                                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                            }
                        }
                    }
                    )
                    );

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void MeterGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MeterGrid.SelectedItem != null)
                {
                    SelectedRow = MeterGrid.SelectedIndex;
                    SelectedMeter Row = new SelectedMeter();
                    Row = (SelectedMeter)MeterGrid.SelectedItem;
                    CommonData.SelectedMeterID = Row.MeterId;
                    CommonData.SelectedMeterNumber = Row.MeterNumber;
                    CommonData.CustomerId = Row.CustomerId;
                    CommonData.MeterID = CommonData.SelectedMeterID;
                    CommonData.MeterNumber = CommonData.SelectedMeterNumber;
                    if (CommonData.showmeterdata != null)
                    {
                        CommonData.showmeterdata.txtMeterNumber.Text = Row.MeterNumber;
                        CommonData.showmeterdata.RereshGridHeader();
                       


                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void NewModel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 9);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[9])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[9] = true;
                        UI.DeviceModel newModel = new UI.DeviceModel();
                        newModel.Title = translateWindowName.TranslateofLable.Object9;
                        newModel.Owner = this;
                        newModel.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = newModel.Title;
                        theTabItem.windowname = newModel;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        newModel.TabPag = theTabItem;
                        newModel.Show();

                    }
                }
                else
                {
                    MessageBox.Show("شما امکان دسترسی به این فرم را ندارید");
                    return;
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }

        }

        #region Card
        //public void SelectICard()
        //{
        //    try
        //    {

        //        if (m_iCard != null)
        //            m_iCard.Disconnect(DISCONNECT.Unpower);

        //        m_iCard = new CardNative();


        //        m_iCard.OnCardInserted += new CardInsertedEventHandler(m_iCard_OnCardInserted);
        //        m_iCard.OnCardRemoved += new CardRemovedEventHandler(m_iCard_OnCardRemoved);
        //        m_iCard.StartCardEvents(DefaultReader);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        ///// <summary>
        ///// CardRemovedEventHandler
        ///// </summary>
        //private void m_iCard_OnCardRemoved()
        //{
        //    if (CommonData.carddataReceive != null)
        //        CommonData.carddataReceive.changeTabVisibility(CommonData.carddataReceive.tabitemCreditsassigned, System.Windows.Visibility.Hidden);
        //         }

        ///// <summary>
        ///// CardInsertedEventHandler
        ///// </summary>
        //private void m_iCard_OnCardInserted()
        //{
        //    try
        //    {
        //        if (CommonData.carddataReceive != null )
        //        {
        //            DateTime nowDateTime = DateTime.Now;

        //            TimeSpan diff = nowDateTime - cardDetectionOldTime;

        //            int millisceonds = (int)diff.TotalMilliseconds;

        //            if (millisceonds < 17000)
        //            {
        //                return;
        //            }

        //            Thread th = new Thread(CommonData.carddataReceive.getDataFromCard);
        //            th.Start();
        //            // CommonData.mainWindows.getDataFromCard();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
        //    }
        //}

        #endregion
        #region Token
        #region ProgressBar

        public void changeProgressBarTag(string text)
        {
            ProgressBar_ReadingCard.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
       new Action(
           delegate ()
           {
               GetProgressBarBackGround(false);
               if (text.Length > 80)
               {
                   ProgressBar_ReadingCard.FontSize = 20;
               }
               else
                   ProgressBar_ReadingCard.FontSize = 26;

               ProgressBar_ReadingCard.Tag = text;

           }));
        }
        public void changeProgressBar_MaximumValue(int max)
        {
            ProgressBar_ReadingCard.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
       new Action(
           delegate ()
           {
               ProgressBar_ReadingCard.Maximum = max;

           }));
        }
       
        public void changeProgressBarValue(double value)
        {
            ProgressBar_ReadingCard.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
       new Action(
           delegate ()
           {
               if (ProgressBar_ReadingCard.Visibility == Visibility.Hidden)
                   ProgressBar_ReadingCard.Visibility = Visibility.Visible;
               if (value != 100000)
               {
                   CommonData.counter += value;
               }
               if (value == 0)
               {
                   CommonData.counter = 0;
                   ProgressBar_ReadingCard.Visibility = Visibility.Visible;
                   ProgressBar_ReadingCard.ClearValue(ProgressBar.ValueProperty);
                   ProgressBar_ReadingCard.SetValue(ProgressBar.ValueProperty, 0.0);
                   ProgressBar_ReadingCard.Value = 0;
                   ProgressBar_ReadingCard.BeginAnimation(ProgressBar.ValueProperty, null);
                   ProgressBar_ReadingCard.Value = 0;
               }
               else if (value == 1000)
               {
                   ProgressBar_ReadingCard.IsIndeterminate = false;
                   ProgressBar_ReadingCard.Orientation = Orientation.Horizontal;

                   Duration duration = new Duration(TimeSpan.FromSeconds(1));
                   DoubleAnimation doubleanimation = new DoubleAnimation(ProgressBar_ReadingCard.Maximum, duration);

                   ProgressBar_ReadingCard.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);

               }

               else
               {
                   ProgressBar_ReadingCard.IsIndeterminate = false;
                   ProgressBar_ReadingCard.Orientation = Orientation.Horizontal;

                   Duration duration = new Duration(TimeSpan.FromSeconds(1));
                   DoubleAnimation doubleanimation = new DoubleAnimation(ProgressBar_ReadingCard.Value+value, duration);
                   ProgressBar_ReadingCard.Value += value;

                   ProgressBar_ReadingCard.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);

               }

           }));
        }
        internal void GetProgressBarBackGround(bool isError)
        {
            ProgressBar_ReadingCard.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                delegate ()
                {
                    if (isError)
                        CommonData.mainwindow.ProgressBar_ReadingCard.Background =System.Windows.Media. Brushes.Yellow;
                    else
                        CommonData.mainwindow.ProgressBar_ReadingCard.Background = System.Windows.Media.Brushes.WhiteSmoke;
                }));
        }
        internal void GetProgressBarValue()
        {
            ProgressBar_ReadingCard.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                delegate ()
                {
                    progressBarValue = (int)CommonData.mainwindow.ProgressBar_ReadingCard.Value;

                }));
        }
        #endregion

        private void window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (m_iCard != null)
                {
                    m_iCard.Disconnect(DISCONNECT.Unpower);
                    m_iCard.StopCardEvents();
                }
               
                Application.Current.Shutdown();

                Environment.Exit(0);
            }
            catch (Exception)
            {
            }
        }
        #endregion Token

        private void CmbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string Filter = "";
                if (CmbGroups.SelectedItem != null)
                {
                    SelectedGroup = (ShowGroups_Result)CmbGroups.SelectedItem;
                    CommonData.selectedGroup = SelectedGroup;

                    SelectAll = false;
                }
                if (SelectedGroup.GroupID != -1 && SelectedGroup.GroupName.ToUpper() == "همه گروه ها")
                {
                    string Filter1 = "", Filter2 = "";
                    int[] Types = new int[CmbGroups.Items.Count];
                    //
                    string f = "";
                    string p = CommonData.ProvinceCode;
                    int value = Int32.Parse(p, NumberStyles.HexNumber);
                    value = value / 1000;
                    if (value == 1)
                        value = 8;
                    else if (value == 8)
                        value = 1;

                    if (value != 0)
                        f = " and ProvinceID in (0," + value + ")";


                    //
                    ShowGroups Groups = new ShowGroups(f, 1, CommonData.LanguagesID);
                    Filter1 = " and Main.MeterID in (Select MeterID From MeterToGroup where GroupID in ( ";
                    Filter2 = " and  GroupType in ( ";
                    for (int i = 0; i < Groups._lstShowGroups.Count; i++)
                    {
                        Types[i] = Groups._lstShowGroups[i].GroupType;
                        if (i == 0)
                        {
                            Filter2 = Filter2 + Types[i];
                            Filter1 = Filter1 + Groups._lstShowGroups[i].GroupID;
                        }
                        else
                        {
                            Filter2 = Filter2 + "," + Types[i];
                            Filter1 = Filter1 + "," + Groups._lstShowGroups[i].GroupID;
                        }

                    }
                    Filter = Filter1 + ")" + Filter2 + ")) ";

                }
                if (SelectedGroup.GroupID != -1 && SelectedGroup.GroupName != "همه گروه ها")
                    Filter = " and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + SelectedGroup.GroupID + "and  GroupType=" + SelectedGroup.GroupType + ") ";
                Load(Filter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        public void Load(string Filter)
        {
            changeProgressBarTag(tm.TranslateofMessage.Message41);
            Thread th = new Thread(delegate () { RefreshSelectedMeters(Filter); });
            th.Start();

        }

        private void MnuAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 12);
                if (us != null)

                    if (us.CanShow)
                    {
                        if (!ClassControl.OpenWin[12])
                        {
                            CommonData.mainwindow.changeProgressBarTag("");
                            CommonData.mainwindow.changeProgressBarValue(0);
                            ClassControl.OpenWin[12] = true;
                            NewUser newuser = new NewUser();
                            newuser.Title = translateWindowName.TranslateofLable.Object12;
                            newuser.Owner = this;
                            newuser.Tab = tabControl1;
                            ClosableTab theTabItem = new ClosableTab();
                            theTabItem.Title = newuser.Title;
                            theTabItem.windowname = newuser;
                            tabControl1.Items.Add(theTabItem);
                            theTabItem.Focus();
                            newuser.TabPag = theTabItem;
                            newuser.Title = translateWindowName.TranslateofLable.Object12;
                            newuser.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show(tm.TranslateofMessage.Message55);
                        return;
                    }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            us = CommonData.ShowButtonBinding("", 17);

            if (us.CanShow)
            {
                if (!ClassControl.OpenWin[17])
                {
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(0);
                    ClassControl.OpenWin[17] = true;
                    CommonData.MainList = lstSelectedMeter;
                    ExportToExcel ExportToExcel = new ExportToExcel();
                    ExportToExcel.Title = translateWindowName.TranslateofLable.Object17;
                    ExportToExcel.Owner = this;
                    ExportToExcel.Tab = tabControl1;
                    ClosableTab theTabItem = new ClosableTab();
                    theTabItem.Title = ExportToExcel.Title;
                    theTabItem.windowname = ExportToExcel;
                    tabControl1.Items.Add(theTabItem);
                    theTabItem.Focus();
                    ExportToExcel.TabPag = theTabItem;
                    ExportToExcel.Show();
                }
            }
            else
            {
                MessageBox.Show(tm.TranslateofMessage.Message55);
                return;
            }
        }
        private void mnuMeterGeneralInformation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 35);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[34])
                    {
                        string Result = CheckMeterForCustomerandGroup(CommonData.MeterNumber);
                        if (Result == "True")
                        {
                            CommonData.mainwindow.changeProgressBarTag("");
                            CommonData.mainwindow.changeProgressBarValue(0);
                            ClassControl.OpenWin[34] = true;
                            ShowCardData Water = new ShowCardData();
                            Water.Title = translateWindowName.TranslateofLable.Object6;
                            Water.Owner = this;
                            Water.Tab = tabControl1;
                            ClosableTab theTabItem = new ClosableTab();
                            theTabItem.Title = Water.Title;
                            theTabItem.windowname = Water;
                            tabControl1.Items.Add(theTabItem);
                            theTabItem.Focus();
                            Water.TabPag = theTabItem;
                            Water.tabControlMain.SelectedItem = Water.tabitemGeneral;
                            Water.Show();
                            CommonData.showmeterdata = Water;
                        }
                        else if (Result == "NO Group")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message94, "", MessageBoxButton.OKCancel);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                        else if (Result == "NO Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message93, "", MessageBoxButton.OKCancel);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                        else if (Result == "NO Group and Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message96, "",MessageBoxButton.OKCancel);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void mnuMeterStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 36);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[34])
                    {
                        string Result = CheckMeterForCustomerandGroup(CommonData.MeterNumber);
                        if (Result == "True")
                        {
                            CommonData.mainwindow.changeProgressBarTag("");
                            CommonData.mainwindow.changeProgressBarValue(0);
                            ClassControl.OpenWin[34] = true;
                            ShowCardData Water = new ShowCardData();
                            Water.Title = translateWindowName.TranslateofLable.Object6;
                            Water.Owner = this;
                            Water.Tab = tabControl1;
                            ClosableTab theTabItem = new ClosableTab();
                            theTabItem.Title = Water.Title;
                            theTabItem.windowname = Water;
                            tabControl1.Items.Add(theTabItem);
                            theTabItem.Focus();
                            Water.TabPag = theTabItem;
                            Water.tabControlMain.SelectedItem = Water.tabitemMeterStatus;
                            Water.Show();
                            CommonData.showmeterdata = Water;
                        }
                        else if (Result == "NO Group")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message94, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                        else if (Result == "NO Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message93, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                        else if (Result == "NO Group and Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message96, "",
                                MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void mnuNegativeCredit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 21);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[21])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[21] = true;
                        negativeCreditReport negativeCreditWater = new negativeCreditReport();
                        negativeCreditWater.Title = translateWindowName.TranslateofLable.Object21;
                        negativeCreditWater.Owner = this;
                        negativeCreditWater.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = negativeCreditWater.Title;
                        theTabItem.windowname = negativeCreditWater;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        negativeCreditWater.TabPag = theTabItem;
                        negativeCreditWater.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void mnuNominalDemand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 22);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[22])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[22] = true;
                        NominalDemandViolationReport NominalDemandreport = new NominalDemandViolationReport();
                        NominalDemandreport.Title = translateWindowName.TranslateofLable.Object22;
                        NominalDemandreport.Owner = this;
                        NominalDemandreport.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = NominalDemandreport.Title;
                        theTabItem.windowname = NominalDemandreport;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        NominalDemandreport.TabPag = theTabItem;
                        NominalDemandreport.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void mnuErrorControllReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 23);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[23])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[23] = true;
                        ErrorControlReport errorreport = new ErrorControlReport();
                        errorreport.Title = translateWindowName.TranslateofLable.Object23;
                        errorreport.Owner = this;
                        errorreport.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = errorreport.Title;
                        theTabItem.windowname = errorreport;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        errorreport.TabPag = theTabItem;
                        errorreport.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void mnuPowermanagmentReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 24);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[24])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[24] = true;
                        PowerManagmentReport powermanageReport = new PowerManagmentReport();
                        powermanageReport.Title = translateWindowName.TranslateofLable.Object24;
                        powermanageReport.Owner = this;
                        powermanageReport.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = powermanageReport.Title;
                        theTabItem.windowname = powermanageReport;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        powermanageReport.TabPag = theTabItem;
                        powermanageReport.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void mnu_303_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 36);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[34])
                    {
                        string Result = CheckMeterForCustomerandGroup(CommonData.MeterNumber);
                        if (Result == "True")
                        {
                            CommonData.mainwindow.changeProgressBarTag("");
                            CommonData.mainwindow.changeProgressBarValue(0);
                            ClassControl.OpenWin[34] = true;
                            ShowCardData Water = new ShowCardData();
                            Water.Title = translateWindowName.TranslateofLable.Object6;
                            Water.Owner = this;
                            Water.Tab = tabControl1;
                            ClosableTab theTabItem = new ClosableTab();
                            theTabItem.Title = Water.Title;
                            theTabItem.windowname = Water;
                            tabControl1.Items.Add(theTabItem);
                            theTabItem.Focus();
                            Water.TabPag = theTabItem;
                            Water.tabControlMain.SelectedItem = Water.tabitemMeterStatus;
                            Water.Show();
                        }
                        else if (Result == "NO Group")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message94, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                        else if (Result == "NO Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message93, "", MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }
                        }
                        else if (Result == "NO Group and Customer")
                        {
                            MessageBoxResult MBResult = MessageBox.Show(tm.TranslateofMessage.Message96, "",
                                MessageBoxButton.OK);
                            if (MBResult == MessageBoxResult.OK)
                            {
                                CommonData.mainwindow.ShowMeters(1, CommonData.MeterNumber, false);
                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        }
        #region Card
        private void SelectICard()
        {
            try
            {

                if (m_iCard != null)
                    m_iCard.Disconnect(DISCONNECT.Unpower);

                m_iCard = new CardNative();


                m_iCard.OnCardInserted += new CardInsertedEventHandler(m_iCard_OnCardInserted);
                m_iCard.OnCardRemoved += new CardRemovedEventHandler(m_iCard_OnCardRemoved);
                m_iCard.StartCardEvents(DefaultReader);

            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("SelectICard");
            }
        }

        /// <summary>
        /// CardRemovedEventHandler
        /// </summary>
        private void m_iCard_OnCardRemoved()
        {
            if (CommonData.carddataReceive != null)
                CommonData.carddataReceive.changeTabVisibility(Visibility.Hidden);
            if (CommonData.mainwindow != null)
            {
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
            }
        }

        /// <summary>
        /// CardInsertedEventHandler
        /// </summary>
        private void m_iCard_OnCardInserted()
        {
            //try
            //{
            //    if (CommonData.show303Data != null && CommonData.show207Data != null)
            //    {
            //        DateTime nowDateTime = DateTime.Now;

            //        TimeSpan diff = nowDateTime - cardDetectionOldTime;

            //        int millisceonds = (int)diff.TotalMilliseconds;

            //        if (millisceonds < 17000)
            //        {
            //            return;
            //        }

            //        Thread th = new Thread(getDataFromCard);
            //        th.Start();
            //        // CommonData.mainWindows.getDataFromCard();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            //}
        }

        #endregion

        private void mnuCustomers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 4);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[4])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[4] = true;
                        Customers customer = new Customers();
                        customer.Title = translateWindowName.TranslateofLable.Object4;
                        customer.Owner = this;
                        customer.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = customer.Title;
                        theTabItem.windowname = customer;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        customer.TabPag = theTabItem;
                        customer.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void VeeListoFMeter()
        {
            try
            {
                // چرا اینجا vee لازم دارد؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟
                changeProgressBarValue(0);

                ShowMeterForVee metervee = new ShowMeterForVee("");
                changeProgressBar_MaximumValue(metervee._lstShowMeterForVEE.Count);
                changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);

                foreach (var item in metervee._lstShowMeterForVEE)
                {

                    //new Thread(delegate()
                    //{
                    // مشترک حتما باید تعریف شده باشد
                    VeeMeterData vee = new VeeMeterData();
                    vee.                Vee207data(Convert.ToDecimal(item.MeterID),item.SerialNum.ToString(),null);
                    //}).Start();
                    changeProgressBarValue(1);
                }
                changeProgressBarValue(metervee._lstShowMeterForVEE.Count);
                changeProgressBarTag("");
            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void VeeListoFMeterFromHHU()
        {
            try
            {
                string str = "";
                foreach (var item in MeterList)
                {

                    if (item.MeterNumber.StartsWith("207") && !(str.Contains(item.MeterNumber + ";")))
                    {
                        //اطلاعات مشترک را هم باید داشته باشیم
                        VeeMeterData vee = new VeeMeterData();
                        ShowMeter_Result MeterInfo = SQLSPS.ShowMeter(" and Main.Valid=1 and Main.MeterNumber='" + item.MeterNumber + "'");
                        var customerid = MeterInfo.CustomerID;
                        vee.Vee207data(Convert.ToDecimal(item.MeterId),item.MeterNumber, customerid);
                        changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message98);
                    }
                    str = str + item.MeterNumber + ";";

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
         

        

        string GetConsumedDate(string readDate, int index)
        {
            
            var dt = RsaDateTime.PersianDateTime.ConvertToGeorgianDateTime(readDate);
            var dt1 = dt.AddMonths(-1 * index);
            var dt2 = dt1.ToPersianString();

            var data = dt2.Split(new char[] { '/', ' ', ':' });
            int year = Convert.ToInt32(data[0]);
            int month = Convert.ToInt32(data[1]);
            int day = Convert.ToInt32(data[2]);
             

            return $"{year}/{month.ToString("00")}/01";
        }

      
      
        

        
        private void mnuSoftwareSwtting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 25);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[25])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[25] = true;
                        Setting setting = new Setting();
                        setting.Title = translateWindowName.TranslateofLable.Object25;
                        setting.Owner = this;
                        setting.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = setting.Title;
                        theTabItem.windowname = setting;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        setting.TabPag = theTabItem;
                        setting.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);

            }
        }

        private void mnuSmartCardReader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 26);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[26])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[26] = true;
                        ImportDataFromSmartCardReader DataFromSmartCardReader = new ImportDataFromSmartCardReader();
                        DataFromSmartCardReader.Title = translateWindowName.TranslateofLable.Object26;
                        DataFromSmartCardReader.Owner = this;
                        DataFromSmartCardReader.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = DataFromSmartCardReader.Title;
                        theTabItem.windowname = DataFromSmartCardReader;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        DataFromSmartCardReader.TabPag = theTabItem;
                        DataFromSmartCardReader.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);

            }
        }

        private void Isvisable_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ISchecked = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }


        private void selectall()
        {
            try
            {
                if (!SelectAll)
                {
                    for (int i = 0; i < MeterGrid.Items.Count; i++)
                    {
                        SelectedMeter UsG = new SelectedMeter();
                        UsG = (SelectedMeter)MeterGrid.Items[i];
                        ChangelstMeter(true, UsG.MeterId);
                    }
                    SelectAll = true;
                }
                else
                {
                    for (int i = 0; i < MeterGrid.Items.Count; i++)
                    {
                        SelectedMeter UsG = new SelectedMeter();
                        UsG = (SelectedMeter)MeterGrid.Items[i];
                        ChangelstMeter(false, UsG.MeterId);
                    }
                    SelectAll = false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            selectall();
        }

        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        
        private void mnuexit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        About about;
        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 43);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[43])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[43] = true;
                         about = new About();
                        about.Title = translateWindowName.TranslateofLable.Object43;
                        about.Owner = this;
                        about.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = about.Title;
                        theTabItem.windowname = about;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        //if (CommonData.IsExpire)                        
                        //    about.btnExpirationDate.IsEnabled = true;
                        //else
                        //    about.btnExpirationDate.IsEnabled = false;
                        
                        about.TabPag = theTabItem;
                        about.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);

            }
        }

        private void mnuMeterError_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 44);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[44])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[44] = true;
                        ErrorControlForSingleMeter ErrorForSelectedMeter = new ErrorControlForSingleMeter();
                        ErrorForSelectedMeter.Title = translateWindowName.TranslateofLable.Object44;
                        ErrorForSelectedMeter.Owner = this;
                        ErrorForSelectedMeter.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = ErrorForSelectedMeter.Title;
                        theTabItem.windowname = ErrorForSelectedMeter;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        ErrorForSelectedMeter.TabPag = theTabItem;
                        ErrorForSelectedMeter.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);

            }
        }

        private void mnudatafromDLMSClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 42);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[42])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[42] = true;
                        ImportDataFromDLMSClient importdatafomDLMSClient = new ImportDataFromDLMSClient();
                        importdatafomDLMSClient.Title = translateWindowName.TranslateofLable.Object42;
                        importdatafomDLMSClient.Owner = this;
                        importdatafomDLMSClient.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = importdatafomDLMSClient.Title;
                        theTabItem.windowname = importdatafomDLMSClient;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        importdatafomDLMSClient.TabPag = theTabItem;
                        importdatafomDLMSClient.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);

            }
        }

        private void CreditTocard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 45);
                if (us.CanShow)
                {
                    if (CommonData.carddataReceive != null)
                    {
                        CommonData.carddataReceive.Close();
                    }
                    ReadFromCard = false;
                    ReadCard(1);

                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }

        }

        private void MnuRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshSelectedMeters("");
                RefreshCmbGroups(); // i insert
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void mnuLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Window childwindow;
                List<System.Windows.Window> lstwindow = new List<System.Windows.Window>();
                foreach (var item in tabControl1.Items)
                {

                    ClosableTab t = (ClosableTab)item;
                    childwindow = t.windowname;
                    lstwindow.Add(childwindow);
                }
                for (int i = 0; i < lstwindow.Count; i++)
                {
                    childwindow = lstwindow[i];
                    childwindow.Close();
                }
                Login login = new Login();
                login.ShowDialog();
                RefreshCmbGroups();

            }
            catch (Exception ex)
            {
                if (ex.ToString().ToLower().Contains("sql"))
                {
                    //System.Windows.Forms.MessageBox.Show("در حال حاضر امکان دسترسی به پایگاه داده مورد نظر امکان ندارد");
                    //MeterGrid.ItemsSource = null;
                    throw ex;

                }
                else
                {
                    MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                }
            }

        }

        private void MeterGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            string rowcount = "";
            if (CommonData.mainwindow != null)
                rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
            lblcount.Content = rowcount + "=" + MeterGrid.Items.Count;
        }

        private void window_ContentRendered(object sender, EventArgs e)
        {
            RefreshCmbDataBaseInfo();
            RefreshCmbGroups();
        }

        private void mnudatafromSabaCandH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                us = CommonData.ShowButtonBinding("", 46);
                if (us.CanShow)
                {
                    if (!ClassControl.OpenWin[46])
                    {
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        ClassControl.OpenWin[46] = true;
                        ImportDataFromSabaCandH importdatafomSabaCandH = new ImportDataFromSabaCandH();
                        importdatafomSabaCandH.Title = translateWindowName.TranslateofLable.Object46;
                        importdatafomSabaCandH.Owner = this;
                        importdatafomSabaCandH.Tab = tabControl1;
                        ClosableTab theTabItem = new ClosableTab();
                        theTabItem.Title = importdatafomSabaCandH.Title;
                        theTabItem.windowname = importdatafomSabaCandH;
                        tabControl1.Items.Add(theTabItem);
                        theTabItem.Focus();
                        importdatafomSabaCandH.TabPag = theTabItem;
                        importdatafomSabaCandH.Show();
                    }
                }
                else
                {
                    MessageBox.Show(tm.TranslateofMessage.Message55);
                    return;
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);

            }
        }

        string lastSelectedDataBase = "";
        bool firstLogin = true;
        private void CmbDataBaseInfo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (firstLogin) return;
            
            string dataSourceName = SabaNewEntities.DataSourceName;
            string dataBaseName = SabaNewEntities.DatabaseName;
            string dataBaseUser = SabaNewEntities.username;
            string dataBasePass = SabaNewEntities.pass;
            string dataBaseSecurity = SabaNewEntities.security;
            try
            {
                if (CmbDataBaseInfo.SelectedItem != null)
                {
                    foreach (var item in _dataBasesInfo._lstShowDataBasesInfo)
                    {
                        if (item.Name.Equals(CmbDataBaseInfo.SelectedItem))
                        {
                            SabaNewEntities.DataSourceName = item.DataBaseIP;
                            SabaNewEntities.DatabaseName = item.DataBaseName;
                            SabaNewEntities.Name = item.Name;
                            SabaNewEntities.username = item.DataBaseUser;
                            SabaNewEntities.pass = item.DataBasePass;
                            SabaNewEntities.security = "persist security";
                            if (lastSelectedDataBase != "" && lastSelectedDataBase != CmbDataBaseInfo.SelectedItem.ToString())
                            {
                                //InitiateNewDataBase();
                                //
                            }
                            
                            lastSelectedDataBase = CmbDataBaseInfo.SelectedItem.ToString();
                            MnuRefresh_Click(null,null);
                            RefreshSelectedMeters("");
                            RefreshCmbGroups();
                            var   oWaitingForm = new WaitingForm();
                            oWaitingForm.Show();
                            oWaitingForm.Close();
                            return;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string msg = " در دسترس نیست";
                msg += " " + CmbDataBaseInfo.SelectedItem + " ";
                msg += " در حال حاضر پایگاه داده ";


                System.Windows.Forms.MessageBox.Show(msg, "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.RightAlign);
                //MeterGrid.ItemsSource = null;
                SabaNewEntities.DataSourceName = dataSourceName;
                SabaNewEntities.DatabaseName = dataBaseName;
                SabaNewEntities.username = dataBaseUser;
                SabaNewEntities.pass = dataBasePass;
                SabaNewEntities.security = dataBaseSecurity;
                CmbDataBaseInfo.SelectedItem = lastSelectedDataBase;
                //RefreshSelectedMeters("");
                //RefreshCmbGroups();
                //CmbDataBaseInfo.SelectedItem = lastSelectedDataBase;
            }
        }

        private void ReadHHuFile_click(object sender, RoutedEventArgs e)
        {

            try
            {
                th = new Thread(delegate () { ReadHhuFile(); });
                th.SetApartmentState(ApartmentState.STA);
                th.IsBackground = true;
                th.Start();
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }

        }

        private void ReadHHu303_click(object sender, RoutedEventArgs e)
        {
             
                try
                {
                    us = CommonData.ShowButtonBinding("", 19);
                    if (us.CanShow)
                    {
                        if (CommonData.HHUDataReceive != null)
                        {
                            CommonData.HHUDataReceive.Close();
                        }
                        ReadFromHHU = false;
                        ReadHhu303();

                    }
                    else
                    {
                        MessageBox.Show(tm.TranslateofMessage.Message55);
                        return;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                }
             
        }

        private void mnuDataBaseBackup_Click(object sender, RoutedEventArgs e)
        {

            

            SaveFileDialog s1 = new SaveFileDialog();
            s1.Title = "save database backup";
            s1.Filter = "backup file |.bak";
            SabaNewEntities Bank = new SabaNewEntities();
            string connectionString = Bank.Database.Connection.ConnectionString;
            var backupFolder = ConfigurationManager.AppSettings["BackupFolder"];

            var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);
            s1.FileName = String.Format("{0}{1}.bak", sqlConStrBuilder.InitialCatalog,           DateTime.Now.ToString("yyyy-MM-dd"));
            if (s1.ShowDialog() == true)
            {
                BackUpDataBase(s1.FileName,true);
            }            
        }
        public void BackupDatabase(string databaseName)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                SqlConnection con = new SqlConnection();
                SqlCommand sqlcmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                SaveFileDialog dlg = new SaveFileDialog();

                dlg.FileName = "Saba_OldDb_" + DateTime.Now.ToString("yyyy-MM-dd HHmmss");
                dlg.DefaultExt = ".Bak'";
                //dlg.Filter = " (.Bak)|*.Bak"; // Filter files by extension 

                Nullable<bool> result = dlg.ShowDialog();
                //if (result == true)
                //{
                string path = dlg.FileName;//System.Environment.CurrentDirectory;
                if (path.StartsWith("C:"))
                {
                    MessageBox.Show("انتخاب نمایید(C:)لطفا مسیری غیراز");
                    result = dlg.ShowDialog();
                    path = dlg.FileName;
                    //if (result==false)
                    //{
                    //    return;
                    //}
                }
                con.ConnectionString = Bank.Database.Connection.ConnectionString;
                string backupDIR = "~/BackupDB";


                try
                {

                    con.Open();

                    sqlcmd = new SqlCommand("backup database " + databaseName + " to disk='" + dlg.FileName, con);
                    sqlcmd.ExecuteNonQuery();
                    con.Close();


                }
                catch (Exception ex)
                {

                }
                //}
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private string BuildBackupPathWithFilename(string databaseName)
        {
            string filename = string.Format("{0}-{1}.bak", databaseName, DateTime.Now.ToString("yyyy-MM-dd"));
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return Path.Combine(currentDirectory, filename);
        }

        void ShowProgressBarTag(string text, bool isError)
        {
            Thread th = new Thread(delegate () { changeProgressBarTag(text); });
            th.SetApartmentState(ApartmentState.STA);
            th.IsBackground = true;
            th.Start();

            Thread th1 = new Thread(delegate () { GetProgressBarBackGround(isError); });
            th1.SetApartmentState(ApartmentState.STA);
            th1.IsBackground = true;
            th1.Start();

        }
        private void BackUpDataBase(string path,bool isFulllPath) 
        {
            try
            {
                ShowProgressBarTag("  ",false);
                Thread.Sleep(50);
                SabaNewEntities Bank = new SabaNewEntities();
                string connectionString = Bank.Database.Connection.ConnectionString;
                var backupFolder = ConfigurationManager.AppSettings["BackupFolder"];

                var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);
                var fileName = path;
                if (!isFulllPath)
                    fileName = path + @"\"+ sqlConStrBuilder .InitialCatalog+ ".bak";
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    var query = String.Format("BACKUP DATABASE {0} TO DISK='{1}'",
                        sqlConStrBuilder.InitialCatalog, fileName);

                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                ShowProgressBarTag("ذخیره سازی نسخه پشتیبان در آدرس موردنظر با موفقیت انجام شد  ", false);

            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains(" Operating system error 5(Access is denied.)"))
                {
                    System.Windows.Forms.MessageBox.Show("لطفا آدرس ذخیره سازی نسخه پشتیبان را تغییر دهید و یا اینکه مجوز دسترسی به آدرس مورد نظر را برای نرم افزار   ");
                    ShowProgressBarTag("لطفا آدرس ذخیره سازی نسخه پشتیبان را تغییر دهید و یا اینکه مجوز دسترسی به آدرس مورد نظر را برای نرم افزار   ", true);                    
                }
                else  if (ex.ToString().ToLower().Contains("sql"))
                {
                    System.Windows.Forms.MessageBox.Show(" خطا در ذخیره سازی فایل پشتیبان   ");
                    ShowProgressBarTag(" خطا در ذخیره سازی فایل پشتیبان   ", true);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(" خطا در تهیه فایل پشتیبان   ");
                    ShowProgressBarTag(" خطا در تهیه فایل پشتیبان   ", true);                    
                }

                CommonData.WriteLOG(ex);
            }            
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show(" تمایل به ذخیره نسخه پشتیبان از اطلاعات نرم افزار دارید؟", "تهیه نسخه پشتیبان", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            { 
                changeProgressBarTag("لطفا تا پایان ذخیره سازی فایل پشتیبان منتظر بمانید"); Thread.Sleep(100);
                var baseAddress = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

                 
                //string path = baseAddress + @"\SabaCandH_" + DateTime.Now.ToPersianDateString().Replace("/", "_") + ".bak";
                string path = baseAddress ;
                
                BackUpDataBase(path,false);               
            }
        }

        

        /////////////////////////////   Check Meter For Customer and Group     \\\\\\\\\\\\\\\\\\\\\\\\\\\\
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
                if (Convert.ToBoolean(IsAllow.Value))
                {
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
                }
                else
                    return "NO Allowed";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                return "False";
            }
        }

    }
}