using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Roles.Models;
using KM_Management.Shared;

namespace KM_Management.EndPoint.Roles.Query;

public record GetUserReferenceQuery(string? Keywords) : IQuery<List<ResponseUserReference>>;

public class GetUserReferenceHandler : IQueryHandler<GetUserReferenceQuery, List<ResponseUserReference>>
{
	private readonly IRolesRepository _rolesRepository;

	public GetUserReferenceHandler(IRolesRepository rolesRepository)
	{
		_rolesRepository = rolesRepository;
	}

	public async Task<Result<List<ResponseUserReference>>> Handle(GetUserReferenceQuery request, CancellationToken cancellationToken)
	{
		var reference = await _rolesRepository.GetUserReferenceAsync(request.Keywords, cancellationToken);
		var response = reference.Select(col => new ResponseUserReference
		{
			Login_Name = col.Login_Name,
			Full_Name = col.Full_Name,
			Email = col.Email,
			Job_Title = col.Job_Title,
		}).ToList();

		return Result.Success(response);
	}
}
