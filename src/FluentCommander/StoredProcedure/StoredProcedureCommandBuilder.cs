using FluentCommander.Core.CommandBuilders;

namespace FluentCommander.StoredProcedure
{
    public abstract class StoredProcedureCommandBuilder : ParameterizedCommandBuilder<StoredProcedureCommand, StoredProcedureResult>
    {
        protected readonly StoredProcedureRequest CommandRequest;

        protected StoredProcedureCommandBuilder(StoredProcedureRequest commandRequest)
            : base(commandRequest)
        {
            CommandRequest = commandRequest;
        }

        public StoredProcedureCommand Name(string storedProcedureName)
        {
            CommandRequest.StoredProcedureName = storedProcedureName;

            return this as StoredProcedureCommand;
        }
    }
}
