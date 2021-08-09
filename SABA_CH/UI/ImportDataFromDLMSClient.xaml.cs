using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for ImportDataFromDLMSClient.xaml
    /// </summary>
    public partial class ImportDataFromDLMSClient : System.Windows.Window
    {
        public ShowTranslateofLable tr = null;
        public readonly int windowID = 42;
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
        public ImportDataFromDLMSClient()
        {
            InitializeComponent();
            translateWindows();
            RefreshCmbGroups();
        }
        
       
        public void ConvertDataFromDLMS()
        {
            try
            {
                 string path = AppDomain.CurrentDomain.BaseDirectory;                

                 DLMS.DLMS dlms = new DLMS.DLMS();
                 dlms.GetDataFromDLMSClient(txtpath.Text);
                 string BasicVersion = "";
                 string SEWM303 = "";
                 BasicVersion = dlms.UnZip(path + @"\sewm\BasicVersion.olf");
                 SEWM303 = dlms.UnZip(path + @"\sewm\SEWM303.olf");
                 dlms = null;
                 changeBasicVersionFiles(BasicVersion);
                 changeSEWMFiles(SEWM303);
                 SQLSPS.ImportdataFromDlmsClient(CommonData.UserID, Selectedgroup.GroupID, Selectedgroup.GroupType, CommonData.LanguagesID, path + "data.xml", BasicVersion, SEWM303);
                 AfterImport(BasicVersion, SEWM303);



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void changeBasicVersionFiles(string BasicVersion)
        {
            try
            {
                string script = File.ReadAllText(BasicVersion);
                script = script.Replace("MeterDefinition.", "");
                int startid = script.IndexOf("<MeterDefinition");
                int endid = script.IndexOf(">", startid);
                string sub = script.Substring(startid, endid - startid);
                script = script.Replace(sub, "<MeterDefinition ");
                script = script.Replace("InterfaceClassDescriptor.", "");
                File.Delete(BasicVersion);
                File.WriteAllText(BasicVersion, script);
            }
            catch(Exception ex)
            {

            }
        }

        public void changeSEWMFiles(string SEWMPath)
        {
            try
            {
                string script = File.ReadAllText(SEWMPath);
                script = script.Replace("MeterDefinition.", "");
                int startid = script.IndexOf("<MeterDefinition");
                int endid = script.IndexOf(">", startid);
                string sub = script.Substring(startid, endid - startid);
                script = script.Replace(sub, "<MeterDefinition ");
                File.Delete(SEWMPath);
                File.WriteAllText(SEWMPath, script);
            }
            catch (Exception ex)
            {

            }
        }
        public void DeleteFiles(string Basicpath, string SEWMPath)
        {
            try
            {
                File.Delete(Basicpath);
                File.Delete(SEWMPath);

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void AfterImport(string Basicpath, string SEWMPath)
        {
            try
            {
                    CommonData.mainwindow.changeProgressBarValue(1);
                    string Filter = "";
                    if (CommonData.mainwindow.SelectedGroup.GroupID != -1)
                        Filter = "and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + CommonData.mainwindow.SelectedGroup.GroupID + "and  GroupType=" + CommonData.mainwindow.SelectedGroup.GroupType + ") ";

                    CommonData.mainwindow.RefreshSelectedMeters(Filter);
                    DeleteFiles(Basicpath, SEWMPath);
                    CommonData.mainwindow.changeProgressBarValue(0);
                    CommonData.mainwindow.changeProgressBarTag("");
            }
            catch (Exception)
            {
                
                throw;
            }
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
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
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
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);   
                CommonData.WriteLOG(ex);
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
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex); 
                CommonData.WriteLOG(ex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                 OpenFileDialog dlg = new OpenFileDialog();
                 dlg.DefaultExt = ".xml"; // Default file extension
                 dlg.Filter = ".xml files (*.xml)|*.bak|All files (*.*)|*.*";
                 Nullable<bool> result = dlg.ShowDialog();
                 string FilePath = dlg.FileName;
                 txtpath.Text = FilePath;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
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

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //List<ObjectDTO> lstObjectDTO = new List<ObjectDTO>();
                //lstObjectDTO = DeSerializeObject<List<ObjectDTO>>(txtpath.Text);
                //lstObjectDTO = changeObjectDTO(lstObjectDTO);
                //CreateNewObjectList(lstObjectDTO);
                ////DLMS.ImportDataFromDLMS dts = new DLMS.ImportDataFromDLMS();
                ////dts.GetDataFromDLMSClient(txtpath.Text);
                ConvertDataFromDLMS();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        

    }

}
