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
    
    public partial class ShowButtonAccess_Result
    {
        public decimal ButtonID { get; set; }
        public string ButtonName { get; set; }
        public string ButtonDesc { get; set; }
        public bool CanShow { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanInsert { get; set; }
        public bool CanImportFromFile { get; set; }
        public decimal UserID { get; set; }
        public string UserName { get; set; }
    }
}