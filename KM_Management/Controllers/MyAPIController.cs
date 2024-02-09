using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MyAPIController : ControllerBase
{
    protected readonly IMediator _Mediator;
    public MyAPIController(IMediator Mediator)
    {
        _Mediator = Mediator;
    }
}
