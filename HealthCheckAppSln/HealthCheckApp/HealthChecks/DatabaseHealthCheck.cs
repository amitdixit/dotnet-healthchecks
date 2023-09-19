using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;
using System.Data.Common;

namespace HealthCheckApp.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IConfiguration _configuration;

    public DatabaseHealthCheck(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _configuration = configuration;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using (var connection = _dbConnectionFactory.CreateSqlConnection(_configuration.GetConnectionString("StudentAppContext")))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT 1";
                    command.ExecuteScalar();
                    await Task.CompletedTask;
                    return HealthCheckResult.Healthy();
                }
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
        finally
        {
        }


    }
}

public interface IDbConnectionFactory
{
    IDbConnection CreateSqlConnection(string connectionName);
}

public class DbConnectionFactory : IDbConnectionFactory
{

    public IDbConnection CreateSqlConnection(string connectionString)
    {
        return CreateDbConnection(connectionString);
    }

    private IDbConnection CreateDbConnection(string connectionString)
    {
        DbConnection connection = null;

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            var factory = SqlClientFactory.Instance;

            connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
        }
        return connection;
    }

}
