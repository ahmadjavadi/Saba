//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Expression.Blend.SampleData.Tarefeh
{
// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class Tarefeh { }
#else

	public class Tarefeh : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public Tarefeh()
		{
			try
			{
				Uri resourceUri = new Uri("/SABA_CH;component/SampleData/Tarefeh/Tarefeh.xaml", UriKind.Relative);
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

		private string _تعرفه = string.Empty;

		public string تعرفه
		{
			get
			{
				return this._تعرفه;
			}

			set
			{
				if (this._تعرفه != value)
				{
					this._تعرفه = value;
					this.OnPropertyChanged("تعرفه");
				}
			}
		}

		private double _مقدار = 0;

		public double مقدار
		{
			get
			{
				return this._مقدار;
			}

			set
			{
				if (this._مقدار != value)
				{
					this._مقدار = value;
					this.OnPropertyChanged("مقدار");
				}
			}
		}
	}

	public class ItemCollection : ObservableCollection<Item>
	{ 
	}
#endif
}
