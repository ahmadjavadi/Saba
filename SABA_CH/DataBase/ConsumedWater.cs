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
    
    public partial class ConsumedWater
    {
        public decimal MeterID { get; set; }
        public string ConsumedWater1 { get; set; }
        public string VEEConsumedWater { get; set; }
        public string ConsumedDate { get; set; }
        public Nullable<decimal> OBISID { get; set; }
        public string ReadDate { get; set; }
        public string DateOfReceivedFromSource { get; set; }
        public Nullable<decimal> OBISHeaderID { get; set; }
        public Nullable<bool> ISVEE { get; set; }
        public string TotalConsumedWater { get; set; }
        public string ErrorMessage { get; set; }
        public Nullable<bool> valid { get; set; }
    
        public virtual Meter Meter { get; set; }
    }
}
