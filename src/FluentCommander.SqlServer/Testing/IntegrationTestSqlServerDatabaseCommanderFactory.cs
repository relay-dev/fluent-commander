using FluentCommander.Core;
using Microsoft.Extensions.Logging;

namespace FluentCommander.SqlServer.Testing
{
    /// <summary>
    /// Has all the same functionality as SqlServerDatabaseCommanderFactory. No more, no less
    /// </summary>
    /// <remarks>
    /// This class is used within the Integration Testing framework, which provides a protected SqlServerDatabaseCommanderFactory to subclasses
    /// Since it's constructor may change, the framework creates an instance of this type by way of the IoC container
    /// The issue is that SqlServerDatabaseCommanderFactory is registered with a Scoped lifetime, but the Integration Testing framework needs it with a Transient lifetime
    /// So we need an explicitly different type to essentially register an alternate SqlServerDatabaseCommanderFactory with a Transient lifetime
    /// </remarks>
    public class IntegrationTestSqlServerDatabaseCommanderFactory : SqlServerDatabaseCommanderFactory
    {
        public IntegrationTestSqlServerDatabaseCommanderFactory(
            IConnectionStringCollection connectionStringCollection,
            DatabaseCommandBuilder databaseCommandBuilder,
            ISqlServerCommandExecutor sqlServerCommandExecutor,
            ILoggerFactory loggerFactory)
            : base(connectionStringCollection, databaseCommandBuilder, sqlServerCommandExecutor, loggerFactory) { }
    }
}