using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Analytic.Query;

public record GetHitExcelQuery(RequestHitAnalytic Request) : IQuery<ResponseExcelHitAnalytic>;

public class GetHitExcelHandler : IQueryHandler<GetHitExcelQuery, ResponseExcelHitAnalytic>
{
    private readonly IAnalyticRepository _analyticRepository;

    public GetHitExcelHandler(IAnalyticRepository analyticRepository)
    {
        _analyticRepository = analyticRepository;
    }

    public async Task<Result<ResponseExcelHitAnalytic>> Handle(GetHitExcelQuery request, CancellationToken cancellationToken)
    {
        var currDate = DateTime.Now;
        var startDate = new DateTime(currDate.Year, currDate.Month, currDate.Day, 0, 0, 0);
        var endDate = startDate.AddDays(1).AddMinutes(-1);

        switch (request.Request.Filter)
        {
            case "Custom":
                startDate = request.Request.Start_Date.Value;
                endDate = request.Request.End_Date.Value;
                break;
            case "Yesterday":
                startDate = startDate.AddDays(-1);
                break;
            case "Last 7 Days":
                startDate = startDate.AddDays(-6);
                break;
            case "Last 30 Days":
                startDate = startDate.AddDays(-29);
                break;
            case "Last 3 Months":
                startDate = startDate.AddMonths(-3);
                break;
            case "Last 1 Year":
                startDate = startDate.AddYears(-1).AddDays(1);
                break;
            default:
                break;
        }

        var filterMatch = new FilterDetailHitAnalytic()
        {
            Start_Date = startDate,
            End_Date = endDate,
            Type = "match"
        };

        var filterUnmatch = new FilterDetailHitAnalytic()
        {
            Start_Date = startDate,
            End_Date = endDate,
            Type = "unmatch"
        };

        var matchData = await _analyticRepository.GetHitDetailAsync(filterMatch, cancellationToken);
        var unmatchData = await _analyticRepository.GetHitDetailAsync(filterUnmatch, cancellationToken);


        if (matchData == null || unmatchData == null)
        {
            return Result.Failure<ResponseExcelHitAnalytic>(new(HttpStatusCode.NotFound));
        }

        var response = new ResponseExcelHitAnalytic()
        {
            Match_Data = matchData.Select(col => new EntityExcelHitAnalytic() { Category = col.Name, Count = col.Count }).ToList(),
            Unmatch_Data = unmatchData.Select(col => new EntityExcelHitAnalytic() { Category = col.Name, Count = col.Count }).ToList(),
            Period = $"{startDate:dd-MMM-yyyy} s/d {endDate:dd-MMM-yyyy}"
        };

        return Result.Success(response);
    }
}

