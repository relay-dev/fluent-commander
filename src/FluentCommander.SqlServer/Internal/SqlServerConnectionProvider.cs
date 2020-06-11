using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerConnectionProvider : ISqlServerConnectionProvider
    {
        private readonly SqlConnectionStringBuilder _builder;

        public SqlServerConnectionProvider(SqlConnectionStringBuilder builder)
        {
            _builder = builder;
        }

        public SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_builder.ConnectionString);

            connection.Open();

            return connection;
        }

        public async Task<SqlConnection> GetConnectionAsync(CancellationToken cancellationToken)
        {
            var connection = new SqlConnection(_builder.ConnectionString);

            await connection.OpenAsync(cancellationToken);

            return connection;
        }
    }
}
