using System;
using System.ComponentModel;

namespace DTOEntities
{
    [Serializable]
    public class ActionDTO
    {
        [DefaultValue(null)]
        public DataDTO Parameter;

        [DefaultValue(false)]
        public bool IsTriggered;

        [DefaultValue("")]
        public string ErrorMessage;
    }
}
