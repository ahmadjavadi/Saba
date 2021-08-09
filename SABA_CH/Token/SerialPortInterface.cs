
using System;
using System.Data.Objects;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO.Ports;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.Token
{
    public delegate void dataReceived(object sender, SerialPortEventArgs arg);
    public class SerialPortEventArgs : EventArgs
    {
        public string ReceivedData { get; private set; }
        public string FullID { get; private set; }
        public string CardNumber = "";
        public SerialPortEventArgs(string data, string id)
        {
            ReceivedData = data;
            FullID = id;
        }
    }
    /// <summary>
    /// Interfaces with a serial port. There should only be one instance
    /// of this class for each serial port to be used.
    /// </summary>
    public class SerialPortInterface
    {
        public SerialPortInterface()
        {
            this.PortName = CommonData.PortName;
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimernotreceive.Tick += new EventHandler(dispatcherTimerreceive_Tick);
            dispatcherTimernotreceive.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimernotreceive.Start();
            dispatcherTimer.IsEnabled = true;
            dispatcherTimer.Start();
        }
        #region Parameter
        string lastSendString = "";
        private string[] requestData;
        private string fullID;
        private int intCount;
        private string RxString = "";
        private int intReceiveStep = 0;
        private int intMessageNumber = 0;
        private int counter = 0;
        public static int NoAccessCounter = 0;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer dispatcherTimernotreceive = new DispatcherTimer();
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event dataReceived DataReceived;
        /// <summary>
        /// Serial port class
        /// </summary>
        private SerialPort serialPort = new SerialPort();

        /// <summary>
        /// BaudRate set to default for Serial Port Class
        /// </summary>
        private int baudRate = 9600;

        /// <summary>
        /// DataBits set to default for Serial Port Class
        /// </summary>
        private int dataBits = 8;

        /// <summary>
        /// Handshake set to default for Serial Port Class
        /// </summary>
        private Handshake handshake = Handshake.None;

        /// <summary>
        /// Parity set to default for Serial Port Class
        /// </summary>
        private Parity parity = Parity.None;

        /// <summary>
        /// Communication Port name, not default in SerialPort. Defaulted to COM1
        /// </summary>
        private string portName = "";

        /// <summary>
        /// StopBits set to default for Serial Port Class
        /// </summary>
        private StopBits stopBits = StopBits.One;

        /// <summary>
        /// Holds data received until we get a terminator.
        /// </summary>
        private string tString = string.Empty;

        /// <summary>
        /// End of transmition byte in this case EOT (ASCII 4).
        /// </summary>
        private byte terminator = 0x4;

        /// <summary>
        /// Gets or sets BaudRate (Default: 9600)
        /// </summary>
        public int BaudRate { get { return this.baudRate; } set { this.baudRate = value; } }

        /// <summary>
        /// Gets or sets DataBits (Default: 8)
        /// </summary>
        public int DataBits { get { return this.dataBits; } set { this.dataBits = value; } }

        /// <summary>
        /// Gets or sets Handshake (Default: None)
        /// </summary>
        public Handshake Handshake { get { return this.handshake; } set { this.handshake = value; } }

        /// <summary>
        /// Gets or sets Parity (Default: None)
        /// </summary>
        public Parity Parity { get { return this.parity; } set { this.parity = value; } }

        /// <summary>
        /// Gets or sets PortName (Default: COM1)
        /// </summary>
        public string PortName { get { return this.portName; } set { this.portName = value; } }

        /// <summary>
        /// Gets or sets StopBits (Default: One}
        /// </summary>
        public StopBits StopBits { get { return this.stopBits; } set { this.stopBits = value; } }

        #endregion



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                counter++;
                dispatcherTimer.Stop();
                dispatcherTimer.IsEnabled = false;

                if (counter < 5)
                    SendData(lastSendString);
                else
                {
                    SerialPortEventArgs args = new SerialPortEventArgs("No Access", "-3");
                    if (args != null)
                    {
                        this.DataReceived(this, args);
                        this.Close();
                    }
                }


            }
            catch (Exception Ex)
            {
                SerialPortEventArgs args = new SerialPortEventArgs("No Access", "-3");
                CommonData.mainwindow.changeProgressBarTag("لطفا سخت افزار تایید اعتبار را جدا کرده و مجددا وصل کنید");
               // this.DataReceived(this, args);
                this.Close();
            }
        }

        private void dispatcherTimerreceive_Tick(object sender, EventArgs e)
        {
            try
            {

                dispatcherTimernotreceive.Stop();
                dispatcherTimernotreceive.IsEnabled = false;

                this.Close();
                //System.Windows.MessageBox.Show("لطفا سخت افزار تایید اعتبار را جدا کرده و مجددا وصل کنید");
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message39);
                Application.Current.Shutdown();


            }
            catch (Exception Ex)
            {
                SerialPortEventArgs args = new SerialPortEventArgs("No Access", "-3");
                CommonData.mainwindow.changeProgressBarTag( "لطفا سخت افزار تایید اعتبار را جدا کرده و مجددا وصل کنید");
                this.DataReceived(this, args);
                this.Close();
            }
        }
        /// <summary>
        /// Sets the current settings for the Comport and tries to open it.
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>        

        public bool Open()
        {
            try
            {
                this.serialPort.BaudRate = this.baudRate;
                this.serialPort.DataBits = this.dataBits;
                this.serialPort.Handshake = this.handshake;
                this.serialPort.Parity = this.parity;
                if (this.serialPort.PortName != this.portName)
                {
                    this.serialPort.PortName = this.portName;
                }
                this.serialPort.StopBits = this.stopBits;

                this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this._serialPort_DataReceived);
                if (!this.serialPort.IsOpen)
                {
                    try { serialPort.DtrEnable = false; }
                    catch { }
                    try { serialPort.RtsEnable = false; }
                    catch { }
                    this.serialPort.Open();
                }

            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Send(byte[] data)
        {
            try
            {
                serialPort.Write(data, 0, data.Length);
            }
            catch { return false; }
            return true;
        }

        private void SendData(string strTx)
        {
            try
            {
                if (CommonData.StopData)
                    return;
                dispatcherTimer.IsEnabled = true;
                dispatcherTimer.Start();
                dispatcherTimernotreceive.IsEnabled = true;
                dispatcherTimernotreceive.Start();
                lastSendString = strTx;

                Thread.Sleep(500);

                if (CommonData.SendMessageToserialPort)
                {
                    if (!serialPort.IsOpen)
                        Open();
                    if (!serialPort.IsOpen)
                        return;
                    if (CommonData.UserName.ToUpper() == "RSAADMIN")
                    {
                        if (CommonData.mainwindow != null)
                            CommonData.mainwindow.changeProgressBarTag("SendDataTo Port: " + strTx);
                    }
                    byte[] byteWriteArray = new byte[50];
                    string strInput = "", strData;
                    int intElementCount = 0;

                    foreach (char character in strTx)
                    {
                        int tmp = character;
                        strInput += String.Format("{0:x2}", (uint)Convert.ToUInt32(tmp.ToString()));
                    }

                    // Check the length of string for being even.
                    if (strInput.Length % 2 == 0)
                    {
                        for (intCount = 0; intCount < strInput.Length; intCount += 2)
                        {
                            strData = strInput.Substring(intCount, 2);
                            byteWriteArray[intElementCount] = byte.Parse(strData,
                                NumberStyles.HexNumber);
                            intElementCount++;
                        }
                    }

                    // Else the length of string is odd.
                    else
                    {
                        for (intCount = 0; intCount < strInput.Length - 1; intCount += 2)
                        {
                            strData = strInput.Substring(intCount, 2);
                            byteWriteArray[intElementCount] = byte.Parse(strData,
                                NumberStyles.HexNumber);
                            intElementCount++;
                        }

                        strData = strInput.Substring(strInput.Length - 1, 1);
                        byteWriteArray[intElementCount] = byte.Parse(strData,
                           NumberStyles.HexNumber);
                        intElementCount++;

                    }
                    try
                    {
                        if (CommonData.SendMessageToserialPort)
                        {
                            for (intCount = 0; intCount < intElementCount; intCount++)
                            {
                                serialPort.Write(byteWriteArray, intCount, 1);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //The port 'COM5' does not exist.
                        string er = "'" + CommonData.PortName + "' does not exist.";
                        if (ex.ToString().Contains(er))
                        {
                            System.Windows.Forms.MessageBox.Show("سخت افزار تایید اعتبار را به کامپیوتر متصل کنید، نرم افزار را مجددا اجرا کنید");

                            Application.Current.Shutdown();

                            Environment.Exit(0);
                        }
                    }
                }

                else
                {
                    try
                    {
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        public bool Send(string[] messages)
        {
            try
            {
                CommonData.StopData = false;
                NoAccessCounter = 0;
                requestData = messages;

                Thread.Sleep(1000);
                if (CommonData.isTokenRequestk && CommonData.IsGenerateFirstToken)
                {
                    SendData(messages[2]);
                    intMessageNumber = 3;
                    intReceiveStep = 2;
                }
                else
                    SendData(messages[0]);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void ResetUSBDevice()
        {
            //CommonData.mainwindow.changeProgressBarValue(0);
            Open();
            SendData("Reset");
            CommonData.IsGenerateFirstToken = false;
            intReceiveStep = 0;
            intMessageNumber = 0;
            RxString = "";
        }

        public void getDangleInfo()
        {
            if (Open())
            {
                CommonData.isHVRequest = false;
                CommonData.isHVRequest = false;
                CommonData.isTokenRequestk = false;
                string[] s = { @"<DangleLSBID=""?""/>" };
                requestData = s;
                intReceiveStep = 12;
                SendData(@"<DangleLSBID=""?""/>");
                RxString = "";
            }
            else
            {
                CommonData.mainwindow.changeProgressBarTag("لطفا سایر برنامه هایی که از دانگل استفاده میکنند را ببندید" + "\r\n" + " سپس دانگل را از کامپیوتر جدا کرده، مجددا وصل کنید " + "\r\n" + " در نهایت  نرم افزار را مجددا اجرا کنید");
                //     MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                //CommonData.mainwindow.window_Closed(null, null);

            }
        }


        /// <summary>
        /// Handles DataReceived Event from SerialPort
        /// </summary>
        /// <param name="sender">Serial Port</param>
        /// <param name="e">Event arguments</param>

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Read all received data to a string variable.
                if (!serialPort.IsOpen)
                    return;
                if (intReceiveStep == 11)
                {
                    Thread.Sleep(1000);
                }
                RxString = serialPort.ReadExisting();
                if (RxString == "")
                {
                    dispatcherTimer.IsEnabled = true;
                    dispatcherTimer.Start();
                    return;
                }
                dispatcherTimer.Stop();
                dispatcherTimer.IsEnabled = false;

                if (CommonData.UserName.ToUpper() == "RSAADMIN")
                {
                    if (CommonData.mainwindow != null)
                        CommonData.mainwindow.changeProgressBarTag("getDataFromPort: " + RxString);
                }
                if (RxString.Contains("Access Denied"))
                {
                    dispatcherTimer.Stop();
                    SerialPortEventArgs args = new SerialPortEventArgs(RxString, "-2");
                    this.DataReceived(this, args);
                    this.Close();
                    NoAccessCounter = 0;
                    return;
                }
                if (RxString.Contains("Try Again") == true)
                {
                    ResetUSBDevice();
                    return;
                }

                else if (RxString.Contains("No Access"))
                {
                    NoAccessCounter++;
                    if (NoAccessCounter > 4)
                    {
                        CommonData.StopData = true;
                        dispatcherTimer.Stop();
                        this.Close();
                        SerialPortEventArgs args = new SerialPortEventArgs("No Access", "-3");
                        this.DataReceived(this, args);
                        NoAccessCounter = 0;
                    }
                    else
                    {
                        CommonData.StopData = false;
                        ResetUSBDevice();
                    }
                    return;
                }
                else if ((RxString.Contains("Invalid Data") == true))
                {
                    ResetUSBDevice();
                    return;
                }
                else
                {
                    ConsiderRecievedDataFromDangle(RxString);
                }
                dispatcherTimernotreceive.Stop();
                dispatcherTimernotreceive.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        public void Close()
        {
            try
            {
                serialPort.DataReceived -= _serialPort_DataReceived;
                serialPort.Close();


            }
            catch { }
        }

        string DanglePasswordGenerator(string usb_pass)
        {
            string reverce_usbPass = CommonData.ReverseString(usb_pass);
            UInt64 a = UInt64.Parse(reverce_usbPass, NumberStyles.HexNumber);
            UInt64 b = UInt64.Parse(CommonData.DanglePass, NumberStyles.HexNumber);
            string p = (a * b).ToString("X");
            p = p.PadLeft(12, '0');
            return p.Substring(6, 6) + p.Substring(0, 6);
        }

        void ConsiderRecievedDataFromDangle(string RxString)
        {
            try
            {
                switch (intReceiveStep)
                {
                    case 0:
                        if (RxString.Contains("<VDCode=\"TG") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);

                            // Send next Message.

                            string usb_pass = RxString.Replace("<VDCode=\"TG ", "");
                            usb_pass = usb_pass.Substring(0, 4);
                            usb_pass = DanglePasswordGenerator(usb_pass);
                            if (!requestData[1].Contains("DanglePassParam"))
                            {
                                requestData[1] = @"<DanglePass=""DanglePassParam""/>";
                            }
                            requestData[1] = requestData[1].Replace("DanglePassParam", usb_pass);
                            SendData(requestData[1]);
                            intMessageNumber = 2;
                            // Increment step.
                            intReceiveStep++;
                            break;
                        }
                        else if (RxString.Contains("<VDCode=\"HV") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);

                            // Send next Message.

                            string usb_pass = RxString.Replace("<VDCode=\"HV ", "");
                            usb_pass = usb_pass.Substring(0, 4);
                            usb_pass = DanglePasswordGenerator(usb_pass);
                            if (!requestData[1].Contains("DanglePassParam"))
                            {
                                requestData[1] = @"<DanglePass=""DanglePassParam""/>";
                            }
                            requestData[1] = requestData[1].Replace("DanglePassParam", usb_pass);
                            SendData(requestData[1]);
                            // Increase message number.
                            intMessageNumber++;
                            // Increment step.
                            intReceiveStep++;
                            break;
                        }
                        if (RxString.Contains("DangleLSBID=") == true)
                        {
                            // CommonData.mainwindow.AddProgressBar_ReadingCard(30, 2);
                            intReceiveStep = 0;
                            intMessageNumber = 0;
                            RxString = RxString.Replace("\"", "");
                            RxString = RxString.Replace("/>", "");
                            RxString = RxString.Replace("DangleLSBID=", "");
                            RxString = RxString.Replace("\0", "");
                            CommonData.DangleLSB = RxString;
                            dispatcherTimer.Stop();
                            dispatcherTimernotreceive.Stop();
                            dispatcherTimernotreceive.IsEnabled = false;
                            this.Close();

                            SerialPortEventArgs args = new SerialPortEventArgs(RxString, "");
                            this.DataReceived(this, args);
                            break;
                        }

                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            break;
                        }

                    case 1:
                        if (RxString.Contains("Access OK") == true)
                        {
                            if (CommonData.isHVRequest)
                            {
                                // Increase message number.
                                intMessageNumber++;
                                // Send next Message.
                                SendData(requestData[intMessageNumber]);

                                // Increment step.
                                intReceiveStep++;
                            }

                            else
                            {
                                // Send next Message.
                                SendData(requestData[intMessageNumber]);
                                if (CommonData.mainwindow != null)
                                    CommonData.mainwindow.changeProgressBarValue(1);
                                // Increase message number.
                                intMessageNumber++;
                                // Increment step.
                                intReceiveStep++;
                            }
                            break;
                        }

                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            break;
                        }


                    case 2:
                        if (RxString.Contains("OK 1") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);

                            // Send next Message.
                            SendData(requestData[intMessageNumber]);
                            // Increase message number.
                            intMessageNumber++;
                            // Increment step.
                            intReceiveStep++;
                            break;
                        }
                        else if (RxString.Contains("OK 1") == true)
                        {
                            break;
                        }
                        else if (RxString.Contains("HVCode="))
                        {
                            RxString = RxString.Replace("<HVCode=", "");
                            RxString = RxString.Replace("\"", "");
                            RxString = RxString.Replace("/>", "");
                            dispatcherTimer.Stop();
                            this.Close();
                            SerialPortEventArgs args = new SerialPortEventArgs(RxString, "");
                            this.DataReceived(this, args);
                            break;
                        }

                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            break;
                        }

                    case 3:
                        if (RxString.Contains("OK 2") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);

                            // Send next Message.
                            SendData(requestData[intMessageNumber]);
                            // Increase message number.
                            intMessageNumber++;
                            // Increment step.
                            intReceiveStep++;
                            break;
                        }
                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            break;
                        }

                    case 4:
                        if (RxString.Contains("FullTID") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);
                            RxString = RxString.Replace("<FullTID=\"", "");
                            RxString = RxString.Replace("\"/>", "");
                            fullID = RxString;
                            ObjectParameter Result = new ObjectParameter("Result", 100000000000000);
                            ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                            ObjectParameter TokenFromDongleID = new ObjectParameter("TokenFromDongleID", 100000000000000);
                            SQLSPS.InsCreateTokenByDongle(CommonData.DayofYear, CommonData.MinuteOfDay, CommonData.SystemID, CommonData.SequenceNumber, RxString, CommonData.CurrentYear, CommonData.MeterID, TokenFromDongleID, Result, ErrMSG);
                            if (ErrMSG.Value != "")
                                System.Windows.Forms.MessageBox.Show(ErrMSG.Value.ToString());

                            // Send next Message.
                            SendData(requestData[intMessageNumber]);
                            // Increase message number.
                            intMessageNumber++;
                            // Increment step.
                            intReceiveStep++;
                            break;
                        }
                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            break;
                        }

                    case 5:
                        if (RxString.Contains("OK 4") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);

                            // Send next Message.
                            SendData(requestData[intMessageNumber]);
                            // Increase message number.
                            intMessageNumber++;
                            // Increment step.
                            intReceiveStep++;
                            break;
                        }
                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            break;
                        }

                    case 6:
                        if (RxString.Contains("OK 5") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);

                            // Send next Message.
                            SendData(requestData[intMessageNumber]);
                            // Increase message number.
                            intMessageNumber++;
                            // Increment step.
                            intReceiveStep++;
                            break;
                        }
                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            break;
                        }

                    case 7:
                        if (RxString.Contains("OK 6") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);

                            // Send next Message.
                            SendData(requestData[intMessageNumber]);
                            // Increase message number.
                            intMessageNumber++;
                            // Increment step.
                            intReceiveStep++;
                            break;
                        }
                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            break;
                        }

                    case 8:
                        if (RxString.Contains("OK 7") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);

                            // Send next Message.
                            SendData(requestData[intMessageNumber]);
                            // Increase message number.
                            intMessageNumber++;
                            // Increment step.
                            intReceiveStep++;
                            break;
                        }
                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            break;
                        }

                    case 9:
                        if (RxString.Contains("OK 8") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);

                            // Send next Message.
                            SendData(requestData[intMessageNumber]);
                            // Increase message number.
                            intMessageNumber++;
                            // Increment step.
                            intReceiveStep++;
                            break;
                        }
                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            break;
                        }
                    case 10:
                        if (RxString.Contains("OK 9") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);
                            // Increase message number.

                            SendData(requestData[intMessageNumber]);
                            intMessageNumber++;
                            // Send next Message.
                            // Increment step.
                            intReceiveStep++;
                            Thread.Sleep(2500);
                            break;
                        }
                        else
                        {
                            SendData(requestData[intMessageNumber]);
                            Thread.Sleep(2500);
                            break;
                        }

                    case 11:

                        //if (RxString.Contains("<HexToken") == true)
                        //{

                        //    CommonData.mainwindow.AddProgressBar_ReadingCard(6,2);
                        //    intReceiveStep = 1;
                        //    intMessageNumber = 0;

                        //    RxString = RxString.Replace("\"", "");
                        //    RxString = RxString.Replace("/>", "");
                        //    RxString = RxString.Replace("<HexToken=", "");
                        //  // SerialPortEventArgs args = new SerialPortEventArgs(RxString);
                        //  //  this.DataReceived(this, args);
                        //}
                        //else 
                        if (RxString.Contains("<Token") == true)
                        {
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);
                            intReceiveStep = 1;
                            intMessageNumber = 0;
                            RxString = RxString.Replace("\"", "");
                            RxString = RxString.Replace("/>", "");
                            RxString = RxString.Replace("<Token=", "");
                            dispatcherTimer.Stop();
                            this.Close();
                            SerialPortEventArgs args = new SerialPortEventArgs(RxString, fullID);
                            this.DataReceived(this, args);
                        }
                        else
                        {
                            Thread.Sleep(1000);
                            if (CommonData.mainwindow != null)
                                CommonData.mainwindow.changeProgressBarValue(1);
                        }
                        break;

                    case 12:
                        if (RxString.Contains("DangleLSBID=") == true)
                        {
                            // CommonData.mainwindow.AddProgressBar_ReadingCard(30, 2);
                            intReceiveStep = 0;
                            intMessageNumber = 0;
                            RxString = RxString.Replace("\"", "");
                            RxString = RxString.Replace("/>", "");
                            RxString = RxString.Replace("DangleLSBID=", "");
                            RxString = RxString.Replace("\0", "");
                            CommonData.DangleLSB = RxString;
                            dispatcherTimer.Stop();
                            SerialPortEventArgs args = new SerialPortEventArgs(RxString, "");
                            serialPort.DiscardInBuffer();
                            this.DataReceived(this, args);
                            this.Close();



                        }
                        else
                        {
                            ResetUSBDevice();
                        }
                        break;
                }


            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarTag("لطفا سایر برنامه هایی که از دانگل استفاده میکنند را ببندید" + "\r\n" + " سپس دانگل را از کامپیوتر جدا کرده، مجددا وصل کنید " + "\r\n" + " در نهایت  نرم افزار را مجددا اجرا کنید");
            }
        }
    }
}