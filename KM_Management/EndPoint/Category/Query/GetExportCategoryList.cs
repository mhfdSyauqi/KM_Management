using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category.Models;
using KM_Management.EndPoint.RateAndFeedback.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Query;

public record GetExportCategoryListQuery(RequestExportCategoryList Argument) : IQuery<ResponseExportCategoryList>;

public class GetExportCategoryListValidator : AbstractValidator<GetExportCategoryListQuery>
{
    public GetExportCategoryListValidator()
    {

    }
}
public class GetExportCategoryListHandler : IQueryHandler<GetExportCategoryListQuery, ResponseExportCategoryList>
{
    private readonly ICategoryRepository _categoriesRepository;

    public GetExportCategoryListHandler(ICategoryRepository categoriesRepository)
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
                Layer_One = col.Layer_One,
                Layer_Two = col.Layer_Two,
                Status = col.Status,
                Publication = col.Publication,
            }).ToList(),

            Third_Layer = ThirdCategories.Select(col => new Third_Layers()
            {
                Layer_One = col.Layer_One,
                Layer_Two = col.Layer_Two,
                Layer_Three = col.Layer_Three,
                Status = col.Status,
                Publication = col.Publication,
            }).ToList(),
        };
        return Result.Success(response);
    }
}

