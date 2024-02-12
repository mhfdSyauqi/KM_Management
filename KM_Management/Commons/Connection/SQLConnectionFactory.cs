using Microsoft.Data.SqlClient;

namespace KM_Management.Commons.Connection;

public class SQLConnectionFactory : ISQLConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SQLConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<SqlConnection> CreateConnectionAsync()
    {
        var connection = new SqlConnection(_configuration.GetConnectionString("DevDB"));
        return await Task.FromResult(connection);
    }

    public SqlConnection CreateConnecton()
    {
        var connection = new SqlConnection(_configuration.GetConnectionString("DevDB"));
        return connection;
    }
}
