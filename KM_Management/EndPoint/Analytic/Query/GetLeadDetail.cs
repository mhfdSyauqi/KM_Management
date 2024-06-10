using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Helper;
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

        var filteredDate = DateFilter.GenerateFilterDate(request.Request.Filter, request.Request.Start_Date, request.Request.End_Date);

        var filter = new FilterLeadAnalytic()
        {
            Start_Date = filteredDate.StartDate,
            End_Date = filteredDate.EndDate,
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
