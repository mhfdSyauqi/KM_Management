using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Query;

public record GetCategoryListQuery(RequestCategoryList Argument) : IQuery<List<ResponseCategoryList>>;

public class GetCategoryListValidator : AbstractValidator<GetCategoryListQuery>
{
    public GetCategoryListValidator()
    {

    }
}
public class GetCategoryListHandler : IQueryHandler<GetCategoryListQuery, List<ResponseCategoryList>>
{
    private readonly ICategoryRepository _categoriesRepository;

    public GetCategoryListHandler(ICategoryRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<Result<List<ResponseCategoryList>>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterCategoryList()
        {
            Uid_Reference = request.Argument.Uid_Reference, 
            Is_Active = request.Argument.Is_Active,
            Layer = request.Argument.Layer

        };

        Guid? uidReference = !string.IsNullOrEmpty(filter.Uid_Reference)
                         ? Guid.Parse(request.Argument.Uid_Reference)
                         : (Guid?)null;

        var categories = await _categoriesRepository.GetCategoryListAsync(uidReference, filter.Layer, filter.Is_Active, cancellationToken);;

        if (categories == null)
        {
            return Result.Failure<List<ResponseCategoryList>>(new(HttpStatusCode.NotFound, default));
        }

        var response = categories.Select(item => new ResponseCategoryList()
        {
            Uid = item.Uid.ToString("N"),
            Name = item.Name,
            Is_Active = item.Is_Active,
        }).ToList();
        return Result.Success(response);
    }
}

