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
    
    public partial class ConsumedMaxDemand
    {
        public decimal MeterID { get; set; }
        public string MaxDemandValue { get; set; }
        public string ConsumedDate { get; set; }
        public Nullable<decimal> OBISID { get; set; }
        public string ReadDate { get; set; }
        public string DateOfReceivedFromSource { get; set; }
        public Nullable<decimal> OBISHeaderID { get; set; }
    }
}