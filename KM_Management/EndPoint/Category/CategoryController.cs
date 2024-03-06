﻿using KM_Management.Controllers;
using KM_Management.EndPoint.Category.Command;
using KM_Management.EndPoint.Category.Models;
using KM_Management.EndPoint.Category.Query;
using KM_Management.EndPoint.Content.Command;
using KM_Management.EndPoint.Content.Models;
using KM_Management.EndPoint.Message.Command;
using KM_Management.EndPoint.Message.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.EndPoint.Category;

public class CategoryController : MyAPIController
{
    public CategoryController(IMediator Mediator) : base(Mediator)
    {
    }

    [HttpPost]
    [Route("GetCategoryList")]
    public async Task<IActionResult> GetCategoryList([FromBody] RequestCategoryList request, CancellationToken cancellationToken)
    {
        var query = new GetCategoryListQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpPost]
    [Route("AddNewCategory")]
    public async Task<IActionResult> PostContent([FromBody] RequestPostCategoryList request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Create_By = computerName.Split("\\")[1];
        request.Create_At = DateTime.Now;
        request.Uid = Guid.NewGuid().ToString("N");
        var command = new PostCategoryListCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);

        return result.MapResponse();
    }


    [HttpPatch]
    [Route("UpdateCategoryList")]

    public async Task<IActionResult> PatchCategoryList([FromBody] RequestPatchCategoryList request, CancellationToken cancellationToken)
    {

        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Modified_By = computerName.Split("\\")[1];
        request.Modified_At = DateTime.Now;
        var command = new PatchCategoryListCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);
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
