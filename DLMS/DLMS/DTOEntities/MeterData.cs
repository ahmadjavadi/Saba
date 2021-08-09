using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLMS.DTOEntities
{
  public class MeterData
    {
        public string ReadDate;
        public List<LstOBISDTO> MeterObjects = new List<LstOBISDTO>();
    }
}
