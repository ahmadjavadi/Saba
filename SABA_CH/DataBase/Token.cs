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
    
    public partial class Token
    {
        public decimal TokenID { get; set; }
        public string USBDeviceCode { get; set; }
        public decimal UserID { get; set; }
        public string ComputerCode { get; set; }
        public int SequenceNumber { get; set; }
        public string Token1 { get; set; }
        public string BuildDate { get; set; }
        public decimal CardID { get; set; }
        public decimal Hash { get; set; }
        public decimal MeterID { get; set; }
        public string CreditTransferModes { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public decimal CreditValue { get; set; }
        public string USBSignature { get; set; }
        public Nullable<decimal> OBISValueHeaderID { get; set; }
        public Nullable<int> credit_Capability_Activation { get; set; }
        public string creditStartDate { get; set; }
        public Nullable<int> disconnectivity_On_Negative_Credit { get; set; }
        public Nullable<int> disconnectivity_On_Expired_Credit { get; set; }
        public bool Isvalid { get; set; }
    
        public virtual Meter Meter { get; set; }
        public virtual User User { get; set; }
    }
}
