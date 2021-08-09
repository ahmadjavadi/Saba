using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication_SABA
{
	/// <summary>
	/// Interaction logic for NewSource.xaml
	/// </summary>
	public partial class NewSource: Window
	{
		public readonly int windowID=39;
		 private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
		public NewSource()
		{
		this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

		private void Window_Activated(object sender, System.EventArgs e)
		{
			 tabCtrl.SelectedItem = tabPag;
            if (!tabCtrl.IsVisible)
            {

                tabCtrl.Visibility = Visibility.Visible;

            }
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			 tabCtrl.Items.Remove(tabPag);
            if (!tabCtrl.HasItems)
            {

                tabCtrl.Visibility = Visibility.Hidden;

            }
		}
	}
}