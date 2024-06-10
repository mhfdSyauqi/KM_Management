using KM_Management.EndPoint.Analytic.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace KM_Management.EndPoint.Analytic.Excel;

public class ExcelPopularAnalytic : BaseExcel
{
    public static byte[] ExportPopolarAnalytic(List<EntityExcelGenPopularAnalytic> dataGeneral, List<EntityExcelDetPopularAnalytic> dataDetail, string Period)
    {
        using var excelFile = new ExcelPackage();

        /// General Sheet
        /// 
        var sheetGeneral = excelFile.Workbook.Worksheets.Add("General");

        sheetGeneral.PrinterSettings.PaperSize = ePaperSize.A4;
        sheetGeneral.PrinterSettings.Orientation = eOrientation.Landscape;
        sheetGeneral.PrinterSettings.FitToPage = true;
        sheetGeneral.PrinterSettings.HeaderMargin = 0.76M;
        sheetGeneral.PrinterSettings.FooterMargin = 0.76M;
        sheetGeneral.PrinterSettings.TopMargin = 0;
        sheetGeneral.PrinterSettings.RightMargin = 0;
        sheetGeneral.PrinterSettings.BottomMargin = 0;
        sheetGeneral.PrinterSettings.LeftMargin = 0;


        using var title = sheetGeneral.Cells["B2:D2"];
        title.Value = "General - Top 10 Categories";
        title.Merge = true;
        title.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        title.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        sheetGeneral.Cells["A4"].Value = $"Period : {Period}";

        sheetGeneral.Cells["A6"].Value = "No.";
        sheetGeneral.Cells["B6"].Value = "Category";
        sheetGeneral.Cells["C6"].Value = "Percentage";
        sheetGeneral.Cells["D6"].Value = "Total";

        using var header = sheetGeneral.Cells["A6:D6"];
        header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        header.Style.Font.Bold = true;
        header.Style.Fill.PatternType = ExcelFillStyle.Solid;
        header.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

        var totalData = 6 + dataGeneral.Count;

        if (dataGeneral.Count > 0)
        {
            sheetGeneral.Cells[$"B7:D{totalData}"].LoadFromCollection(dataGeneral, false);

        }

        sheetGeneral.Cells[$"A6:D{totalData}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        sheetGeneral.Cells[$"A6:D{totalData}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        sheetGeneral.Cells[$"A6:D{totalData}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        sheetGeneral.Cells[$"A6:D{totalData}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        sheetGeneral.Cells[$"A6:D{totalData}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        sheetGeneral.Cells[$"C6:C{totalData}"].Style.Numberformat.Format = "0\\%";

        for (int i = 1; i <= dataGeneral.Count; i++)
        {
            sheetGeneral.Cells[$"A{6 + i}"].Value = i;
        }

        sheetGeneral.Columns.Width = 17;

        /// Detail Sheet
        /// 
        var sheetDetail = excelFile.Workbook.Worksheets.Add("Detail");

        sheetDetail.PrinterSettings.PaperSize = ePaperSize.A4;
        sheetDetail.PrinterSettings.Orientation = eOrientation.Landscape;
        sheetDetail.PrinterSettings.FitToPage = true;
        sheetDetail.PrinterSettings.HeaderMargin = 0.76M;
        sheetDetail.PrinterSettings.FooterMargin = 0.76M;
        sheetDetail.PrinterSettings.TopMargin = 0;
        sheetDetail.PrinterSettings.RightMargin = 0;
        sheetDetail.PrinterSettings.BottomMargin = 0;
        sheetDetail.PrinterSettings.LeftMargin = 0;


        using var titleDet = sheetDetail.Cells["B2:D2"];
        titleDet.Value = "Detail - Top 10 Categories";
        titleDet.Merge = true;
        titleDet.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        titleDet.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        sheetDetail.Cells["A4"].Value = $"Period : {Period}";

        sheetDetail.Cells["A6"].Value = "No.";
        sheetDetail.Cells["B6"].Value = "Case";
        sheetDetail.Cells["C6"].Value = "Category";
        sheetDetail.Cells["D6"].Value = "Percentage";
        sheetDetail.Cells["E6"].Value = "Total";

        using var headerDet = sheetDetail.Cells["A6:E6"];
        headerDet.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        headerDet.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        headerDet.Style.Font.Bold = true;
        headerDet.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerDet.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

        var totalDataDet = 6 + dataDetail.Count;

        if (dataDetail.Count > 0)
        {
            sheetDetail.Cells[$"B7:E{totalDataDet}"].LoadFromCollection(dataDetail, false);

        }
        sheetDetail.Cells[$"A6:E{totalDataDet}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        sheetDetail.Cells[$"A6:E{totalDataDet}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        sheetDetail.Cells[$"A6:E{totalDataDet}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        sheetDetail.Cells[$"A6:E{totalDataDet}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        sheetDetail.Cells[$"A6:E{totalDataDet}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        sheetDetail.Cells[$"D6:D{totalDataDet}"].Style.Numberformat.Format = "0\\%";

        for (int i = 1; i <= dataDetail.Count; i++)
        {
            sheetDetail.Cells[$"A{6 + i}"].Value = i;
        }

        sheetDetail.Columns.Width = 17;

        return excelFile.GetAsByteArray();
    }
}
