using Microsoft.Extensions.Configuration;
using Setup;
using System;
using System.Data;
using System.Text.Json;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public abstract class IntegrationTest<TSUT>
    {
        private readonly ITestOutputHelper _output;
        private DatabaseService _databaseService;

        protected IntegrationTest(ITestOutputHelper output)
        {
            _output = output;
        }

        // Give each test their own instance
        protected TSUT SUT => ResolveService<TSUT>();
        protected const string TestUsername = "IntegrationTest";
        protected readonly DateTime Timestamp = DateTime.UtcNow;
        protected abstract TService ResolveService<TService>();

        public IntegrationTest<TSUT> Init()
        {
            _databaseService = new DatabaseService(ResolveService<IConfiguration>());

            return this;
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
            _output.WriteLine(JsonSerializer.Serialize(o));
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
