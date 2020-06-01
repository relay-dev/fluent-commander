using FluentCommander.Core.CommandBuilders;

namespace FluentCommander.StoredProcedure
{
    public abstract class StoredProcedureCommandBuilder<TEntity> : ParameterizedCommandBuilder<StoredProcedureRequest, StoredProcedureCommand<TEntity>, StoredProcedureResult<TEntity>>
    {
        protected readonly StoredProcedureRequest CommandRequest;

        protected StoredProcedureCommandBuilder(StoredProcedureRequest commandRequest)
            : base(commandRequest)
        {
            CommandRequest = commandRequest;
        }

        public StoredProcedureCommand<TEntity> Name(string storedProcedureName)
        {
            CommandRequest.StoredProcedureName = storedProcedureName;

            return this as StoredProcedureCommand<TEntity>;
        }
    }
}
