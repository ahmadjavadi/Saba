using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ErikEJ.SqlCeScripting;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for ImportDataFromSQLCe.xaml
    /// </summary>
    public partial class ImportDataFromSQLCe : System.Windows.Window
    {
        public ShowTranslateofLable tr = null;
        public readonly int windowID = 20;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public ShowGroups_Result Selectedgroup;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
        string FilePath = "";
        string connectionString = "";
        public ImportDataFromSQLCe()
        {
            InitializeComponent();
            changeFlowDirection();
            translateWindows();
            RefreshCmbGroups();
        }
        public void translateWindows()
        {
            try
            {
                tr = CommonData.translateWindow(windowID);
                GridMain.DataContext = tr.TranslateofLable;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void changeFlowDirection()
        {
            try
            {
                //GridMain.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtpath.Text == "")
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message69);          
                    return;
                }
                if (txtpath.Text != "")
                {
                    FilePath = txtpath.Text;
                    if (FilePath.Contains(".sdf"))
                    {

                        connectionString = ConfigurationManager.ConnectionStrings["SabaNewEntities"].ConnectionString.ToString();
                        int i = connectionString.ToLower().IndexOf("data source");
                        int j = connectionString.LastIndexOf(";");
                        connectionString = connectionString.Substring(i, j - i + 1);
                        CommonData.mainwindow.changeProgressBar_MaximumValue(5);
                        CommonData.mainwindow.changeProgressBarTag("");
                        CommonData.mainwindow.changeProgressBarValue(0);
                        Thread th = new Thread(export);
                        th.IsBackground = true;
                        th.Start();
                    }
                    else
                    {
                        MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message63);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex); CommonData.WriteLOG(ex);
            }

        }
        public void export()
        {
            try
            {
                int i = 0;
                changeEnable(false);
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);
                MessageBox.Show(" نمایید"+"OK" + "برای ادامه دریافت اطلاعات");//CommonData.mainwindow.tm.TranslateofMessage.Message41);
                CommonData.mainwindow.changeProgressBar_MaximumValue(11);
                i = CreateShema(FilePath, connectionString);
                CommonData.mainwindow.changeProgressBarValue(1);
                if (i == 0)
                    i = CReateDataScript(FilePath, connectionString);
                else
                {
                    changeEnable(true);
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(0);
                    return;
                }
                if (i == 0)
                {
                    CommonData.mainwindow.changeProgressBarValue(1);
                    CReateDB();
                    CommonData.mainwindow.changeProgressBarValue(1);
                    Thread.Sleep(30000);
                    ExecuteDataScript();
                    CommonData.mainwindow.changeProgressBarValue(1);
                    ExecuteExportScript();
                    CommonData.mainwindow.changeProgressBarValue(1);
                    CommonData.mainwindow.VeeListoFMeter();
                    CommonData.mainwindow.changeProgressBarValue(1);
                    ObjectParameter Resul = new ObjectParameter("Result", 10000000000);
                    ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                    decimal? result = 1000000;
                    string errMSG = "";
                    SQLSPS.ExportMeterError207(result, errMSG);
                    CommonData.mainwindow.changeProgressBarValue(1);
                    SQLSPS.DropTableFromSQLCompact(Resul, ErrMSG);
                    CommonData.mainwindow.changeProgressBarValue(1);
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(1);
                    changeEnable(true);
                    CommonData.mainwindow.changeProgressBarValue(1);
                    string Filter = "";
                    if (CommonData.mainwindow.SelectedGroup.GroupID != -1)
                        Filter = "and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + CommonData.mainwindow.SelectedGroup.GroupID + "and  GroupType=" + CommonData.mainwindow.SelectedGroup.GroupType + ") ";

                    CommonData.mainwindow.RefreshSelectedMeters(Filter);
                    DeleteDataScript();
                    CommonData.mainwindow.changeProgressBarValue(0);
                    CommonData.mainwindow.changeProgressBarTag("");
                }
                else
                {
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(0);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex); CommonData.WriteLOG(ex);
            }
        }
        public int dosomthing(string[] args)
        {
            try
            {
                string connectionString = args[0];
                string outputFileLocation = args[1];

                bool includeData = true;
                bool includeDataForServer = false;
                bool includeSchema = true;
                bool saveImageFiles = false;
                bool sqlAzure = false;
                bool sqlite = false;
                List<string> exclusions = new List<string>();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                if (args[0].Equals("diff", StringComparison.OrdinalIgnoreCase))
                {
#if V31
                        PrintUsageGuide();
                        return 2;                        
#else
                    if (args.Length == 4)
                    {
                        using (var source = Helper.CreateRepository(args[1]))
                        {
                            using (var target = Helper.CreateRepository(args[2]))
                            {
                                var generator = Helper.CreateGenerator(source);
                                SqlCeDiff.CreateDiffScript(source, target, generator, false);
                                File.WriteAllText(args[3], generator.GeneratedScript);
                                return 0;
                            }
                        }
                    }
                    else
                    {
                        //PrintUsageGuide();
                        return 0;
                    }
#endif
                }
                else if (args[0].Equals("dgml", StringComparison.OrdinalIgnoreCase))
                {
#if V31
                        PrintUsageGuide();
                        return 2;
#endif
                    if (args.Length == 3)
                    {
                        using (var source = Helper.CreateRepository(args[1]))
                        {
                            var generator = Helper.CreateGenerator(source, args[2]);
                            generator.GenerateSchemaGraph(args[1]);
                        }
                        return 0;
                    }
                    else
                    {
                        //PrintUsageGuide();
                        return 0;
                    }
                }
                else if (args[0].Equals("wpdc", StringComparison.OrdinalIgnoreCase))
                {
#if V31
                        PrintUsageGuide();
                        return 2;
#endif
#if V40
                        PrintUsageGuide();
                        return 2;
#else
                    if (args.Length == 3)
                    {
                        using (var repo = Helper.CreateRepository(args[1]))
                        {
                            var dch = new DataContextHelper();
                            dch.GenerateWPDataContext(repo, args[1], args[2]);
                        }
                        return 0;
                    }
                    else
                    {
                        //PrintUsageGuide();
                        return 0;
                    }
#endif
                }

                else
                {
                    for (int i = 2; i < args.Length; i++)
                    {
                        if (args[i].Contains("dataonly"))
                        {
                            includeData = true;
                            includeSchema = false;
                        }
                        if (args[i].Contains("dataonlyserver"))
                        {
                            includeData = true;
                            includeDataForServer = true;
                            includeSchema = false;
                        }
                        if (args[i].Contains("schemaonly"))
                        {
                            includeData = false;
                            includeSchema = true;
                        }
                        if (args[i].Contains("saveimages"))
                            saveImageFiles = true;
                        if (args[i].Contains("sqlazure"))
                            sqlAzure = true;
                        if (args[i].Contains("sqlite"))
                            sqlite = true;
                        if (args[i].StartsWith("exclude:"))
                            ParseExclusions(exclusions, args[i]);
                    }

                    using (IRepository repository = Helper.CreateRepository(connectionString))
                    {
                        CommonData.mainwindow.changeProgressBarTag("Initializing....");
                        Helper.FinalFiles = outputFileLocation;
#if V40
                            var generator = new Generator4(repository, outputFileLocation, sqlAzure, false, sqlite);
#else
                        var generator = new Generator(repository, outputFileLocation, sqlAzure, false, sqlite);
#endif
                        generator.ExcludeTables(exclusions);
                        CommonData.mainwindow.changeProgressBarTag("Generating the tables....");
                        if (includeSchema)
                        {
#if V31
                                generator.GenerateTable(false);
#else
                            generator.GenerateTable(includeData);
#endif
                        }
                        if (sqlite)
                        {
                            CommonData.mainwindow.changeProgressBarTag("Generating the data....");
                            generator.GenerateTableContent(false);
                            CommonData.mainwindow.changeProgressBarTag("Generating the indexes....");
                            generator.GenerateIndex();
                        }
                        else
                        {

                            if (sqlAzure && includeSchema)
                            {
                                CommonData.mainwindow.changeProgressBarTag("Generating the primary keys (SQL Azure)....");
                                generator.GeneratePrimaryKeys();
                            }
                            if (includeData)
                            {
                                CommonData.mainwindow.changeProgressBarTag("Generating the data....");
                                generator.GenerateTableContent(saveImageFiles);
                                if (!includeSchema) // ie. DataOnly
                                {
                                    CommonData.mainwindow.changeProgressBarTag("Generating IDENTITY reset statements....");
                                    // generator.GenerateIdentityResets(includeDataForServer);
                                }
                            }
                            if (!sqlAzure && includeSchema)
                            {
                                CommonData.mainwindow.changeProgressBarTag("Generating the primary keys....");
                                generator.GeneratePrimaryKeys();
                            }
                            if (includeSchema)
                            {
                                CommonData.mainwindow.changeProgressBarTag("Generating the indexes....");
                                generator.GenerateIndex();
                                CommonData.mainwindow.changeProgressBarTag("Generating the foreign keys....");
                                generator.GenerateForeignKeys();
                            }
                        }
                        CommonData.mainwindow.changeProgressBarTag("write in file....");

                        Helper.WriteIntoFile(generator.GeneratedScript, outputFileLocation, generator.FileCounter, sqlite);
                        CommonData.mainwindow.changeProgressBarTag("writed in file....");
                    }
                    //CommonData.mainwindow.changeProgressBarTag("Sent script to output file(s) : {0} in {1} ms", Helper.FinalFiles, (sw.ElapsedMilliseconds).ToString());
                    return 0;
                }
            }
            catch (SqlCeException ed)
            {
                MessageBox.Show(Helper.ShowErrors(ed));
                return -1;
            }
            catch (SqlException es)
            {
                MessageBox.Show(Helper.ShowErrors(es));
                return -1;
            }

            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarTag("Error: " + ex);
                return -1;
            }
        }

        private static void ParseExclusions(List<string> exclusions, string excludeParam)
        {
            excludeParam = excludeParam.Replace("exclude:", string.Empty);
            if (!string.IsNullOrEmpty(excludeParam))
            {
                string[] tables = excludeParam.Split(',');
                foreach (var item in tables)
                {
                    exclusions.Add(item);
                }
            }
        }
        
        public int CreateShema(string Filepath, string ConnectString)
        {
            try
            {
                int i = 0;
                string[] args = new string[4];
                string path = AppDomain.CurrentDomain.BaseDirectory;
                args[0] = "diff";
                args[1] = " Data Source =" + Filepath + ";Password =\"1234\";";
                args[2] = ConnectString;//"Data Source=.;Initial Catalog=SabaCandH;Persist Security Info=True;User ID=sa;Password=88102351-7";

                args[3] = path + "schemaonly.sql";
                if (args.Length < 2 || args.Length > 6)
                {
                    //PrintUsageGuide();
                    return -1;
                }
                else
                {
                    i = dosomthing(args);
                    CommonData.mainwindow.changeProgressBarValue(1);
                    CommonData.mainwindow.changeProgressBarTag("Create Schema");

                }
                return i;
            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                return -1;
            }
        }
        
        public int CReateDataScript(string Filepath, string Connectionstring)
        {

            try
            {
                int i = 0;
                string[] args = new string[4];
                args[0] = " Data Source =" + Filepath + ";Password =\"1234\";";
                string path = AppDomain.CurrentDomain.BaseDirectory;
                args[1] = path + "data.sql";
                args[2] = Connectionstring;
                //"Data Source=.;Initial Catalog=SabaCandH;Persist Security Info=True;User ID=sa;Password=88102351-7";
                //args[3] = "exclude:";
                //args[2]="Northwind.sql";
                args[3] = "dataonly";

                if (args.Length < 2 || args.Length > 6)
                {
                    //PrintUsageGuide();
                    return 0;
                }
                else
                {
                    i = dosomthing(args);
                    if (i == 0)
                    {
                        CommonData.mainwindow.changeProgressBarValue(1);
                        CommonData.mainwindow.changeProgressBarTag("Create data");
                    }

                }
                return i;
            }

            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                return -1;
            }
        }
        
        public void CReateDB()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "schemaonly.sql";

                string script = File.ReadAllText(path);
                SabaNewEntities Bank = new SabaNewEntities();
                script = script.Replace("GO", " ");
                int startid = script.IndexOf("[MainTableID] int IDENTITY");
                int endid = script.IndexOf(", [", startid);
                string sub = script.Substring(startid, endid - startid);
                script = script.Replace(sub, "[MainTableID] int  NOT NULL");
                Bank.Database.Connection.Open();
                Bank.execsabaschema(script);
                CommonData.mainwindow.changeProgressBarValue(1);
                CommonData.mainwindow.changeProgressBarTag("db Created");
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        public void ExecuteDataScript()
        {
            string filename = "";
            SabaNewEntities Bank = new SabaNewEntities();
            try
            {
                CommonData.mainwindow.changeProgressBarTag("restoring Data ...");
                Bank.Database.Connection.Open();
                ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = int.MaxValue;
                foreach (string command in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.sql"))
                {
                    if (!command.Contains("SQLChanges"))
                    {
                        if (command.Contains("data"))
                        {
                            filename = command;
                            string script = File.ReadAllText(command);
                            script = script.Replace("GO", " ");
                            script = script.Replace("SET IDENTITY_INSERT [MainTable] ON;", " ");
                            script = script.Replace("SET IDENTITY_INSERT [MainTable] OFF;", " ");
                            //Bank.execsabaschema(script);
                            string s = Bank.Database.Connection.ConnectionString;
                            try
                            {
                                SqlConnection conn = new SqlConnection(s);
                                conn.Open();

                                SqlCommand cmd = new SqlCommand(script, conn);
                                cmd.CommandTimeout = int.MaxValue;
                                cmd.ExecuteNonQuery();

                                conn.Close();
                                conn.Dispose();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    CommonData.mainwindow.changeProgressBarValue(1);
                    CommonData.mainwindow.changeProgressBarTag("Data Restored");
                }
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
                MessageBox.Show(ex.ToString() + "textfile:" + filename); CommonData.WriteLOG(ex);
            }
        }

        public void DeleteDataScript()
        {
            SabaNewEntities Bank = new SabaNewEntities();
            try
            {

                Bank.Database.Connection.Open();
                foreach (string command in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.sql"))
                {
                    if (command.Contains("data"))
                    {
                        File.Delete(command);

                    }

                }
                CommonData.mainwindow.changeProgressBarValue(1);


            }
            catch (Exception ex)
            {

                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        
        public void ExecuteExportScript()
        {
            SabaNewEntities Bank = new SabaNewEntities();
            ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
            ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
            try
            {
                Bank.Database.Connection.Open();
                ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = int.MaxValue;
                Bank.ExportSaba2SabaCandH(Convert.ToDecimal(Selectedgroup.GroupID), Convert.ToInt32(Selectedgroup.GroupType), Result, ErrMSG);
                CommonData.mainwindow.changeProgressBarValue(2);
                CommonData.mainwindow.changeProgressBarTag("Data Coverted");
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                Bank.Database.Connection.Close();
                MessageBox.Show(ErrMSG.Value.ToString());
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        
        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                tabCtrl.SelectedItem = tabPag;
                if (!tabCtrl.IsVisible)
                {
                    tabCtrl.Visibility = Visibility.Visible;
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
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.DefaultExt = ".sdf"; // Default file extension
                dlg.Filter = "SQLCE files (*.sdf)|*.sdf|All files (*.*)|*.*";
                Nullable<bool> result = dlg.ShowDialog();
                string FilePath = dlg.FileName;
                txtpath.Text = FilePath;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        
        public void RefreshCmbGroups()
        {
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
                ShowGroups Groups = new ShowGroups(filter, 0, CommonData.LanguagesID);
                CmbGroups.ItemsSource = Groups.CollectShowGroups;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        
        private void changeEnable(bool IsEnable)
        {
            try
            {
                btnOk.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
               delegate()
               {
                   btnOk.IsEnabled = IsEnable;
               }));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void CmbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CmbGroups.SelectedItem != null)
                {
                    Selectedgroup = (ShowGroups_Result)CmbGroups.SelectedItem;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
    }
}