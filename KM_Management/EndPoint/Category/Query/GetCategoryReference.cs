using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Query;

public record GetCategoryReferenceQuery : IQuery<List<ResponseCategoriesReference>>;

public class GetCategoryReferenceHandler : IQueryHandler<GetCategoryReferenceQuery, List<ResponseCategoriesReference>>
{
    private readonly ICategoryRepository _repository;

    public GetCategoryReferenceHandler(ICategoryRepository categoryRepository)
    {
        _repository = categoryRepository;
    }

    public async Task<Result<List<ResponseCategoriesReference>>> Handle(GetCategoryReferenceQuery request, CancellationToken cancellationToken)
    {
        var reference = await _repository.GetCategoryReferenceAsync(cancellationToken);
        if (reference == null)
        {
            return Result.Failure<List<ResponseCategoriesReference>>(new(HttpStatusCode.NotFound, default));
        }

        var response = reference.Select(item => new ResponseCategoriesReference() { Category_Id = item.Uid.ToString("N"), Category_Name = item.Name }).ToList();
        return Result.Success(response);
    }
}
