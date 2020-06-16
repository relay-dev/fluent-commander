using Microsoft.Extensions.Configuration;
using Sampler.ConsoleApplication;
using System.Data;
using FluentCommander.Samples.Setup;

namespace FluentCommander.Samples.Commands
{
    public abstract class SampleBase : SampleAsync
    {
        private readonly DatabaseService _databaseService;

        protected SampleBase(IConfiguration config)
        {
            _databaseService = new DatabaseService(config);
        }

        protected DataTable ExecuteSql(string sql)
        {
            return _databaseService.ExecuteSql(sql);
        }

        protected TResult ExecuteScalar<TResult>(string sql)
        {
            return _databaseService.ExecuteScalar<TResult>(sql);
        }

        protected string Print(DataTable dataTable)
        {
            return DatabaseService.Print(dataTable);
        }
    }
}
