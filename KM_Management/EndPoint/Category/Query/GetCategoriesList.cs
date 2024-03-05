using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Query;

public record GetCategoriesListQuery(RequestCategoriesList Argument) : IQuery<List<ResponseCategoriesList>>;

public class GetCategoriesListValidator : AbstractValidator<GetCategoriesListQuery>
{
    public GetCategoriesListValidator()
    {

    }
}

//public class GetCategoriesListHandler : IQueryHandler<GetCategoriesListQuery, List<ResponseCategoriesList>>
//{
//    private readonly ICategoryRepository _categoriesRepository;

//    public GetCategoriesListHandler(ICategoryRepository categoriesRepository)
//    {
//        _categoriesRepository = categoriesRepository;
//    }

//    public async Task<Result<List<ResponseCategoriesList>>> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
//    {
//        var filter = new FilterCategoriesList()
//        {
//            Uid_Reference = request.Argument.Uid_Reference,
//            Is_Active = request.Argument.Is_Active,
//            Layer = request.Argument.Layer

//        };

//        var categories = await _categoriesRepository.GetCategoriesListAsync(filter.Uid_Reference, filter.Layer, filter.Is_Active, cancellationToken);

//        if (categories == null)
//        {
//            return Result.Failure<List<ResponseCategoriesList>>(new(HttpStatusCode.NotFound, default));
//        }

//        var response = categories.Select(item => new ResponseCategoriesList()
//        {
//            Uid = item.Uid.ToString("N"),
//            Name = item.Name,
//            Is_Active = item.Is_Active,
//        }).ToList();
//        return Result.Success(response);
//    }
//}

public class GetCategoriesListHandler : IQueryHandler<GetCategoriesListQuery, List<ResponseCategoriesList>>
{
    private readonly ICategoryRepository _categoriesRepository;

    public GetCategoriesListHandler(ICategoryRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<Result<List<ResponseCategoriesList>>> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterCategoriesList()
        {
            Uid_Reference = request.Argument.Uid_Reference,
            Is_Active = request.Argument.Is_Active,
            Layer = request.Argument.Layer

        };

        var categories = await _categoriesRepository.GetCategoriesListAsync(filter.Uid_Reference, filter.Layer, filter.Is_Active, cancellationToken);

        if (categories == null)
        {
            return Result.Failure<List<ResponseCategoriesList>>(new(HttpStatusCode.NotFound, default));
        }

        var response = categories.Select(item => new ResponseCategoriesList()
        {
            Uid = item.Uid.ToString("N"),
            Name = item.Name,
            Is_Active = item.Is_Active,
        }).ToList();
        return Result.Success(response);
    }
}

