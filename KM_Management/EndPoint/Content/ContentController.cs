using KM_Management.Controllers;
using KM_Management.EndPoint.Content.Moldels;
using KM_Management.EndPoint.Content.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.EndPoint.Content;

public class ContentController : MyAPIController
{
    public ContentController(IMediator Mediator) : base(Mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> GetContents([FromBody] RequestContent request, CancellationToken cancellationToken)
    {
        var query = new GetContentsQuery(request);
        var result = await _Mediator.Send(query, cancellationToken);

        return result.MapResponse();
    }
}
