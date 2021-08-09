using System.Collections.Generic;
using System.Xml.Serialization;

namespace SABA_CH.UI
{
    public  class ExportToSabaData
    {
        [XmlAttribute("ExportDate")]
        public string ExportDate { get; set; }

        [XmlAttribute("FromDate")]
         public string FromDate { get; set; }

        [XmlAttribute("ToDate")]
        public string ToDate { get; set; }

        [XmlAttribute("TotalMeterReadingsCount")]
        public string TotalMeterReadingsCount { get; set; }

        [XmlAttribute("User")]
        public string User { get; set; }

      [XmlElement("MeterReadingsList")] public MeterReadingsList MeterReadingsList = new MeterReadingsList();

    }

    public class MeterReadingsList
    {
        [XmlElement("MeterReading")] public List<MeterReading> MeterReading { get; set; }

    }

    public class MeterReading
    {
        [XmlAttribute("Meter")]
        public string Meter { get; set; }

        [XmlAttribute("ReadingDateTime")]
        public string ReadingDateTime { get; set; }

        [XmlAttribute("MeterDateTime")]
        public string MeterDateTime { get; set; }

        //[XmlElement("User")]
        //public string User1 { get; set; }

        [XmlElement("ReadOuts")] public ReadOuts ReadOuts = new ReadOuts();

        [XmlElement("CurveInformation")] public CurveInformation CurveInformation = new CurveInformation();

        [XmlElement("Billing")] public Billing Billing = new Billing();
    }

    public class ReadOuts
    {
        [XmlElement("Object")]
        public List<Object> Object { get; set; }

    }
    public class Object
    {
        [XmlAttribute("Obis")]
        public string Obis { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }

    }

    public class CurveInformation
    {
      
        [XmlElement("Object")]
        public List<ObjectCurve> ObjectCurve { get; set; }

      
    }
    public class ObjectCurve
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }

    }
    public class Billing
    {
        [XmlElement("Bill")]
        public List<Bill> Bill { get; set; }
    }

    public class Bill
    {
        [XmlAttribute("DateTime")]
        public string DateTime { get; set; }

        [XmlAttribute("WaterConsumption")]
        public string WaterConsumption { get; set; }

        [XmlAttribute("TimeOfPumpWorking")]
        public string TimeOfPumpWorking { get; set; }
    }


   
    //#region ErrorMessage
    //public class ErrorMessage
    //{
    //    [XmlElement("CardErrorMessage")]
    //    public List<Message> CardErrorMessage { get; set; }

    //    [XmlElement("MeterErrorMessage")]
    //    public List<Message> MeterErrorMessage { get; set; }
    //}
    //public class Message
    //{
    //    [XmlAttribute()]
    //    public string message = "";
    //}


    //#endregion ErrorMessage


}
