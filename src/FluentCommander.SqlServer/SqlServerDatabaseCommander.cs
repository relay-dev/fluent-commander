using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer
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

        public BulkCopyResult BulkCopy(BulkCopyRequest request)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlBulkCopy(connection)
            {
                DestinationTableName = request.TableName
            };

            if (request.TimeoutInSeconds.HasValue)
            {
                command.BulkCopyTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.ColumnMapping != null)
            {
                foreach (ColumnMap columnMap in request.ColumnMapping.ColumnMaps)
                {
                    command.ColumnMappings.Add(columnMap.Source, columnMap.Destination);
                }
            }

            try
            {
                connection.Open();
                command.WriteToServer(request.DataTable);
                connection.Close();
            }
            catch (Exception e)
            {
                HandleBulkCopyException(e, command);

                throw;
            }

            return new BulkCopyResult(request.DataTable.Rows.Count);
        }

        public async Task<BulkCopyResult> BulkCopyAsync(BulkCopyRequest request, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlBulkCopy(connection)
            {
                DestinationTableName = request.TableName
            };

            if (request.TimeoutInSeconds.HasValue)
            {
                command.BulkCopyTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.ColumnMapping != null)
            {
                foreach (ColumnMap columnMap in request.ColumnMapping.ColumnMaps)
                {
                    command.ColumnMappings.Add(columnMap.Source, columnMap.Destination);
                }
            }

            try
            {
                connection.Open();
                await command.WriteToServerAsync(request.DataTable, cancellationToken);
                connection.Close();
            }
            catch (Exception e)
            {
                HandleBulkCopyException(e, command);

                throw;
            }

            return new BulkCopyResult(request.DataTable.Rows.Count);
        }

        public SqlNonQueryResult ExecuteNonQuery(SqlRequest request)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(request.DatabaseParameters));
            }

            connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            connection.Close();

            return new SqlNonQueryResult(numberOfRowsAffected);
        }

        public async Task<SqlNonQueryResult> ExecuteNonQueryAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(request.DatabaseParameters));
            }

            connection.Open();
            var numberOfRowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            connection.Close();

            return new SqlNonQueryResult(numberOfRowsAffected);
        }

        public int ExecuteNonQuery(string sql)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(sql, connection);

            connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            connection.Close();

            return numberOfRowsAffected;
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(sql, connection);

            connection.Open();
            var numberOfRowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            connection.Close();

            return numberOfRowsAffected;
        }

        public TResult ExecuteScalar<TResult>(SqlRequest request)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(request.DatabaseParameters));
            }

            connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        public async Task<TResult> ExecuteScalarAsync<TResult>(SqlRequest request, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(request.DatabaseParameters));
            }

            connection.Open();
            var result = await command.ExecuteScalarAsync(cancellationToken);
            connection.Close();

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        public TResult ExecuteScalar<TResult>(string sql)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(sql, connection);

            connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        public async Task<TResult> ExecuteScalarAsync<TResult>(string sql, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(sql, connection);

            connection.Open();
            var result = await command.ExecuteScalarAsync(cancellationToken);
            connection.Close();

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        public SqlQueryResult ExecuteSql(SqlRequest request)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(request.DatabaseParameters));
            }

            var dataTable = new DataTable();

            connection.Open();
            new SqlDataAdapter(command).Fill(dataTable);
            connection.Close();

            return new SqlQueryResult(dataTable);
        }

        public async Task<SqlQueryResult> ExecuteSqlAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToSqlParameters(request.DatabaseParameters));
            }

            var dataTable = new DataTable();

            connection.Open();
            var reader = await command.ExecuteReaderAsync(cancellationToken);
            dataTable.Load(reader);
            connection.Close();

            return new SqlQueryResult(dataTable);
        }

        public DataTable ExecuteSql(string sql)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(sql, connection);

            var dataTable = new DataTable();

            connection.Open();
            new SqlDataAdapter(command).Fill(dataTable);
            connection.Close();

            return dataTable;
        }

        public async Task<DataTable> ExecuteSqlAsync(string sql, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(sql, connection);

            var dataTable = new DataTable();

            connection.Open();
            var reader = await command.ExecuteReaderAsync(cancellationToken);
            dataTable.Load(reader);
            connection.Close();

            return dataTable;
        }

        public StoredProcedureResult ExecuteStoredProcedure(StoredProcedureRequest request)
        {
            using var connection = new SqlConnection(_builder.ConnectionString);
            using var command = new SqlCommand(request.StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            SqlParameter[] parameters = ToSqlParameters(request.DatabaseParameters);

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            var dataTable = new DataTable();

            connection.Open();
            new SqlDataAdapter(command).Fill(dataTable);
            connection.Close();

            if (request.DatabaseParameters != null)
            {
                foreach (DatabaseCommandParameter databaseCommandParameter in request.DatabaseParameters.Where(dp => dp.Direction == ParameterDirection.Output || dp.Direction == ParameterDirection.InputOutput || dp.Direction == ParameterDirection.ReturnValue))
                {
                    databaseCommandParameter.Value = parameters.Single(sp => sp.ParameterName == databaseCommandParameter.Name).Value;
                }
            }

            return new StoredProcedureResult(request.DatabaseParameters, dataTable);
        }

        public async Task<StoredProcedureResult> ExecuteStoredProcedureAsync(StoredProcedureRequest request, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_builder.ConnectionString);
            await using var command = new SqlCommand(request.StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            SqlParameter[] parameters = ToSqlParameters(request.DatabaseParameters);

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            var dataTable = new DataTable();

            connection.Open();
            var reader = await command.ExecuteReaderAsync(cancellationToken);
            dataTable.Load(reader);
            connection.Close();

            if (request.DatabaseParameters != null)
            {
                foreach (DatabaseCommandParameter databaseCommandParameter in request.DatabaseParameters.Where(dp => dp.Direction == ParameterDirection.Output || dp.Direction == ParameterDirection.InputOutput || dp.Direction == ParameterDirection.ReturnValue))
                {
                    databaseCommandParameter.Value = parameters.Single(sp => sp.ParameterName == databaseCommandParameter.Name).Value;
                }
            }

            return new StoredProcedureResult(request.DatabaseParameters, dataTable);
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

        public PaginationResult Paginate(PaginationRequest request)
        {
            string sql = GetPaginationSql(request);
            string sqlCount = GetPaginationCountSql(request);

            DataTable dataTable = ExecuteSql(sql);

            int totalCount = ExecuteScalar<int>(sqlCount);

            return new PaginationResult(dataTable, totalCount);
        }

        public async Task<PaginationResult> PaginateAsync(PaginationRequest request, CancellationToken cancellationToken)
        {
            string sql = GetPaginationSql(request);
            string sqlCount = GetPaginationCountSql(request);

            Task<DataTable> getDataTask = ExecuteSqlAsync(sql, cancellationToken);

            Task<int> getCountTask = ExecuteScalarAsync<int>(sqlCount, cancellationToken);

            await Task.WhenAll(getDataTask, getCountTask);

            DataTable dataTable = await getDataTask;

            int totalCount = await getCountTask;

            return new PaginationResult(dataTable, totalCount);
        }

        private SqlParameter[] ToSqlParameters(List<DatabaseCommandParameter> databaseCommandParameters)
        {
            return databaseCommandParameters?.Select(ToSqlParameter).ToArray();
        }

        private SqlParameter ToSqlParameter(DatabaseCommandParameter databaseCommandParameter)
        {
            var parameter = new SqlParameter
            {
                ParameterName = databaseCommandParameter.Name,
                Value = databaseCommandParameter.Value,
                Direction = databaseCommandParameter.Direction,
                Size = databaseCommandParameter.Size
            };

            if (!string.IsNullOrEmpty(databaseCommandParameter.DatabaseType))
            {
                if (!Enum.TryParse(databaseCommandParameter.DatabaseType, true, out DbType dbType))
                {
                    throw new InvalidOperationException($"Could not parse databaseType of '{databaseCommandParameter.DatabaseType}' to a System.Data.DbType");
                }

                parameter.DbType = dbType;
            }

            return parameter;
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

        private string GetPaginationSql(PaginationRequest request)
        {
            string sql =
@"SELECT {0}
FROM {1} (NOLOCK)
WHERE 1 = 1 {2}
ORDER BY {3}
OFFSET {4} ROWS
FETCH NEXT {5} ROWS ONLY";

            request.SetDefaults();

            string offset = (request.PageSize * (request.PageNumber - 1)).ToString();

            return string.Format(sql, request.Columns, request.TableName, request.GetWhereClause(), request.OrderBy, offset, request.PageSize.ToString());
        }

        private string GetPaginationCountSql(PaginationRequest request)
        {
            string sql =
@"SELECT COUNT(1)
FROM {0} (NOLOCK)
WHERE 1 = 1 {1}";

            request.SetDefaults();

            return string.Format(sql, request.TableName, request.GetWhereClause());
        }
    }
}
