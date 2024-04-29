using Dapper;
using KM_Management.Commons.Connection;
using KM_Management.EndPoint.Category.Models;
using KM_Management.EndPoint.Message.Models;
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

    public async Task<int> PatchCategoryListAsync(EntityPatchCategoryList patchCategoryList, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Update_Category_List]";
        var command = new CommandDefinition(storeProcedureName, patchCategoryList, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.ExecuteAsync(command);
        return result;
    }

    public async Task<int> PostCategoryListAsync(EntityPostCategoryList postCategory, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Add_New_Category]";
        var command = new CommandDefinition(storeProcedureName, postCategory, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.ExecuteAsync(command);
        return result;
    }

    public async Task<IEnumerable<EntityCategoryList?>> GetCategoryListAsync(Guid? uid_reference, int layer, bool? is_active, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Category_List]";
        var filterCategories = new
        {
            Uid_Reference = uid_reference,
            Layer = layer,  
            Is_Active = is_active,
        };
        var command = new CommandDefinition(storeProcedureName, filterCategories, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityCategoryList?>(command);
        return result;
    }




    public async Task<IEnumerable<EntityExportCategoryListFirstLayer?>> GetExportCategoriesFirstLayerAsync(bool? is_active, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureFirstLayer = "[dbo].[Get_Export_Category_List_First_Layer]";
        var filterExport = new
        {
            Is_Active = is_active,
        };
        var command = new CommandDefinition(storeProcedureFirstLayer, filterExport, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityExportCategoryListFirstLayer?>(command);
        return result;
    }

    public async Task<IEnumerable<EntityExportCategoryListSecondLayer?>> GetExportCategoriesSecondLayerAsync(bool? is_active, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();


        var storeProcedureSecondLayer = "[dbo].[Get_Export_Category_List_Second_Layer]";
        var filterExport = new
        {
            Is_Active = is_active,
        };
        var command = new CommandDefinition(storeProcedureSecondLayer, filterExport, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityExportCategoryListSecondLayer?>(command);
        return result;
    }

    public async Task<IEnumerable<EntityExportCategoryListThirdLayer?>> GetExportCategoriesThirdLayerAsync(bool? is_active, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();


        var storeProcedureThirdLayer = "[dbo].[Get_Export_Category_List_Third_Layer]";
        var filterExport = new
        {
            Is_Active = is_active,
        };
        var command = new CommandDefinition(storeProcedureThirdLayer, filterExport, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityExportCategoryListThirdLayer?>(command);
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


    public bool VerifyAvailableCategoryName(string name)
    {
        using var connection = _connection.CreateConnecton();

        var query = @"
             SELECT [name] FROM [dbo].[Bot_Category] WHERE [name] = @name
        ";

        var command = new CommandDefinition(query, new { Name = name});
        var result = connection.QueryFirstOrDefault<string>(command);

        return result == null;
    }

    public bool VerifyAvailableCategoryName(string Name, Guid categoryUid)
    {
        using var connection = _connection.CreateConnecton();

        var query = @"
             SELECT [name] FROM [dbo].[Bot_Category] WHERE [name] = @Name
             AND [uid] <> @Id
        ";

        var command = new CommandDefinition(query, new { Name = Name, Id= categoryUid });
        var result = connection.QueryFirstOrDefault<string>(command);

        return result == null;
    }
}

public interface ICategoryRepository
{
    // Async Fn
    Task<IEnumerable<EntityCategoriesReference>> GetCategoryReferenceAsync(CancellationToken cancellationToken);
    Task<IEnumerable<EntityCategoryList?>> GetCategoryListAsync(Guid? uid_reference, int layer, bool? is_active, CancellationToken cancellationToken);
    Task<int> PatchCategoryListAsync(EntityPatchCategoryList patchCategoryList, CancellationToken cancellationToken);
    Task<int> PostCategoryListAsync(EntityPostCategoryList postCategory, CancellationToken cancellationToken);
    
    Task<IEnumerable<EntityExportCategoryListFirstLayer?>> GetExportCategoriesFirstLayerAsync(bool? is_active, CancellationToken cancellationToken);

    Task<IEnumerable<EntityExportCategoryListSecondLayer?>> GetExportCategoriesSecondLayerAsync(bool? is_active, CancellationToken cancellationToken);

    Task<IEnumerable<EntityExportCategoryListThirdLayer?>> GetExportCategoriesThirdLayerAsync(bool? is_active, CancellationToken cancellationToken);

    // Sync Fn
    bool VerifyAvailableCategoryName(string name);
    bool VerifyAvailableCategoryName(string name, Guid categoryUid);
    bool VerifyValidCategory(Guid categoryId);
}