using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace SABA_CH
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            
                
            //var d = System.IO.File.ReadAllText("e:\\updateMeter.sql");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            //culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //culture.DateTimeFormat.LongTimePattern = "dd/MM/yyyy HH:mm:ss";
            //Thread.CurrentThread.CurrentCulture = culture;
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;

            EncryptConnString();

        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.ToString());
            //MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // EncryptConnString();
        }


        private void EncryptConnString()
        {
            try
            {
                System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(System.AppDomain.CurrentDomain.BaseDirectory + "Saba.exe");

                ConfigurationSection connectionStringSection = config.GetSection("connectionStrings");
                if (connectionStringSection != null)
                {
                    if (!connectionStringSection.SectionInformation.IsProtected)
                    {
                        connectionStringSection.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                        connectionStringSection.SectionInformation.ForceSave = true;
                        config.Save(ConfigurationSaveMode.Modified);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DecryptConnString()
        {
            System.Configuration.Configuration config =
                ConfigurationManager.OpenExeConfiguration(System.AppDomain.CurrentDomain.BaseDirectory + "\\HHUPC.exe");
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                config.Save();
            }
        }
    }
}