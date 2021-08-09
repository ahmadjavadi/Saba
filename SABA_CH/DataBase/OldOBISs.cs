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
    
    public partial class OldOBISs
    {
        public OldOBISs()
        {
            this.OBISToSoftversions = new HashSet<OBISToSoftversion>();
            this.StatusOBISsDescs = new HashSet<StatusOBISsDesc>();
            this.Windows = new HashSet<Window>();
        }
    
        public decimal OBISID { get; set; }
        public string Obis { get; set; }
        public string ObisCode { get; set; }
        public string ObisFarsiDesc { get; set; }
        public string ObisLatinDesc { get; set; }
        public string ObisArabicDesc { get; set; }
        public string Type { get; set; }
        public Nullable<decimal> DeviceTypeID { get; set; }
        public Nullable<decimal> OBISUnitID { get; set; }
        public Nullable<decimal> ObisTypeID { get; set; }
        public string Format { get; set; }
        public Nullable<int> ClassID { get; set; }
        public string CardFormatType { get; set; }
        public string HHuFormatType { get; set; }
        public Nullable<int> UnitConvertType { get; set; }
        public Nullable<decimal> UnitIDForshow { get; set; }
        public string FixedOBISCode { get; set; }
    
        public virtual UnitGroup UnitGroup { get; set; }
        public virtual ICollection<OBISToSoftversion> OBISToSoftversions { get; set; }
        public virtual ICollection<StatusOBISsDesc> StatusOBISsDescs { get; set; }
        public virtual ICollection<Window> Windows { get; set; }
    }
}
