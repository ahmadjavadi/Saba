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

namespace SABA_CH.Global
{
    class ExcelSerializernew<T, SubT, U>
        where T : class
        where SubT : class
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
        private object _optionalValue = Missing.Value;
        public void CreateWorkSheet(string filePath, string SheetsName, string MeterNumber, List<T> registers, string MeterDateTime, bool withsublist,string sourcetypename)
        {
            try
            {
                string ExcelTemplateFilePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + @"\Template.xlsx";
                var newFile = new FileInfo(filePath);
                var templateFile = new FileInfo(ExcelTemplateFilePath);
                using (var package = new ExcelPackage(newFile, templateFile))
                {
                    var worksheet = package.Workbook.Worksheets[1];
                    string sheetsName = SheetsName;
                    usedRows = 14;
                    if (withsublist)
                        CreateWorksheetsnewwithsublist(worksheet, registers, usedRows);
                    else
                        CreateWorksheetsnew(worksheet, registers, usedRows);
                    SetMeterNumberandDatetime(worksheet, MeterNumber, MeterDateTime);
                    SetSourceTypeName(worksheet, sourcetypename);
                    //worksheet.DeleteRow(14, 1);

                    worksheet.Name = SheetsName;
                    package.Workbook.Properties.Company = "@RSA Electronics Co.";
                    package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "Saba C and H");
                    package.Save();
                    OpenExcelFile(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void SetMeterNumberandDatetime(ExcelWorksheet ws, string MeterNumber, string ReadDateTime)
        {
            try
            {
                ws.Cells[5, 5].Value = MeterNumber;

                ws.Cells[6, 5].Value = ReadDateTime;

                if (CommonData.LanguagesID == 2)
                    ws.Cells[7, 5].Value = DateTime.Now.ToPersianString();
                else
                    ws.Cells[7, 5].Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                ExcelRange r1 = ws.Cells[5, 5, 7, 5];
                r1.AutoFitColumns();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void SetSourceTypeName(ExcelWorksheet ws, string SourceTypeName)
        {
            try
            {
                ws.Cells[9, 6].Value = SourceTypeName;

                ws.Cells[9, 5].Value = "Source";

                
                ExcelRange r1 = ws.Cells[9, 5,9, 6];
                r1.AutoFitColumns();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }
        private void copyrange(ExcelWorksheet wsdest, ExcelWorksheet wssource)
        {

        }
        private object[] CreateHeader()
        {
            PropertyInfo[] headerInfo = typeof(T).GetProperties();

            // Create an array for the headers and add it to the
            // worksheet starting at cell A1.
            List<object> objHeaders = new List<object>();
            for (int n = 0; n < headerInfo.Length; n++)
            {
                objHeaders.Add(headerInfo[n].Name);
            }

            var headerToAdd = objHeaders.ToArray();
            //AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
            //SetHeaderStyle();

            return headerToAdd;
        }
        private void SetHeaderStyle()
        {
            _font = _range.Font;
            _font.Bold = true;
        }
        public void CreateWorksheetsnew(ExcelWorksheet ws, List<T> registers, int usedRows)
        {
            try
            {
                CreateRegisterRows(ws, registers.Count);

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
                        ws.Cells[14 + j, 3 + i].Value = objData[j, i];
                        ws.Cells[14 + j, 3 + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                }
                setAllRowsColor(ws, registers, header);
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
        public void CreateSplitRows(ExcelWorksheet worksheet, int rownumber, string MeterSerialNumber)
        {

            worksheet.InsertRow(rownumber, 2, 14);
            worksheet.Cells["C" + rownumber].Value = MeterSerialNumber;
            int newrow = rownumber + 1;

            using (ExcelRange r = worksheet.Cells["C" + rownumber + ":F" + newrow])
            {
                if (r.Merge != true)
                    r.Merge = true;
                r.Style.Font.SetFromFont(new System.Drawing.Font("Tahoma", 22, FontStyle.Regular));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.DarkOrange);

            }
        }
        public void CreateWorksheetsnewwithsublist(ExcelWorksheet ws, List<T> registers, int usedRows)
        {
            try
            {
                //CreateRegisterRows(ws, registers.Count);

                ////
                List<T> dataToPrint;
                dataToPrint = registers;
                //object[] header = CreateHeader();


                object[,] objData = new object[dataToPrint.Count, header.Length];
                int rownumber = 14;
                for (int j = 0; j < dataToPrint.Count; j++)
                {
                    //rownumber = rownumber + j;
                    var item = dataToPrint[j];
                    for (int i = 0; i < header.Length; i++)
                    {

                        var y = typeof(T).InvokeMember(header[i].ToString(), BindingFlags.GetProperty, null, item, null);
                        objData[j, i] = (y == null) ? "" : y.ToString();

                        if (y is List<SubT>)
                        {
                            List<SubT> sublist = (List<SubT>)y;

                            SetSubHeaderText(ws, SubheaderText, rownumber + 1);

                            for (int K = 0; K < sublist.Count; K++)
                            {
                                var subitem = sublist[K];
                                for (int M = 0; M < Subheader.Length; M++)
                                {

                                    var z = typeof(SubT).InvokeMember(Subheader[M].ToString(), BindingFlags.GetProperty, null, subitem, null);
                                    ws.Cells[rownumber + 2 + K, 3 + M].Value = (z == null) ? "" : z.ToString();
                                    ws.Cells[rownumber + 2 + K, 3 + M].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Row(rownumber + 2 + K).Height = ws.Row(14).Height;
                                    ws.Row(rownumber).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ColorConverter color = new ColorConverter();
                                    ws.Row(rownumber).Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FFF0F0F0"));

                                }
                                ws.Row(rownumber + 2 + K).OutlineLevel = 1;

                            }
                            ExcelRange r1 = ws.Cells[rownumber + 1, 3, rownumber + sublist.Count + 1, 3 + Subheader.Length];

                            r1.Style.Border.BorderAround(ExcelBorderStyle.Dotted);
                            rownumber = rownumber + sublist.Count + 2;
                        }
                        else
                        {
                            ws.Cells[rownumber, 3 + i].Value = objData[j, i];
                            ws.Row(rownumber).OutlineLevel = 0;
                            ws.Row(rownumber).Height = ws.Row(14).Height;
                            ws.Row(rownumber).Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ColorConverter color = new ColorConverter();
                            ws.Row(rownumber).Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FFF1F7E8"));

                            ws.Cells[rownumber, 3 + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }

                    }
                }
                // setAllRowsColor(ws, registers, header);
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
        private void setAllRowsColor(ExcelWorksheet ws, List<T> registers, object[] header)
        {
            ExcelRange r1 = ws.Cells[14, 3, 14 + registers.Count, 3 + header.Length];
            ColorConverter color = new ColorConverter();
            r1.Style.Fill.PatternType = ExcelFillStyle.Solid;
            r1.Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FFF1F7E8"));
            r1.Style.Border.BorderAround(ExcelBorderStyle.Medium);
            r1.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 10, FontStyle.Bold));
            //r1.AutoFitColumns();

            ws.Column(5).Collapsed = true;
            ExcelRange r2 = ws.Cells[13, 3, 13, 3 + header.Length];
            ColorConverter color2 = new ColorConverter();
            r2.Style.Fill.PatternType = ExcelFillStyle.Solid;
            r2.Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FF555550"));


            if (3 + header.Length > 6)
            {
                ExcelRange r3 = ws.Cells[1, 7, 12, 3 + header.Length];
                ColorConverter color3 = new ColorConverter();
                r3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                r3.Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FFF2F2F2"));

            }
        }
        private void SetSubHeaderText(ExcelWorksheet ws, object[] Subheader, int rownumber)
        {
            for (int i = 0; i < SubheaderText.Length; i++)
            {
                ws.Cells[rownumber, i + 4].Value = SubheaderText[i].ToString();
                ws.Cells[rownumber, i + 4].AutoFitColumns();
                ws.Row(rownumber).OutlineLevel = 1;
                ws.Row(rownumber).Height = ws.Row(14).Height;
                ws.Row(rownumber).Style.Fill.PatternType = ExcelFillStyle.Solid;
                ColorConverter color = new ColorConverter();
                ws.Row(rownumber).Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FFFF8F0E"));
                ws.Cells[rownumber, 3 + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
        }
        private void SeHeaderText(ExcelWorksheet ws, object[] header)
        {
            for (int i = 0; i < header.Length; i++)
            {
                ws.Cells[13, i + 3].Value = header[i].ToString();
                //ws.Cells[13, i + 3].AutoFitColumns();
                ws.Cells[13, 3 + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ExcelRange r2 = ws.Cells[13, 3, 13, 3 + header.Length];
                ColorConverter color = new ColorConverter();
               r2.Style.Fill.PatternType = ExcelFillStyle.Solid;
               r2.Style.Fill.BackgroundColor.SetColor((Color)color.ConvertFromString("#FF555550"));
            }
        }
        private void AddExcelRows(ExcelWorksheet ws, List<T> registers, string startRange, int rowCount, int colCount, object values)
        {
            foreach (var obj in registers)
            {

            }

        }
        private void AddExcelRows(string startRange, int rowCount, int colCount, object values)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.set_Value(_optionalValue, values);
        }
        private void WriteRegisterValuesnew(int i, ReverseConsumedOBIS regob, ExcelWorksheet worksheet, int RowFrom)
        {
            string indexString = (RowFrom + i).ToString();
            worksheet.Cells["C" + indexString].Value = i + 1;
            worksheet.Cells["D" + indexString].Value = regob.RowName;
            worksheet.Cells["E" + indexString].Value = regob.Column0Value;
            worksheet.Cells["F" + indexString].Value = regob.Column1Value;

        }
        //private void CreateSplitRows(ExcelWorksheet worksheet, int rownumber, string MeterSerialNumber)
        //{

        //    worksheet.InsertRow(rownumber, 2, 14);
        //    worksheet.Cells["C" + rownumber].Value = MeterSerialNumber;
        //    int newrow = rownumber + 1;

        //    using (ExcelRange r = worksheet.Cells["C" + rownumber + ":F" + newrow])
        //    {
        //        if (r.Merge != true)
        //            r.Merge = true;
        //        r.Style.Font.SetFromFont(new System.Drawing.Font("Tahoma", 22, System.Drawing.FontStyle.Regular));
        //        r.Style.Font.Color.SetColor(Color.White);
        //        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
        //        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        r.Style.Fill.BackgroundColor.SetColor(Color.DarkOrange);

        //    }
        //}
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
        public void OpenExcelFile(string path)
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
