using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Roles.Models;
using KM_Management.Shared;

namespace KM_Management.EndPoint.Roles.Query;

public record GetUsersRoleQuery(RequestUsersRole Argument) : IQuery<ResponseUsersRole>;

public class GetUsersRoleValidator : AbstractValidator<GetUsersRoleQuery>
{
	public GetUsersRoleValidator()
	{

	}
}

public class GetUsersRoleHandler : IQueryHandler<GetUsersRoleQuery, ResponseUsersRole>
{
	private readonly IRolesRepository _rolesRepository;
	private readonly IValidator<GetUsersRoleQuery> _validator;

	public GetUsersRoleHandler(IRolesRepository rolesRepository, IValidator<GetUsersRoleQuery> validator)
	{
		_rolesRepository = rolesRepository;
		_validator = validator;
	}

	public async Task<Result<ResponseUsersRole>> Handle(GetUsersRoleQuery request, CancellationToken cancellationToken)
	{
		await _validator.ValidateAndThrowAsync(request, cancellationToken);

		var filter = new FilterUsersRole()
		{
			Search_Keyword = request.Argument.Search_Keyword,
			Current_Page = request.Argument.Searched_Page,
			Page_Limit = request.Argument.Limit_Page,
			Sort_Order = request.Argument.Order_Sort,
			Sort_Column = request.Argument.Column_Sort
		};

		var usersRole = await _rolesRepository.GetUsersRoleAsync(filter, cancellationToken);

		var response = new ResponseUsersRole()
		{
			Items = usersRole.Select(col => new UsersRole()
			{
				Login_Name = col.Login_Name,
				Full_Name = col.Full_Name,
				Role = col.Roles,
				Email = col.Email,
				Job_Title = col.Job_Title
			}).ToList(),
			Total_Row = usersRole.FirstOrDefault()?.Total_Row,
			Curr_Page = usersRole.FirstOrDefault()?.Curr_Page,
			Next_Page = usersRole.FirstOrDefault()?.Next_Page,
			Prev_Page = usersRole.FirstOrDefault()?.Prev_Page,
			Max_Page = usersRole.FirstOrDefault()?.Max_Page,
		};

		return Result.Success(response);

	}
}
