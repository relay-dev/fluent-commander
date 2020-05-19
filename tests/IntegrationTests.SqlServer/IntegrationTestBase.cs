using System;
using System.Data;
using Core.Plugins.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Xunit.Abstractions;

namespace IntegrationTests.SqlServer
{
    public abstract class IntegrationTest<TSUT>
    {
        private readonly ServiceProviderFixture _serviceProviderFixture;
        private readonly IConfiguration _configuration;
        private readonly ITestOutputHelper _output;

        protected IntegrationTest(
            ServiceProviderFixture serviceProviderFixture,
            ITestOutputHelper output)
        {
            _serviceProviderFixture = serviceProviderFixture;
            _configuration = ResolveService<IConfiguration>();
            _output = output;
        }

        // Give each test their own instance
        protected TSUT SUT => ResolveService<TSUT>();
        protected const string TestUsername = "IntegrationTest";
        protected readonly DateTime Timestamp = DateTime.UtcNow;

        protected TService ResolveService<TService>()
        {
            return (TService)_serviceProviderFixture.ServiceProvider.GetRequiredService(typeof(TService));
        }

        protected virtual void WriteLine(string s)
        {
            _output.WriteLine(s);
        }

        protected virtual void WriteLine(string s, params object[] args)
        {
            _output.WriteLine(s, args);
        }

        protected virtual void WriteLine(DataTable d)
        {
            _output.WriteLine(d.ToPrintFriendly());
        }

        protected virtual void WriteLine(object o)
        {
            _output.WriteLine(o.ToString());
        }

        protected void ExecuteNonQuery(string sql)
        {
            // Intentionally avoiding the use of DatabaseCommander to execute commands against the database
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            Server server = new Server(new ServerConnection(connection));
            server.ConnectionContext.ExecuteNonQuery(sql);
        }

        protected TResult ExecuteScalar<TResult>(string sql)
        {
            // Intentionally avoiding the use of DatabaseCommander to execute commands against the database
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            Server server = new Server(new ServerConnection(connection));
            var dataTable = server.ConnectionContext.ExecuteWithResults(sql).Tables[0];

            return (TResult)dataTable.Rows[0][0];
        }

        protected DataTable ExecuteSql(string sql)
        {
            // Intentionally avoiding the use of DatabaseCommander to execute commands against the database
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            Server server = new Server(new ServerConnection(connection));
            var dataTable = server.ConnectionContext.ExecuteWithResults(sql).Tables[0];

            return dataTable;
        }
    }
}
