using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DLMS.DTOEntities
{
   
        [Serializable]
        public class LstOBISDTO
        {

            [DefaultValue("")]
            public string LogicalName;

            [DefaultValue(null)]
            public int ClaseID;

            [DefaultValue(null)]
            public int AttributeID;

            [DefaultValue(null)]
            public int DataId;

            [DefaultValue(null)]
            public int Level;

            [DefaultValue("")]
            public string ValueString;

            public string TypeString;

        }
    }
