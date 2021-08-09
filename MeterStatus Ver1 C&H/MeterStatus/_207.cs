using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MeterStatus
{
    public class _207
    {
        public List<Status_Result> StatusRegister207_0802606101FF(string CreditRemained, string errorControlValue, string languageName)
        {
            List<Status_Result> list = new List<Status_Result>();
            errorControlValue = errorControlValue.Replace(" ", "");
            if ((errorControlValue == "") || (CreditRemained == ""))
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, languageName, MeterType._207)));
            }
            else
            {
                char[] chArray = GeneralFunction.Hex2Binary(errorControlValue, 8).ToCharArray();
                if (chArray[7] == '1')
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(1, languageName, MeterType._207)));
                }


                if (chArray[6] == '1')
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(2, languageName, MeterType._207)));
                }
                
                 
                if (chArray[5] == '1')
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(3, languageName, MeterType._207)));
                }
                
                if (chArray[4] == '1')
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(4, languageName, MeterType._207)));
                }
                 
                if (chArray[3] == '1')
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(5, languageName, MeterType._207)));
                }
                else
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(6, languageName, MeterType._207)));
                }

                if (chArray[2] == '1')
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(7, languageName, MeterType._207)));
                }
                else
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(8, languageName, MeterType._207)));
                }

                if (chArray[1] == '1')
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(9, languageName, MeterType._207)));
                }
                else
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(10, languageName, MeterType._207)));
                }
                if (chArray[0] == '1')
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(11, languageName, MeterType._207)));
                }
                else
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(12, languageName, MeterType._207)));
                }
                CreditRemained = CreditRemained.Replace("m3", "");
                CreditRemained = CreditRemained.Replace("/", ".");
                try
                {
                    if (CreditRemained!="0.0")
                    {
                        if (double.Parse(CreditRemained, CultureInfo.InvariantCulture)  < 0.0)
                        {
                            var dec = StatusContext.GetContext(13, languageName, MeterType._207);
                             
                            if (!string.IsNullOrEmpty(dec) && !list.Any(x=>x.Description== dec && x.IsStatuseTrue== Status.dontCare))
                                list.Add(new Status_Result(Status.dontCare, dec));                             
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    throw new Exception(CreditRemained + ex.ToString());
                }
                
            }
            return list;
        }
    }
}