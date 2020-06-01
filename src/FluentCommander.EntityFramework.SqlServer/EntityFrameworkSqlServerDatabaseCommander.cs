using FluentCommander.SqlNonQuery;
using FluentCommander.SqlQuery;
using FluentCommander.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.EntityFramework.SqlServer
{
    public class EntityFrameworkSqlServerDatabaseCommander : SqlServerDatabaseCommander
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkSqlServerDatabaseCommander(DbContext dbContext, SqlConnectionStringBuilder builder, DatabaseCommandBuilder databaseCommandBuilder)
            : base(builder, databaseCommandBuilder)
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

        protected override SqlConnection GetDbConnection()
        {
            return new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString);
        }
    }
}
