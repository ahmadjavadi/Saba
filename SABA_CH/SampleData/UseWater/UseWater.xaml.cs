﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Expression.Blend.SampleData.UseWater
{
// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class UseWater { }
#else

	public class UseWater : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public UseWater()
		{
			try
			{
				Uri resourceUri = new Uri("/SABA_CH;component/SampleData/UseWater/UseWater.xaml", UriKind.Relative);
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

		private double _تاریخ__قرائت = 0;

		public double تاریخ__قرائت
		{
			get
			{
				return this._تاریخ__قرائت;
			}

			set
			{
				if (this._تاریخ__قرائت != value)
				{
					this._تاریخ__قرائت = value;
					this.OnPropertyChanged("تاریخ__قرائت");
				}
			}
		}

		private double _منبع__قرائت___مصرف = 0;

		public double منبع__قرائت___مصرف
		{
			get
			{
				return this._منبع__قرائت___مصرف;
			}

			set
			{
				if (this._منبع__قرائت___مصرف != value)
				{
					this._منبع__قرائت___مصرف = value;
					this.OnPropertyChanged("منبع__قرائت___مصرف");
				}
			}
		}

		private double _دبی__حداکثر = 0;

		public double دبی__حداکثر
		{
			get
			{
				return this._دبی__حداکثر;
			}

			set
			{
				if (this._دبی__حداکثر != value)
				{
					this._دبی__حداکثر = value;
					this.OnPropertyChanged("دبی__حداکثر");
				}
			}
		}

		private double _دبی__لحظه__ای = 0;

		public double دبی__لحظه__ای
		{
			get
			{
				return this._دبی__لحظه__ای;
			}

			set
			{
				if (this._دبی__لحظه__ای != value)
				{
					this._دبی__لحظه__ای = value;
					this.OnPropertyChanged("دبی__لحظه__ای");
				}
			}
		}
	}

	public class ItemCollection : ObservableCollection<Item>
	{ 
	}
#endif
}
