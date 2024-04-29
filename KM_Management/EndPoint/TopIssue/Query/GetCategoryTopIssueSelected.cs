using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.TopIssue;
using KM_Management.EndPoint.TopIssue.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.TopIssue.Query;

public record GetCategoryTopIssueSelectedQuery(RequestCategoryTopIssueSelected Argument) : IQuery<List<ResponseCategoryTopIssueSelected>>;

public class GetCategoryTopIssueSelectedValidator : AbstractValidator<GetCategoryTopIssueSelectedQuery>
{
    public GetCategoryTopIssueSelectedValidator()
    {

    }
}
public class GetCategoryTopIssueSelectedHandler : IQueryHandler<GetCategoryTopIssueSelectedQuery, List<ResponseCategoryTopIssueSelected>>
{
    private readonly ITopIssueRepository _topIssueRepository;

    public GetCategoryTopIssueSelectedHandler(ITopIssueRepository categoriesRepository)
    {
        _topIssueRepository = categoriesRepository;
    }

    public async Task<Result<List<ResponseCategoryTopIssueSelected>>> Handle(GetCategoryTopIssueSelectedQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterCategoryTopIssueSelected()
        {
            Is_Active = request.Argument.Is_Active,

        };

        var categoryTopIssue = await _topIssueRepository.GetCategoryTopIssueSelectedAsync(filter.Is_Active, cancellationToken);

        if (categoryTopIssue == null)
        {
            return Result.Failure<List<ResponseCategoryTopIssueSelected>>(new(HttpStatusCode.NotFound, default));
        }

        var response = categoryTopIssue.Select(item => new ResponseCategoryTopIssueSelected()
        {
            Uid = item.Uid.ToString("N"),
            Name = item.Name,
            Is_Active = item.Is_Active,
            Sequence = item.Sequence,
            Uid_Bot_Category = item.Uid_Bot_Category.ToString("N")
        }).ToList();
        return Result.Success(response);
    }
}

