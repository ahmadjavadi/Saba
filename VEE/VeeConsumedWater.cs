
namespace VEE
{
    using System;
    public class VeeConsumedWater
    {
        

        public Nullable<decimal> CustomerId { get; set; }
        public Nullable<decimal> MeterId { get; set; }
        public decimal Id { get; set; }
        public string ConsumedDate { get; set; }
        public string Flow { get; set; }
        public string MonthlyConsumption { get; set; }
        public string TotalConsumption { get; set; }
        public Nullable<bool> IsValid { get; set; }
    }
}
