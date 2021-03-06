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
    
    public partial class Credit
    {
        public decimal Id { get; set; }
        public int Credit1 { get; set; }
        public int Type { get; set; }
        public decimal CustomerId { get; set; }
        public decimal UserId { get; set; }
        public string CreateDate { get; set; }
        public bool IsConfirmed { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<bool> UsedByMeter { get; set; }
        public Nullable<bool> TransferedToCard { get; set; }
        public string MeterNumber { get; set; }
        public string CardNumber { get; set; }
        public Nullable<bool> IsValid { get; set; }
    
        public virtual Credit Credits1 { get; set; }
        public virtual Credit Credit2 { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual User User { get; set; }
    }
}
