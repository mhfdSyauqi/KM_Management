using KM_Management.Controllers;
using KM_Management.EndPoint.Roles.Command;
using KM_Management.EndPoint.Roles.Models;
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
		var loginName = computerName.Split("\\")[1];

		var query = new GetUserRoleQuery(loginName);
		var result = await _Mediator.Send(query, cancellationToken);

		return result.MapResponse();
	}

	[HttpPost]
	public async Task<IActionResult> GetUsersRoleWithFilter([FromBody] RequestUsersRole request, CancellationToken cancellationToken)
	{
		var query = new GetUsersRoleQuery(request);
		var result = await _Mediator.Send(query, cancellationToken);

		return result.MapResponse();
	}

	[HttpGet("ref/{keywords?}")]
	public async Task<IActionResult> GetUserReference(string? keywords, CancellationToken cancellationToken)
	{
		var query = new GetUserReferenceQuery(keywords);
		var result = await _Mediator.Send(query, cancellationToken);

		return result.MapResponse();
	}

	[HttpPost("action")]
	public async Task<IActionResult> PostUserRole([FromBody] RequestUserRole request, CancellationToken cancellationToken)
	{
		var command = new PostUserRolesCommand(request);
		var result = await _Mediator.Send(command, cancellationToken);

		return result.MapResponse();
	}

	[HttpPatch("action")]
	public async Task<IActionResult> PatchUserRole([FromBody] RequestUserRole request, CancellationToken cancellationToken)
	{
		var command = new PatchUserRolesCommand(request);
		var result = await _Mediator.Send(command, cancellationToken);

		return result.MapResponse();
	}

	[HttpDelete("action/{userName}")]
	public async Task<IActionResult> DeleteUserRole(string userName, CancellationToken cancellationToken)
	{
		var command = new DeleteUserRolesCommand(userName);
		var result = await _Mediator.Send(command, cancellationToken);

		return result.MapResponse();
	}
}
