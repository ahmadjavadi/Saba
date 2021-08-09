

using SABA_CH.Enums;
using SABA_CH.ViewModels.Core;

namespace SABA_CH.ViewModes.Credit
{
    public class CreditVM : PropertyNotificationObject
    {
        public CreditVM()
        {
            Status = "";
        }
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

        private string _usedDate;
        public string UsedDate
        {
            get { return _usedDate; }
            set
            {
                _usedDate = value;
                NotifyPropertyChanged("UsedDate");
            }
        }

        private CreditTypes _creditType;
        public CreditTypes CreditType
        {
            get { return _creditType; }
            set
            {
                _creditType = value;
                CreditTypeString = ToString();
                NotifyPropertyChanged("CreditType");
            }
        }

        private string _creditTypeString;
        public string CreditTypeString
        {
            get { return _creditTypeString; }
            set
            {
                _creditTypeString = value;
                NotifyPropertyChanged("CreditTypeString");
            }
        }

        private string _volume;
        public string Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                NotifyPropertyChanged("Volume");
            }
        }

        private string _waterSubscriberNo;
        public string WaterSubscriberNo
        {
            get { return _waterSubscriberNo; }
            set
            {
                _waterSubscriberNo = value;
                NotifyPropertyChanged("WaterSubscriberNo");
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyPropertyChanged("UserName");
            }
        }
        private string _status;
        public string Status
        {
            get { return string.IsNullOrEmpty(_status) ? "" : _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public override string ToString()
        {
            string res = "";

            switch (CreditType)
            {
                case CreditTypes.AnnualCredit:
                    res = "اعتبار سالیانه"; break;
                case CreditTypes.ExcessCredit:
                    res = "اعتبار مازاد"; break;
                case CreditTypes.DeductionOfTheYearCredit:
                    res = "اعتبار کسر از سال"; break;
                case CreditTypes.MeterDefectCredit:
                    res = "اعتبار نقص کنتور"; break;
                case CreditTypes.ReplaceTheMeterCredit:
                    res = "اعتبار تعویض کنتور"; break;
                case CreditTypes.ResidualValidityOfTheInitialVolumeCredit:
                    res = "اعتبار باقیمانده حجم اولیه"; break;
                case CreditTypes.Others:
                    res = "سایر"; break;
            }
            // return $"{res}-{Volume}-{StartDate}-{EndDate}";
            return res;
        }
    }
}
