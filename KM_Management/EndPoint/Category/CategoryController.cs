using KM_Management.Controllers;
using KM_Management.EndPoint.Category.Models;
using KM_Management.EndPoint.Category.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.EndPoint.Category;

public class CategoryController : MyAPIController
{
    public CategoryController(IMediator Mediator) : base(Mediator)
    {
    }

    [HttpPost]
    [Route("GetCategoriesList")]
    public async Task<IActionResult> GetCategoriesList([FromBody] RequestCategoriesList request, CancellationToken cancellationToken)
    {
        var query = new GetCategoriesListQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpGet("ref")]
    public async Task<IActionResult> GetCategoryReference(CancellationToken cancellationToken)
    {
        var query = new GetCategoryReferenceQuery();
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }
}
