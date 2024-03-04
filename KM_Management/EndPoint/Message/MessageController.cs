using KM_Management.Controllers;
using KM_Management.EndPoint.Content.Command;
using KM_Management.EndPoint.Content.Models;
using KM_Management.EndPoint.Message.Command;
using KM_Management.EndPoint.Message.Models;
using KM_Management.EndPoint.Message.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.EndPoint.Message;

public class MessageController : MyAPIController
{
    public MessageController(IMediator Mediator) : base(Mediator)
    {
    }


    [HttpPost]
    [Route("GetMessage")]
    public async Task<IActionResult> GetActiveMessageByType([FromBody] RequestActiveMessage request, CancellationToken cancellationToken)
    {
        var query = new GetActiveMessageQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpPost]
    [Route("AddNewMessage")]
    public async Task<IActionResult> PostMessage([FromBody] RequestPostMessage request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Create_By = computerName.Split("\\")[1];
        request.Create_At = DateTime.Now;
        var command = new PostMessageCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);

        return result.MapResponse();
    }

    [HttpPatch]
    [Route("UpdateSetupMessage")]

    public async Task<IActionResult> PostMessage([FromBody] RequestPatchMessage request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Modified_By = computerName.Split("\\")[1];
        request.Modified_At = DateTime.Now;
        var command = new PatchMessageCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);
        return result.MapResponse();
    }

    [HttpPatch]
    [Route("SoftDeleteMessage")]

    public async Task<IActionResult> DeleteMessage([FromBody] RequestDeleteMessage request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Modified_By = computerName.Split("\\")[1];
        request.Modified_At = DateTime.Now;
        var command = new DeleteMessageCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);
        return result.MapResponse();
    }

    [HttpPatch]
    [Route("UpdateSequenceMessage")]

    public async Task<IActionResult> PatchSequenceMessage([FromBody] RequestPatchSequenceMessage request, CancellationToken cancellationToken)
    {
        string computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
        request.Modified_By = computerName.Split("\\")[1];
        request.Modified_At = DateTime.Now;
        var command = new PatchSequenceMessageCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);
        return result.MapResponse();
    }



}
