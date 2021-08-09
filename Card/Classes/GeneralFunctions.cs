

/*=======================================================================================

 GeneralFunctions.cs
 Usefull and common functions deal with String,Array,...
 Created by : Ahmad Javadi (A.Javadi@rsa.co.ir)
 Created on : 1392/6/25
 Last Updated by : 
 Last Updated : 
 DBAccess ver 

	Copyright © 1392, Rahrovan Sepehr Andisheh (Pte. Co.)
	http://www.rsa-electronics.com
=======================================================================================*/
using RsaDateTime;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;

namespace CARD
{
    internal class ErrorHandle
    {
        public static void showError(string s1, int s2, int s3)
        {
            string errDesc;
            switch (s2)
            {
                case 0:
                    errDesc = "";
                    break;
                // CARD Errors
                case 3001:
                    errDesc = "عمليات تبادل اطلاعات با کارت با موفقيت انجام شد";
                    break;
                case 3002:
                    errDesc = "ورودي معتبر نمي باشد";
                    break;
                case 3003:
                    errDesc = "کارت در کارتخوان موجود نمي باشد";
                    break;
                case 3004:
                    errDesc = "کارت نامعتبر است و يا کارت اشتباه قرارداده شده است";
                    break;
                case 3005:
                    errDesc = "(3005) مشكل در كارت";
                    break;
                case 3006:
                    errDesc = "کارتخوان معتبر نمي باشد";
                    break;
                case 3007:
                    errDesc = "کارت نامعتبر";
                    break;
                case 3008:
                    errDesc = "(3008) بروز خطا در خواندن اطلاعات از کارت";
                    break;
                case 3009:
                    errDesc = "بروز خطا در نوشتن اطلاعات در کارت";
                    break;
                case 3010:
                    errDesc = "(3010) کارت نامعتبر";
                    break;
                case 3011:
                    errDesc = "(3011) کارت نامعتبر";
                    break;
                case 3012:
                    errDesc = "ذخيره اطلاعات با مشکل مواجه گرديد";
                    break;
                case 3013:
                    errDesc = "(3013) مشكل در كارت";
                    break;
                case 3014:
                    errDesc = "ذخيره کد با مشکل مواجه گرديد";
                    break;
                case 3015:
                    errDesc = "(3015) کارت نامعتبر";
                    break;
                case 3016:
                    errDesc = "(3016) بروز خطا در خواندن اطلاعات از کارت";
                    break;
                case 3017:
                    errDesc = "(3017)بروز خطا در خواندن اطلاعات از کارت";
                    break;
                case 4100:
                    errDesc = "عمليات تبادل اطلاعات با کارت با موفقيت انجام شد";
                    break;
                case 4101:
                    errDesc = "کارت در کارتخوان موجود نمي باشد";
                    break;
                case 4102:
                    errDesc = "کارت نامعتبر است و يا کارت اشتباه قرارداده شده است";
                    break;
                case 4103:
                    errDesc = "لطفا کارتخوان را به کامپیوتر متصل کنید";
                    break;
                case 4104:
                    errDesc = "(4104)  مشكل در كارت";
                    break;
                case 4200:
                    errDesc = "نوشتن كد بر روي كات";
                    break;
                case 4201:
                    errDesc = "كارت راه انداز در كارتخوان قرار دارد. لطفا كارت مربوط به كنتور را در كارتخوان قرار دهيد ودوباره اطلاعات را ارسال نماييد.";
                    break;
                case 4202:
                    errDesc = "كارت راه انداز در كارتخوان قرار دارد. لطفا كارت مربوط به كنتور را در كارتخوان قرار دهيد .";
                    break;
                // End of CARD Errors
                // Security Message
                case 500:
                    errDesc = "كد كارت مربوط به اين شهرستان نمي باشد";
                    break;
                case 501:
                    errDesc = "كد كارت مربوط به اين استان نمي باشد";
                    break;
                case 502:
                    errDesc = "كد كارت نامعتبر است";
                    break;
                case 503:
                    errDesc = "لطفا كارت راه انداز را در كارت خوان قرار دهيد";
                    break;
                case 504:
                    errDesc = "نام كاربر صحيح نمي باشد";
                    break;
                case 505:
                    errDesc = "كلمه عبور صحيح نمي باشد";
                    break;
                // End of Security Message

                case 601:
                    errDesc = "شماره چاه را وراد کنید";
                    break;
                // General Message
                case 901:
                    errDesc = " پايان انجام فرمان ";
                    break;
                case 902:
                    errDesc = " اين فرآيند تعريف نشده است ";
                    break;
                // 999 خروج از تابع بدون نمایش هیچ پیغامی
                case 999:
                    return;
                // End of General Message
                default:
                    errDesc = "خطاي ناشناخته در اجراي فرمان ";
                    break;
            }

            Card.CardManagerEvent cardManagerEvent = new Card.CardManagerEvent();
            cardManagerEvent.message = errDesc;

        }
        public static void showError(int s2)
        {
            string errDesc = "";
            switch (s2)
            {
                case 0:
                    errDesc = "";
                    break;
                // CARD Errors
                case 3001:
                    errDesc = "عمليات تبادل اطلاعات با کارت با موفقيت انجام شد";
                    break;
                case 3002:
                    errDesc = "ورودي معتبر نمي باشد";
                    break;
                case 3003:
                    errDesc = "کارت در کارتخوان موجود نمي باشد";
                    break;
                case 3004:
                    errDesc = "کارت نامعتبر است و يا کارت اشتباه قرارداده شده است";
                    break;
                case 3005:
                    errDesc = "(3005) مشكل در كارت";
                    break;
                case 3006:
                    errDesc = "کارتخوان معتبر نمي باشد";
                    break;
                case 3007:
                    errDesc = "کارت نامعتبر";
                    break;
                case 3008:
                    errDesc = "(3008) بروز خطا در خواندن اطلاعات از کارت";
                    break;
                case 3009:
                    errDesc = "بروز خطا در نوشتن اطلاعات در کارت";
                    break;
                case 3010:
                    errDesc = "(3010) کارت نامعتبر";
                    break;
                case 3011:
                    errDesc = "(3011) کارت نامعتبر";
                    break;
                case 3012:
                    errDesc = "ذخيره اطلاعات با مشکل مواجه گرديد";
                    break;
                case 3013:
                    errDesc = "(3013) مشكل در كارت";
                    break;
                case 3014:
                    errDesc = "ذخيره کد با مشکل مواجه گرديد";
                    break;
                case 3015:
                    errDesc = "(3015) کارت نامعتبر";
                    break;
                case 3016:
                    errDesc = "(3016) بروز خطا در خواندن اطلاعات از کارت";
                    break;
                case 3017:
                    errDesc = "(3017)بروز خطا در خواندن اطلاعات از کارت";
                    break;
                case 4100:
                    errDesc = "عمليات تبادل اطلاعات با کارت با موفقيت انجام شد";
                    break;
                case 4101:
                    errDesc = "کارت در کارتخوان موجود نمي باشد";
                    break;
                case 4102:
                    errDesc = "کارت نامعتبر است و يا کارت اشتباه قرارداده شده است";
                    break;
                case 4103:
                    errDesc = "لطفا کارتخوان را به کامپیوتر متصل کنید";
                    break;
                case 4104:
                    errDesc = "(4104)  مشكل در كارت";
                    break;
                case 4200:
                    errDesc = "نوشتن كد بر روي كات";
                    break;
                case 4201:
                    errDesc = "كارت راه انداز در كارتخوان قرار دارد. لطفا كارت مربوط به كنتور را در كارتخوان قرار دهيد ودوباره اطلاعات را ارسال نماييد.";
                    break;
                case 4202:
                    errDesc = "كارت راه انداز در كارتخوان قرار دارد. لطفا كارت مربوط به كنتور را در كارتخوان قرار دهيد .";
                    break;
                // End of CARD Errors
                // Security Message
                case 500:
                    errDesc = "كد كارت مربوط به اين شهرستان نمي باشد";
                    break;
                case 501:
                    errDesc = "كد كارت مربوط به اين استان نمي باشد";
                    break;
                case 502:
                    errDesc = "كد كارت نامعتبر است";
                    break;
                case 503:
                    errDesc = "لطفا كارت راه انداز را در كارت خوان قرار دهيد";
                    break;
                case 504:
                    errDesc = "نام كاربر صحيح نمي باشد";
                    break;
                case 505:
                    errDesc = "كلمه عبور صحيح نمي باشد";
                    break;
                // End of Security Message


                // General Message
                case 901:
                    errDesc = " پايان انجام فرمان ";
                    break;
                case 902:
                    errDesc = " اين فرآيند تعريف نشده است ";
                    break;
                // 999 خروج از تابع بدون نمایش هیچ پیغامی
                case 999:
                    return;
                // End of General Message
                default:
                    errDesc = "خطاي ناشناخته در اجراي فرمان ";
                    break;
            }
            //if (s2!= 3016)
            //CardFormatter.App.mainWindows.AppendTextBlock(errDesc);
        }
    }

