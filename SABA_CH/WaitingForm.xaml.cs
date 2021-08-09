using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using SABA_CH.DataBase;
using SABA_CH.Global;
using SABA_CH.VEEClasses;

namespace SABA_CH
{
    /// <summary>
    /// Interaction logic for WaitingForm.xaml
    /// </summary>
    public partial class WaitingForm : System.Windows.Window
    {

        public ShowTranslateofMessage tm = null;
        ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
        ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
        ObjectParameter ID = new ObjectParameter("ID", 10000000000000000);
        public new SabaNewEntities Bank;
        private double counter = 0;
        private static EventWaitHandle _waitHandle;
        public WaitingForm()
        {
            InitializeComponent(); 
            Thread th = new Thread(SetDataBase);
            th.IsBackground = true;
            th.Start();
        }



        public void SetDataBase()
        {
            _waitHandle = new AutoResetEvent(false);
            tm = CommonData.translateMessage();
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            Thread th = new Thread(UpdateDataBase);
            th.IsBackground = true;
            th.Start();
            _waitHandle.WaitOne();
        }

        
        private void UpdateDataBase()
        {
            try
            {
                this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
                CommonData.WaitingFormRun = true;
                CheckDatabaseVersion();
                CommonData.WaitingFormRun = false;
                _waitHandle.Set();

            }
            catch (Exception)
            {
                // MessageBox.Show(" خطا در بروزرسانی دیتابیس لطفا با شرکت رهروان سپهر اندیشه تماس بگیرید");
            }
            
        }

         
        public void CheckDatabaseVersion()
        {
            try
            {
                //VeeMeterData();
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();

                //Corect ConsumedWater readDate
                Bank.EditeConsumedWater(false);
                Bank.EditeConsumedWater(true);



                string Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                CommonData.ProgramVersion = Assembly.GetEntryAssembly().GetName().Version.ToString(); // AppDomain.CurrentDomain.DomainManager.EntryAssembly.GetName().Version.ToString();                                        
                bool existInDb = false;

                foreach (var item in Bank.ShowChangeDB(""))
                {
                    if (item.SoftwareVersion == CommonData.ProgramVersion)
                    {
                        existInDb = true;
                    }
                }
                if (!existInDb)
                {
                    CommonData.mainwindow.ChangeEnable(false, "لطفا تا پایان بررسی اطلاعات دیتابیس منتظر بمانید ");
                    Scripts s = new Scripts();
                    s.RunScripts(Bank.Database.Connection.ConnectionString);
                    VeeMeterData();
                    Bank.InsChangeDB(Date, CommonData.ProgramVersion, ID, Result, ErrMSG);
                    CommonData.mainwindow.ChangeEnable(true, "");
                }
                Bank.Database.Connection.Close();
            }
            catch (Exception ex)
            {
                string currentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string archiveFolder = System.IO.Path.Combine(currentDirectory, "Error In Sps.txt");
                File.WriteAllText(archiveFolder, " Error in : InsertVersionSoftwaer :" + "\r\n");
                Bank.Database.Connection.Close();
                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }            
        }
 
        void VeeMeterData()
        {
            try
            {
                //MeterGrid.ItemsSource = null;
                SabaNewEntities Bank = new SabaNewEntities();
                ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 8000;
                VeeMeterData vee = new VeeMeterData();
                Bank.Database.Connection.Open();
                
                foreach (ShowMeter_Result item in Bank.ShowMeter("", CommonData.UserID))
                {
                    CommonData.mainwindow.ChangeEnable(false, "لطفا تا پایان بررسی اطلاعات دیتابیس منتظر بمانید ");
                    var meterNo = item.MeterNumber;

                    //20726473
                    //if (meterNo == "1939300020735")
                    {
                        try
                        {
                            if (item.MeterNumber.StartsWith("207"))
                            {
                                vee.Vee207data(Convert.ToDecimal(item.MeterID), item.MeterNumber, item.CustomerID);
                            }
                            else
                                vee.Vee303data(Convert.ToDecimal(item.MeterID), item.MeterNumber, item.CustomerID, item.Softversion);

                        }
                        catch (Exception ex)
                        {

                        }                        
                    }
                }
                Bank.Database.Connection.Close();
                Bank.Dispose();
                
            }
            catch (Exception ex)
            {
            }
            CommonData.mainwindow.ChangeEnable(true,"");
        }
        
        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            //MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        public static void Register(FrameworkElement root)
        {
            root.IsHitTestVisible = false;
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Register(this);
        }
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            Register(this);
        }
    }
}