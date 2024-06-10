using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Helper;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Analytic.Query;

public record GetHitDetailQuery(RequestHitAnalytic Request) : IQuery<ResponseDetailHitAnalytic>;

public class GetHitDetailValidator : AbstractValidator<GetHitDetailQuery>
{
    public GetHitDetailValidator()
    {
        RuleFor(key => key.Request.Type).NotNull().Must(col => col.Contains("match") || col.Contains("unmach")).WithMessage("please insert correct type");
    }
}

public class GetHitDetailHandler : IQueryHandler<GetHitDetailQuery, ResponseDetailHitAnalytic>
{
    private readonly IAnalyticRepository _analyticRepository;
    private readonly IValidator<GetHitDetailQuery> _validator;

    public GetHitDetailHandler(IAnalyticRepository analyticRepository, IValidator<GetHitDetailQuery> validator)
    {
        _analyticRepository = analyticRepository;
        _validator = validator;
    }

    public async Task<Result<ResponseDetailHitAnalytic>> Handle(GetHitDetailQuery request, CancellationToken cancellationToken)
    {
        var validator = await _validator.ValidateAsync(request, cancellationToken);

        if (!validator.IsValid)
        {
            var errMsg = validator.Errors.First().ErrorMessage;
            return Result.Failure<ResponseDetailHitAnalytic>(new(HttpStatusCode.BadRequest, errMsg));
        }

        var filteredDate = DateFilter.GenerateFilterDate(request.Request.Filter, request.Request.Start_Date, request.Request.End_Date);

        var filter = new FilterDetailHitAnalytic()
        {
            Type = request.Request.Type ?? "unmatch",
            Start_Date = filteredDate.StartDate,
            End_Date = filteredDate.EndDate
        };

        var result = await _analyticRepository.GetHitDetailAsync(filter, cancellationToken);

        if (result == null)
        {
            return Result.Failure<ResponseDetailHitAnalytic>(new(HttpStatusCode.NotFound));
        }

        var response = new ResponseDetailHitAnalytic()
        {
            Total_Percentage = result.First().Percent,
            Categories = result.Select(col => new ResponseDetailHitItem()
            {
                Category = col.Name,
                Count = col.Count
            }).Take(10).ToList()
        };

        return Result.Success(response);
    }
}
