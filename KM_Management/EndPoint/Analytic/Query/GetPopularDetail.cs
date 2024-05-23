using FluentValidation;
using KM_Management.Commons.FluentExtention;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Analytic.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Analytic.Query;

public record GetPopularDetailQuery(RequestPopularAnalytic Request) : IQuery<List<ResponseDetailPopularAnalytic>>;

public class GetPopularDetailValidator : AbstractValidator<GetPopularDetailQuery>
{
    public GetPopularDetailValidator()
    {
        RuleFor(key => key.Request.Reference).NotNull();

        When(col => !string.IsNullOrEmpty(col.Request.Reference), () =>
        {
            RuleFor(key => key.Request.Reference).BeValidGuid();
        });

    }
}

public class GetPopularDetailHandler : IQueryHandler<GetPopularDetailQuery, List<ResponseDetailPopularAnalytic>>
{
    private readonly IAnalyticRepository _analyticRepository;
    private readonly IValidator<GetPopularDetailQuery> _validator;

    public GetPopularDetailHandler(IAnalyticRepository analyticRepository, IValidator<GetPopularDetailQuery> validator)
    {
        _analyticRepository = analyticRepository;
        _validator = validator;
    }

    public async Task<Result<List<ResponseDetailPopularAnalytic>>> Handle(GetPopularDetailQuery request, CancellationToken cancellationToken)
    {
        var validator = await _validator.ValidateAsync(request, cancellationToken);

        if (!validator.IsValid)
        {
            var errMsg = validator.Errors?.First()?.ErrorMessage;
            return Result.Failure<List<ResponseDetailPopularAnalytic>>(new(HttpStatusCode.BadRequest, errMsg));
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

        var filter = new FilterDetailPopularAnalytic()
        {
            Reference = Guid.Parse(request.Request.Reference),
            Start_Date = startDate,
            End_Date = endDate
        };

        var result = await _analyticRepository.GetPopularDetailAsync(filter, cancellationToken);

        var response = result.Select(col => new ResponseDetailPopularAnalytic()
        {
            Reference = col.Uid.ToString("N"),
            Name = col.Name,
            Count = col.Total,
            Percent = (double)col.Total / (double)result.Sum(key => key.Total) * 100
        }).ToList();

        return Result.Success(response);
    }
}
