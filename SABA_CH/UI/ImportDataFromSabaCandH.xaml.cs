using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for ImportDataFromSabaCandH.xaml
    /// </summary>
    public partial class ImportDataFromSabaCandH : System.Windows.Window
    {
        private TabControl tabCtrl;
        private TabItem tabPag;
        public ShowTranslateofLable tr = null;
        public ShowGroups_Result Selectedgroup;
        public readonly int windowID = 46;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
        public ImportDataFromSabaCandH()
        {
            InitializeComponent();
            translateWindows();
            changeFlowDirection();
            RefreshCmbGroups();

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
                MessageBox.Show(ex.ToString().ToString());
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
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
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
        public void RestoreDataBase(string FilePath)
        {
            try
            {
                CommonData.mainwindow.changeProgressBar_MaximumValue(100);
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag("");
                ObjectParameter Resul = new ObjectParameter("Result", 1000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                SQLSPS.RestoreDatabase("SabaCandHOld", FilePath, Resul, ErrMSG);
                if (ErrMSG.Value.ToString() == "")
                {

                    SQLSPS.ImportFromSabaCandHNew(Selectedgroup.GroupID, Selectedgroup.GroupType, 1, Resul, ErrMSG);
                }
                else
                    MessageBox.Show(ErrMSG.Value.ToString());
                if (ErrMSG.Value.ToString() == "")
                {
                    CommonData.mainwindow.changeProgressBarValue(100);
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message64);
                    string Filter = "";
                    if (CommonData.mainwindow.SelectedGroup.GroupID != -1)
                        Filter = "and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + CommonData.mainwindow.SelectedGroup.GroupID + "and  GroupType=" + CommonData.mainwindow.SelectedGroup.GroupType + ") ";

                    CommonData.mainwindow.RefreshSelectedMeters(Filter);
                }
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
                dlg.DefaultExt = ".bak"; // Default file extension
                dlg.Filter = "SQL backup files (*.bak)|*.bak|All files (*.*)|*.*";
                Nullable<bool> result = dlg.ShowDialog();
                string FilePath = dlg.FileName;
                txtpath.Text = FilePath;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());
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
                string FilePath = txtpath.Text;
                if (FilePath.ToUpper().Contains(".BAK"))
                    RestoreDataBase(FilePath);
                else
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message63);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
    }
}
