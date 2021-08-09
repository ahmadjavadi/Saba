using Card.Classes;
using CARD;
using RsaDateTime;
/*=======================================================================================
CardManager.cs
Manager for Read from Card & Write on Card.
Created by : Ahmad Javadi (A.Javadi@rsa.co.ir)
Created on : 1392/6/21
Last Updated by : .
Last Updated : 
Copyright © 1392, Rahrovan Sepehr Andisheh (Pte. Co.)
http://www.rsa-electronics.com
=======================================================================================*/
using System;
using System.Collections.Generic;

namespace Card
{
    public class CardManager
    {
        CardReadOut cardReadOut;

        MessageHandler messageHandler;


        public CardManager()
        {
            messageHandler = new MessageHandler();
            messageHandler.DataReceived += new dataReceived(messageHandler_DataReceived);
            cardReadOut = new CardReadOut();
            cardReadOut.ErrorMessage = new ErrorMessage();
            cardReadOut.ErrorMessage.CardErrorMessage = new List<Message>();
            cardReadOut.ErrorMessage.MeterErrorMessage = new List<Message>();

            cardReadOut.CardData = new global::Card.CardData();
            cardReadOut.CardData.ObjectList = new List<OBISObject>();
        }
        public CardReadOut getCardData()
        {

            CardDataList.ClearMainData();
            ClearCardReadout();
            decimal totalWaterConsumption = 0;
            string capturesTime = "";

            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    #region Read Main Data From Card

                    for (int i = 0; i < 4; i++)
                    {
                        CardDataList.MainData207[i, 0] = ReadRecord(CardDataList.MainData207[i, 1], CardDataList.MainData207[i, 2], CardDataList.MainData207[i, 5], CardDataList.MainData207[i, 3], CardDataList.MainData207[i, 4], "100");
                    }

                    cardReadOut.Card_CityCode = CardDataList.MainData207[1, 0];
                    cardReadOut.CardNumber = CardDataList.MainData207[0, 0];
                    cardReadOut.SerialNumber = CardDataList.MainData207[2, 0];
                    #endregion

                    if (CardDataList.MainData207[2, 0].StartsWith("207"))
                    {
                        #region Read 207 Objects
                        if (Connect2Card())
                            if (Auth2Card())
                            {
                                for (int i = 0; i < (CardDataList.card207Data.Length / 8); i++)
                                {
                                    OBISObject oo = new OBISObject();
                                    oo.value = ReadRecord(CardDataList.card207Data[i, 1], CardDataList.card207Data[i, 2], CardDataList.card207Data[i, 5], CardDataList.card207Data[i, 3], CardDataList.card207Data[i, 4], CardDataList.card207Data[i, 7]);
                                    oo.code = CardDataList.card207Data[i, 6];

                                    if (oo.code.Equals("0100000402FF"))
                                    {
                                        string[] s = oo.value.Split('/');
                                        oo.value = s[0];
                                        cardReadOut.CardData.ObjectList.Add(oo);

                                        OBISObject oo1 = new OBISObject();
                                        oo1.value = s[1];
                                        oo1.code = "0100000405FF";
                                        cardReadOut.CardData.ObjectList.Add(oo1);
                                    }
                                    else
                                    {
                                        if (oo.code.Equals("0000010000FF"))
                                        {
                                            cardReadOut.ReadOutDateTime = oo.value.Replace(" 00:00:00", "");
                                        }

                                        if (oo.code.Equals("0100000200FF"))
                                        {
                                            cardReadOut.SoftwareVersion = oo.value;
                                        }


                                        cardReadOut.CardData.ObjectList.Add(oo);
                                    }
                                }

                                cardReadOut.SerialNumber = CardDataList.MainData207[2, 0];
                            }

                        DisconnectCard();
                        return cardReadOut;
                        #endregion Read 207 Objects
                    }

                    else
                    {
                        #region Read 303 Objects
                        OBISObject currentConsumption = null;
                        CardDataList.MainData303[0, 0] = ReadRecord(CardDataList.MainData303[0, 1], CardDataList.MainData303[0, 2], CardDataList.MainData303[0, 5], CardDataList.MainData303[0, 3], CardDataList.MainData303[0, 4], "100");

                        if (CardDataList.MainData303[0, 0].Equals(""))
                        {
                            if (cardReadOut.SerialNumber == "167838211")
                                messageHandler.shoWMeterMessage("-2");
                            else
                                messageHandler.shoWMeterMessage("-1");
                            DisconnectCard();
                            return cardReadOut;
                        }

                        if (CardDataList.MainData303[0, 0].StartsWith("1197643879"))
                        {
                            messageHandler.shoWMeterMessage("-2");
                            DisconnectCard();
                            return cardReadOut;
                        }

                        else
                        {
                            List<OBISObject> waterConsumption = new List<OBISObject>();
                            bool isDuplicateData = false;
                            for (int i = 0; i < (CardDataList.card303Data.Length / 8); i++)
                            {
                                try
                                {
                                    isDuplicateData = false;
                                    OBISObject oo = new OBISObject();
                                    oo.code = CardDataList.card303Data[i, 6];
                                    int a = -1;
                                    Int32.TryParse(CardDataList.card303Data[i, 2].ToString(), out a);

                                    if (a > 90)
                                        CardDataList.card303Data[i, 2] = CardDataList.card303Data[i, 2];
                                    oo.value = ReadRecord303Card(CardDataList.card303Data[i, 1], CardDataList.card303Data[i, 2], CardDataList.card303Data[i, 6], CardDataList.card303Data[i, 5], i, out capturesTime);
                                    oo.code = CardDataList.card303Data[i, 6];

                                    if (oo.code.Contains("0802606200"))
                                        oo.code = oo.code.Replace("0802606200", "0802606202");


                                    if (oo.code.Equals("0000010000FF"))
                                    {
                                        if (oo.value != @"0001/01/01 00:00:00")
                                        {
                                            if(cardReadOut.ReadOutDateTime == null)
                                                cardReadOut.ReadOutDateTime = oo.value;
                                            if (currentConsumption != null)
                                                currentConsumption.dateTime = oo.value;
                                        }
                                    }

                                    if (oo.code.Equals("0100000200FF"))
                                        if (!string.IsNullOrEmpty(oo.value) && oo.value.Contains("RSA") )
                                        {
                                            cardReadOut.SoftwareVersion = oo.value;
                                            //cardReadOut.CardData.ObjectList[6].value = oo.value;
                                        }

                                    foreach (var item in cardReadOut.CardData.ObjectList)
                                    {
                                        if (item.code == oo.code)
                                        {
                                            isDuplicateData = true;
                                            if (item.code.Equals("0100000200FF") && oo.value.Contains("RSA"))
                                                item.value = oo.value;
                                            //cardReadOut.CardData.ObjectList[6].value = oo.value;
                                            break;
                                        }
                                    }
                                    if (isDuplicateData)
                                        continue;

                                    if (oo.code.StartsWith("0802010000"))
                                    {
                                        if (oo.code.StartsWith("0802010000FF"))
                                        {

                                            if (cardReadOut.ReadOutDateTime == null)
                                                currentConsumption = oo;
                                            else
                                                oo.dateTime = cardReadOut.ReadOutDateTime;
                                        }
                                        else
                                            oo.dateTime = capturesTime;
                                        if(oo.dateTime!= "1921/03/21 00:00:00" || oo.value!= "0")
                                            waterConsumption.Add(oo);
                                    }

                                    if (oo.code.StartsWith("0802606202"))
                                    {
                                        if (oo.code.StartsWith("0802606202FF"))
                                        {

                                            if (cardReadOut.ReadOutDateTime == null)
                                                currentConsumption = oo;
                                            else
                                                oo.dateTime = cardReadOut.ReadOutDateTime;
                                        }
                                        else
                                            oo.dateTime = capturesTime;
                                    }

                                    if (oo.code.StartsWith("0802010000FF"))
                                        totalWaterConsumption =GeneralFunctions. ConvertToDecimal(oo.value);
                                    if (oo.value != "Empty" && oo.code != "")
                                        cardReadOut.CardData.ObjectList.Add(oo);

                                }
                                catch (Exception ex)
                                {
                                }

                            }

                            cardReadOut.SerialNumber = CardDataList.MainData303[0, 0];
                            DisconnectCard();
                            cardReadOut = GeneralFunctions.Compute_Monthly_Consumption_Data(cardReadOut, waterConsumption, totalWaterConsumption);

                            return cardReadOut;
                        }
                        #endregion Read 303 Objects
                    }

                }
            }
            DisconnectCard();
            return cardReadOut;
        }

        private void ClearCardReadout()
        {
            cardReadOut = new CardReadOut();
            cardReadOut.ErrorMessage = new ErrorMessage();
            cardReadOut.ErrorMessage.CardErrorMessage = new List<Message>();
            cardReadOut.ErrorMessage.MeterErrorMessage = new List<Message>();

            cardReadOut.CardData = new global::Card.CardData();
            cardReadOut.CardData.ObjectList = new List<OBISObject>();
        }

        public string[] getAllCardData()
        {
            string[] AllCardData = new string[1000];
            int i = 0;
            string selectFileName = "";
            int LastErrorCode = 0;
            string value = "";

            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    #region  MCU ID

                    AllCardData[i] = "=================== MCU ID ======================================";
                    i++;
                    selectFileName = "FF00";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 2; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }
                    #endregion

                    #region   ManufactureFile
                    AllCardData[i] = "======================== ManufactureFile ========================";
                    i++;

                    selectFileName = "FF01";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 2; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }
                    #endregion

                    #region PersonalizationFile
                    AllCardData[i] = "================ PersonalizationFile  ===========================";
                    i++;
                    selectFileName = "FF02";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 4; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }

                    #endregion

                    #region  UserManagementFile
                    AllCardData[i] = "=====================  UserManagementFile  ======================";
                    i++;

                    selectFileName = "FF04";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 9; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }

                    #endregion

                    #region  Meter ID
                    AllCardData[i] = "=======================  Meter ID  ======================================";
                    i++;

                    selectFileName = "1001";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 5; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }

                    #endregion

                    #region  Credit Parameter
                    AllCardData[i] = "========================  Credit Parameter  =====================";
                    i++;

                    selectFileName = "1003";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 10; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }

                    #endregion

                    #region  WellParameters
                    AllCardData[i] = "======================  WellParameters  =========================";
                    i++;


                    selectFileName = "1004";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 20; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }

                    #endregion

                    #region  WaterConsumption

                    AllCardData[i] = "================ WaterConsumption  ===============================";
                    i++;

                    selectFileName = "1005";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 56; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }

                    #endregion

                    #region  ElectricConsumption
                    AllCardData[i] = "====================  ElectricConsumption  ======================";
                    i++;

                    selectFileName = "1006";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 36; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }

                    #endregion

                    #region 303METER IDS
                    AllCardData[i] = "======================  303METER IDS  ===========================";
                    i++;


                    selectFileName = "2001";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 10; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }

                    #endregion

                    #region CREDIT PARAMETERS
                    AllCardData[i] = "======================== CREDIT PARAMETERS ======================";
                    i++;


                    selectFileName = "2002";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 30; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }
                    #endregion

                    #region  "MCU ID"
                    AllCardData[i] = "=================================================================";
                    i++;


                    selectFileName = "2003";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 150; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }

                    #endregion

                    #region  REPORTED_VARIABLES
                    AllCardData[i] = "================   REPORTED_VARIABLES    ========================";
                    i++;

                    selectFileName = "2004";
                    card.SelectFile(selectFileName);
                    for (int j = 0; j < 100; j++)
                    {
                        value = "";

                        LastErrorCode = card.ReadRecord(Convert.ToByte(j), out value, selectFileName);
                        if (LastErrorCode == 3001)
                            AllCardData[i] = selectFileName + ", " + j.ToString() + ", " + value;
                        else
                            AllCardData[i] = selectFileName + ", " + j.ToString() + " Error " + LastErrorCode.ToString();
                        i++;
                    }

                    #endregion


                    return AllCardData;
                }
            }
            return null;

        }

        private void messageHandler_DataReceived(object sender, MessageEventArgs arg)
        {
            Message m = new Message();
            m.message = arg.Message;
            if (arg.MessageType == MessageEventsType.CardError)
                cardReadOut.ErrorMessage.CardErrorMessage.Add(m);
            else
            {
                if (cardReadOut.ErrorMessage.CardErrorMessage.Count > 0)
                {
                    if (m.message.Equals("اطلاعاتی در کارت ثبت نشده است") && cardReadOut.ErrorMessage.CardErrorMessage[0].message.Contains("3008"))
                    {
                        cardReadOut.ErrorMessage.MeterErrorMessage.Add(m);
                        cardReadOut.ErrorMessage.CardErrorMessage[0].message = "اطلاعاتی در کارت ثبت نشده است";
                    }
                    else
                        cardReadOut.ErrorMessage.MeterErrorMessage.Add(m);
                }
                else
                    cardReadOut.ErrorMessage.MeterErrorMessage.Add(m);
            }
        }

        #region Read Main Meter Data From Card303
        void Read_Main_Meter_Data_From_Card303(CardReadOut cardreadout)
        {
            CardDataList.ClearMainData();
            if (Connect2Card())
            {

                if (Auth2Card())
                {
                    for (int i = 0; i < 3; i++)
                    {
                        CardDataList.MainData303[i, 0] = ReadRecord(CardDataList.MainData303[i, 1], CardDataList.MainData303[i, 2], CardDataList.MainData303[i, 5], CardDataList.MainData303[i, 3], CardDataList.MainData303[i, 4], "100");
                    }
                }

                DisconnectCard();
            }
        }
        #endregion Read Main Meter Data From Card303

        private string ConvertReadValueOf303Card(string readValue, string dataConvertRule, string OBIS, string unitConvertRule, out string capturesTime)
        {
            capturesTime = "";
            try
            {
                string OBISCode = "", data = "", Value = "";
                if (OBIS == "")
                    OBISCode = readValue.Substring(0, 12);

                Value = readValue.Substring(14, readValue.Length - 14);

                switch (dataConvertRule)
                {
                    case "0":
                        data = Value;
                        break;
                    case "1":
                        int yy = int.Parse(CardDataConverter.Reversevalue(Value.Substring(0, 4)), System.Globalization.NumberStyles.HexNumber);
                        int mm = int.Parse(Value.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                        int dd = int.Parse(Value.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                        int h = int.Parse(Value.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);
                        int m = int.Parse(Value.Substring(12, 2), System.Globalization.NumberStyles.HexNumber);
                        int s = int.Parse(Value.Substring(14, 2), System.Globalization.NumberStyles.HexNumber);
                        DateTime dt = new DateTime(1921,3,21,0,0,0);
                        try
                        {
                            if (yy.ToString().StartsWith("20"))
                                dt = new DateTime(yy, mm, dd, h, m, s);
                            else
                            {
                                PersianDate pd = new PersianDate(yy, mm, dd, h, m, s);
                                dt = pd.ConvertToGeorgianDateTime();
                            }
                        }
                        catch (Exception) { }
                        //data = GeneralFunctions.ConvertPersianDateTimeTostring(PersianDateConverter.ToPersianDate(dt));
                        data = dt.ToString("yyyy/MM/dd HH:mm:ss");
                        break;

                    case "2":
                        data = GeneralFunctions.Reverse(Value.Substring(0, 2 * sizeof(int)));
                        data = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber).ToString();
                        break;
                    case "3":
                        data = GeneralFunctions.Reverse(Value.Substring(0, 2 * sizeof(uint)));
                        data = UInt32.Parse(data, System.Globalization.NumberStyles.HexNumber).ToString();
                        break;

                    case "4":
                        data = GeneralFunctions.ByteArrayStringToDateTime(Value.Substring(0, 40)).ToString();
                        data = Convert.ToDateTime(data).ToString();
                        break;
                    case "5":
                        data = Value.Substring(0, 2);
                        if (data == "00")
                            data = false.ToString();
                        else
                            data = true.ToString();
                        break;
                    case "6":
                        data = GeneralFunctions.Reverse(Value.Substring(0, 2 * sizeof(byte)));
                        data = Convert.ToByte(data).ToString();
                        break;
                    case "7":
                        DateTime dt1 = GeneralFunctions.ByteArrayStringToDateTime(Value.Substring(0, 24));

                        if (dt1.Year.ToString().StartsWith("13") || dt1.Year.ToString().StartsWith("14"))
                        {
                            PersianDate pd = new PersianDate(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, dt1.Second);
                            dt = pd.ConvertToGeorgianDateTime();
                            data = dt.ToString("yyyy/MM/dd HH:mm:ss");
                        }
                        else
                        {
                            data = dt1.ToString("yyyy/MM/dd HH:mm:ss");
                        }
                        break;
                    case "8":
                        data = GeneralFunctions.Reverse(Value.Substring(0, 2 * sizeof(ulong)));
                        data = UInt64.Parse(data, System.Globalization.NumberStyles.HexNumber).ToString();
                        break;
                    case "9":
                        data = GeneralFunctions.Reverse(Value.Substring(0, 2 * sizeof(ushort)));
                        data = UInt16.Parse(data, System.Globalization.NumberStyles.HexNumber).ToString();
                        break;
                    case "10":
                        data = GeneralFunctions.Reverse(Value.Substring(0, 2 * sizeof(long)));
                        data = Int64.Parse(data, System.Globalization.NumberStyles.HexNumber).ToString();
                        break;

                    case "11":
                        data = GeneralFunctions.ASCIIStringToString(Value.Substring(0, 40));
                        data = data.Replace("\0", "");

                        break;
                    case "12":
                        data = GeneralFunctions.Reverse(Value.Substring(0, 2 * sizeof(short)));
                        data = Int16.Parse(data, System.Globalization.NumberStyles.HexNumber).ToString();
                        break;
                    case "13":
                        // byte[] array = Encoding.ASCII.GetBytes(Value);
                        data = Value;
                        break;

                    case "14":
                        data = Value.Substring(0, 2 * sizeof(uint));
                        break;

                    case "20": //Water Consumption With Capture time
                        data = GeneralFunctions.Reverse(Value.Substring(0, 2 * sizeof(ulong)));
                        data = UInt64.Parse(data, System.Globalization.NumberStyles.HexNumber).ToString();
                        if (Value.Substring(16) != "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")
                        {                            
                            yy = int.Parse(Value.Substring(16, 4), System.Globalization.NumberStyles.HexNumber);
                            //if(!yy.ToString().StartsWith("20"))
                            //    yy = int.Parse(CardDataConverter.Reversevalue(Value.Substring(16, 4)), System.Globalization.NumberStyles.HexNumber);
                            mm = int.Parse(Value.Substring(20, 2), System.Globalization.NumberStyles.HexNumber);
                            dd = int.Parse(Value.Substring(22, 2), System.Globalization.NumberStyles.HexNumber);
                            h = int.Parse(Value.Substring(26, 2), System.Globalization.NumberStyles.HexNumber);
                            m = int.Parse(Value.Substring(28, 2), System.Globalization.NumberStyles.HexNumber);
                            s = int.Parse(Value.Substring(30, 2), System.Globalization.NumberStyles.HexNumber);
                            dt = new DateTime(1921,3,21,0,0,0);
                            try
                            {
                                if (yy.ToString().StartsWith("20"))
                                    dt = new DateTime(yy, mm, dd, h, m, s);
                                else
                                {
                                    PersianDate pd = new PersianDate(yy, mm, dd, h, m, s);
                                    dt = pd.ConvertToGeorgianDateTime();
                                }
                            }
                            catch (Exception) { }
                            //data = GeneralFunctions.ConvertPersianDateTimeTostring(PersianDateConverter.ToPersianDate(dt));
                            capturesTime = dt.ToString("yyyy/MM/dd HH:mm:ss");
                        }
                        else
                        {
                            capturesTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        }

                        break;

                    case "21": //Time Of Pump Working With Capture time
                        data = GeneralFunctions.Reverse(Value.Substring(0, 2 * sizeof(uint)));
                        data = UInt32.Parse(data, System.Globalization.NumberStyles.HexNumber).ToString();

                          if (Value.Substring(16) != "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")
                          {
                            //yy = int.Parse(CardDataConverter.Reversevalue(Value.Substring(16, 4)), System.Globalization.NumberStyles.HexNumber);
                            yy = int.Parse(Value.Substring(16, 4), System.Globalization.NumberStyles.HexNumber);
                            
                            mm = int.Parse(Value.Substring(20, 2), System.Globalization.NumberStyles.HexNumber);
                            dd = int.Parse(Value.Substring(22, 2), System.Globalization.NumberStyles.HexNumber);
                            h = int.Parse(Value.Substring(26, 2), System.Globalization.NumberStyles.HexNumber);
                            m = int.Parse(Value.Substring(28, 2), System.Globalization.NumberStyles.HexNumber);
                            s = int.Parse(Value.Substring(30, 2), System.Globalization.NumberStyles.HexNumber);
                            dt = new DateTime(1921,3,21,0,0,0);;
                            try
                            {
                                if (yy.ToString().StartsWith("20"))
                                    dt = new DateTime(yy, mm, dd, h, m, s);
                                else
                                {
                                    PersianDate pd = new PersianDate(yy, mm, dd, h, m, s);
                                    dt = pd.ConvertToGeorgianDateTime();
                                }
                            }
                            catch (Exception) { }
                            //data = GeneralFunctions.ConvertPersianDateTimeTostring(PersianDateConverter.ToPersianDate(dt));
                            capturesTime = dt.ToString("yyyy/MM/dd HH:mm:ss");
                        }
                        else
                        {
                            capturesTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        }
                          break;

                    default:
                        data = Value;
                        //System.IO.StreamWriter sw = System.IO.File.AppendText(@"C:\Data.txt");
                        //sw.WriteLine("OBIS: " + OBIS + "  Value: " + Value + "   dataConvertRule:  " + dataConvertRule + "    unitConvertRule:  " + unitConvertRule);
                        //sw.Close();
                        break;
                }
               
                return ConvertUnit(data, unitConvertRule);
            }
            catch
            {
                //System.IO.StreamWriter sw = System.IO.File.AppendText(@"C:\Data.txt");
                //sw.WriteLine(OBIS + "  ");
                //sw.Close();
                return "NA";
            }
        }

        private string ConvertUnit(string data, string unitConvertRule)
        {
            decimal d = 0;
            if (unitConvertRule != "0")
            {
                try
                {
                    d = Convert.ToDecimal(data);
                }
                catch (Exception)
                {
                    return data;
                }
            }
            switch (unitConvertRule)
            {
                case "1":
                    data = (d / 1000).ToString("0.##").Replace("/",".");
                    break;
                case "2":
                    try {
                        UInt64 time = Convert.ToUInt64(data);
                        UInt64 h = time / 3600;
                        UInt64 m = (time - (h* 3600)) / 60;
                        UInt64 sec = time - (h * 3600) - (m*60) ;
                        data = h.ToString().PadLeft(2, '0') + "." + m.ToString().PadLeft(2, '0');// +"." + sec.ToString().PadLeft(2, '0');
                    }
                    catch { return data; }
                    
                    break;
                default:
                    break;
            }
            return data;
        }

        private string changeReadValueUnit(string ReadValue, int unitConvertType)
        {
            string convertedValue = ReadValue;
            decimal d = 0;
            try
            {

                switch (unitConvertType)
                {
                    case 1: //Lit To Cubic Metre
                        d = Convert.ToDecimal(ReadValue) / 1000;
                        convertedValue = d.ToString("0.##");
                        break;
                    case 2: // mA To A
                        d = Convert.ToDecimal(ReadValue) / 1000;
                        convertedValue = d.ToString("0.##");
                        break;
                    case 3:// mLit/Sec To Lit/Sec
                        d = Convert.ToDecimal(ReadValue) / 1000;
                        convertedValue = d.ToString("0.##");
                        break;
                    case 4: //mV To V
                        d = Convert.ToDecimal(ReadValue) / 1000;
                        convertedValue = d.ToString("0.##");
                        break;
                    case 5: // Second To Minutes
                        d = Convert.ToDecimal(ReadValue) / 60;
                        convertedValue = d.ToString("0.##");
                        break;
                    case 6: //Varh To KVarh
                        d = Convert.ToDecimal(ReadValue) / 1000;
                        convertedValue = d.ToString("0.##");
                        break;
                    case 7:// W to KW
                        d = Convert.ToDecimal(ReadValue) / 1000;
                        convertedValue = d.ToString("0.##");
                        break;
                    case 8: //WH To KWH                       
                        d = Convert.ToDecimal(ReadValue) / 1000;
                        convertedValue = d.ToString("0.##");
                        break;
                    case 9: //second To Hour
                        d = Convert.ToDecimal(ReadValue) / 3600;
                        convertedValue = d.ToString("0.##");
                        break;
                    default:
                        break;
                }
                return convertedValue;
            }
            catch (Exception)
            {
                return convertedValue;

            }
        }

        #region Private Objects
        private CardReaderFunction card = new CardReaderFunction();
        private string selectFileName = "";
        #endregion

        #region private Functions
        private bool Connect2Card()
        {
            int LastErrorCode = card.ConnectToCard();

            if (LastErrorCode == 3001)
                return true;
            else
            {
                System.Threading.Thread.Sleep(1000);
                LastErrorCode = card.ConnectToCard();
                if (LastErrorCode == 3001)
                    return true;
                messageHandler.showCardMessage(LastErrorCode.ToString());
                return false;
            }
        }

        private bool Auth2Card()
        {
            int LastErrorCode = card.Authentication();
            if (LastErrorCode == 3001)
                return true;
            else
            {
                System.Threading.Thread.Sleep(1000);
                LastErrorCode = card.Authentication();
                if (LastErrorCode == 3001)
                    return true;
                messageHandler.showCardMessage(LastErrorCode.ToString());
                return false;
            }
        }

        private bool DisconnectCard()
        {
            try
            {
                card.DisconnectFromCard();
            }
            catch
            {
            }
            return true;
        }

        private string ReadRecord(string sfileName, string recordIndex, string convertType, string startbyte, string recordLength, string unitConvertRule)
        {
            string value = "", capturesTime = "";
            int LastErrorCode;

            try
            {
                selectFileName = sfileName;
                LastErrorCode = card.SelectFile(selectFileName);

                LastErrorCode = card.ReadRecord(Convert.ToByte(recordIndex), out value, selectFileName);
                if (LastErrorCode == 3001)
                {
                    
                    value = CardDataConverter.convertCardReadValue(convertType, startbyte, recordLength, value);

                    if (selectFileName == "2001" && recordIndex == "00")
                    {
                        if (value == "0")
                        {
                            cardReadOut.SerialNumber = "0";
                            messageHandler.shoWMeterMessage("0");
                        }
                        else if (value == "1197643879")
                        {
                            cardReadOut.SerialNumber = "1197643879";
                            messageHandler.shoWMeterMessage("-2");
                        }
                        else if (value.Length == 7)
                        {
                            string year = "92";
                            if (Convert.ToInt32(value.Substring(1, 6)) > 4999)
                                year = "93";

                            value = "19" + value[0] + year + value.Substring(1, 6);
                            cardReadOut.SerialNumber = value;
                        }
                        else
                        {
                            value = ReadRecord303Card("2001", "02", "0100000001FF", "11", 1, out  capturesTime);
                            if (!(value.StartsWith("194") || value.StartsWith("193")))
                            {
                                cardReadOut.SerialNumber = value;
                                messageHandler.shoWMeterMessage("-1");
                            }
                            cardReadOut.SerialNumber = value;
                        }
                    }

                    return value;
                }
                else
                {
                    if (cardReadOut.Card_CityCode.Length < 2)
                        messageHandler.showCardMessage(LastErrorCode.ToString());
                    return value;
                }
            }
            catch
            {
                return "";
            }
        }

        private string ReadRecord303Card(string sfileName, string recordIndex, string OBIS, string convrtRule, int i, out string capturesTime)
        {
            string value = "";
            int LastErrorCode;
            capturesTime = "";
            try
            {
                selectFileName = sfileName;

                LastErrorCode = card.SelectFile(selectFileName);

                LastErrorCode = card.ReadRecord(Convert.ToByte(recordIndex), out value, selectFileName);
                 
                if (LastErrorCode == 3001)
                {
                    
                         
                    if (value == "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")
                        return "Empty";

                    if (value.Contains("0100010400FF02") || value.Contains("01000F0400FF"))//0100010400FF
                        OBIS = "0100010400FF02";

                    if (value.Contains("0100010400FF09"))
                        OBIS = "0100010400FF09";


                    if (OBIS.Equals(""))
                    {
                        OBIS = value.Substring(0, 12);
                        value = value.Replace(OBIS, "");
                        value = OBIS.PadRight(12, '0') + value;

                    }

                    else if (!value.StartsWith(OBIS))
                    {
                        if (selectFileName != "2004")
                            value = OBIS.PadRight(12, '0') + "00" + value;
                        else
                        {
                            OBIS = value.Substring(0, 12);
                            value = value.Replace(OBIS, "");
                            value = OBIS.PadRight(12, '0') + value;
                        }

                    }

                    CardDataList.card303Data[i, 6] = OBIS;


                    if (OBIS != "FFFFFFFFFFFF")
                    {
                        string unitConvertRule = "0";

                        value = ConvertReadValueOf303Card(value, CardDataList.GetConvertRuleOFALLOBISList(OBIS, ref unitConvertRule), OBIS, unitConvertRule, out capturesTime);
                        if (selectFileName == "2001" && recordIndex == "00" || OBIS.Contains("0000600100FF"))
                        {

                            string electricityIdentier = ReadRecord303Card("2001", "02", "0100000001FF", "11", 1, out  capturesTime);

                            if (value.Length < 7)
                            {
                                value = electricityIdentier;
                            }
                            else
                            {
                                char ch = value[0];
                                int meterNo = int.Parse(value.Substring(1));
                                if (meterNo < 5000)
                                    value = "19" + ch + "92" + value.Substring(1);
                                else
                                    value = "19" + ch + "93" + value.Substring(1);

                            }
                        }
                    }
                    return value;
                }
                else
                {
                    messageHandler.showCardMessage(LastErrorCode.ToString());
                    return value;
                }
            }
            catch
            {

                return "";
            }
        }

        private bool WriteRecord(string sfileName, string recordIndex, string value)
        {
            int LastErrorCode = 0;

            try
            {
                selectFileName = sfileName;
                if (Connect2Card())
                    if (Auth2Card())
                    {
                        LastErrorCode = card.SelectFile(selectFileName);
                        LastErrorCode = card.WriteRecord(Convert.ToByte(recordIndex), value, selectFileName);
                    }
                if (LastErrorCode == 3001)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Write Token To 303 Card

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns>
        /// -1 : خطا در برقراری ارتباط با کارت 
        /// 0: خطا در ثبت اطلاعات در کارت
        /// 1 : اطلاعات با موفقیت در کارت ثبت شد
        /// </returns>
        int CheckToken303Card(string token)
        {
            if (Connect2Card())
            {

                if (Auth2Card())
                {
                    string readRecord = ReadRecord("2002", "00", "16", "0", "0", "100");
                    if (readRecord == token)
                    {
                        DisconnectCard();
                        return 1;
                    }
                    else
                    {
                        DisconnectCard();
                        return 0;
                    }
                }

                else
                {
                    DisconnectCard();
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns>
        /// -2 : کارت تغییر کرده است
        /// -1 : خطا در برقراری ارتباط با کارت 
        /// 0: خطا در ثبت اطلاعات در کارت
        /// 1 : اطلاعات با موفقیت در کارت ثبت شد
        /// </returns>        
        public int WriteTokenTo303Card(string token, string MeterNo, bool activeCreditActivation, int credit_Capability_Activation,
            DateTime creditStartDate, int disconnectivity_On_Negative_Credit, int disconnectivity_On_Expired_Credit)
        {
            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    string readMetereOnCard = ReadRecord("2001", "00", "14", "0", "", "100");

                    if (!readMetereOnCard.Contains(MeterNo))
                    {
                        DisconnectCard();
                        return -2;
                    }
                    WriteRecord("2002", "00", token);

                    #region Active Credit Capability
                    if (!activeCreditActivation)
                    {
                        WriteRecord("2002", "28", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
                    }
                    else
                    {
                        string finalCode = "39";
                        string credit_Capability = "FF";
                        string Negative_Credit = "FF";
                        string Expired_Credit = "FF";
                        string hyear = "FF", lyear = "FF", month = "FF", day = "FF";

                        if (credit_Capability_Activation == 1)
                        {
                            credit_Capability = "75";

                            lyear = creditStartDate.Year.ToString("X").PadLeft(4, '0').Substring(2, 2);
                            hyear = creditStartDate.Year.ToString("X").PadLeft(4, '0').Substring(0, 2);
                         
                            month = creditStartDate.Month.ToString("X").PadLeft(2, '0');

                            day = creditStartDate.Day.ToString("X").PadLeft(2, '0');

                        }
                        if (credit_Capability_Activation == -1)
                            credit_Capability = "79";

                        if (disconnectivity_On_Negative_Credit == 1)
                            Negative_Credit = "75";
                        if (disconnectivity_On_Negative_Credit == -1)
                            Negative_Credit = "79";

                        if (disconnectivity_On_Expired_Credit == 1)
                            Expired_Credit = "75";
                        if (disconnectivity_On_Expired_Credit == -1)
                            Expired_Credit = "79";

                        finalCode = "39" + credit_Capability + hyear + lyear + month + day + "75" + "00000000" +
                            Negative_Credit + Expired_Credit + "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                       
                        WriteRecord("2002", "28", finalCode);
                    }
                    #endregion Active Credit Capability

                    DisconnectCard();
                    return CheckToken303Card(token);
                }

                else
                {
                    DisconnectCard();
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        #endregion  Write Token To 303 Card

        #region  Create New Card For 303
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LogicalName"></param>
        /// <param name="Citycode"></param>
        /// <returns>
        /// -1 : خطا در برقراری ارتباط با کارت 
        /// 0: خطا در ثبت اطلاعات در کارت
        /// 1 : اطلاعات با موفقیت در کارت ثبت شد
        /// 2 : لطفا از کارت غیر اولیه استفاده کنید
        /// 3: لطفا از کارت فرمت شده استفاده کنید
        /// 4: این کارت برای کنتور 303 فرمت نشده است
        /// </returns>
        public int WriteNew303Card(string LogicalName, string Citycode, ref string cardNumber)
        {
            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    cardNumber = ReadRecord(CardDataList.MainData207[0, 1], CardDataList.MainData207[0, 2],
                        CardDataList.MainData207[0, 5], CardDataList.MainData207[0, 3],
                        CardDataList.MainData207[0, 4], "100");


                    string meterNo = ReadRecord("1001", "00", "13", "0", "", "0");
                    if (!meterNo.Equals("167838211"))
                        return 3;

                    meterNo = ReadRecord(CardDataList.MainData303[0, 1], CardDataList.MainData303[0, 2], CardDataList.MainData303[0, 5], CardDataList.MainData303[0, 3], CardDataList.MainData303[0, 4], "100");

                    if (meterNo.Length == 0)
                        return 4;
                    if (!meterNo.Equals("1197643879"))
                        return 3;


                    if (cardNumber.Equals("0"))
                    {
                        return 2;
                    }

                    string card_city_Code = ReadCityCodeOncard();
                    if (card_city_Code != "00000000")
                        if (card_city_Code.Substring(0, 4) != Citycode.Substring(0, 4))
                            return 5;

                    string s = CardDataConverter.StringToASCIIString(LogicalName).ToUpper().PadRight(40, '0');
                    if (WriteRecord("2001", "06", s))
                        if (WriteRecord("1001", "03", Citycode))
                            if (WriteRecord("1001", "02", Citycode))
                            {
                                DisconnectCard();
                                return CheckNew303Card(LogicalName);
                            }
                    DisconnectCard();
                    return 0;
                }

                else
                {
                    DisconnectCard();
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }

        int CheckNew303Card(string str)
        {
            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    string readRecord = ReadRecord("2001", "06", "15", "0", "0", "100");
                    if (readRecord == str)
                    {
                        DisconnectCard();
                        return 1;
                    }
                    else
                    {
                        DisconnectCard();
                        return 0;
                    }
                }

                else
                {
                    DisconnectCard();
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        #endregion  Create New Card For 303

        #region Write Credit To 207 Card
        int CheckCredit207Card(string credit, string endCreditDate, string erControl)
        {
            if (Connect2Card())
            {
                if (Auth2Card())
                {

                    string readRecord = ReadRecord("1003", "00", "0", "0", "4", "100");
                    if (readRecord == System.Convert.ToInt32(credit, 16).ToString())
                    {
                        readRecord = ReadRecord("1003", "01", "0", "0", "4", "100");
                        if (readRecord == System.Convert.ToInt32(endCreditDate, 16).ToString())
                        {
                            readRecord = ReadRecord("1003", "03", "0", "0", "4", "100");
                            if (readRecord == System.Convert.ToInt32(erControl, 16).ToString())
                            {
                                DisconnectCard();
                                return 1;
                            }
                        }
                    }
                }
                DisconnectCard();
                return 0;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credit"></param>
        /// <param name="endCreditDate"></param>
        /// <param name="erControl"></param>
        /// <returns>
        /// -2 : کارت تغییر کرده است
        /// -1 : خطا در برقراری ارتباط با کارت 
        /// 0: خطا در ثبت اطلاعات در کارت
        /// 1 : اطلاعات با موفقیت در کارت ثبت شد
        /// </returns>
        public int WriteCreditTo207Card(string credit, string endCreditDate, string erControl, string MeterNo)
        {
            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    string meterNoOnCard = ReadRecord("1001", "00", "13", "0", "", "0");
                    if (meterNoOnCard != MeterNo)
                    {
                        DisconnectCard();
                        return -2;
                    }

                    WriteRecord("1003", "00", credit);
                    WriteRecord("1003", "01", endCreditDate);
                    WriteRecord("1003", "03", erControl);
                    DisconnectCard();
                    return CheckCredit207Card(credit, endCreditDate, erControl);
                }

                else
                {
                    DisconnectCard();
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        #endregion  Write Credit To 207 Card

        #region  Create New Card For 207
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wellNumber"></param>
        /// <param name="CityCode"></param>
        /// <returns>
        /// -1 : خطا در برقراری ارتباط با کارت 
        /// 0: خطا در ثبت اطلاعات در کارت
        /// 1 : اطلاعات با موفقیت در کارت ثبت شد
        /// 2 : لطفا از کارت غیر اولیه استفاده کنید
        /// 3: لطفا از کارت فرمت شده استفاده کنید
        /// 5: کارت مطعلق به این استان نیست
        /// </returns>        
        public int WriteNew207Card(string wellNumber, string CityCode, ref string cardNumber)
        {
            if (Connect2Card())
            {
                if (Auth2Card())
                {               
                    cardNumber = ReadRecord(CardDataList.MainData207[0, 1], CardDataList.MainData207[0, 2],
                        CardDataList.MainData207[0, 5], CardDataList.MainData207[0, 3],
                        CardDataList.MainData207[0, 4], "100");
                    //if (cardNumber.Equals("0"))
                    //{
                    //    return 2;
                    //}

                    string meterNo = ReadRecord("1001", "00", "13", "0", "", "0");
                    if (!meterNo.Equals("167838211"))
                        return 3;

                    meterNo = ReadRecord(CardDataList.MainData303[0, 1], CardDataList.MainData303[0, 2], CardDataList.MainData303[0, 5], CardDataList.MainData303[0, 3], CardDataList.MainData303[0, 4], "100");

                    if (!meterNo.Equals("1197643879") && meterNo.Length > 0)
                        return 3;

                    string card_city_Code = ReadCityCodeOncard();
                    if (card_city_Code != "00000000")
                        if (card_city_Code.Substring(0, 4) != CityCode.Substring(0, 4))
                            return 5;

                    if (!WriteRecord("1004", "16", Convert.ToInt32(wellNumber).ToString("X8")))
                        return 0;
                    if (!WriteRecord("1001", "02", CityCode))
                        return 0;
                    if (!WriteRecord("1001", "03", CityCode))
                        return 0;

                    if (!WriteRecord("1003", "03", "000000A0"))
                        return 0;
                    DisconnectCard();

                    return CheckNew207Card(wellNumber); ;
                }

                else
                {
                    DisconnectCard();
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }


        int CheckNew207Card(string str)
        {
            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    string readRecord = ReadRecord("1004", "16", "0", "0", "4", "100");
                    if (readRecord == str)
                    {
                        DisconnectCard();
                        return 1;
                    }
                    else
                    {
                        DisconnectCard();
                        return 0;
                    }
                }

                else
                {
                    DisconnectCard();
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        #endregion  Create New Card For 207

        #region Write City Code

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wellNumber"></param>
        /// <param name="CityCode"></param>
        /// <returns>
        /// -1 : خطا در برقراری ارتباط با کارت 
        /// 0: خطا در ثبت اطلاعات در کارت
        /// 1 : اطلاعات با موفقیت در کارت ثبت شد
        /// </returns>
        public int WriteCityCodeOncard(string Citycode)
        {

            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    if (WriteRecord("1001", "02", Citycode.ToString()))
                        if (WriteRecord("1001", "03", Citycode.ToString()))
                        {
                            DisconnectCard();
                            return CheckCityCodeOncard(Citycode);
                        }
                    return 0;
                }

                else
                {
                    DisconnectCard();
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        int CheckCityCodeOncard(string Citycode)
        {

            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    string city = ReadRecord("1001", "03", "", "", "", "100");

                    if (city == Citycode)
                    {
                        DisconnectCard();
                        return 1;
                    }
                    else
                        return 0;
                }

                else
                {
                    DisconnectCard();
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        string ReadCityCodeOncard()
        {

            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    string cityCode = ReadRecord("1001", "03", "", "", "", "100");
                    // DisconnectCard();
                    return cityCode;
                }
            }
            return "";
        }

        #endregion Write City Code

        /// <summary>
        /// 
        /// </summary>
        /// <returns>1:SUCCESS   -1:ERROR </returns>
        public int ClearCard()
        {
            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    if (card.ClearCard(false) == 4106)
                        return 1;
                }

                else
                {
                    DisconnectCard();
                    return -1;
                }
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>1:SUCCESS   -1:ERROR </returns>
        public int FormatCard(string card_number)
        {
            if (Connect2Card())
            {
                if (Auth2Card())
                {
                    if (card.FormatCard(false, card_number) == 4105)
                        return 1;
                }

                else
                {
                    DisconnectCard();
                    return -1;
                }
            }
            return -1;
        }


        public string[] GetConvertedData(string[] data)
        {
            string[] d = { "", "" };
            if (data[0].StartsWith("10"))
            {
                for (int i = 0; i < (CardDataList.card207Data.Length / 8); i++)
                {
                    if (data[0].Equals(CardDataList.card207Data[i, 1]) && data[1].PadLeft(2, '0').Equals(CardDataList.card207Data[i, 2].PadLeft(2, '0')))
                    {
                        d[0] = CardDataConverter.convertCardReadValue(CardDataList.card207Data[i, 5], CardDataList.card207Data[i, 3], CardDataList.card207Data[i, 4], data[2]);
                        d[1] = CardDataList.card207Data[i, 6];
                        return d;
                    }
                }
            }

            return d;
        }

        public bool WriteData2Card(string[] data)
        {
            return WriteRecord(data[0], data[1].PadLeft(2, '0'), data[2]);
        }
    }
}