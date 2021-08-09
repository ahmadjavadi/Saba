using HandHeldReader;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using VEE;
using System.Linq;

namespace HHUReaderTest
{
    public partial class Form1 : Form
    {
       
        VEE.Vee_207 _207;

        public Form1()
        {  
            InitializeComponent();

            txtMeterNo.Text = "20714442";
            txtMeterNo.Text = "20718592";
        }

        private void btnReadHHU_Click(object sender, EventArgs e)
        {            
            this.Enabled = false;
            
            HHUReadOut hhureadout = HandHeldReader.HandHeldReader.PortableDeviceReader();            

            if (HandHeldReader.HandHeldReader.FindHhuReadOut)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(hhureadout.GetType());
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, hhureadout);
                string s = textWriter.ToString();
                txtResult.Text = s;
            }
            else
                txtResult.Text = "HHU Error Code: "+ hhureadout.ErrorCode;
            this.Enabled = true;
        }


        private void btnReadCard_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Card.CardManager cm = new Card.CardManager();
            Card.CardReadOut cr = cm.getCardData();

            XmlSerializer xmlSerializer = new XmlSerializer(cr.GetType());
            StringWriter textWriter = new StringWriter();

            xmlSerializer.Serialize(textWriter, cr);
            string s = textWriter.ToString();
            txtResult.Text = s;
            this.Enabled = true;
        }

        private void btnNewCard_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Card.CardManager cm = new Card.CardManager();
            string cardNumber = "";
            txtResult.Text = "Write City Code Result: " + cm.WriteCityCodeOncard("00000000").ToString();
            if (txtMeterNo.Text.StartsWith("207"))
                txtResult.Text = "Write New Card 207 Result: " + cm.WriteNew207Card("123456", "10001234", ref cardNumber).ToString() + " CardNO=" + cardNumber;
            else
                txtResult.Text = "Write New Card 303 Result: " + cm.WriteNew303Card("RSASEWM303001104", "10001234", ref cardNumber).ToString() + " CardNO=" + cardNumber;
            this.Enabled = true;
        }


        //=========================================================================== VEE +++++++++++++++++++++++++++++
        public List<VEE.ShowConsumedWaterForVee_Result> ShowConsumedWaterForVee(string meterNo)
        {
            List<VEE.ShowConsumedWaterForVee_Result> _lstConsumedWaterForVee_Result = new List<VEE.ShowConsumedWaterForVee_Result>();
            SabaCandHTestGEntities Bank = new SabaCandHTestGEntities();
            Bank.Database.Connection.Open();

            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 2000;
            foreach (ShowConsumedWaterForVee_Result item in Bank.ShowConsumedWaterForVee("and MeterNumber='" + meterNo + "'"))
                _lstConsumedWaterForVee_Result.Add(ConvertForVee(item));
            Bank.Database.Connection.Close();
            Bank.Dispose();
            return _lstConsumedWaterForVee_Result;
        }

        public List<VEE.ShowVEEConsumedWaterForVEE_Result> ShowVEEConsumedWaterForVEE(string meterNo)
        {
            List<VEE.ShowVEEConsumedWaterForVEE_Result> _lstVEEConsumedWaterForVEE_Result = new List<VEE.ShowVEEConsumedWaterForVEE_Result>();
            SabaCandHTestGEntities Bank = new SabaCandHTestGEntities();

            Bank.Database.Connection.Open();
            ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 2000;
            foreach (ShowVEEConsumedWaterForVEE_Result item in Bank.ShowVEEConsumedWaterForVEE("and MeterNumber='" + meterNo + "'"))
            {
                _lstVEEConsumedWaterForVEE_Result.Add(ConvertForVee(item));
            }
            Bank.Database.Connection.Close();
            Bank.Dispose();
            return _lstVEEConsumedWaterForVEE_Result;
        }

        public VEE.ShowVEEConsumedWaterForVEE_Result ConvertForVee(ShowVEEConsumedWaterForVEE_Result item)
        {
            VEE.ShowVEEConsumedWaterForVEE_Result data = new VEE.ShowVEEConsumedWaterForVEE_Result();
            data.MeterNumber = item.MeterNumber;
            data.TotalWater1 =  item.TotalWater1.ToString();
            data.ReadDate = item.ReadDate;
            data.Flow1 =  item.Flow1.ToString();
            data.Flow2 =  item.Flow2.ToString();
            data.Flow3 =  item.Flow3.ToString();
            data.Flow4 =  item.Flow4.ToString();
            data.Flow5 =  item.Flow5.ToString();
            data.Flow6 =  item.Flow6.ToString();
            data.Flow7 =  item.Flow7.ToString();
            data.Flow8 =  item.Flow8.ToString();
            data.Flow9 =  item.Flow9.ToString();
            data.Flow10 =  item.Flow10.ToString();
            data.Flow11 =  item.Flow11.ToString();
            data.Flow12 =  item.Flow12.ToString();
            data.Flow13 =  item.Flow13.ToString();
            data.Flow14 =  item.Flow14.ToString();
            data.Flow15 =  item.Flow15.ToString();
            data.Flow16 =  item.Flow16.ToString();
            data.Flow17 =  item.Flow17.ToString();
            data.Flow18 =  item.Flow18.ToString();
            data.Flow19 =  item.Flow19.ToString();
            data.Flow20 =  item.Flow20.ToString();
            data.Flow21 =  item.Flow21.ToString();
            data.Flow22 =  item.Flow22.ToString();
            data.Flow23 =  item.Flow23.ToString();
            data.Flow24 =  item.Flow24.ToString();

            data.W1 =  item.W1.ToString();
            data.W2 =  item.W2.ToString();
            data.W3 =  item.W3.ToString();
            data.W4 =  item.W4.ToString();
            data.W5 =  item.W5.ToString();
            data.W6 =  item.W6.ToString();
            data.W7 =  item.W7.ToString();
            data.W8 =  item.W8.ToString();
            data.W9 =  item.W9.ToString();
            data.W10 =  item.W10.ToString();
            data.W11 =  item.W11.ToString();
            data.W12 =  item.W12.ToString();
            data.W13 =  item.W13.ToString();
            data.W14 =  item.W14.ToString();
            data.W15 =  item.W15.ToString();
            data.W16 =  item.W16.ToString();
            data.W17 =  item.W17.ToString();
            data.W18 =  item.W18.ToString();
            data.W19 =  item.W19.ToString();
            data.W20 =  item.W20.ToString();
            data.W21 =  item.W21.ToString();
            data.W22 =  item.W22.ToString();
            data.W23 =  item.W23.ToString();
            data.W24 =  item.W24.ToString();
            return data;
        }

        public VEE.ShowConsumedWaterForVee_Result ConvertForVee(ShowConsumedWaterForVee_Result item)
        {
            VEE.ShowConsumedWaterForVee_Result data = new VEE.ShowConsumedWaterForVee_Result();
            data.MeterNumber = item.MeterNumber;
            data.TotalWater1 = item.TotalWater1.ToString();
            data.ReadDate = item.ReadDate;
            data.Flow1 = item.Flow1;
            data.Flow2 = item.Flow2;
            data.Flow3 = item.Flow3;
            data.Flow4 = item.Flow4;
            data.Flow5 = item.Flow5;
            data.Flow6 = item.Flow6;
            data.Flow7 = item.Flow7;
            data.Flow8 = item.Flow8;
            data.Flow9 = item.Flow9;
            data.Flow10 = item.Flow10;
            data.Flow11 = item.Flow11;
            data.Flow12 = item.Flow12;
            data.Flow13 = item.Flow13;
            data.Flow14 = item.Flow14;
            data.Flow15 = item.Flow15;
            data.Flow16 = item.Flow16;
            data.Flow17 = item.Flow17;
            data.Flow18 = item.Flow18;
            data.Flow19 = item.Flow19;
            data.Flow20 = item.Flow20;
            data.Flow21 = item.Flow21;
            data.Flow22 = item.Flow22;
            data.Flow23 = item.Flow23;
            data.Flow24 = item.Flow24;

            data.W1 = item.W1;
            data.W2 = item.W2;
            data.W3 = item.W3;
            data.W4 = item.W4;
            data.W5 = item.W5;
            data.W6 = item.W6;
            data.W7 = item.W7;
            data.W8 = item.W8;
            data.W9 = item.W9;
            data.W10 = item.W10;
            data.W11 = item.W11;
            data.W12 = item.W12;
            data.W13 = item.W13;
            data.W14 = item.W14;
            data.W15 = item.W15;
            data.W16 = item.W16;
            data.W17 = item.W17;
            data.W18 = item.W18;
            data.W19 = item.W19;
            data.W20 = item.W20;
            data.W21 = item.W21;
            data.W22 = item.W22;
            data.W23 = item.W23;
            data.W24 = item.W24;
            return data;
        }

        private void btnVEE_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            _207 = new VEE.Vee_207((List<VEE.ShowConsumedWaterForVee_Result>)ShowConsumedWaterForVee(txtMeterNo.Text),
                (List<VEE.ShowVEEConsumedWaterForVEE_Result>)ShowVEEConsumedWaterForVEE(txtMeterNo.Text));
            txtResult.Text = _207.Meter_Consumption_Data.VEE_Total_WaterDataString();
            txtMonthly.Text = _207.Meter_Consumption_Data.VEE_Monthly_Consumption_WaterDataString();
            txtOrginal.Text = _207.Meter_Consumption_Data.OrginalWaterDataString();
            this.Enabled = true;
        }

        private void btnExportVEE2Excell_Click(object sender, EventArgs e)
        {
            //this.Enabled = false;
            SaveWaterConsumptionData();
          //  this.Enabled = true;
        }

        internal void SaveWaterConsumptionData()
        {

            if (_207 == null)
                return;
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.FileName = "Consumption.xlsx";
            savedialog.Filter = "*.xlsx|*.xlsx|All File|*.*";
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    if (xlApp == null)
                    {
                        MessageBox.Show("Excel is not properly installed!!");
                        return;
                    }

                    Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                    object misValue = System.Reflection.Missing.Value;

                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    xlWorkSheet.DisplayRightToLeft = false;

                    int colCount = 2;
                    int rowCount = 2;

                    foreach (var item in _207.Meter_Consumption_Data.OrginalWaterDataList)
                    {
                        xlWorkSheet.Rows[rowCount].Interior.Color = System.Drawing.Color.Olive;
                        xlWorkSheet.Rows[rowCount + 1].Interior.Color = System.Drawing.Color.Orange;

                        xlWorkSheet.Cells[rowCount, colCount++] = item.Reading_Date;
                        xlWorkSheet.Cells[rowCount, colCount++] = item.TotalWater;

                        foreach (var water_data in item.water_data_Reading_List)
                        {
                            xlWorkSheet.Cells[rowCount, colCount] = water_data.Water_Consumption;
                            xlWorkSheet.Cells[rowCount + 1, colCount] = water_data.Month_Maximum_Debi;
                            colCount++;
                        }
                        rowCount += 3;
                        colCount = 2;
                    }

                    rowCount += 3;
                    colCount = 2;

                    foreach (var item in _207.Meter_Consumption_Data.Valid_Monthly_Consumption_Data_List)
                    {
                        xlWorkSheet.Rows[rowCount].Interior.Color = System.Drawing.Color.Olive;
                        xlWorkSheet.Rows[rowCount + 1].Interior.Color = System.Drawing.Color.Orange;

                        xlWorkSheet.Cells[rowCount, colCount++] = item.Reading_Date;
                        xlWorkSheet.Cells[rowCount, colCount++] = item.TotalWater;

                        foreach (var water_data in item.water_data_Reading_List)
                        {
                            xlWorkSheet.Cells[rowCount, colCount] = water_data.Water_Consumption;
                            xlWorkSheet.Cells[rowCount + 1, colCount] = water_data.Month_Maximum_Debi;
                            colCount++;
                        }
                        rowCount += 3;
                        colCount = 2;
                    }

                    rowCount += 3;
                    colCount = 2;

                    foreach (var item in _207.Meter_Consumption_Data.Valid_Total_Consumption_Data_List)
                    {
                        xlWorkSheet.Rows[rowCount].Interior.Color = System.Drawing.Color.LightGreen;
                        xlWorkSheet.Rows[rowCount + 1].Interior.Color = System.Drawing.Color.LightSkyBlue;

                        xlWorkSheet.Cells[rowCount, colCount++] = item.Reading_Date;
                        xlWorkSheet.Cells[rowCount, colCount++] = item.TotalWater;

                        foreach (var water_data in item.water_data_Reading_List)
                        {
                            xlWorkSheet.Cells[rowCount, colCount] = water_data.Water_Consumption;
                            xlWorkSheet.Cells[rowCount + 1, colCount] = water_data.Month_Maximum_Debi;
                            colCount++;
                        }
                        rowCount += 3;
                        colCount = 2;
                    }

                    xlWorkBook.SaveAs(savedialog.FileName,
                        Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, false, false,
                        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlUserResolution,
                        true, misValue, misValue, misValue);


                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    releaseObject(xlWorkSheet);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);

                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = true;
                    excelApp.Workbooks.Open(savedialog.FileName);



                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.Message);
                }
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        //============================================================================================================================


        private void btnCredit207_Click(object sender, System.EventArgs e)
        {
            this.Enabled = false;
            try
            {                
                Card.CardManager cm = new Card.CardManager();
                txtResult.Text = "Write Credit To Card 207 Result: " + cm.WriteCreditTo207Card(txtCredit.Text, txtCreditEndDate.Text, txtErrorControl.Text, txtMeterNo.Text).ToString();
            }
            catch (Exception)
            {

            }
            this.Enabled = true;
        }

        private void btncredit303_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            try
            {
                Card.CardManager cm = new Card.CardManager();
                txtResult.Text = "Write Credit To Card 303 Result: " + cm.WriteTokenTo303Card(txtToken.Text,txtMeterNo.Text,true,
                    1,DateTime.Now,1,1).ToString();
            }
            catch (Exception)
            {

            } 
            this.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtResult.Clear();
            txtOrginal.Clear();
            txtMonthly.Clear();
        }

        private void btnExcelTest_Click(object sender, EventArgs e)
        {
            OpenFileDialog o1 = new OpenFileDialog();
            o1.Filter = "Excel File|*.xlsx|All File|*.*";
            if (o1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if (!o1.FileName.Contains(".xlsx"))
                        return;


                    // The delegate to call.
                    ThreadStart ts = delegate() { SendSpecialOBISToModem(o1.FileName); };

                    // The thread.
                    Thread t = new Thread(ts);

                    // Run the thread.
                    t.IsBackground = true;
                    t.Start();
                }
                catch { }
            }
        }

        private void SendSpecialOBISToModem(string fileName)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlApp.Visible = false;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(fileName, 0, false, 5, "", "", false,
                Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);   //@"H:\TestFile.xlsx"
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets.get_Item(1);

                Microsoft.Office.Interop.Excel.Range rng = xlWorkSheet.UsedRange;

                int colCount = rng.Columns.Count;
                int rowCount = rng.Rows.Count;
                rng = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[rowCount, colCount];
                Microsoft.Office.Interop.Excel.Range newColumn = rng.EntireColumn;

                List<Modem> modems = new List<Modem>();

                for (int i = 2; i < rowCount; i++)
                {
                    if (xlWorkSheet.Cells[i, 1].Value == null)
                        break;
                    if (xlWorkSheet.Cells[i, 1].Value.ToString().Length == 8 && (
                          xlWorkSheet.Cells[i, 1].Value.ToString().StartsWith("532") ||
                          xlWorkSheet.Cells[i, 1].Value.ToString().StartsWith("305")))
                    {

                        modems.Add(new Modem(xlWorkSheet.Cells[i, 1].Value.ToString(), xlWorkSheet.Cells[i, 2].Value.ToString()));
                    }
                    //
                }
                foreach (var item in modems)
                {
                    for (int i = 2; i < rowCount; i++)
                    {
                        if (xlWorkSheet.Cells[i, 3].Value == null)
                            break;
                        item.requestList.Add(new Request(xlWorkSheet.Cells[i, 3].Value.ToString(),
                            xlWorkSheet.Cells[i, 4].Value.ToString(),
                            xlWorkSheet.Cells[i, 5].Value.ToString()));

                        ObisCommandType obisCommandType = (ObisCommandType)Convert.ToInt32(xlWorkSheet.Cells[i, 6].Value.ToString());
                        bool b = Convert.ToBoolean(xlWorkSheet.Cells[i, 5].Value.ToString());

                    }
                }

                xlWorkBook.Close();
                xlApp.Quit();
            }
            catch { }
        }

        private void btnReadAllCardData_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Card.CardManager cm = new Card.CardManager();
            string[] s = cm.getAllCardData();

            txtResult.Lines = s;
            //if (s != null)
            //    foreach (var item in s)
            //    {
            //        if(item != null)
            //            txtResult.AppendText(item+"\r\n");
            //    }  
            this.Enabled = true;
        }

        private void btnClearCard_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            try
            {
                Card.CardManager cm = new Card.CardManager();
                if (cm.ClearCard() == 1)
                    txtResult.Text = "ClearCard: SUCCESS";
                else
                    txtResult.Text = "ClearCard: Error";
            }
            catch
            {
                txtResult.Text = "ClearCard: Error";
            }
            this.Enabled = true;
        } 

        private void btnFormatCard_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            try
            {
                UInt64 cn = UInt64.Parse(txtCardNumber.Text);

                string card_number = cn.ToString("X8");

                Card.CardManager cm = new Card.CardManager();
                if (cm.FormatCard(card_number) == 1)
                    txtResult.Text = "FormatCard: SUCCESS";
                else
                    txtResult.Text = "FormatCard: Error";
            }
            catch
            {
                txtResult.Text = "Invalid Card Number";
            }
            this.Enabled = true;
        }

        private void btnConvertCardData_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (txtResult.Lines.Length > 0) 
            {
                Card.CardManager cm = new Card.CardManager();

                foreach (var item in txtResult.Lines)
                {
                    if (item.StartsWith("==")||item.StartsWith("FF")||item.Contains("Error"))
                        continue;
                    String pattern = @",\s";
                    String[] elements = System.Text.RegularExpressions.Regex.Split(item, pattern);
                 string[]  datttttttt = cm. GetConvertedData(elements);
                 if (datttttttt[1].Length>1)
                     txtOrginal.Text += datttttttt[1].PadRight(20,' ') + " , " + datttttttt[0] + "\r\n";    
                }
            }
            this.Enabled = true;
            
        }

        private void btnWriteData2Card_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (txtResult.Lines.Length > 0)
            {
                Card.CardManager cm = new Card.CardManager();

                foreach (var item in txtResult.Lines)
                {
                    if (item.StartsWith("==") || item.StartsWith("FF") || item.Contains("Error"))
                        continue;
                    String pattern = @",\s";
                    String[] elements = System.Text.RegularExpressions.Regex.Split(item, pattern);
                    cm.WriteData2Card(elements);
                }
                cm.WriteCityCodeOncard("00000000");
            }
            this.Enabled = true;
        }

        private void btnWriteCityCode_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (txtCityCode.Text.Length == 8)
            {
                Card.CardManager cm = new Card.CardManager();
                cm.WriteCityCodeOncard(txtCityCode.Text);
            }
            this.Enabled = true;
        }

        private void btnHHu303_Click(object sender, EventArgs e)
        {                   
            //HHU303.HHU303.folderName = @"c:\PortableDevice";
          var data =  HHU303.HHU303.PortableDeviceReader();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void btnWriteToken_Click(object sender, EventArgs e)
        {
            Card.CardManager cm = new Card.CardManager();
            cm.WriteTokenTo303Card("03FD7E06396F0D01002191C007E007110C0D2007E008120C0D2003875F396A784914D10000000000", "1949400007897", true, 1, DateTime.Now, 1, 1);
        }

        private void btnResetCityCode_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Card.CardManager cm = new Card.CardManager();
            cm.WriteCityCodeOncard("00000000");
            this.Enabled = true;
        }
    }
         

    public enum ObisCommandType
    {
        Read = 1, Write = 2, Execution = 3
    }

   public  class Modem
    {
        public Modem(string modemUId, string SimNo)
        {
            this.Modemuid = modemUId;
            this.SimNo = SimNo;
        }
        public string Modemuid  { get; set; }
        public string  SimNo { get; set; }
        public List<Request> requestList = new List<Request>();

    }
   public  class Request
    {
        public string OBIS { get; set; }
        public string Value { get; set; }
        public string  Type { get; set; }

        public Request(string obis, string value, string type)
        {
            this.OBIS = obis;
            this.Value = value;
            this.Type = type;
        }
    }
}