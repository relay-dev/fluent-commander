using FluentCommander.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FluentCommander.EntityFramework
{
    public class EntityFrameworkDatabaseCommander : SqlServerDatabaseCommander
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkDatabaseCommander(DbContext dbContext, SqlConnectionStringBuilder builder,  DatabaseCommandBuilder databaseCommandBuilder)
            : base(builder, databaseCommandBuilder)
        {
            _dbContext = dbContext;
        }

        protected override SqlConnection GetDbConnection()
        {
            return new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString);
        }
    }
}
