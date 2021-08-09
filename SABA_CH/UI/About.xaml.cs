using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : System.Windows.Window
    {
        public readonly int WindowId = 43;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
        public ShowTranslateofLable Tr = null;
        public About()
        {
            InitializeComponent();
            ChangeFlowDirection();
            Tr = CommonData.translateWindow(43);
            GridMain.DataContext = Tr.TranslateofLable;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location); 
            Lbl3.Content ="version: " + fvi.FileVersion;
            ShowExpiredDate();
        }

        public void ShowExpiredDate()
        {
            try
            {
                
                LblExpiredDate1.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                 new Action(
                                     delegate
                                     {
                                         if (CommonData.ExpireDate.Substring(0, 2).StartsWith("0"))
                                             LblExpiredDate1.Content ="14"+ CommonData.ExpireDate.Substring(0, 2) + "/" + CommonData.ExpireDate.Substring(2, 2) + "/" + CommonData.ExpireDate.Substring(4, 2);
                                         else
                                             LblExpiredDate1.Content = "13"+CommonData.ExpireDate.Substring(0, 2) + "/" + CommonData.ExpireDate.Substring(2, 2) + "/" + CommonData.ExpireDate.Substring(4, 2);
                                     }
                  ));
            }
            catch (Exception ex)
            {
                 
            }
            
            
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
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
                ClassControl.OpenWin[WindowId] = false;
                _tabCtrl.Items.Remove(_tabPag);
                if (!_tabCtrl.HasItems)
                {
                    _tabCtrl.Visibility = Visibility.Hidden;
                }
                CommonData.mainwindow.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void ChangeFlowDirection()
        {
            try
            {
                //Lbl1.FlowDirection = CommonData.FlowDirection;
                //Lbl2.FlowDirection = CommonData.FlowDirection;
                //Lbl3.FlowDirection = CommonData.FlowDirection;
                 
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btnExpirationDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActiveSoftwareWindow.Instance.ShowDialogForm();
            }
            catch 
            {
                
            }
           
        }
    }
}
