using Azure.Core;
using KM_Management.Controllers;
using KM_Management.EndPoint.TopIssue.Command;
using KM_Management.EndPoint.TopIssue.Models;
using KM_Management.EndPoint.TopIssue.Query;
using KM_Management.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace KM_Management.EndPoint.TopIssue;

public class TopIssueController : MyAPIController
{
    public TopIssueController(IMediator Mediator) : base(Mediator)
    {
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
        request.Modified_By = computerName.Split("\\")[1];
        request.Modified_At = DateTime.Now;
        var command = new PostCategoryTopIssueSelectedCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);
        return result.MapResponse();
    }

    [HttpPost]
    [Route("AddAllSelectedTopIssue")]
    public async Task<IActionResult> PostCategoryTopIssueAllSelected(CancellationToken cancellationToken)
    {
        var request = new RequestPostCategoryTopIssueAllSelected();
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Create_By = computerName.Split("\\")[1];
        request.Create_At = DateTime.Now;
        request.Modified_By = computerName.Split("\\")[1];
        request.Modified_At = DateTime.Now;
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
    public async Task<IActionResult> DeleteCategoryTopIssueAllSelected(CancellationToken cancellationToken)
    {
        var request = new RequestDeleteCategoryTopIssueAllSelected();
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


}