    public static class GeneralFunctions
    {

        //303 function
        #region 303 function
        public static byte[] hexStrToByteArray(String s)
        {
            byte[] result = new byte[s.Length / 2];
            int i = 0;
            int j = 0;
            int h = 0;
            int k = 0;
            while (i < s.Length)
            {
                if ((s[i] >= '0') && (s[i] <= '9'))
                {
                    h = (h << 4) + s[i] - '0';
                    j++;
                }
                else if ((s[i] >= 'A') && (s[i] <= 'F'))
                {
                    h = (h << 4) + s[i] - 'A' + 10;
                    j++;
                }
                else if (s[i] == ' ')
                {
                }
                else
                {
                    // throw new XmlPduException("HexStrToByteStr: Illegal character");
                }
                if (j == 2)
                {
                    result[k++] = (byte)h;
                    h = 0;
                    j = 0;
                }
                i++;
            }
            return result;
        }

        public static System.DateTime ByteArrayStringToDateTime(String s)
        {
            byte[] byteArray = hexStrToByteArray(Reverse(s));
            byte[] buff = new byte[2];
            System.Array.Copy(byteArray, 0, buff, 0, 2);
            System.Array.Reverse(buff);
            int year = (ushort)(BitConverter.ToUInt16(buff, 0) & 0xFFFF);
            if (year == 0xFFFF)
                year = 1900;
            byte hour = byteArray[5];
            if (hour == 24)
                hour = 0;
            return new System.DateTime(year, byteArray[2], byteArray[3],
                hour, byteArray[6], byteArray[7], byteArray[8]);
        }


