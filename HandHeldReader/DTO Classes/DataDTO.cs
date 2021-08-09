using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DTOEntities
{
    [Serializable]
    internal class DataDTO
    {
        public string TypeString;

        [DefaultValue("")]
        public string ValueString;

        [DefaultValue(false)]
        public bool IsDirty;

        public DataDTO[] Items;
    }
}
