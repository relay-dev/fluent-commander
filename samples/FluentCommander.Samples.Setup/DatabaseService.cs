using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace FluentCommander.Samples.Setup
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

        public static string Print(DataTable dataTable)
        {
            return DataTableToString(dataTable);
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
            string resourceFilename = $"FluentCommander.Samples.Setup.Resources.{filename}";

            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFilename);

            if (stream == null)
            {
                throw new FileNotFoundException($"Could not find a file with name '{filename}'");
            }

            using StreamReader streamReader = new StreamReader(stream);

            var resourceFileContent = streamReader.ReadToEnd();

            return resourceFileContent;
        }

        private static string DataTableToString(DataTable dataTable)
        {
            var printFriendly = new StringBuilder();
            var underline = new StringBuilder();

            if (dataTable == null || dataTable.Rows.Count == 0)
                return "<No rows found>\n";

            Dictionary<int, int> maxStringLengthPerColumn = GetMaxStringLengths(dataTable);

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                printFriendly.Append(GetPrintFriendlyString(dataTable.Columns[i].ColumnName, maxStringLengthPerColumn[i]));
                underline.Append(GetPrintFriendlyString(String.Empty, maxStringLengthPerColumn[i] - 1).Replace(" ", "-") + " ");
            }

            printFriendly.Append("\n" + underline + "\n");

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                printFriendly.Append(GetPrintFriendlyRow(dataTable.Rows[i], maxStringLengthPerColumn));
            }

            return printFriendly.ToString();
        }

        private static Dictionary<int, int> GetMaxStringLengths(DataTable dataTable)
        {
            var maxStringLengthPerColumn = new Dictionary<int, int>();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                int maxLength = dataTable.Columns[i].ColumnName.Length;

                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    if (dataTable.Rows[j][i] != DBNull.Value && dataTable.Rows[j][i].ToString().Length > maxLength)
                        maxLength = dataTable.Rows[j][i].ToString().Length;
                }

                maxStringLengthPerColumn.Add(i, maxLength);
            }

            return maxStringLengthPerColumn;
        }

        private static string GetPrintFriendlyRow(DataRow row, Dictionary<int, int> maxStringLengthPerColumn)
        {
            var printFriendly = new StringBuilder();

            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                string value = row[i] == DBNull.Value
                    ? "{null}"
                    : row[i].ToString();

                printFriendly.Append(GetPrintFriendlyString(value, maxStringLengthPerColumn[i]));
            }

            return printFriendly + "\n";
        }

        private static string GetPrintFriendlyString(string value, int lengthNeeded)
        {
            int spacesNeeded = lengthNeeded - value.Length;

            return value + string.Empty.PadRight(spacesNeeded + 2, ' ');
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
