 

namespace SABA_CH.Models.Credit
{
    class WaterSubscriberGrid
    {
        public string WaterSubscriber_No { get; set; }


        #region WaterSubscriberName
        private string _WaterSubscriberName;
        public string WaterSubscriberName
        {
            get { return _WaterSubscriberName; }
            set
            {
                _WaterSubscriberName = value;
                //NotifyPropertyChanged("WaterSubscriberName");
            }
        }
        #endregion

        #region WaterSubscriberFamily
        private string _WaterSubscriberFamily;
        public string WaterSubscriberFamily
        {
            get { return _WaterSubscriberFamily; }
            set
            {
                _WaterSubscriberFamily = value;
                //NotifyPropertyChanged("Clock");
            }
        }
        #endregion

        #region lastCreditAnnualDate
        private string _lastCreditAnnualDate;
        public string lastCreditAnnualDate
        {
            get { return _lastCreditAnnualDate; }
            set
            {
                _lastCreditAnnualDate = value;
                //NotifyPropertyChanged("lastCreditAnnualDate");
            }
        }
        #endregion

        #region StartDate
        private string _StartDate;
        public string StartDate
        {
            get { return _StartDate; }
            set
            {
                _StartDate = value;
                // NotifyPropertyChanged("Clock");
            }
        }
        #endregion

        #region EndDate
        private string _EndDate;
        public string EndDate
        {
            get { return _EndDate; }
            set
            {
                _EndDate = value;
                // NotifyPropertyChanged("Clock");
            }
        }
        #endregion

        #region CreditTransferToMetterStatus
        private string _CreditTransferToMetterStatus;
        public string CreditTransferToMetterStatus
        {
            get { return _CreditTransferToMetterStatus; }
            set
            {
                _CreditTransferToMetterStatus = value;
                // NotifyPropertyChanged("Clock");
            }
        }
        #endregion

        #region CreditTransferToCardStatus
        private string _CreditTransferToCardStatus;
        public string CreditTransferToCardStatus
        {
            get { return _CreditTransferToCardStatus; }
            set
            {
                _CreditTransferToCardStatus = value;
                // NotifyPropertyChanged("Clock");
            }
        }
        #endregion



        public WaterSubscriberGrid(string WaterSubscriberNo, string NameWaterSubscriber, string FamilyWaterSubscriber, string lastCreditAnnual,
                                   string StartD, string EndD, string StatusCreditTransferToMetter, string StatusCreditTransferToCard)
        {
            if (WaterSubscriberNo != null)
            {
                WaterSubscriber_No = WaterSubscriberNo;
                WaterSubscriberName = NameWaterSubscriber;
                WaterSubscriberFamily = FamilyWaterSubscriber;
                lastCreditAnnualDate = lastCreditAnnual;
                StartDate = StartD;
                EndDate = EndD;
                CreditTransferToMetterStatus = StatusCreditTransferToMetter;
                CreditTransferToCardStatus = StatusCreditTransferToCard;

            }
            else
            {

            }
        }
    }
}
