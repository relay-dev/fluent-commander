﻿using FluentCommander.Core.Options;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FluentCommander.UnitTests")]
namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerConnectionProvider : ISqlServerConnectionProvider
    {
        private readonly SqlConnectionStringBuilder _builder;

        public SqlServerConnectionProvider(SqlConnectionStringBuilder builder)
        {
            _builder = builder;
        }

        public SqlConnection GetConnection(CommandOptions options)
        {
            var connection = new SqlConnection(_builder.ConnectionString);

            if (IsOpenConnectionWithoutRetry(options))
            {
                connection.Open(SqlConnectionOverrides.OpenWithoutRetry);
            }
            else
            {
                connection.Open();
            }

            return connection;
        }

        public async Task<SqlConnection> GetConnectionAsync(CancellationToken cancellationToken)
        {
            var connection = new SqlConnection(_builder.ConnectionString);

            await connection.OpenAsync(cancellationToken);

            return connection;
        }

        private bool IsOpenConnectionWithoutRetry(CommandOptions options)
        {
            if (options == null)
            {
                return false;
            }

            return options.OpenConnectionWithoutRetry.HasValue && options.OpenConnectionWithoutRetry.Value;
        }
    }
}
