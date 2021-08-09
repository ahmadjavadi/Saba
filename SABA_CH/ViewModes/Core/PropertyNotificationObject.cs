using System;
using System.ComponentModel;

namespace SABA_CH.ViewModels.Core
{
    public class PropertyNotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void UnsubscribePropertyChanged()
        {
            PropertyChanged = null;
        }
    }
}
