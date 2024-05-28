using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Helper;
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
        var filteredDate = DateFilter.GenerateFilterDate(request.Request.Filter, request.Request.Start_Date, request.Request.End_Date);

        var filterMatch = new FilterDetailHitAnalytic()
        {
            Start_Date = filteredDate.StartDate,
            End_Date = filteredDate.EndDate,
            Type = "match"
        };

        var filterUnmatch = new FilterDetailHitAnalytic()
        {
            Start_Date = filteredDate.StartDate,
            End_Date = filteredDate.EndDate,
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
            Period = $"{filteredDate.StartDate:dd-MMM-yyyy} s/d {filteredDate.EndDate:dd-MMM-yyyy}"
        };

        return Result.Success(response);
    }
}

