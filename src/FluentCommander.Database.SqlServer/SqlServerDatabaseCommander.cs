using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.SqlServer
{
    public class SqlServerDatabaseCommander : IDatabaseCommander
    {
        private readonly SqlConnectionStringBuilder _builder;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;

        public SqlServerDatabaseCommander(SqlConnectionStringBuilder builder, DatabaseCommandBuilder databaseCommandBuilder)
        {
            _builder = builder;
            _databaseCommandBuilder = databaseCommandBuilder;
        }

        public DatabaseCommandBuilder BuildCommand()
        {
            return _databaseCommandBuilder;
        }

        public void BulkCopy(string tableName, DataTable dataTable, ColumnMapping columnMapping)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlBulkCopy(connection)
            {
                DestinationTableName = tableName,
                BulkCopyTimeout = TimeoutInSeconds
            };

            if (columnMapping != null)
            {
                foreach (ColumnMap columnMap in columnMapping.ColumnMaps)
                {
                    command.ColumnMappings.Add(columnMap.Source, columnMap.Destination);
                }
            }

            try
            {
                connection.Open();
                command.WriteToServer(dataTable);
                connection.Close();
            }
            catch (Exception e)
            {
                HandleBulkCopyException(e, command);

                throw;
            }
        }

        public async Task BulkCopyAsync(string tableName, DataTable dataTable, ColumnMapping columnMapping, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);

            using var command = new SqlBulkCopy(connection)
            {
                DestinationTableName = tableName,
                BulkCopyTimeout = TimeoutInSeconds
            };

            if (columnMapping != null)
            {
                foreach (ColumnMap columnMap in columnMapping.ColumnMaps)
                {
                    command.ColumnMappings.Add(columnMap.Source, columnMap.Destination);
                }
            }

            try
            {
                connection.Open();
                await command.WriteToServerAsync(dataTable, cancellationToken);
                connection.Close();
            }
            catch (Exception e)
            {
                HandleBulkCopyException(e, command);

                throw;
            }
        }

        public int ExecuteNonQuery(string sql, List<DatabaseCommandParameter> databaseParameters = null)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(sql, connection)
            {
                CommandTimeout = TimeoutInSeconds
            };

            if (databaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(databaseParameters));
            }

            connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            connection.Close();

            return numberOfRowsAffected;
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken, List<DatabaseCommandParameter> databaseParameters = null)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(sql, connection)
            {
                CommandTimeout = TimeoutInSeconds
            };

            if (databaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(databaseParameters));
            }

            connection.Open();
            var numberOfRowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            connection.Close();

            return numberOfRowsAffected;
        }

        public TResult ExecuteScalar<TResult>(string sql, List<DatabaseCommandParameter> databaseParameters = null)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(sql, connection)
            {
                CommandTimeout = TimeoutInSeconds
            };

            if (databaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(databaseParameters));
            }

            connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        public async Task<TResult> ExecuteScalarAsync<TResult>(string sql, CancellationToken cancellationToken, List<DatabaseCommandParameter> databaseParameters = null)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(sql, connection)
            {
                CommandTimeout = TimeoutInSeconds
            };

            if (databaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(databaseParameters));
            }

            connection.Open();
            var result = await command.ExecuteScalarAsync(cancellationToken);
            connection.Close();

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        public DataTable ExecuteSql(string sql, List<DatabaseCommandParameter> databaseParameters = null)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(sql, connection)
            {
                CommandTimeout = TimeoutInSeconds
            };

            if (databaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(databaseParameters));
            }

            var dataTable = new DataTable();

            connection.Open();
            new SqlDataAdapter(command).Fill(dataTable);
            connection.Close();

            return dataTable;
        }

        public async Task<DataTable> ExecuteSqlAsync(string sql, CancellationToken cancellationToken, List<DatabaseCommandParameter> databaseParameters = null)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(sql, connection)
            {
                CommandTimeout = TimeoutInSeconds
            };

            if (databaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(databaseParameters));
            }

            var dataTable = new DataTable();

            connection.Open();
            var reader = await command.ExecuteReaderAsync(cancellationToken);
            dataTable.Load(reader);
            connection.Close();

            return dataTable;
        }

        public StoredProcedureResult ExecuteStoredProcedure(string storedProcedureName, List<DatabaseCommandParameter> databaseParameters = null)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = TimeoutInSeconds
            };

            SqlParameter[] sqlParameters = ToSqlParameters(databaseParameters);

            if (databaseParameters != null)
            {
                command.Parameters.AddRange(sqlParameters);
            }

            var dataTable = new DataTable();

            connection.Open();
            new SqlDataAdapter(command).Fill(dataTable);
            connection.Close();

            if (databaseParameters != null)
            {
                foreach (DatabaseCommandParameter databaseCommandParameter in databaseParameters.Where(dp => dp.Direction == ParameterDirection.Output || dp.Direction == ParameterDirection.InputOutput || dp.Direction == ParameterDirection.ReturnValue))
                {
                    databaseCommandParameter.Value = sqlParameters.Single(sp => sp.ParameterName == databaseCommandParameter.Name).Value;
                }
            }

            return new StoredProcedureResult(databaseParameters, dataTable);
        }

        public async Task<StoredProcedureResult> ExecuteStoredProcedureAsync(string storedProcedureName, CancellationToken cancellationToken, List<DatabaseCommandParameter> databaseParameters = null)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = TimeoutInSeconds
            };

            SqlParameter[] sqlParameters = ToSqlParameters(databaseParameters);

            if (databaseParameters != null)
            {
                command.Parameters.AddRange(sqlParameters);
            }

            var dataTable = new DataTable();

            connection.Open();
            var reader = await command.ExecuteReaderAsync(cancellationToken);
            dataTable.Load(reader);
            connection.Close();

            if (databaseParameters != null)
            {
                foreach (DatabaseCommandParameter databaseCommandParameter in databaseParameters.Where(dp => dp.Direction == ParameterDirection.Output || dp.Direction == ParameterDirection.InputOutput || dp.Direction == ParameterDirection.ReturnValue))
                {
                    databaseCommandParameter.Value = sqlParameters.Single(sp => sp.ParameterName == databaseCommandParameter.Name).Value;
                }
            }

            return new StoredProcedureResult(databaseParameters, dataTable);
        }

        public string GetServerName()
        {
            string sql = GetServerNameSql();

            return ExecuteScalar<string>(sql);
        }

        public async Task<string> GetServerNameAsync(CancellationToken cancellationToken)
        {
            string sql = GetServerNameSql();

            return await ExecuteScalarAsync<string>(sql, cancellationToken);
        }

        public PaginationResult Paginate(PaginationRequest paginationRequest)
        {
            string sql = GetPaginationSql(paginationRequest);
            string sqlCount = GetPaginationCountSql(paginationRequest);

            DataTable dataTable = ExecuteSql(sql);

            int totalCount = ExecuteScalar<int>(sqlCount);

            return new PaginationResult(dataTable, totalCount);
        }

        public async Task<PaginationResult> PaginateAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            string sql = GetPaginationSql(paginationRequest);
            string sqlCount = GetPaginationCountSql(paginationRequest);

            Task<DataTable> getDataTask = ExecuteSqlAsync(sql, cancellationToken);

            Task<int> getCountTask = ExecuteScalarAsync<int>(sqlCount, cancellationToken);

            await Task.WhenAll(getDataTask, getCountTask);

            DataTable dataTable = await getDataTask;

            int totalCount = await getCountTask;

            return new PaginationResult(dataTable, totalCount);
        }

        public int TimeoutInSeconds { get; set; }

        private SqlParameter[] ToSqlParameters(List<DatabaseCommandParameter> databaseCommandParameters)
        {
            return databaseCommandParameters?.Select(ToSqlParameter).ToArray();
        }

        private SqlParameter ToSqlParameter(DatabaseCommandParameter databaseCommandParameter)
        {
            var sqlParameter = new SqlParameter
            {
                ParameterName = databaseCommandParameter.Name,
                Value = databaseCommandParameter.Value,
                Direction = databaseCommandParameter.Direction,
                Size = databaseCommandParameter.Size
            };

            if (databaseCommandParameter.DbType.HasValue)
            {
                sqlParameter.DbType = databaseCommandParameter.DbType.Value;
            }

            return sqlParameter;
        }

        private void HandleBulkCopyException(Exception e, SqlBulkCopy sqlBulkCopy)
        {
            // Credit: http://stackoverflow.com/questions/10442686/received-an-invalid-column-length-from-the-bcp-client-for-colid-6
            if (!e.Message.Contains("Received an invalid column length from the bcp client for colid"))
                return;

            try
            {
                string pattern = @"\d+";
                Match match = Regex.Match(e.Message, pattern);
                var index = Convert.ToInt32(match.Value) - 1;

                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi == null) return;
                var sortedColumns = fi.GetValue(sqlBulkCopy);
                var items = (object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(sortedColumns);

                if (items == null) return;
                FieldInfo itemData = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                if (itemData == null) return;
                var metadata = itemData.GetValue(items[index]);

                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(metadata);
                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(metadata);

                throw new Exception($"Column: {column} contains data with a length greater than: {length}");
            }
            catch (Exception)
            {
                throw e;
            }
        }

        private string GetServerNameSql()
        {
            return "SELECT @@SERVERNAME";
        }

        private string GetPaginationSql(PaginationRequest paginationRequest)
        {
            string sql =
@"SELECT {0}
FROM {1} (NOLOCK)
WHERE 1 = 1 {2}
ORDER BY {3}
OFFSET {4} ROWS
FETCH NEXT {5} ROWS ONLY";

            paginationRequest.SetDefaults();

            string offset = (paginationRequest.PageSize * (paginationRequest.PageNumber - 1)).ToString();

            return string.Format(sql, paginationRequest.Columns, paginationRequest.TableName, paginationRequest.GetWhereClause(), paginationRequest.OrderBy, offset, paginationRequest.PageSize.ToString());
        }

        private string GetPaginationCountSql(PaginationRequest paginationRequest)
        {
            string sql =
@"SELECT COUNT(1)
FROM {0} (NOLOCK)
WHERE 1 = 1 {1}";

            paginationRequest.SetDefaults();

            return string.Format(sql, paginationRequest.TableName, paginationRequest.GetWhereClause());
        }
    }
}
