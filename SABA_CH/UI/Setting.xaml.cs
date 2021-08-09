using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : System.Windows.Window
    {
        public readonly int windowID = 25;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public ObjectParameter Result = new ObjectParameter("Result", 1000000);
        public ObjectParameter ModemID = new ObjectParameter("ModemID", 1000000);
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public TabControl Tab { set { tabCtrl = value; } }
        public Setting()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                foreach (char item in e.Text)
                    if (!Char.IsDigit(item) && !Char.IsControl(item))
                        e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(txtFontsize.Text)<8 || Convert.ToInt32(txtFontsize.Text)>48 )
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message60);
                    return;
                }
                CommonData.setting.FontSize = Convert.ToInt32(txtFontsize.Text);
                CommonData.setting.Fontfamily = FontCombo.Text;
                CommonData.setting.RowColor = superCombo.SelectedColor.ToString();
                MessageBox.Show("ذخیره اطلاعات به درستی انجام شد");//CommonData.mainwindow.tm.TranslateofMessage.Message91);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در ذخیره اطلاعات");//CommonData.mainwindow.tm.TranslateofMessage.Message92);
            }
        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {

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
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                this.DataContext = CommonData.setting;                
                toolBar1.DataContext = CommonData.ShowButtonBinding("", windowID);
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
    }
}
