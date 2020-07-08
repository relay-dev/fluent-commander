using FluentCommander.Core.Options;
using System.Data;

namespace FluentCommander.SqlServer
{
    public class SqlServerDatabaseCommanderTransactionFactory : IDatabaseCommanderTransactionFactory
    {
        private readonly ISqlServerConnectionProvider _connectionProvider;

        public SqlServerDatabaseCommanderTransactionFactory(ISqlServerConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public IDbTransaction Create(string transactionName = null, IsolationLevel? iso = null)
        {
            iso ??= IsolationLevel.Unspecified;

            return _connectionProvider.GetConnection(new CommandOptions()).BeginTransaction(iso.Value, transactionName);
        }
    }
}
