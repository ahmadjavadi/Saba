using System;
using System.CodeDom;
using System.ComponentModel;
using System.Configuration;
using System.Data.Objects;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Xml.Serialization;
using Ionic.Zip;
using SABA_CH.DataBase;
using SABA_CH.Global;
using MessageBox = System.Windows.MessageBox;
using TabControl = System.Windows.Controls.TabControl;
using RsaDateTime;
using System.Collections.Generic;
using HandHeldReader;
using SABA_CH.VEEClasses;
using VEE.WaterData;
using System.Data;
using System.Linq;

using Button = System.Windows.Controls.Button;
namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for ImportFromSABAPC.xaml
    /// </summary>
    public partial class ImportFromSABAPC : System.Windows.Window
    {
        public ShowTranslateofLable tr = null;
        public readonly int windowID = 48;
        private TabControl tabCtrl;
        private TabItem tabPag;
        public ShowGroups_Result Selectedgroup;
        private string FilePath1;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
        string FilePath;
        string connectionString = "";
        public ImportFromSABAPC()
        {
            InitializeComponent();
            translateWindows();
            RefreshCmbGroups();
        }

        public void translateWindows()
        {
            try
            {
                tr = CommonData.translateWindow(windowID);
                GridMain.DataContext = tr.TranslateofLable;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }
        }
        public void RefreshCmbGroups()
        {
            try
            {
                string p = CommonData.ProvinceCode;
                string filter = "";
                try
                {
                    int value = Int32.Parse(p, NumberStyles.HexNumber);
                    value = value / 1000;
                    if (value == 8)
                        value = 1;
                    else if (value == 1)
                        value = 8;

                    if (value != 0)
                        filter = "and m.ProvinceID in (0," + value + ")";
                }
                catch
                {
                    filter = "";
                }
                ShowGroups Groups = new ShowGroups(filter, 0, CommonData.LanguagesID);
                CmbGroups.ItemsSource = Groups.CollectShowGroups;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                tabCtrl.SelectedItem = tabPag;
                if (!tabCtrl.IsVisible)
                {
                    tabCtrl.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                ClassControl.OpenWin[windowID] = false;
                tabCtrl.Items.Remove(tabPag);
                if (!tabCtrl.HasItems)
                {
                    tabCtrl.Visibility = Visibility.Hidden;
                }
                CommonData.mainwindow.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }
        private void CmbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CmbGroups.SelectedItem != null)
                {
                    Selectedgroup = (ShowGroups_Result)CmbGroups.SelectedItem;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
               
                OpenFileDialog dlg = new OpenFileDialog();

                dlg.DefaultExt = "*.zip"; // Default file extension
                dlg.Filter = "zip file (*.zip)|*.zip|  All files (*.*)|*.*";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string FilePath = dlg.FileName;

                    string st = FilePath.Remove(FilePath.LastIndexOf('\\')) + "\\TempExportToSaba.xml";
                    if (File.Exists(st))
                        File.Delete(st);

                    GetValue(FilePath);
                }
              //  FilePath = txtpath.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
        }

        private void GetValue(string FilePath)
        {
            ZipFile zip = new ZipFile(FilePath);
            string path;
            txtpath.Text = FilePath;
            path = FilePath.Remove((FilePath.LastIndexOf('\\')));
            zip.Password = "";
            zip.TempFileFolder = path;
            zip.ExtractAll(path);
            FilePath1 = zip.TempFileFolder+ "\\TempExportToSaba.xml";
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtpath.Text == "")
            {
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message69);
                return;
            }
            if (txtpath.Text != "")
            {
                FilePath = FilePath1;
                if (FilePath.Contains(".xml"))
                {
                    connectionString = ConfigurationManager.ConnectionStrings["SabaNewEntities"].ConnectionString.ToLower();
                    int i = connectionString.IndexOf("data source");
                    
                    int j = connectionString.LastIndexOf(";");
                    connectionString = connectionString.Substring(i, j - i + 1);

                    CommonData.mainwindow.changeProgressBar_MaximumValue(5);
                    CommonData.mainwindow.changeProgressBarTag("");
                    CommonData.mainwindow.changeProgressBarValue(0);
                    //exportSABAPC();

                    Thread th = new Thread(exportSABAPC);
                    th.IsBackground = true;
                    th.Start();


                }
                else
                {
                    MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message63);
                }

            }

        }

        private void exportSABAPC()
        {
            try
            {
                int i = 0;
                changeEnable(false);
                CommonData.mainwindow.changeProgressBarValue(0);
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message41);
                CommonData.mainwindow.changeProgressBar_MaximumValue(11);
                CreateShema(FilePath, connectionString);
                //CommonData.mainwindow.changeProgressBarValue(1);

                changeEnable(true);
                //this.Close();
                //File.Delete(FilePath);
                //CommonData.mainwindow.RefreshSelectedMeters("and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + CommonData.mainwindow.SelectedGroup.GroupID + "and  GroupType=" + CommonData.mainwindow.SelectedGroup.GroupType + ") ");              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
            }
            
        }


        public int  CreateShema(string Filepath, string ConnectString)
        {
            try
            {
                int i = 0;
             
                string path = AppDomain.CurrentDomain.BaseDirectory;

                ExportToSabaData oClass = new ExportToSabaData();
                XmlSerializer serializer = new XmlSerializer(typeof(ExportToSabaData));

                ExportToSabaData deserializedList;
                using (FileStream stream = File.OpenRead(Filepath))
                {
                   deserializedList = (ExportToSabaData)serializer.Deserialize(stream);
                }
                //AddTotalConsumptionToList(deserializedList);

                InsertDatatoDB(deserializedList);
                

                return i;
                
            }
            catch (Exception ex)
            {
                CommonData.mainwindow.changeProgressBarTag("");
                CommonData.mainwindow.changeProgressBarValue(0);
                MessageBox.Show("اطلاعات این فایل ذخیره شده است"); CommonData.WriteLOG(ex);
                CommonData.mainwindow.RefreshSelectedMeters("and Main.MeterID in (Select MeterID From MeterToGroup where GroupID=" + CommonData.mainwindow.SelectedGroup.GroupID + "and  GroupType=" + CommonData.mainwindow.SelectedGroup.GroupType + ") ");
             
                return -1;
            }
          
        }

        private void AddTotalConsumptionToList(ExportToSabaData deserializedList)
        {
            
            if (deserializedList != null && NumericConverter.IntConverter(deserializedList.TotalMeterReadingsCount) > 0)
            {
                foreach (var item in deserializedList.MeterReadingsList.MeterReading)
                {
                    if (item.Meter.StartsWith("207"))
                    {
                        var objectList = item.ReadOuts.Object;
                        decimal wt = 0;
                        for (int i = 0; i < objectList.Count; i++)
                        {
                            UI.Object o1 = new Object();
                            switch (objectList[i].Obis)
                            {
                                case "3.0.1":
                                    wt = Convert.ToDecimal(objectList[i].Value.Replace("/", "."));   break;//آب مصرفی كل
                                case "3.1.0":
                                    o1.Obis = "080201000065";wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString(); break;//آب مصرفی ماه جاری
                                case "3.1.1":
                                    o1.Obis = "080201000066";
                                    wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", "."));
                                    o1.Value = wt.ToString();
                                    
                                    objectList.Add(o1);
                                    break;//آب مصرفی ماه 1
                                case "3.1.2":
                                    o1.Obis = "080201000067";
                                    wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", "."));
                                    o1.Value = wt.ToString();
                                    objectList.Add(o1);
                                    break;//آب مصرفی ماه 2
                                case "3.1.3":
                                    o1.Obis = "080201000068"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 3
                                case "3.1.4":
                                    o1.Obis = "080201000069"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 4
                                case "3.1.5":
                                    o1.Obis = "08020100006A"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 5
                                case "3.2.0":
                                    o1.Obis = "08020100006B"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 6
                                case "3.2.1":
                                    o1.Obis = "08020100006C"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 7
                                case "3.2.2":
                                    o1.Obis = "08020100006D"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 8
                                case "3.2.3":
                                    o1.Obis = "08020100006E"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 9
                                case "3.2.4":
                                    o1.Obis = "08020100006F"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 10
                                case "3.2.5":
                                    o1.Obis = "080201000070"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 11
                                case "3.3.0":
                                    o1.Obis = "080201000071"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 12
                                case "3.3.1":
                                    o1.Obis = "080201000072"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 13
                                case "3.3.2":
                                    o1.Obis = "080201000073"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 14
                                case "3.3.3":
                                    o1.Obis = "080201000074"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 15
                                case "3.3.4":
                                    o1.Obis = "080201000075"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 16
                                case "3.3.5":
                                    o1.Obis = "080201000076"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 17
                                case "3.4.0":
                                    o1.Obis = "080201000077"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 18
                                case "3.4.1":
                                    o1.Obis = "080201000078"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 19
                                case "3.4.2":
                                    o1.Obis = "080201000079"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 20
                                case "3.4.3":
                                    o1.Obis = "08020100007A"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 21
                                case "3.4.4":
                                    o1.Obis = "08020100007B"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 22
                                case "3.4.5":
                                    o1.Obis = "08020100007C"; wt = wt - Convert.ToDecimal(objectList[i].Value.Replace("/", ".")); o1.Value = wt.ToString();  objectList.Add(o1); break;//آب مصرفی ماه 23
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public bool InsertDatatoDB(ExportToSabaData deserializedList)
        {          
            bool IsCorrect = false;
            try
            {
                ObjectParameter meterId = new ObjectParameter("MeterID", 100000000000000);
                ObjectParameter Result;
     
                ObjectParameter isExits = new ObjectParameter("IsExits", false);
                ObjectParameter ObisValueID;
                // thre isn't reapit
                #region MeterReadingsList

                if (NumericConverter.IntConverter(deserializedList.TotalMeterReadingsCount) > 0)
                {
                    CommonData.mainwindow.MeterList = new List<SelectedMeter>();
                    foreach (var item in deserializedList.MeterReadingsList.MeterReading)
                    {
                        Thread.Sleep(1000);
                        if (item.ReadOuts.Object == null|| item.ReadOuts.Object.Count==0)
                            continue;
                        var ErrMSG = new ObjectParameter("ErrMSG", "");
                        Result = new ObjectParameter("Result", 10000000000000000);

                        string MeterNumber = item.Meter;
                        
                        string ReadingDateTime =RsaDateTime.PersianDateTime.ConvertTo24HTime( item.ReadingDateTime);
                        string MeterDateTime = RsaDateTime.PersianDateTime.ConvertTo24HTime(item.MeterDateTime);
                        string clockValue = "";
                        string transferDate = "";
                        decimal? SelectedMeterID = 0;
                        var softwareVersion = "";
                        foreach (var obj in item.ReadOuts.Object)
                        {
                            if (obj.Obis == "0100000200FF")
                            {
                                softwareVersion = obj.Value;
                                break;
                            }
                        }
                        
                        SQLSPS.ShowMeterNumber(MeterNumber, isExits, Result, ErrMSG);
                        decimal? Customerid = -1; 
                        ShowMeter_Result meterInfo;
                        if (Convert.ToBoolean(isExits.Value))
                        {
                            meterInfo =
                               SQLSPS.ShowMeter(" and Main.Valid=1 and Main.MeterNumber='" + MeterNumber + "'");
                            Customerid = meterInfo.CustomerID;
                        }
                        else
                        {
                            Customerid = null;
                            SQLSPS.InsMeter(MeterNumber, null, true, softwareVersion, null, null, Customerid, CommonData.selectedGroup.GroupID, CommonData.selectedGroup.GroupType, true, meterId, ErrMSG,
                            Result);
                        }
                        CommonData.CustomerId = Customerid;
                        //if (Convert.ToBoolean(isExits.Value) == true && Customerid == 0 && CommonData.UserID != 1)
                        //{
                        //    MessageBox.Show("شما مجاز به دیدن اطلاعات این کنتور نیستید");
                        //    return false;
                        //}



                        meterInfo = SQLSPS.ShowMeter(" and Main.Valid=1 and Main.MeterNumber='" + MeterNumber + "'");
                        Customerid = meterInfo.CustomerID;
                        var meterInfo1 = SQLSPS.ShowMeter(" and Main.Valid=1 and Main.MeterNumber='" + MeterNumber + "'");
                        if (meterInfo1 == null|| meterInfo1.MeterID<1)
                        { 
                        }
                        SelectedMeterID = Convert.ToDecimal(meterInfo.MeterID);
                        
                        SQLSPS.InsMeterToGroup(SelectedMeterID, Selectedgroup.GroupID, Selectedgroup.GroupType, Result, ErrMSG);

                        if (CommonData.LanguagesID == 2)
                        {
                            clockValue = PersianDateTime.ConvertToPersianDateTimeSaba(MeterDateTime);
                            
                            transferDate = PersianDateTime.ConvertToPersianDateTimeSaba(ReadingDateTime); // DateTime.Now.ToPersianString(); 
                            if (clockValue == "1300/01/01 00:00:00" || transferDate == "1300/01/01 00:00:00")
                                continue;
                        }
                       
                        if (!Convert.ToBoolean(SQLSPS.ShowClockObisValue(clockValue, (SelectedMeterID))))
                        {
                            Result = new ObjectParameter("Result", 10000000000000000);
                            ErrMSG = new ObjectParameter("ErrMSG", "");
                            ObjectParameter obisValueHeaderId = new ObjectParameter("OBISValueHeaderID",
                                1000000000000000);
                            // get OBISValueHeaderID
                            SQLSPS.InsobisValueHeader(clockValue, 1, CommonData.UserID, transferDate,
                                Convert.ToDecimal(SelectedMeterID), 5, obisValueHeaderId, Result, ErrMSG);
                            //  INSCurve
                            if (item.CurveInformation.ObjectCurve != null && item.CurveInformation.ObjectCurve.Count > 0)
                            {
                                SQLSPS.InsCurve(SelectedMeterID, Convert.ToDecimal(obisValueHeaderId.Value),
                                    item.CurveInformation.ObjectCurve[12].Value,
                                    item.CurveInformation.ObjectCurve[0].Value, item.CurveInformation.ObjectCurve[1].Value
                                    , item.CurveInformation.ObjectCurve[2].Value, item.CurveInformation.ObjectCurve[3].Value,
                                    item.CurveInformation.ObjectCurve[4].Value, item.CurveInformation.ObjectCurve[5].Value,
                                    item.CurveInformation.ObjectCurve[6].Value
                                    , item.CurveInformation.ObjectCurve[7].Value, item.CurveInformation.ObjectCurve[8].Value,
                                    item.CurveInformation.ObjectCurve[9].Value, item.CurveInformation.ObjectCurve[10].Value,
                                    item.CurveInformation.ObjectCurve[11].Value
                                    , item.CurveInformation.ObjectCurve[13].Value,
                                    item.CurveInformation.ObjectCurve[14].Value,
                                    item.CurveInformation.ObjectCurve[15].Value, Result, ErrMSG);
                            }
                            // InsertReadOutstoObisValueHeader
                            if (MeterNumber.StartsWith("207"))
                            {
                                CommonData.mainwindow.AddMeterToList(MeterNumber, clockValue, "", out var customerId);
                                SaveINDBReadValueFromHHU(MeterNumber,Convert.ToDecimal( SelectedMeterID),obisValueHeaderId, clockValue, "", item);
                                VeeMeterData vee = new VeeMeterData();
                                vee.Vee207data(Convert.ToDecimal(SelectedMeterID), MeterNumber, Customerid);
                                //Save207Data(item, MeterNumber, clockValue, SelectedMeterID, obisValueHeaderId);

                            }
                            else
                                Save303Data(item, MeterNumber, clockValue, SelectedMeterID, obisValueHeaderId,Customerid);
                        }
                        if (CommonData.mainwindow != null)
                        {
                            CommonData.mainwindow.changeProgressBarTag("ذخیره ...");
                            CommonData.mainwindow.changeProgressBarValue(80);
                        }
                    }                   
                }

                #endregion MeterReadingsList
                if (CommonData.mainwindow != null)
                {
                    CommonData.mainwindow.VeeListoFMeterFromHHU();
                    CommonData.mainwindow.RefreshSelectedMeters("");
                    Thread.Sleep(1000);
                    CommonData.mainwindow.changeProgressBar_MaximumValue(100);
                    CommonData.mainwindow.changeProgressBarTag("ذخیره سازی با موفقیت به اتمام رسید");
                   
                    CommonData.mainwindow.changeProgressBarValue(100);
                }
            }
            catch (Exception ex)
            {
                if (CommonData.mainwindow != null)
                {
                    CommonData.mainwindow.changeProgressBarTag("خطا در ذخیره سازی داده ها ");
                    CommonData.mainwindow.changeProgressBarValue(1000);
                }
            }
           
            return IsCorrect;
        }

       

        private void Save303Data( MeterReading item,            
            string MeterNumber, string clockValue, decimal? SelectedMeterID, 
            ObjectParameter obisValueHeaderId,decimal? customerId)
        {
            var ErrMSG = new ObjectParameter("ErrMSG", "");
            string softwareVersion = "";
            double PreWater = 0;
            bool isFirstMonth = true;
            string preDateTime = "";
            ObjectParameter Result;
            ObjectParameter returnUnitConvertType;
            ObjectParameter returnObisType;
            ObjectParameter FixedOBISCode;            
            ObjectParameter isExits = new ObjectParameter("IsExits", false);
            #region ReadOuts

            ObjectParameter obisid;
            if (item.ReadOuts.Object != null && item.ReadOuts.Object.Count > 0)
            {
                foreach (var readOut in item.ReadOuts.Object)
                {
                    try
                    {

                        readOut.Value = RsaDateTime.PersianDateTime.ConvertTo24HTime(readOut.Value);
                        obisid = new ObjectParameter("OBISID", 10000000000000000);
                        returnObisType = new ObjectParameter("ReturnOBISType", 1000000000000000);
                        FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                        returnUnitConvertType = new ObjectParameter("ReturnUnitConvertType",
                            1000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        Result = new ObjectParameter("Result", 10000000000000000);

                        if (MeterNumber.StartsWith("19"))
                            if (readOut.Obis == "0100000200FF")
                                softwareVersion = readOut.Value;

                        //Insert ReadOuts to ObisValueDetails
                        SQLSPS.INSOBISs("", readOut.Obis, "", "", "", Convert.ToDecimal(1), "",
                            Convert.ToDecimal(1), "", 1, "", "", "",
                            FixedOBISCode, returnUnitConvertType, returnObisType, obisid, Result, ErrMSG);



                        if (FixedOBISCode.Value.ToString() != "")
                        {
                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) == 500 &&
                                CommonData.LanguagesID == 2)
                            {
                                readOut.Value = SaveDateValue(readOut.Value);
                            }
                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 900 &&
                                Convert.ToInt32(FixedOBISCode.Value.ToString()) < 925)
                            {
                                SaveConsumedActiveEnergy(clockValue, SelectedMeterID, FixedOBISCode,
                                    obisid, obisValueHeaderId, readOut.Value);
                            }
                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 800 &&
                                Convert.ToInt32(FixedOBISCode.Value.ToString()) < 825)
                            {

                                if (Convert.ToInt32(obisid.Value.ToString()) == 81)
                                {
                                    SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID,
                                        softwareVersion, FixedOBISCode, obisid, obisValueHeaderId, ConvertToMinutes(readOut.Value),
                                        "");
                                }
                                else
                                {
                                    SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID, softwareVersion,
                                      FixedOBISCode, obisid, obisValueHeaderId, readOut.Value, "");
                                }

                            }
                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 825 &&
                                Convert.ToInt32(FixedOBISCode.Value.ToString()) < 863)
                            {
                                SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID,
                                    softwareVersion, FixedOBISCode, obisid, obisValueHeaderId, readOut.Value, "");
                            }

                        }
                        Result = new ObjectParameter("Result", 10000000000000000);
                        var ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        SQLSPS.InsobisValueDetail(Convert.ToDecimal(obisValueHeaderId.Value),
                            softwareVersion,
                            Convert.ToDecimal(obisid.Value), readOut.Value, null, "", ObisValueID, Result,
                            ErrMSG);
                    }
                    catch (Exception ex)
                    {
 
                    }


                }
            }
       
            #endregion ReadOuts

            #region Billing

            int ApartOBIS = 101;
            double monthlyWater = 0;
            if (item.Billing.Bill != null && item.Billing.Bill.Count > 0)
            {
               
                ObjectParameter obisidw = new ObjectParameter("OBISID", 10000000000000000);
                ObjectParameter obisPomp = new ObjectParameter("OBISID", 10000000000000000);

                var ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);

                string pompObis;
                string waterCode;
                string myHex = ApartOBIS.ToString("X");
                string obisCode = "0802010000" + myHex;
                Meter_Consumption_Data303.ConsumedWaterdata = new List<VEE.VeeConsumedWater>();
                try
                {
                    foreach (var water in item.Billing.Bill)
                    {
                        Result = new ObjectParameter("Result", 10000000000000000);
                        obisid = new ObjectParameter("OBISID", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                        returnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", 1000000000000000);
                        returnObisType = new ObjectParameter("ReturnOBISType", 1000000000000000);


                        water.DateTime = RsaDateTime.PersianDateTime.ConvertTo24HTime(water.DateTime);

                        myHex = ApartOBIS.ToString("X");



                        // Time of pump working Xst Month
                        pompObis = "0802606202" + myHex;

                        // Overall Consumed volume of water 
                        obisCode = "0802010000" + myHex;


                        SQLSPS.INSOBISs("", obisCode, "", "", "", 1, "", null, "", 1, "", "", "",
                            FixedOBISCode, returnUnitConvertType, returnObisType, obisid, Result, ErrMSG);
                        SQLSPS.InsobisValueDetail(Convert.ToDecimal(obisValueHeaderId.Value),
                            softwareVersion, Convert.ToDecimal(obisid.Value), water.WaterConsumption,
                            null, "", ObisValueID, Result, ErrMSG);




                        Result = new ObjectParameter("Result", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                        returnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", 1000000000000000);
                        returnObisType = new ObjectParameter("ReturnOBISType", 1000000000000000);
                        ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);

                        SQLSPS.INSOBISs("", pompObis, "", "", "", 1, "", null, "", 1, "", "", "",
                            FixedOBISCode, returnUnitConvertType, returnObisType, obisPomp, Result, ErrMSG);



                        SQLSPS.InsobisValueDetail(Convert.ToDecimal(obisValueHeaderId.Value),
                            softwareVersion, Convert.ToDecimal(obisPomp.Value), water.TimeOfPumpWorking,
                            null, "", ObisValueID, Result, ErrMSG);







                        // Xst Overall Consumed volume of water
                        SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID, softwareVersion,
                            FixedOBISCode, obisPomp, obisValueHeaderId, ConvertToMinutes(water.TimeOfPumpWorking), water.DateTime);

                        // Xst Overall Consumed volume of water
                        SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID, softwareVersion,
                            FixedOBISCode, obisid, obisValueHeaderId, water.WaterConsumption, water.DateTime);

                        // Xst Month Consumed Water
                        if (isFirstMonth)
                        {
                            PreWater = NumericConverter.DoubleConverter(water.WaterConsumption.Replace("/","."));
                            preDateTime = water.DateTime;
                            isFirstMonth = false;
                        }
                        else
                        {
                            myHex = (ApartOBIS - 1).ToString("X");
                            //Month Consumed Water
                            waterCode = "0802010100" + myHex;
                            Result = new ObjectParameter("Result", 10000000000000000);
                            ErrMSG = new ObjectParameter("ErrMSG", "");
                            FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                            returnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", 1000000000000000);
                            returnObisType = new ObjectParameter("ReturnOBISType", 1000000000000000);
                            ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                            obisidw = new ObjectParameter("OBISID", 10000000000000000);

                            SQLSPS.INSOBISs("", waterCode, "", "", "", 1, "", null, "", 1, "", "", "",
                            FixedOBISCode, returnUnitConvertType, returnObisType, obisidw, Result, ErrMSG);

                            monthlyWater = Math.Round(PreWater - NumericConverter.DoubleConverter(water.WaterConsumption), 2);

                            SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID, softwareVersion,
                                FixedOBISCode, obisidw, obisValueHeaderId, monthlyWater.ToString(),
                                preDateTime);
                            PreWater = NumericConverter.DoubleConverter(water.WaterConsumption);
                            preDateTime = water.DateTime;
                        }

                        var date1 = RsaDateTime.PersianDateTime.ConvertToPersianDateSaba(water.DateTime);
                        if (!date1.StartsWith("1300/01/01")) {
                            if (date1.Contains(" "))
                                date1 = date1.Substring(0, date1.IndexOf(' '));
                            if (!Meter_Consumption_Data303.ConsumedWaterdata.Any(x => x.ConsumedDate.Contains(date1)))
                            {
                                Meter_Consumption_Data303.ConsumedWaterdata.Add(new VEE.VeeConsumedWater()
                                {
                                    ConsumedDate = date1,
                                    CustomerId = customerId,
                                    MeterId = SelectedMeterID,
                                    IsValid = true,
                                    TotalConsumption = water.WaterConsumption,
                                    MonthlyConsumption = monthlyWater.ToString(),
                                    Flow = ConvertToMinutes(water.TimeOfPumpWorking)
                                }
                            );
                            }
                        }
                        ApartOBIS++;
                    }
                }
                catch (Exception ex)
                {
                     
                }
                
                
                SaveToVeeConsumedWater(softwareVersion, SelectedMeterID);
            }

            #endregion Billing
        }







        private void SaveToVeeConsumedWater(string softwareVersion, decimal? meterId)
        {
            try
            {
                if (softwareVersion.EndsWith("3"))
                {
                    return;
                }
                ShowVeeConsumedWater consumedWater1 = new ShowVeeConsumedWater(meterId);
               var preConsumption= new List<VEE.VeeConsumedWater>();
                foreach (var item in consumedWater1._lstShowVeeConsumedWater)
                {
                    preConsumption.Add
                        (new VEE.VeeConsumedWater()
                        {
                            ConsumedDate = item.ConsumedDate,
                            Flow = item.Flow,
                            IsValid = item.IsValid,
                            MeterId = item.MeterId,
                            CustomerId = item.CustomerId,
                            MonthlyConsumption = item.MonthlyConsumption,
                            TotalConsumption = item.TotalConsumption
                        });
                }
                if (preConsumption.Count > 0 )
                {
                    
                    if (Meter_Consumption_Data303.ConsumedWaterdata[0].ConsumedDate.CompareTo(preConsumption[0].ConsumedDate) <= 0 )
                    {
                        return;
                    }
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("CustomerId");
                dt.Columns.Add("MeterId");
                dt.Columns.Add("ConsumedDate");
                dt.Columns.Add("Flow");
                dt.Columns.Add("MonthlyConsumption");
                dt.Columns.Add("TotalConsumption");
                dt.Columns.Add("IsValid");
                dt.Columns.Add("PumpWorkingHour");
                for (int i = 0; i < Meter_Consumption_Data303.ConsumedWaterdata.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["CustomerId"] = Meter_Consumption_Data303.ConsumedWaterdata[i].CustomerId;
                    dr["MeterId"] = Meter_Consumption_Data303.ConsumedWaterdata[i].MeterId;
                    dr["ConsumedDate"] = Meter_Consumption_Data303.ConsumedWaterdata[i].ConsumedDate;
                    dr["Flow"] = "";
                    
                    dr["TotalConsumption"] = Math.Round(NumericConverter.DoubleConverter(Meter_Consumption_Data303.ConsumedWaterdata[i].TotalConsumption), 2).ToString().Replace("/",".");
                    dr["IsValid"] = true;
                    if (Meter_Consumption_Data303.ConsumedWaterdata[i].Flow == "-1")
                        dr["PumpWorkingHour"] = "NA";
                    else
                        dr["PumpWorkingHour"] = Meter_Consumption_Data303.ConsumedWaterdata[i].Flow.ToString().Replace("/", ".");
                    if (i< Meter_Consumption_Data303.ConsumedWaterdata.Count-1)
                        dr["MonthlyConsumption"] = Math.Round(                       NumericConverter.DoubleConverter(Meter_Consumption_Data303.ConsumedWaterdata[i].TotalConsumption) - NumericConverter.DoubleConverter(Meter_Consumption_Data303.ConsumedWaterdata[i+1].TotalConsumption), 2).ToString().Replace("/", ".");
                    else
                        dr["MonthlyConsumption"] = Math.Round(NumericConverter.DoubleConverter(Meter_Consumption_Data303.ConsumedWaterdata[i].TotalConsumption),2).ToString().Replace("/", ".");
                    dt.Rows.Add(dr);
                }
                var errMsg = new ObjectParameter("ErrMsg", "");
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                //SQLSPS.DelVeeConsumedWater(Meter_Consumption_Data303.ConsumedWaterdata[0].CustomerId, Meter_Consumption_Data303.ConsumedWaterdata[0].MeterId,
                //    Meter_Consumption_Data303.ConsumedWaterdata[Meter_Consumption_Data303.ConsumedWaterdata.Count - 1].ConsumedDate, 
                //    Meter_Consumption_Data303.ConsumedWaterdata[0].ConsumedDate,
                //    errMsg);
                SQLSPS.UPDVeeConsumedWater(dt);

               
            }
            catch (Exception)
            {
                throw;
            }
        }




        private void Save207Data(MeterReading item,
            string MeterNumber, string clockValue, decimal? SelectedMeterID,
            ObjectParameter obisValueHeaderId)
        {
            var ErrMSG = new ObjectParameter("ErrMSG", "");
            string softwareVersion = "";
            double PreWater = 0;
            bool isFirstMonth = true;
            string preDateTime = "";
            ObjectParameter Result;
            ObjectParameter returnUnitConvertType;
            ObjectParameter returnObisType;
            ObjectParameter FixedOBISCode;
            ObjectParameter isExits = new ObjectParameter("IsExits", false);
            #region ReadOuts

            ObjectParameter obisid;
            if (item.ReadOuts.Object != null && item.ReadOuts.Object.Count > 0)
            {
                foreach (var readOut in item.ReadOuts.Object)
                {
                    obisid = new ObjectParameter("OBISID", 10000000000000000);
                    returnObisType = new ObjectParameter("ReturnOBISType", 1000000000000000);
                    FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                    returnUnitConvertType = new ObjectParameter("ReturnUnitConvertType",
                        1000000000000000);
                    ErrMSG = new ObjectParameter("ErrMSG", "");
                    Result = new ObjectParameter("Result", 10000000000000000);

                   
                        

                    var obj = HandHeldReader.OBISMapping.getStandardOBIS(readOut.Obis, readOut.Value);
                    if (obj.code == "0100000200FF")
                        softwareVersion = obj.value;

                    //Insert ReadOuts to ObisValueDetails
                    SQLSPS.INSOBISs("", obj.code, "", "", "", Convert.ToDecimal(1), "",
                        Convert.ToDecimal(1), "", 1, "", "", "",
                        FixedOBISCode, returnUnitConvertType, returnObisType, obisid, Result, ErrMSG);


                    try
                    {
                        if (FixedOBISCode.Value.ToString() != "")
                        {
                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) == 500 &&
                                CommonData.LanguagesID == 2)
                            {
                                readOut.Value = SaveDateValue(readOut.Value);
                            }
                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 900 &&
                                Convert.ToInt32(FixedOBISCode.Value.ToString()) < 925)
                            {
                                SaveConsumedActiveEnergy(clockValue, SelectedMeterID, FixedOBISCode,obisid, obisValueHeaderId, readOut.Value);
                            }
                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 800 &&
                                Convert.ToInt32(FixedOBISCode.Value.ToString()) < 825)
                            {

                                if (Convert.ToInt32(obisid.Value.ToString()) == 81)
                                {
                                    SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID,softwareVersion, FixedOBISCode, obisid, obisValueHeaderId, ConvertToMinutes(readOut.Value),"");
                                }
                                else
                                {
                                    SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID, softwareVersion,FixedOBISCode, obisid, obisValueHeaderId, readOut.Value, "");
                                }

                            }
                            if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 825 &&
                                Convert.ToInt32(FixedOBISCode.Value.ToString()) < 863)
                            {
                                //SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID,softwareVersion, FixedOBISCode, obisid, obisValueHeaderId, readOut.Value,"");

                            }

                        }
                        Result = new ObjectParameter("Result", 10000000000000000);
                        var ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        SQLSPS.InsobisValueDetail(Convert.ToDecimal(obisValueHeaderId.Value),
                            softwareVersion,
                            Convert.ToDecimal(obisid.Value), obj.value, null, "", ObisValueID, Result,
                            ErrMSG);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            #endregion ReadOuts

            #region Billing

            int ApartOBIS = 101;

            if (item.Billing.Bill != null && item.Billing.Bill.Count > 0)
            {
                double monthlyWater;
                ObjectParameter obisidw = new ObjectParameter("OBISID", 10000000000000000);
                ObjectParameter obisPomp = new ObjectParameter("OBISID", 10000000000000000);
                string pompObis;
                string waterCode;
                string myHex = ApartOBIS.ToString("X");
                string obisCode = "0802010000" + myHex;
                foreach (var water in item.Billing.Bill)
                {



                    myHex = ApartOBIS.ToString("X");



                    // Time of pump working Xst Month
                    pompObis = "0802606202" + myHex;

                    // Overall Consumed volume of water 
                    obisCode = "0802010000" + myHex;
                    Result = new ObjectParameter("Result", 10000000000000000);
                    obisid = new ObjectParameter("OBISID", 10000000000000000);
                    ErrMSG = new ObjectParameter("ErrMSG", "");
                    FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                    returnUnitConvertType = new ObjectParameter("ReturnUnitConvertType",
                        1000000000000000);
                    returnObisType = new ObjectParameter("ReturnOBISType", 1000000000000000);

                    SQLSPS.INSOBISs("", obisCode, "", "", "", 1, "", null, "", 1, "", "", "",
                        FixedOBISCode, returnUnitConvertType, returnObisType, obisid, Result, ErrMSG);

                    SQLSPS.INSOBISs("", pompObis, "", "", "", 1, "", null, "", 1, "", "", "",
                        FixedOBISCode, returnUnitConvertType, returnObisType, obisPomp, Result, ErrMSG);
                    Result = new ObjectParameter("Result", 10000000000000000);
                    var ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                    ErrMSG = new ObjectParameter("ErrMSG", "");

                    //Time of pump working Xst Month
                    Result = new ObjectParameter("Result", 10000000000000000);
                    ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                    ErrMSG = new ObjectParameter("ErrMSG", "");
                    SQLSPS.InsobisValueDetail(Convert.ToDecimal(obisValueHeaderId.Value),
                        softwareVersion, Convert.ToDecimal(obisPomp.Value), water.TimeOfPumpWorking,
                        null, "", ObisValueID, Result, ErrMSG);
                    // Xst Overall Consumed volume of water
                    SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID, softwareVersion,
                        FixedOBISCode, obisPomp, obisValueHeaderId, ConvertToMinutes(water.TimeOfPumpWorking), water.DateTime);

                    // Xst Overall Consumed volume of water
                    SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID, softwareVersion,
                        FixedOBISCode, obisid, obisValueHeaderId, water.WaterConsumption, water.DateTime);

                    // Xst Month Consumed Water
                    if (isFirstMonth)
                    {
                        PreWater = NumericConverter.DoubleConverter(water.WaterConsumption);
                        preDateTime = water.DateTime;
                        isFirstMonth = false;
                    }
                    else
                    {
                        myHex = (ApartOBIS - 1).ToString("X");
                        //Month Consumed Water
                        waterCode = "0802010100" + myHex;
                        SQLSPS.INSOBISs("", waterCode, "", "", "", 1, "", null, "", 1, "", "", "",
                        FixedOBISCode, returnUnitConvertType, returnObisType, obisidw, Result, ErrMSG);

                        monthlyWater = Math.Round(PreWater - NumericConverter.DoubleConverter(water.WaterConsumption), 2);

                        SaveConsumedWater(MeterNumber, clockValue, SelectedMeterID, softwareVersion,
                            FixedOBISCode, obisidw, obisValueHeaderId, monthlyWater.ToString(),
                            preDateTime);
                        PreWater = NumericConverter.DoubleConverter(water.WaterConsumption);
                        preDateTime = water.DateTime;
                    }
                    ApartOBIS++;
                }
            }

            #endregion Billing
        }


        public void SaveINDBReadValueFromHHU(string meterNumber, decimal meterId, ObjectParameter OBISValueHeaderID, string clockValue, string softwareVersion, MeterReading  meterReading)
        {
            try
            {
                //SabaNewEntities Bank = new SabaNewEntities();
                //Bank.Database.Connection.Open();
                
                var objectList = meterReading.ReadOuts.Object;
                ObjectParameter FixedOBISCode = new ObjectParameter("FixedOBISCode", "");
                ObjectParameter ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", "");
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter OBISID = new ObjectParameter("OBISID", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                ObjectParameter IsExits = new ObjectParameter("IsExits", false);
                ObjectParameter ObisValueID = new ObjectParameter("ObisValueID", 1000000000000000);
                ObjectParameter ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);

                
                if (CommonData.correctReading)
                {

                    if (!Convert.ToBoolean(SQLSPS.ShowClockObisValue(clockValue, (meterId))))
                    {
                        Result = new ObjectParameter("Result", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        
                        string transferDate = "";
                        

                        
                        var CurveList = new string[16];
                        for (int j = 0; j < objectList.Count; j++)
                        {
                            try
                            {

                                {
                                    if (objectList[j].Value.Contains("Reserved"))
                                        continue;
                                    if (objectList[j].Obis.Contains("1.0.2"))
                                        objectList[j].Obis = objectList[j].Obis;
                                    var obisObj = GetObisObject(objectList[j].Obis, objectList[j].Value);
                                    if (obisObj == null &&!objectList[j].Obis.Contains("0802010000"))
                                        continue;
                                    Result = new ObjectParameter("Result", 10000000000000000);
                                    OBISID = new ObjectParameter("OBISID", 10000000000000000);
                                    ErrMSG = new ObjectParameter("ErrMSG", "");
                                    ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", 1000000000000000);
                                    ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);
                                    SQLSPS.INSOBISs("", obisObj.code, "", "", "", 1, "", null, "", 1, "", "", "", FixedOBISCode, ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMSG);


                                    //if (objectList[j].Obis.ToLower().Contains("0802802800ff_"))
                                    //{
                                    //    SaveInCurveTable(objectList[j].Obis, objectList[j].Value, meterId.ToString(), OBISValueHeaderID.Value.ToString(),CurveList);
                                    //}



                                    if (FixedOBISCode.Value.ToString() != "")
                                    {
                                        if (Convert.ToInt32(FixedOBISCode.Value.ToString()) == 500 && CommonData.LanguagesID == 2)
                                        {
                                            objectList[j].Value = SaveDateValue(objectList[j].Value);
                                            obisObj.value = objectList[j].Value;
                                        }
                                        if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 900 && Convert.ToInt32(FixedOBISCode.Value.ToString()) < 925)
                                        {
                                            SaveConsumedActiveEnergy(clockValue, meterId, FixedOBISCode, OBISID, OBISValueHeaderID, objectList[j].Value.ToString());
                                        }
                                        if (Convert.ToInt32(FixedOBISCode.Value.ToString()) >= 800 && Convert.ToInt32(FixedOBISCode.Value.ToString()) < 825)
                                        {
                                            SaveConsumedWater207(clockValue, meterId, FixedOBISCode, OBISID,
                                                OBISValueHeaderID, obisObj.value.ToString());
                                        }
                                    }
                                    Result = new ObjectParameter("Result", 10000000000000000);
                                    ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                                    ErrMSG = new ObjectParameter("ErrMSG", "");
                                    if (obisObj.code == "0000010000FF_0.0.6")
                                    {
                                        SQLSPS.InsobisValueDetail(Convert.ToDecimal(OBISValueHeaderID.Value), softwareVersion, Convert.ToDecimal(OBISID.Value), clockValue, null, "",
                                        ObisValueID, Result, ErrMSG);
                                    }
                                    else
                                    {
                                        if (j > 0 && objectList[j].Obis.Contains("0.0.5") && objectList[j - 1].Obis.Contains("0.0.5"))
                                        {
                                            SQLSPS.INSOBISs("", "0100000405FF", "", "", "", 1, "", null, "", 1, "", "", "", FixedOBISCode, ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMSG);
                                            SQLSPS.InsobisValueDetail(Convert.ToDecimal(OBISValueHeaderID.Value), softwareVersion, Convert.ToDecimal(OBISID.Value), obisObj.value, null, "",
                                          ObisValueID, Result, ErrMSG);
                                        }
                                        else
                                        {
                                            SQLSPS.InsobisValueDetail(Convert.ToDecimal(OBISValueHeaderID.Value), softwareVersion, Convert.ToDecimal(OBISID.Value), obisObj.value, null, "",
                                              ObisValueID, Result, ErrMSG);
                                        }
                                    }

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        Result = new ObjectParameter("Result", 10000000000000000);
                        OBISID = new ObjectParameter("OBISID", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        ReturnUnitConvertType = new ObjectParameter("ReturnUnitConvertType", 1000000000000000);
                        ReturnOBISType = new ObjectParameter("ReturnOBISType", 1000000000000000);

                        SQLSPS.INSOBISs("", "0000", "", "", "", 1, "", null, "", 1, "", "", "", FixedOBISCode, ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMSG);

                        Result = new ObjectParameter("Result", 10000000000000000);
                        ObisValueID = new ObjectParameter("ObisValueID", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        SQLSPS.InsobisValueDetail(Convert.ToDecimal(OBISValueHeaderID.Value), softwareVersion, Convert.ToDecimal(OBISID.Value), DateTime.Now.ToPersianString(), clockValue, "", 
                            ObisValueID, Result, ErrMSG);

                        Result = new ObjectParameter("Result", 10000000000000000);
                        ErrMSG = new ObjectParameter("ErrMSG", "");
                        if (meterNumber.StartsWith("207"))
                            SQLSPS.InsCurve(Convert.ToDecimal(meterId), Convert.ToDecimal(OBISValueHeaderID.Value), CurveList[0], CurveList[1], CurveList[2], CurveList[3], CurveList[4]
                       , CurveList[5], CurveList[6], CurveList[7], CurveList[8], CurveList[9], CurveList[10], CurveList[11],
                       CurveList[12], CurveList[13], CurveList[14], CurveList[15], Result, ErrMSG);
                    }
                    //if (CommonData.SelectedMeterNumber.StartsWith("207"))
                    //    CommonData.mainwindow.Vee207data(CommonData.SelectedMeterID);

                }

                CommonData.mainwindow.changeProgressBarValue(1);

            }
            catch (Exception ex)
            {

                //System.Windows.MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        private OBISObject GetObisObject(string obis,string value)
        {
            return HandHeldReader.OBISMapping.getStandardOBIS(obis,value);            
        }

        public void SaveConsumedWater207(string clockValue, decimal meterId, ObjectParameter FixedOBISCode, ObjectParameter OBISID, ObjectParameter OBISValueHeaderID, string objectList)
        {
            try
            {

                DateTime pd = DateTime.Now;


                int dif = Convert.ToInt32(FixedOBISCode.Value.ToString()) - 800;
                DateTime cardDate = new DateTime();

                string[] str = clockValue.Split('/');

                if (CommonData.LanguagesID == 2)
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(Convert.ToInt32(str[0]), Convert.ToInt32(str[1]), Convert.ToInt32(str[2].Substring(0, 2)));
                }
                else
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(Convert.ToInt32(str[2].Substring(0, 4)),
                           Convert.ToInt32(str[0]), Convert.ToInt32(str[1].Substring(0, 2)));
                }
                if (dif == 0)
                {
                    pd = cardDate;
                }
                else
                {
                    int day = cardDate.Day;
                    pd = cardDate.AddDays(-day + 1);
                    //if (day >= 2)
                    //    pd = (PersianDate)calender.AddMonths(pd, -dif+1 );
                    //else
                    //    pd = (PersianDate)calender.AddMonths(pd, -dif );
                    pd = pd.AddMonths(-dif + 1);

                }
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");

                SQLSPS.INSConsumedWater(meterId, objectList, null, pd.ToPersianString(), clockValue,
                    Convert.ToDecimal(OBISID.Value), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), Convert.ToDecimal(OBISValueHeaderID.Value),
                    Result, ErrMSG);
            }

            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);

            }
        }

        public void SaveInCurveTable(string obissCode, string value, string meterID, string headerID , string[] CurveList)
        {
            try
            {

                switch (obissCode)
                {
                    case "0802802800FF_2.0.0":
                        CurveList[0] = value;
                        break;
                    case "0802802800FF_2.1.0":
                        CurveList[1] = value;
                        break;
                    case "0802802800FF_2.1.1":
                        CurveList[2] = value;
                        break;
                    case "0802802800FF_2.1.2":
                        CurveList[3] = value;
                        break;
                    case "0802802800FF_2.1.3":
                        CurveList[4] = value;
                        break;
                    case "0802802800FF_2.1.4":
                        CurveList[5] = value;
                        break;
                    case "0802802800FF_2.1.5":
                        CurveList[6] = value;
                        break;
                    case "0802802800FF_2.2.0":
                        CurveList[7] = value;
                        break;
                    case "0802802800FF_2.2.1":
                        CurveList[8] = value;
                        break;
                    case "0802802800FF_2.2.2":
                        CurveList[9] = value;
                        break;
                    case "0802802800FF_2.2.3":
                        CurveList[10] = value;
                        break;
                    case "0802802800FF_2.2.4":
                        CurveList[11] = value;
                        break;
                    case "0802802800FF_2.2.5":
                        CurveList[12] = value;
                        break;
                    case "0802802800FF_2.0.1":
                        CurveList[13] = value;
                        break;
                    case "0802802800FF_2.0.2":
                        CurveList[14] = value;
                        break;
                    case "0802802800FF_2.0.3":
                        CurveList[15] = value;
                        break;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        
        private string ConvertToMinutes(string timeOfPumpWorking)
        {
            timeOfPumpWorking = timeOfPumpWorking.Replace("/", ":");
            string[] hh = timeOfPumpWorking.Split(new char[] { ':' });
            return hh[0]+"."+hh[1];
        }

        public void SaveConsumedWater(string meterNumber, string clockValue, decimal? meterId, string softwareVersion,
            ObjectParameter FixedOBISCode, ObjectParameter OBISID, ObjectParameter OBISValueHeaderID, string value, string dateTime)
        {
            try
            {
                if (meterNumber.StartsWith("207"))
                    SaveConsumedWater207(clockValue, meterId, FixedOBISCode, OBISID, OBISValueHeaderID, value);
                else
                    SaveConsumedWater303(clockValue, meterId, softwareVersion, FixedOBISCode, OBISID, OBISValueHeaderID, value, dateTime);
            }
            catch (Exception ex)
            {

            }
        }

        public void SaveConsumedWater303(string clockValue, decimal? meterId, string softwareVersion, ObjectParameter fixedObisCode, ObjectParameter obisid, ObjectParameter obisValueHeaderId, string value, string dateTime)
        {
            try
            {               
                DateTime pden = new DateTime();
                PersianCalendar calender = new PersianCalendar();
                // int dif = Convert.ToInt32(FixedOBISCode.Value.ToString()) - 800;

                int year, month, day1;
                DateTime newdate = new DateTime();
                string[] str = clockValue.Split('/');
                 

                 
                if (CommonData.LanguagesID == 2)
                {
                    year = Convert.ToInt32(str[0]);
                    month = Convert.ToInt32(str[1]);
                    day1 = Convert.ToInt32(str[2].Substring(0, 2));
                    if (Convert.ToInt32(str[0]) > 50 && str[0].Length <= 2)
                        year = Convert.ToInt32(str[0]) + 1300;
                    else if (Convert.ToInt32(str[0]) < 50 && str[0].Length <= 2)
                        year = Convert.ToInt32(str[0]) + 1400;
                    PersianCalendar pc1 = new PersianCalendar();
                    if (clockValue.Contains(":"))
                    {
                        var i = clockValue.IndexOf(' ');
                        var dd = clockValue.Substring(i + 1);
                        string[] str1 = dd.Split(':');
                        newdate = PersianDateTime.ConvertToGeorgianDateTime(year, month, day1,Convert.ToInt32(str1[0]), Convert.ToInt32(str1[1]), Convert.ToInt32(str1[2]));
                    }
                    else
                    newdate = PersianDateTime.ConvertToGeorgianDateTime(year, month, day1);
                   
                }
                else
                {
                    year = Convert.ToInt32(str[2].Substring(0, 4));
                    month = Convert.ToInt32(str[0]);
                    day1 = Convert.ToInt32(str[1].Substring(0, 2)); if (clockValue.Contains(":"))
                    {
                        var i = clockValue.IndexOf(' ');
                        var dd = clockValue.Substring(i + 1);
                        string[] str1 = dd.Split(':');
                        newdate = PersianDateTime.ConvertToGeorgianDateTime(year, month, day1, Convert.ToInt32(str1[0]), Convert.ToInt32(str1[1]), Convert.ToInt32(str1[2]));
                    }
                    else
                        newdate = PersianDateTime.ConvertToGeorgianDateTime(year, month, day1);
                }

                //int day = newdate.Day;
                //pden = newdate.AddDays(20 - day);          
                //string dt = pden.Year + "/" + pden.Month + "/" + pden.Day;
                string dt = newdate.ToString("MM/dd/yyyy HH:mm:ss");   

                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");

                //if (softwareVersion.StartsWith("RSASEWM303") && NumericConverter.IntConverter(softwareVersion[softwareVersion.Length - 1].ToString()) > 3)
                if (softwareVersion.StartsWith("RSASEWM303") )
                {

                    if (!string.IsNullOrEmpty(dateTime) && (dateTime != "0001/01/01 00:00:00"))
                    {
                        //dateTime = dateTime.Substring(0, dateTime.IndexOf(' '));
                        SQLSPS.INSConsumedWater(meterId, value, null,dateTime , clockValue,
                            Convert.ToDecimal(obisid.Value), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            Convert.ToDecimal(obisValueHeaderId.Value), Result, ErrMSG);
                    }
                    else
                        SQLSPS.INSConsumedWater(meterId, value, null, dt, clockValue,
                            Convert.ToDecimal(obisid.Value), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            Convert.ToDecimal(obisValueHeaderId.Value), Result, ErrMSG);

                }
                else
                {
                    //MessageBox.Show(value);
                    SQLSPS.INSConsumedWater(meterId, value, null, dt, clockValue,
                        Convert.ToDecimal(obisid.Value), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        Convert.ToDecimal(obisValueHeaderId.Value), Result, ErrMSG);
                }

            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }
        }

        public void SaveConsumedWater207(string clockValue, decimal? meterId, ObjectParameter fixedObisCode, ObjectParameter obisid, ObjectParameter obisValueHeaderId, string objectList)
        {
            try
            {
                int dif = Convert.ToInt32(fixedObisCode.Value.ToString()) - 800;
                DateTime cardDate = new DateTime();
                DateTime pd = DateTime.Now;
                string[] str = clockValue.Split('/');

                if (CommonData.LanguagesID == 2)
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(
                        Convert.ToInt32(str[0]),
                        Convert.ToInt32(str[1]),
                        Convert.ToInt32(str[2].Substring(0, 2)));
                }
                else
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(
                        Convert.ToInt32(str[2].Substring(0, 4)),
                        Convert.ToInt32(str[0]),
                        Convert.ToInt32(str[1].Substring(0, 2)));
                }

                if (dif == 0)
                {
                    pd = cardDate;
                }
                else
                {
                    int day = cardDate.Day;
                    pd = cardDate.AddDays(-day + 1);
                    pd = pd.AddMonths(-dif + 1);
                }

               
               
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");

                SQLSPS.INSConsumedWater(meterId, objectList, null, pd.ToPersianString(), clockValue,
                    Convert.ToDecimal(obisid.Value), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), Convert.ToDecimal(obisValueHeaderId.Value),
                    Result, ErrMSG);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);

            }
        }

        public void SaveConsumedActiveEnergy(string clockValue, decimal? meterId, ObjectParameter FixedOBISCode, ObjectParameter OBISID, ObjectParameter OBISValueHeaderID, string objectList)
        {
            ObjectParameter errMsg = new ObjectParameter("ErrMSG", "");
            ObjectParameter result = new ObjectParameter("Result", 10000000000000000);
            try
            {
                DateTime pd = DateTime.Now;
                DateTime cardDate;

                int dif = Convert.ToInt32(FixedOBISCode.Value.ToString()) - 900;
              
                string[] str = clockValue.Split('/');

                if (CommonData.LanguagesID == 2)
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(
                        Convert.ToInt32(str[0]),
                        Convert.ToInt32(str[1]),
                        Convert.ToInt32(str[2].Substring(0, 2)));
                }
                else
                {
                    cardDate = PersianDateTime.ConvertToGeorgianDateTime(
                        Convert.ToInt32(str[2].Substring(0, 4)),
                        Convert.ToInt32(str[0]),
                        Convert.ToInt32(str[1].Substring(0, 2)));
                }
                if (dif == 0)
                {
                    pd = cardDate;
                }
                else
                {
                    int day = cardDate.Day;
                    pd = cardDate.AddDays(-day + 1);
                    pd = pd.AddMonths(-dif + 1);
                }
                SQLSPS.INSConsumedActiveEnergy(meterId, objectList,pd.ToPersianString(), DateTime.Now.ToPersianString(), Convert.ToDecimal(OBISID.Value), clockValue, Convert.ToDecimal(OBISValueHeaderID.Value), result, errMsg);
            }

            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }

        public string SaveDateValue(string OBISValue)
        {
            
            

            var d = OBISValue.Split(new char[] { '/', ' ', '-', ':' });
            
            if (d.Length < 3)
                return OBISValue;


            string NewValue = OBISValue;
            try
            {
                if (OBISValue.StartsWith("9") && OBISValue[2] == '/')
                    NewValue = "13" + OBISValue;
                if (OBISValue.Contains("ب.ظ") || OBISValue.Contains("ق.ظ"))
                {
                    NewValue = RsaDateTime.PersianDateTime.ConvertTo24HTime(OBISValue);
                }
                else if (OBISValue.Length > 7)
                {
                    if (d.Length > 2)
                    {
                        if (d[0].Length == 4)
                        {
                            if (d.Length >= 6)
                                return new DateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), Convert.ToInt32(d[3]), Convert.ToInt32(d[4]), Convert.ToInt32(d[5])).ToPersianDateString();
                            else
                                return new DateTime(Convert.ToInt32(d[0]), Convert.ToInt32(d[1]), Convert.ToInt32(d[2]), 0, 0, 0).ToPersianDateString();
                        }
                    }
                    DateTime cardDate = Convert.ToDateTime(OBISValue);
                    NewValue = cardDate.ToPersianString();
                }
                
            }
            catch (Exception ex)
            {


                //MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
            return NewValue;
        }

    
        private void changeEnable(bool IsEnable)
        {
            try
            {
                btnOk.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(
               delegate ()
               {
                   btnOk.IsEnabled = IsEnable;
               }));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }
        }

    }
}
