using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.EntityFramework.SqlServer.Entities
{
    public class DatabaseCommanderDomainContext : DatabaseCommanderContext
    {
        private readonly IConfiguration _config;

        public DatabaseCommanderDomainContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
