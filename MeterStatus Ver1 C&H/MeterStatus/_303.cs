using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MeterStatus
{
    public enum DateType
    {
        status,
        events,
        logfile
    }
    public enum Status
    {
        False,
        True,
        dontCare
    }
    public class _303
    {
        public List<Status_Result> PerformanceMeteroncreditevents(string _0000603F00FF_Value, string _0000603F01FF_Value, string _0000603F02FF_Value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            string dec = "";
            string str3 = "";
            string str4 = "";
            str4 = GeneralFunction.ReverseStatusValue(_0000603F00FF_Value.Substring(0, 8));
            dec = StatusContext.GetContext(0x2c1, language, MeterType._303);
            if (_0000603F01FF_Value.ToUpper() == "0000TRUE")
            {
                list.Add(new Status_Result(Status.True, dec));
            }
            else if (_0000603F01FF_Value.ToUpper() == "000FALSE")
            {
                list.Add(new Status_Result(Status.False, dec));
            }
            str3 = StatusContext.GetContext(0x2c2, language, MeterType._303);
            if (_0000603F02FF_Value.ToUpper() == "0000TRUE")
            {
                list.Add(new Status_Result(Status.True, str3));
            }
            else if (_0000603F02FF_Value.ToUpper() == "000FALSE")
            {
                list.Add(new Status_Result(Status.False, str3));
            }
            return list;
        }

        public List<Status_Result> StatuseGeneralRegister2(string value, string language, int type)
        {
            List<Status_Result> list = new List<Status_Result>();
            int messageID = 0x3e8;
            try
            {
                value = GeneralFunction.ReverseStatusValue(value.Substring(0, 8));
                if (value == "")
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
                }
                else
                {
                    string str;
                    string str2 = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                    str2 = GeneralFunction.ReverseString(str2);
                    int num3 = 0;
                    if (type == 1)
                    {
                        int num4 = 12;
                        while (true)
                        {
                            if (num4 >= 0x10)
                            {
                                break;
                            }
                            num3 = 576;
                            str = StatusContext.GetContext(messageID, language, MeterType._303);
                            if (str2[num3] == '1')
                            {
                                list.Add(new Status_Result(Status.True, str));
                            }
                            else
                            {
                                list.Add(new Status_Result(Status.False, str));
                            }
                            messageID++;
                            num4++;
                        }
                    }
                    else if (type == 2)
                    {
                        str = StatusContext.GetContext(0x3ec, language, MeterType._303);
                        
                             
                        if (str2[15] == '1')
                        {
                            list.Add(new Status_Result(Status.True, str));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, str));
                        }
                        str = StatusContext.GetContext(0x3ed, language, MeterType._303);
                        if (str2[14] == '1')
                        {
                            list.Add(new Status_Result(Status.True, str));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, str));
                        }
                    }
                    else if (type == 3)
                    {
                        messageID = 0x3ee;
                        int num5 = 0x16;
                        while (true)
                        {
                            if (num5 >= 0x1b)
                            {
                                break;
                            }
                            num3 = 0x1f - num5;
                            str = StatusContext.GetContext(messageID, language, MeterType._303);
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (str2[num3] == '1')
                                {
                                    list.Add(new Status_Result(Status.True, str));
                                }
                                else
                                {
                                    list.Add(new Status_Result(Status.False, str));
                                }
                            }
                            messageID++;
                            num5++;
                        }
                    }
                }
            }
            catch
            {
            }
            return list;
        }

        public List<Status_Result> StatuseGeneralRegister3(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            int messageID = 0x41a;
            try
            {
                value = GeneralFunction.ReverseStatusValue(value.Substring(0, 8));
                if (value == "")
                {
                    list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
                }
                else
                {
                    string str2 = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                    int num3 = 0;
                    int num4 = 6;
                    while (true)
                    {
                        string str;
                        if (num4 >= 10)
                        {
                            int num5 = 0x10;
                            while (true)
                            {
                                if (num5 >= 0x17)
                                {
                                    break;
                                }
                                num3 = 0x1f - num5;
                                str = StatusContext.GetContext(messageID, language, MeterType._303);
                                if (str2[num3] == '1')
                                {
                                    list.Add(new Status_Result(Status.True, str));
                                }
                                else
                                {
                                    list.Add(new Status_Result(Status.False, str));
                                }
                                messageID++;
                                num5++;
                            }
                            break;
                        }
                        num3 = 0x1f - num4;
                        str = StatusContext.GetContext(messageID, language, MeterType._303);
                        if (str2[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, str));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, str));
                        }
                        messageID++;
                        num4++;
                    }
                }
            }
            catch
            {
            }
            return list;
        }
        public List<Status_Result> statusRegister(string obis, string value, string languageName)
        {
            string s = obis;
            uint num = ComputeStringHash(s);
            if (num > 0x9ee4_6512)
            {
                if (num > 0xb81a_e1df)
                {
                    if (num > 0xcc2e_441f)
                    {
                        if (num > 0xe4bd_7a8f)
                        {
                            if (num == 0xe67d_5a14)
                            {
                                if (s == "0100600501FF")
                                {
                                    return this.StatusRegister_0100600501FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                                }
                            }
                            else if ((num == 0xf98d_33b5) && (s == "0000603F01FF"))
                            {
                                return this.PerformanceMeteroncreditevents("0", value, "0", languageName);
                            }
                        }
                        else if (num == 0xd65a_7aa7)
                        {
                            if (s == "0000600A05FF")
                            {
                                return this.statusRegister_0000600A05FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                            }
                        }
                        else if ((num == 0xe4bd_7a8f) && (s == "0100600A00FF"))
                        {
                            return this.StatusRegister_0100600A00FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                        }
                    }
                    else if (num == 0xbd42_e50b)
                    {
                        if (s == "0000600303FF")
                        {
                            return this.statusRegister_0000600303FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                        }
                    }
                    else if (num == 0xcbee_5b76)
                    {
                        if (s == "0000603F00FF")
                        {
                            return this.PerformanceMeteroncreditevents(value, "0", "0", languageName);
                        }
                    }
                    else if ((num == 0xcc2e_441f) && (s == "0802606101FF"))
                    {
                        return this.statusRegister_0802606101FF(value.Substring(0, 8), languageName);
                    }
                }
                else if (num > 0xb032_3829)
                {
                    if (num == 0xb217_04f0)
                    {
                        if (s == "0000603F02FF")
                        {
                            return this.PerformanceMeteroncreditevents("0", "0", value, languageName);
                        }
                    }
                    else if (num == 0xb4a4_3950)
                    {
                        if (s == "0100600A01FF")
                        {
                            return this.statusRegister_0100600A01FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                        }
                    }
                    else if ((num == 0xb81a_e1df) && (s == "0000603D01FF"))
                    {
                        return this.statusRegister_0000603D01FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 2)), languageName);
                    }
                }
                else if (num == 0xa77e_6de8)
                {
                    if (s == "0000600A04FF")
                    {
                        return this.statusRegister_0000600A04FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                    }
                }
                else if (num == 0xaec6_3f41)
                {
                    if (s == "0000616100FF")
                    {
                        return this.statusRegister_0000616100FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                    }
                }
                else if ((num == 0xb032_3829) && (s == "0100616101FF"))
                {
                    return this.statusRegister_0100616101FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                }
            }
            else if (num > 0x1e66_27fc)
            {
                if (num > 0x4fb7_fad6)
                {
                    if (num == 0x7d36_e4b3)
                    {
                        if (s == "0000600404FF")
                        {
                            return this.statusRegister_0000600404FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                        }
                    }
                    else if (num == 0x893e_d520)
                    {
                        if (s == "0000603D00FF")
                        {
                            return this.statusRegister_0000603D00FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 2)), languageName);
                        }
                    }
                    else if ((num == 0x9ee4_6512) && (s == "0000600A02FF"))
                    {
                        return this.statusRegister_0000600A02FF(value, languageName);
                    }
                }
                else if (num == 0x3c0c_b3b4)
                {
                    if (s == "9")
                    {
                        return this.statusRegister_9(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                    }
                }
                else if (num == 0x3d0c_b547)
                {
                    if (s == "8")
                    {
                        return this.statusRegister_8(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                    }
                }
                else if ((num == 0x4fb7_fad6) && (s == "0100600A03FF"))
                {
                    return this.statusRegister_0100600A03FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                }
            }
            else if (num > 0x162b_53bb)
            {
                if (num == 0x1beb_2a44)
                {
                    if (s == "10")
                    {
                        return this.statusRegister_10(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                    }
                }
                else if (num == 0x1de7_ff31)
                {
                    if (s == "0000600A03FF")
                    {
                        return this.statusRegister_0000600A03FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                    }
                }
                else if ((num == 0x1e66_27fc) && (s == "0000600302FF"))
                {
                    return this.statusRegister_0000600302FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
                }
            }
            else if (num == 0x410_a8ab)
            {
                if (s == "0000600A01FF")
                {
                    return this.statusRegister_0000600A01FF(value.Substring(0, 8), languageName);
                }
            }
            else if (num == 0x9f6_245b)
            {
                if (s == "0000603E04FF")
                {
                    return statusRegister_0000603E04FF(value, languageName);
                }
            }
            else if ((num == 0x162b_53bb) && (s == "0000616102FF"))
            {
                return this.statusRegister_0000616102FF(GeneralFunction.ReverseStatusValue(value.Substring(0, 8)), languageName);
            }
            return null;
        }

        private uint ComputeStringHash(string s)
        {
            uint num=0;
            if (s != null)
            {
                num = 0x811c_9dc5;
                for (int i = 0; i < s.Length; i++)
                {
                    num = (s[i] ^ num) * 0x100_0193;
                }
            }
            return num;

        }

        public List<Status_Result> statusRegister_0000600302FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                str = GeneralFunction.ReverseString(str); 
                for (int i = 0; i < 18; i++)
                {
                    string dec = StatusContext.GetContext(i + 1, language, MeterType._303);
                    if (!string.IsNullOrEmpty(dec))
                    {
                        if (str[i] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }                        
                }                 
            }
            return list;
        }


        public List<Status_Result> statusRegister_0000600303FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 0x260;
                while (true)
                {
                    if (messageID >= 0x27e)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 0x27d - messageID;
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }





        public List<Status_Result> statusRegister_0000600404FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 640;
                while (true)
                {
                    if (messageID >= 0x284)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 0x1f - (3 - (0x283 - messageID));
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }
        public List<Status_Result> statusRegister_0000600A01FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 0x20;
                while (true)
                {
                    if (messageID >= 0x40)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    if (dec != "")
                    {
                        if (str[0x3f - messageID] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }





        public List<Status_Result> statusRegister_0000600A02FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                value = GeneralFunction.ReverseStatusValue(value);
                string str = Convert.ToString(int.Parse(value, NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 64;
                str = GeneralFunction.ReverseString(str);
                while (true)
                {
                    if (messageID >= 94)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    if (dec != "")
                    {
                        if (str[messageID - 64] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }
    
    public List<Status_Result> GetPumpStatusFrom_0000600A02FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                value = value.Substring(0, 8);
                value = value.Substring(6, 2) + value.Substring(4, 2) + value.Substring(2, 2) + value.Substring(0,2);
                string str = Convert.ToString(int.Parse(value, NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 576;
                str =GeneralFunction. ReverseString(str);
                while (true)
                {
                    if (messageID >= 584)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    if (dec != "")
                    {
                        if (str[messageID - 554] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }
         

        public List<Status_Result> statusRegister_0000600A03FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                value = GeneralFunction.ReverseStatusValue(value.Substring(0, 8));
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                str = GeneralFunction.ReverseString(str);
                int messageID = 96;
                while (true)
                {
                    if (messageID >=119)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    if (dec != "")
                    {
                        if (str[messageID-96] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }




        public List<Status_Result> statusRegister_0000600A04FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 0xc0;
                while (true)
                {
                    if (messageID >= 0xe0)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 0xdf - messageID;
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }





        public List<Status_Result> statusRegister_0000600A05FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 0xe0;
                while (true)
                {
                    if (messageID >= 0x100)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 0xff - messageID;
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }






        public List<Status_Result> statusRegister_0000603D00FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else if (value.Substring(0, 2) == "00")
            {
                list.Add(new Status_Result(Status.False, StatusContext.GetContext(800, language, MeterType._303)));
            }
            else
            {
                list.Add(new Status_Result(Status.True, StatusContext.GetContext(0x321, language, MeterType._303)));
            }
            return list;
        }


        public List<Status_Result> statusRegister_0000603D01FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else if (value.Substring(0, 2) == "00")
            {
                list.Add(new Status_Result(Status.False, StatusContext.GetContext(900, language, MeterType._303)));
            }
            else
            {
                list.Add(new Status_Result(Status.True, StatusContext.GetContext(0x385, language, MeterType._303)));
            }
            return list;
        }





        public List<Status_Result> statusRegister_0000603E04FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                int mm = int.Parse(value.PadLeft(2,'0').Substring(0,2), System.Globalization.NumberStyles.HexNumber);
                string temp = GeneralFunction.ReverseString(Convert.ToString(mm, 2).PadLeft(5, '0'));
                int k =0;
                for (int i = 0; i <=8; i+=2)
                {
                    var dec = "";

                    {

                        if (temp[k] == '1')
                        {
                            dec = StatusContext.GetContext(i+ 1  + 672, language, MeterType._303);
                            if (!string.IsNullOrEmpty(dec))
                                list.Add(new Status_Result(Status.dontCare, dec));
                        }
                        else
                        {
                            dec = StatusContext.GetContext(i + 672, language, MeterType._303);
                            if (!string.IsNullOrEmpty(dec))
                                list.Add(new Status_Result(Status.dontCare, dec));
                        }
                        k++;
                    }
                }
            }

          
            return list;
        }


        public List<Status_Result> statusRegister_0000616100FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 0x160;
                while (true)
                {
                    if (messageID >= 0x16f)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 0x16e - messageID;
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }



        public List<Status_Result> statusRegister_0000616102FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 0x180;
                while (true)
                {
                    if (messageID >= 0x187)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 390 - messageID;
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }

        public List<Status_Result> StatusRegister_0100600501FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 0x1c0;
                while (true)
                {
                    if (messageID >= 0x1c1)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 0x1c0 - messageID;
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }
        public List<Status_Result> StatusRegister_0100600A00FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 480;
                while (true)
                {
                    if (messageID >= 0x1ff)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 510 - messageID;
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }

        public List<Status_Result> statusRegister_0100600A01FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 0x200;
                while (true)
                {
                    if (messageID >= 520)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 0x207 - messageID;
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }

        public List<Status_Result> statusRegister_0100600A03FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 0x1a0;
                while (true)
                {
                    if (messageID >= 0x1a8)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 0x1a7 - messageID;
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }

        public List<Status_Result> statusRegister_0100616101FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                int messageID = 0x220;
                while (true)
                {
                    if (messageID >= 0x222)
                    {
                        break;
                    }
                    string dec = "";
                    dec = StatusContext.GetContext(messageID, language, MeterType._303);
                    int num3 = 0x221 - messageID;
                    if (dec != "")
                    {
                        if (str[num3] == '1')
                        {
                            list.Add(new Status_Result(Status.True, dec));
                        }
                        else
                        {
                            list.Add(new Status_Result(Status.False, dec));
                        }
                    }
                    messageID++;
                }
            }
            return list;
        }


        public List<Status_Result> statusRegister_0802606101FF(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            else
            {
                string str = Convert.ToString(int.Parse(value.Substring(0, 8), NumberStyles.HexNumber), 2).PadLeft(0x20, '0');
                str =GeneralFunction. ReverseString(str);
                int messageID = 576;
                for (int i = 0; i < 32; i++)
                {
                    string dec = "";
                    dec = StatusContext.GetContext(messageID + i, language, MeterType._303);
                    if (!string.IsNullOrEmpty(dec))
                    {
                        if (str[i] == '1')
                            list.Add(new Status_Result(Status.True, dec));
                        else
                            list.Add(new Status_Result(Status.False, dec));
                    }
                }
            }
            return list;
        }





        public List<Status_Result> statusRegister_10(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            return list;
        }



        public List<Status_Result> statusRegister_9(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            return list;
        }




      





        public List<Status_Result> statusRegister_8(string value, string language)
        {
            List<Status_Result> list = new List<Status_Result>();
            if (value == "")
            {
                list.Add(new Status_Result(Status.dontCare, StatusContext.GetContext(0, language, MeterType._303)));
            }
            return list;
        }





























    }
}