using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Helper;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Analytic.Query;

public record GetLeadGeneralQuery(RequestLeaderboardAnalytic Request) : IQuery<List<ResponseGeneralLeadAnalytic>>;

public class GetLeadGeneralValidator : AbstractValidator<GetLeadGeneralQuery>
{
    public GetLeadGeneralValidator()
    {

    }
}

public class GetLeadGeneralHandler : IQueryHandler<GetLeadGeneralQuery, List<ResponseGeneralLeadAnalytic>>
{
    private readonly IAnalyticRepository _analyticRepository;
    private readonly IValidator<GetLeadGeneralQuery> _validator;

    public GetLeadGeneralHandler(IAnalyticRepository analyticRepository, IValidator<GetLeadGeneralQuery> validator)
    {
        _analyticRepository = analyticRepository;
        _validator = validator;
    }

    public async Task<Result<List<ResponseGeneralLeadAnalytic>>> Handle(GetLeadGeneralQuery request, CancellationToken cancellationToken)
    {
        var validator = await _validator.ValidateAsync(request, cancellationToken);

        if (!validator.IsValid)
        {
            var errMsg = validator.Errors.First().ErrorMessage;
            return Result.Failure<List<ResponseGeneralLeadAnalytic>>(new(HttpStatusCode.BadRequest, errMsg));
        }

        var filteredDate = DateFilter.GenerateFilterDate(request.Request.Filter, request.Request.Start_Date, request.Request.End_Date);

        var filter = new FilterLeadAnalytic()
        {
            Start_Date = filteredDate.StartDate,
            End_Date = filteredDate.EndDate,
        };

        var result = await _analyticRepository.GetLeadGeneralAsync(filter, cancellationToken);

        if (result == null)
        {
            return Result.Failure<List<ResponseGeneralLeadAnalytic>>(new(HttpStatusCode.NotFound));
        }

        var response = result.Select(col => new ResponseGeneralLeadAnalytic()
        {
            User_Name = col.User_Name,
            Count = col.Category_Count
        }).ToList();

        return Result.Success(response);
    }
}
