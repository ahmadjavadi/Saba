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
    
    public partial class Button
    {
        public Button()
        {
            this.ButtonAccesses = new HashSet<ButtonAccess>();
        }
    
        public decimal ButtonID { get; set; }
        public string ButtonName { get; set; }
        public string ButtonDesc { get; set; }
        public string ButtonArabicDesc { get; set; }
        public Nullable<bool> IsVisable { get; set; }
    
        public virtual ICollection<ButtonAccess> ButtonAccesses { get; set; }
    }
}
