using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DLMS.DTOEntities;
using OfficeOpenXml;
using SABA_CH.DataBase;
using SABA_CH.UI;

namespace SABA_CH.Global
{
    public static class CommonData
    {
        //public static List<frmselectlist> list = new List<frmselectlist>();
        public static MainWindow mainwindow;

        public static List<SelectedMeter> MainList = null;

        #region CardInfo
        public static CardInfoReceive carddataReceive;
        public static string MeterNumber="";
        public static decimal? CardID = 1000000000;
        //public static decimal? MeterID = 8;
        static decimal? _meterID;
        public static decimal? MeterID
        {
            get
            {
                return _meterID;
            }
            set
            {
                _meterID = value;
            }
        }
        public static bool correctReading = true;
        public static bool IsCompleteReadOut = false;
        public static string MeterType = "";
        public static bool SetCreditToCard = true;
        public static double counter = 0;
        public static string CardMeterNumber = "";
        public static decimal? CardMeterID;
        public static string[,] ArrayMainTable207 = new string[7, 36];
        public static string[,] Arraytemp207 = new string[7, 5];
        public static string[,] ArrayCurve207 = new string[7, 17];
        public static string[,] ArrayWater207 = new string[7, 50];
        public static string[,] ArrayActiveEnergy207 = new string[6, 28];
        public static string ProvinceCode = "";
        public static string[,] ArrayMainTable303 = new string[7, 109];
        public static string[,] Arraytemp303 = new string[7, 5];
        public static string[,] ArrayCurve303 = new string[7, 17];
        public static string[,] ArrayWater303 = new string[7, 50];
        public static string[,] ArrayActiveEnergy303 = new string[6, 28];

        public static bool GetShutdownInterval { get; internal set; }
        #endregion CardInfo
        #region Token
        public static bool SendMessageToserialPort = false;
        public static string CreditTransferModes = "";
        public static bool isHVRequest = false;
        public static bool completeRequest = false;
        public static bool isTokenRequestk { get; set; }

        public static string DanglePass = "";//"11CF123C";
        public static string DangleSerial = "";
        public static string LSB = "";
        public static string HVCode = "";
        public static string ClockValue = "";
        public static string ExpireDate = "";
        public static string EncryptExpireDate = "";
        
        public static bool IsGenerateFirstToken = false;
        public static bool getDanglePassFromDB = false;
        public static bool getExpireDateFromDB = false;

        public static bool getResponserFromDangle = false;
        public static int CurrentYear, DayofYear, MinuteOfDay;
        public static int SystemID = 5;
        public static string DefultHashNumber = "0";
        public static int SequenceNumber = 1;
        public static int UserID = 1000000;

        public static string PortName = "";


        

       

        public static bool StopData = false;
        #endregion Token
        public static decimal? LanguagesID = 3;
        public static decimal? SabaCAndHID = 3;
        public static string LanguageName = "";
        public static SelectDetail selectedDetail;
        public static FlowDirection FlowDirection = FlowDirection.LeftToRight;
        public static FontStyle font;
        public static string UserName = "";
        public static bool ShowButton = true;

        public static bool CompleteReadOut = false;
        public static bool AccessUserToDefultGroup = true;
        public static bool getDataFromCard = false;
        public static ShowCardData showmeterdata;
        public static HHUInfoReceive HHUDataReceive;
        public static ExcelPackage ExPackage = new ExcelPackage();
        public static decimal? SelectedMeterID = 10000000;
        public static string SelectedMeterNumber = "";
        public static string   SelectedMeterReadDate = "";
        public static string SelectedMeterTransferDate = "";
        
        public static ShowGroups_Result selectedGroup;
        public static string CardNumber = "";
        public static string SoftwareVersion = "";
        public static decimal? OBISValueHeaderID = 10000000;
        public static Setting setting = new Setting(10, "Arial", "#FFCEBAE0");

        public static string ProgramVersion = "";
        public static bool IsExpire = false;
        // version 4 and uper
        public static int creditCapabilityActivation = 0;
        public static string creditStartDate = "";
        public static int disconnectivityNegativeCredit=0;
        public static int disconnectivityExpiredCredit=0;

        public static bool WaitingFormRun =false;

        //#region ReportCustomer  //29
        //public static string[] ReportCustomer = new string[] {"CustomerName", "Customerfamily","ElecsubscriptionNumber","WatersubscriptionNumber","CustomerTel","NationalCode","CustomerAddress",
        //"","","","","Latitude","Longitude","","","","","","",};

        //#endregion

