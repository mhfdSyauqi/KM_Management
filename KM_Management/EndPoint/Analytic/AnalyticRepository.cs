using Dapper;
using KM_Management.Commons.Connection;
using KM_Management.EndPoint.Analytic.Models;
using NetCore.AutoRegisterDi;

namespace KM_Management.EndPoint.Analytic;

[RegisterAsScoped]
public class AnalyticRepository : IAnalyticRepository
{
    private readonly ISQLConnectionFactory _connection;

    public AnalyticRepository(ISQLConnectionFactory connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<EntityDetailHitAnalytic>> GetHitDetailAsync(FilterDetailHitAnalytic filter, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Analytic_Hit_Detail]";
        var command = new CommandDefinition(storeProcedureName, filter, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityDetailHitAnalytic>(command);

        return result;
    }

    public async Task<EntityGeneralHitAnalytic?> GetHitGeneralAsync(FilterGeneralHitAnalytic filter, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Analytic_Hit_General]";
        var command = new CommandDefinition(storeProcedureName, filter, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryFirstOrDefaultAsync<EntityGeneralHitAnalytic>(command);

        return result;
    }

    public async Task<IEnumerable<EntityDetailLeadAnalytic>> GetLeadDetailAsync(FilterLeadAnalytic filter, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Analytic_Leaderboard_Detail]";
        var command = new CommandDefinition(storeProcedureName, filter, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityDetailLeadAnalytic>(command);

        return result;
    }

    public async Task<IEnumerable<EntityExcelLeadAnalytic>> GetLeadExcelAsync(FilterLeadAnalytic filter, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Analytic_Leaderboard_Excel]";
        var command = new CommandDefinition(storeProcedureName, filter, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityExcelLeadAnalytic>(command);

        return result;
    }

    public async Task<IEnumerable<EntityGeneralLeadAnalytic>> GetLeadGeneralAsync(FilterLeadAnalytic filter, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Analytic_Leaderboard_General]";
        var command = new CommandDefinition(storeProcedureName, filter, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityGeneralLeadAnalytic>(command);

        return result;
    }

    public async Task<IEnumerable<EntityPopularAnalytic>> GetPopularDetailAsync(FilterDetailPopularAnalytic filter, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Analytic_Category_Detail]";
        var command = new CommandDefinition(storeProcedureName, filter, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityPopularAnalytic>(command);

        return result;
    }

    public async Task<EntityExcelPopularAnalytic> GetPopularExcelAsync(FilterGeneralPopularAnalytic filter, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Analytic_Category_Excel]";
        var result = await SqlMapper.QueryMultipleAsync(connection, storeProcedureName, filter, commandType: System.Data.CommandType.StoredProcedure);

        var excelData = new EntityExcelPopularAnalytic();
        excelData.General = result.Read<EntityExcelGenPopularAnalytic>().ToList();
        excelData.Detail = result.Read<EntityExcelDetPopularAnalytic>().ToList();
        excelData.Period = $"{filter.Start_Date:dd-MMM-yyyy} s/d {filter.End_Date:dd-MMM-yyyy}";

        return excelData;
    }

    public async Task<IEnumerable<EntityPopularAnalytic>> GetPopularGeneralAsync(FilterGeneralPopularAnalytic filter, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Analytic_Category_General]";
        var command = new CommandDefinition(storeProcedureName, filter, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityPopularAnalytic>(command);

        return result;
    }
}

public interface IAnalyticRepository
{
    Task<IEnumerable<EntityPopularAnalytic>> GetPopularGeneralAsync(FilterGeneralPopularAnalytic filter, CancellationToken cancellationToken);

    Task<IEnumerable<EntityPopularAnalytic>> GetPopularDetailAsync(FilterDetailPopularAnalytic filter, CancellationToken cancellationToken);

    Task<EntityExcelPopularAnalytic> GetPopularExcelAsync(FilterGeneralPopularAnalytic filter, CancellationToken cancellationToken);


    Task<EntityGeneralHitAnalytic?> GetHitGeneralAsync(FilterGeneralHitAnalytic filter, CancellationToken cancellationToken);

    Task<IEnumerable<EntityDetailHitAnalytic>> GetHitDetailAsync(FilterDetailHitAnalytic filter, CancellationToken cancellationToken);


    Task<IEnumerable<EntityGeneralLeadAnalytic>> GetLeadGeneralAsync(FilterLeadAnalytic filter, CancellationToken cancellationToken);
    Task<IEnumerable<EntityDetailLeadAnalytic>> GetLeadDetailAsync(FilterLeadAnalytic filter, CancellationToken cancellationToken);
    Task<IEnumerable<EntityExcelLeadAnalytic>> GetLeadExcelAsync(FilterLeadAnalytic filter, CancellationToken cancellationToken);
}
