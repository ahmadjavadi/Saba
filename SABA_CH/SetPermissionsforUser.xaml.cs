using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH
{
    /// <summary>
    /// Interaction logic for SetPermissionsforUser.xaml
    /// </summary>
    public partial class SetPermissionsforUser : System.Windows.Window
    {
       
        string _userId = "";
        public List<UserPermissions> Buttonlist = new List<UserPermissions>();        
        public int SelectedRow = 0;
        public readonly int WindowId = 8;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public ShowTranslateofLable Tr = null;
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
        public SetPermissionsforUser()
        {
            InitializeComponent();
            Showusers users = new Showusers(" and IsVisible=1 ");
            DataContext = users;
            Tr = CommonData.translateWindow(8);
            ChangeFlowDirection();
            TranslateGrid();
        }
        public void TranslateGrid()
        {
            try
            {
                UserDataGrid.Columns[0].Header = Tr.TranslateofLable.Object1;

                gridus.Columns[0].Header = Tr.TranslateofLable.Object2;
                gridus.Columns[1].Header = Tr.TranslateofLable.Object3;
                gridus.Columns[2].Header = Tr.TranslateofLable.Object4;
                gridus.Columns[3].Header = Tr.TranslateofLable.Object5;
                gridus.Columns[4].Header = Tr.TranslateofLable.Object6;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public void ChangeFlowDirection()
        {
            try
            {
                //MGrid.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        private void ToolStripButtonSave_Click(object sender, RoutedEventArgs e)
        {
            bool canShow = false, CanDelete = false, CanInsert = false, CanImportFromFile = false, CanEdit = false;
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
                        canShow = false;
                    else
                        canShow = true;

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

                    SQLSPS.InsButtonAccess(ButtonID, Convert.ToDecimal(_userId), canShow, CanDelete, CanEdit, CanInsert,CanImportFromFile, result, errMsg);

                }
                RefreshUsgrid();
                //MessageBox.Show("ذخیره به درستی انجام شد");//CommonData.mainwindow.tm.TranslateofMessage.Message91);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در انجام عملیات");//CommonData.mainwindow.tm.TranslateofMessage.Message92);

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
                    _userId = user.UserID.ToString();
                    RefreshUsgrid();
                    //labelUser.Content = user.UserName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
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
                        if (us.CanUpdateImage == @"..\Image\erase.png")
                            us.CanUpdateImage = @"..\Image\button_ok.png";
                        else
                            us.CanUpdateImage = @"..\Image\erase.png";
                        break;
                    case 5:
                        if (us.CanImportFromFilImage == @"..\Image\erase.png")
                            us.CanImportFromFilImage = @"..\Image\button_ok.png";
                        else
                            us.CanImportFromFilImage = @"..\Image\erase.png";
                        break;


                }
                gridus.SelectedItem = us;
                gridus.ItemsSource = null;
                this.Buttonlist[SelectedRow] = us;
                gridus.ItemsSource = this.Buttonlist;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }

        }
        public void RefreshUsgrid()
        {
            try
            {
                gridus.ItemsSource = null;
                string filter = " and Access.UserID=" + _userId;
                ShowButtonAccess buttonAccess = new ShowButtonAccess(filter, "");
                this.Buttonlist = buttonAccess.Buttonlist;
                gridus.ItemsSource = this.Buttonlist;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }

        private void frmSetPermission_Activated(object sender, EventArgs e)
        {
            _tabCtrl.SelectedItem = _tabPag;
            if (!_tabCtrl.IsVisible)
            {

                _tabCtrl.Visibility = Visibility.Visible;

            }
        }

        private void frmSetPermission_Closing(object sender, CancelEventArgs e)
        {
            ClassControl.OpenWin[WindowId] = false;
            _tabCtrl.Items.Remove(_tabPag);
            if (!_tabCtrl.HasItems)
            {

                _tabCtrl.Visibility = Visibility.Hidden;

            }
        }
    }
}
