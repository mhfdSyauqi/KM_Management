using Dapper;
using KM_Management.Commons.Connection;
using KM_Management.EndPoint.Message.Models;
using KM_Management.EndPoint.RateAndFeedback.Models;
using NetCore.AutoRegisterDi;

namespace KM_Management.EndPoint.RateAndFeedback;

[RegisterAsScoped]
public class RateAndFeedbackRepository : IRateAndFeedbackRepository
{
    private readonly ISQLConnectionFactory _connection;

    public RateAndFeedbackRepository(ISQLConnectionFactory connection)
    {
        _connection = connection;
    }


    public async Task<IEnumerable<EntityRateAndFeedback?>> GetRateAndFeedbackAsync(FilterRateAndFeedback filter, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();
        var rating = string.Join(",", filter.Rating);
        var storeProcedureName = "[dbo].[Get_User_Feedback_By_Filter]";
        var filterFeedback = new
        {
            Filter_Date = filter.Filter_Date,
            Start_Date = filter.Start_Date,
            End_Date = filter.End_Date,
            Rating = rating,
            Current_Page = filter.Current_Page,
        };
        var command = new CommandDefinition(storeProcedureName, filterFeedback, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryAsync<EntityRateAndFeedback?>(command);
        return result;
    }

    public async Task<EntityRateAndFeedback?> GetRateAndFeedbackSummaryAsync(FilterRateAndFeedbackSummary filter, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        var storeProcedureName = "[dbo].[Get_User_Feedback_Summary_By_Filter]";
        var filterFeedback = new
        {
            Filter_Date = filter.Filter_Date,
            Start_Date = filter.Start_Date,
            End_Date = filter.End_Date,
        };
        var command = new CommandDefinition(storeProcedureName, filterFeedback, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.QueryFirstOrDefaultAsync<EntityRateAndFeedback?>(command);
        return result;
    }

}

public interface IRateAndFeedbackRepository
{
    // Async Fn
    //Task<IEnumerable<EntityRateAndFeedback>> GetRateAndFeedbackAsync(FilterRateAndFeedback filterRateAndFeedback, CancellationToken cancellationToken);
    Task<IEnumerable<EntityRateAndFeedback?>> GetRateAndFeedbackAsync(FilterRateAndFeedback filter, CancellationToken cancellationToken);
    Task<EntityRateAndFeedback?> GetRateAndFeedbackSummaryAsync(FilterRateAndFeedbackSummary filter, CancellationToken cancellationToken);
    // Sync Fn
}
