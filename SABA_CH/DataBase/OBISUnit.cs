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
    
    public partial class OBISUnit
    {
        public OBISUnit()
        {
            this.OBISValueDetails = new HashSet<OBISValueDetail>();
        }
    
        public decimal OBISUnitID { get; set; }
        public string OBISUnitDesc { get; set; }
        public decimal UnitGroupID { get; set; }
    
        public virtual ICollection<OBISValueDetail> OBISValueDetails { get; set; }
    }
}