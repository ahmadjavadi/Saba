
using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using RsaDateTime;
using SABA_CH.DataBase;
using SABA_CH.Global;
using SABA_CH.Token;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using MessageBoxOptions = System.Windows.MessageBoxOptions;
using TextBox = System.Windows.Controls.TextBox;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for ActiveSoftwareWindow.xaml
    /// </summary>
    public partial class ActiveSoftwareWindow : System.Windows.Window
    {
        public static ActiveSoftwareWindow Instance { get { return _instance == null ? new ActiveSoftwareWindow() : _instance; } }
        private  static ActiveSoftwareWindow _instance;


         DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public string DangelPass = "", DangelSerial = "", ExpireDate = "";
        public bool DangelOk = true;
        public int ResetCount = 0;
        public bool LsbRequest = false;
        public bool Closewithbutton = false;
        public bool GetActivationCode = false;
        public bool Completerequest = false;

        SerialPortInterface _serialPortDataReceivedObject;


        public ActiveSoftwareWindow()
        {
            try
            {
                _instance = this;        
                _dispatcherTimer.Interval = TimeSpan.FromSeconds(80);
                _dispatcherTimer.Start();
                InitializeComponent();
                textbox1.Focus();
                CommonData.completeRequest = false;
                LSBdangleCheck();
                //while (!CommonData.completeRequest) ;



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        public void CheckDangle()
        {
            
        }
        public void Start() {

            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
                   delegate ()
                   {                      
                       Instance.Show();
                   }));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void AfterDangelChek()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
              delegate ()
              {                                
                  try
            {
                stopTimer();
                _dispatcherTimer.IsEnabled = false;
                ShowDangelInfo showDangelInfo = new ShowDangelInfo("");
                bool isExists = false;



                for (int i = showDangelInfo._lstShowDangelInfo.Count-1; i >-1; i--)
                {
                    var item = showDangelInfo._lstShowDangelInfo[i];

                    int intProvinceCode = (NumericConverter.IntConverter(item.LSBCode.Substring(12, 4), NumberStyles.HexNumber) / 1000) * 1000;
                    CommonData.ProvinceCode = intProvinceCode.ToString("X4");
                    if (CommonData.DangleLSB == item.LSBCode && !CommonData.IsExpire)
                    {
                        CommonData.DangleSerial = ReverseStr(BytesToHexString(CryptorEngine.Decrypt(GetBytesFromString(item.DangelSerial), true)));
                        CommonData.ExpireDate = ReverseStr(BytesToHexString(CryptorEngine.Decrypt(GetBytesFromString(item.ExpireDate), true)));
                        if (item.ExpireDate != "")
                            CommonData.ExpireDate = NumericConverter.IntConverter(CommonData.ExpireDate, NumberStyles.HexNumber).ToString();
                        else
                            CommonData.ExpireDate = "990101";
                        if (CommonData.ExpireDate.Length == 5)
                            CommonData.ExpireDate = "0" + CommonData.ExpireDate;
                        CommonData.DangleSerial = CommonData.DangleSerial.Substring(CommonData.DangleSerial.Length - 8, 8);
                        CommonData.DanglePass = ReverseStr(BytesToHexString(CryptorEngine.Decrypt(GetBytesFromString(item.DangelPass), true)));
                        CommonData.DanglePass = CommonData.DanglePass.Substring(CommonData.DanglePass.Length - 8, 8);

                        CommonData.DangleLSB = item.LSBCode;



                        intProvinceCode = (NumericConverter.IntConverter(CommonData.DangleLSB.Substring(12, 4), NumberStyles.HexNumber) / 1000) * 1000;
                        string strProvinceCode = intProvinceCode.ToString("X4");
                        string CityCode = CommonData.DangleLSB.Substring(4, 2);
                        CommonData.Citycode = strProvinceCode + CityCode.ToString().PadLeft(4, '0');

                        if (CityCode.Equals("00000000"))
                        {
                            MessageBox.Show("لطفا دانگل را برای اصلاح به شرکت رهروان سپهر اندیشه ارسال فرمائید", "خطا در دانگل", MessageBoxButton.OK, MessageBoxImage.Information);
                            Application.Current.Shutdown();
                        }
                        if (!CommonData.mainwindow.TrialCheckedIsValid())
                        {
                            this.Close();
                            Environment.Exit(1);
                        }
                        CommonData.getDanglePassFromDB = true;
                        CommonData.mainwindow.changeMainPageGrid(true);
                        CommonData.mainwindow.changeProgressBarValue(0);

                        isExists = true;
                        break;
                    }

                }

                if (!isExists && CommonData.DangleSerial != "" && CommonData.DanglePass != "")
                {
                    dangleCheck(true);
                }
                
                if ((CommonData.DangleSerial == "" && CommonData.DanglePass == "") || CommonData.ExpireDate == "" )
                {
                    CommonData.getDanglePassFromDB = false;
                    CommonData.getExpireDateFromDB = false;
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(
                       delegate ()
                       {
                           CommonData.mainwindow.changeMainPageGrid(false);
                           ShowDialog();
                       }));

                }
                else
                    CommonData.getExpireDateFromDB = true;
            }
            catch (SqlException ex)
            {
                //MessageBox.Show(ex.ToString().ToString()); 
                      CommonData.WriteLOG(ex);
            }
              }));

        }

     
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!CommonData.IsExpire)
                Application.Current.Shutdown();
            else
                this.Close();

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Hidden;
                _dispatcherTimer.Stop();
                string clearText = textbox1.Text.Trim();// + textbox2.Text.Trim() + textbox3.Text.Trim() + textbox4.Text.Trim() + textbox5.Text.Trim() + textbox6.Text.Trim() + textbox7.Text.Trim() + textbox8.Text.Trim();
                clearText = clearText.Replace("-", "");
                clearText = clearText.Replace(" ", "");
                string decryptedText = BytesToHexString(CryptorEngine.Decrypt(GetBytesFromString(clearText), true));

                decryptedText = ReverseStr(decryptedText);
                DangelSerial = decryptedText.Substring(0, 8);
                DangelPass = decryptedText.Substring(10, 6);
                CommonData.DangleSerial = DangelSerial;
                CommonData.DanglePass = DangelPass;
                CommonData.ExpireDate = decryptedText.Substring(16, 16);
                var a = UInt64.Parse(CommonData.ExpireDate, NumberStyles.HexNumber);
                int len = a.ToString().Length;
                string b = a.ToString().Substring(len - 7, 7);
                CommonData.ExpireDate = "00" + b.Substring(0, 6);


                if (!CheckExpiredDate())
                {
                    if (System.Windows.Forms.MessageBox.Show("تاریخ اعتبار کد معتبر نیست") == System.Windows.Forms.DialogResult.OK)
                    {
                    }
                    this.Visibility = Visibility.Visible;
                    textbox1.Focus();
                   
                    textbox1.Text = "";
                    btnOK.IsEnabled = false;
                    return;
                }

                

                CommonData.EncryptExpireDate = Convert.ToInt32(CommonData.ExpireDate).ToString("X16");
                CommonData.EncryptExpireDate = GetStringFromBytes(CryptorEngine.Encrypt(HexStringToBytes(ReverseStr(CommonData.EncryptExpireDate)), true));
                CommonData.getDanglePassFromDB = false;
                _dispatcherTimer.Interval = new TimeSpan(0, 1, 20);
                _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                _dispatcherTimer.Start();
                CommonData.SendMessageToserialPort = true;
                CommonData.completeRequest = false;

                dangleCheck(false);
                while (!CommonData.completeRequest) ;
                Closewithbutton = true;
                this.Close();
                // Province code 
                ShowDangelInfo showDangelInfo = new ShowDangelInfo("");
                int provinceCode = (NumericConverter.IntConverter(CommonData.DangleLSB.Substring(12, 4), NumberStyles.HexNumber) / 1000) * 1000;
                CommonData.ProvinceCode = provinceCode.ToString("X4");
            }
            catch
            {
                ResetCount++;
                if (ResetCount >= 3)
                {
                    MessageBox.Show("کد وارد شده اشتباه می باشد", "خطا");
                    Environment.Exit(1);
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    textbox1.Focus();

                    textbox1.Text = "";
                    btnOK.IsEnabled = false;
                    return;

                }
            }
        }

        private bool CheckExpiredDate()
        {
            try
            {

                string temp = DateTime.Now.ToPersianString().Substring(0, 2);
                if (CommonData.ExpireDate.StartsWith("00") && (CommonData.ExpireDate.Length == 8))
                {
                    CommonData.ExpireDate = CommonData.ExpireDate.Substring(2, CommonData.ExpireDate.Length - 2);
                }
                if (CommonData.ExpireDate.Length >= 6)
                {
                    int year = Convert.ToInt32(temp) * 100 + Convert.ToInt32(CommonData.ExpireDate.Substring(0, 2));
                    if (Convert.ToInt32(CommonData.ExpireDate.Substring(0, 2))>90)
                        year = 1300 + Convert.ToInt32(CommonData.ExpireDate.Substring(0, 2));
                    if (year < 1390)
                        year = Convert.ToInt32(14) * 100 + Convert.ToInt32(CommonData.ExpireDate.Substring(0, 2));
                    int month = Convert.ToInt32(CommonData.ExpireDate.Substring(2, 2));
                    int day = Convert.ToInt32(CommonData.ExpireDate.Substring(4, 2));
                    var expirationDate = PersianDateTime.ConvertToGeorgianDateTime(year, month, day, 0, 0, 0);
                    if (DateTime.Now > expirationDate)
                    {
                        return false;
                    }
                }
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            stopTimer();
            _dispatcherTimer.IsEnabled = false;

        }
        private void newdispatcherTimer_Tick(object sender, EventArgs e)
        {
            //stopTimer();
            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message39);
            Environment.Exit(1);
        }
        public void stopTimer()
        {
            _dispatcherTimer.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(
                    delegate ()
                    {
                        _dispatcherTimer.Stop();
                        _dispatcherTimer.IsEnabled = false;
                    }));
        }
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (38 < (sender as TextBox).Text.Length)
                {
                    bool textok = true;
                  
                    if (textok)
                    {
                        btnOK.IsEnabled = true;
                        btnOK.Focus();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void ActiveMainPageGrid(object sender, bool arg)
        {
            CommonData.mainwindow.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
             delegate ()
             {
                 if (arg)
                 {
                     try
                     {
                         _dispatcherTimer.Stop();
                         _dispatcherTimer.IsEnabled = false;
                         CommonData.mainwindow.changeMainPageGrid(arg);
                         //CommonData.mainwindow.SelectICard();
                         CommonData.mainwindow.changeProgressBarValue(0);
                         CommonData.mainwindow.changeProgressBarTag("");
                     }
                     catch (Exception ex)
                     {
                         CommonData.mainwindow.changeProgressBarTag(ex.ToString());
                     }
                 }
                
             }));
        }
        public void dangleCheck(bool isHVRequest)
        {
            try
            {
                CommonData.StopData = false;
                IsConnectToDangle();
                if (_serialPortDataReceivedObject == null)
                    _serialPortDataReceivedObject = new SerialPortInterface();
                if (!_serialPortDataReceivedObject.Open())
                {
                    //سخت افزار تایید اعتبار را به کامپیوتر متصل کرده ، مجددا نرم افزار را اجرا کنید
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message36);
                    CommonData.mainwindow.changeProgressBarValue(0);
                }
                else
                {
                    _serialPortDataReceivedObject.DataReceived += new dataReceived(__serialPort_DataReceived);
                    if (isHVRequest)
                    {
                        CommonData.SendMessageToserialPort = true;
                        CommonData.isHVRequest = true;
                        CommonData.isTokenRequestk = false;
                        string[] messages = {
                                 @"<VDCode=""HV""/>",
                                 @"<DanglePass=""DanglePassParam""/>",
                                 @"<HVCode=""12345678""/>"
                             };
                        _serialPortDataReceivedObject.Send(messages);
                    }
                    else
                    {
                        CommonData.isHVRequest = false;
                        CommonData.isTokenRequestk = false;
                        _serialPortDataReceivedObject.getDangleInfo();
                    }
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(10);

                }

                _dispatcherTimer.IsEnabled = true;
                _dispatcherTimer.Start();
            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }
        public void LSBdangleCheck()
        {
            try
            {
                _dispatcherTimer.Interval = new TimeSpan(0, 1, 20);
                _dispatcherTimer.Tick += new EventHandler(newdispatcherTimer_Tick);
                CommonData.StopData = false;
                CommonData.SendMessageToserialPort = true;
                LsbRequest = true;
                IsConnectToDangle();

                if (_serialPortDataReceivedObject == null)
                    _serialPortDataReceivedObject = new SerialPortInterface();
                if (!_serialPortDataReceivedObject.Open())
                {
                    //سخت افزار تایید اعتبار را به کامپیوتر متصل کرده ، مجددا نرم افزار را اجرا کنید

                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message36);
                    CommonData.mainwindow.changeProgressBarValue(0);

                }
                else
                {
                    _serialPortDataReceivedObject.DataReceived += new dataReceived(__serialPort_DataReceived_LSB);
                    CommonData.isHVRequest = false;
                    CommonData.isTokenRequestk = false;
                    _serialPortDataReceivedObject.getDangleInfo();
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(10);

                }
                //CommonData.completeRequest = true;
                _dispatcherTimer.IsEnabled = true;
                _dispatcherTimer.Start();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);

            }
        }
        private void IsConnectToDangle()
        {
            CommonData.PortName = "";

            try
            {
                ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"] != null)
                        if (queryObj["Caption"].ToString() != null)
                            if (queryObj["Caption"].ToString().Contains("STMicroelectronics Virtual COM"))
                            {
                                string s = queryObj["Caption"].ToString();
                                s = s.Replace("STMicroelectronics Virtual COM Port (", "");
                                CommonData.PortName = s.Replace(")", "");
                                break;
                            }
                }
            }
            catch (ManagementException e)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message36);
                if (System.Windows.Forms.MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message36, "", MessageBoxButtons.OK) == System.Windows.Forms.DialogResult.OK)
                {
                    DangelOk = false;
                    Environment.Exit(1);
                }
                DangelOk = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
            if (CommonData.PortName == "")
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message36);
                if (System.Windows.Forms.MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message36, "", MessageBoxButtons.OK) == System.Windows.Forms.DialogResult.OK)
                {
                    DangelOk = false;
                    Environment.Exit(1);
                }
                DangelOk = false;
            }
        }
        private void __serialPort_DataReceived_LSB(object sender, SerialPortEventArgs arg)
        {
            try
            {
                if (arg.FullID == "-1")//|| arg.FullID == "-3")
                {
                    //" لطفا سخت افزار تایید اعتبار را از کامپیوتر جدا کرده مجددا وصل کنید "
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message36);
                    this._dispatcherTimer.IsEnabled = false;
                    this._dispatcherTimer.Stop();
                    return;
                }
                if (arg.FullID == "-2" || arg.ReceivedData == "No Access")
                {
                    this._dispatcherTimer.IsEnabled = false;
                    this._dispatcherTimer.Stop();
                    string message = CommonData.mainwindow.tm.TranslateofMessage.Message37;
                    message += CommonData.mainwindow.tm.TranslateofMessage.Message36;
                    message += CommonData.mainwindow.tm.TranslateofMessage.Message38;

                    CommonData.mainwindow.changeProgressBarTag(message);
                    CommonData.mainwindow.changeProgressBarValue(1000);

                }
                if (arg.FullID == "" && arg.ReceivedData.Length == 16 && LsbRequest)
                {
                    CommonData.DangleLSB = arg.ReceivedData;
                    LsbRequest = false;
                    CommonData.completeRequest = true;
                    if (CommonData.completeRequest )
                    {
                        AfterDangelChek();
                    }
                    this._dispatcherTimer.IsEnabled = false;
                    this._dispatcherTimer.Stop();
                    return;

                }
                CommonData.completeRequest = true;
            }
            catch (Exception ex)
            {
                 MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);

            }
        }
        private void __serialPort_DataReceived(object sender, SerialPortEventArgs arg)
        {
            try
            {
                if (arg.FullID == "-1")//|| arg.FullID == "-3")
                {
                    //" لطفا سخت افزار تایید اعتبار را از کامپیوتر جدا کرده مجددا وصل کنید "
                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message36);
                    this._dispatcherTimer.IsEnabled = false;
                    this._dispatcherTimer.Stop();
                    return;
                }
                if (arg.FullID == "-2" || arg.ReceivedData == "No Access")
                {
                    this._dispatcherTimer.IsEnabled = false;
                    this._dispatcherTimer.Stop();
                    string message = CommonData.mainwindow.tm.TranslateofMessage.Message37;
                    message += CommonData.mainwindow.tm.TranslateofMessage.Message36;
                    message += CommonData.mainwindow.tm.TranslateofMessage.Message38;

                    CommonData.mainwindow.changeProgressBarTag(message);
                    CommonData.mainwindow.changeProgressBarValue(1000);
                    this.ShowDialogForm();
                    return;
                }

                #region Create DanglePass
                if (!CommonData.isHVRequest)
                {
                    if (arg.FullID == "" && arg.ReceivedData.Length == 16)
                    {
                        string s1 = xorIt(arg.ReceivedData.Substring(10, 2), arg.ReceivedData.Substring(14, 2));
                        CommonData.DanglePass = s1 + CommonData.DanglePass;

                        string[] messages = {
                                 @"<VDCode=""HV""/>",
                                 @"<DanglePass=""DanglePassParam""/>",
                                 @"<HVCode=""12345678""/>"
                             };

                        CommonData.isHVRequest = true;


                        if (_serialPortDataReceivedObject.Open())
                        {
                            _serialPortDataReceivedObject.Send(messages);
                        }
                    }
                }
                #endregion Create DanglePass

                #region Save DanglePass
                else if (CommonData.isHVRequest)
                {
                    CommonData.HVCode = getHVCode(arg.ReceivedData);
                    stopTimer();
                    if (CommonData.HVCode.ToUpper() == CommonData.DangleSerial.ToUpper())
                    {
                        if (!CommonData.getDanglePassFromDB)
                        {

                            ObjectParameter Result = new ObjectParameter("Result", 1000000);
                            ObjectParameter DangelID = new ObjectParameter("DangelID", 1000000);
                            ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                            DangelSerial = GetStringFromBytes(CryptorEngine.Encrypt(HexStringToBytes(ReverseStr(DangelSerial)), true));
                            DangelPass = GetStringFromBytes(CryptorEngine.Encrypt(HexStringToBytes(ReverseStr(CommonData.DanglePass)), true));

                            int ProvinceCode = (NumericConverter.IntConverter(CommonData.DangleLSB.Substring(12, 4), NumberStyles.HexNumber) / 1000) * 1000;
                            string str = ProvinceCode.ToString("X4");
                            if (CommonData.ProvinceCode != "")
                                if (CommonData.ProvinceCode != str)
                                {
                                    CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message40);
                                    return;
                                    //Application.Current.Shutdown();
                                }
                            string CityCode = CommonData.DangleLSB.Substring(4, 2).PadLeft(4, '0');

                            CommonData.Citycode = str + CityCode;

                            if (CityCode.ToString().PadLeft(4, '0').Equals("0000"))
                            {
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message36, "Error", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                                try
                                {
                                    Process.GetCurrentProcess().Kill();
                                }
                                catch (Exception ex)
                                {

                                    MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                                }
                                // Application.Current.Shutdown();
                                return;
                            }
                            SQLSPS.InsDangelInfo(DangelPass, DangelSerial, null, null, null, null, CommonData.EncryptExpireDate, null, CommonData.DangleLSB, ProvinceCode.ToString(), DangelID, Result, ErrMSG);
                            CommonData.getDanglePassFromDB = true;



                        }

                        CommonData.completeRequest = true;
                        //changeEnable(true);
                        CommonData.mainwindow.changeProgressBarValue(1000);
                        CommonData.mainwindow.changeProgressBarTag("");

                        CommonData.mainwindow.changeMainPageGrid(true);
                        CommonData.mainwindow.changeProgressBarValue(0);
                        if (!CommonData.mainwindow.TrialCheckedIsValid())
                        {
                            this.Close();
                            Environment.Exit(1);
                        }

                    }

                    else
                    {
                        this.Completerequest = true;
                        CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message39);
                    }
                }
                #endregion Save DanglePass
            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.ToString().ToString());
                CommonData.mainwindow.changeProgressBarTag("لطفا سایر برنامه هایی که از دانگل استفاده میکنند را ببندید" + "\r\n" + " سپس دانگل را از کامپیوتر جدا کرده، مجددا وصل کنید " + "\r\n" + " در نهایت  نرم افزار را مجددا اجرا کنید");
                CommonData.WriteLOG(ex);

            }
        }
        public void ShowDialogForm()
        {

            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
               delegate ()
               {
                   try
                   {
                       
                       this.Visibility = Visibility.Visible;
                   }
                   catch (Exception ex)
                   {
                       // MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                       this.ShowDialog(); 
                       this.Visibility = Visibility.Visible;
                   }

               }));
        }

        private void getHVCode(object sender, TokenEventArgs arg)
        {
            CommonData.HVCode = arg.TokenData;
        }
        private string getHVCode(string code)
        {
            try
            {
                string rev = code.Substring(8, 8) + code.Substring(0, 8);
                UInt64 c = UInt64.Parse(rev, NumberStyles.HexNumber);
                UInt64 z = 2271560481;
                UInt64 res = c / z;
                return res.ToString("x8"); ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                return "";
            }
        }
        #region General Function
        public static string xorIt(string key, string input)
        {
            string s = "";
            try
            {
                StringBuilder Keys = new StringBuilder("0000", 4);
                StringBuilder inputs = new StringBuilder("0000", 4);
                Keys[0] = '0';
                Keys[1] = key[0];
                Keys[2] = '0';
                Keys[3] = key[1];

                inputs[0] = '0';
                inputs[1] = input[0];
                inputs[2] = '0';
                inputs[3] = input[1];

                char a = (char)(NumericConverter.IntConverter(Keys.ToString().Substring(0, 2), NumberStyles.HexNumber));
                char a1 = (char)(NumericConverter.IntConverter(Keys.ToString().Substring(2, 2), NumberStyles.HexNumber));

                char b = (char)NumericConverter.IntConverter(inputs.ToString().Substring(0, 2), NumberStyles.HexNumber);
                char b1 = (char)NumericConverter.IntConverter(inputs.ToString().Substring(2, 2), NumberStyles.HexNumber);

                s = ((b ^ a)).ToString("X") + ((b1 ^ a1)).ToString("X");


            }
            catch (Exception)
            {

            }
            return s;
        }

        public string GetStringFromBytes(byte[] bytes)
        {
            Array.Reverse(bytes);
            string str = BytesToHexString(bytes);
            StringBuilder strb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
                strb.Append((char)(str[i] + 20));
            return strb.ToString();
        }

        public byte[] GetBytesFromString(string str)
        {
            StringBuilder strb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
                strb.Append((char)(str[i] - 20));
            byte[] bytes = HexStringToBytes(strb.ToString());
            Array.Reverse(bytes);
            return bytes;
        }

        public byte[] HexStringToBytes(string s)
        {
            if ((s.Length & 1) == 1)
                throw new ArgumentException("The length of the string must be a multiple of 2.", "s");

            string hexDigitChars = "0123456789ABCDEF";
            // Determine how many bytes there are.      
            byte[] _bytes = new byte[s.Length >> 1];

            for (int i = 0; i < s.Length; i += 2)
            {
                int _highNibble = hexDigitChars.IndexOf(Char.ToUpper(s[i]));
                int _lowNibble = hexDigitChars.IndexOf(Char.ToUpper(s[i + 1]));

                if (_highNibble == -1 || _lowNibble == -1)
                    throw new ArgumentException("Invalid digit.", "s");

                _bytes[i >> 1] = (byte)((_highNibble << 4) | _lowNibble);
            }

            return _bytes;
        }

        public string BytesToHexString(byte[] bytes)
        {
            string _s = BitConverter.ToString(bytes).Replace("-", "");
            return _s;
        }

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {

        }

        public string ReverseStr(string input)
        {
            try
            {
                char[] arr = input.ToCharArray();
                Array.Reverse(arr);
                return new string(arr);
            }
            catch
            {
                return "";
            }
        }
        #endregion General Function

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                textbox1.Clear();
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
                if (!Closewithbutton)
                {
                    if (!CommonData.IsExpire)
                        Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

    }
}