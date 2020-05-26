using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Setup;
using System;
using System.Data;
using Xunit.Abstractions;

namespace IntegrationTests.SqlServer
{
    public abstract class IntegrationTest<TSUT>
    {
        private readonly ServiceProviderFixture _serviceProviderFixture;
        private readonly DatabaseService _databaseService;
        private readonly ITestOutputHelper _output;

        protected IntegrationTest(
            ServiceProviderFixture serviceProviderFixture,
            ITestOutputHelper output)
        {
            _serviceProviderFixture = serviceProviderFixture;
            _databaseService = new DatabaseService(ResolveService<IConfiguration>());
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
            _output.WriteLine(_databaseService.Print(d));
        }

        protected virtual void WriteLine(object o)
        {
            _output.WriteLine(o.ToString());
        }

        protected DataTable ExecuteSql(string sql)
        {
            return _databaseService.ExecuteSql(sql);
        }

        protected TResult ExecuteScalar<TResult>(string sql)
        {
            return _databaseService.ExecuteScalar<TResult>(sql);
        }
    }
}
