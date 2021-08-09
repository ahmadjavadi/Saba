using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DTOEntities
{
    [Serializable]
    internal class ObjectDTO
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
