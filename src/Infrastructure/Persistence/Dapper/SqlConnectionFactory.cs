using System.Data;
using Application.Common.Interfaces;
using Npgsql;

namespace Infrastructure.Persistence.Dapper;

public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}