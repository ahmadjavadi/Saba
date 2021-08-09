using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DTOEntities
{
    [Serializable]
    internal class AttributeDTO
    {
        [DefaultValue(null)]
        public DataDTO Data;

        [DefaultValue("")]
        public string ErrorMessage;
    }
}
