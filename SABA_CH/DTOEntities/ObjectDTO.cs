using System;
using System.ComponentModel;

namespace SABA_CH.DTOEntities
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
