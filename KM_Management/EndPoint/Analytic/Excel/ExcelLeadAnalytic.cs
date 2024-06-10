using KM_Management.EndPoint.Analytic.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace KM_Management.EndPoint.Analytic.Excel;

public class ExcelLeadAnalytic : BaseExcel
{
    public static byte[] ExportLeaderboardAnalytic(ResponseExcelLeadAnalytic response)
    {
        using var excelFile = new ExcelPackage();

        var sheetLeaderboard = excelFile.Workbook.Worksheets.Add("Leaderboard");

        sheetLeaderboard.PrinterSettings.PaperSize = ePaperSize.A4;
        sheetLeaderboard.PrinterSettings.Orientation = eOrientation.Landscape;
        sheetLeaderboard.PrinterSettings.FitToPage = true;
        sheetLeaderboard.PrinterSettings.HeaderMargin = 0.76M;
        sheetLeaderboard.PrinterSettings.FooterMargin = 0.76M;
        sheetLeaderboard.PrinterSettings.TopMargin = 0;
        sheetLeaderboard.PrinterSettings.RightMargin = 0;
        sheetLeaderboard.PrinterSettings.BottomMargin = 0;
        sheetLeaderboard.PrinterSettings.LeftMargin = 0;

        using var title = sheetLeaderboard.Cells["B2:D2"];
        title.Value = "Leaderboard";
        title.Merge = true;
        title.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        title.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        sheetLeaderboard.Cells["A4"].Value = $"Period : {response.Period}";

        sheetLeaderboard.Cells["A6"].Value = "No.";
        sheetLeaderboard.Cells["B6"].Value = "Name";
        sheetLeaderboard.Cells["C6"].Value = "Usage";
        sheetLeaderboard.Cells["D6"].Value = "Category";
        sheetLeaderboard.Cells["E6"].Value = "Total";

        using var header = sheetLeaderboard.Cells["A6:E6"];
        header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        header.Style.Font.Bold = true;
        header.Style.Fill.PatternType = ExcelFillStyle.Solid;
        header.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

        var totalData = 6 + response.Lead_Data.Count;

        if (response.Lead_Data.Count > 0)
        {
            sheetLeaderboard.Cells[$"B7:E{totalData}"].LoadFromCollection(response.Lead_Data, false);
        }

        sheetLeaderboard.Cells[$"A6:E{totalData}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        sheetLeaderboard.Cells[$"A6:E{totalData}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        sheetLeaderboard.Cells[$"A6:E{totalData}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        sheetLeaderboard.Cells[$"A6:E{totalData}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        sheetLeaderboard.Cells[$"A6:E{totalData}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        for (int i = 1; i <= response.Lead_Data.Count; i++)
        {
            sheetLeaderboard.Cells[$"A{6 + i}"].Value = i;
        }

        sheetLeaderboard.Columns.Width = 17;

        return excelFile.GetAsByteArray();
    }
}
