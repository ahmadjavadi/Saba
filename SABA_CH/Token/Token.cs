using System;
using System.Data.Objects;
using System.Globalization;
using System.Windows.Forms;
using SABA_CH.DataBase;
using SABA_CH.Global;
using MessageBox = System.Windows.MessageBox;

namespace SABA_CH.Token
{
    public delegate void tokenDataReceived(object sender, TokenEventArgs arg);
   
   
    public class TokenEventArgs : EventArgs
    {
        public string TokenData { get; private set; }

        public TokenEventArgs(string data)
        {
            TokenData = data;
        }
    }

    class Token
    {

        int CreateTokenCounter = 0;
        string Token1 = "", Token2 = "" , Token3="", Token4="" , TotalToken="";
        string UsbToken1 = "", UsbToken2 = "", UsbToken3 = "", UsbToken4 = "", UsbToken = "";
        //ShowTokenMessage oShowMessage = new ShowTokenMessage();
        string[] messages = {
                                 @"<VDCode=""TG""/>",
                                 @"<DanglePass=""DanglePassParam""/>",                          
                                 @"<SerialNumber=""SerialNumberParam""/>",
                                 @"<HashGen=""HashParam""/>",
                                 @"<TID=""IDParam""/>",
                                 @"<StartTime=""startTimeParam""/>",
                                 @"<EndTime=""endTimeParam""/>",
                                 @"<StartDate=""startDateParam""/>",             
                                 @"<EndDate=""enddDateParam""/>",
                                 @"<CreditValue=""CreditValueParam""/>",
                                 @"<CreditTMode=""CreditModeParam""/>",
                                 @"<GenToken=""""/>"
                             };

        public static string DangleToken = "";
        public static string AnsariToken = "";

        string TokenID = "0";
        string SerialNumber = "", CreditValue = "", CreditTransferModes = "", c_v, c_v2;
      
        DateTime startDateTime, endDateTime;
        public event tokenDataReceived getToken;
        SerialPortInterface _workingObject = new SerialPortInterface();

        //public Token(string SerialNumber, string CreditValue, DateTime startDate, DateTime enddDate, string startTime, string endTime, string CreditTransferModes)
        public Token(string SerialNumber, string CreditValue, DateTime startDate, DateTime endDate, string CreditTransferModes)
        {

            CreateTokenCounter = 0;
            Token1 = ""; 
            Token2 = "";
            UsbToken1 = "";
            UsbToken2 = "";

            DangleToken = "";
            AnsariToken = "";

            CommonData.SendMessageToserialPort = true;
            CommonData.CreditTransferModes = CreditTransferModes;
            CommonData.isTokenRequestk = true;
            CommonData.isHVRequest = false;
         
            this.SerialNumber = SerialNumber;
            this.CreditValue = CreditValue;
            this.CreditTransferModes = CreditTransferModes;

            this.startDateTime = startDate;
            this.endDateTime = endDate;


            uint uintCreditValue = Convert.ToUInt32(CreditValue) * 1000;
            c_v = uintCreditValue.ToString("X8");
            c_v2 = c_v.Substring(6, 2) + c_v.Substring(4, 2) + c_v.Substring(2, 2) + c_v.Substring(0, 2);


            CommonData.CurrentYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            CommonData.DayofYear = Convert.ToInt32(DateTime.Now.DayOfYear.ToString());
            CommonData.MinuteOfDay = Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMinutes);

            CommonData.mainwindow.changeProgressBarValue(1);
            string SendStr = Convert.ToString(CommonData.CurrentYear, 2).PadLeft(12, '0') + Convert.ToString(CommonData.DayofYear, 2).PadLeft(9, '0') + Convert.ToString(CommonData.MinuteOfDay, 2).PadLeft(12, '0') + Convert.ToString(CommonData.UserID, 2).PadLeft(4, '0') + Convert.ToString(CommonData.SystemID, 2).PadLeft(3, '0') + Convert.ToString(CommonData.SequenceNumber, 2).PadLeft(8, '0');
            TokenID = Convert.ToUInt64(SendStr, 2).ToString("X");

            messages[2] = messages[2].Replace("SerialNumberParam", SerialNumber.ToUpper());
            messages[3] = messages[3].Replace("HashParam", CommonData.DefultHashNumber.ToUpper());
            messages[4] = messages[4].Replace("IDParam", TokenID.ToUpper());

            messages[5] = messages[5].Replace("startTimeParam", this.startDateTime.ToString("HH:mm:ss"));
            messages[6] = messages[6].Replace("endTimeParam", this.endDateTime.ToString("HH:mm:ss"));

            messages[7] = messages[7].Replace("startDateParam", this.startDateTime.ToString("yyyy/MM/dd"));
            messages[8] = messages[8].Replace("enddDateParam", this.endDateTime.ToString("yyyy/MM/dd"));

            messages[9] = messages[9].Replace("CreditValueParam", c_v.ToUpper());
            messages[10] = messages[10].Replace("CreditModeParam", CreditTransferModes.ToUpper()); 

            _workingObject.DataReceived += new dataReceived(_workingObject_DataReceived);

            if (!_workingObject.Open())
            {
                CommonData.mainwindow.changeProgressBarTag("سخت افزار تایید اعتبار را به کامپیوتر متصل کرده ، مجددا نرم افزار را اجرا کنید");
                CommonData.mainwindow.changeProgressBarValue(0);
            }
            else
            {
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(10);
                _workingObject.Send(messages);
            }
        }



        void _workingObject_DataReceived(object sender, SerialPortEventArgs arg)
        {
           // CommonData.show303Data.ChangeGridEnable(true);
            if (arg.FullID == "-1" || arg.FullID == "-3")
            {
                CommonData.mainwindow.changeProgressBarTag("  لطفا سخت افزار تایید اعتبار را از کامپیوتر جدا کرده و را مجددا وصل کنید");
               // CommonData.show303Data.ChangeButtonVisibility(CommonData.show303Data.btnSendCredit2Card, Visibility.Visible);
                return;
            }
            

            if (CommonData.isTokenRequestk)
            {
                if (arg.ReceivedData.Length > 10)
                {
                    TokenGenerator(arg.ReceivedData, arg.FullID);
                }
            }
        }
       
        void TokenGenerator(string usbToken,string FullID )
        {
            CommonData.IsGenerateFirstToken = true;

           
            string finalToken = "";
            ulong ulongToken=0;
            if (usbToken.Length > 82)
                ulongToken = ulong.Parse(usbToken.Substring(82));
            else
            {
                CommonData.mainwindow.changeProgressBarValue(1000);
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message97);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message97);
           
                return;
            }
            ulong fid = ulong.Parse(FullID,NumberStyles.HexNumber);

            
            string usbTokenHex = ulongToken.ToString("X16");
            string ComputerCode = "", USBSignature = "";
            decimal? Hash = 0,  token = 1000000;

            ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
            ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
            ObjectParameter TokenID = new ObjectParameter("TokenID", 10000000000000000);

            int? SequenceNumber = 0;
            //UInt64 uintTokenID = Convert.ToUInt64(TokenID);

            //*********************************************************************************************  Convert Credit Value
            //uint uintCreditValue = Convert.ToUInt32(this.CreditValue);
            //string  c_v = uintCreditValue.ToString("X8");
            ////SequenceNumber = SaveINDB.showMaxSequenceNumber(CommonData.MeterID, out Result, out ErrMSG);

            string starttime = this.startDateTime.Hour.ToString("X2") + this.startDateTime.Minute.ToString("X2") + this.startDateTime.Second.ToString("X2");
            string endtime = this.endDateTime.Hour.ToString("X2") + this.endDateTime.Minute.ToString("X2") + this.endDateTime.Second.ToString("X2");

            if (CreditTransferModes.Length < 2)
                CreditTransferModes = "0" + CreditTransferModes;
            finalToken = fid.ToString("X16") + c_v +
                this.startDateTime.Year.ToString("X4") + this.startDateTime.Month.ToString("X2") + this.startDateTime.Day.ToString("X2") + starttime +
                this.endDateTime.Year.ToString("X4") + this.endDateTime.Month.ToString("X2") + this.endDateTime.Day.ToString("X2") + endtime +
                this.CreditTransferModes + usbTokenHex + "0000000000";

            if (CreateTokenCounter == 0)
            {
                Token1 = finalToken;
                CreateTokenCounter++;
                _workingObject.Send(messages);
                return;
            }

            else if (CreateTokenCounter == 1)
            {
                if (Token1 != finalToken)
                {
                    CommonData.mainwindow.changeProgressBarValue(0);
                    Token2 = finalToken;
                    UsbToken1 = usbToken;
                    //System.Windows.Forms.MessageBox.Show("Token1 != Token2 \r\n Token1: \r\n" + Token1 + "\r\n Token2: \r\n" + finalToken+ "\r\n usbToken1: " + UsbToken1);
                    TotalToken += "usbToken1: "+ UsbToken1+ "\r\nToken1 : "+ Token1 + "\r\n finalToken:" + finalToken +"\r\n";
                    CreateTokenCounter++;
                    _workingObject.Send(messages);                   
                    return;
                }
            }
            else if (CreateTokenCounter == 2)
            {
                if (Token2 != finalToken && Token1 !=finalToken )
                {
                    UsbToken2 = usbToken;
                    CommonData.mainwindow.changeProgressBarValue(0);
                    Token3 = finalToken;
                   // System.Windows.Forms.MessageBox.Show("Token1 != Token2 \r\n Token1: \r\n" + Token1 + "\r\n Token2: \r\n" + Token2 + "\r\n Token3: \r\n" + finalToken + "\r\n usbToken1: " + UsbToken1 + "usbToken2: " + UsbToken2);
                    TotalToken += "usbToken1: " + UsbToken1 + "usbToken2: " + UsbToken2 +  "\r\n finalToken:" + finalToken + "\r\n";
                    //TotalToken += finalToken + "\r\n";       
                    CommonData.WriteLOG(new Exception(TotalToken));
                    CreateTokenCounter++;
                    _workingObject.Send(messages);
                    return;
                }
            }
            else if (CreateTokenCounter == 3)
            {
                if (Token3 != finalToken && Token2 != finalToken && Token1 != finalToken)
                {
                    CommonData.mainwindow.changeProgressBarValue(0);
                    Token4 = finalToken;
                    UsbToken3 = usbToken;
                   // System.Windows.Forms.MessageBox.Show("Token1 != Token2 \r\n Token1: \r\n" + Token1 + "\r\n Token2: \r\n" + Token2 + "\r\n Token3: \r\n"+Token3+ "usbToken1: " + UsbToken1 + "usbToken2: " + UsbToken2 + "usbToken3: " + UsbToken3);
                    TotalToken += "usbToken1: " + UsbToken1 + "usbToken2: " + UsbToken2 + "usbToken3: " + UsbToken3 + "\r\n finalToken:" + finalToken + "\r\n";
                    
                  CommonData.WriteLOG( new Exception( TotalToken));
                    CreateTokenCounter++;
                    _workingObject.Send(messages);
                    return;
                }
            }
            else
            {
                if (Token4 != finalToken && Token3 != finalToken && Token2 != finalToken && Token1 != finalToken)
                {
                    CommonData.mainwindow.changeProgressBarValue(0);
                    UsbToken4 = usbToken;
                    System.Windows.Forms.MessageBox.Show("Token1: \r\n" + Token1 + "\r\n Token2: \r\n" + Token2 + "\r\n Token3: \r\n"+Token3+ "\r\n Token4: \r\n" + Token4+ "\r\n Final Tolen: " + finalToken, "Dongle Create Different Token"+ "usbToken1: " + UsbToken1 + "usbToken2: " + UsbToken2 + "usbToken3: " + UsbToken3+ "usbToken4: " + UsbToken4);
                   
                    TotalToken +=  "usbToken: " + usbToken + "\r\n finalToken:" + finalToken + "\r\n";
                    CommonData.WriteLOG(new Exception(TotalToken));
                  
                    return;
                }
            }
           
           
            Int64 credit = Int64.Parse(c_v, NumberStyles.HexNumber);            
            
           
            if (!( Token4 == finalToken || Token3== finalToken || Token2 == finalToken || Token1 == finalToken))
            {
                
                if (System.Windows.Forms.MessageBox.Show("Dongle\r\n" + TotalToken +"\r\n"+"آیا مایل به تولید مجدد توکن هستید", "Token Error", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                   
                    TotalToken += "usbToken: " + usbToken + "\r\n finalToken:"+ finalToken ;
                    CommonData.WriteLOG(new Exception(TotalToken));
                    Token1 =Token2 =Token3=Token4 =TotalToken= "";
                    
                    _workingObject.Send(messages);
                    return;
                }
            }
        
            string BuildDate = "";
            //if(CommonData.LanguagesID==2)
            //    BuildDate=DateTime.Now.ToPersianString();
            //else
                BuildDate=DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            ObjectParameter CardID = new ObjectParameter("CardID", 1000000000000);
            SQLSPS.INSCards(CommonData.CardNumber, CardID, Result, ErrMSG);
            SQLSPS.InsToken(FullID, CommonData.UserID, ComputerCode, SequenceNumber, ulongToken.ToString(), BuildDate,
                Convert.ToDecimal(CardID.Value), Hash, CommonData.CardMeterID, this.CreditTransferModes, this.startDateTime.ToMiladiString(), 
                this.endDateTime.ToMiladiString(), Convert.ToDecimal(CreditValue), USBSignature, CommonData.OBISValueHeaderID, CommonData.creditCapabilityActivation,
                CommonData.creditStartDate, CommonData.disconnectivityNegativeCredit, CommonData.disconnectivityExpiredCredit, TokenID, Result, ErrMSG);

            if(CommonData.SoftwareVersion!=null && Convert.ToInt16(CommonData.SoftwareVersion[CommonData.SoftwareVersion.Length-1])>3)
              SQLSPS.InsCredit303(CommonData.CardMeterID, CommonData.creditCapabilityActivation, CommonData.creditStartDate, CommonData.disconnectivityNegativeCredit, 
                CommonData.disconnectivityExpiredCredit, CommonData.OBISValueHeaderID, Result, ErrMSG);
            TokenEventArgs args = new TokenEventArgs(finalToken);
            this.getToken(this, args);
        }

     

    }
}