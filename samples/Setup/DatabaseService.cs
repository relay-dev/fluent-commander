using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace Setup
{
    public class DatabaseService
    {
        private readonly IConfiguration _config;

        public DatabaseService(IConfiguration config)
        {
            _config = config;

            Validate();
        }

        public void SetupDatabase()
        {
            var scriptsToRun = new List<string>
            {
                "setup-sample-database-batch-1.sql",
                "setup-sample-database-batch-2.sql",
                "setup-sample-database-batch-3.sql"
            };

            RunScripts(scriptsToRun);
            InsertTestData();
        }

        public void TeardownDatabase()
        {
            var scriptsToRun = new List<string>
            {
                "teardown-sample-database.sql"
            };

            RunScripts(scriptsToRun);
        }

        public bool IsInitialized()
        {
            string sql = "SELECT COUNT(1) FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = 'DatabaseCommander' OR name = 'DatabaseCommander')";

            return ExecuteScalar<int>(sql, "DatabaseServerConnection") == 1;
        }

        // Intentionally avoiding the use of DatabaseCommander to execute commands against the database
        public DataTable ExecuteSql(string sql, string connectionStringName = "DefaultConnection")
        {
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(_config.GetConnectionString(connectionStringName));

            Server server = new Server(new ServerConnection(connection));

            return server.ConnectionContext.ExecuteWithResults(sql).Tables[0];
        }

        public TResult ExecuteScalar<TResult>(string sql, string connectionStringName = "DefaultConnection")
        {
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(_config.GetConnectionString(connectionStringName));

            Server server = new Server(new ServerConnection(connection));

            return (TResult)server.ConnectionContext.ExecuteScalar(sql);
        }

        public void ExecuteNonQuery(string sql, string connectionStringName = "DefaultConnection")
        {
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(_config.GetConnectionString(connectionStringName));

            Server server = new Server(new ServerConnection(connection));

            server.ConnectionContext.ExecuteNonQuery(sql);
        }

        private void Validate()
        {
            string connectionString = _config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("The default connection string has not been set. Please find the ConnectionStrings section of the appsettings.json file in the project of the entry point");
            }
        }

        private void RunScripts(List<string> scriptsToRun)
        {
            foreach (string scriptToRun in scriptsToRun)
            {
                string sql = GetResourceFile(scriptToRun);

                ExecuteNonQuery(sql, "DatabaseServerConnection");
            }
        }

        private void InsertTestData(int rowCount = 100)
        {
            for (int i = 0; i < rowCount; i++)
            {
                string sql = string.Format(InsertSql, DateTime.UtcNow, Guid.NewGuid(), $"Row {i + 4}");

                ExecuteNonQuery(sql);
            }
        }

        private string GetResourceFile(string filename)
        {
            string resourceFilename = $"Setup.Resources.{filename}";

            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFilename);

            if (stream == null)
            {
                throw new FileNotFoundException($"Could not find a file with name '{filename}'");
            }

            using StreamReader streamReader = new StreamReader(stream);

            var resourceFileContent = streamReader.ReadToEnd();

            return resourceFileContent;
        }

        private string InsertSql =>
@"INSERT INTO [dbo].[SampleTable]
           ([SampleInt]
           ,[SampleSmallInt]
           ,[SampleTinyInt]
           ,[SampleBit]
           ,[SampleDecimal]
           ,[SampleFloat]
           ,[SampleDateTime]
           ,[SampleUniqueIdentifier]
           ,[SampleVarChar]
           ,[CreatedBy]
           ,[CreatedDate])
     VALUES
           (1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,'{0}'
           ,'{1}'
           ,'{2}'
           ,'DatabaseService'
           ,GETUTCDATE())";
    }
}
