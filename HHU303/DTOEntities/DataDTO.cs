using System;
using System.ComponentModel;

namespace DTOEntities
{
    [Serializable]
    public class DataDTO
    {
        public string TypeString;

        [DefaultValue("")]
        public string ValueString;

        [DefaultValue(false)]
        public bool IsDirty;

        [DefaultValue(null)]
        public int DataID;

        [DefaultValue(null)]
        public int Level;

        public DataDTO[] Items;
    }
}
