using Microsoft.Data.SqlClient;

namespace KM_Management.Commons.Connection;

public interface ISQLConnectionFactory
{
    Task<SqlConnection> CreateConnectionAsync();
}
