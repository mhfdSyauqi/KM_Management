using Dapper;
using KM_Management.Commons.Connection;
using KM_Management.EndPoint.Roles.Models;
using NetCore.AutoRegisterDi;

namespace KM_Management.EndPoint.Roles;

[RegisterAsScoped]
public class RolesRepository : IRolesRepository
{
	private readonly ISQLConnectionFactory _connection;

	public RolesRepository(ISQLConnectionFactory connection)
	{
		_connection = connection;
	}

	public async Task<IEnumerable<EntityUserReference>> GetUserReferenceAsync(string? keyword, CancellationToken cancellationToken)
	{
		await using var connection = await _connection.CreateConnectionAsync();

		var storeProcedureName = "[dbo].[Get_User_Role_Reference]";
		var command = new CommandDefinition(storeProcedureName, new { Keyword = keyword }, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
		var result = await connection.QueryAsync<EntityUserReference>(command);
		return result;
	}

	public async Task<string?> GetUserRoleByLoginNameAsync(string loginName, CancellationToken cancellationToken)
	{
		await using var connection = await _connection.CreateConnectionAsync();

		var query = @"
             SELECT [roles] FROM [dbo].[Bot_Roles] WHERE [login_name] = @Login_Name
        ";

		var command = new CommandDefinition(query, new { Login_Name = loginName });
		var result = connection.QueryFirstOrDefault<string?>(command);

		return result;
	}

	public async Task<IEnumerable<EntityUsersRole>> GetUsersRoleAsync(FilterUsersRole filter, CancellationToken cancellationToken)
	{
		await using var connection = await _connection.CreateConnectionAsync();

		var storeProcedureName = "[dbo].[Get_User_Roles]";
		var command = new CommandDefinition(storeProcedureName, filter, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
		var result = await connection.QueryAsync<EntityUsersRole>(command);
		return result;
	}

	public async Task<int> PostUserRole(EntityUserRole postUserRole, CancellationToken cancellationToken)
	{
		await using var connection = await _connection.CreateConnectionAsync();

		var query = @"
			INSERT INTO [dbo].[Bot_Roles] ([login_name],[roles])
			VALUES
				(@Login_Name,@Role)
		";

		var command = new CommandDefinition(query, postUserRole, cancellationToken: cancellationToken);
		var result = await connection.ExecuteAsync(command);

		return result;
	}

	public async Task<int> PatchUserRole(EntityUserRole patchUserRole, CancellationToken cancellationToken)
	{
		await using var connection = await _connection.CreateConnectionAsync();

		var query = @"
			UPDATE [dbo].[Bot_Roles]
				SET [roles] = @Role
			WHERE
				[login_name] = @Login_Name
		";

		var command = new CommandDefinition(query, patchUserRole, cancellationToken: cancellationToken);
		var result = await connection.ExecuteAsync(command);

		return result;
	}

	public async Task<int> DeleteUserRole(string userName, CancellationToken cancellationToken)
	{
		await using var connection = await _connection.CreateConnectionAsync();

		var query = @"
			DELETE [dbo].[Bot_Roles]
			WHERE
				[login_name] = @Login_Name
		";

		var command = new CommandDefinition(query, new { Login_Name = userName }, cancellationToken: cancellationToken);
		var result = await connection.ExecuteAsync(command);

		return result;
	}
}

public interface IRolesRepository
{
	Task<IEnumerable<EntityUsersRole>> GetUsersRoleAsync(FilterUsersRole filter, CancellationToken cancellationToken);

	Task<string?> GetUserRoleByLoginNameAsync(string loginName, CancellationToken cancellationToken);

	Task<IEnumerable<EntityUserReference>> GetUserReferenceAsync(string? keyword, CancellationToken cancellationToken);

	Task<int> PostUserRole(EntityUserRole postUserRole, CancellationToken cancellationToken);
	Task<int> PatchUserRole(EntityUserRole patchUserRole, CancellationToken cancellationToken);
	Task<int> DeleteUserRole(string userName, CancellationToken cancellationToken);
}
