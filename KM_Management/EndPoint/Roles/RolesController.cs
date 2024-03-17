using KM_Management.Controllers;
using KM_Management.EndPoint.Roles.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KM_Management.EndPoint.Roles;

public class RolesController : MyAPIController
{
	public RolesController(IMediator Mediator) : base(Mediator)
	{
	}

	[HttpGet]
	public async Task<IActionResult> GetUserRole(CancellationToken cancellationToken)
	{
		var computerName = User.Identity?.Name ?? "Error\\NotAuthUser";
		var userName = computerName.Split("\\")[1];

		var query = new GetUserRoleQuery(userName);
		var result = await _Mediator.Send(query, cancellationToken);

		return result.MapResponse();
	}
}
