//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Expression.Blend.SampleData.newgroup
{
// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class newgroup { }
#else

	public class newgroup : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public newgroup()
		{
			try
			{
				Uri resourceUri = new Uri("/SABA_CH;component/SampleData/newgroup/newgroup.xaml", UriKind.Relative);
				if (Application.GetResourceStream(resourceUri) != null)
				{
					Application.LoadComponent(this, resourceUri);
				}
			}
			catch (Exception)
			{
			}
		}

		private ItemCollection _Collection = new ItemCollection();

		public ItemCollection Collection
		{
			get
			{
				return this._Collection;
			}
		}
	}

	public class Item : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private bool _حذف = false;

		public bool حذف
		{
			get
			{
				return this._حذف;
			}

			set
			{
				if (this._حذف != value)
				{
					this._حذف = value;
					this.OnPropertyChanged("حذف");
				}
			}
		}

		private bool _ويرايش = false;

		public bool ويرايش
		{
			get
			{
				return this._ويرايش;
			}

			set
			{
				if (this._ويرايش != value)
				{
					this._ويرايش = value;
					this.OnPropertyChanged("ويرايش");
				}
			}
		}
	}

	public class ItemCollection : ObservableCollection<Item>
	{ 
	}
#endif
}
