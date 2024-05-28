using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Helper;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Analytic.Query;

public record GetHitGeneralQuery(RequestHitAnalytic Request) : IQuery<ResponseGeneralHitAnalytic>;

public class GetHitGeneralValidator : AbstractValidator<GetHitGeneralQuery>
{
    public GetHitGeneralValidator()
    {

    }
}

public class GetHitGeneralHandler : IQueryHandler<GetHitGeneralQuery, ResponseGeneralHitAnalytic>
{
    private readonly IAnalyticRepository _analyticRepository;
    private readonly IValidator<GetHitGeneralQuery> _validator;

    public GetHitGeneralHandler(IAnalyticRepository analyticRepository, IValidator<GetHitGeneralQuery> validator)
    {
        _analyticRepository = analyticRepository;
        _validator = validator;
    }

    public async Task<Result<ResponseGeneralHitAnalytic>> Handle(GetHitGeneralQuery request, CancellationToken cancellationToken)
    {
        var validator = await _validator.ValidateAsync(request, cancellationToken);

        if (!validator.IsValid)
        {
            var errMsg = validator.Errors.First().ErrorMessage;
            return Result.Failure<ResponseGeneralHitAnalytic>(new(HttpStatusCode.BadRequest, errMsg));
        }

        var filteredDate = DateFilter.GenerateFilterDate(request.Request.Filter, request.Request.Start_Date, request.Request.End_Date);

        var filter = new FilterGeneralHitAnalytic()
        {
            Start_Date = filteredDate.StartDate,
            End_Date = filteredDate.EndDate
        };

        var result = await _analyticRepository.GetHitGeneralAsync(filter, cancellationToken);

        if (result == null)
        {
            return Result.Failure<ResponseGeneralHitAnalytic>(new(HttpStatusCode.NotFound));
        }

        var response = new ResponseGeneralHitAnalytic()
        {
            Match_Percentage = result.Match_Percent,
            Unmatch_Percentage = result.Unmatch_Percent,
        };

        return Result.Success(response);
    }
}
