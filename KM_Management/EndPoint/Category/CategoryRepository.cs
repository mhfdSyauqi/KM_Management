using Dapper;
using KM_Management.Commons.Connection;
using NetCore.AutoRegisterDi;

namespace KM_Management.EndPoint.Category;

[RegisterAsScoped]
public class CategoryRepository : ICategoryRepository
{
    private readonly ISQLConnectionFactory _connection;

    public CategoryRepository(ISQLConnectionFactory connnection)
    {
        _connection = connnection;
    }

    public bool VerifyValidCategory(Guid categoryId)
    {
        using var connection = _connection.CreateConnecton();

        var query = @"
             SELECT [name] FROM [dbo].[Bot_Category] WHERE [uid] = @Category_Id
        ";
        var command = new CommandDefinition(query, new { Category_Id = categoryId });
        var result = connection.QueryFirstOrDefault<string>(command);

        return result != null;
    }
}

public interface ICategoryRepository
{
    // Async Fn

    // Sync Fn
    bool VerifyValidCategory(Guid categoryId);
}