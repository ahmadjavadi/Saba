using System;
using System.Windows;
using System.Windows.Input;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for Select.xaml
    /// </summary>
    public partial class Select : System.Windows.Window
    {
        
        public Select()
        {
            InitializeComponent();
            
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GridMain.SelectedItem != null)
                {
                    SelectDetail SD = new SelectDetail();
                    SD = (SelectDetail)GridMain.SelectedItem;
                    CommonData.selectedDetail = SD;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                         
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void GridMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnOK_Click(null, e);
        }

        
    }
}
