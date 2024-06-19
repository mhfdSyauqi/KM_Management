using Dapper;
using KM_Management.Commons.Connection;
using KM_Management.EndPoint.General.Models;
using NetCore.AutoRegisterDi;

namespace KM_Management.EndPoint.General;

[RegisterAsScoped]
public class GeneralRepository : IGeneralRepository
{
    private readonly ISQLConnectionFactory _connection;

    public GeneralRepository(ISQLConnectionFactory connection)
    {
        _connection = connection;
    }

    public async Task<EntityConfigGeneral?> GetConfigurationGeneralAsync(CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();
        var query = @"
            SELECT 
	            [LAYER_ONE_LIMIT]
                ,[LAYER_TWO_LIMIT]
                ,[LAYER_THREE_LIMIT]
                ,[SUGGESTION_LIMIT]
                ,[DELAY_TYPING]
                ,[IDLE_DURATION]
                ,[KEYWORDS]
                ,[MAIL_HISTORY_FROM]
                ,[MAIL_HISTORY_SUBJECT]
                ,[MAIL_HELPDESK_FROM]
                ,[MAIL_HELPDESK_TO]
                ,[MAIL_HELPDESK_SUBJECT]
                ,[MAIL_HELPDESK_CONTENT]
                ,[MAIL_HELPDESK_CONTENT_HTML]
                ,[MAIL_CONFIG_USERNAME]
                ,[MAIL_CONFIG_SERVER]
                ,[MAIL_CONFIG_PORT]
              FROM [dbo].[View_Configuration_General]
        ";
        var command = new CommandDefinition(query, cancellationToken: cancellationToken);
        var result = connection.QueryFirstOrDefault<EntityConfigGeneral?>(command);

        return result;
    }

    public async Task<string> GetDefaultMailbotPassword(CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();
        var query = @"
            SELECT [MAIL_CONFIG_PASSWORD] FROM [dbo].[View_Configuration_General]
        ";
        var command = new CommandDefinition(query, cancellationToken: cancellationToken);
        var result = connection.QueryFirst<string>(command);

        return result;
    }

    public async Task<int> PatchConfigurationGeneralAsync(FilterConfigGeneral patchConfig, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Patch_General_Config]";
        var command = new CommandDefinition(storeProcedureName, patchConfig, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.ExecuteAsync(command);
        return result;
    }
}

public interface IGeneralRepository
{
    Task<EntityConfigGeneral?> GetConfigurationGeneralAsync(CancellationToken cancellationToken);

    Task<int> PatchConfigurationGeneralAsync(FilterConfigGeneral filter, CancellationToken cancellationToken);

    Task<string> GetDefaultMailbotPassword(CancellationToken cancellationToken);
}
