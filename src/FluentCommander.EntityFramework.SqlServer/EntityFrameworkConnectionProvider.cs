using FluentCommander.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.EntityFramework.SqlServer
{
    public class EntityFrameworkConnectionProvider : ISqlServerConnectionProvider
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkConnectionProvider(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString);

            connection.Open();

            return connection;
        }

        public async Task<SqlConnection> GetConnectionAsync(CancellationToken cancellationToken)
        {
            var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString);

            await connection.OpenAsync(cancellationToken);

            return connection;
        }
    }
}
