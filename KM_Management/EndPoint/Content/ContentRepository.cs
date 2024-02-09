using Dapper;
using KM_Management.Commons.Connection;
using KM_Management.EndPoint.Content.Moldels;

namespace KM_Management.EndPoint.Content;

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
}

public interface IContentRepository
{
    Task<IEnumerable<EntityContent>> GetContentAsync(FilterContent filterContent, CancellationToken cancellationToken);
}
