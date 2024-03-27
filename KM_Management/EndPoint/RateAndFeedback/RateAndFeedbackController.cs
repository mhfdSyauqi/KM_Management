using KM_Management.Helper;
using KM_Management.Controllers;
using KM_Management.EndPoint.RateAndFeedback.Models;
using KM_Management.EndPoint.RateAndFeedback.Query;
using KM_Management.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.EndPoint.RateAndFeedback;

public class RateAndFeedbackController : MyAPIController
{
    public RateAndFeedbackController(IMediator Mediator) : base(Mediator)
    {
    }

    [HttpPost]
    [Route("GetRateAndFeedback")]
    public async Task<IActionResult> GetRateAndFeedbackWithFilter([FromBody] RequestRateAndFeedback request, CancellationToken cancellationToken)
    {
        var query = new GetRateAndFeedbackQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpPost]
    [Route("ExportRateAndFeedbackToExcel")]
    public async Task<IActionResult> ExportRateAndFeedbackToExcel([FromBody] RequestRateAndFeedback request, CancellationToken cancellationToken)
    {
        var query = new GetRateAndFeedbackQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);


        var data = result.Value.Items;
        var periode = result.Value.Periode;
        var newData = data.Select(item => new
        {

            Create_At = item.Create_At.ToString("dd-MM-yyyy HH:mm:ss"),
            Create_By = item.Create_By,
            Total_Category = item.Total_Category,
            Rating = item.Rating,
            Remark = item.Remark,
        }).ToList();

        var columns = new List<string> { "Time", "Name", "Total Category", "Rating", "Feedback"};

        byte[] fileBytes = ExcelExportHelper.ExportExcelRateAndFeedback(newData, periode ,columns);

        return File(fileBytes, ExcelExportHelper.ExcelContentType, "RateAndFeedback.xlsx");

    }

}
