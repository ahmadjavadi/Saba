using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Font = Microsoft.Office.Interop.Excel.Font;
using FontStyle = System.Drawing.FontStyle;
using RsaDateTime;

namespace SABA_CH.Global
{

    class ExcelReport<T, U>
        where T : class
        where U : List<T>
    {
        public int usedRows = 0;
        public object[] header;
        public object[] Subheader;
        public object[] headerText;
        public object[] SubheaderText;

        // Excel object references.
        private _Worksheet _sheet = null;
        private Range _range = null;
        private Font _font = null;

        // Optional argument variable
        public ShowTranslateofLable tr;
        private object _optionalValue = Missing.Value;
        public void CreateExcelFile(string filePath)
        {
            try
            {
                string ExcelTemplateFilePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + @"\Template.xlsx";
                var newFile = new FileInfo(filePath);
                var templateFile = new FileInfo(ExcelTemplateFilePath);
                var package = new ExcelPackage(newFile, templateFile);
                CommonData.ExPackage = package;

            }
            catch (Exception)
            {               
                           
                
            }
        }
        public ExcelWorksheet CreateWorkSheet()
        {            
            var worksheet = CommonData.ExPackage.Workbook.Worksheets[1];
            return worksheet;
        }
       
        private void CreateRegisterRows(ExcelWorksheet worksheet, int count, int RowFrom)
        {
            if (count == 1)
                worksheet.DeleteRow(RowFrom - 1, 1);
            else if (count > 2)
            {
                worksheet.InsertRow(RowFrom, count , 14);
                for (int i = RowFrom; i < count + 13; i++)
                    worksheet.Row(i).Height = worksheet.Row(14).Height;
            }
        }
        public void CreateWorksheetsnew(ExcelWorksheet ws, List<T> registers, int usedRows)
        {
            try
            {
                tr = CommonData.translateWindow(5);
                CreateRegisterRows(ws, registers.Count, usedRows);

                ////
                List<T> dataToPrint;
                dataToPrint = registers;
                //object[] header = CreateHeader();


                object[,] objData = new object[dataToPrint.Count, header.Length];

                for (int j = 0; j < dataToPrint.Count; j++)
                {
                    var item = dataToPrint[j];
                    for (int i = 0; i < header.Length; i++)
                    {
                        var y = typeof(T).InvokeMember(header[i].ToString(), BindingFlags.GetProperty, null, item, null);
                        objData[j, i] = (y == null) ? "" : y.ToString();
                        ws.Cells[usedRows + j, 3 + i].Value = objData[j, i];
                        //ws.Cells[14 + j, 3 + i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ExcelRange r2 = ws.Cells[usedRows + j, 3 + i, usedRows + j, 3 + i];
                        ColorConverter color = new ColorConverter();
                        r2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r2.Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FFF1F7E8"));
                    }
                }
                //setAllRowsColor(ws, registers, header);
                SeHeaderText(ws, headerText);
                for (int j = 3; j <= 4; j++)
                    ws.Column(j).AutoFit();
                ws.Cells[13, 3, 13, 3 + header.Length].AutoFilter = true;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void SeHeaderText(ExcelWorksheet ws, object[] header)
        {
            for (int i = 0; i < header.Length; i++)
            {
                ws.Cells[13, i + 3].Value = header[i].ToString();
               // ws.Cells[13, i + 3].AutoFitColumns();
                ws.Cells[13, 3 + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ExcelRange r2 = ws.Cells[13, 3, 13, 3 + header.Length];
                ColorConverter color = new ColorConverter();
                ws.Row(13).Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Row(13).Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FF555550"));
            }
        }
        public void SetMeterNumberandDatetime(ExcelWorksheet ws, string MeterNumber, string ReadDateTime)
        {
            try
            {
                ws.Cells[5, 5].Value = MeterNumber;
                ws.Cells[6, 5].Value = ReadDateTime;

                if (CommonData.LanguagesID == 2)
                    ws.Cells[7, 5].Value = DateTime.Now.ToPersianString();
                else
                    ws.Cells[7, 5].Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        public void setAllRowsColor(ExcelWorksheet ws, int fromrow,int torow, object[] header)
        {
            ExcelRange r1 = ws.Cells[fromrow, 1, torow, 1 + header.Length+2];
            ColorConverter color = new ColorConverter();
            r1.Style.Fill.PatternType = ExcelFillStyle.Solid;
            r1.Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FFF1F7E8"));
            r1.Style.Border.BorderAround(ExcelBorderStyle.Medium);
            //r1.AutoFitColumns();

            //ws.Column(5).Collapsed = true;
            //ExcelRange r2 = ws.Cells[13, 3, 13, 3 + header.Length];
            //System.Drawing.ColorConverter color2 = new System.Drawing.ColorConverter();
            //r2.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //r2.Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FF555550"));


            //if (3 + header.Length > 6)
            //{
            //    ExcelRange r3 = ws.Cells[1, 7, 12, 3 + header.Length];
            //    System.Drawing.ColorConverter color3 = new System.Drawing.ColorConverter();
            //    r3.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //    r3.Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FFF2F2F2"));

            //}
        }
        
        public void CreateSplitRows(ExcelWorksheet worksheet, int rownumber, string MeterSerialNumber,string SourceTypeName)
        {
            
            worksheet.InsertRow(rownumber, 2, 14);
            worksheet.Cells["C" + rownumber].Value = MeterSerialNumber +"  "+ "source:" + SourceTypeName;
            int newrow = rownumber + 1;

            using (ExcelRange r = worksheet.Cells["C" + rownumber + ":F" + newrow])
            {
                if (r.Merge != true)
                    r.Merge = true;
                r.Style.Font.SetFromFont(new System.Drawing.Font("Tahoma", 14, FontStyle.Regular));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.DarkOrange);

            }
        }
        public void CreateSplitRowsRorReadDate(ExcelWorksheet worksheet, int rownumber, string ReadDate)
        {

            worksheet.InsertRow(rownumber, 2, 14);
            worksheet.Cells["C" + rownumber].Value = ReadDate;
            int newrow = rownumber + 1;

            using (ExcelRange r = worksheet.Cells["C" + rownumber + ":F" + newrow])
            {
                if (r.Merge != true)
                    r.Merge = true;
                r.Style.Font.SetFromFont(new System.Drawing.Font("Tahoma", 14, FontStyle.Regular));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.DarkOrange);

            }
        }
        public void DeleteWorkSheet()
        {
            CommonData.ExPackage.Workbook.Worksheets.Delete(1);
        }
        public void SaveExcelFile()
        {
            CommonData.ExPackage.Workbook.Properties.Company = "@RSA Electronics Co.";
            CommonData.ExPackage.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "SabaCandH");
            CommonData.ExPackage.Save();
            
        }
        public void OpenExcelFile( string path)
        {
            
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                Process.Start(path);
            }
            else
            {
                //file doesn't exist
            }


            
        }
       
    }
}
