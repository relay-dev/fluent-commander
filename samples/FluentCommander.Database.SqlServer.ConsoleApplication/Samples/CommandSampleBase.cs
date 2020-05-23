using ConsoleApplication.SqlServer.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data;

namespace ConsoleApplication.SqlServer.Samples
{
    public abstract class CommandSampleBase : SampleAsync
    {
        private readonly IConfiguration _config;

        protected CommandSampleBase(IConfiguration config)
        {
            _config = config;
        }

        protected DataTable ExecuteSql(string sql)
        {
            // Intentionally avoiding the use of DatabaseCommander to execute commands against the database
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(_config.GetConnectionString("DefaultConnection"));
            
            Server server = new Server(new ServerConnection(connection));
            var dataTable = server.ConnectionContext.ExecuteWithResults(sql).Tables[0];

            return dataTable;
        }

        protected TResult ExecuteScalar<TResult>(string sql)
        {
            // Intentionally avoiding the use of DatabaseCommander to execute commands against the database
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(_config.GetConnectionString("DefaultConnection"));

            Server server = new Server(new ServerConnection(connection));
            return (TResult)server.ConnectionContext.ExecuteScalar(sql);
        }
    }
}
