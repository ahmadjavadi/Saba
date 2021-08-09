using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DLMS.DTOEntities
{
    [Serializable]
    public class AttributeDTO
    {
        [DefaultValue(null)]
        public int AttributeID;

        [DefaultValue(null)]
        public DataDTO Data;

        [DefaultValue("")]
        public string ErrorMessage;
    }
}
