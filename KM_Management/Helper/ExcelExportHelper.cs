using OfficeOpenXml;
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

                worksheet.Cells["A4"].Value = "No";
                data.ForEach(x =>
                {
                    worksheet.Cells[$"A{4 + i}"].Value = i;
                    i++;
                });


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


        public class MappingExcelDetail
        {
            public string CellHeader { get; set; }

            public int Column { get; set; }
        }
    }
}