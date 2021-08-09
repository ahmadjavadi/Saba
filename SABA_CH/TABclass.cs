using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ACCProj
{
    /// <summary>
    /// Interaction logic for frmSetting.xaml
    /// </summary>
    public partial class frmSetting : Window
    {
        private static bool IsNew = false;
       // public DataBaseDataContext DataBase = new DataBaseDataContext();
        private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
        
        public frmSetting()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            tabCtrl.SelectedItem = tabPag;
            if (!tabCtrl.IsVisible)
            {

                tabCtrl.Visibility = Visibility.Visible;

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tabCtrl.Items.Remove(tabPag);
            if (!tabCtrl.HasItems)
            {

                tabCtrl.Visibility = Visibility.Hidden;

            }
        }       

        private void GrouphCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

     /*   private void ToolStripButtonSave_Click(object sender, RoutedEventArgs e)
        {
            decimal? Result = 1000000;
            string ErrMsg = "";
            DataBase.AccInsUpdSetting(Convert.ToInt16(GrouphCode.Text.ToString()), Convert.ToInt16(KolCode.Text.ToString()), Convert.ToInt16(tafsilcode.Text.ToString()), Convert.ToInt16(TopicCode.Text.ToString()), ref Result, ref ErrMsg);

        }*/

    /*    private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (AccShowSettingResult show in DataBase.AccShowSetting(""))
            {
                GrouphCode.Text = show.GrouphCode.ToString();
                KolCode.Text = show.KolCode.ToString();
                tafsilcode.Text = show.TafsilCode.ToString();
                TopicCode.Text = show.TafsilCode.ToString();
            }
        }

        private void ToolStripButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }*/
    }
}
