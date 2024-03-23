using Dapper;
using KM_Management.Commons.Connection;
using KM_Management.EndPoint.AssistantProfile.Models;
using KM_Management.EndPoint.Category.Models;
using Microsoft.Identity.Client;
using NetCore.AutoRegisterDi;

namespace KM_Management.EndPoint.AssistantProfile;

[RegisterAsScoped]
public class AssistantProfileRepository : IAssistantProfileRepository
{
    private readonly ISQLConnectionFactory _connection;
    private readonly string _uploadFolderPath;
    private readonly string _uploadPathUrl;


    public AssistantProfileRepository(ISQLConnectionFactory connnection, IWebHostEnvironment webHostEnvironment)
    {
        _connection = connnection;
        _uploadFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "upload");
        _uploadPathUrl = $"{""}/upload";
    }

    public async Task<EntityAssistantProfile?> GetAssistantProfileAsync(CancellationToken cancellationToken)
    {
        using var connection = _connection.CreateConnecton();

        var query = @"
             SELECT [APP_NAME], [APP_IMAGE] FROM [dbo].[View_Assistant_Profile]
        ";
        var command = new CommandDefinition(query, cancellationToken: cancellationToken);
        var result = await connection.QueryFirstOrDefaultAsync<EntityAssistantProfile?>(command);

        return result;
    }

    public async Task<int> PostAssistantProfileAsync(EntityPostAssistantProfile postAssistantProfile, string host, CancellationToken cancellationToken)
    {
        await using var connection = await _connection.CreateConnectionAsync();

        int validFile = 0;

        if (postAssistantProfile.Files == null || postAssistantProfile.Files.Count == 0)
        {
            validFile = 0;
        }
        else {
            validFile = postAssistantProfile.Files.Count();
        }

        if (validFile>0) {
        

        foreach (var file in postAssistantProfile.Files)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            var fileNameWithTimestamp = $"{fileNameWithoutExtension}_{DateTime.Now:dd-MM-yyyy-HH-mm-ss}";
            var fileNameFullFormat = $"{fileNameWithTimestamp}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(_uploadFolderPath, fileNameFullFormat);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }
        }

        if (validFile > 0)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(postAssistantProfile.Files[0].FileName);
            var fileNameWithTimestamp = $"{fileNameWithoutExtension}_{DateTime.Now:dd-MM-yyyy-HH-mm-ss}";
            var fileNameFullFormat = $"{fileNameWithTimestamp}{Path.GetExtension(postAssistantProfile.Files[0].FileName)}";
            postAssistantProfile.AppImage = $"{host}/Upload/{fileNameFullFormat}";
        }

        var storeProcedureName = "[dbo].[Update_Assistant_Profile]";
        var parameters = new { AppName = postAssistantProfile?.AppName, AppImage = postAssistantProfile?.AppImage };
        var command = new CommandDefinition(storeProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var result = await connection.ExecuteAsync(command);
        return result;
    }

    public bool VerifyAvailableName(string app_name)
    {
        using var connection = _connection.CreateConnecton();

        var query = @"
             SELECT [APP_NAME] FROM [dbo].[View_Assistant_Profile] WHERE [APP_NAME] = @app_name
        ";

        var command = new CommandDefinition(query, new { APP_NAME = app_name });
        var result = connection.QueryFirstOrDefault<string>(command);

        return result == null;
    }



}

public interface IAssistantProfileRepository
{
    // Async Fn
    Task<EntityAssistantProfile?> GetAssistantProfileAsync(CancellationToken cancellationToken);
    Task<int> PostAssistantProfileAsync(EntityPostAssistantProfile postAssistantProfile, string host, CancellationToken cancellationToken);
    // Sync Fn
    public bool VerifyAvailableName(string name);
}

