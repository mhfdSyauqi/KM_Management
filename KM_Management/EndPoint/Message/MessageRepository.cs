using Dapper;
using KM_Management.Commons.Connection;
using KM_Management.EndPoint.Content.Models;
using KM_Management.EndPoint.Message.Models;
using NetCore.AutoRegisterDi;

namespace KM_Management.EndPoint.Message;

[RegisterAsScoped]
public class MessageRepository : IMessageRepository
{
    private readonly ISQLConnectionFactory _connection;

    public MessageRepository(ISQLConnectionFactory connection)
    {
        _connection = connection;
    }

    public async Task<int> PostMessageAsync(EntityPostMessage postMessage, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Add_New_Message]";
        var command = new CommandDefinition(storeProcedureName, postMessage, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.ExecuteAsync(command);
        return result;
    }

    public async Task<int> PatchMessageAsync(EntityPatchMessage patchMessage, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Update_Setup_Message]";
        var command = new CommandDefinition(storeProcedureName, patchMessage, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.ExecuteAsync(command);
        return result;
    }

    public async Task<int> DeleteMessageAsync(EntityDeleteMessage deleteMessage, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Soft_Delete_Setup_Message]";
        var command = new CommandDefinition(storeProcedureName, deleteMessage, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.ExecuteAsync(command);
        return result;
    }


    public async Task<IEnumerable<EntityActiveMessage?>> GetActiveMessageAsync(string? type, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_Active_Messages_By_Type]";
        var filterMessage = new
        {
            messageType = type
        };
        var command = new CommandDefinition(storeProcedureName, filterMessage, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityActiveMessage?>(command);
        return result;
    }

    public async Task<int> PatchSequenceMessageAsync(EntityPatchSequenceMessage patchSequenceMessage, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Update_Sequence_Message]";
        var command = new CommandDefinition(storeProcedureName, patchSequenceMessage, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.ExecuteAsync(command);
        return result;
    }

    

}

public interface IMessageRepository
{
    // Async Fn
    Task<IEnumerable<EntityActiveMessage?>> GetActiveMessageAsync(string? type, CancellationToken cancellationToken);
    Task<int> PatchMessageAsync(EntityPatchMessage patchMessage, CancellationToken cancellationToken);
    Task<int> PostMessageAsync(EntityPostMessage postMessage, CancellationToken cancellationToken);
    Task<int> PatchSequenceMessageAsync(EntityPatchSequenceMessage patchSequenceMessage, CancellationToken cancellationToken);
    Task<int> DeleteMessageAsync(EntityDeleteMessage deleteMessage, CancellationToken cancellationToken);

}
