using Dapper;
using KM_Management.Commons.Connection;
using KM_Management.EndPoint.Content.Models;
using NetCore.AutoRegisterDi;

namespace KM_Management.EndPoint.Content;

[RegisterAsScoped]
public class ContentRepository : IContentRepository
{
	private readonly ISQLConnectionFactory _connection;

	public ContentRepository(ISQLConnectionFactory connection)
	{
		_connection = connection;
	}

	public async Task<IEnumerable<EntityContent>> GetContentAsync(FilterContent filterContent, CancellationToken cancellationToken)
	{
		await using var connection = await _connection.CreateConnectionAsync();

		var storeProcedureName = "[dbo].[Get_Content_General]";
		var command = new CommandDefinition(storeProcedureName, filterContent, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
		var result = await connection.QueryAsync<EntityContent>(command);
		return result;
	}

	public async Task<EntityDetailContent?> GetContentByIdAsync(Guid contentId, CancellationToken cancellationToken)
	{
		await using var connection = await _connection.CreateConnectionAsync();

		var storeProcedureName = "[dbo].[Get_Content_By_Identity]";
		var filterContent = new
		{
			Content_Id = contentId
		};
		var command = new CommandDefinition(storeProcedureName, filterContent, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
		var result = await connection.QueryFirstOrDefaultAsync<EntityDetailContent?>(command);
		return result;
	}

	public async Task<int> PatchContentAsync(EntityPatchContent patchContent, CancellationToken cancellationToken)
	{
		await using var connection = await _connection.CreateConnectionAsync();

		var storeProcedureName = "[dbo].[Patch_Bot_Content]";
		var command = new CommandDefinition(storeProcedureName, patchContent, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
		var result = await connection.ExecuteAsync(command);
		return result;
	}

	public async Task<int> PostContentAsync(EntityPostContent postContent, CancellationToken cancellationToken)
	{
		await using var connection = await _connection.CreateConnectionAsync();

		var storeProcedureName = "[dbo].[Add_Bot_Content]";
		var command = new CommandDefinition(storeProcedureName, postContent, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
		var result = await connection.ExecuteAsync(command);
		return result;
	}

	public bool VerifyAvailableTitle(string title)
	{
		using var connection = _connection.CreateConnecton();

		var query = @"
             SELECT [title] FROM [dbo].[Bot_Content] WHERE [title] = @Title
        ";

		var command = new CommandDefinition(query, new { Title = title });
		var result = connection.QueryFirstOrDefault<string>(command);

		return result == null;
	}

	public bool VerifyAvailableTitle(string title, Guid contentId)
	{
		using var connection = _connection.CreateConnecton();

		var query = @"
             SELECT [title] FROM [dbo].[Bot_Content] WHERE [title] = @Title AND [uid] <> @Content_Id
        ";

		var command = new CommandDefinition(query, new { Title = title, Content_Id = contentId });
		var result = connection.QueryFirstOrDefault<string>(command);

		return result == null;
	}
}

public interface IContentRepository
{
	// Async Fn
	Task<IEnumerable<EntityContent>> GetContentAsync(FilterContent filterContent, CancellationToken cancellationToken);

	Task<int> PostContentAsync(EntityPostContent postContent, CancellationToken cancellationToken);

	Task<int> PatchContentAsync(EntityPatchContent patchContent, CancellationToken cancellationToken);

	Task<EntityDetailContent?> GetContentByIdAsync(Guid contentId, CancellationToken cancellationToken);

	// Sync Fn
	bool VerifyAvailableTitle(string title);
	bool VerifyAvailableTitle(string title, Guid contentId);
}
