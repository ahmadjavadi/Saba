using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Windows;
using System.Windows.Controls;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : System.Windows.Window
    {
        public bool ISEditing = false;
        public ShowTranslateofLable tr = null;
        
        public decimal? userID = 1000000;
        public string UserName = "";
        public ObjectParameter UserID = new ObjectParameter("MeterID", 1000000);
        public ObjectParameter Result = new ObjectParameter("Result", 1000000);
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public ShowButtonAccess us = null;
        public readonly int windowID = 12;
        public bool IsNew = false;
        public ShowUsers_Result user = new ShowUsers_Result();
        private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
        public NewUser()
        {
            try
            {
                InitializeComponent();
                RefreshDataGrid("");
                tr = CommonData.translateWindow(windowID);                
                CommonData.ChangeFlowDirection(Griddown);
                GridLabel.DataContext = tr.TranslateofLable;
                this.DataContext = tr.TranslateofLable;
                TranslateDataGrid();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshDataGrid(string Filter)
        {
            try
            {
                IsNew = false;
                //GridMain.ItemsSource = null;
                Filter = Filter + " and IsVisible=1 and UPPER(UserName)!='RSAADMIN'  ";
                Showusers Users = new Showusers(Filter);
                GridMain.ItemsSource = Users.CollectShowUsers;
                Griddown.DataContext = Users;
                if (GridMain.Items.Count > 0)
                    GridMain.SelectedIndex = GridMain.Items.Count - 1;
                else if (GridMain.Items.Count == 0)
                {
                    ToolStripButtonNew_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        
        public void TranslateDataGrid()
        {
            try
            {
                GridMain.Columns[0].Header = tr.TranslateofLable.Object1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonSave_Click(object sender, RoutedEventArgs e)
        {
            UserID = new ObjectParameter("UserID", 1000000);
            Result = new ObjectParameter("Result", 1000000);
            ErrMsg = new ObjectParameter("ErrMsg", "");
         
            if (txtConfirmPass.Password!=txtPass.Password)
            {
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message4);
                return;
            }
            if (txtUserName.Text.Length<1 || txtPass.Password.Length<1)
            {
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message90);
                return;
            }
            try
            {
                ISEditing = false;
                decimal? invalidUserID = 10000000;
                if (IsNew)
                {
                    int retuntype = 0;
                    MessageBoxResult MBR=MessageBoxResult.Cancel;
                    Showusers s = new Showusers("  ");
                   
                    foreach (var item in s._lstShowUsers)
                    {
                        if (item.UserName.ToUpper() == txtUserName.Text.ToUpper() && item.IsVisible == true)
                            retuntype = 1;
                        else if (item.UserName.ToUpper() == txtUserName.Text.ToUpper() && item.IsVisible == false)
                        {
                            retuntype = -1;
                            invalidUserID = item.UserID;
                        }
                    }
                    if (retuntype == 1)
                    {
                        MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message77);
                        return;
                    }
                    else if (retuntype == -1)
                    {
                        MBR= MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message78, "", MessageBoxButton.OKCancel);

                    }
                    if (retuntype == 0)
                    {
                        SQLSPS.InsUsers(txtUserName.Text, txtPass.Password, UserID, Result, ErrMsg);
                        if (ErrMsg.Value.ToString() != "")
                        {
                            MessageBox.Show(ErrMsg.Value.ToString());
                            return;
                        }
                        else
                        {
                            userID = Convert.ToDecimal(UserID.Value);
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message83);
                            CommonData.mainwindow.JoinInGroup(null, null);

                        }
                    }
                    else if (retuntype == -1 && MBR == MessageBoxResult.OK)
                    {
                        Result = new ObjectParameter("Result", 1000000);
                        ErrMsg = new ObjectParameter("ErrMsg", "");
                        if (userID == 0)
                            userID = null;
                        SQLSPS.UpdUser(txtUserName.Text, txtPass.Password, invalidUserID, ErrMsg, Result);
                        if (ErrMsg.Value.ToString() != "")
                        {
                            MessageBox.Show(ErrMsg.Value.ToString());
                        }
                        else
                        {
                            userID = Convert.ToDecimal(UserID.Value);
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message83);
                            CommonData.mainwindow.JoinInGroup(null, null);


                        }
                    }
                    
                }
                else if (!IsNew)
                {
                    Result = new ObjectParameter("Result", 1000000);
                    ErrMsg = new ObjectParameter("ErrMsg", "");
                    if (userID == 0)
                        userID = null;

                    Showusers OUsers = new Showusers(" and IsVisible=1 and UserID="+userID);

                    if (OUsers._lstShowUsers[0].UserName.ToUpper() == "ADMIN" && txtUserName.Text.ToUpper() != "ADMIN" ) 
                    {
                        MessageBox.Show("تغییرات نام کاربر امکان پذیر نمی باشد");
                        RefreshDataGrid("");
                        return;
                    }
                    if (OUsers._lstShowUsers[0].UserName.ToUpper() == "ANSARI" && txtUserName.Text.ToUpper() != "ANSARI")
                    {
                        MessageBox.Show("تغییرات نام  کاربر  امکان پذیر نمی باشد");
                        RefreshDataGrid("");
                        return;
                    }
                    if (OUsers._lstShowUsers[0].UserName.ToUpper() == "ANSARI" && txtPass.Password != "88102351-7")
                    {
                        MessageBox.Show("تغییرات پسورد  کاربر  امکان پذیر نمی باشد");
                        RefreshDataGrid("");
                        return;
                    }
                    //if (CommonData.UserName.ToUpper() == "ADMIN" && txtUserName.Text.ToUpper() != "ADMIN")
                    //{
                    //    MessageBox.Show("تغییرات نام در کاربر اصلی امکان پذیر نمی باشد");
                    //    return;
                    //}

                    SQLSPS.UpdUser(txtUserName.Text, txtPass.Password,userID,ErrMsg, Result);
                    if (ErrMsg.Value.ToString() != "")
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                    }

                }
                RefreshDataGrid("");
                //MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message7);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در ذخیره داده");
            }
        }

        private void ToolStripButtonNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsNew = true;
                CommonData.New(this.Griddown);
                txtPass.Password = "";
                txtConfirmPass.Password = "";
                Griddown.DataContext = null;
                userID = 1000000;               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void ToolStripButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Message = "";
                if (userID == null)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message2);
                    return;
                }
                MessageBoxResult res = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message1, "Warning", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    Result = new ObjectParameter("Result", 1000000);
                    ErrMsg = new ObjectParameter("ErrMsg", "");
                    if (userID==CommonData.UserID)
                    {
                         MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message68);
                         return;
                    }
                    Showusers OUSER = new Showusers(" and IsVisible=1 and UserID="+ userID);
                    if (OUSER._lstShowUsers[0].UserName.ToUpper()=="ANSARI" || OUSER._lstShowUsers[0].UserName.ToUpper() == "ADMIN")
                    {
                        MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message68);
                        return;
                    }
                    SQLSPS.DelUser(userID, Result, ErrMsg);
                    if (ErrMsg.Value.ToString() != "")
                    {
                        MessageBox.Show(ErrMsg.Value.ToString());
                    }
                    RefreshDataGrid("");
                }
                MessageBox.Show("عملیات به درستی انجام شد");//CommonData.mainwindow.tm.TranslateofMessage.Message91);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در انجام عملیات");//CommonData.mainwindow.tm.TranslateofMessage.Message92);
            }
        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid("");
        }

        private void ToolStripButtonImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                toolBar1.DataContext = CommonData.ShowButtonBinding("", windowID);
                tabCtrl.SelectedItem = tabPag;
                if (!tabCtrl.IsVisible)
                {

                    tabCtrl.Visibility = Visibility.Visible;
                }
                //if (SelectedModem != null)
                //    txtModemManufacturerName.Text = SelectedModem.ManufacturerName.ToString();
                //if (SelectedMeter != null)
                //    txtManufacturer.Text = SelectedMeter.ManufacturerName.ToString();
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
                if (!ISEDiting())
                {
                    e.Cancel = true;
                    return;
                }
                tabCtrl.Items.Remove(tabPag);
                if (!tabCtrl.HasItems)
                {

                    tabCtrl.Visibility = Visibility.Hidden;

                }
                ClassControl.OpenWin[windowID] = false;
                CommonData.mainwindow.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public bool ISEDiting()
        {
            try
            {
                if (ISEditing)
                {
                    MessageBoxResult res = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message5, "Warning", MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.No)
                        return false;
                    else
                    {
                        ISEditing = false;
                        return true;
                    }
                }
                else
                    return true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                return false;
            }
        }

        private void GridMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!ISEDiting())
                    return;
                user = new ShowUsers_Result();
                IsNew = false;
                if (GridMain.SelectedItem != null)
                {
                    user = (ShowUsers_Result)GridMain.SelectedItem;
                    Griddown.DataContext = user;                    
                    userID = user.UserID;
                    UserName = user.UserName;
                    txtPass.Password = user.UserPass.ToString();
                    txtConfirmPass.Password = user.UserPass.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
    }
}
