using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Sampler.ConsoleApplication;
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
            var dataTable = Server.ConnectionContext.ExecuteWithResults(sql).Tables[0];

            return dataTable;
        }

        protected TResult ExecuteScalar<TResult>(string sql)
        {
            // Intentionally avoiding the use of DatabaseCommander to execute commands against the database
            return (TResult)Server.ConnectionContext.ExecuteScalar(sql);
        }

        private Server Server
        {
            get
            {
                using var connection = new Microsoft.Data.SqlClient.SqlConnection(_config.GetConnectionString("DefaultConnection"));

                return new Server(new ServerConnection(connection));
            }
        }
    }
}
