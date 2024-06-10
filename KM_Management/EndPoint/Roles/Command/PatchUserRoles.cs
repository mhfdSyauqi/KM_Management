using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Roles.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Roles.Command;

public record PatchUserRolesCommand(RequestUserRole Argument) : ICommand;

public class PatchUserRolesValidator : AbstractValidator<PatchUserRolesCommand>
{
	public PatchUserRolesValidator()
	{

	}
}

public class PatchUserRolesHandler : ICommandHandler<PatchUserRolesCommand>
{
	private readonly IRolesRepository _rolesRepository;
	private readonly IValidator<PatchUserRolesCommand> _validator;

	public PatchUserRolesHandler(IRolesRepository rolesRepository, IValidator<PatchUserRolesCommand> validator)
	{
		_rolesRepository = rolesRepository;
		_validator = validator;
	}

	public async Task<Result> Handle(PatchUserRolesCommand request, CancellationToken cancellationToken)
	{
		var validation = await _validator.ValidateAsync(request, cancellationToken);

		if (!validation.IsValid)
		{
			var errorMsg = validation.Errors.First()?.ErrorMessage;
			return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
		}

		var postRoles = new EntityUserRole()
		{
			Login_Name = request.Argument.User_Name,
			Role = request.Argument.Role,
		};

		await _rolesRepository.PatchUserRole(postRoles, cancellationToken);
		return Result.Success();
	}
}
