using System;
using FluentCommander.Core.Behaviors;

namespace FluentCommander.StoredProcedure
{
    public abstract class StoredProcedureCommandBuilder : StoredProcedureCommandBuilderBase<StoredProcedureCommand, StoredProcedureResult>
    {
        protected readonly StoredProcedureRequest CommandRequest;

        protected StoredProcedureCommandBuilder(StoredProcedureRequest commandRequest)
            : base(commandRequest)
        {
            CommandRequest = commandRequest;
        }

        public StoredProcedureCommandBuilder Behaviors(Func<ReadBehaviorsBuilder, ReadBehaviorsBuilder> options)
        {
            options.Invoke(new ReadBehaviorsBuilder(CommandRequest));

            return this;
        }

        public StoredProcedureCommand Name(string storedProcedureName)
        {
            CommandRequest.StoredProcedureName = storedProcedureName;

            return this as StoredProcedureCommand;
        }
    }
}
