//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HHUReaderTest
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class SabaCandHTestGEntities : DbContext
    {
        public SabaCandHTestGEntities()
            : base("name=SabaCandHTestGEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<ShowConsumedWaterForVee_Result> ShowConsumedWaterForVee(string filter)
        {
            var filterParameter = filter != null ?
                new ObjectParameter("Filter", filter) :
                new ObjectParameter("Filter", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ShowConsumedWaterForVee_Result>("ShowConsumedWaterForVee", filterParameter);
        }
    
        public virtual ObjectResult<ShowVEEConsumedWaterForVEE_Result> ShowVEEConsumedWaterForVEE(string filter)
        {
            var filterParameter = filter != null ?
                new ObjectParameter("Filter", filter) :
                new ObjectParameter("Filter", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ShowVEEConsumedWaterForVEE_Result>("ShowVEEConsumedWaterForVEE", filterParameter);
        }
    }
}
