using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DTOEntities
{
    [Serializable]
    internal class ActionDTO
    {
        [DefaultValue(null)]
        public DataDTO Parameter;

        [DefaultValue(false)]
        public bool IsTriggered;

        [DefaultValue("")]
        public string ErrorMessage;
    }
}
