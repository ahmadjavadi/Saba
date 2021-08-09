﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DLMS.DTOEntities
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