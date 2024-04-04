using KM_Management.Controllers;
using KM_Management.EndPoint.General.Command;
using KM_Management.EndPoint.General.Models;
using KM_Management.EndPoint.General.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.EndPoint.General;

public class GeneralController : MyAPIController
{
    public GeneralController(IMediator Mediator) : base(Mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetConfigGeneral(CancellationToken cancellationToken)
    {
        var query = new GetConfigGeneralQuery();
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }

    [HttpPatch]
    public async Task<IActionResult> PatchConfigGeneral([FromBody] RequestConfigGeneral request, CancellationToken cancellationToken)
    {
        var command = new PatchConfigGeneralCommand(request);
        var result = await _Mediator.Send(command, cancellationToken);

        return result.MapResponse();
    }
}
