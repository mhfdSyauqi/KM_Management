using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace KM_Management.Helper
{
    public class ExcelExportHelper
    {
        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportExcelRateAndFeedback<T>(List<T> data, List<string> AllVisibleColumns)
        {

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var excelFile = new ExcelPackage())
            {
                var i = 1;
                var worksheet = excelFile.Workbook.Worksheets.Add("Sheet1");
                worksheet.PrinterSettings.PaperSize = ePaperSize.A3;
                worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                worksheet.PrinterSettings.FitToPage = true;
                worksheet.PrinterSettings.HeaderMargin = 0.76M;
                worksheet.PrinterSettings.FooterMargin = 0.76M;
                worksheet.PrinterSettings.TopMargin = 0;
                worksheet.PrinterSettings.RightMargin = 0;
                worksheet.PrinterSettings.BottomMargin = 0;
                worksheet.PrinterSettings.LeftMargin = 0;

                

                worksheet.Cells["B4"].LoadFromCollection(data, true);


                //def header
                worksheet.Cells["B4"].Value = "Time";
                worksheet.Cells["C4"].Value = "Name";
                worksheet.Cells["D4"].Value = "Total Category";
                worksheet.Cells["E4"].Value = "Rating";
                worksheet.Cells["F4"].Value = "Feedback";


                worksheet.Cells["A1"].Value = "Rate & Feedback ";
                worksheet.Cells["A1"].Style.Font.Size = 14;
                worksheet.Cells["A2"].Style.Font.Size = 14;

                for (int column = 2; column <= 6; column++)
                {
                    worksheet.Column(column).AutoFit();
                }

                return excelFile.GetAsByteArray();

            }
        }


        public static byte[] ExportExcelCategories<T,U,X>(List<T> firstData, List<U> secondData, List<X> thirdData)
        {

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var excelFile = new ExcelPackage())
            {
                var i = 1;
                var firstWorksheet = excelFile.Workbook.Worksheets.Add("Layer 1");
                firstWorksheet.PrinterSettings.PaperSize = ePaperSize.A3;
                firstWorksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                firstWorksheet.PrinterSettings.FitToPage = true;
                firstWorksheet.PrinterSettings.HeaderMargin = 0.76M;
                firstWorksheet.PrinterSettings.FooterMargin = 0.76M;
                firstWorksheet.PrinterSettings.TopMargin = 0;
                firstWorksheet.PrinterSettings.RightMargin = 0;
                firstWorksheet.PrinterSettings.BottomMargin = 0;
                firstWorksheet.PrinterSettings.LeftMargin = 0;

                firstWorksheet.Cells["A6"].Value = "No";
                firstWorksheet.Cells[$"A2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                firstData.ForEach(x =>
                {
                    firstWorksheet.Cells[$"A{6 + i}:D{6 + i}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    firstWorksheet.Cells[$"A{6 + i}:D{6 + i}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    firstWorksheet.Cells[$"A{6 + i}:D{6 + i}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    firstWorksheet.Cells[$"A{6 + i}:D{6 + i}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    firstWorksheet.Cells[$"A{6 + i}:B{6 + i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    firstWorksheet.Cells[$"C{6 + i}:D{6 + i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    firstWorksheet.Cells[$"B{6 + i}:D{6 + i}"].Calculate();
                    firstWorksheet.Cells[$"A{6 + i}"].Value = i;
                    i++;
                });

                firstWorksheet.Column(1).Width = 5;

                using (var range = firstWorksheet.Cells["A6:D6"])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    range.Style.Font.Bold = true;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                firstWorksheet.Cells["B6"].LoadFromCollection(firstData, true);
                firstWorksheet.Cells["A2:D2"].Merge = true;
                firstWorksheet.Cells["A2"].Style.Font.Bold = true;
                firstWorksheet.Cells["A2"].Value = "Layer 1 - List Categories";






                firstWorksheet.Cells["A4"].Value = "Date : " + DateTime.Now.ToString("MMM dd, yyyy");
                //def header
                firstWorksheet.Cells["B6"].Value = "Layer 1";
                firstWorksheet.Cells["C6"].Value = "Status";
                firstWorksheet.Cells["D6"].Value = "Publication";



                for (int column = 2; column <= 6; column++)
                {
                    firstWorksheet.Column(column).AutoFit();
                    firstWorksheet.Column(column).Width = firstWorksheet.Column(column).Width + 5;
                }

               var y = 1;
                var secondWorksheet = excelFile.Workbook.Worksheets.Add("Layer 2");
                secondWorksheet.PrinterSettings.PaperSize = ePaperSize.A3;
                secondWorksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                secondWorksheet.PrinterSettings.FitToPage = true;
                secondWorksheet.PrinterSettings.HeaderMargin = 0.76M;
                secondWorksheet.PrinterSettings.FooterMargin = 0.76M;
                secondWorksheet.PrinterSettings.TopMargin = 0;
                secondWorksheet.PrinterSettings.RightMargin = 0;
                secondWorksheet.PrinterSettings.BottomMargin = 0;
                secondWorksheet.PrinterSettings.LeftMargin = 0;

                secondWorksheet.Cells["A6"].Value = "No";
                secondWorksheet.Cells[$"A2:E2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                secondData.ForEach(x =>
                {
                    secondWorksheet.Cells[$"A{6 + y}:E{6 + y}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    secondWorksheet.Cells[$"A{6 + y}:E{6 + y}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    secondWorksheet.Cells[$"A{6 + y}:E{6 + y}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    secondWorksheet.Cells[$"A{6 + y}:E{6 + y}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    secondWorksheet.Cells[$"A{6 + y}:C{6 + y}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    secondWorksheet.Cells[$"D{6 + y}:E{6 + y}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    secondWorksheet.Cells[$"B{6 + y}:E{6 + y}"].Calculate();
                    secondWorksheet.Cells[$"A{6 + y}"].Value = y;
                    y++;
                });

                secondWorksheet.Column(1).Width = 5;

                using (var range = secondWorksheet.Cells["A6:E6"])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    range.Style.Font.Bold = true;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                secondWorksheet.Cells["B6"].LoadFromCollection(secondData, true);
                secondWorksheet.Cells["A2:E2"].Merge = true;
                secondWorksheet.Cells["A2"].Style.Font.Bold = true;
                secondWorksheet.Cells["A2"].Value = "Layer 2 - List Categories";






                secondWorksheet.Cells["A4"].Value = "Date : " + DateTime.Now.ToString("MMM dd, yyyy");
                //def header
                secondWorksheet.Cells["B6"].Value = "Layer 2";
                secondWorksheet.Cells["C6"].Value = "Layer 1";
                secondWorksheet.Cells["D6"].Value = "Status";
                secondWorksheet.Cells["E6"].Value = "Publication";


                for (int column = 2; column <= 6; column++)
                {
                    secondWorksheet.Column(column).AutoFit();
                    secondWorksheet.Column(column).Width = secondWorksheet.Column(column).Width + 5;
                }

                var j = 1;
                var thirdWorksheet = excelFile.Workbook.Worksheets.Add("Layer 3");
                thirdWorksheet.PrinterSettings.PaperSize = ePaperSize.A3;
                thirdWorksheet.PrinterSettings.Orientation = eOrientation.Landscape;
                thirdWorksheet.PrinterSettings.FitToPage = true;
                thirdWorksheet.PrinterSettings.HeaderMargin = 0.76M;
                thirdWorksheet.PrinterSettings.FooterMargin = 0.76M;
                thirdWorksheet.PrinterSettings.TopMargin = 0;
                thirdWorksheet.PrinterSettings.RightMargin = 0;
                thirdWorksheet.PrinterSettings.BottomMargin = 0;
                thirdWorksheet.PrinterSettings.LeftMargin = 0;

                thirdWorksheet.Cells["A6"].Value = "No";
                thirdWorksheet.Cells[$"A2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                thirdData.ForEach(x =>
                {
                    thirdWorksheet.Cells[$"A{6 + j}:F{6 + j}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    thirdWorksheet.Cells[$"A{6 + j}:F{6 + j}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    thirdWorksheet.Cells[$"A{6 + j}:F{6 + j}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    thirdWorksheet.Cells[$"A{6 + j}:F{6 + j}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    thirdWorksheet.Cells[$"A{6 + j}:D{6 + j}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    thirdWorksheet.Cells[$"E{6 + j}:F{6 + j}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    thirdWorksheet.Cells[$"B{6 + j}:F{6 + j}"].Calculate();
                    thirdWorksheet.Cells[$"A{6 + j}"].Value = j;
                    j++;
                });

                thirdWorksheet.Column(1).Width = 5;

                using (var range = thirdWorksheet.Cells["A6:F6"])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    range.Style.Font.Bold = true;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                thirdWorksheet.Cells["B6"].LoadFromCollection(thirdData, true);
                thirdWorksheet.Cells["A2:F2"].Merge = true;
                thirdWorksheet.Cells["A2"].Style.Font.Bold = true;
                thirdWorksheet.Cells["A2"].Value = "Layer 3 - List Categories";






                thirdWorksheet.Cells["A4"].Value = "Date : " + DateTime.Now.ToString("MMM dd, yyyy");
                //def header
                thirdWorksheet.Cells["B6"].Value = "Layer 3";
                thirdWorksheet.Cells["C6"].Value = "Layer 2";
                thirdWorksheet.Cells["D6"].Value = "Layer 1";
                thirdWorksheet.Cells["E6"].Value = "Status";
                thirdWorksheet.Cells["F6"].Value = "Publication";



                for (int column = 2; column <= 6; column++)
                {
                    thirdWorksheet.Column(column).AutoFit();
                    thirdWorksheet.Column(column).Width = thirdWorksheet.Column(column).Width + 5;
                }

                return excelFile.GetAsByteArray();

            }
        }







        public class MappingExcelDetail
        {
            public string CellHeader { get; set; }

            public int Column { get; set; }
        }
    }
}