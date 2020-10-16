using Consolater;
using FluentCommander.Samples.Setup;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace FluentCommander.Samples.Commands
{
    public abstract class Sample : ConsoleAppAsync
    {
        private readonly DatabaseService _databaseService;

        protected Sample(IConfiguration config)
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
