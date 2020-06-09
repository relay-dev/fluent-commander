using FluentCommander.Core;
using Microsoft.EntityFrameworkCore;

namespace FluentCommander.EntityFramework
{
    public class DbContextCommandBuilder : DatabaseCommandBuilder
    {
        internal DbContext DbContext { get; private set; }

        public DbContextCommandBuilder(IDatabaseCommandFactory commandFactory) 
            : base(commandFactory) { }

        public DbContextCommandBuilder SetContext(DbContext dbContext)
        {
            DbContext = dbContext;

            return this;
        }
    }
}
