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
    
    public partial class SoftversionToDeviceModel
    {
        public SoftversionToDeviceModel()
        {
            this.Meters = new HashSet<Meter>();
            this.Modems = new HashSet<Modem>();
        }
    
        public decimal SoftversionToDeviceModelID { get; set; }
        public string Softversion { get; set; }
        public decimal DeviceModelID { get; set; }
        public string OnMeterName { get; set; }
    
        public virtual ICollection<Meter> Meters { get; set; }
        public virtual ICollection<Modem> Modems { get; set; }
    }
}
