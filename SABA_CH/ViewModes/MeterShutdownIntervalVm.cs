using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SABA_CH.ViewModels.Core;

namespace SABA_CH.ViewModes
{
    public class MeterShutdownIntervalVm : ViewModelBase
    {
        private static MeterShutdownIntervalVm _instanceVm;

        public static MeterShutdownIntervalVm Instance
        {
            get
            {
                if (_instanceVm == null)
                    _instanceVm = new MeterShutdownIntervalVm();
                return _instanceVm;
            }
        }

        public MeterShutdownIntervalVm()
        {
            MeterIsShutdown = false;
            _instanceVm = this;
            MeterIsShutdown = true;
            StartDate = "1399/04/02";
            EndDate = "1399/05/05";

        }

        #region StartDate
        private string _startDate;
        public string StartDate
        {
            get { return _startDate; }

            set
            {
                _startDate = value;
                NotifyPropertyChanged("StartDate");
            }
        }
        #endregion

        #region EndDate
        private string _endDate;
        public string EndDate
        {
            get { return _endDate; }

            set
            {
                _endDate = value;
                NotifyPropertyChanged("EndDate");
            }
        }
        #endregion

        #region MeterIsShutdown
        private bool _meterIsShutdown;

        public bool MeterIsShutdown
        {
            get { return _meterIsShutdown; }

            set
            {
                _meterIsShutdown = value;
                NotifyPropertyChanged("MeterIsShutdown");
            }
        }
        #endregion
    }
}
