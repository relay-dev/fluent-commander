namespace FluentCommander.Core.Builders
{
    public abstract class ParameterizedSqlCommandBuilder<TBuilder, TResult> : ParameterizedCommandBuilder<TBuilder, TResult> where TBuilder : class
    {
        protected readonly SqlRequest CommandRequest;

        protected ParameterizedSqlCommandBuilder(SqlRequest commandRequest)
            : base(commandRequest)
        {
            CommandRequest = commandRequest;
        }

        public TBuilder Sql(string sql)
        {
            CommandRequest.Sql = sql;

            return this as TBuilder;
        }
    }
}