        public static void New(Grid Griddown)
        {

            try
            {
                TextBox text = null;
                ComboBox Combo = null;
                foreach (var item in Griddown.Children)
                    if (item is TextBox)
                    {
                        text = (TextBox)item;
                        text.Text = "";
                    }
                    //else if (item is ComboBox)
                    //{
                    //    Combo = (ComboBox)item;
                    //    Combo.SelectedIndex = 0;
                    //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       WriteLOG(ex);
            }
        }
        public static void changeFlowDirection(Grid Griddown,FlowDirection flow)
        {
            TextBox text;
            Label   lbl;
            try
            {
                Griddown.FlowDirection = flow;
                foreach (var item in Griddown.Children)
                {
                    if (item is TextBox)
                    {
                        text = (TextBox)item;
                        text.FlowDirection = flow;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       WriteLOG(ex);

            }
        }
        public static ShowTranslateofLable translateWindow(int WindowID)
        {
            try
            {
                ShowTranslateofLable tr = new ShowTranslateofLable("  and languageID=" + LanguagesID.ToString() + "  and WindowID=" + WindowID);
                return tr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       WriteLOG(ex);
                return null;
            }
        }
        public static ShowTranslateofMessage translateMessage()
        {
            try
            {
                ShowTranslateofMessage tm = new ShowTranslateofMessage("  and languageID=" + LanguagesID.ToString() );
                return tm;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       WriteLOG(ex);
                return null;
            }
        }
        public static ShowButtonAccess_Result ShowButtonBinding(string filter, int windowID)
        {
            try
            {
                ShowButtonAccess_Result us = new ShowButtonAccess_Result();
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                string otherFilter=" and Access.UserID="+UserID.ToString() + " and Access.ButtonID=" + windowID.ToString();
                foreach (ShowButtonAccess_Result item in Bank.ShowButtonAccess(filter, otherFilter, LanguagesID))
                {
                    us = new ShowButtonAccess_Result();
                    us = item;
                }
                return us;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static void ChangeFlowDirection( Grid mainGrid)
        {
            try
            {
                Grid Gridown = null,ChildGrid=null;
                DataGrid datagrid = null;
                foreach (var item in mainGrid.Children)
                {
                    if (item is Grid)
                    {
                        Gridown = (Grid)item;
                       // Gridown.FlowDirection = FlowDirection;
                        foreach (var Childitem in Gridown.Children)
                        {
                            if (Childitem is Grid)
                            {
                                ChildGrid = (Grid)Childitem;
                                //ChildGrid.FlowDirection = FlowDirection;
                            }
                        }
                    }
                    else if (item is DataGrid)
                    {
                        datagrid = (DataGrid)item;
                        //datagrid.FlowDirection = FlowDirection;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       WriteLOG(ex);
            }
        }
        public static void showHearderGrid(DataGrid MainGrid, string[,] caption)
        {
            bool Columnview = false;
            try
            {
                for (int i = 0; i < MainGrid.Columns.Count; i++)
                {
                    for (int j = 0; j < caption.Length / 2; j++)
                        if (MainGrid.Columns[i].Header.ToString() == caption[j, 0])
                        {
                            MainGrid.Columns[i].Header = caption[j, 1];
                            Columnview = true;
                        }
                    if (!Columnview)
                        MainGrid.Columns[i].Visibility = Visibility.Hidden;
                    Columnview = false;
                }

            }
            catch
            {
            }
        }
       
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }


        public static string ReverseStatusValue(string OBISValue)
        {
            try
            {
                string str = OBISValue.Substring(6, 2) + OBISValue.Substring(4, 2) + OBISValue.Substring(2, 2) + OBISValue.Substring(0, 2);
                return str;
            }
            catch
            {
                return "";
            }
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
        public static DateTime ByteArrayStringToDateTime(String s)
        {
            try
            {
                byte[] byteArray = hexStrToByteArray(s);//Reverse(s));
                byte[] buff = new byte[2];
                Array.Copy(byteArray, 0, buff, 0, 2);
                Array.Reverse(buff);
                int year = (ushort)(BitConverter.ToUInt16(buff, 0) & 0xFFFF);
                if (year == 0xFFFF)
                    year = 1900;
                byte hour = byteArray[5];
                if (hour == 24)
                    hour = 0;
                return new DateTime(year, byteArray[2], byteArray[3],
                    hour, byteArray[6], byteArray[7], byteArray[8]);

            }
            catch (Exception)
            {
                return RsaDateTime.PersianDateTime.ConvertToGeorgianDateTime(s);
                
            }
           
        }

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
        public static string ASCIIStringToString(string str)
        {

            string result = "";
            for (int i = 0; i < str.Length; i += 2)
            {
                int n = Int32.Parse(str.Substring(i, 2), NumberStyles.HexNumber);
                result += Convert.ToChar(n);
            }
            return result;
        }
        public static string ReverceASCIIStringToString(string str)
        {

            string result = "";
            for (int i = 0; i < str.Length; i += 2)
            {
                int n = Int32.Parse(str.Substring(i, 2), NumberStyles.HexNumber);
                result += Convert.ToChar(n);
            }
            return ReverseString( result);
        }


        public static void WriteLOG(Exception ex)
        {
            try
            {
                if (!EventLog.SourceExists("SABAC_H Log"))
                {                    
                    EventLog.CreateEventSource("SABAC_H Log", ex.ToString());                    
                    return;
                }

                // Create an EventLog instance and assign its source.
                EventLog myLog = new EventLog();
                myLog.Source = "SABAC_H Log";

                // Write an informational entry to the event log.
                myLog.WriteEntry(ex.ToString());

                 
                 
            }
            catch(Exception EX)
            {
            }
        }

        public static string DangleLSB { get; set; }

        public static string Citycode { get; set; }
        public static RsaDateTime.PersianDate MeterShutdownStartDate { get; internal set; }
        public static RsaDateTime.PersianDate MeterShutdownEndDate { get; internal set; }
        public static int MeterShutdownMonths { get; internal set; }
        public static bool? WasTheMeterOff { get; internal set; }
        public static decimal? CustomerId { get; internal set; }
        public static decimal CardCustomerId { get; internal set; }

        internal static string ConvertValue(LstOBISDTO item)
        {
            var convertUnitToHourList = new string[] { "000060070FFF", "0000603300FF", "0802606200FF","0100012500FF", "0802606000FF" };
            var convertUnitToKiloList = new string[] {
                "01000F0800FF",
                "0100010800FF",
                "0100030800FF",
                "0100040800FF",
                "0100010700FF",
                "0100010600FF",
                "0100012E00FF",
                "0802010000FF",
                "01000F0801FF",
                "01000F0802FF",
                "01000F0803FF",
                "01000F0804FF",
                "0100010801FF",
                "0100010802FF",
                "0100010803FF",
                "0100010804FF",
                "0100050800FF",
                "0100060800FF",
                "0100070800FF",
                "0100080800FF",
                "01001F0700FF",
                "0100200700FF",
                "0100330700FF",
                "0100340700FF",
                "0100470700FF",
                "0100480700FF",
                "01000D0700FF",
                "0100820700FF",
                "0802020000FF",
                "0802020500FF",                              
                "0000603E00FF",
                "0000603E01FF",
                "0000603E02FF",
                "0000603E03FF",

            };
            if (item.LogicalName == "0000603E04FF")
            {
                return Convert.ToInt64(item.ValueString).ToString("X2");
            }
            foreach (var obis in convertUnitToHourList)
            {
                if (obis == item.LogicalName)
                {
                    item.ValueString =Math.Round( (Convert.ToDecimal(item.ValueString) / 3600),2).ToString();
                    break;
                }
            }
            foreach (var obis in convertUnitToKiloList)
            {
                if (obis == item.LogicalName)
                {
                    item.ValueString = Math.Round((Convert.ToDecimal(item.ValueString) / 1000), 2).ToString();
                    break;
                }
            }
            switch (item.TypeString)
            {
                case "DateTime":
                    if (item.ValueString.Length == 24)
                    {
                        DateTime cardDate = ByteArrayStringToDateTime(item.ValueString);
                        return  cardDate.ToPersianString();
                    }
                    else if (item.ValueString.Length > 10)
                    {
                        DateTime cardDate = Convert.ToDateTime(item.ValueString);
                        return cardDate.ToPersianString();
                    }
                    break;
                case "ReversedFixSizedVisibleString":
                    return ReversedFixSizedVisibleString(item.ValueString);
                case  "UInt8":
                case "UInt16":
                case "Boolean":
                case "Int64":
                case "Int32":
                case "UInt64":
                case "UInt32": return item.ValueString;
                default:
                    break;
            }
            
            return item.ValueString;
        }


        private static string GetDateTimeFromOctedString(string value)
        {
            string year = int.Parse(value.Substring(0, 4), System.Globalization.NumberStyles.HexNumber).ToString();
            string month = int.Parse(value.Substring(4, 2), System.Globalization.NumberStyles.HexNumber).ToString();
            string day = int.Parse(value.Substring(6, 2), System.Globalization.NumberStyles.HexNumber).ToString();

            string hour = int.Parse(value.Substring(10, 2), System.Globalization.NumberStyles.HexNumber).ToString();
            string minute = int.Parse(value.Substring(12, 2), System.Globalization.NumberStyles.HexNumber).ToString();
            string second = int.Parse(value.Substring(14, 2), System.Globalization.NumberStyles.HexNumber).ToString();
            return year.PadLeft(4, '0') + "-" + month.PadLeft(2, '0') + "-" + day.PadLeft(2, '0') + " " + hour.PadLeft(2, '0') + ":" + minute.PadLeft(2, '0') + ":" + second.PadLeft(2, '0');
        }
        private static string GetDateFromOctedString(string value)
        {
            string year = int.Parse(value.Substring(0, 4), System.Globalization.NumberStyles.HexNumber).ToString();
            string month = int.Parse(value.Substring(4, 2), System.Globalization.NumberStyles.HexNumber).ToString();
            string day = int.Parse(value.Substring(6, 2), System.Globalization.NumberStyles.HexNumber).ToString();
            return year.PadLeft(4, '0') + "-" + month.PadLeft(2, '0') + "-" + day.PadLeft(2, '0');
        }

        private static string ReversedFixSizedVisibleString(string s3)
        {
            StringBuilder result = new StringBuilder();
            string s1 = "";
            for (int i = 0; i < s3.Length; i += 2)
            {
                s1 = s3.Substring(i, 2) + s1;
            }
            result.Append(s1);
            return HexStrToVisibleStr(result).ToString().Replace("\0", string.Empty);
        }
        private static string FixSizedVisibleString(string s)
        {
            if (s.Length == s.Split('0').Length - 1)
                return "0";

            StringBuilder result = new StringBuilder();
            result.Append(s);
            return HexStrToVisibleStr(result).ToString().Replace("\0", string.Empty);
        }

        public static string ReverseHexString(string input)
        {
            try
            {
                char[] arr = input.ToCharArray();
                System.Array.Reverse(arr);
                return new string(arr);
            }
            catch
            {
                return "";
            }
        }

        public static string ByteArraytoHexStr(byte[] s)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                result.Append(ToHexChar((s[i] >> 4) & 0x0F));
                result.Append(ToHexChar(s[i] & 0x0F));
            }
            return result.ToString();
        }
        public static char ToHexChar(int i)
        {
            return ((0 <= i) && (i <= 9)) ? (char)('0' + i) : (char)('A' + (i - 10));
        }
        static string  ConvertEnumEventTypeToString(string value)
        {
            var c1 = int.Parse(value, NumberStyles.HexNumber).ToString();
            switch (c1)
            {
                case "0": return "بدون رویداد ";
                case "1": return "قطع توان کنتور";
                case "2": return "وصل توان کنتور";
                case "3": return " فرارسیدن زمان تعویض باتری کنتور";
                case "4": return " خطای منطقی یا فیزیکی در کنتور";
                case "5": return " راه اندازی Firmware جدید";
                case "6": return "تخصیص اعتبار جدید";
                case "7": return " میدان مغناطیسی DC شدید در نزدیکی کنتور";
                case "8": return " باز شدن در ترمینال  کنتور";
                case "9": return " پاک شدن رویدادهای ثبت شده";
                case "10": return " تجاوز بیشیه دبی لحظه ای از 20 درصد حد آستانه برای دوروز متوالی";
                case "11": return " رسیدن به آستانه مجاز حجم برداشت آب";
                case "12": return "صدور دستور قطع و اعمال آن توسط کنتور برق به صور محلی";
                case "13": return "صدور دستور وصل مجدد و اعمال توسط کنتور برق به صورمحلی";
                case "14": return "برداشت آب در هنگام اعمال دستور قطع تا قبل از وصل مجدد";
                case "15": return "ثبت کاربری که با موفقیت احراز هویت شده است";
                case "16": return "احراز هویت ناموفق";
                case "17": return " OPERATIONAL KEYتغییر کلید";
                case "18": return "تغییر Secret 1";
                case "19": return "تغییر Secret 2";

                case "20": return "تنظیم ساعت توسط ارتباط محلی";
                case "21": return "تغییر MASTER KEY  ";
                case "22": return "اعلان لوله خالی";
                case "30": return " باز شدن محفظه  کنتور";

                case "31": return "اعلان خطا دریافت Firmware جدید";
                case "32": return "اعلان تغییر تنظیمات در ماژول";
                case "33": return "فرا رسیدن Day Light Saving";
                case "35": return " MBUS قطع کابل";




                default:
                    break;
            }
            return value;
        }
        public static StringBuilder HexStrToVisibleStr(StringBuilder s)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;
            int j = 0;
            int h = 0;
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
                    throw new Exception("HexStrToByteStr: Illegal character");
                }
                if (j == 2)
                {
                    result.Append((char)h);
                    h = 0;
                    j = 0;
                }
                i++;
            }
            return result;
        }
    }

   
}
