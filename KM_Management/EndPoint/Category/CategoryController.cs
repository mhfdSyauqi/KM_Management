using KM_Management.Controllers;
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

    [HttpPost]
    [Route("GetCategoryTopIssueSelected")]
    public async Task<IActionResult> GetCategoryTopIssueSelected([FromBody] RequestCategoryTopIssueSelected request, CancellationToken cancellationToken)
    {
        var query = new GetCategoryTopIssueSelectedQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpPost]
    [Route("GetCategoryTopIssueAvailable")]
    public async Task<IActionResult> GetCategoryTopIssueAvailable([FromBody] RequestCategoryTopIssueAvailable request, CancellationToken cancellationToken)
    {
        var query = new GetCategoryTopIssueAvailableQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpPost]
    [Route("AddSelectedTopIssue")]
    public async Task<IActionResult> PostCategoryTopIssueSelected([FromBody] RequestPostCategoryTopIssueSelected request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Create_By = computerName.Split("\\")[1];
        request.Create_At = DateTime.Now;
        var command = new PostCategoryTopIssueSelectedCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);
        return result.MapResponse();
    }

    [HttpPost]
    [Route("AddAllSelectedTopIssue")]
    public async Task<IActionResult> PostCategoryTopIssueAllSelected([FromBody] RequestPostCategoryTopIssueAllSelected request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Create_By = computerName.Split("\\")[1];
        request.Create_At = DateTime.Now;
        var command = new PostCategoryTopIssueAllSelectedCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);
        return result.MapResponse();
    }

    [HttpPatch]
    [Route("RemoveSelectedTopIssue")]
    public async Task<IActionResult> DeleteCategoryTopIssueSelected([FromBody] RequestDeleteCategoryTopIssueSelected request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Modified_By = computerName.Split("\\")[1];
        request.Modified_At = DateTime.Now;
        var command = new DeleteCategoryTopIssueSelectedCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);
        return result.MapResponse();
    }

    [HttpPatch]
    [Route("RemoveAllSelectedTopIssue")]
    public async Task<IActionResult> DeleteCategoryTopIssueAllSelected([FromBody] RequestDeleteCategoryTopIssueAllSelected request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Modified_By = computerName.Split("\\")[1];
        request.Modified_At = DateTime.Now;
        var command = new DeleteCategoryTopIssueAllSelectedCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);
        return result.MapResponse();
    }

    [HttpPatch]
    [Route("PatchSequenceSelectedTopIssue")]
    public async Task<IActionResult> PatchSequenceCategoryTopIssueSelected([FromBody] RequestPatchCategoryTopIssueSelectedSequence request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Modified_By = computerName.Split("\\")[1];
        request.Modified_At = DateTime.Now;
        var command = new PatchCategoryTopIssueSelectedSequenceCommand(request);
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
