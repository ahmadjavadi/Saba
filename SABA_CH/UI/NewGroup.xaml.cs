using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using SABA_CH.DataBase;
using SABA_CH.Global;

using Button = System.Windows.Controls.Button;
namespace SABA_CH
{
	/// <summary>
	/// Interaction logic for NewGroup.xaml
	/// </summary>
	public partial class NewGroup : System.Windows.Window
	{
		public readonly int windowID=10;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public ShowButtonAccess_Result us = null;
        public ObjectParameter Result = new ObjectParameter("Result", 1000000);
        public ObjectParameter groupid = new ObjectParameter("groupid", 1000000);
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public TabControl Tab { set { tabCtrl = value; } }
        public ShowGroups_Result SelectedGroups = null;
        public ShowMeter_Result RSelectedMeter = null;
        public ShowMeter_Result LSelectedMeter = null;
        public ShowTranslateofLable tr = null;
        
        public bool IsNew = false;
        public decimal? GroupID = 1000000;
        public int GroupType = 1;
        public bool MeterToGroupSave = true;
        public List<ShowMeter_Result> _LlstShowMeterstemp;
        public List<ShowMeter_Result> _RlstShowMeterstemp;
		public NewGroup()
		{
			this.InitializeComponent();
            Refresh();
            ChangeFlowDirection();
			// Insert code required on object creation below this point.
		}
        public void Refresh()
        {
            try
            {
                string p = CommonData.ProvinceCode;
                string filter = "";
                try
                {
                    int value = Int32.Parse(p, NumberStyles.HexNumber);
                    value = value / 1000;
                    if (value == 1)
                        value = 8;
                    else if (value == 8)
                        value = 1;

                    if (value != 0)
                        filter = "and m.ProvinceID in (0," + value + ")";
                }
                catch
                {
                    filter = "";
                }

                ShowGroups Groups = new ShowGroups(filter,0,CommonData.LanguagesID);
                MainGrid.ItemsSource = Groups._lstShowGroups;
                if (MainGrid.Items.Count > 0)
                    if (SelectedGroups != null)
                        MainGrid.SelectedItem = SelectedGroups;
                    else
                        MainGrid.SelectedIndex = 0;
                Translate();
                ChangeFlowDirection();
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
                //LayoutRoot.FlowDirection = CommonData.FlowDirection;
                //MeterListGrid.FlowDirection = CommonData.FlowDirection;
                //MainGrid.FlowDirection = CommonData.FlowDirection;
                //MeterListGrid.FlowDirection = CommonData.FlowDirection;
                //btnSendOne.FlowDirection = CommonData.FlowDirection;
                //btnSendAll.FlowDirection = CommonData.FlowDirection;
                //btngetOne.FlowDirection = CommonData.FlowDirection;
                //btngetAll.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void Translate()
        {
            try
            {
                tr = CommonData.translateWindow(windowID);
                GridLabel.DataContext = tr.TranslateofLable;
                MeterListGrid.DataContext = tr.TranslateofLable;
                TranslateDatagrid();
            }
            catch (Exception ex) 
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void TranslateDatagrid()
        {
            try
            {
                MainGrid.Columns[0].Header = tr.TranslateofLable.Object1.ToString();
                //MainGrid.Columns[1].Header = tr.TranslateofLable.Object4.ToString();

                LGrid.Columns[0].Header = tr.TranslateofLable.Object2.ToString();
                LGrid.Columns[1].Header = tr.TranslateofLable.Object3.ToString();

                RGrid.Columns[0].Header = tr.TranslateofLable.Object2.ToString();
                RGrid.Columns[1].Header = tr.TranslateofLable.Object3.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshLandRGrig()
        {
            try
            {              
                string LFilter = "  and Main.MeterID  in ( Select MeterID From MeterToGroup  where GroupID=" + GroupID.ToString() + " and GroupType="+GroupType+")";
                string RFilter = "  and Main.MeterID not in (Select MeterID From MeterToGroup  where GroupID=" + GroupID.ToString() + " and GroupType="+GroupType+")";
                ShowMeter LShowMeter = new ShowMeter(LFilter);
                ShowMeter RShowMeter = new ShowMeter(RFilter);


                LGrid.ItemsSource = LShowMeter._lstShowMeters;
                RGrid.ItemsSource = RShowMeter._lstShowMeters;
                _LlstShowMeterstemp = LShowMeter._lstShowMeters ;
                _RlstShowMeterstemp = RShowMeter._lstShowMeters;
                if (RGrid.Items != null)
                    RGrid.SelectedIndex = 0;
                if (LGrid.Items != null)
                    LGrid.SelectedIndex = 0;
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcountR.Content =rowcount+"="+ RGrid.Items.Count;
                lblcountL.Content = rowcount + "=" + LGrid.Items.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshLandRGrigtemp()
        {
            try
            {
                LGrid.ItemsSource = null;
                RGrid.ItemsSource = null;
            }
            catch (Exception ex)
            {
                RGrid.ItemsSource = null;
            }
            LGrid.ItemsSource = _LlstShowMeterstemp;
                RGrid.ItemsSource = _RlstShowMeterstemp;

                if (RGrid.Items != null)
                    RGrid.SelectedIndex = 0;
                if (LGrid.Items != null)
                    LGrid.SelectedIndex = 0;
           
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

        private void ToolStripButtonNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonData.New(LayoutRoot);
                IsNew = true;
                GroupID = 1000000;                
                RefreshLandRGrig();
                txtGroupName.IsEnabled = true;
                //txtHash.IsEnabled = true;

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
                //if (LGrid.Items.Count<1)
                //{
                //      MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message88);
                //      return;
                //}
                if (txtGroupName.Text=="")
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message89);
                    return;
                }
                
                if (IsNew)
                {
                    //Encoding ascii = Encoding.ASCII;
                    //Encoding enc=Encoding.GetEncoding(1256);
                    //byte[] AsciiBytes = enc.GetBytes(txtGroupName.Text);
                    //byte[] unicodeBytes = Encoding.Convert(ascii, enc, AsciiBytes);
                    //char[] asciiChars = new char[ascii.GetCharCount(unicodeBytes, 0, unicodeBytes.Length)];
                    //ascii.GetChars(unicodeBytes, 0, unicodeBytes.Length, asciiChars, 0);
                    //string asciiString = new string(asciiChars);


                    //ShowGroups Groups = new ShowGroups(" and m.GroupName='" + txtGroupName.Text + "' COLLATE Arabic_100_Ci_AS", 0, CommonData.LanguagesID);
                    ShowGroups Groups = new ShowGroups("", 0, CommonData.LanguagesID);
                    for (int i = 0; i < Groups._lstShowGroups.Count; i++)
                    {
                        string nameGroup = Groups._lstShowGroups[i].GroupName.Replace(" ", "");
                        string NameNewGroup= txtGroupName.Text.Replace(" ", "");
                        if (nameGroup.Replace('ي','ی').Replace('ك', 'ک').ToUpper()== NameNewGroup.ToUpper())
                        {
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message76);
                            return;
                        }
                        
                    }
                    //if (Groups._lstShowGroups.Count > 0)
                    //{
                    //    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message76);
                    //    return;
                    //}
                    SQLSPS.InsGroups(txtGroupName.Text, "", groupid, Result, ErrMsg);                  

                    if (ErrMsg.Value.ToString() != "")
                        MessageBox.Show(ErrMsg.Value.ToString());
                    else
                    {
                        GroupID = Convert.ToDecimal(groupid.Value);
                        GroupType = 1;
                        SQLSPS.InsUserToGroup(Convert.ToDecimal(groupid.Value), CommonData.UserID, true, 1, Result, ErrMsg);
                    }
                }
                else
                {
                    if (SelectedGroups.GroupType==1)
                    {
                        ShowGroups Groups = new ShowGroups(" and m.GroupName='" + txtGroupName.Text + "' COLLATE Arabic_100_Ci_AS and m.GroupID!=" + SelectedGroups.GroupID, 0, CommonData.LanguagesID);
                        if (Groups._lstShowGroups.Count > 0)
                        {
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message76);
                            return;
                        }

                        SQLSPS.UpdGroups(txtGroupName.Text, "", Convert.ToDecimal(SelectedGroups.GroupID), Result, ErrMsg);
                        if (ErrMsg.Value.ToString() != "")
                            MessageBox.Show(ErrMsg.Value.ToString());
                    }
                }
                SaveMeterTOGroup();
                Refresh();
                RefreshLandRGrig();
                CommonData.mainwindow.RefreshCmbGroups();
                MessageBox.Show("ذخیره اطلاعات به درستی انجام شد");//CommonData.mainwindow.tm.TranslateofMessage.Message91);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در انجام عملیات");//CommonData.mainwindow.tm.TranslateofMessage.Message92);
            }
        }

        private void ToolStripButtonImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void ToolStripButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool ISEXISTS = false;
                if (GroupID == 1000000 || GroupID == null)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message19);
                    return;
                }
                if (GroupType!=1 || txtGroupName.Text=="گروه جامع")
                {
                     MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message70);
                    return;
                }
                MessageBoxResult res = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message1, CommonData.mainwindow.tm.TranslateofMessage.Message11, MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    ShowMeterToGroup Groups=new ShowMeterToGroup(" and m.GroupID=" + Convert.ToDecimal(GroupID),1);
                    if (LGrid.Items.Count > 0)
                    {
                        MessageBoxResult res1 = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message20+ CommonData.mainwindow.tm.TranslateofMessage.Message1, CommonData.mainwindow.tm.TranslateofMessage.Message11, MessageBoxButton.OKCancel);
                        if (res1 == MessageBoxResult.OK)
                        {
                            if (GroupID==1)
                            {
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message51);
                                return;
                            }
                            SQLSPS.DelGroups(GroupID, Result, ErrMsg);
                            Refresh();
                            CommonData.New(LayoutRoot);
                        }
                    }
                    else
                    {
                        SQLSPS.DelGroups(GroupID, Result, ErrMsg);
                        Refresh();
                        CommonData.New(LayoutRoot);
                    }
                    
                }
                MessageBox.Show("عملیات به درستی انجام شد");//CommonData.mainwindow.tm.TranslateofMessage.Message91);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در انجام عملیات");//CommonData.mainwindow.tm.TranslateofMessage.Message92);
            }
        }

        private void MainGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ////if (!MeterToGroupSave)
                ////{
                ////    MessageBoxResult MessageResult = new MessageBoxResult();
                ////    MessageResult = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message81,"",MessageBoxButton.YesNo);
                ////    if (MessageResult == MessageBoxResult.Yes)
                ////    {
                ////        SaveMeterTOGroup();
                ////        e.Handled = true;
                ////        return;
                ////    }
                ////    else
                ////        MeterToGroupSave = true;
                    
                ////}
                if (MainGrid.SelectedItem != null)
                {
                    IsNew = false;
                    _LlstShowMeterstemp = new List<ShowMeter_Result>();
                    _RlstShowMeterstemp = new List<ShowMeter_Result>();
                    SelectedGroups = (ShowGroups_Result)MainGrid.SelectedItem;
                    GroupID = Convert.ToDecimal(SelectedGroups.GroupID);
                    GroupType = Convert.ToInt32(SelectedGroups.GroupType);
                    txtGroupName.Text = SelectedGroups.GroupName;
                    //txtHash.Text = SelectedGroups.HashValue;
                    if (SelectedGroups.GroupType == 1 && SelectedGroups.GroupID != 1)
                    {
                        txtGroupName.IsEnabled = true;
                        //txtHash.IsEnabled = true;
                    }
                    else if (SelectedGroups.GroupType != 1)
                    {
                        txtGroupName.IsEnabled = false;
                        //txtHash.IsEnabled = false;
                    }
                    RefreshLandRGrig();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void SaveMeterTOGroup()
        {
            try
            {
               
                for (int i = 0; i < RGrid.Items.Count; i++)
                {
                     RSelectedMeter = (ShowMeter_Result)RGrid.Items[i];

                     ShowMeterToGroup MeterToGroup = new ShowMeterToGroup(" and Isvisable=1", Convert.ToDecimal(RSelectedMeter.MeterID));
                     if (MeterToGroup._lstShowMeterToGroup.Count ==0)
                     {
                         MessageBoxResult MBResult = MessageBox.Show(tr.TranslateofLable.Object8 + RSelectedMeter.MeterNumber.ToString() + tr.TranslateofLable.Object9, "", MessageBoxButton.YesNo);
                        if (MBResult == MessageBoxResult.No)
                            return;
                     }
                }
                MeterToGroupSave = true;
                for (int i = 0; i < RGrid.Items.Count; i++)
                {
                    RSelectedMeter = (ShowMeter_Result)RGrid.Items[i];
                    SQLSPS.DelMeterToGroup(RSelectedMeter.MeterID, GroupID, GroupType, Result, ErrMsg);                   
                    
                }
                for (int i = 0; i < LGrid.Items.Count; i++)
                {
                    LSelectedMeter = (ShowMeter_Result)LGrid.Items[i];
                    SQLSPS.InsMeterToGroup(LSelectedMeter.MeterID, GroupID, GroupType, Result, ErrMsg);                   
                    
                }

                Refresh();

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void btnSendOne_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                if (RSelectedMeter == null )
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message18);
                    return;
                }
                if ((GroupID == 1000000 || GroupID == null) && !IsNew)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message19);
                    return;
                }
                MeterToGroupSave = false;
                for (int i = 0; i < RGrid.SelectedItems.Count; i++)
                {
                    RSelectedMeter = (ShowMeter_Result)RGrid.SelectedItems[i];
                    //SQLSPS.InsMeterToGroup(RSelectedMeter.MeterID, GroupID, SelectedGroups.GroupType, Result, ErrMsg);                   
                    _LlstShowMeterstemp.Add(RSelectedMeter);
                    _RlstShowMeterstemp.Remove(RSelectedMeter);
                }
                RefreshLandRGrigtemp();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void RGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RSelectedMeter = (ShowMeter_Result)RGrid.SelectedItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void LGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LSelectedMeter = (ShowMeter_Result)LGrid.SelectedItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btngetOne_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MeterToGroupSave = false;
                if (LSelectedMeter == null)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message18);
                    return;
                }
                if ((GroupID == 1000000 || GroupID == null) && !IsNew)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message19);
                    return;
                }
                for (int i = 0; i < LGrid.SelectedItems.Count; i++)
                {
                    LSelectedMeter = (ShowMeter_Result)LGrid.SelectedItems[i];
                    //SQLSPS.DelMeterToGroup(LSelectedMeter.MeterID, GroupID, SelectedGroups.GroupType, Result, ErrMsg);                   
                    _LlstShowMeterstemp.Remove(LSelectedMeter);
                    _RlstShowMeterstemp.Add(LSelectedMeter);
                }
                RefreshLandRGrigtemp();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btnSendAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MeterToGroupSave = false;
                if ((GroupID == 1000000 || GroupID == null) && !IsNew)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message19);
                    return;
                }
                int count = RGrid.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    RSelectedMeter = (ShowMeter_Result)RGrid.Items[i];
                    //SQLSPS.InsMeterToGroup(RSelectedMeter.MeterID, GroupID,SelectedGroups.GroupType, Result, ErrMsg);       
                    _LlstShowMeterstemp.Add(RSelectedMeter);
                    
                }
                _RlstShowMeterstemp.RemoveRange(0, count);
                RefreshLandRGrigtemp();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btngetAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MeterToGroupSave = false;
                if ((GroupID == 1000000 || GroupID == null) && !IsNew)
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message19);
                    return;
                }
                int count = LGrid.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    LSelectedMeter = (ShowMeter_Result)LGrid.Items[i];
                    //SQLSPS.DelMeterToGroup(LSelectedMeter.MeterID, GroupID, SelectedGroups.GroupType, Result, ErrMsg);                    
                    _RlstShowMeterstemp.Add(LSelectedMeter);
                }
                _LlstShowMeterstemp.RemoveRange(0, count);
                RefreshLandRGrigtemp();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void MainGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcount.Content =rowcount+"="+ MainGrid.Items.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void RGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcountR.Content =rowcount+"="+ RGrid.Items.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void LGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                string rowcount = "";
                if (CommonData.mainwindow != null)
                    rowcount = CommonData.mainwindow.tr.TranslateofLable.Object72;
                lblcountL.Content =rowcount+"="+ LGrid.Items.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        

        
	}
}