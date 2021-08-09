using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for SelectDataTypeForExportToExcel.xaml
    /// </summary>
    public partial class SelectDataTypeForExportToExcel : System.Windows.Window
    {
        public ShowReports_Result Selectedreport;
        public bool IsNew;
        public readonly int WindowId = 16;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public List<ShowOBISToReport_Result> LstObisReport;
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }

        public TabControl Tab { set { _tabCtrl = value; } }
        public ShowTranslateofLable Tr;
        public SelectDataTypeForExportToExcel()
        {
            InitializeComponent();
            Tr = CommonData.translateWindow(WindowId);
            CreateParentNodeOfTree();
            ChangeFlowDirection();            
            TranslateLabels();
            RefreshGridMain();
        }
        public void TranslateLabels()
        {
            try
            {
                lblReportName.Content = Tr.TranslateofLable.Object1;
                GridMain.Columns[0].Header = Tr.TranslateofLable.Object1;
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
                //MainGrid.FlowDirection = CommonData.FlowDirection;
                //GridMain.FlowDirection = CommonData.FlowDirection;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void CreateParentNodeOfTree()
        {
            try
            {              
                ShowObisType OBISTYPE = new ShowObisType("");
                int i = 0;
                foreach (ShowOBISType_Result obistype in OBISTYPE._lstShowOBISType)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = obistype.TypeDesc;
                    item.Tag = obistype.TypeID;
                    MainTree.Items.Add(item);
                    FillChildOfNodes(item, obistype.TypeID.ToString());
                }
                
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void FillChildOfNodes(TreeViewItem item,string type)
        {
            string Filter;
            try
            {
               
                Filter = "  and (charindex('\"" + type + "\"',Main.type)>0 ) ";
                ShowObiSsDesc obiSs = new ShowObiSsDesc(Filter);
                int i = 0;
                foreach (ShowOBISsDesc_Result obis in obiSs._lstShowOBISsDesc)
                {
                    if (obis.Obis.ToString()!="0000010000FF_0.0.6"  && obis.Obis.ToString()!= "0802606200FF" && obis.Obis.ToString() != "0000603D03FF")
                    {
                        TreeViewItem childitem = new TreeViewItem();
                        childitem.Header = obis.OBISDesc;
                        childitem.Tag = obis.OBISID;
                        item.Items.Add(childitem);
                        i++;
                    }    
                }

                if (type == "12")
                {
                    //Filter = "  and  windowid=4 and languageID=" + CommonData.LanguagesID;
                    // ShowTranslate Customer = new ShowTranslate(Filter);
                    //foreach (ShowTranslate_Result tr in Customer._lstShowTranslate)
                    //{
                    TreeViewItem childitem = new TreeViewItem();
                    childitem.Header = "اطلاعات مشترک"; //tr.ObjectDesc;
                    childitem.Tag = 1;//tr.ObjectID;
                    item.Items.Add(childitem);
                    i++;
                    //}

                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }      
        
        private List<CheckBox> GetSelectedCheckBoxes(ItemCollection items,bool value,CheckBox selectedCheck)
        {
            List<CheckBox> list = new List<CheckBox>();
            foreach (TreeViewItem item in items)
            {
                UIElement elemnt = GetChildControl(item, "chk");
                if (elemnt != null)
                    if (selectedCheck == elemnt)
                    {
                        CheckBox chk = (CheckBox)elemnt;
                        if (chk.IsChecked.Value)
                        {
                            Checkedtreeviewitem( item,true);
                        }
                        if (!chk.IsChecked.Value)
                        {
                            Checkedtreeviewitem(item, false);
                        }
                    }
                
            }

            return list;
        }
        private List<string> GetSelectedCheckBoxesId(TreeViewItem items)
        {
            List<string> list = new List<string>();
            foreach (TreeViewItem child in items.Items)
            {
                UIElement elemnt = GetChildControl(child, "chk");
                    if (elemnt != null)
                    {
                        CheckBox chk = (CheckBox)elemnt;
                        if (chk.IsChecked.Value)
                        {
                            list.Add(child.Tag.ToString());
                        }

                    }
            }

            return list;
        }
        private void Checkedtreeviewitem(TreeViewItem item,bool value)
        {
            foreach (TreeViewItem child in item.Items)
            {
                UIElement elemnt = GetChildControl(child, "chk");
                if (elemnt != null)
                {
                    CheckBox chk = (CheckBox)elemnt;
                    chk.IsChecked = value;
                }
            }
                
                
        }
        private UIElement GetChildControl(DependencyObject parentObject, string childName)
        {

            UIElement element = null;

            if (parentObject != null)
            {
                int totalChild = VisualTreeHelper.GetChildrenCount(parentObject);
                for (int i = 0; i < totalChild; i++)
                {
                    DependencyObject childObject = VisualTreeHelper.GetChild(parentObject, i);

                    if (childObject is FrameworkElement &&
                ((FrameworkElement)childObject).Name == childName)
                    {
                        element = childObject as UIElement;
                        break;
                    }

                    // get its child 
                    element = GetChildControl(childObject, childName);
                    if (element != null) break;
                }
            }

            return element;
        }
        public void IsCheckedChange()
        {
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                
                _tabCtrl.Items.Remove(_tabPag);
                if (!_tabCtrl.HasItems)
                {

                    _tabCtrl.Visibility = Visibility.Hidden;

                }
                ClassControl.OpenWin[WindowId] = false;
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
                toolBar1.DataContext = CommonData.ShowButtonBinding("", WindowId);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string obisType;
            string s;
            GetSelectedId(out obisType,out s);
        }
        public void GetSelectedId(out string obisType, out string s)
        {
            try
            {
                List<string> list;
                obisType = "";
                s = "";
                foreach (TreeViewItem item in MainTree.Items)
                {
                    list = new List<string>();
                    list = GetSelectedCheckBoxesId(item);
                    foreach (string id in list)
                        s = s + id + ",";
                    if (list.Count>0)
                    {
                        UIElement elemnt = GetChildControl(item, "chk");
                        if (elemnt != null)
                        {
                            obisType = obisType + item.Tag + ",";
                        }
                    }
                }
                if (s.Length>0)                
                    s = s.Substring(0, s.Length - 1);
                if (obisType.Length>0)
                 obisType = obisType.Substring(0, obisType.Length - 1);
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                obisType = "";
                s = "";
            }
        }
        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chktemp = (sender as CheckBox);            
            GetSelectedCheckBoxes(MainTree.Items, true, chktemp);
        }

        private void chk_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chktemp = (sender as CheckBox);
            GetSelectedCheckBoxes(MainTree.Items, false, chktemp);
        }

        private void ToolStripButtonNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsNew = true;
                CommonData.New(this.MainGrid);
                txtReportName.Text="";
                UncheckedObisList();
                foreach (TreeViewItem item in MainTree.Items)
                {
                    item.IsExpanded = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void UncheckedObisList()
        {
            try
            {
                foreach (TreeViewItem item in MainTree.Items)
                {
                    UIElement elemnt = GetChildControl(item, "chk");
                    if (elemnt != null)
                    {
                        CheckBox chk = (CheckBox)elemnt;
                        chk.IsChecked = false;

                    }
                    foreach (TreeViewItem child in item.Items)
                    {
                        UIElement childelemnt = GetChildControl(child, "chk");
                            if (childelemnt != null)
                            {
                                CheckBox chk = (CheckBox)childelemnt;
                                chk.IsChecked = false;

                            }
                    }
                }
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
                ObjectParameter reportId = new ObjectParameter("ReportID", 10000000000);
                ObjectParameter result = new ObjectParameter("Result", 10000000000);
                ObjectParameter errMsg = new ObjectParameter("ErrMSG", "");
                if (txtReportName.Text=="")
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message53);
                    return;
                }
                if (IsNew)
                    SQLSPS.InsReports(CommonData.UserID, txtReportName.Text, reportId, result, errMsg);
                else
                {
                    SQLSPS.UpdReports(txtReportName.Text, Selectedreport.ReportID, result, errMsg);
                    reportId.Value = Selectedreport.ReportID;
                }
                string obisTypes = "";
                string obisType = "";
                result = new ObjectParameter("Result", 10000000000);
                errMsg = new ObjectParameter("ErrMSG", "");
                SQLSPS.DelObisToReport(Convert.ToDecimal(reportId.Value), 0, result, errMsg);
       
                foreach (TreeViewItem item in MainTree.Items)
                {
                    List<string> list = new List<string>();
                    list = GetSelectedCheckBoxesId(item);                  
                    if (list.Count > 0)
                    {
                        UIElement elemnt = GetChildControl(item, "chk");
                        if (elemnt != null)
                        {
                            CheckBox chk = (CheckBox)elemnt;
                            obisTypes = obisTypes + item.Tag.ToString() + ",";
                            obisType = item.Tag.ToString();
                        }
                    }                                          
                    for (int i = 0; i < list.Count; i++)
                    {
                        result = new ObjectParameter("Result", 10000000000);
                        errMsg = new ObjectParameter("ErrMSG", "");

                        ////if (Convert.ToDecimal(OBISType) != 12)
                            SQLSPS.InsobisToReport(Convert.ToDecimal(reportId.Value), Convert.ToDecimal(list[i]), null, Convert.ToDecimal(obisType), result, errMsg);

                        ////else
                        ////{
                        ////    //SQLSPS.
                        ////}
                    }
                    
                }
                if (obisTypes.Length > 0 )
                {
                   
                    if (errMsg.Value == "")
                    {
                        result = new ObjectParameter("Result", 10000000000);
                        errMsg = new ObjectParameter("ErrMSG", "");
                        SQLSPS.DelObisTypeToReport(Convert.ToDecimal(reportId.Value), result, errMsg);
                    }
                    if (errMsg.Value == "")
                    {                       
                        if (obisTypes.Length > 0)
                        {
                            obisTypes = obisTypes.Substring(0, obisTypes.Length - 1);
                            string[] obistype = obisTypes.Split(',');
                            for (int i = 0; i < obistype.Length; i++)
                            {
                                result = new ObjectParameter("Result", 10000000000);
                                errMsg = new ObjectParameter("ErrMSG", "");
                                SQLSPS.InsObisTypeToReport(Convert.ToDecimal(reportId.Value), Convert.ToInt32(obistype[i]), result, errMsg);
                            }
                        }

                    }
                    RefreshGridMain();
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message7);
                }
                else
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message34);
                
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در ذخیره اطلاعات");//CommonData.mainwindow.tm.TranslateofMessage.Message92);
            }
        }
        public void RefreshGridMain()
        {
            try
            {
                GridMain.ItemsSource = null;
                ShowReports showreports = new ShowReports("");
                GridMain.ItemsSource = showreports._lstShowReports;
                if (GridMain.Items.Count>0)
                {
                    GridMain.SelectedIndex = 0;
                }
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
                 MessageBoxResult res = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message1, CommonData.mainwindow.tm.TranslateofMessage.Message11, MessageBoxButton.OKCancel);
                 if (res == MessageBoxResult.OK)
                 {
                    ObjectParameter Result = new ObjectParameter("Result", 10000000000);
                    ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");                     
                    Result = new ObjectParameter("Result", 10000000000);
                    ErrMSG = new ObjectParameter("ErrMSG", "");
                    SQLSPS.DelReport(Selectedreport.ReportID, Result, ErrMSG);                     
                    RefreshGridMain();
                 }
                MessageBox.Show("عملیات به درستی انجام شد");//CommonData.mainwindow.tm.TranslateofMessage.Message91);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                MessageBox.Show("اشکال در انجان عملیات");//CommonData.mainwindow.tm.TranslateofMessage.Message92);
            }
        }

        private void ToolStripButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshGridMain();
        }

        private void GridMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Selectedreport = new ShowReports_Result();                
               
                if (GridMain.SelectedItem!=null)
	            {
                    UnCheckedAllNode();
		            Selectedreport=(ShowReports_Result)GridMain.SelectedItem;
                    string filter = "  and Main.ReportID=" + Selectedreport.ReportID;
                    ShowObisToReport obisReport = new ShowObisToReport(filter);
                    LstObisReport = new List<ShowOBISToReport_Result>();
                    LstObisReport = obisReport._lstShowOBISToReport;                    
                    CheckedObisid();
                    IsNew = false;
                    txtReportName.Text = Selectedreport.ReportName;
	            }

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void UnCheckedAllNode()
        {
            try
            {
                foreach (TreeViewItem item in MainTree.Items)
                {
                    item.IsExpanded = true;
                    UIElement elemnt = GetChildControl(item, "chk");
                    if (elemnt != null)
                    {
                        CheckBox chk = (CheckBox)elemnt;
                        chk.IsChecked = false;
                    }

                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void CheckedObisid()
        {
            try
            {
               foreach (TreeViewItem item in MainTree.Items)
                {
                    item.IsExpanded = true;                 

                    CheckCheckBoxesId(item);
                    if (AllChiledIsSelected( item))
                    {
                        UIElement elemnt = GetChildControl(item, "chk");
                        if (elemnt != null)
                        {
                            CheckBox chk = (CheckBox)elemnt;
                            if (!chk.IsChecked.Value)
                                chk.IsChecked = true;
                        }
                    }
                    else if (!AllChiledIsSelected(item))
                    {
                        UIElement elemnt = GetChildControl(item, "chk");
                        if (elemnt != null)
                        {
                            CheckBox chk = (CheckBox)elemnt;
                            if (!chk.IsChecked.Value)
                                chk.IsChecked = false;
                        }
                    }
                }
               
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);                
            }
        }
        public bool AllChiledIsSelected(TreeViewItem item)
        {
            bool allchildSelected = true;
            bool haschild = false;
            foreach (TreeViewItem child in item.Items)
            {
                UIElement elemnt = GetChildControl(child, "chk");
                if (elemnt != null)
                {
                    haschild = true;
                    allchildSelected = true;
                    CheckBox chk = (CheckBox)elemnt;
                    if (!chk.IsChecked.Value)
                        return false;
                }
                else
                    allchildSelected= false;
            }
            if (haschild)
                return allchildSelected;
            else
                return false;
        }
        private void CheckCheckBoxesId(TreeViewItem items)
        {
            string s = ",";
            s = s+GetObisid(items);
            foreach (TreeViewItem child in items.Items)
            {
                
                UIElement elemnt = GetChildControl(child, "chk");
                if (elemnt != null)
                {
                    CheckBox chk = (CheckBox)elemnt;
                    if (s.Contains(","+child.Tag.ToString()+","))
                    {
                        chk.IsChecked = true;
                    }
                    else
                        chk.IsChecked = false; 
                }
            }

            
        }
        private string GetObisid(TreeViewItem item)
        {
            try
            {
                UIElement elemnt = GetChildControl(item, "chk");
                string obiSs = "";
                foreach (var obis in LstObisReport)
                {
                    if (obis.OBISTypeID.ToString() == item.Tag.ToString())
                    {
                        if(obis.OBISID!=81)
                        obiSs = obiSs +obis.OBISID+ ',';
                    }
                }
               
                return obiSs;
                
            }
            catch (Exception)
            {
                return "";
            }
        }

        
    }
}
