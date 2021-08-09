using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHU303.DTOEntities
{
   public class HHUReadout
    {
       public string ErrorMessage = "";
       public List<MeterReadout> hhureadout = new List<MeterReadout>();
    }
}
