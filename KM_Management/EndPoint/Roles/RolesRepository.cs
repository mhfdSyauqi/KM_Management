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


}

public interface IRolesRepository
{
	Task<IEnumerable<EntityUsersRole>> GetUsersRoleAsync(FilterUsersRole filter, CancellationToken cancellationToken);

	Task<string?> GetUserRoleByLoginNameAsync(string loginName, CancellationToken cancellationToken);
}
