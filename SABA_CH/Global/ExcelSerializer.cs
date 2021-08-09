using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using MeterStatus;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SABA_CH.DataBase;
using FontStyle = System.Drawing.FontStyle;

namespace SABA_CH.Global
{
    public class ShowOBISValueDetail
    {
        
        public ICollectionView CollectShowObisValueDetail { get; private set; }
        public List<ShowOBISValueDetail_Result> _lstShowOBISValueDetail;
        public new SabaNewEntities Bank = new SabaNewEntities();
       
        public ShowOBISValueDetail(string filter, decimal? languageId)
        {
            Bank.Database.Connection.Open();
            _lstShowOBISValueDetail = new List<ShowOBISValueDetail_Result>();
            foreach (ShowOBISValueDetail_Result item in Bank.ShowOBISValueDetail(filter, languageId, CommonData.UserID))
                _lstShowOBISValueDetail.Add(item);
            CollectShowObisValueDetail = CollectionViewSource.GetDefaultView(_lstShowOBISValueDetail);
            Bank.Database.Connection.Close();
            Bank.Dispose();
        }
    }
    public class ExcelSerializer
    {
        public int UsedRows;
        public ShowTranslateofLable tr;
        public void Serialize(string filePath, List<ShowOBISValueDetail_Result> registers, string sheetsName, string meterNumber, string meterdatetime, int sheetNumber)
        {            
            if (registers.Count == 0)
                return;           
            var newFile = new FileInfo(filePath);
            string excelTemplateFilePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + @"\Template.xlsx";
            var templateFile = new FileInfo(excelTemplateFilePath); 
           
            using (var package = new ExcelPackage(newFile))
            {
                var worksheet = package.Workbook.Worksheets[sheetNumber];
               // ExcelWorksheet worksheet = CopyWorkSheet(filePath, SheetsName);
                package.Workbook.Worksheets.Add(sheetsName, worksheet);

                worksheet.Cells["E5"].Value = meterNumber;
                worksheet.Cells["E6"].Value = meterdatetime;                

                CreateRegisterRows(worksheet, registers.Count);

                for (int i = 0; i < registers.Count; i++)
                    WriteRegisterValues(i, registers[i], worksheet);

                for (int i = 6; i <= 6; i++)
                    worksheet.Column(i).AutoFit();
                worksheet.Cells["D13:F13"].AutoFilter = true;                
                package.Workbook.Properties.Company = "@RSA Electronics Co.";
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "DLMS Client");
                package.Save();
            }
        }
        public void CopyWorkSheet(string filePath, string[] SheetsName,string[] obisType ,string meterNumber, string meterdatetime)
        {
            try
            {
                string excelTemplateFilePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + @"\Template.xlsx";
                var newFile = new FileInfo(filePath);
                var templateFile = new FileInfo(excelTemplateFilePath);
                using (var package = new ExcelPackage(newFile, templateFile))
                {
                    var worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells["E5"].Value = meterNumber;
                    worksheet.Cells["E6"].Value = meterdatetime;
                    //worksheet.Name = SheetsName[0];
                    string filter = "";
                    ShowOBISValueDetail detail = null;
                    List<ShowOBISValueDetail_Result> registers = null;
                    for (int i = 1; i < SheetsName.Length; i++)
			        {
                        string sheetsName = SheetsName[i];
                        var ws= package.Workbook.Worksheets.Add(sheetsName, worksheet);
                        filter = "  and (charindex('\"" + obisType[i] + "\"',obiss.type)>0    or charindex('\"100\"',obiss.type)>0) and Header.MeterID=1";
                        detail = new ShowOBISValueDetail(filter, 2);
                        registers = detail._lstShowOBISValueDetail;
                        CreateRegisterRows(ws, registers.Count);
                        for (int j = 0; j < registers.Count; j++)
                            WriteRegisterValues(j, registers[j], ws);

                        for (int j = 6; j <= 6; j++)
                            ws.Column(j).AutoFit();
                        ws.Cells["D13:F13"].AutoFilter = true;
			        }
                    filter = "  and (charindex('\"" + obisType[0] + "\"',obiss.type)>0    or charindex('\"100\"',obiss.type)>0) and Header.MeterID=1";
                    detail = new ShowOBISValueDetail(filter, 2);
                    registers = detail._lstShowOBISValueDetail;
                    CreateRegisterRows(worksheet, registers.Count);

                    for (int j = 0; j < registers.Count; j++)
                        WriteRegisterValues(j, registers[j], worksheet);

                    for (int j = 6; j <= 6; j++)
                        worksheet.Column(j).AutoFit();
                    worksheet.Cells["D13:F13"].AutoFilter = true;
                    worksheet.Name = SheetsName[0];
                    package.Workbook.Properties.Company = "@RSA Electronics Co.";
                    package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "DLMS Client");
                    package.Save();
                }
                //return worksheet;
            }
            catch (Exception ex)
            {
                //return null;
            }           
            
        }
        public void CopyWorkSheet(string filePath, string[] SheetsName, string[] obisType, string [] meterNumber, string meterdatetime,string [] meterId)
        {
            try
            {
                int usedRows = 14;
                string ExcelTemplateFilePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + @"\Template.xlsx";
                var newFile = new FileInfo(filePath);
                var templateFile = new FileInfo(ExcelTemplateFilePath);
                using (var package = new ExcelPackage(newFile, templateFile))
                {
                    string Filter = "";
                    ShowOBISValueDetail detail = null;
                    List<ShowOBISValueDetail_Result> registers = null;
                    var worksheet = package.Workbook.Worksheets[1];
                    for (int i = 1; i < SheetsName.Length; i++)
                    {
                        string sheetsName = SheetsName[i];
                        usedRows = 14;
                        var ws = package.Workbook.Worksheets.Add(sheetsName, worksheet);
                        for (int K = 0; K < meterId.Length; K++)
                        {
                            CreateSplitRows(ws, usedRows + 1, meterNumber[K]);
                            usedRows = usedRows + 2;
                            Filter = "  and (charindex('\"" + obisType[i] + "\"',obiss.type)>0    or charindex('\"100\"',obiss.type)>0) and Header.MeterID= " + meterId[K];
                            detail = new ShowOBISValueDetail(Filter, 2);
                            registers = detail._lstShowOBISValueDetail;
                            CreateRegisterRows(ws, registers.Count + 2, usedRows + 1);
                            for (int j = 0; j < registers.Count; j++)
                                WriteRegisterValues(j, registers[j], ws, usedRows + 1);

                            for (int j = 6; j <= 6; j++)
                                ws.Column(j).AutoFit();
                            ws.Cells["D13:F13"].AutoFilter = true;
                            usedRows = usedRows + registers.Count;
                        }
                        ws.DeleteRow(14, 1);
                    }
                    Filter = "  and (charindex('\"" + obisType[0] + "\"',obiss.type)>0    or charindex('\"100\"',obiss.type)>0) and Header.MeterID=1";
                    detail = new ShowOBISValueDetail(Filter, 2);
                    registers = detail._lstShowOBISValueDetail;
                    CreateRegisterRows(worksheet, registers.Count);

                    for (int j = 0; j < registers.Count; j++)
                        WriteRegisterValues(j, registers[j], worksheet);

                    for (int j = 6; j <= 6; j++)
                        worksheet.Column(j).AutoFit();
                    worksheet.Cells["D13:F13"].AutoFilter = true;
                    worksheet.Name = SheetsName[0];
                    package.Workbook.Properties.Company = "@RSA Electronics Co.";
                    package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "DLMS Client");
                    package.Save();
                }
                //return worksheet;
            }
            catch (Exception ex)
            {
                //return null;
            }


        }
        public void CopyWorkSheetConsumed(string filePath, string[] SheetsName, string[] obisType, string[] meterNumber, string meterdatetime, string[] meterId,string obissId)
        {
            try
            {
                string Filter = "";
                ShowOBISValueDetail detail = null;
                List<ShowOBISValueDetail_Result> registers = null;
                tr = CommonData.translateWindow(5);
                UsedRows = 14;
                string excelTemplateFilePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + @"\Template.xlsx";
                var newFile = new FileInfo(filePath);
                var templateFile = new FileInfo(excelTemplateFilePath);
                using (var package = new ExcelPackage(newFile, templateFile))
                {                   
                    var worksheet = package.Workbook.Worksheets[1];
                    for (int i = 1; i < SheetsName.Length; i++)
                    {
                        string sheetsName = SheetsName[i];
                        UsedRows = 14;
                        var ws = package.Workbook.Worksheets.Add(sheetsName, worksheet);
                        for (int K = 0; K < meterId.Length; K++)
                        {
                            CreateSplitRows(ws, UsedRows + 1, meterNumber[K]);
                            UsedRows = UsedRows + 2;
                            if (obisType[i] != "8" && obisType[i]!="2")
                            {
                                Filter = "  and (charindex('\"" + obisType[i] + "\"',obiss.type)>0    or charindex('\"100\"',obiss.type)>0) and Header.MeterID= " + meterId[K] + " and OBISs.OBISID in (" + obissId + " ) and Header.TransferDate=(Select Max(TransferDate) From OBISValueHeader where MeterID=" + meterId[K] + ") ";
                                detail = new ShowOBISValueDetail(Filter, 2);
                                registers = detail._lstShowOBISValueDetail;
                                CreateWorksheets(ws, registers, UsedRows);
                                UsedRows = UsedRows + registers.Count;

                            }
                            else if (obisType[i] == "2")
                            {

                                CreateWorksheetsStatus(ws, obissId, meterId[K], UsedRows);

                            }
                            else
                            {
                                CreateConsumedworksheets(ws, meterId[K], UsedRows);
                                UsedRows = UsedRows + registers.Count;
                            }
                           
                            ws.Cells["D13:F13"].AutoFilter = true;
                            
                            
                        }
                        ws.DeleteRow(14, 1);
                    }
                    FillWorksheet0(worksheet, obisType[0], SheetsName[0], meterId, meterNumber);
                    worksheet.Name = SheetsName[0];
                    package.Workbook.Properties.Company = "@RSA Electronics Co.";
                    package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "DLMS Client");
                    package.Save();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }


        }
        public void CopyWorkSheetConsumedwithReadDate(string filePath, string[] SheetsName, string[] obisType, string meterNumber, string meterdatetime, string meterId, string obissId,string[] readDate)
        {
            try
            {
                string Filter = "";
                ShowOBISValueDetail detail = null;
                List<ShowOBISValueDetail_Result> registers = null;
                tr = CommonData.translateWindow(5);
                UsedRows = 14;
                string excelTemplateFilePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + @"\Template.xlsx";
                var newFile = new FileInfo(filePath);
                var templateFile = new FileInfo(excelTemplateFilePath);
                using (var package = new ExcelPackage(newFile, templateFile))
                {
                    var worksheet = package.Workbook.Worksheets[1];
                    for (int i = 1; i < SheetsName.Length; i++)
                    {
                        string sheetsName = SheetsName[i];
                        UsedRows = 14;
                        var ws = package.Workbook.Worksheets.Add(sheetsName, worksheet);
                        for (int K = 0; K < readDate.Length; K++)
                        {
                            CreateSplitRows(ws, UsedRows + 1, readDate[K]);
                            UsedRows = UsedRows + 2;
                            if (obisType[i] != "8" && obisType[i] != "10" && obisType[i] != "2")
                            {
                                Filter = "  and (charindex('\"" + obisType[i] + "\"',obiss.type)>0    or charindex('\"100\"',obiss.type)>0) and Header.MeterID= " + meterId + " and OBISs.OBISID in (" + obissId + " ) and Header.TransferDate='" + readDate[K] + "' ";
                                detail = new ShowOBISValueDetail(Filter, 2);
                                registers = detail._lstShowOBISValueDetail;
                                CreateWorksheets(ws, registers, UsedRows);
                                UsedRows = UsedRows + registers.Count;

                            }
                            else if (obisType[i] == "2")
                            {

                                CreateWorksheetsStatus(ws, obissId, meterId, UsedRows);

                            }
                            else
                            {
                                CreateConsumedworksheets(ws, meterId, UsedRows);
                                UsedRows = UsedRows + registers.Count;
                            }

                            ws.Cells["D13:F13"].AutoFilter = true;


                        }
                        ws.DeleteRow(14, 1);
                    }
                    FillWorksheet0WithReadOut(worksheet, obisType[0], SheetsName[0], meterId, meterNumber,readDate);
                    worksheet.Name = SheetsName[0];
                    package.Workbook.Properties.Company = "@RSA Electronics Co.";
                    package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "DLMS Client");
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }


        }
        public void CreateWorksheets(ExcelWorksheet ws, List<ShowOBISValueDetail_Result> registers, int usedRows)
        {
            try 
	        {	
                CreateRegisterRows(ws, registers.Count + 2, usedRows + 1);
                for (int j = 0; j < registers.Count; j++)
                    WriteRegisterValues(j, registers[j], ws, usedRows + 1);

                for (int j = 6; j <= 6; j++)
                    ws.Column(j).AutoFit(); 
	        }
	        catch (Exception ex)
	        {
		
		        MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
	        }
        }
        
        
        #region Consumedwoksheets
        public void CreateConsumedworksheets(ExcelWorksheet ws, string MeterID, int usedRows)
        {
            try
            {
                string Filter = " and Main.MeterID=" + MeterID;
                DataTable dt = new DataTable();
                dt = GetDataTableForElecReport(Filter);
                List<ReverseConsumedOBIS> _lstConsumedWater = new List<ReverseConsumedOBIS>();
                _lstConsumedWater=ExecuteElecReport(dt);
                CreateRegisterRows(ws, _lstConsumedWater.Count + 2, usedRows + 1);
                for (int j = 0; j < _lstConsumedWater.Count; j++)
                    WriteRegisterValues(j, _lstConsumedWater[j], ws, usedRows);
                if (_lstConsumedWater.Count>0)
                {
                    UpdateHeaderConsumedworksheets(ws, _lstConsumedWater[0]);
                }
                for (int j = 6; j <= 6; j++)
                    ws.Column(j).AutoFit(); 

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public void UpdateHeaderConsumedworksheets(ExcelWorksheet ws,ReverseConsumedOBIS reg)
        {
            try
            {
                ws.Cells["D13"].Value = "Date";
                ws.Cells["E13"].Value = reg.Column0Name.ToString();
                ws.Cells["F13"].Value = reg.Column1Name.ToString();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public DataTable GetDataTableForElecReport(string Filter)
        {
            DataTable dt = new DataTable();
            try
            {
                ShowConsumedWater_Result row = null;
                ShowConsumedWater_Result re = new ShowConsumedWater_Result();
                Type t = re.GetType();
                MemberInfo[] members = t.GetMembers(BindingFlags.NonPublic |
                BindingFlags.Instance);
                foreach (MemberInfo member in members)
                {
                    DataColumn dc = new DataColumn();
                    int start = member.Name.ToString().IndexOf('<');
                    int end = member.Name.ToString().IndexOf('>');
                    int len = member.Name.ToString().Length;
                    if (start >= 0)
                    {
                        dc.ColumnName = (member.Name.ToString().Substring(start + 1, end - start - 1));
                        dt.Columns.Add(dc);
                    }

                }
                //  add each of the data rows to the table
                SabaNewEntities Bank = new SabaNewEntities();

                Bank.Database.Connection.Open();
                foreach (var item in Bank.ShowConsumedWater(Filter, 2,CommonData.UserID))
                {
                    DataRow dr;
                    dr = dt.NewRow();
                    row = item;
                    dr["ConsumedWater"] = row.ConsumedWater.ToString();
                    dr["OBISDesc"] = row.OBISDesc.ToString();
                    dr["ConsumedDate"] = row.ConsumedDate.ToString();
                    dr["MeterID"] = row.MeterID.ToString();
                    dr["MeterNumber"] = row.MeterNumber.ToString();
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
            return dt;
        }
        public List<ReverseConsumedOBIS> ExecuteElecReport(DataTable dt)
        {
            
            try
            {
                string x = "OBISDesc";// "VEEValue";
                string y = "ConsumedDate";
                string z = "ConsumedWater";
                DataTable newDt = new DataTable();
                newDt = PivotTable.GetInversedDataTable(dt, x, y, z, "-", false);                
                return DataTable2List(newDt);
               

            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.ToString());
                return null;
               
            }
            
        }

        public List<ReverseConsumedOBIS> DataTable2List(DataTable dt)
        {
            List<ReverseConsumedOBIS> _lstConsumedWater = new List<ReverseConsumedOBIS>();
            try
            {
              
               DataColumn dc = dt.Columns[0];
               foreach (DataRow item in dt.Rows)
               {
                   ReverseConsumedOBIS newrow=new ReverseConsumedOBIS();
                   newrow.RowName=item[0].ToString();
                   newrow.Column0Value = item[1].ToString();
                   newrow.Column1Value = item[2].ToString();
                   newrow.Column0Name = dt.Columns[1].ColumnName.ToString();
                   newrow.Column1Name = dt.Columns[2].ColumnName.ToString();
                   _lstConsumedWater.Add(newrow);
               }

            }
            catch (Exception ex )
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
            return _lstConsumedWater;
        }
        #endregion Consumedwoksheets
        #region CreateStatusWorksheets
        private void CreateWorksheetsStatus(ExcelWorksheet worksheet, string OBISIDs, string MeterID, int rownumber)
        {
            try
            {
                string OBISs = CreateOBISsByOBISID(OBISIDs);
                
                MeetrsStatus MS = CreateListOfStatusValue(OBISs);
                foreach (Statusvalue item in MS.MeetrsList)
                {
                    
                    CreateSplitRows(worksheet, UsedRows+1, item.StatusDesc);
                    UsedRows = UsedRows + 2;
                    CreateRegisterRows(worksheet, item.List.Count+2,UsedRows+1);
                    for (int i = 0; i < item.List.Count; i++)
                    {

                        WriteRegisterValues(i, item.List[i], worksheet, UsedRows + 1);                       

                    }
                    UsedRows = UsedRows + item.List.Count;
                    

                }


            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        public string CreateOBISsByOBISID(string OBISIDs)
        {
            string OBISs = "";
            try
            {
                string Filter = " and Main.OBISID in (" + OBISIDs + ")";
                ShowOBISs showobis = new ShowOBISs(Filter);
                foreach (var item in showobis._lstShowOBISs)
                {
                    OBISs = OBISs + item.Obis + ",";
                }
                if (OBISs.Length > 0)
                    OBISs = OBISs.Substring(0, OBISs.Length - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
            return OBISs;
        }
        public MeetrsStatus CreateListOfStatusValue(string OBISs)
        {
            MeetrsStatus MS = new MeetrsStatus();
            try
            {
                string Value1 = "";
                string Value2 = "";
                string Value3 = "";
                string[] OBIDS;
                OBIDS = OBISs.Split(',');
                _303 Obj303 = new _303();
                List<Status_Result> List = new List<Status_Result>();
                
                if (OBISs.Contains("0000603F00FF") || OBISs.Contains("0000603F01FF") || OBISs.Contains("0000603F02FF"))
                {
                    try
                    {
                        Obj303 = new _303();
                        List = new List<Status_Result>();
                        Statusvalue sv = new Statusvalue();
                        Value1 = RefreshDatagridMeterStatus("0000603F00FF");
                        Value2 = RefreshDatagridMeterStatus("0000603F01FF");
                        Value3 = RefreshDatagridMeterStatus("0000603F02FF");
                        List = Obj303.PerformanceMeteroncreditevents(Value1.PadLeft(8, '0'), Value2.PadLeft(8, '0'), Value3.PadLeft(8, '0'), CommonData.LanguageName);
                        sv.StatusDesc = tr.TranslateofLable.Object49;
                        sv.List = List;
                        MS.MeetrsList.Add(sv);
                    }
                    catch { }
                }
                if (OBISs.Contains("0000600302FF"))
                {
                    try
                    {
                        string Value = "";
                        Obj303 = new _303();
                        List = new List<Status_Result>();
                        Statusvalue sv = new Statusvalue();
                        Value = RefreshDatagridMeterStatus("0000600302FF");
                        List = Obj303.statusRegister("0000600302FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        sv.StatusDesc = tr.TranslateofLable.Object48;
                        sv.List = List;
                        MS.MeetrsList.Add(sv);
                    }
                    catch { }
                }
                if (OBISs.Contains("0802606101FF"))
                {
                    try
                    {
                        string Value = "";
                        Obj303 = new _303();
                        List = new List<Status_Result>();
                        Statusvalue sv = new Statusvalue();
                        Value = RefreshDatagridMeterStatus("0802606101FF");
                        List = Obj303.statusRegister("0802606101FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        sv.StatusDesc = tr.TranslateofLable.Object47;
                        sv.List = List;
                        MS.MeetrsList.Add(sv);
                    }
                    catch { }


                }
                if (OBISs.Contains("0000600404FF"))
                {
                    try
                    {
                        string Value = "";
                        Obj303 = new _303();
                        List = new List<Status_Result>();
                        Statusvalue sv = new Statusvalue();
                        Value = RefreshDatagridMeterStatus("0000600404FF");
                        List = Obj303.statusRegister("0000600404FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        sv.StatusDesc = tr.TranslateofLable.Object46;
                        sv.List = List;
                        MS.MeetrsList.Add(sv);
                    }
                    catch { }
                }
                if (OBISs.Contains("0000603E04FF"))
                {
                    try
                    {
                        string Value = "";
                        Obj303 = new _303();
                        List = new List<Status_Result>();
                        Statusvalue sv = new Statusvalue();
                        Value = RefreshDatagridMeterStatus("0000603E04FF");
                        List = Obj303.statusRegister("0000603E04FF", Value.PadLeft(8, '0'), CommonData.LanguageName);
                        sv.StatusDesc = tr.TranslateofLable.Object50;
                        sv.List = List;
                        MS.MeetrsList.Add(sv);
                    }
                    catch { }
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
            return MS;
        }
        public string RefreshDatagridMeterStatus(string OBIS)
        {
            try
            {
                string Value = "";
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                string Filter = " and Header.MeterID=" + CommonData.SelectedMeterID + "  and Header.ReadDate=(Select Max(ReadDate) From OBISValueHeader where MeterID=" + CommonData.SelectedMeterID + ")" + "  and OBISs.OBIS='" + OBIS + "'";
                ShowOBISValueDetail EGeneralvalue = new ShowOBISValueDetail(Filter, CommonData.LanguagesID);
                foreach (ShowOBISValueDetail_Result item in Bank.ShowOBISValueDetail(Filter, CommonData.LanguagesID, CommonData.UserID))
                    Value = item.Value;
                Bank.Database.Connection.Close();
                return Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
                return "";
            }
        }
        #endregion CreateStatusWorksheets
        private void FillWorksheet0(ExcelWorksheet worksheet, string obisType, string sheetsName, string[] meterId, string[] meterNumber)
        {
            try
            {
                ShowOBISValueDetail detail = null;
                List<ShowOBISValueDetail_Result> registers = null;                
               
                int usedRows = 14;
                for (int K = 0; K < meterId.Length; K++)
                {
                    CreateSplitRows(worksheet, usedRows + 1, meterNumber[K]);
                    usedRows = usedRows + 2;
                    if (obisType != "8" && obisType!= "10")
                    {
                        string Filter = "  and (charindex('\"" + obisType + "\"',obiss.type)>0    or charindex('\"100\"',obiss.type)>0) and Header.MeterID= " + meterId[K];
                        detail = new ShowOBISValueDetail(Filter, 2);
                        registers = detail._lstShowOBISValueDetail;
                        CreateWorksheets(worksheet, registers, usedRows);

                    }
                    else
                    {
                        CreateConsumedworksheets(worksheet, meterId[K], usedRows);
                    }

                    worksheet.Cells["D13:F13"].AutoFilter = true;
                    usedRows = usedRows + registers.Count;

                }
                worksheet.DeleteRow(14, 1);
                worksheet.Name = sheetsName;

            }
            catch
            {
                // ignored
            }
        }
        private void FillWorksheet0WithReadOut(ExcelWorksheet worksheet, string obisType, string sheetsName, string meterId, string meterNumber,string[] readOut)
        {
            try
            {
                ShowOBISValueDetail detail = null;
                List<ShowOBISValueDetail_Result> registers = null;

                int usedRows = 14;
                for (int K = 0; K < readOut.Length; K++)
                {
                    CreateSplitRows(worksheet, usedRows + 1, readOut[K]);
                    usedRows = usedRows + 2;
                    if (obisType != "8" && obisType != "10")
                    {
                        string filter = "  and (charindex('\"" + obisType + "\"',obiss.type)>0    or charindex('\"100\"',obiss.type)>0) and Header.MeterID= " + meterId;
                        detail = new ShowOBISValueDetail(filter, 2);
                        registers = detail._lstShowOBISValueDetail;
                        CreateWorksheets(worksheet, registers, usedRows);

                    }
                    else
                    {
                        CreateConsumedworksheets(worksheet, meterId, usedRows);
                    }

                    worksheet.Cells["D13:F13"].AutoFilter = true;
                    usedRows = usedRows + registers.Count;

                }
                worksheet.DeleteRow(14, 1);
                worksheet.Name = sheetsName;

            }
            catch (Exception)
            {


            }
        }
        private void CreateRegisterRows(ExcelWorksheet worksheet, int count)
        {
            if (count == 1)
                worksheet.DeleteRow(14, 1);
            else if (count > 2)
            {
                worksheet.InsertRow(15, count - 2, 14);
                for (int i = 15; i < count + 13; i++)
                    worksheet.Row(i).Height = worksheet.Row(14).Height;
            }
        }
        private void CreateRegisterRows(ExcelWorksheet worksheet, int count,int rowFrom)
        {
            if (count == 1)
                worksheet.DeleteRow(rowFrom-1, 1);
            else if (count > 2)
            {
                worksheet.InsertRow(rowFrom, count - 2, 14);
                for (int i = rowFrom; i < count + 13; i++)
                    worksheet.Row(i).Height = worksheet.Row(14).Height;
            }
        }

        private void CreateSplitRows(ExcelWorksheet worksheet, int rownumber,string meterSerialNumber)
        {
           
            worksheet.InsertRow(rownumber, 2, 14);
            worksheet.Cells["C" + rownumber].Value = meterSerialNumber;
            int newrow = rownumber + 1;
            using (ExcelRange r = worksheet.Cells["C" + rownumber + ":F" + newrow])
            {
                if(r.Merge!=true)
                    r.Merge = true;
                r.Style.Font.SetFromFont(new Font("Tahoma", 22, FontStyle.Regular));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.DarkOrange);
            }
            
           
           
        }
        private void WriteRegisterValues(int i, ShowOBISValueDetail_Result regob, ExcelWorksheet worksheet)
        {
            string indexString = (14 + i).ToString();               
            worksheet.Cells["C" + indexString].Value = i + 1;
            worksheet.Cells["D" + indexString].Value = regob.ObisDesc;
            worksheet.Cells["E" + indexString].Value = regob.Value;
            worksheet.Cells["F" + indexString].Value = regob.OBISUnitDesc;
           
        }
        private void WriteRegisterValues(int i, ReverseConsumedOBIS regob, ExcelWorksheet worksheet, int rowFrom)
        {
            string indexString = (rowFrom + i).ToString();
            worksheet.Cells["C" + indexString].Value = i + 1;
            worksheet.Cells["D" + indexString].Value = regob.RowName;
            worksheet.Cells["E" + indexString].Value = regob.Column0Value;
            worksheet.Cells["F" + indexString].Value = regob.Column1Value;

        }

        private void WriteRegisterValues(int i, ShowOBISValueDetail_Result regob, ExcelWorksheet worksheet, int rowFrom)
        {
            string indexString = (rowFrom + i).ToString();
            worksheet.Cells["C" + indexString].Value = i + 1;
            worksheet.Cells["D" + indexString].Value = regob.ObisDesc;
            worksheet.Cells["E" + indexString].Value = regob.Value;
            worksheet.Cells["F" + indexString].Value = regob.OBISUnitDesc;

        }
        private void WriteRegisterValues(int i, Status_Result regob, ExcelWorksheet worksheet, int rowFrom)
        {
            string indexString = (rowFrom + i).ToString();
            worksheet.Cells["C" + indexString].Value = i + 1;
            worksheet.Cells["D" + indexString].Value = regob.Description;
            if(regob.IsStatuseTrue.ToString().ToUpper()=="TRUE")
                worksheet.Cells["E" + indexString].Value = true;
            else if (regob.IsStatuseTrue.ToString().ToUpper() == "FALSE")
                worksheet.Cells["E" + indexString].Value = false;

        }
        private void CreateTemplate(string  filePath)
        {
            try
            {                
                var newFile = new FileInfo(filePath);
                using (var package = new ExcelPackage(newFile))
                {                   
                    var worksheet = package.Workbook.Worksheets[1];
                    
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());         CommonData.WriteLOG(ex);
            }
        }
        
    }
}
