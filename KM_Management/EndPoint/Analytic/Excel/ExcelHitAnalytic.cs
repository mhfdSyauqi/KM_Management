using KM_Management.EndPoint.Analytic.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace KM_Management.EndPoint.Analytic.Excel;

public class ExcelHitAnalytic : BaseExcel
{
    public static byte[] ExportHitAnalytic(ResponseExcelHitAnalytic response)
    {
        using var excelFile = new ExcelPackage();

        /// Match Sheet
        /// 
        var sheetMatch = excelFile.Workbook.Worksheets.Add("Match Categories");

        sheetMatch.PrinterSettings.PaperSize = ePaperSize.A4;
        sheetMatch.PrinterSettings.Orientation = eOrientation.Landscape;
        sheetMatch.PrinterSettings.FitToPage = true;
        sheetMatch.PrinterSettings.HeaderMargin = 0.76M;
        sheetMatch.PrinterSettings.FooterMargin = 0.76M;
        sheetMatch.PrinterSettings.TopMargin = 0;
        sheetMatch.PrinterSettings.RightMargin = 0;
        sheetMatch.PrinterSettings.BottomMargin = 0;
        sheetMatch.PrinterSettings.LeftMargin = 0;


        using var title = sheetMatch.Cells["B2:D2"];
        title.Value = "Match Categories";
        title.Merge = true;
        title.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        title.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        sheetMatch.Cells["A4"].Value = $"Period : {response.Period}";

        sheetMatch.Cells["A6"].Value = "No.";
        sheetMatch.Cells["B6"].Value = "Category";
        sheetMatch.Cells["C6"].Value = "Total";

        using var header = sheetMatch.Cells["A6:C6"];
        header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        header.Style.Font.Bold = true;
        header.Style.Fill.PatternType = ExcelFillStyle.Solid;
        header.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

        var totalData = 6 + response.Match_Data.Count;

        if (response.Match_Data.Count > 0)
        {
            sheetMatch.Cells[$"B7:C{totalData}"].LoadFromCollection(response.Match_Data, false);

        }

        sheetMatch.Cells[$"A6:C{totalData}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        sheetMatch.Cells[$"A6:C{totalData}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        sheetMatch.Cells[$"A6:C{totalData}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        sheetMatch.Cells[$"A6:C{totalData}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        sheetMatch.Cells[$"A6:C{totalData}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        for (int i = 1; i <= response.Match_Data.Count; i++)
        {
            sheetMatch.Cells[$"A{6 + i}"].Value = i;
        }

        sheetMatch.Columns.Width = 17;

        /// Detail Sheet
        /// 
        var sheetUnmatch = excelFile.Workbook.Worksheets.Add("No Match Categories");

        sheetUnmatch.PrinterSettings.PaperSize = ePaperSize.A4;
        sheetUnmatch.PrinterSettings.Orientation = eOrientation.Landscape;
        sheetUnmatch.PrinterSettings.FitToPage = true;
        sheetUnmatch.PrinterSettings.HeaderMargin = 0.76M;
        sheetUnmatch.PrinterSettings.FooterMargin = 0.76M;
        sheetUnmatch.PrinterSettings.TopMargin = 0;
        sheetUnmatch.PrinterSettings.RightMargin = 0;
        sheetUnmatch.PrinterSettings.BottomMargin = 0;
        sheetUnmatch.PrinterSettings.LeftMargin = 0;


        using var titleDet = sheetUnmatch.Cells["B2:D2"];
        titleDet.Value = "No Match Categories";
        titleDet.Merge = true;
        titleDet.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        titleDet.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        sheetUnmatch.Cells["A4"].Value = $"Period : {response.Period}";

        sheetUnmatch.Cells["A6"].Value = "No.";
        sheetUnmatch.Cells["B6"].Value = "Category";
        sheetUnmatch.Cells["C6"].Value = "Total";

        using var headerDet = sheetUnmatch.Cells["A6:C6"];
        headerDet.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        headerDet.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        headerDet.Style.Font.Bold = true;
        headerDet.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerDet.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

        var totalDataDet = 6 + response.Unmatch_Data.Count;

        if (response.Unmatch_Data.Count > 0)
        {
            sheetUnmatch.Cells[$"B7:C{totalDataDet}"].LoadFromCollection(response.Unmatch_Data, false);

        }
        sheetUnmatch.Cells[$"A6:C{totalDataDet}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        sheetUnmatch.Cells[$"A6:C{totalDataDet}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        sheetUnmatch.Cells[$"A6:C{totalDataDet}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        sheetUnmatch.Cells[$"A6:C{totalDataDet}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        sheetUnmatch.Cells[$"A6:C{totalDataDet}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        for (int i = 1; i <= response.Unmatch_Data.Count; i++)
        {
            sheetUnmatch.Cells[$"A{6 + i}"].Value = i;
        }

        sheetUnmatch.Columns.Width = 17;

        return excelFile.GetAsByteArray();
    }
}
