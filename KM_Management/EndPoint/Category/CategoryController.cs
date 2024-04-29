using Azure.Core;
using KM_Management.Controllers;
using KM_Management.EndPoint.Category.Query;
using KM_Management.EndPoint.List.Models;
using KM_Management.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace KM_Management.EndPoint.Category;

public class CategoryController : MyAPIController
{
    public CategoryController(IMediator Mediator) : base(Mediator)
    {
    }


    [HttpGet("ref")]
    public async Task<IActionResult> GetCategoryReference(CancellationToken cancellationToken)
    {
        var query = new GetCategoryReferenceQuery();
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }
}
