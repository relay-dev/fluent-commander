using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ConsoleApplication.SqlServer
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
        }

        public void TeardownDatabase()
        {
            var scriptsToRun = new List<string>
            {
                "teardown-sample-database.sql"
            };

            RunScripts(scriptsToRun);
        }

        private void Validate()
        {
            string connectionString = _config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("The default connection string has not been set. Please find the ConnectionStrings section of the appsettings.json file in the ClientApplication project");
            }
        }

        private void RunScripts(List<string> scriptsToRun)
        {
            foreach (string scriptToRun in scriptsToRun)
            {
                string sql = GetResourceFile(scriptToRun);

                ExecuteNonQuery(sql);
            }
        }

        private void ExecuteNonQuery(string sql)
        {
            // Intentionally avoiding the use of DatabaseCommander to execute commands against the database
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(_config.GetConnectionString("DatabaseServerConnection"));

            Server server = new Server(new ServerConnection(connection));
            server.ConnectionContext.ExecuteNonQuery(sql);
        }

        private string GetResourceFile(string filename)
        {
            string resourceFilename = $"ConsoleApplication.SqlServer.Resources.{filename}";

            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFilename);

            if (stream == null)
            {
                throw new FileNotFoundException($"Could not find a file with name '{filename}'");
            }

            using StreamReader streamReader = new StreamReader(stream);

            var resourceFileContent = streamReader.ReadToEnd();

            return resourceFileContent;
        }
    }
}
