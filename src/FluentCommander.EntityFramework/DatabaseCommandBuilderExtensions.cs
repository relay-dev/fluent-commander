namespace FluentCommander.EntityFramework
{
    public static class DatabaseCommandBuilderExtensions
    {
        public static StoredProcedureEntityCommand<TEntity> ForStoredProcedure<TEntity>(this DatabaseCommandBuilder builder, string storedProcedureName)
        {
            return builder.ForCommand<StoredProcedureEntityCommand<TEntity>>().Name(storedProcedureName);
        }
    }
}
