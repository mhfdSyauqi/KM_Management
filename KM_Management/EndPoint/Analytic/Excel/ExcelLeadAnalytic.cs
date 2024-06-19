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

        var totalData = 6 + response.General_Data.Count;

        if (response.General_Data.Count > 0)
        {
            sheetLeaderboard.Cells[$"B7:E{totalData}"].LoadFromCollection(response.General_Data, false);
        }

        sheetLeaderboard.Cells[$"A6:E{totalData}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        sheetLeaderboard.Cells[$"A6:E{totalData}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        sheetLeaderboard.Cells[$"A6:E{totalData}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        sheetLeaderboard.Cells[$"A6:E{totalData}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        sheetLeaderboard.Cells[$"A6:E{totalData}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        for (int i = 1; i <= response.General_Data.Count; i++)
        {
            sheetLeaderboard.Cells[$"A{6 + i}"].Value = i;
        }

        sheetLeaderboard.Columns.Width = 17;


        var sheetLeaderboardDetail = excelFile.Workbook.Worksheets.Add("Detail");

        sheetLeaderboardDetail.PrinterSettings.PaperSize = ePaperSize.A4;
        sheetLeaderboardDetail.PrinterSettings.Orientation = eOrientation.Landscape;
        sheetLeaderboardDetail.PrinterSettings.FitToPage = true;
        sheetLeaderboardDetail.PrinterSettings.HeaderMargin = 0.76M;
        sheetLeaderboardDetail.PrinterSettings.FooterMargin = 0.76M;
        sheetLeaderboardDetail.PrinterSettings.TopMargin = 0;
        sheetLeaderboardDetail.PrinterSettings.RightMargin = 0;
        sheetLeaderboardDetail.PrinterSettings.BottomMargin = 0;
        sheetLeaderboardDetail.PrinterSettings.LeftMargin = 0;

        using var titleDetail = sheetLeaderboardDetail.Cells["B2:D2"];
        titleDetail.Value = "Detail - Leaderboard";
        titleDetail.Merge = true;
        titleDetail.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        titleDetail.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        sheetLeaderboardDetail.Cells["A4"].Value = $"Period : {response.Period}";

        sheetLeaderboardDetail.Cells["A6"].Value = "No.";
        sheetLeaderboardDetail.Cells["B6"].Value = "Name";
        sheetLeaderboardDetail.Cells["C6"].Value = "Usage";
        sheetLeaderboardDetail.Cells["D6"].Value = "Category";
        sheetLeaderboardDetail.Cells["E6"].Value = "Time";

        using var headerDetail = sheetLeaderboardDetail.Cells["A6:E6"];
        headerDetail.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        headerDetail.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        headerDetail.Style.Font.Bold = true;
        headerDetail.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerDetail.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

        var totalDetailData = 6 + response.Detail_Data.Count;

        if (response.Detail_Data.Count > 0)
        {
            sheetLeaderboardDetail.Cells[$"B7:E{totalDetailData}"].LoadFromCollection(response.Detail_Data, false);
        }

        sheetLeaderboardDetail.Cells[$"A6:E{totalDetailData}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        sheetLeaderboardDetail.Cells[$"A6:E{totalDetailData}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        sheetLeaderboardDetail.Cells[$"A6:E{totalDetailData}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        sheetLeaderboardDetail.Cells[$"A6:E{totalDetailData}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        sheetLeaderboardDetail.Cells[$"A6:E{totalDetailData}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        for (int i = 1; i <= response.Detail_Data.Count; i++)
        {
            sheetLeaderboardDetail.Cells[$"A{6 + i}"].Value = i;
        }

        sheetLeaderboardDetail.Columns.Width = 17;

        return excelFile.GetAsByteArray();
    }
}
