using KM_Management.Controllers;
using KM_Management.EndPoint.AssistantProfile.Command;
using KM_Management.EndPoint.AssistantProfile.Models;
using KM_Management.EndPoint.AssistantProfile.Query;
using KM_Management.EndPoint.Content.Command;
using KM_Management.EndPoint.Content.Models;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.EndPoint.AssistantProfile;

public class AssistantProfileController : MyAPIController
{
    public AssistantProfileController(IMediator Mediator) : base(Mediator)
    {
    }

    [HttpGet("GetAssistantProfile")]
    public async Task<IActionResult> GetAssistantProfile(CancellationToken cancellationToken)
    {
        var query = new GetAssistantProfileQuery();
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpPost]
    [Route("AddAssistantProfile")]
    public async Task<IActionResult> PostContent([FromForm] RequestPostAssistantProfile request, CancellationToken cancellationToken)
    {
        var httpContext = HttpContext; 

        var command = new PostAssistantProfileCommand(request, httpContext);

        var result = await _Mediator.Send(command, cancellationToken);

        return result.MapResponse();
    }

}
