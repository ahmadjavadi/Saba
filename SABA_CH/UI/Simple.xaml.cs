using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Windows;
using System.Windows.Controls;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for Simple.xaml
    /// </summary>
    public partial class Simple : System.Windows.Window
    {
        public readonly int windowID = 2;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public ObjectParameter Result = new ObjectParameter("Result", 1000000);
        public ObjectParameter ID = new ObjectParameter("groupid", 1000000);
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public TabControl Tab { set { tabCtrl = value; } }
        public Simple()
        {
            InitializeComponent();
        }
        private void ToolStripButtonNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToolStripButtonSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToolStripButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToolStripButtonImport_Click(object sender, RoutedEventArgs e)
        {

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
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                ClassControl.OpenWin[2] = false;
                tabCtrl.Items.Remove(tabPag);
                if (!tabCtrl.HasItems)
                {

                    tabCtrl.Visibility = Visibility.Hidden;

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
    }
}
