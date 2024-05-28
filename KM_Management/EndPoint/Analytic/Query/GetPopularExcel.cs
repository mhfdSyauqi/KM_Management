using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Helper;
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
        var filteredDate = DateFilter.GenerateFilterDate(request.Request.Filter, request.Request.Start_Date, request.Request.End_Date);

        var filter = new FilterGeneralPopularAnalytic()
        {
            Start_Date = filteredDate.StartDate,
            End_Date = filteredDate.EndDate
        };

        var result = await _analyticRepository.GetPopularExcelAsync(filter, cancellationToken);

        if (result == null)
        {
            return Result.Failure<EntityExcelPopularAnalytic>(new(HttpStatusCode.NotFound, null));
        }


        return Result.Success(result);
    }
}