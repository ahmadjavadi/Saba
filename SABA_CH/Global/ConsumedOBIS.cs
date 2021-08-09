using System.ComponentModel;

namespace SABA_CH.Global
{
    class ConsumedOBIS
    {
        public string ConsumedWater{get;set;}
        public string OBISDesc{get;set;}
        public string ConsumedDate{get;set;}
        public string MeterID{get;set;}
        public string MeterNumber{get;set;}
    }
   public  class ReverseConsumedOBIS
    {
       public string _rowName;
       public string _column0Value;
       public string _column1Value;
       public string _column0Name;
       public string _column1Name;
       public string _meterNumber;


       public string RowName
       {
           get { return _rowName; }
           set
           {
               _rowName = value;
               NotifyPropertyChanged("RowName");
           }
       }

       public string Column0Value
       {
           get { return _column0Value; }
           set
           {
               _column0Value = value;
               NotifyPropertyChanged("Column0Value");
           }
       }

       public string Column1Value
       {
           get { return _column1Value; }
           set
           {
               _column1Value = value;
               NotifyPropertyChanged("Column1Value");
           }
       }
       public string Column0Name
       {
           get { return _column0Name; }
           set
           {
               _column0Name = value;
               NotifyPropertyChanged("Column0Name");
           }
       }
       public string Column1Name
       {
           get { return _column1Name; }
           set
           {
               _column1Name = value;
               NotifyPropertyChanged("Column1Name");
           }
       }
       public string MeterNumber
       {
           get { return _meterNumber; }
           set
           {
               _meterNumber = value;
               NotifyPropertyChanged("MeterNumber");
           }
       }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
