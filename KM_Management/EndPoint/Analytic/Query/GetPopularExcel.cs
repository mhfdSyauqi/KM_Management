using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Analytic.Query;

public record GetPopularExcelQuery(RequestPopularAnalytic Request) : IQuery<EntityExcelPopularAnalytic>;

public class GetExcelPopularAnalyticHandler : IQueryHandler<GetPopularExcelQuery, EntityExcelPopularAnalytic>
{
    private readonly IAnalyticRepository _analyticRepository;

    public GetExcelPopularAnalyticHandler(IAnalyticRepository analyticRepository)
    {
        _analyticRepository = analyticRepository;
    }

    public async Task<Result<EntityExcelPopularAnalytic>> Handle(GetPopularExcelQuery request, CancellationToken cancellationToken)
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


        var filter = new FilterGeneralPopularAnalytic()
        {
            Start_Date = startDate,
            End_Date = endDate
        };

        var result = await _analyticRepository.GetPopularExcelAsync(filter, cancellationToken);

        if (result == null)
        {
            return Result.Failure<EntityExcelPopularAnalytic>(new(HttpStatusCode.NotFound, null));
        }


        return Result.Success(result);
    }
}