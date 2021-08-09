using System.Collections.Generic;
using System.Xml.Serialization;

namespace HandHeldReader
{
    public class HHUReadOut
    {

       
        /// <summary>        
        /// <para> 0: Find new readout in the HHU  </para>       
        /// <para>-1: Error In Connection with HHU  </para>       
        /// <para>-2: Dont Find \Program Files\HHU\ReadOut\New Folder   </para>            
        /// <para>-3: Cannot Cody Data From HHU To C Drive    </para>    
        /// <para>-4: Cannot Move Data To Old ReadOut  </para>      
        /// <para>-5: HHU\ReadOut\New  is Empty  </para>
        /// </summary>
        [XmlAttribute()]        
        public string ErrorCode = ""; 

        [XmlElement("ReadOut")]
        public List<ReadOut> ReadOutList { get; set; }
    }

    public class ReadOut
    {
        [XmlAttribute()]
        public string SerialNumber = "";

        [XmlAttribute()]
        public string ReadOutDateTime = "";

        [XmlAttribute()]
        public string SoftwareVersion = "";

        [XmlElement("OBISObjectsList")]
        public OBISObjectsList OBISObjectList { get; set; }      
    }

    public class OBISObjectsList
    {
        [XmlElement("OBISObjects")]
        public List<OBISObject> OBISObjectList { get; set; }
    }
    
 

    #region OBISObject
    public class OBISObject
    {
        [XmlAttribute()]
        public string code { get; set; }

        [XmlAttribute()]
        public string value { get; set; }

        [XmlAttribute()]
        public string fixedCode { get; set; }
    }
    #endregion OBISObject
}