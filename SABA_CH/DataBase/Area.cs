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
    
    public partial class Area
    {
        public Area()
        {
            this.Locations = new HashSet<Location>();
        }
    
        public decimal AreaID { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
    
        public virtual ICollection<Location> Locations { get; set; }
    }
}