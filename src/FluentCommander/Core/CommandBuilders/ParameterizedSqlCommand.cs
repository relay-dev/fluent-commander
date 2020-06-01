using FluentCommander.SqlQuery;

namespace FluentCommander.Core.CommandBuilders
{
    public abstract class ParameterizedSqlCommand<TCommand, TResult> : ParameterizedInputCommand<TCommand, TResult> where TCommand : class
    {
        protected readonly SqlRequest SqlRequest;

        protected ParameterizedSqlCommand()
        {
            SqlRequest = new SqlRequest();
        }

        public TCommand Sql(string sql)
        {
            SqlRequest.Sql = sql;

            return this as TCommand;
        }
    }
}
