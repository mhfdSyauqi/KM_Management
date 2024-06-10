using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Helper;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Analytic.Query;

public record GetPopularGeneralQuery(RequestPopularAnalytic Request) : IQuery<List<ResponseGeneralPopularAnalytic>>;

public class GetPopularGeneralValidator : AbstractValidator<GetPopularGeneralQuery>
{
    public GetPopularGeneralValidator()
    {

    }
}

public class GetPopularGeneralHandler : IQueryHandler<GetPopularGeneralQuery, List<ResponseGeneralPopularAnalytic>>
{
    private readonly IAnalyticRepository _analyticRepository;
    private readonly IValidator<GetPopularGeneralQuery> _validator;

    public GetPopularGeneralHandler(IAnalyticRepository analyticRepository, IValidator<GetPopularGeneralQuery> validator)
    {
        _analyticRepository = analyticRepository;
        _validator = validator;
    }

    public async Task<Result<List<ResponseGeneralPopularAnalytic>>> Handle(GetPopularGeneralQuery request, CancellationToken cancellationToken)
    {
        var validator = await _validator.ValidateAsync(request, cancellationToken);

        if (!validator.IsValid)
        {
            var errMsg = validator.Errors?.First()?.ErrorMessage;
            return Result.Failure<List<ResponseGeneralPopularAnalytic>>(new(HttpStatusCode.BadRequest, errMsg));
        }

        var filteredDate = DateFilter.GenerateFilterDate(request.Request.Filter, request.Request.Start_Date, request.Request.End_Date);


        var filter = new FilterGeneralPopularAnalytic()
        {
            Start_Date = filteredDate.StartDate,
            End_Date = filteredDate.EndDate
        };

        var result = await _analyticRepository.GetPopularGeneralAsync(filter, cancellationToken);

        var response = result.Select(col => new ResponseGeneralPopularAnalytic()
        {
            Reference = col.Uid.ToString("N"),
            Name = col.Name,
            Count = col.Total,
            Percent = (double)col.Total / (double)result.Sum(key => key.Total) * 100
        }).ToList();

        return Result.Success(response);
    }
}