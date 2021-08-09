using CARD;
/*=======================================================================================
 CardReader.cs
 Main File work with Card Driver DLL use for Read from Card & Write on Card.
 Created by : Ali Amanpour
 Created on : 1387/9/1
 Last Updated by : Javadi.
 Last Updated : 1392/06/25
 DBAccess ver 1.2
	Copyright © 1387, Rahrovan Sepehr Andisheh (Pte. Co.)
	http://www.rsa-electronics.com
=======================================================================================*/
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Card.Classes
{
    internal class CardReaderFunction
    {
        [DllImport("SCardReaderDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int OpenReaderDll();
        [DllImport("SCardReaderDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int CloseReaderDll();
        [DllImport("SCardReaderDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SendCommandToCardDll(string sCmd, StringBuilder sRspns);
        [DllImport("SCardReaderDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int ReconnectReaderDll();
        [DllImport("SCardReaderDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool IsConnectionValidDll();
        [DllImport("SCardReaderDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void EncryptDll(byte[] Key, byte[] Input);
        [DllImport("SCardReaderDll.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DecryptDll(byte[] Key, byte[] Input);

        private const int OPERATION_SUCCESS = 3001;
        private const int INVALID_ARGUMENTS = 3002;
        private const int EMPTY_CARDREADER = 3003;
        private const int BAD_CARD = 3004;
        private const int CARD_ERROR = 3005;
        private const int INVALID_READER = 3006;
        private const int INVALID_CARD = 3007;
        private const int READ_CARD_ERROR = 3008;
        private const int WRITE_CARD_ERROR = 3009;
        private const int NO_CREDIT_FILE = 3010;
        private const int INVALID_CREDIT_FILE = 3011;
        private const int SAVE_REPORT_FAILED = 3012;
        private const int OPERATION_ERROR = 3013;
        private const int SAVE_CODE_FAILED = 3014;
        private const int INVALID_CODE_FILE = 3015;
        private const int SELECT_FILE_FAILED = 3016;
        private const int FILE_DOSNT_EXIST = 3017;
        public const int Format_CARD_ERROR = 3018;
        private const string CREDIT_PARAMETERS_FILE = "2002";
        private const string CREDIT_HISTORY_FILE = "2003";
        private const string REPORTED_VARIABLES_FILE = "2004";
        public const int Format_SUCCESS = 4105;
        private const string METER_IDS_FILE = "2001";

        private const int SCARD_SUCCESS = 4100;
        private const int SCARD_REMOVED = 4101;
        private const int SCARD_BADCARD = 4102;
        private const int SCARD_DISCONNECT = 4103;
        private const int SCARD_ERROR = 4104;

        private const string MCU_IDFile = "FF00";
        private const string ManufactureFile = "FF01";
        private const string PersonalizationFile = "FF02";
        private const string SecurityFile = "FF03";
        private const string UserManagementFile = "FF04";

        private const string MeterIDFile = "1001";
        private const string CreditFile = "1003";
        private const string WellParametersFile = "1004";
        private const string WaterConsumptionFile = "1005";
        private const string ElectricConsumptionFile = "1006";

        private const string STARTSESSION = "8084000008";
        private const string AUTHENTICATION = "8082000010";
        private const string SUBMITCODE = "8020000008";
        private const string SELECTFILE = "80A4000002";
        private const string READRECORD = "80B20000";
        private const string WRITERECORD = "80D20000";
        private const string GETRESPONSE = "80C0000008";

        private const string IC = "41434F5354455354";
        private const string PIN = "F03E9202D69779FC";
        private const string Kc = "7669C2340A85F24E";
        private const string Kt = "DBB41567274FD75C";
        private const string AC1 = "C0EF4B63F8A382FF";

        private bool Encrypt(string[] m_sData, string m_sKey)
        {
            int i;
            byte[] plan = new byte[8];
            byte[] Key = new byte[8];

            for (i = 0; i < 8; i++)
            {
                plan[i] = 0;
                Key[i] = 0;
            }
            if (!CheckInput(m_sData[0]))
                return false;
            if (!CheckInput(m_sKey))
                return false;
            StrToValue(m_sData[0], plan);
            StrToValue(m_sKey, Key);
            EncryptDll(Key, plan);
            m_sData[0] = plan[0].ToString("X2") + plan[1].ToString("X2") + plan[2].ToString("X2") + plan[3].ToString("X2") + plan[4].ToString("X2") + plan[5].ToString("X2") + plan[6].ToString("X2") + plan[7].ToString("X2");
            return true;
        }

        private bool CheckInput(string Input)
        {
            char[] chSample = new char[1];

            for (int i = 0; i < Input.Length; i++)
            {
                chSample = Input.ToCharArray(i, 1);
                if (!((chSample[0] >= '0') && (chSample[0] <= '9')) && !((chSample[0] >= 'a') && (chSample[0] <= 'f')) && !((chSample[0] >= 'A') && (chSample[0] <= 'F')))
                    return false;
            }
            return true;
        }

        private void StrToValue(string Str, byte[] Value)
        {
            while (Str.Length < 16)
                Str = Str.Insert(0, "0");
            for (int i = 0; i < 8; i++)
                Value[i] = System.Convert.ToByte(Str.Substring(i * 2, 2), 16);
        }

        public int SelectFile(string sFileName)
        {
            int iResult;
            string sCommand, sResponse;
            StringBuilder sbRsp = new StringBuilder(256);

            sCommand = SELECTFILE;
            sCommand += sFileName;
            iResult = SendCommandToCardDll(sCommand, sbRsp);
            if (iResult != SCARD_SUCCESS)
                return SELECT_FILE_FAILED;
            sResponse = sbRsp.ToString();
            sResponse = sResponse.Substring(sResponse.Length - 4, 2);
            if (sResponse != "91")
                return SELECT_FILE_FAILED;
            return OPERATION_SUCCESS;
        }

        public int ReadRecord(byte iRecordIndex, out string sValue, string SELECTFILE)
        {
            int iResult;
            string sCommand, sResponse;
            StringBuilder sbRsp = new StringBuilder(256);

            sValue = "";
            sCommand = READRECORD;

            if (SELECTFILE.StartsWith("2"))
            {
                switch (SELECTFILE[3])
                {
                    case '1':
                        sCommand += "14";
                        break;
                    case '2':
                    case '3':
                        sCommand += "28";
                        break;
                    case '4':
                        sCommand += "1E";
                        break;
                    default:
                        sCommand += "04";
                        break;
                }
            }
            else if (SELECTFILE.StartsWith("F"))
            {
                switch (SELECTFILE[3])
                {
                    case '0':
                    case '1':
                    case '3':
                    case '6':
                        sCommand += "08";
                        break;
                    case '7':
                        sCommand += "24";
                        break;
                    case '4':
                        sCommand += "06";
                        break;
                    default:
                        sCommand += "04";
                        break;
                }
            }
            else
                sCommand += "04";

            sCommand = sCommand.Remove(4, 2);
            sCommand = sCommand.Insert(4, iRecordIndex.ToString("X2"));

            iResult = SendCommandToCardDll(sCommand, sbRsp);
            sResponse = sbRsp.ToString();
            sResponse = sResponse.Substring(sResponse.Length - 4, 4);
            if (sResponse != "9000")
                return READ_CARD_ERROR;
            sResponse = sbRsp.ToString();
            sResponse = sResponse.Remove(sResponse.Length - 4, 4);

            if (SELECTFILE.StartsWith("2"))
            {
                switch (SELECTFILE[3])
                {
                    case '1':
                        sValue = sResponse.Substring(0, 40);
                        break;
                    case '2':
                    case '3':
                        sValue = sResponse.Substring(0, 80);
                        break;
                    case '4':
                        sValue = sResponse.Substring(0, 60);
                        break;
                    default:
                        sValue = sResponse.Substring(0, 8);
                        break;
                }
            }
            else if (SELECTFILE.StartsWith("F"))
            {
                switch (SELECTFILE[3])
                {
                    case '0':
                    case '1':
                    case '3':
                    case '6':
                        sValue = sResponse.Substring(0, 16);
                        break;
                    case '7':
                        sValue = sResponse.Substring(0, 72);
                        break;
                    case '4':
                        sValue = sResponse.Substring(0, 12);
                        break;
                    default:
                        sValue = sResponse.Substring(0, 8);
                        break;
                }
            }
            else
                sValue = sResponse.Substring(0, 8);

            return OPERATION_SUCCESS;
        }

        public int WriteRecord(byte iRecordIndex, string sValue, string SELECTFILE)
        {
            int iResult;
            string sCommand, sResponse;
            StringBuilder sbRsp = new StringBuilder(256);

            sCommand = WRITERECORD;

            if (SELECTFILE.StartsWith("2"))
            {
                switch (SELECTFILE[3])
                {
                    case '1':
                        if (sValue.Length != 40)
                        {
                            //System.Windows.MessageBox.Show("Invalid record data (20 bytes)");
                            return WRITE_CARD_ERROR;
                        }
                        sCommand += "14";
                        break;
                    case '2':
                    case '3':
                        if (sValue.Length != 80)
                        {
                            //System.Windows.MessageBox.Show("Invalid record data (40 bytes)");
                            return WRITE_CARD_ERROR;
                        }
                        sCommand += "28";
                        break;
                    case '4':
                        if (sValue.Length != 60)
                        {
                            //System.Windows.MessageBox.Show("Invalid record data (30 bytes)");
                            return WRITE_CARD_ERROR;
                        }
                        sCommand += "1E";
                        break;
                    default:
                        if (sValue.Length != 8)
                        {
                            //System.Windows.MessageBox.Show("Invalid record data (4 bytes)");
                            return WRITE_CARD_ERROR;
                        }
                        sCommand += "04";
                        break;
                }
            }
            else if (SELECTFILE.StartsWith("F"))
            {
                switch (SELECTFILE[3])
                {
                    case '0':
                    case '1':
                    case '3':
                    case '6':
                        if (sValue.Length != 16)
                        {
                            //System.Windows.MessageBox.Show("Invalid record data (8 bytes)");
                            return WRITE_CARD_ERROR;
                        }
                        sCommand += "08";
                        break;
                    case '7':
                        if (sValue.Length != 72)
                        {
                            //System.Windows.MessageBox.Show("Invalid record data (36 bytes)");
                            return WRITE_CARD_ERROR;
                        }
                        sCommand += "24";
                        break;
                    case '4':
                        if (sValue.Length != 12)
                        {
                            //System.Windows.MessageBox.Show("Invalid record data (6 bytes)");
                            return WRITE_CARD_ERROR;
                        }
                        sCommand += "06";
                        break;
                    default:
                        if (sValue.Length != 8)
                        {
                            //System.Windows.MessageBox.Show("Invalid record data (4 bytes)");
                            return WRITE_CARD_ERROR;
                        }
                        sCommand += "04";
                        break;
                }
            }
            else
            {
                if (sValue.Length != 8)
                {
                    //System.Windows.MessageBox.Show("Invalid record data (4 bytes)");
                    return WRITE_CARD_ERROR;
                }
                sCommand += "04";
            }

            sCommand += sValue;
            sCommand = sCommand.Remove(4, 2);
            sCommand = sCommand.Insert(4, iRecordIndex.ToString("X2"));
            iResult = SendCommandToCardDll(sCommand, sbRsp);
            sResponse = sbRsp.ToString();
            if (iResult != SCARD_SUCCESS)
                return WRITE_CARD_ERROR;
            sResponse = sResponse.Substring(sResponse.Length - 4, 4);
            if (sResponse != "9000")
                return WRITE_CARD_ERROR;
            return OPERATION_SUCCESS;
        }

        public int ConnectToCard()
        {
            int iResult = new int();

            if (IsConnectionValidDll())
                return OPERATION_SUCCESS;
            iResult = OpenReaderDll();
            if (iResult != SCARD_SUCCESS)
                return iResult;
            return OPERATION_SUCCESS;
        }

        public void DisconnectFromCard()
        {
            CloseReaderDll();
        }

        public int Authentication()
        {
            int i = new int();
            int iResult = new int();
            byte bData = new byte();
            byte[] bTemp1 = new byte[8];
            byte[] bTemp2 = new byte[8];
            string RNDc, RNDt, Key, sResultKey;
            string sStatus;
            string[] sData = new string[1];
            string sCommand;
            string sResponse;
            Random cRandom = new Random();
            StringBuilder sbResp = new StringBuilder(256);

            //--------------------------------------------- Step1 --------------------------------------------------//
            sCommand = STARTSESSION;
            iResult = SendCommandToCardDll(sCommand, sbResp);
            if (iResult != SCARD_SUCCESS)
                return iResult;
            sResponse = sbResp.ToString();
            sStatus = sResponse.Substring(sResponse.Length - 4, 4);
            if (sStatus != "9000")
                return INVALID_CARD;
            RNDc = sResponse.Substring(0, 16);
            sData[0] = RNDc;
            Key = Kt;
            Encrypt(sData, Key);
            RNDt = "";
            for (i = 0; i < 8; i++)
            {
                bData = (byte)cRandom.Next();
                RNDt += bData.ToString("X2");
            }
            sCommand = AUTHENTICATION;
            sCommand += sData[0];
            sCommand += RNDt;
            iResult = SendCommandToCardDll(sCommand, sbResp);
            if (iResult != SCARD_SUCCESS)
                return iResult;
            sResponse = sbResp.ToString();
            sStatus = sResponse.Substring(sResponse.Length - 4, 4);
            if (sStatus != "6108")
                return INVALID_CARD;
            sCommand = GETRESPONSE;
            iResult = SendCommandToCardDll(sCommand, sbResp);
            if (iResult != SCARD_SUCCESS)
                return iResult;
            sResponse = sbResp.ToString();
            sStatus = sResponse.Substring(sResponse.Length - 4, 4);
            if (sStatus != "9000")
                return INVALID_CARD;
            sResultKey = sResponse.Substring(0, 16);

            //--------------------------------------------- Step2 --------------------------------------------------//
            sData[0] = RNDc;
            Key = Kc;
            Encrypt(sData, Key);
            StrToValue(sData[0], bTemp1);
            StrToValue(RNDt, bTemp2);
            for (i = 0; i < 8; i++)
                bTemp1[i] ^= bTemp2[i];
            sData[0] = bTemp1[0].ToString("X2") + bTemp1[1].ToString("X2") + bTemp1[2].ToString("X2") + bTemp1[3].ToString("X2") + bTemp1[4].ToString("X2") + bTemp1[5].ToString("X2") + bTemp1[6].ToString("X2") + bTemp1[7].ToString("X2");
            Key = Kt;
            Encrypt(sData, Key);
            Key = sData[0];
            sData[0] = RNDt;
            Encrypt(sData, Key);
            if (sData[0] != sResultKey)
                return INVALID_CARD;

            //--------------------------------------------- Step3 --------------------------------------------------//
            sData[0] = AC1;
            Encrypt(sData, Key);
            sCommand = SUBMITCODE;
            sCommand = sCommand.Remove(5, 1);
            sCommand = sCommand.Insert(5, "1");
            sCommand += sData[0];
            iResult = SendCommandToCardDll(sCommand, sbResp);
            if (iResult != SCARD_SUCCESS)
                return iResult;
            sResponse = sbResp.ToString();
            sStatus = sResponse.Substring(sResponse.Length - 4, 4);
            if (sStatus != "9000")
                return INVALID_CARD;
            sData[0] = PIN;
            Encrypt(sData, Key);
            sCommand = SUBMITCODE;
            sCommand = sCommand.Remove(5, 1);
            sCommand = sCommand.Insert(5, "6");
            sCommand += sData[0];
            iResult = SendCommandToCardDll(sCommand, sbResp);
            if (iResult != SCARD_SUCCESS)
                return iResult;
            sResponse = sbResp.ToString();
            sStatus = sResponse.Substring(sResponse.Length - 4, 4);
            if (sStatus != "9000")
                return INVALID_CARD;

            return OPERATION_SUCCESS;
        }

        public int ClearCard(bool bPersonalize)
        {
            int i = 0;

            string sCommand;
            StringBuilder sbRsp = new StringBuilder(256);

            //--------------------------------------------- Step3 --------------------------------------------------//

            //Select Meter ID file of IEWM207
            sCommand = SELECTFILE;
            sCommand += MeterIDFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Meter ID file of IEWM207
            sCommand = WRITERECORD;
            sCommand += "040A010203";


            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            for (i = 1; i < 5; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "0400000000";

                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));


                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }

            //Select Credit file of IEWM207
            sCommand = SELECTFILE;
            sCommand += CreditFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Credit file of IEWM207
            for (i = 0; i < 10; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "0400000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }

            //Select Well Parameters file of IEWM207
            sCommand = SELECTFILE;
            sCommand += WellParametersFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Well Parameters file of IEWM207
            for (i = 0; i < 20; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "0400000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }

            //Select Water Consumption file of IEWM207
            sCommand = SELECTFILE;
            sCommand += WaterConsumptionFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Water Consumption file of IEWM207
            for (i = 0; i < 56; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "0400000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }

            //Select Electric Consumption file of IEWM207
            sCommand = SELECTFILE;
            sCommand += ElectricConsumptionFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            //Clear Electric Consumption file of IEWM207
            for (i = 0; i < 36; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "0400000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }

            //Select Meter IDs file of SEWM303
            sCommand = SELECTFILE;
            sCommand += METER_IDS_FILE;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            //Clear Meter IDs  file of SEWM303
            for (i = 0; i < 10; i++)
            {
                sCommand = WRITERECORD;
                if (i == 0)
                {
                    sCommand += "146798624700000000000000000000000000000000";
                }
                else
                {
                    sCommand += "140000000000000000000000000000000000000000";
                }
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }

            //Select Credit Parameters file of SEWM303
            sCommand = SELECTFILE;
            sCommand += CREDIT_PARAMETERS_FILE;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            //Clear Credit Parameters file of SEWM303
            for (i = 0; i < 30; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "23000000000000000000000000056E0101000000056E0101000000000000000000000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }

            //Select Credit History file of SEWM303
            sCommand = SELECTFILE;
            sCommand += CREDIT_HISTORY_FILE;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            //Clear Credit History file of SEWM303
            for (i = 0; i < 150; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "23000000000000000000000000056E0101000000056E0101000000000000000000000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }

            //Select Reported Variables file of SEWM303
            sCommand = SELECTFILE;
            sCommand += REPORTED_VARIABLES_FILE;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            //Clear Reported Variables file of SEWM303
            for (i = 0; i < 80; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "1A00603C00FF020000000000000000000000000000000000000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }


            if (ConnectToCard() != OPERATION_SUCCESS)
                return Format_CARD_ERROR;



            return Format_SUCCESS; ;
        }

        public int FormatCard(bool bPersonalize,string card_number)
        {
            int i = 0;
            string sCommand;
            StringBuilder sbRsp = new StringBuilder(256);

            //--------------------------------------------- Step1 --------------------------------------------------//
            //Submit the default Issuer Code (IC)
            sCommand = SUBMITCODE;
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '7');
            sCommand += IC;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Select Personalization file
            sCommand = SELECTFILE;
            sCommand += PersonalizationFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            // Define the number of files (9 files for SEWM303)
            sCommand = WRITERECORD;
            sCommand += "0400000900";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '0');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Define the card serial number
            sCommand = WRITERECORD;
            sCommand += "04";
            sCommand += card_number;
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '1');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            // Reset the card
            if (CloseReaderDll() != SCARD_SUCCESS)
                return Format_CARD_ERROR;
            if (ConnectToCard() != OPERATION_SUCCESS)
                return Format_CARD_ERROR;



            //--------------------------------------------- Step2 --------------------------------------------------//
            //Submit the default Issuer Code (IC)
            sCommand = SUBMITCODE;
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '7');
            sCommand += IC;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Select UserManagement file
            sCommand = SELECTFILE;
            sCommand += UserManagementFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            // Write the UserManagement file (Meter ID file block for IEWM 207)
            sCommand = WRITERECORD;
            sCommand += "06040500001001";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '0');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Credit file block for IEWM207)
            sCommand = WRITERECORD;
            sCommand += "06040A00001003";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '1');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Well Parameters file block for IEWM207)
            sCommand = WRITERECORD;
            sCommand += "06041400001004";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '2');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Water Consumption file block for IEWM207)
            sCommand = WRITERECORD;
            sCommand += "06043800001005";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '3');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Electric Consumption file block for IEWM207)
            sCommand = WRITERECORD;
            sCommand += "06042400001006";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '4');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            // Write the UserManagement file (Meter IDs file block for SEWM303)
            sCommand = WRITERECORD;
            sCommand += "06140A00002001";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '5');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Credit Parameters file block for SEWM303)
            sCommand = WRITERECORD;
            sCommand += "06281E00002002";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '6');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Credit History file block for SEWM303)
            sCommand = WRITERECORD;
            sCommand += "06289600002003";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '7');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            // Write the UserManagement file (Reported Values file block for SEWM303)
            sCommand = WRITERECORD;
            sCommand += "061E6400002004";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '8');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            //--------------------------------------------- Step3 --------------------------------------------------//

            //Select Meter ID file of IEWM207
            sCommand = SELECTFILE;
            sCommand += MeterIDFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Meter ID file of IEWM207
            sCommand = WRITERECORD;
            sCommand += "040A010203";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '0');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            for (i = 1; i < 5; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "0400000000";

                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));


                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;
            }

            //Select Credit file of IEWM207
            sCommand = SELECTFILE;
            sCommand += CreditFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Credit file of IEWM207
            for (i = 0; i < 10; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "0400000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }

            //Select Well Parameters file of IEWM207
            sCommand = SELECTFILE;
            sCommand += WellParametersFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Well Parameters file of IEWM207
            for (i = 0; i < 20; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "0400000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;

            }

            //Select Water Consumption file of IEWM207
            sCommand = SELECTFILE;
            sCommand += WaterConsumptionFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Water Consumption file of IEWM207
            for (i = 0; i < 56; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "0400000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;
            }

            //Select Electric Consumption file of IEWM207
            sCommand = SELECTFILE;
            sCommand += ElectricConsumptionFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Electric Consumption file of IEWM207
            for (i = 0; i < 36; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "0400000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;
            }

            //Select Meter IDs file of SEWM303
            sCommand = SELECTFILE;
            sCommand += METER_IDS_FILE;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Meter IDs  file of SEWM303
            for (i = 0; i < 10; i++)
            {
                sCommand = WRITERECORD;
                if (i == 0)
                {
                    sCommand += "146798624700000000000000000000000000000000";
                }
                else
                {
                    sCommand += "140000000000000000000000000000000000000000";
                }
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;
            }

            //Select Credit Parameters file of SEWM303
            sCommand = SELECTFILE;
            sCommand += CREDIT_PARAMETERS_FILE;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Credit Parameters file of SEWM303
            for (i = 0; i < 30; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "23000000000000000000000000056E0101000000056E0101000000000000000000000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;
            }

            //Select Credit History file of SEWM303
            sCommand = SELECTFILE;
            sCommand += CREDIT_HISTORY_FILE;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Credit History file of SEWM303
            for (i = 0; i < 150; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "23000000000000000000000000056E0101000000056E0101000000000000000000000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;
            }

            //Select Reported Variables file of SEWM303
            sCommand = SELECTFILE;
            sCommand += REPORTED_VARIABLES_FILE;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            //Clear Reported Variables file of SEWM303
            for (i = 0; i < 80; i++)
            {
                sCommand = WRITERECORD;
                sCommand += "1A00603C00FF020000000000000000000000000000000000000000";
                sCommand = sCommand.Remove(4, 2);
                sCommand = sCommand.Insert(4, i.ToString("X2"));

                if (!ExecuteCommand(sCommand, sbRsp))
                    return Format_CARD_ERROR;
            }

            //--------------------------------------------- Step4 --------------------------------------------------//
            //Select UserManagement file
            sCommand = SELECTFILE;
            sCommand += UserManagementFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            // Write the UserManagement file (Meter ID file block)
            sCommand = WRITERECORD;
            sCommand += "0404054242";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '0');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Credit file block)
            sCommand = WRITERECORD;
            sCommand += "04040A4242";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '1');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Well Parameters file block)
            sCommand = WRITERECORD;
            sCommand += "0404144242";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '2');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Water Consumption file block)
            sCommand = WRITERECORD;
            sCommand += "0404384242";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '3');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Electric Consumption file block)
            sCommand = WRITERECORD;
            sCommand += "0404244242";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '4');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Meter IDs file block)
            sCommand = WRITERECORD;
            sCommand += "04140A4242";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '5');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Credit Parameters file block)
            sCommand = WRITERECORD;
            sCommand += "04281E4242";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '6');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write the UserManagement file (Credit History file block)
            sCommand = WRITERECORD;
            sCommand += "0428964242";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '7');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            // Write the UserManagement file (Reported Variables file block)
            sCommand = WRITERECORD;
            sCommand += "041E644242";
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '8');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            //--------------------------------------------- Step5 --------------------------------------------------//
            //Select Security file
            sCommand = SELECTFILE;
            sCommand += SecurityFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            // Write PIN code
            sCommand = WRITERECORD;
            sCommand += "08";
            sCommand += PIN;
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '1');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            // Write Kc code
            sCommand = WRITERECORD;
            sCommand += "08";
            sCommand += Kc;
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '2');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write Kt code
            sCommand = WRITERECORD;
            sCommand += "08";
            sCommand += Kt;
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '3');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Write AC1 code
            sCommand = WRITERECORD;
            sCommand += "08";
            sCommand += AC1;
            sCommand = GeneralFunctions.ReplaceAt(sCommand, 5, '5');

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            //--------------------------------------------- Step6 --------------------------------------------------//
            //Select Personalization file
            sCommand = SELECTFILE;
            sCommand += PersonalizationFile;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;

            // Define the security attributes and number of files (9 files)
            sCommand = WRITERECORD;
            //	if(bPersonalize)
            sCommand += "04007E0981";
            //	else
            //		sCommand +=   "04007E0981" ;

            if (!ExecuteCommand(sCommand, sbRsp))
                return Format_CARD_ERROR;


            // Reset the card
            if (CloseReaderDll() != SCARD_SUCCESS)
                return Format_CARD_ERROR;
            if (ConnectToCard() != OPERATION_SUCCESS)
                return Format_CARD_ERROR;
            return Format_SUCCESS; ;
        }

        private bool ExecuteCommand(string sCommand, StringBuilder sbRsp)
        {
            int iResult = SendCommandToCardDll(sCommand, sbRsp);

            if (iResult != SCARD_SUCCESS)
            {

                return false;
            }

            string sResponse = sbRsp.ToString();
            sResponse = sResponse.Substring(sResponse.Length - 4, 4);

            if (sCommand.Contains(SELECTFILE.ToString()) && sCommand.Contains(MeterIDFile.ToString()))
            {
                if (sResponse != "9100")
                {

                    return false;
                }

            }

            else if (sCommand.Contains(SELECTFILE.ToString()) && sCommand.Contains(CreditFile.ToString()))
            {
                if (sResponse != "9101")
                {

                    return false;
                }
            }

            else if (sCommand.Contains(SELECTFILE.ToString()) && sCommand.Contains(WellParametersFile.ToString()))
            {
                if (sResponse != "9102")
                {

                    return false;
                }
            }

            else if (sCommand.Contains(SELECTFILE.ToString()) && sCommand.Contains(WaterConsumptionFile.ToString()))
            {
                if (sResponse != "9103")
                {

                    return false;
                }
            }
            else if (sCommand.Contains(SELECTFILE.ToString()) && sCommand.Contains(ElectricConsumptionFile.ToString()))
            {
                if (sResponse != "9104")
                {

                    return false;
                }
            }
            else if (sCommand.Contains(SELECTFILE.ToString()) && sCommand.Contains(METER_IDS_FILE.ToString()))
            {
                if (sResponse != "9105")
                {

                    return false;
                }
            }
            else if (sCommand.Contains(SELECTFILE.ToString()) && sCommand.Contains(CREDIT_PARAMETERS_FILE.ToString()))
            {
                if (sResponse != "9106")
                {

                    return false;
                }
            }
            else if (sCommand.Contains(SELECTFILE.ToString()) && sCommand.Contains(CREDIT_HISTORY_FILE.ToString()))
            {
                if (sResponse != "9107")
                {

                    return false;
                }
            }
            else if (sCommand.Contains(SELECTFILE.ToString()) && sCommand.Contains(REPORTED_VARIABLES_FILE.ToString()))
            {
                if (sResponse != "9108")
                {

                    return false;
                }
            }

            else if (sResponse != "9000")
            {

                return false;
            }
            return true;
        } 
    }
}
