using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Windows;
using System.Windows.Controls;
using SABA_CH.DataBase;
using SABA_CH.Global;

using Button = System.Windows.Controls.Button; 
namespace SABA_CH
{
	/// <summary>
	/// Interaction logic for InfoShowRegulation.xaml
	/// </summary>
	public partial class InfoShowRegulation : System.Windows.Window
	{
        public int Windowid;
        public ObjectParameter Result = new ObjectParameter("Result", 1000000);       
        public ObjectParameter ErrMsg = new ObjectParameter("ErrMsg", "");
        public ShowWindows_Result SelectedWindow;
        public ShowDiatinctOBISs_Result RSelectedObis;
        public ShowDiatinctOBISs_Result LSelectedObis;
        public ShowTranslateofLable tr = null;
		public readonly int WindowId=6;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
		public InfoShowRegulation()
		{
			this.InitializeComponent();
            RefreshMainGrid();
            tr = CommonData.translateWindow(6);
            TranslateGrids();
            ChangeFlowDirection();
			// Insert code required on object creation below this point.
		}
        public void ChangeFlowDirection()
        {
            try
            {
                //MGrid.FlowDirection = CommonData.FlowDirection;
                //DGrid.FlowDirection = CommonData.FlowDirection;
                //RGrid.FlowDirection = CommonData.FlowDirection;
                //LGrid.FlowDirection = CommonData.FlowDirection;
                //GridMain.FlowDirection = CommonData.FlowDirection;
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
                GridMain.Columns[0].Header = tr.TranslateofLable.Object1;
                LGrid.Columns[0].Header = tr.TranslateofLable.Object3;                
                RGrid.Columns[0].Header = tr.TranslateofLable.Object3;
                
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
        public void RefreshRGrid()
        {
            try
            {
                string rFilter = "and Main.OBISID not in (Select OBISID From OBISToWindow where WindowID="+Windowid.ToString()+")";
                ShowDiatinctObiSs OBIS = new ShowDiatinctObiSs(rFilter);
                RGrid.ItemsSource = null;
                RGrid.ItemsSource = OBIS._lstShowDiatinctOBISs;
                if (RGrid.Items.Count > 0)
                    RGrid.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshLGrid()
        {
            try
            {
                string LFilter = "and Main.OBISID  in (Select OBISID From OBISToWindow where WindowID=" + Windowid.ToString() + ")";
                ShowDiatinctObiSs OBIS = new ShowDiatinctObiSs(LFilter);
                LGrid.ItemsSource = null;
                LGrid.ItemsSource = OBIS._lstShowDiatinctOBISs;
                if (LGrid.Items.Count > 0)
                    LGrid.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RefreshMainGrid()
        {
            try
            {
                string Filter = "";
                ShowWindows window = new ShowWindows(Filter);
                GridMain.ItemsSource = null;
                GridMain.ItemsSource = window.CollectShowWindows;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void GridMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (GridMain.SelectedItem!=null)
                {
                    SelectedWindow = (ShowWindows_Result)GridMain.SelectedItem;
                    Windowid = SelectedWindow.WindowID;
                    RefreshLGrid();
                    RefreshRGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btnsendone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Result = new ObjectParameter("Result", 1000000);       
                ErrMsg = new ObjectParameter("ErrMsg", "");
                if (SelectedWindow == null)
                {
                    MessageBox.Show(" لطفا یک پنجره را انتخاب کنید");
                    return;
                }
                if (RSelectedObis==null)
                {
                    MessageBox.Show(" لطفا یک OBIS را انتخاب کنید");
                    return;
                }
                for (int i = 0; i < RGrid.SelectedItems.Count; i++)
                {
                    RSelectedObis = (ShowDiatinctOBISs_Result)RGrid.SelectedItems[i];
                    SQLSPS.InsObisToWindow(RSelectedObis.OBISID, Windowid, Result, ErrMsg);
                    SQLSPS.UPDOBISsType(SelectedWindow.OBIStypeID.ToString(),Convert.ToDecimal( RSelectedObis.OBISID), Result, ErrMsg,true);
                    
                }
                RefreshLGrid();
                RefreshRGrid();
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
                if (RGrid.SelectedItem!=null)
                {
                    RSelectedObis = (ShowDiatinctOBISs_Result)RGrid.SelectedItem;
                }
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
                if (LGrid.SelectedItem != null)
                {
                    LSelectedObis = (ShowDiatinctOBISs_Result)LGrid.SelectedItem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btnsendAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Result = new ObjectParameter("Result", 1000000);
                ErrMsg = new ObjectParameter("ErrMsg", "");
                if (SelectedWindow == null)
                {
                    MessageBox.Show(" لطفا یک پنجره را انتخاب کنید");
                    return;
                }
                if (RSelectedObis == null)
                {
                    MessageBox.Show(" لطفا یک OBIS را انتخاب کنید");
                    return;
                }
                for (int i = 0; i < RGrid.Items.Count; i++)
                {
                    RSelectedObis = (ShowDiatinctOBISs_Result)RGrid.Items[i];
                    SQLSPS.InsObisToWindow(RSelectedObis.OBISID, Windowid, Result, ErrMsg);
                    if (i < RGrid.Items.Count)
                        SQLSPS.UPDOBISsType(SelectedWindow.OBIStypeID.ToString(), Convert.ToDecimal(RSelectedObis.OBISID), Result, ErrMsg, false);
                    else
                        SQLSPS.UPDOBISsType(SelectedWindow.OBIStypeID.ToString(), Convert.ToDecimal(RSelectedObis.OBISID), Result, ErrMsg, true);

                }
                RefreshLGrid();
                RefreshRGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btnGetone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Result = new ObjectParameter("Result", 1000000);
                ErrMsg = new ObjectParameter("ErrMsg", "");
                if (SelectedWindow == null)
                {
                    MessageBox.Show(" لطفا یک پنجره را انتخاب کنید");
                    return;
                }
                if (LSelectedObis == null)
                {
                    MessageBox.Show(" لطفا یک OBIS را انتخاب کنید");
                    return;
                }
                for (int i = 0; i < LGrid.SelectedItems.Count; i++)
                {
                    LSelectedObis = (ShowDiatinctOBISs_Result)LGrid.SelectedItems[i];
                    SQLSPS.DelObisToWindow(LSelectedObis.OBISID, Windowid, Result, ErrMsg);
                    SQLSPS.DelOBISsType(SelectedWindow.OBIStypeID.ToString(), Convert.ToDecimal(RSelectedObis.OBISID), Result, ErrMsg, true);
                }
                RefreshLGrid();
                RefreshRGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private void btnGetAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Result = new ObjectParameter("Result", 1000000);
                ErrMsg = new ObjectParameter("ErrMsg", "");
                if (SelectedWindow == null)
                {
                    MessageBox.Show(" لطفا یک پنجره را انتخاب کنید");
                    return;
                }
                if (LSelectedObis == null)
                {
                    MessageBox.Show(" لطفا یک OBIS را انتخاب کنید");
                    return;
                }
                for (int i = 0; i < LGrid.Items.Count; i++)
                {
                    LSelectedObis = (ShowDiatinctOBISs_Result)LGrid.Items[i];
                    SQLSPS.DelObisToWindow(LSelectedObis.OBISID, Windowid, Result, ErrMsg);
                    if (i < RGrid.Items.Count)
                        SQLSPS.DelOBISsType(SelectedWindow.OBIStypeID.ToString(), Convert.ToDecimal(RSelectedObis.OBISID), Result, ErrMsg, false);
                    else
                        SQLSPS.DelOBISsType(SelectedWindow.OBIStypeID.ToString(), Convert.ToDecimal(RSelectedObis.OBISID), Result, ErrMsg, true);

                    
                }
                RefreshLGrid();
                RefreshRGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
	}
}