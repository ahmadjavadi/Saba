using System;
using System.ComponentModel;

namespace DTOEntities
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
