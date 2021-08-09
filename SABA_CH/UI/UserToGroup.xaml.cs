using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for UserToGroup.xaml
    /// </summary>
    public partial class UserToGroup : System.Windows.Window
    {
        private int _activeTabnumber;
        public readonly int WindowId = 8;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public int SelectedRow;
        public ShowTranslateofLable Tr;
        public ShowButtonAccess_Result Us = null;
        public List<UserPermissions> Buttonlist = new List<UserPermissions>(); 
        public ShowUserToGroup_Result UsToGp = null;
        public List<UserToGr> LstUserTogroup; 
        public ObjectParameter Result = new ObjectParameter("Result", 1000000);
        public ObjectParameter Userid = new ObjectParameter("userid", 1000000);
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public TabControl Tab { set { _tabCtrl = value; } }
        public ShowUsers_Result SelectedUser;       
        public decimal? UserId = 1000000; 
        public UserToGroup()
        {
            InitializeComponent();
            Refresh();
            Tr = CommonData.translateWindow(7);
            TranslateGrids();
            ChangeFlowDirection();
            Showusers users = new Showusers(" and IsVisible=1 and UPPER(UserName)!='RSAADMIN' ");
            DataContext = users;
            Tr = CommonData.translateWindow(8);
            ChangeFlowDirection();
            TranslateGrid();

        }
        public void TranslateGrid()
        {
            try
            {
                tabitem0.Header = Tr.TranslateofLable.Object7;
                tabitem1.Header = Tr.TranslateofLable.Object8;
                UserDataGrid.Columns[0].Header = Tr.TranslateofLable.Object1;
                gridus.Columns[0].Header = Tr.TranslateofLable.Object2;
                gridus.Columns[1].Header = Tr.TranslateofLable.Object3;
                gridus.Columns[2].Header = Tr.TranslateofLable.Object4;
                gridus.Columns[3].Header = Tr.TranslateofLable.Object5;
                gridus.Columns[4].Header = Tr.TranslateofLable.Object6;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void Refresh()
        {
            try
            {
                Showusers users = new Showusers(" and IsVisible=1 and UPPER(UserName)!='RSAADMIN' ");
                
                MainGrid.ItemsSource = users.CollectShowUsers;
                UserDataGrid.ItemsSource = users.CollectShowUsers;
                MainGrid.SelectedIndex = 0;
                UserDataGrid.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        public void TranslateGrids()
        {
            try
            {
                MainGrid.Columns[0].Header = Tr.TranslateofLable.Object1;
             //   UserToGroupGrid.Columns[0].Header = tr.TranslateofLable.Object2;
                UserToGroupGrid.Columns[1].Header = Tr.TranslateofLable.Object3;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void ChangeFlowDirection()
        {
            try
            {
                //MGrid.FlowDirection = CommonData.FlowDirection;
                //Maintabctr.FlowDirection = CommonData.FlowDirection;
                //UserDataGrid.FlowDirection = CommonData.FlowDirection;
                //UserDataGrid.FlowDirection = CommonData.FlowDirection;
                //gridus.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshUserToGroup()
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();

                string p = CommonData.ProvinceCode;
                string filter = "";
                try
                {
                  int value= Int32.Parse(p, NumberStyles.HexNumber);
                  value = value / 1000;
                    if (value == 1)
                        value = 8;
                    else if (value == 8)
                        value = 1;

                    if (value != 0)
                        filter = "and Groups.ProvinceID in (0," + value + ")";
                }
                catch 
                {
                     filter = "";
                }

                LstUserTogroup = new List<UserToGr>();
                bank.Database.Connection.Open();
             
                foreach (ShowUserToGroup_Result item in bank.ShowUserToGroup(filter, UserId))
                {
                    UserToGr UsG = new UserToGr();
                    UsG.UserId = item.UserID;
                    UsG.GroupId = item.GID;
                    UsG.GroupType = item.GroupType;
                    UsG.GroupName = item.GroupName;
                    UsG.Isvisable = Convert.ToBoolean(item.Isvisable);
                    LstUserTogroup.Add(UsG);
                }
                bank.Database.Connection.Close();
                bank.Dispose();
                UserToGroupGrid.ItemsSource = LstUserTogroup;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
        public void ChangelstUserTogroup(int i)
        {
            try
            {
                if (LstUserTogroup.Count>i && i>=0)
                {
                    UserToGr UsG;
                    UsG = LstUserTogroup[i];
                   
                    
                    UsG.Isvisable = !UsG.Isvisable;
                    LstUserTogroup[i] = UsG;
                }
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

        private void MainGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                SelectedUser = (ShowUsers_Result)MainGrid.SelectedItem;
                UserDataGrid.SelectedItem = SelectedUser;
                UserId =Convert.ToDecimal(SelectedUser.UserID);
                labelUser.Content = SelectedUser.UserName;
                RefreshUserToGroup();
            }
            catch (Exception ex)
            {
                
            }
        }

        private void ToolStripButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void ToolStripButtonNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToolStripButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void ToolStripButtonImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserToGroupGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectedRow = UserToGroupGrid.SelectedIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }

        private void UserToGroupGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
        }

        private void SetAllUserToGroup(bool permission)
        {
            for (int i=0;i<=LstUserTogroup.Count-1;i++)
            {
                    UserToGr UsG = new UserToGr();
                    UsG = LstUserTogroup[i];

                    UsG.Isvisable = permission;
                    LstUserTogroup[i] = UsG;
            }
        }
     

        private void ToolStripButtonSave11_Click(object sender, RoutedEventArgs e)
        {
            bool CanShow = false, CanDelete = false, CanInsert = false, CanImportFromFile = false, CanEdit = false;
            decimal? ButtonID = 1000000;
            ObjectParameter result = new ObjectParameter("Result", 1000000);
            ObjectParameter errMsg = new ObjectParameter("ErrMsg", "");
            try
            {
                for (int i = 0; i < Buttonlist.Count; i++)
                {
                    UserPermissions us = new UserPermissions();
                    us = (UserPermissions)Buttonlist[i];
                    if (us.CanShowImage == "..\\Image\\erase.png")
                        CanShow = false;
                    else
                        CanShow = true;

                    if (us.CanDelImage == "..\\Image\\erase.png")
                        CanDelete = false;
                    else
                        CanDelete = true;

                    if (us.CanInsertImage == "..\\Image\\erase.png")
                        CanInsert = false;
                    else
                        CanInsert = true;

                    if (us.CanImportFromFilImage == "..\\Image\\erase.png")
                        CanImportFromFile = false;
                    else
                        CanImportFromFile = true;
                    ButtonID = Convert.ToDecimal(us.ButtonId);

                    SQLSPS.InsButtonAccess(ButtonID, Convert.ToDecimal(UserId), CanShow, CanDelete, CanEdit, CanInsert,CanImportFromFile, result, errMsg);
                    

                }
                RefreshUsgrid();
                MessageBox.Show("پایان انجام فرمان");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);

            }
            if (!_isSaved)
            {
                MessageBox.Show("تنظیمات گروهها ذخیره نشده است لطفا به قسمت گروهها برگشته و آنرا ذخیره نمایید");
            }
            else
                _isSaved = false;
        }

        private void gridus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SelectedRow = gridus.SelectedIndex;
                UserPermissions us = (UserPermissions)gridus.SelectedItem;
                int i = gridus.CurrentCell.Column.DisplayIndex;
                switch (i)
                {
                    case 1:
                        if (us.CanShowImage == @"..\Image\erase.png")
                            us.CanShowImage = @"..\Image\button_ok.png";
                        else
                            us.CanShowImage = @"..\Image\erase.png";
                        break;
                    case 2:
                        if (us.CanInsertImage == @"..\Image\erase.png")
                            us.CanInsertImage = @"..\Image\button_ok.png";
                        else
                            us.CanInsertImage = @"..\Image\erase.png";
                        break;
                    case 3:
                        if (us.CanDelImage == @"..\Image\erase.png")
                            us.CanDelImage = @"..\Image\button_ok.png";
                        else
                            us.CanDelImage = @"..\Image\erase.png";
                        break;
                    case 4:
                        if (us.CanImportFromFilImage == @"..\Image\erase.png")
                            us.CanImportFromFilImage = @"..\Image\button_ok.png";
                        else
                            us.CanImportFromFilImage = @"..\Image\erase.png";
                        break;
                    case 5:

                        if (us.CanUpdateImage == @"..\Image\erase.png")
                            us.CanUpdateImage = @"..\Image\button_ok.png";
                        else
                            us.CanUpdateImage = @"..\Image\erase.png";
                        break;                    


                }
                gridus.SelectedItem = us;
                gridus.ItemsSource = null;
                this.Buttonlist[SelectedRow] = us;
                gridus.ItemsSource = this.Buttonlist;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void UserDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ShowUsers_Result user = new ShowUsers_Result();
                user = (ShowUsers_Result)UserDataGrid.SelectedItem;
                if (user != null)
                {
                    UserId = user.UserID;
                    RefreshUsgrid();
                    labelUser1.Content = user.UserName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshUsgrid()
        {
            try
            {
                gridus.ItemsSource = null;
                string filter = " and Access.UserID=" + UserId;
                ShowButtonAccess buttonAccess = new ShowButtonAccess(filter, "");
                this.Buttonlist = buttonAccess.Buttonlist;
                gridus.ItemsSource = this.Buttonlist;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void Maintabctr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _activeTabnumber = Maintabctr.SelectedIndex;
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        bool _isSaved = false;
        private void Save()
        {
            try
            {
                if (_activeTabnumber == 0)
                {
                    try
                    {

                        Result = new ObjectParameter("Result", 1000000);
                        Userid = new ObjectParameter("userid", 1000000);
                        ErrMsg = new ObjectParameter("ErrMsg", "");
                        SQLSPS.DelUserToGroup(UserId, Result, ErrMsg);
                        Result = new ObjectParameter("Result", 1000000);
                        ErrMsg = new ObjectParameter("ErrMsg", "");
                        bool isGroupSelected = false;
                        for (int i = 0; i < LstUserTogroup.Count; i++)
                        {
                            UserToGr UsG = new UserToGr();
                            UsG = LstUserTogroup[i];
                            if (Convert.ToBoolean(UsG.Isvisable))
                            {
                                isGroupSelected = true;
                                break;
                            }

                        }
                        if (!isGroupSelected)
                        {
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message82);
                            return;
                        }
                        for (int i = 0; i < LstUserTogroup.Count; i++)
                        {
                            UserToGr UsG = new UserToGr();
                            UsG = LstUserTogroup[i];
                            if (Convert.ToBoolean(UsG.Isvisable))
                            {
                                SQLSPS.InsUserToGroup(UsG.GroupId, UserId, true, UsG.GroupType, Result, ErrMsg);
                                if (ErrMsg.Value.ToString() != "")
                                {
                                    MessageBox.Show(ErrMsg.Value.ToString());
                                }
                            }
                        }
                        CommonData.mainwindow.RefreshCmbGroups();
                        _isSaved = true;
                        MessageBox.Show("عملیات به درستی انجام شد");//CommonData.mainwindow.tm.TranslateofMessage.Message91);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                        MessageBox.Show("اشکال در انجام عملیات");//CommonData.mainwindow.tm.TranslateofMessage.Message92);
                    }
                }
                else
                {
                  
                    ToolStripButtonSave11_Click(null, null);
                    if (!_isSaved)
                    {
                        MessageBox.Show("تنظیمات گروهها ذخیره نشده است لطفا به قسمت گروهها برگشته و آنرا ذخیره نمایید");
                    }
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message92);
            }
        }

        private void MainGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcount.Content =rowcount+"=" + MainGrid.Items.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void UserToGroupGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcountGroup.Content =rowcount+"="+ UserToGroupGrid.Items.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentRowIndex = UserToGroupGrid.Items.IndexOf(UserToGroupGrid.CurrentItem);
            ChangelstUserTogroup(currentRowIndex);
        }
        private void IsAllvisable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsAllvisable.IsChecked==true)
                SetAllUserToGroup(true);
                else SetAllUserToGroup(false);
                //  ChangelstUserTogroup(true, SelectedRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
    }
}
