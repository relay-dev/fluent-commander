using FluentCommander.Core;
using FluentCommander.SqlNonQuery;
using FluentCommander.SqlQuery;
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
    public class EntityFrameworkSqlServerDatabaseCommander : SqlServerDatabaseCommander, IDatabaseEntityCommander
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkSqlServerDatabaseCommander(DbContext dbContext, DatabaseCommandBuilder databaseCommandBuilder, IDatabaseCommandFactory commandFactory)
            : base(commandFactory, databaseCommandBuilder)
        {
            _dbContext = dbContext;
        }

        public override SqlNonQueryResult ExecuteNonQuery(SqlRequest request)
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

        public override async Task<SqlNonQueryResult> ExecuteNonQueryAsync(SqlRequest request, CancellationToken cancellationToken)
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
            List<TEntity> result;
            SqlParameter[] parameters = ToSqlParameters(request.Parameters);

            if (parameters == null)
            {
                result = _dbContext.Set<TEntity>().FromSqlRaw(request.StoredProcedureName).ToList();
            }
            else
            {
                string sqlCommand = $"{request.StoredProcedureName} {string.Join(", ", parameters.Select(p => $"@{p.ParameterName}"))}";

                result = _dbContext.Set<TEntity>().FromSqlRaw(sqlCommand, parameters).ToList();

                foreach (DatabaseCommandParameter parameter in request.Parameters.Where(dp => dp.Direction == ParameterDirection.Output || dp.Direction == ParameterDirection.InputOutput || dp.Direction == ParameterDirection.ReturnValue))
                {
                    parameter.Value = parameters.Single(sp => sp.ParameterName == parameter.Name).Value;
                }
            }

            return new StoredProcedureResult<TEntity>(result, request.Parameters);
        }

        public async Task<StoredProcedureResult<TEntity>> ExecuteStoredProcedureAsync<TEntity>(StoredProcedureRequest request, CancellationToken cancellationToken) where TEntity : class
        {
            List<TEntity> result;
            SqlParameter[] parameters = ToSqlParameters(request.Parameters);

            if (parameters == null)
            {
                result = await _dbContext.Set<TEntity>().FromSqlRaw(request.StoredProcedureName).ToListAsync(cancellationToken);
            }
            else
            {
                string sqlCommand = $"{request.StoredProcedureName} {string.Join(", ", parameters.Select(p => $"@{p.ParameterName}"))}";

                result = await _dbContext.Set<TEntity>().FromSqlRaw(sqlCommand, parameters).ToListAsync(cancellationToken);

                foreach (DatabaseCommandParameter parameter in request.Parameters.Where(dp => dp.Direction == ParameterDirection.Output || dp.Direction == ParameterDirection.InputOutput || dp.Direction == ParameterDirection.ReturnValue))
                {
                    parameter.Value = parameters.Single(sp => sp.ParameterName == parameter.Name).Value;
                }
            }

            return new StoredProcedureResult<TEntity>(result, request.Parameters);
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