        public static string Reverse(string text)
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

        public static string ASCIIStringToString(string str)
        {

            string result = "";
            for (int i = 0; i < str.Length; i += 2)
            {
                int n = Int32.Parse(str.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                result += Convert.ToChar(n);
            }
            return result;
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

        #endregion


        // Binary , HEX , .... Functions
        #region Binary , HEX , .... Functions

        public static string Binary2Hex(string value, int fixedLenght)
        {
            int b = (System.Convert.ToInt16(value, 2));
            return Convert2FixString(System.Convert.ToString(System.Convert.ToInt16(b), 16), "0", fixedLenght);
        }
        public static string int2Binary(int Number, int fixedLenght)
        {
            return Convert2FixString(System.Convert.ToString(Number, 2), "0", fixedLenght);
        }
        public static int Binary2int(string Value, int fixedLenght)
        {
            return Hex2int(Binary2Hex(Value, fixedLenght));
        }
        public static string int2Hex(string number)
        {
            if (number.Trim() != "")
                return System.Convert.ToString(System.Convert.ToInt32(number), 16);
            return "";
        }
        public static string int2Hex(int number)
        {
            return System.Convert.ToString((number), 16);
        }
        public static int Hex2int(string value)
        {
            return System.Convert.ToInt16(value, 16);
        }
        public static string Hex2Binary(string value, int fixedLenght)
        {
            try
            {
                return Convert2FixString(System.Convert.ToString(System.Convert.ToInt16(value, 16), 2), "0", fixedLenght);
            }
            catch
            {
                return Convert2FixString("0", "0", fixedLenght);
            }
        }
        #endregion
        // Array Functions
        #region Array Functions
        public static string Array2OptionalString(string[] MyArray, string optionalChar)
        {
            string cvs = "";
            for (int i = 1; i < ((MyArray.GetLongLength(0))); i++)
            {
                cvs += RemoveChars(MyArray[i]) + optionalChar;
            }
            return cvs;
        }
        public static string[] ResetArray(string[] MyArray)
        {
            for (int i = 0; i < ((MyArray.GetLongLength(0))); i++)
            {
                MyArray[i] = "0";
            }
            return MyArray;
        }
        public static string[,] ResetArray(string[,] MyArray)
        {
            for (int i = 0; i < ((MyArray.GetLongLength(0))); i++)
            {
                for (int j = 0; j < ((MyArray.GetLongLength(1))); j++)
                {
                    MyArray[i, j] = "0";
                }
            }
            return MyArray;
        }
        public static void EncryptStr2txtFile(string str, string FileName)
        {
            StreamWriter stream = null;
            //FileName = @"\" + FileName;
            stream = File.CreateText(FileName);
            stream.WriteLine("{0}", str);
            stream.Close();
        }

        public static string[] TxtFile2Array(string FileName)
        {
            StreamReader reader = System.IO.File.OpenText(FileName);
            string[] delimiter = new string[] { ((char)(0x0D)).ToString(), ((char)(0x0A)).ToString() };
            string str_tmp = reader.ReadToEnd();
            string[] Output = str_tmp.Split(delimiter, StringSplitOptions.None);
            reader.Close();

            return Output;

        }
        public static string[] TxtFile2Array(string FileName, int IndexNum)
        {
            StreamReader reader = System.IO.File.OpenText(FileName);
            string[] MyArray = new string[IndexNum + 1];
            for (int i = 0; i < IndexNum + 1; i++)
            {
                try
                {
                    MyArray[i] = reader.ReadLine();
                }
                catch
                {
                    MyArray[i] = "";
                }
            }
            reader.Close();
            return MyArray;
        }


        public static void Array2txtFile2(string[] MyArray, string FileName)
        {
            StreamWriter stream = null;
            stream = File.CreateText(FileName);
            string cipherText = "";
            for (int i = 0; i < ((MyArray.GetLongLength(0))); i++)
            {
                if (MyArray[i].Trim() != "")
                {
                    cipherText = (MyArray[i]);
                    stream.WriteLine("{0}", cipherText);
                }
                else
                    stream.WriteLine("{0}", MyArray[i]);
            }
            stream.Close();
        }
        public static void Array2txtFile(string[] MyArray, string FileName)
        {
            StreamWriter stream = null;
            //FileName = @"\" + FileName;
            stream = File.CreateText(FileName);
            for (int i = 0; i < ((MyArray.GetLongLength(0))); i++)
            {
                stream.WriteLine("{0}", MyArray[i]);
            }
            stream.Close();
        }
        public static void Array2txtFile(string[,] MyArray, string FileName)
        {
            StreamWriter stream = null;
            //FileName = @"\" + FileName;
            stream = File.CreateText(FileName);
            for (int i = 0; i < ((MyArray.GetLongLength(1))); i++)
            {
                stream.WriteLine("{0} {1} {2} {3} {4} {5}", MyArray[0, i], MyArray[1, i], MyArray[2, i], MyArray[3, i], MyArray[4, i], MyArray[5, i]);
            }
            stream.Close();
        }
        public static string[,] CalendarArraySort(string[,] MyArray)
        {
            int i = 0;
            int j = 0;
            for (i = 3; i < 28; i++)
            {
                for (j = i; j < 28; j++)
                {
                    if (MyArray[1, j] == "0")
                        break;
                    else if (MyArray[1, i] == MyArray[1, j])
                    {
                        break;
                    }

                }
            }
            return MyArray;
        }
        public static double ArrayDoubleTotalFromString(string[] MyArray, long SumCount)
        {
            double sum = 0;
            if (SumCount < 1)
                SumCount = (MyArray.GetLongLength(0));
            for (int i = 1; i < SumCount; i++)
            {
                sum += double.Parse(MyArray[i].ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            return sum;
        }
        public static Boolean ArrayRowIsEmpty(string[,] MyArray, int RowIndex)
        {
            string tmp = "";
            int k = MyArray.GetLength(0);
            for (int i = 0; i < MyArray.GetLength(0); i++)
            {
                try
                {
                    if (MyArray[i, RowIndex] != null)
                        tmp += MyArray[i, RowIndex].ToString().Trim();
                }
                catch (Exception e)
                {
                }
            }
            if (tmp.Trim() == "")
                return true;
            return false;
        }
        public static Boolean ArrayColumnIsEmpty(string[,] MyArray, int ColIndex)
        {
            string tmp = "";
            int k = MyArray.GetLength(1);
            for (int i = 0; i < MyArray.GetLength(1); i++)
            {
                try
                {
                    if (MyArray[ColIndex, i].ToString() != null)
                        tmp += MyArray[ColIndex, i].ToString().Trim();
                }
                catch (Exception e)
                {
                }
            }
            if (tmp.Trim() == "")
                return true;
            return false;
        }
        public static string[,] MergeArrayVertically(string[,] MyArray1, string[,] MyArray2, int columns, int Rows)
        {
            string tmp = "";
            string[,] OutputArray = new string[columns, Rows];
            int Array1Rows = MyArray1.GetLength(0);
            for (int i = 0; i < MyArray1.GetLength(0); i++)
            {
                for (int j = 0; j < MyArray1.GetLength(1); j++)
                {
                    try
                    {
                        if (MyArray1[i, j].ToString().Trim() != null)
                            OutputArray[i, j] = MyArray1[i, j].ToString().Trim();
                    }

                    catch (Exception e)
                    {
                        return null;
                    }
                }
            }
            for (int i = 0; i < MyArray2.GetLength(0); i++)
            {
                for (int j = 0; j < MyArray2.GetLength(1); j++)
                {
                    try
                    {
                        if (MyArray2[i, j].ToString().Trim() != null)
                            OutputArray[i + Array1Rows - 1, j] = MyArray2[i, j].ToString().Trim();
                    }

                    catch (Exception e)
                    {
                        return null;
                    }
                }
            }
            return OutputArray;
        }
        public static string[,] MergeArray(string[,] MyArray1, string[,] MyArray2, int columns, int Rows)
        {
            string[,] OutputArray = new string[columns, Rows];
            int Array1Rows = MyArray1.GetLength(1);
            for (int i = 0; i < MyArray1.GetLength(1); i++)
            {
                for (int j = 0; j < MyArray1.GetLength(0); j++)
                {
                    try
                    {
                        if (MyArray1[j, i].ToString().Trim() != null)
                            OutputArray[j, i] = MyArray1[j, i].ToString().Trim();
                    }

                    catch (Exception e)
                    {
                        return null;
                    }
                }
            }
            for (int i = 0; i < MyArray2.GetLength(1); i++)
            {
                for (int j = 0; j < MyArray2.GetLength(0); j++)
                {
                    // try
                    //{
                    if (MyArray2[j, i].ToString().Trim() != null)
                        OutputArray[j, i + Array1Rows] = MyArray2[j, i].ToString().Trim();
                    /*}

                    catch (Exception e)
                    {
                        return null;
                    }*/
                }
            }
            return OutputArray;
        }

        #endregion
        // String Function
        #region String Functions
        public static string RemoveChars(string value)
        {
            if (value != null)
                value = value.Replace("'", "").Replace('"', ' ');
            return value;
        }
        public static int CompareStrings(string value1, string value2)
        {
            // return -1 if val2>val1
            // return 0 if val2=val1
            // return +1 if val1>val2            
            return Comparer.DefaultInvariant.Compare(value1, value2);
        }
        public static string RepeatChars(string value, int RepeatNum)
        {
            string tmp = "";
            for (int i = 0; i < RepeatNum; i++)
                tmp += value;
            return tmp;
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
        public static string Convert2FixString(string value, int FixNum)
        {
            int i = FixNum - value.Length;
            if (i < 0)
            {
                return value.Substring(0, FixNum);
            }
            else
                return value;
        }
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



        #endregion
        // Date Function
        #region Date Functions
        public static string DecreaseMonths6Digit(string shDate, int MonthNums)
        {
            if (shDate.Length < 6)
                return "0";
            int YearNum = System.Convert.ToInt16(Math.Truncate(Convert.ToDecimal(MonthNums / 12)));
            PersianDate prDate = new PersianDate();
            int Month = System.Convert.ToInt16(shDate.Substring(2, 2)) - (MonthNums - (YearNum * 12));
            if (Month > 12)
            {
                Month = Month - 12;
                YearNum = YearNum - 1;
            }
            if (Month < 1)
            {
                Month = Month + 12;
                YearNum = YearNum + 1;
            }
            prDate.Month = Month;
            prDate.Year = (System.Convert.ToInt16(shDate.Substring(0, 2)) - YearNum);
            int Day = System.Convert.ToInt16(shDate.Substring(4, 2));
            try
            {
                prDate.Day = Day;
            }
            catch
            {
                if (Month == 12)
                    prDate.Day = 29;
                else if (Month > 6)
                    prDate.Day = 30;
                else
                    prDate.Day = 31;
            }
            string s = prDate.ToDateString();
            return s.Replace("/", "").Trim().ToString();
        }
        //اين تابع براي بيلينگ است كه تاريخ را سر ماه مي آورد
        public static string DecreaseMonths6Digit4Billing(string shDate, int MonthNums)
        {
            if (shDate.Length < 6)
                return "0";
            int YearNum = System.Convert.ToInt16(Math.Truncate(Convert.ToDecimal(MonthNums / 12)));
            PersianDate prDate = new PersianDate();
            int Month = System.Convert.ToInt16(shDate.Substring(2, 2)) - (MonthNums - (YearNum * 12));
            if (Month > 12)
            {
                Month = Month - 12;
                YearNum = YearNum - 1;
            }
            if (Month < 1)
            {
                Month = Month + 12;
                YearNum = YearNum + 1;
            }
            prDate.Month = Month;
            if (Month == 12)
                prDate.Day = 29;
            else if (Month > 6)
                prDate.Day = 30;
            else
                prDate.Day = 31;
            prDate.Year = (System.Convert.ToInt16(shDate.Substring(0, 2)) - YearNum);
            string s = prDate.ToDateString();
            return s.Replace("/", "").Trim().ToString();
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
        public static string Convert8bitInt2Date(string value)
        {
            try
            {
                return value.Substring(0, 2) + "/" + value.Substring(2, 2) + "/" + value.Substring(4, 2);
            }
            catch
            {
                return value;
            }
        }
        public static string Convert8bitInt2Time(string value)
        {
            try
            {
                return value.Substring(0, 2) + ":" + value.Substring(2, 2) + ":" + value.Substring(4, 2);
            }
            catch
            {
                return value;
            }
        }
        public static string Convert10DigitDate28DigitHex(string value)
        {
            return Convert2FixString(int2Hex(value.Substring(2, 2)), "0", 2) + Convert2FixString(int2Hex(value.Substring(5, 2)), "0", 2) + Convert2FixString(int2Hex(value.Substring(8, 2)), "0", 2);
        }
        public static string ConvertMiadi2Shamsi(DateTime dt)
        {
            PersianDate pd = new PersianDate(dt);
            
            return "'" + pd.ToDateString().Substring(2, 8) + "'";
        }
        public static string ShamsiYear(DateTime dt)
        {
            PersianDate pd = new PersianDate(dt);
            return pd.ToDateString().Substring(2, 2);
        }
        public static string Shamsi8DigitDate(DateTime dt)
        {
            PersianDate pd = new PersianDate(dt);
            string p = pd.ToDateString();
            return p.Substring(2, 8);
        }
        public static string Shamsi10DigitDate(DateTime dt)
        {
            PersianDate pd = new PersianDate(dt);
            return pd.ToDateString();
        }
        public static string ConvertLongDate210DigitDate(string value)
        {
            return  new PersianDate().ConvertToPersianDate(value).ConvertToGeorgianDateTime().ToRsaString();
            // return PersianDateConverter.ToPersianDate(value).ToString().Substring(0, 10);
            //return PersianDateConverter.ToPersianDate(value).ToString("d");
        }
        public static Boolean IsInt(string value)
        {
            try
            {
                Int64 myInt = System.Convert.ToInt64(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ConvertDayNumber2Date(int year, int value)
        {
            if (value == 0)
                return "";
            int myMonth, myDay;
            if (value > 186)
            {
                value = value - 186;
                myMonth = (value / 30) + 7;
                myDay = value % 30;
                if (myDay == 0)
                {
                    myMonth -= 1;
                    myDay = 30;
                }
            }
            else
            {
                myMonth = (value / 31) + 1;
                myDay = value % 31;
                if (myDay == 0)
                {
                    myMonth -= 1;
                    myDay = 31;
                }
            }
            return (1300 + year).ToString() + "/" + Convert2FixString(myMonth.ToString(), "0", 2) + "/" + Convert2FixString(myDay.ToString(), "0", 2); ;
        }
        public static string Convert2DayNumberInYear(string value)
        {
            if (value.Trim() == "")
                return "0";
            byte mounth = System.Convert.ToByte(value.Substring(5, 2));
            byte day = System.Convert.ToByte(value.Substring(8, 2));
            if (mounth > 6)
                return ((mounth - 7) * 30 + 186 + day).ToString();
            else
                return ((mounth - 1) * 31 + day).ToString();
        }
        public static string DecreaseMonths(string shDate, int MonthNums, int option)
        {
            string s;
            if (option == 0)
            {
                try
                {
                    int YearNum = System.Convert.ToInt16(Math.Truncate(System.Convert.ToDecimal(MonthNums / 12)));
                    PersianDate prDate = new PersianDate();
                    int Month = System.Convert.ToInt16(shDate.Substring(3, 2)) - (MonthNums - (YearNum * 12));
                    if (Month > 12)
                    {
                        Month = Month - 12;
                        YearNum = YearNum - 1;
                    }
                    if (Month < 1)
                    {
                        Month = Month + 12;
                        YearNum = YearNum + 1;
                    }
                    prDate.Month = Month;
                    prDate.Year = (System.Convert.ToInt16(shDate.Substring(0, 2)) - YearNum);
                    try
                    {
                        prDate.Day = System.Convert.ToInt16(shDate.Substring(6, 2));
                    }
                    catch
                    {
                        prDate.Day = 29;
                    }
                    //s = prDate.ToString("M");
                    s=prDate.ToDateString();
                    string[] words = s.Split(' ');
                    s = words[1] + words[0];
                }
                catch
                {
                    return "خطا در تاریخ";
                }
            }
            else
            {
                switch (MonthNums)
                {
                    case 0:
                        s = "جاري";
                        break;
                    default:
                        s = MonthNums.ToString();
                        break;
                }
            }
            return s;
        }
        public static string DecreaseMonths8Digits(string shDate, int MonthNums)
        {
            PersianDate prDate = new PersianDate();
            int YearNum = System.Convert.ToInt16(Math.Truncate(System.Convert.ToDecimal(MonthNums / 12)));
            
            int Month = System.Convert.ToInt16(shDate.Substring(3, 2)) - (MonthNums - (YearNum * 12));
            if (Month < 1)
            {
                Month = Month + 12;
                YearNum = YearNum + 1;
            }
            prDate.Month = Month;
            prDate.Year = (System.Convert.ToInt16(shDate.Substring(0, 2)) - YearNum);
            try
            {
                prDate.Day = System.Convert.ToInt16(shDate.Substring(6, 2));
            }
            catch
            {
                prDate.Day = 29;
            }
            return (prDate).ToDateString();
        }
        public static string IncreaseYear12Digit(string shDate, int YearNums)
        {
            //PersianDate prDate = new PersianDate();
            //int Year = System.Convert.ToInt16(shDate.Substring(0, 4));
            //prDate.Year = (System.Convert.ToInt16(shDate.Substring(0, 4)) + YearNums);
            //string s = prDate.ToString("M");
            //string[] words = s.Split(' ');
            //s = words[1] + words[0];
            //return prDate.ToString("d");
            return "";
        }
        public static string tempDecreaseMonths(string shDate, int MonthNums)
        {
            //System.DateTime dt = System.Convert.ToDateTime(Slashed2PersianDate(shDate));
            //System.TimeSpan duration = new System.TimeSpan(MonthNums, 0, 0, 0, 0);
            //dt = dt.Add(duration);
            //return PersianDateConverter.ToPersianDate(dt).ToString("M");
            return "";
        }
        public static PersianDate Slashed2PersianDate(string slashDate)
        {
            PersianDate prDate = new PersianDate();
            prDate.Day = System.Convert.ToInt16(slashDate.Substring(6, 2));
            prDate.Month = System.Convert.ToInt16(slashDate.Substring(3, 2));
            prDate.Year = System.Convert.ToInt16(slashDate.Substring(0, 2));
            return prDate;
        }
        public static string ShamsiMonth(string value)
        {
            try
            {
                int Month = System.Convert.ToInt16(value);
                switch (Month)
                {
                    case 1:
                        value = "فروردين";
                        break;
                    case 2:
                        value = "ارديبهشت";
                        break;
                    case 3:
                        value = "خرداد";
                        break;
                    case 4:
                        value = "تير";
                        break;
                    case 5:
                        value = "مرداد";
                        break;
                    case 6:
                        value = "شهريور";
                        break;
                    case 7:
                        value = "مهر";
                        break;
                    case 8:
                        value = "آبان";
                        break;
                    case 9:
                        value = "آذر";
                        break;
                    case 10:
                        value = "دي";
                        break;
                    case 11:
                        value = "بهمن";
                        break;
                    case 12:
                        value = "اسفند";
                        break;
                    default:
                        value = "-";
                        break;
                }
            }
            catch { }
            return value;
        }
        #endregion

      
        #region Tariff Function
        public static string GetFarsiTariffDesc(string TariffHex)
        {
            if (TariffHex.Trim() == "")
                return "-";
            if (TariffHex.Trim() == "0")
                return "-";
            try
            {
                TariffHex = Hex2Binary(TariffHex, 8);
                string TariffNum = Binary2int(TariffHex.Substring(0, 3), 3).ToString();
                string TariffHour = Binary2int(TariffHex.Substring(3, 5), 5).ToString();
                return " از ساعت  " + TariffHour + "  با تعرفه " + TariffNum + " ";
            }
            catch { }
            return TariffHex;
        }
        #endregion
        #region File Functions
        public static Boolean copyfile(string FullFilePath, string FileName, string DestinationDir)
        {
            try
            {
                DestinationDir = DestinationDir.Replace("/", "_");//.Replace(":", "");
                FileName = FileName.Replace("/", "_");//.Replace(":", "");
                FullFilePath = FullFilePath.Replace("/", "_");//.Replace(":", "");
                if (!Directory.Exists(DestinationDir))
                    Directory.CreateDirectory(DestinationDir);
                string FullFileName = FullFilePath + "\\" + FileName;
                string FullDesFileName = (DestinationDir + "\\" + FileName);
                if (File.Exists(FullFileName))
                    File.Copy(FullFileName, FullDesFileName);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region db Functions
        public static void ConvertDataTable2TextFile(DataTable Dt, string DestinationDir, string FileName)
        {
            try
            {
                if (!Directory.Exists(DestinationDir))
                    Directory.CreateDirectory(DestinationDir);
                string FullDesFileName = (DestinationDir + "\\" + FileName);
                StreamWriter stream = null;
                stream = File.CreateText(FullDesFileName);
                string cipherText = "";
                int rowcount = Dt.Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {
                    cipherText = "";
                    for (int j = 0; j < Dt.Columns.Count; j++)
                    {
                        cipherText += "  " + Dt.Rows[i].ItemArray[j].ToString();
                    }
                    stream.WriteLine("{0}", cipherText);
                }
                stream.Close();
            }
            catch { }
        }
        #endregion
        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null)
            {
                return null;
            }
            if (input.Length > index)
            {
                StringBuilder builder = new StringBuilder(input);
                builder[index] = newChar;
                return builder.ToString();
            }
            return input;
        }
        
        public static string ConvertPersianDateTimeTostring(PersianDate pd)
        {
            return pd.Year.ToString() + "/" + pd.Month.ToString("") + "/" + pd.Day.ToString("") +
                            " " + pd.Hour.ToString("") + ":" + pd.Minute.ToString("") + ":" + pd.Second.ToString(""); ;
        }





        internal static Card.CardReadOut Compute_Monthly_Consumption_Data(Card.CardReadOut cardReadOut, System.Collections.Generic.List<Card.OBISObject> waterConsumption, decimal totalWaterConsumption)
        {
            try
            {
                if (cardReadOut.SoftwareVersion.EndsWith("3"))
                {
                    bool iszeroData = true;
                    if (waterConsumption.Count > 10  && ConvertToDecimal(waterConsumption[0].value) != 0)
                    {
                        for (int i = 1; i < waterConsumption.Count; i++)
                        {
                            if (ConvertToDecimal(waterConsumption[i].value) != 0)
                            {
                                iszeroData = false;
                            }
                        }
                    }
                    if (iszeroData)
                    {
                        cardReadOut.ErrorMessage.MeterErrorMessage.Add(new Card.Message() { message = "اطلاعات به درستی به کارت منتقل نشده است، لطفا مجددا کارت را به کنتور بزنید." });
                    }

                }
                
                
                for (int i = 0; i < waterConsumption.Count - 1; i++)
                {
                    for (int j = i + 1; j < waterConsumption.Count; j++)
                    {
                        if (waterConsumption[i].code.CompareTo(waterConsumption[j].code) > 0)
                        {
                            string value = waterConsumption[i].value;
                            string code = waterConsumption[i].code;
                            string datetime = waterConsumption[i].dateTime;
                            waterConsumption[i].value = waterConsumption[j].value;
                            waterConsumption[i].code = waterConsumption[j].code;
                            waterConsumption[i].dateTime = waterConsumption[j].dateTime;
                            waterConsumption[j].value = value;
                            waterConsumption[j].code = code;
                            waterConsumption[j].dateTime = datetime;
                        }
                    }
                }

                if (waterConsumption.Count== 0 || waterConsumption[0].code != "080201000065")
                    return cardReadOut;

                //double total = double.Parse(waterConsumption[waterConsumption.Count - 1].value);
                decimal total = totalWaterConsumption;
                int k = 101;
                for (int i = 0; i < waterConsumption.Count - 1; i++)
                {
                    Card.OBISObject oo = new Card.OBISObject();
                    if (i == 0)
                    {
                        oo.code = "0802010100FF";
                        
                        oo.value = Math.Round(total - ConvertToDecimal(waterConsumption[i].value),2).ToString();
                        
                        
                        // oo.dateTime = waterConsumption[i].dateTime;
                        oo.dateTime = cardReadOut.ReadOutDateTime;

                        total = ConvertToDecimal(waterConsumption[i].value);
                        cardReadOut.CardData.ObjectList.Add(oo);
                       // SetCaptureDateTimeOfPupmWorking(oo.code, oo.dateTime, cardReadOut.CardData.ObjectList);
                    }
                    if (i > 0 && !waterConsumption[i].code.Equals("0802010000FF"))
                    {
                        oo.code = "0802010100" + k.ToString("X2");
                        oo.value = Math.Round(total - ConvertToDecimal(waterConsumption[i].value), 2).ToString();
                        
                        //oo.dateTime = waterConsumption[i].dateTime;
                        oo.dateTime = waterConsumption[i-1].dateTime;


                        total = ConvertToDecimal(waterConsumption[i].value);
                        cardReadOut.CardData.ObjectList.Add(oo);
                      //  SetCaptureDateTimeOfPupmWorking(oo.code, oo.dateTime, cardReadOut.CardData.ObjectList);
                        k++;
                    }
                }
            }
            catch(Exception ex) 
            {

            }
            return cardReadOut;
        }

       public static decimal ConvertToDecimal(string value)
        {
            decimal res; 
            try
            {
                if (string.IsNullOrEmpty(value))
                    return 0;
                if (decimal.TryParse(value, out res))
                    return res;
                return Convert.ToDecimal(value.Replace(".", "/"));          
            }
            catch (Exception ex)
            {
                return 0;
            }
           
        }

        private static void SetCaptureDateTimeOfPupmWorking(string obis, string dateTime, System.Collections.Generic.List<Card.OBISObject> list)
        {
           
          string  monthlyWaterConsumptionObis = obis.Replace("0802010100","0802606202");
          string monthlyMaxFlowObis = obis.Replace("0802010100", "0802020500");
          
            foreach (var item in list)
            {
                if (item.code.Equals(monthlyWaterConsumptionObis) || item.code.Equals(monthlyMaxFlowObis))
                {
                    item.dateTime = dateTime;                  
                }
            }
        }
       
    }
}