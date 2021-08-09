using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterStatus
{
  public   class Status_Result
    {
        public Status  IsStatuseTrue { get; set; }
        public string Description { get; set; }


        public  Status_Result(Status isStatuseTrue,string dec)
        {
            IsStatuseTrue = isStatuseTrue;
            Description = dec;
        }

    }
}
