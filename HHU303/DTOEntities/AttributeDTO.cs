﻿using System;
using System.ComponentModel;

namespace DTOEntities
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
