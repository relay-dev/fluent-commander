﻿using FluentCommander.SqlNonQuery;
using FluentCommander.SqlServer;
using FluentCommander.StoredProcedure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.EntityFramework.SqlServer
{
    public class EntityFrameworkSqlServerDatabaseRequestHandler : SqlServerDatabaseRequestHandler, IDatabaseEntityRequestHandler
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkSqlServerDatabaseRequestHandler(
            DbContext dbContext,
            DatabaseCommandBuilder databaseCommandBuilder,
            SqlConnectionStringBuilder builder,
            ISqlServerCommandExecutor commandExecutor)
            : base(builder, databaseCommandBuilder, commandExecutor)
        {
            _dbContext = dbContext;
        }

        public override SqlNonQueryResult ExecuteNonQuery(SqlNonQueryRequest request)
        {
            int rowCountAffected;

            if (request.Parameters == null)
            {
                rowCountAffected = _dbContext.Database.ExecuteSqlRaw(request.Sql);
            }
            else
            {
                SqlParameter[] parameters = ToSqlParameters(request.Parameters);

                rowCountAffected = _dbContext.Database.ExecuteSqlRaw(request.Sql, parameters);
            }

            return new SqlNonQueryResult(rowCountAffected);
        }

        public override async Task<SqlNonQueryResult> ExecuteNonQueryAsync(SqlNonQueryRequest request, CancellationToken cancellationToken)
        {
            int rowCountAffected;

            if (request.Parameters == null)
            {
                rowCountAffected = await _dbContext.Database.ExecuteSqlRawAsync(request.Sql, cancellationToken);
            }
            else
            {
                SqlParameter[] parameters = ToSqlParameters(request.Parameters);

                rowCountAffected = await _dbContext.Database.ExecuteSqlRawAsync(request.Sql, parameters, cancellationToken);
            }

            return new SqlNonQueryResult(rowCountAffected);
        }

        public override int ExecuteNonQuery(string sql)
        {
            return _dbContext.Database.ExecuteSqlRaw(sql);
        }

        public override async Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }

        public StoredProcedureResult<TEntity> ExecuteStoredProcedure<TEntity>(StoredProcedureRequest request) where TEntity : class
        {
            List<TEntity> data;
            SqlParameter[] parameters = ToSqlParameters(request.Parameters);

            if (parameters == null)
            {
                data = _dbContext.Set<TEntity>().FromSqlRaw(request.StoredProcedureName).ToList();
            }
            else
            {
                string sqlCommand = $"{request.StoredProcedureName} {string.Join(", ", parameters.Select(p => $"@{p.ParameterName}"))}";

                data = _dbContext.Set<TEntity>().FromSqlRaw(sqlCommand, parameters).ToList();

                foreach (DatabaseCommandParameter parameter in request.Parameters.Where(dp => dp.Direction == ParameterDirection.Output || dp.Direction == ParameterDirection.InputOutput || dp.Direction == ParameterDirection.ReturnValue))
                {
                    parameter.Value = parameters.Single(sp => sp.ParameterName == parameter.Name).Value;
                }
            }

            return new StoredProcedureResult<TEntity>(data, request.Parameters);
        }

        public async Task<StoredProcedureResult<TEntity>> ExecuteStoredProcedureAsync<TEntity>(StoredProcedureRequest request, CancellationToken cancellationToken) where TEntity : class
        {
            List<TEntity> data;
            SqlParameter[] parameters = ToSqlParameters(request.Parameters);

            if (parameters == null)
            {
                data = await _dbContext.Set<TEntity>().FromSqlRaw(request.StoredProcedureName).ToListAsync(cancellationToken);
            }
            else
            {
                string sqlCommand = $"{request.StoredProcedureName} {string.Join(", ", parameters.Select(p => $"@{p.ParameterName}"))}";

                data = await _dbContext.Set<TEntity>().FromSqlRaw(sqlCommand, parameters).ToListAsync(cancellationToken);

                foreach (DatabaseCommandParameter parameter in request.Parameters.Where(dp => dp.Direction == ParameterDirection.Output || dp.Direction == ParameterDirection.InputOutput || dp.Direction == ParameterDirection.ReturnValue))
                {
                    parameter.Value = parameters.Single(sp => sp.ParameterName == parameter.Name).Value;
                }
            }

            return new StoredProcedureResult<TEntity>(data, request.Parameters);
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
                if (!Enum.TryParse(databaseCommandParameter.DatabaseType, true, out SqlDbType sqlDbType))
                {
                    throw new InvalidOperationException($"Could not parse databaseType of '{databaseCommandParameter.DatabaseType}' to a System.Data.DbType");
                }

                parameter.SqlDbType = sqlDbType;
            }

            return parameter;
        }
    }
}
