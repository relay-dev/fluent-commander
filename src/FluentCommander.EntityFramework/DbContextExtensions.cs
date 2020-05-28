using Microsoft.EntityFrameworkCore;

namespace FluentCommander.EntityFramework
{
    public static class DbContextExtensions
    {
        public static DbContextCommandBuilder DatabaseCommandBuilder { get; set; }

        public static DbContextCommandBuilder BuildCommand(this DbContext dbContext)
        {
            return DatabaseCommandBuilder.SetContext(dbContext);
        }
    }
}
