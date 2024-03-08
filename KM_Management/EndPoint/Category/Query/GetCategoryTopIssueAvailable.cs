﻿using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Query;

public record GetCategoryTopIssueAvailableQuery(RequestCategoryTopIssueAvailable Argument) : IQuery<List<ResponseCategoryTopIssueAvailable>>;

public class GetCategoryTopIssueAvailableValidator : AbstractValidator<GetCategoryTopIssueAvailableQuery>
{
    public GetCategoryTopIssueAvailableValidator()
    {

    }
}
public class GetCategoryTopIssueAvailableHandler : IQueryHandler<GetCategoryTopIssueAvailableQuery, List<ResponseCategoryTopIssueAvailable>>
{
    private readonly ICategoryRepository _categoriesRepository;

    public GetCategoryTopIssueAvailableHandler(ICategoryRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<Result<List<ResponseCategoryTopIssueAvailable>>> Handle(GetCategoryTopIssueAvailableQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterCategoryTopIssueAvailable()
        {
            Is_Active = request.Argument.Is_Active,

        };

        var categoryTopIssue = await _categoriesRepository.GetCategoryTopIssueAvailableAsync(filter.Is_Active,cancellationToken);

        if (categoryTopIssue == null)
        {
            return Result.Failure<List<ResponseCategoryTopIssueAvailable>>(new(HttpStatusCode.NotFound, default));
        }

        var response = categoryTopIssue.Select(item => new ResponseCategoryTopIssueAvailable()
        {
            Uid = item.Uid.ToString("N"),
            Name = item.Name,
            Is_Active = item.Is_Active,
        }).ToList();
        return Result.Success(response);
    }
}

