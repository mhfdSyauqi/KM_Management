using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Roles.Models;
using KM_Management.Shared;

namespace KM_Management.EndPoint.Roles.Query;

public record GetUserRoleQuery(string LoginName) : IQuery<ResponseUserRole>;

public class GetUserRoleHandler : IQueryHandler<GetUserRoleQuery, ResponseUserRole>
{
	private readonly IRolesRepository _rolesRepository;

	public GetUserRoleHandler(IRolesRepository rolesRepository)
	{
		_rolesRepository = rolesRepository;
	}

	public async Task<Result<ResponseUserRole>> Handle(GetUserRoleQuery request, CancellationToken cancellationToken)
	{
		var result = await _rolesRepository.GetUserRoleByLoginNameAsync(request.LoginName, cancellationToken);

		var response = new ResponseUserRole()
		{
			User_Name = request.LoginName,
			Role = result == null ? "User" : result
		};


		return Result.Success(response);
	}
}