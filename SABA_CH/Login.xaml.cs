using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : System.Windows.Window
    {
        public bool closewithbutton = false;
        public ShowUsers_Result SelectedUser = null;
        public ShowLanguage_Result SelectedLanguage=null;
        public ShowLanguage Language = null;
        public Showusers Users = null;
        public Login()
        {
            InitializeComponent();
            Refresh();
        }
        public void Refresh()
        {
            try
            {
                Language = new ShowLanguage("");
                Users = new Showusers(" and IsVisible=1 ");
                cmbLanguage.ItemsSource = Language.CollectShowLanguage;
                
            }
             
            catch (Exception ex)
	        {
	            if (ex.ToString().ToLower().Contains("sql"))
	                throw ex;

	            else
	            {
                    MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                }
	        }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (SelectedUser == null || SelectedLanguage==null)
                //{
                //    MessageBox.Show("لطفا کاربر و زبان را انتخاب کنید");
                //    return;
                //}
                Users = new Showusers(" and IsVisible=1 ");
                foreach (var item in Users.CollectShowUsers)                
                {
                    SelectedUser = new ShowUsers_Result();
                    SelectedUser = (ShowUsers_Result)item;
                    if (SelectedUser.UserName.ToString().ToUpper() == txtUserName.Text.ToUpper().Trim() && SelectedUser.UserPass.ToString() == txtPass.Password)
                    {
                        CommonData.UserID = Convert.ToInt32(SelectedUser.UserID);
                        CommonData.UserName = SelectedUser.UserName.ToString();
                        closewithbutton = true;
                        this.Close();
                        return;
                    }
                }
                //MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message71);
                MessageBox.Show("نام کاربری و یا رمز عبور وارد شده اشتباه است");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

       

        private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbLanguage.SelectedItem != null)
                {
                    SelectedLanguage = (ShowLanguage_Result)cmbLanguage.SelectedItem;
                    CommonData.LanguagesID = Convert.ToDecimal(SelectedLanguage.LanguagesID);
                    CommonData.LanguageName = SelectedLanguage.LanguageName;
                    //if (SelectedLanguage.FlowDirection.ToUpper() == "RIGHTTOLEFT")
                    //    CommonData.FlowDirection = FlowDirection.RightToLeft;
                    //else
                    //    CommonData.FlowDirection = FlowDirection.LeftToRight;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtUserName.Focus();
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
                if (!closewithbutton)
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
    }
}
