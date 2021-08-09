using System.Collections.Generic;
using System.Xml.Serialization;

namespace Card
{
    public class CardReadOut
    {
        [XmlElement("SerialNumber")]
        public string SerialNumber;

        [XmlElement("ReadOutDateTime")]
        public string ReadOutDateTime;

        [XmlElement("SoftwareVersion")]
        public string SoftwareVersion { get; set; }

        [XmlElement("Card_CityCode")]
        public string Card_CityCode;

        [XmlElement("CardNumber")]
        public string CardNumber;

        [XmlElement("CardData")]
        public CardData CardData { get; set; }


        [XmlElement("ErrorMessage")]
        public ErrorMessage ErrorMessage { get; set; }
    }

    #region ReadOut
    public class CardData
    {
        [XmlElement("OBISObjects")]
        public List<OBISObject> ObjectList { get; set; }
    }       
    
    public class OBISObject
    {
        [XmlAttribute()]
        public string code { get; set; }

        [XmlAttribute()]
        public string value { get; set; }

        [XmlAttribute()]
        public string dateTime { get; set; }
    }
  
    #endregion ReadOut

    #region ErrorMessage
    public class ErrorMessage
    {
        [XmlElement("CardErrorMessage")]
        public List<Message> CardErrorMessage { get; set; }

        [XmlElement("MeterErrorMessage")]
        public List<Message> MeterErrorMessage { get; set; }
    }      
    public class Message
    {
        [XmlAttribute()]
        public string message = "";       
    }

   
    #endregion ErrorMessage
}