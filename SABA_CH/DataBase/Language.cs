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
    
    public partial class Language
    {
        public Language()
        {
            this.MessageTexts = new HashSet<MessageText>();
            this.Translates = new HashSet<Translate>();
        }
    
        public int LanguagesID { get; set; }
        public string LanguageName { get; set; }
        public string FlowDirection { get; set; }
    
        public virtual ICollection<MessageText> MessageTexts { get; set; }
        public virtual ICollection<Translate> Translates { get; set; }
    }
}
