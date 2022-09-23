using Oracle.ManagedDataAccess.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Oracle
{
    public class OracleDatabaseCommander : DatabaseCommanderBase
    {
        private readonly OracleConnectionStringBuilder _builder;

        public OracleDatabaseCommander(OracleConnectionStringBuilder builder, DatabaseCommandBuilder databaseCommandBuilder)
            : base(databaseCommandBuilder)
        {
            _builder = builder;
        }

        public override string GetServerName()
        {
            return ExecuteScalar<string>(SqlSelectServerName);
        }

        public override async Task<string> GetServerNameAsync(CancellationToken cancellationToken)
        {
            return await ExecuteScalarAsync<string>(SqlSelectServerName, cancellationToken);
        }

        private TResult ExecuteScalar<TResult>(string sql)
        {
            using var connection = new OracleConnection(_builder.ConnectionString);
            using var command = new OracleCommand(sql, connection);

            connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        private async Task<TResult> ExecuteScalarAsync<TResult>(string sql, CancellationToken cancellationToken)
        {
            await using var connection = new OracleConnection(_builder.ConnectionString);
            await using var command = new OracleCommand(sql, connection);

            connection.Open();
            var result = await command.ExecuteScalarAsync(cancellationToken);
            connection.Close();

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        private string SqlSelectServerName => "SELECT host_name FROM v$instance";
    }
}
