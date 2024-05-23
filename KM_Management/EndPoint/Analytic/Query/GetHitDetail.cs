using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
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

        var filter = new FilterDetailHitAnalytic()
        {
            Type = request.Request.Type ?? "unmatch",
            Start_Date = startDate,
            End_Date = endDate
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
