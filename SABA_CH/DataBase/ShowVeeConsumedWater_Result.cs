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
    
    public partial class ShowVeeConsumedWater_Result
    {
        public string MeterNumber { get; set; }
        public decimal Id { get; set; }
        public Nullable<decimal> CustomerId { get; set; }
        public Nullable<decimal> MeterId { get; set; }
        public string ConsumedDate { get; set; }
        public string Flow { get; set; }
        public string MonthlyConsumption { get; set; }
        public string TotalConsumption { get; set; }
        public Nullable<bool> IsValid { get; set; }
        public string PumpWorkingHour { get; set; }
    }
}
