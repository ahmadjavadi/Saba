using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DLMS.DTOEntities
{
    [Serializable]
    public class ObjectDTO
    {
        public ObisDTO Obis;
        public AttributeDTO[] Attributes;
        public ActionDTO[] Actions;

        [DefaultValue(false)]
        public bool IsDirty;

        [DefaultValue(false)]
        public bool? IsRead;

        [DefaultValue(false)]
        public bool IsTriggered;
    }
}
