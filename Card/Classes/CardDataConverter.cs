using CARD;
using RsaDateTime;
using System;

namespace Card.Classes
{
    internal class CardDataConverter
    {
        public static string convertCardReadValue(string datatype, string start_byte, string number_of_bytes, string readValue)
        {
            int rule;
            try
            {
                rule = int.Parse(datatype);
            }
            catch (Exception)
            {
                return readValue;
            }


            float floattmp = 1;
            int NumOfBytes = 0;
            int StartIndex = 0;
            int itmp = -1;
            string Value = readValue;



            switch (rule)
            {
                #region 207 Data

                case 0:
                    Value= System.Convert.ToInt32(readValue, 16).ToString();
                    break;
                case 1:
                    StartIndex = System.Convert.ToInt16(start_byte) * 2 - 2;
                    NumOfBytes = System.Convert.ToByte(number_of_bytes) * 2;
                    if (StartIndex < 0)
                        StartIndex = 0;
                    readValue = readValue.Substring(StartIndex, NumOfBytes);
                    itmp = System.Convert.ToInt32(readValue, 16);
                    Value = Convert2Float(itmp.ToString(), 1);
                    break;
                case 2:
                    StartIndex = System.Convert.ToInt16(start_byte) * 2 - 2;
                    NumOfBytes = System.Convert.ToByte(number_of_bytes) * 2;
                    readValue = readValue.Substring(StartIndex, NumOfBytes);
                    itmp = System.Convert.ToInt32(readValue, 16);
                    Value = Convert2Float(itmp.ToString(), 2);
                    break;
                case 3:
                    Value = Convert8bitHex2Date(readValue);
                    char ch = '/';
                    string[] dateParam = Value.Split(ch);

                    int year = 1300;
                    int day = 0, month = 0;

                    int.TryParse(dateParam[0], out year);
                    int.TryParse(dateParam[1], out month);
                    int.TryParse(dateParam[2], out day);

                    if (year > 50)
                        year += 1300;
                    else
                        year += 1400;
                    
                    try
                    {
                    var pd = new PersianDate(year, month, day, 0, 0, 0);

                        // if (pd.Compare(PersianDate.Now, pd) > 0)
                        Value =pd.ConvertToGeorgianDateTime().ToRsaString();
                        // else
                        //   Value = new DateTime(1921,3,21,0,0,0).ToString();
                    }
                    catch
                    {
                        Value = new DateTime(1921,3,21,0,0,0).ToString();
                    }
                    break;

                case 4:
                    Value = Hex2int(readValue.Substring(4, 4)).ToString() + "/" + Hex2int(readValue.Substring(0, 4)).ToString();
                    break;

                case 5:
                    Value = readValue.Substring(2 * (Convert.ToInt32(start_byte)-1), 2 * (Convert.ToInt32(number_of_bytes)));
                    break;

                case 6:
                    Value = readValue;
                    break;

                case 7:
                    Value = readValue.Substring(6, 2);
                    break;

                case 8:
                    StartIndex = System.Convert.ToInt16(start_byte) * 2 - 2;
                    NumOfBytes = System.Convert.ToByte(number_of_bytes) * 2;
                    readValue = readValue.Substring(StartIndex, NumOfBytes);
                    Value = System.Convert.ToInt32(readValue, 16).ToString();
                    break;

                case 9:
                    floattmp = (System.Convert.ToInt32(readValue, 16)) / 60f;
                    Value = floattmp.ToString();
                    break;

                case 10:
                    Value = "0";
                    break;

                case 11:
                    itmp = System.Convert.ToInt32(readValue, 16);
                    readValue = Convert2FixString(itmp.ToString(), "0", 8);
                    Value = readValue.Substring(4, 1) + "/" + readValue.Substring(5, 3);
                    break;
                #endregion 207 Data

                case 12:
                    Value = System.Convert.ToUInt64(readValue, 16).ToString();
                    break;
                case 13:
                    Value = System.Convert.ToInt32(readValue, 16).ToString();
                    break;
                case 14: // 303 Serial Number
                    Value = "-1";
                    if (readValue.Length > 0)
                    {
                        readValue = Reversevalue(readValue);

                        try
                        {
                            Value = System.Convert.ToInt32(readValue, 16).ToString();
                            break;
                        }
                        catch
                        {
                            return "-1";
                        }
                    }
                    break;

                case 15:
                    Value = GeneralFunctions.ASCIIStringToString(Value.Substring(0, 40));
                    Value = Value.Replace("\0", "");
                    break;
                
                case 16:                    
                    break;

                case 17: // Card Number                    
                    if (readValue.Length > 0)
                    {
                        readValue = Reversevalue(readValue);

                        try
                        {
                            Value = System.Convert.ToInt64(readValue, 16).ToString();
                            break;
                        }
                        catch
                        {
                            return "-1";
                        }
                    }
                    break;

                case 18: // Convert To Hor.Minutes                    
                    Value = (System.Convert.ToInt32(readValue, 16)) / 60 + "." + ((System.Convert.ToInt32(readValue, 16)) % 60).ToString().PadLeft(2, '0');
                    
                    break;
                   

                default:
                    break;
            }
            if (Value.StartsWith("."))
                Value = Value.Replace(".", "0.");
            return Value;
        }

        #region 207 dataconverter Function
        public static string Convert2Float(string value, int FloatDigits)
        {
            int l = value.Length;
            if (l < FloatDigits)
            {
                value += "0";
                l += 1;
            }
            return value.Insert(l - FloatDigits, ".");
        }

        public static string StringToASCIIString(string str)
        {

            string result = "";
            for (int i = 0; i < str.Length; i += 1)
            {
                result += Convert.ToByte(str[i]).ToString("x2");
            }
            return result;
        }


        public static string Convert8bitHex2Date(string value)
        {
            try
            {
                return Convert2FixString(Convert.ToInt16(value.Substring(2, 2), 16).ToString(), "0", 2) + "/" + Convert2FixString(Convert.ToInt16(value.Substring(4, 2), 16).ToString(), "0", 2) + "/" + Convert2FixString(Convert.ToInt16(value.Substring(6, 2), 16).ToString(), "0", 2);
            }
            catch
            {
                return value;
            }
        }

        public static string Convert2FixString(string value, string FixChar, int FixNum)
        {
            int i = FixNum - value.Length;
            if (i > 0)
            {
                string s = RepeatChars(FixChar, i) + value;
                return RepeatChars(FixChar, i) + value;
            }
            else
                return value;
        }

        public static string RepeatChars(string value, int RepeatNum)
        {
            string tmp = "";
            for (int i = 0; i < RepeatNum; i++)
                tmp += value;
            return tmp;
        }

        public static int Hex2int(string value)
        {
            return System.Convert.ToInt16(value, 16);
        }


        #endregion 207 dataconverter Function

        #region 303 Dataconverter Function
        public static string Reversevalue(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
            for (int i = cArray.Length - 1; i > 0; i -= 2)
            {
                reverse += cArray[i - 1];
                reverse += cArray[i];
            }
            return reverse;
        }

        #endregion 303 Dataconverter Function


        public static string CorrectUnit(string value, string unitConvertRule)
        {
            try
            {
                if (unitConvertRule == "1")
                {
                    double d = double.Parse(value);
                    return (d * 1000).ToString();
                }
                else if (unitConvertRule == "2")
                {
                    double d = double.Parse(value, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                    return (d * 3600).ToString();
                }
                return value;
            }
            catch (Exception)
            {
                return value;
            }            
            
        }
    }
}
