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
    
    public partial class OBISToSoftversion
    {
        public decimal OBISID { get; set; }
        public decimal SoftversionToDeviceModelID { get; set; }
    
        public virtual OldOBISs OldOBISs { get; set; }
    }
}
