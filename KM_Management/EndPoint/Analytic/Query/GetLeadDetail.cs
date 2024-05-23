using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Analytic.Query;

public record GetLeadDetailQuery(RequestLeaderboardAnalytic Request) : IQuery<List<ResponseDetailLeadAnalytic>>;

public class GetLeadDetailValidator : AbstractValidator<GetLeadDetailQuery>
{
    public GetLeadDetailValidator()
    {

    }
}

public class GetLeadDetailHandler : IQueryHandler<GetLeadDetailQuery, List<ResponseDetailLeadAnalytic>>
{
    private readonly IAnalyticRepository _analyticRepository;
    private readonly IValidator<GetLeadDetailQuery> _validator;

    public GetLeadDetailHandler(IAnalyticRepository analyticRepository, IValidator<GetLeadDetailQuery> validator)
    {
        _analyticRepository = analyticRepository;
        _validator = validator;
    }

    public async Task<Result<List<ResponseDetailLeadAnalytic>>> Handle(GetLeadDetailQuery request, CancellationToken cancellationToken)
    {
        var validator = await _validator.ValidateAsync(request, cancellationToken);

        if (!validator.IsValid)
        {
            var errMsg = validator.Errors.First().ErrorMessage;
            return Result.Failure<List<ResponseDetailLeadAnalytic>>(new(HttpStatusCode.BadRequest, errMsg));
        }

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

        var filter = new FilterLeadAnalytic()
        {
            Start_Date = startDate,
            End_Date = endDate,
        };

        var result = await _analyticRepository.GetLeadDetailAsync(filter, cancellationToken);

        if (result == null)
        {

            return Result.Failure<List<ResponseDetailLeadAnalytic>>(new(HttpStatusCode.NotFound));
        }

        var response = result.Select(col => new ResponseDetailLeadAnalytic()
        {
            User_Name = col.User_Name,
            Category = col.Category,
        }).ToList();

        return Result.Success(response);
    }
}
