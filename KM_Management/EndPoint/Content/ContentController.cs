using KM_Management.Controllers;
using KM_Management.EndPoint.Content.Command;
using KM_Management.EndPoint.Content.Models;
using KM_Management.EndPoint.Content.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.EndPoint.Content;

public class ContentController : MyAPIController
{
    public ContentController(IMediator Mediator) : base(Mediator)
    {
    }

    [HttpGet("{ContentId}")]
    public async Task<IActionResult> GetContentById(string ContentId, CancellationToken cancellationToken)
    {
        var query = new GetDetailContentQuery(ContentId);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }


    [HttpPost]
    public async Task<IActionResult> GetContentsWithFilter([FromBody] RequestContent request, CancellationToken cancellationToken)
    {
        var query = new GetContentsQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }


    [HttpPost]
    [Route("action")]
    public async Task<IActionResult> PostContent([FromBody] RequestPostContent request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Create_By = computerName.Split("\\")[1];

        var command = new PostContentCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);

        return result.MapResponse();
    }

    [HttpPatch]
    [Route("action")]

    public async Task<IActionResult> PatchContentById([FromBody] RequestPatchContent request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Modified_By = computerName.Split("\\")[1];

        var command = new PatchContentCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);

        return result.MapResponse();
    }
}
