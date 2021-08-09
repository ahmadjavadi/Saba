using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHU303.DTOEntities
{
    public class MeterReadout
    {
       
        public string meterNo;
        public List<DLMS.DTOEntities.MeterData> readoutList = new List<DLMS.DTOEntities.MeterData>();
    }
}
