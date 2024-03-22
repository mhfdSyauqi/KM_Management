using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Roles.Models;
using KM_Management.Shared;

namespace KM_Management.EndPoint.Roles.Query;

public record GetUserRoleQuery(string UserName) : IQuery<ResponseUserRole>;

public class GetUserRoleHandler : IQueryHandler<GetUserRoleQuery, ResponseUserRole>
{
	public async Task<Result<ResponseUserRole>> Handle(GetUserRoleQuery request, CancellationToken cancellationToken)
	{
		var response = new ResponseUserRole()
		{
			User_Name = request.UserName,
            //Role = "Admin"
            Role = "Super"
        };


		return Result.Success(response);
	}
}