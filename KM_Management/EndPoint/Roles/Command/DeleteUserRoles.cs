using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Roles.Command;

public record DeleteUserRolesCommand(string UserName) : ICommand;

public class DeleteUserRolesValidator : AbstractValidator<DeleteUserRolesCommand>
{
	public DeleteUserRolesValidator()
	{

	}
}

public class DeleteUserRolesHandler : ICommandHandler<DeleteUserRolesCommand>
{
	private readonly IRolesRepository _rolesRepository;
	private readonly IValidator<DeleteUserRolesCommand> _validator;

	public DeleteUserRolesHandler(IRolesRepository rolesRepository, IValidator<DeleteUserRolesCommand> validator)
	{
		_rolesRepository = rolesRepository;
		_validator = validator;
	}

	public async Task<Result> Handle(DeleteUserRolesCommand request, CancellationToken cancellationToken)
	{
		var validation = await _validator.ValidateAsync(request, cancellationToken);

		if (!validation.IsValid)
		{
			var errorMsg = validation.Errors.First()?.ErrorMessage;
			return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
		}


		await _rolesRepository.DeleteUserRole(request.UserName, cancellationToken);
		return Result.Success();
	}
}
