using FluentCommander.Scalar;
using FluentCommander.SqlServer.Internal;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer
{
    public class SqlServerDatabaseCommander : DatabaseCommanderBase
    {
        private readonly SqlConnectionStringBuilder _builder;

        public SqlServerDatabaseCommander(
            SqlConnectionStringBuilder builder,
            DatabaseCommandBuilder databaseCommandBuilder)
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
            return new SqlServerScalarCommand<TResult>(ConnectionProvider).Execute(new ScalarRequest(sql));
        }

        public async Task<TResult> ExecuteScalarAsync<TResult>(string sql, CancellationToken cancellationToken)
        {
            return await new SqlServerScalarCommand<TResult>(ConnectionProvider).ExecuteAsync(new ScalarRequest(sql), cancellationToken);
        }

        private string SqlSelectServerName => "SELECT @@SERVERNAME";
        private ISqlServerConnectionProvider ConnectionProvider => new SqlServerConnectionProvider(_builder);
    }
}
