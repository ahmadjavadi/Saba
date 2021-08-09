using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DLMS.DTOEntities
{
    [Serializable]
    public class ObisDTO
    {
        public string LogicalName;
        public ushort ClassId;
    }
}
