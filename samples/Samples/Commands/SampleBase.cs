using Microsoft.Extensions.Configuration;
using Sampler.ConsoleApplication;
using Setup;
using System.Data;

namespace Samples.Commands
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
            return _databaseService.Print(dataTable);
        }
    }
}
