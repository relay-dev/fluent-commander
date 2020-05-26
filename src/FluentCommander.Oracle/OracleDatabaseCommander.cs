﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentCommander.Oracle.Internal;

namespace FluentCommander.Oracle
{
    public class OracleDatabaseCommander : IDatabaseCommander
    {
        private readonly OracleConnectionStringBuilder _builder;
        private readonly DatabaseCommandBuilder _databaseCommandBuilder;

        public OracleDatabaseCommander(OracleConnectionStringBuilder builder, DatabaseCommandBuilder databaseCommandBuilder)
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
            var writer = new OracleBulkCopyWriter(request);

            foreach (DataRow dataRow in request.DataTable.Rows)
            {
                SqlRequest sqlRequest = writer.ToSqlRequest(dataRow);

                ExecuteNonQuery(sqlRequest);
            }

            return new BulkCopyResult(request.DataTable.Rows.Count);
        }

        public async Task<BulkCopyResult> BulkCopyAsync(BulkCopyRequest request, CancellationToken cancellationToken)
        {
            var writer = new OracleBulkCopyWriter(request);

            foreach (DataRow dataRow in request.DataTable.Rows)
            {
                SqlRequest sqlRequest = writer.ToSqlRequest(dataRow);

                await ExecuteNonQueryAsync(sqlRequest, cancellationToken);
            }

            return new BulkCopyResult(request.DataTable.Rows.Count);
        }

        public SqlNonQueryResult ExecuteNonQuery(SqlRequest request)
        {
            using var connection = new OracleConnection(_builder.ConnectionString);
            using var command = new OracleCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToOracleParameters(request.DatabaseParameters));
            }

            connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            connection.Close();

            return new SqlNonQueryResult(numberOfRowsAffected);
        }

        public async Task<SqlNonQueryResult> ExecuteNonQueryAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            await using var connection = new OracleConnection(_builder.ConnectionString);
            await using var command = new OracleCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToOracleParameters(request.DatabaseParameters));
            }

            connection.Open();
            var numberOfRowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            connection.Close();

            return new SqlNonQueryResult(numberOfRowsAffected);
        }

        public int ExecuteNonQuery(string sql)
        {
            using var connection = new OracleConnection(_builder.ConnectionString);
            using var command = new OracleCommand(sql, connection);

            connection.Open();
            var numberOfRowsAffected = command.ExecuteNonQuery();
            connection.Close();

            return numberOfRowsAffected;
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken)
        {
            await using var connection = new OracleConnection(_builder.ConnectionString);
            await using var command = new OracleCommand(sql, connection);

            connection.Open();
            var numberOfRowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            connection.Close();

            return numberOfRowsAffected;
        }

        public TResult ExecuteScalar<TResult>(SqlRequest request)
        {
            using var connection = new OracleConnection(_builder.ConnectionString);
            using var command = new OracleCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToOracleParameters(request.DatabaseParameters));
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
            await using var connection = new OracleConnection(_builder.ConnectionString);
            await using var command = new OracleCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToOracleParameters(request.DatabaseParameters));
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
            using var connection = new OracleConnection(_builder.ConnectionString);
            using var command = new OracleCommand(sql, connection);

            connection.Open();
            var result = command.ExecuteScalar();
            connection.Close();

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        public async Task<TResult> ExecuteScalarAsync<TResult>(string sql, CancellationToken cancellationToken)
        {
            await using var connection = new OracleConnection(_builder.ConnectionString);
            await using var command = new OracleCommand(sql, connection);

            connection.Open();
            var result = await command.ExecuteScalarAsync(cancellationToken);
            connection.Close();

            return result == null || result == DBNull.Value
                ? default(TResult)
                : (TResult)result;
        }

        public SqlQueryResult ExecuteSql(SqlRequest request)
        {
            using var connection = new OracleConnection(_builder.ConnectionString);
            using var command = new OracleCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToOracleParameters(request.DatabaseParameters));
            }

            var dataTable = new DataTable();

            connection.Open();
            new OracleDataAdapter(command).Fill(dataTable);
            connection.Close();

            return new SqlQueryResult(dataTable);
        }

        public async Task<SqlQueryResult> ExecuteSqlAsync(SqlRequest request, CancellationToken cancellationToken)
        {
            await using var connection = new OracleConnection(_builder.ConnectionString);
            await using var command = new OracleCommand(request.Sql, connection);

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(ToOracleParameters(request.DatabaseParameters));
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
            using var connection = new OracleConnection(_builder.ConnectionString);
            using var command = new OracleCommand(sql, connection);

            var dataTable = new DataTable();

            connection.Open();
            new OracleDataAdapter(command).Fill(dataTable);
            connection.Close();

            return dataTable;
        }

        public async Task<DataTable> ExecuteSqlAsync(string sql, CancellationToken cancellationToken)
        {
            await using var connection = new OracleConnection(_builder.ConnectionString);
            await using var command = new OracleCommand(sql, connection);

            var dataTable = new DataTable();

            connection.Open();
            var reader = await command.ExecuteReaderAsync(cancellationToken);
            dataTable.Load(reader);
            connection.Close();

            return dataTable;
        }

        public StoredProcedureResult ExecuteStoredProcedure(StoredProcedureRequest request)
        {
            using var connection = new OracleConnection(_builder.ConnectionString);
            using var command = new OracleCommand(request.StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            OracleParameter[] parameters = ToOracleParameters(request.DatabaseParameters);

            if (request.DatabaseParameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            var dataTable = new DataTable();

            connection.Open();
            new OracleDataAdapter(command).Fill(dataTable);
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
            await using var connection = new OracleConnection(_builder.ConnectionString);
            await using var command = new OracleCommand(request.StoredProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (request.TimeoutInSeconds.HasValue)
            {
                command.CommandTimeout = request.TimeoutInSeconds.Value;
            }

            OracleParameter[] parameters = ToOracleParameters(request.DatabaseParameters);

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

        private OracleParameter[] ToOracleParameters(List<DatabaseCommandParameter> databaseCommandParameters)
        {
            return databaseCommandParameters?.Select(ToOracleParameter).ToArray();
        }

        private OracleParameter ToOracleParameter(DatabaseCommandParameter databaseCommandParameter)
        {
            var parameter = new OracleParameter
            {
                ParameterName = databaseCommandParameter.Name,
                Value = databaseCommandParameter.Value,
                Direction = databaseCommandParameter.Direction,
                Size = databaseCommandParameter.Size
            };

            if (databaseCommandParameter.DbType.HasValue)
            {
                parameter.DbType = databaseCommandParameter.DbType.Value;
            }

            return parameter;
        }

        private string GetServerNameSql()
        {
            return "SELECT host_name FROM v$instance";
        }

        private string GetPaginationSql(PaginationRequest request)
        {
            string sql =
@"SELECT {0}
FROM {1}
WHERE 1 = 1 {2}
ORDER BY {3}
OFFSET 5 ROWS FETCH NEXT 10 ROWS ONLY;";

            request.SetDefaults();

            string offset = (request.PageSize * (request.PageNumber - 1)).ToString();

            return string.Format(sql, request.Columns, request.TableName, request.GetWhereClause(), request.OrderBy, offset, request.PageSize.ToString());
        }

        private string GetPaginationCountSql(PaginationRequest request)
        {
            string sql =
@"SELECT COUNT(*)
FROM {0}
WHERE 1 = 1 {1}";

            request.SetDefaults();

            return string.Format(sql, request.TableName, request.GetWhereClause());
        }
    }
}