using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH
{
	/// <summary>
	/// Interaction logic for WaterPublicInfo.xaml
	/// </summary>
	public partial class WaterPublicInfo : System.Windows.Window
    {
		public readonly int WindowId=13;
        private TabControl _tabCtrl;
        private TabItem _tabPag;
        public ShowTranslateofLable tr = null;
        public decimal MeterId = 100000000;
        public string ReadDate = "";
        public string ReportType = "";
        public TabItem TabPag
        {
            get { return _tabPag; }

            set { _tabPag = value; }
        }
        public TabControl Tab { set { _tabCtrl = value; } }
		public WaterPublicInfo()
		{
			this.InitializeComponent();
            RereshGridHeader();
            RefreshDatagridgeneralWater();
            ChangeLanguage();
            ChangeFlowDirection();
            TranslateHeaderOfDatagridgeneralWater();
            
			// Insert code required on object creation below this point.
		}
        public void ChangeLanguage()
        {
            try
            {
                tr = CommonData.translateWindow(WindowId);
                MainGrid.DataContext = tr.TranslateofLable;
                this.DataContext = tr.TranslateofLable;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }
		private void Window_Activated(object sender, EventArgs e)
		{
			 _tabCtrl.SelectedItem = _tabPag;
            if (!_tabCtrl.IsVisible)
            {

                _tabCtrl.Visibility = Visibility.Visible;

            }
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
            try
            {
                ClassControl.OpenWin[8] = false;
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
        public void RefreshDatagridgeneralWater()
        {
            try
            {
                DatagridgeneralWater.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                delegate()
                {
                    string filter = "  and (charindex('\"" + ReportType + "\"',obiss.type)>0 or charindex('\"100\"',obiss.type)>0) and Header.MeterID=" + MeterId + " and ReadDate= '" + ReadDate + "'";
                    ShowObisValueDetail value = new ShowObisValueDetail(filter, CommonData.LanguagesID);
                    DatagridgeneralWater.ItemsSource = null;
                    DatagridgeneralWater.ItemsSource = value.CollectShowObisValueDetail;
                }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void RereshGridHeader()
        {
            try
            {
                string Filter="";
                ShowObisValueHeader obisValueHeader=new ShowObisValueHeader(Filter);
                DatagridHeader.ItemsSource = null;
                DatagridHeader.ItemsSource = obisValueHeader._lstShowOBISValueHeader;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void TranslateHeaderOfDatagridgeneralWater()
        {
            try
            {
                DatagridgeneralWater.Columns[0].Header = tr.TranslateofLable.Object8;
                DatagridgeneralWater.Columns[1].Header = tr.TranslateofLable.Object7;
                DatagridgeneralWater.Columns[2].Header = tr.TranslateofLable.Object9;
                DatagridHeader.Columns[0].Header = tr.TranslateofLable.Object1;
                DatagridHeader.Columns[1].Header = tr.TranslateofLable.Object2;

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }

        }

        private void DatagridHeader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ShowOBISValueHeader_Result row = new ShowOBISValueHeader_Result();
                if ( DatagridHeader.SelectedItem!=null)
                {
                    row = (ShowOBISValueHeader_Result)DatagridHeader.SelectedItem;
                    MeterId = row.MeterID;
                    ReadDate = row.ReadDate;
                    RefreshDatagridgeneralWater();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
	}

}