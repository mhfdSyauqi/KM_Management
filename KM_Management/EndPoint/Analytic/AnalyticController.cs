using KM_Management.Controllers;
using KM_Management.EndPoint.Analytic.Excel;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.EndPoint.Analytic.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.EndPoint.Analytic;

public class AnalyticController : MyAPIController
{
    public AnalyticController(IMediator Mediator) : base(Mediator)
    {
    }

    [HttpPost]
    [Route("popular/general")]
    public async Task<IActionResult> GetPopularGeneral([FromBody] RequestPopularAnalytic request, CancellationToken cancellationToken)
    {
        var query = new GetPopularGeneralQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpPost]
    [Route("popular/detail")]
    public async Task<IActionResult> GetPopularDetail([FromBody] RequestPopularAnalytic request, CancellationToken cancellationToken)
    {
        var query = new GetPopularDetailQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpGet]
    [Route("popular/excel")]
    public async Task<IActionResult> GetPopularExcel([FromQuery] RequestPopularAnalytic request, CancellationToken cancellationToken)
    {
        var query = new GetPopularExcelQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        var excelReport = ExcelPopularAnalytic.ExportPopolarAnalytic(result.Value.General, result.Value.Detail, result.Value.Period);

        var filter = request.Filter == "Custom" ? result.Value.Period : request.Filter;

        return File(excelReport, ExcelPopularAnalytic.ContentType, $"Top Categories Report ({filter}).xlsx");
    }


    [HttpPost]
    [Route("hit/general")]
    public async Task<IActionResult> GetHitGeneral([FromBody] RequestHitAnalytic request, CancellationToken cancellationToken)
    {
        var query = new GetHitGeneralQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpPost]
    [Route("hit/detail")]
    public async Task<IActionResult> GetHitDetail([FromBody] RequestHitAnalytic request, CancellationToken cancellationToken)
    {
        var query = new GetHitDetailQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpGet]
    [Route("hit/excel")]
    public async Task<IActionResult> GetHitExcel([FromQuery] RequestHitAnalytic request, CancellationToken cancellationToken)
    {
        var query = new GetHitExcelQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        var excelReport = ExcelHitAnalytic.ExportHitAnalytic(result.Value);

        var filter = request.Filter == "Custom" ? result.Value.Period : request.Filter;

        return File(excelReport, ExcelPopularAnalytic.ContentType, $"Hit Categories Report ({filter}).xlsx");
    }


    [HttpPost]
    [Route("lead/general")]
    public async Task<IActionResult> GetLeadGeneral([FromBody] RequestLeaderboardAnalytic request, CancellationToken cancellationToken)
    {
        var query = new GetLeadGeneralQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpPost]
    [Route("lead/detail")]
    public async Task<IActionResult> GetLeadDetail([FromBody] RequestLeaderboardAnalytic request, CancellationToken cancellationToken)
    {
        var query = new GetLeadDetailQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpGet]
    [Route("lead/excel")]
    public async Task<IActionResult> GetLeadExcel([FromQuery] RequestLeaderboardAnalytic request, CancellationToken cancellationToken)
    {
        var query = new GetLeadExcelQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        var excelReport = ExcelLeadAnalytic.ExportLeaderboardAnalytic(result.Value);

        var filter = request.Filter == "Custom" ? result.Value.Period : request.Filter;

        return File(excelReport, ExcelPopularAnalytic.ContentType, $"Leaderboard Report ({filter}).xlsx");
    }



}
