using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Helper;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Analytic.Query;

public record GetLeadExcelQuery(RequestLeaderboardAnalytic Request) : IQuery<ResponseExcelLeadAnalytic>;

public class GetLeadExcelHandler : IQueryHandler<GetLeadExcelQuery, ResponseExcelLeadAnalytic>
{
    private readonly IAnalyticRepository _analyticRepository;

    public GetLeadExcelHandler(IAnalyticRepository analyticRepository)
    {
        _analyticRepository = analyticRepository;
    }

    public async Task<Result<ResponseExcelLeadAnalytic>> Handle(GetLeadExcelQuery request, CancellationToken cancellationToken)
    {
        var filteredDate = DateFilter.GenerateFilterDate(request.Request.Filter, request.Request.Start_Date, request.Request.End_Date);

        var filter = new FilterLeadAnalytic()
        {
            Start_Date = filteredDate.StartDate,
            End_Date = filteredDate.EndDate,
        };

        var result = await _analyticRepository.GetLeadExcelAsync(filter, cancellationToken);

        if (result == null)
        {
            return Result.Failure<ResponseExcelLeadAnalytic>(new(HttpStatusCode.BadRequest));
        }

        var response = new ResponseExcelLeadAnalytic()
        {
            General_Data = result.General,
            Detail_Data = result.Detail,
            Period = result.Period,
        };

        return Result.Success(response);
    }
}

