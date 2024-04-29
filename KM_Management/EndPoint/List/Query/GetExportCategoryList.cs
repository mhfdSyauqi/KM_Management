using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.List.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.List.Query;

public record GetExportCategoryListQuery(RequestExportCategoryList Argument) : IQuery<ResponseExportCategoryList>;

public class GetExportCategoryListValidator : AbstractValidator<GetExportCategoryListQuery>
{
    public GetExportCategoryListValidator()
    {

    }
}
public class GetExportCategoryListHandler : IQueryHandler<GetExportCategoryListQuery, ResponseExportCategoryList>
{
    private readonly IListRepository _categoriesRepository;

    public GetExportCategoryListHandler(IListRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<Result<ResponseExportCategoryList>> Handle(GetExportCategoryListQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterExportCategoryList()
        {
            Is_Active = request.Argument.Is_Active

        };

        var FirstCategories = await _categoriesRepository.GetExportCategoriesFirstLayerAsync(filter.Is_Active, cancellationToken);
        var SecondCategories = await _categoriesRepository.GetExportCategoriesSecondLayerAsync(filter.Is_Active, cancellationToken);
        var ThirdCategories = await _categoriesRepository.GetExportCategoriesThirdLayerAsync(filter.Is_Active, cancellationToken);
        if (FirstCategories == null || SecondCategories == null || ThirdCategories == null)
        {
            return Result.Failure<ResponseExportCategoryList>(new(HttpStatusCode.NotFound, default));
        }

        var response = new ResponseExportCategoryList()
        {
            First_Layer = FirstCategories.Select(col => new First_Layers()
            {
                Layer_One = col.Layer_One,
                Status = col.Status,
                Publication = col.Publication,
            }).ToList(),
            Second_Layer = SecondCategories.Select(col => new Second_Layers()
            {
                Layer_Two = col.Layer_Two,
                Layer_One = col.Layer_One,
                Status = col.Status,
                Publication = col.Publication,
            }).ToList(),

            Third_Layer = ThirdCategories.Select(col => new Third_Layers()
            {
                Layer_Three = col.Layer_Three,
                Layer_Two = col.Layer_Two,
                Layer_One = col.Layer_One,
                Status = col.Status,
                Publication = col.Publication,
            }).ToList(),
        };
        return Result.Success(response);
    }
}

