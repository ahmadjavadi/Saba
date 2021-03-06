//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SABA_CH.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Modem
    {
        public Modem()
        {
            this.Meters = new HashSet<Meter>();
        }
    
        public decimal ModemID { get; set; }
        public string ModemNumber { get; set; }
        public string SimNumber { get; set; }
        public decimal SoftversionToDeviceModelID { get; set; }
        public decimal DeviceModelID { get; set; }
        public bool Valid { get; set; }
    
        public virtual DeviceModel DeviceModel { get; set; }
        public virtual ICollection<Meter> Meters { get; set; }
        public virtual SoftversionToDeviceModel SoftversionToDeviceModel { get; set; }
    }
}
