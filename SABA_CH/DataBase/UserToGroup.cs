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
    
    public partial class UserToGroup
    {
        public decimal GroupID { get; set; }
        public decimal UserID { get; set; }
        public bool Valid { get; set; }
        public int GroupType { get; set; }
    
        public virtual User User { get; set; }
    }
}
