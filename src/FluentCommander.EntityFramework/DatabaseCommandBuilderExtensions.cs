namespace FluentCommander.EntityFramework
{
    public static class DatabaseCommandBuilderExtensions
    {
        public static StoredProcedureEntityDatabaseCommand<TEntity> ForStoredProcedure<TEntity>(this DatabaseCommandBuilder builder, string storedProcedureName)
        {
            return builder.ForCommand<StoredProcedureEntityDatabaseCommand<TEntity>>().Name(storedProcedureName);
        }
    }
}
