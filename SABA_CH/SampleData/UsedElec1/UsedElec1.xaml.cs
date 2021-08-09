﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Expression.Blend.SampleData.UsedElec1
{
// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class UsedElec1 { }
#else

	public class UsedElec1 : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public UsedElec1()
		{
			try
			{
				Uri resourceUri = new Uri("/SABA_CH;component/SampleData/UsedElec1/UsedElec1.xaml", UriKind.Relative);
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

		private string _دیماند = string.Empty;

		public string دیماند
		{
			get
			{
				return this._دیماند;
			}

			set
			{
				if (this._دیماند != value)
				{
					this._دیماند = value;
					this.OnPropertyChanged("دیماند");
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