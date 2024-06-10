using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Roles.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Roles.Command;

public record PostUserRolesCommand(RequestUserRole Argument) : ICommand;

public class PostUserRolesValidator : AbstractValidator<PostUserRolesCommand>
{
	public PostUserRolesValidator()
	{

	}
}

public class PostUserRolesHandler : ICommandHandler<PostUserRolesCommand>
{
	private readonly IRolesRepository _rolesRepository;
	private readonly IValidator<PostUserRolesCommand> _validator;

	public PostUserRolesHandler(IRolesRepository rolesRepository, IValidator<PostUserRolesCommand> validator)
	{
		_rolesRepository = rolesRepository;
		_validator = validator;
	}

	public async Task<Result> Handle(PostUserRolesCommand request, CancellationToken cancellationToken)
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

		await _rolesRepository.PostUserRole(postRoles, cancellationToken);
		return Result.Success();
	}
}
