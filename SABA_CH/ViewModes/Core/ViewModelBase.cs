using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SABA_CH.ViewModels.Core
{
    public class ViewModelBase : PropertyNotificationObject
    {
        public bool IsBusy { get; set; }

        protected void ShowError(Exception ex, String operationName)
        {
            //ExceptionViewer ev = new ExceptionViewer(operationName, ex);
            //ev.Show();
        }

        protected void ShowErrors(List<Exception> ex, String operationName)
        {
            //ExceptionViewer ev = new ExceptionViewer(operationName, ex);
            //ev.Show();
        }

        bool? isDesignMode;
        public bool IsDesignMode
        {
            get
            {
                if (isDesignMode == null)
                    isDesignMode = (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue);
                return (bool)isDesignMode;
            }
        }

        protected void UpdateUI()
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(CommandManager.InvalidateRequerySuggested));
        }
    }
}
