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
    
    public partial class ShowAllCreditTokenWithNecessary_Result
    {
        public decimal MeterID { get; set; }
        public string MeterNumber { get; set; }
        public string Value { get; set; }
        public decimal CreditValue { get; set; }
        public decimal TokenID { get; set; }
        public string WatersubscriptionNumber { get; set; }
        public string CustomerName { get; set; }
        public string CreditTransferModes { get; set; }
        public int credit_Capability_Activation { get; set; }
        public string creditStartDate { get; set; }
        public int disconnectivity_On_Negative_Credit { get; set; }
        public int disconnectivity_On_Expired_Credit { get; set; }
    }
}
