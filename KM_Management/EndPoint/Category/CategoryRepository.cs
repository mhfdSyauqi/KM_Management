using Dapper;
using KM_Management.Commons.Connection;
using KM_Management.EndPoint.Category.Models;
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

    public async Task<IEnumerable<EntityCategoriesReference>> GetCategoryReferenceAsync(CancellationToken cancellationToken)
    {
        using var connection = _connection.CreateConnecton();

        var query = @"
             SELECT [uid], [name] FROM [dbo].[View_Available_Categories]
        ";
        var command = new CommandDefinition(query, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityCategoriesReference>(command);

        return result;
    }

    public async Task<IEnumerable<EntityCategoriesList?>> GetCategoriesListAsync(Guid? uid_reference, int layer, bool? is_active, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Categories_List]";
        var filterCategories = new
        {
            Uid_Reference = uid_reference,
            Layer = layer,  
            Is_Active = is_active,
        };
        var command = new CommandDefinition(storeProcedureName, filterCategories, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityCategoriesList?>(command);
        return result;
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
    Task<IEnumerable<EntityCategoriesReference>> GetCategoryReferenceAsync(CancellationToken cancellationToken);
    Task<IEnumerable<EntityCategoriesList?>> GetCategoriesListAsync(Guid? uidReference, int layer, bool? is_Active, CancellationToken cancellationToken);
    // Sync Fn
    bool VerifyValidCategory(Guid categoryId);
}