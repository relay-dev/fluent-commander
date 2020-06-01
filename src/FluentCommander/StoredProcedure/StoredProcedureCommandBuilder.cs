﻿using FluentCommander.Core.CommandBuilders;

namespace FluentCommander.StoredProcedure
{
    public abstract class StoredProcedureCommandBuilder : ParameterizedCommandBuilder<StoredProcedureRequest, StoredProcedureCommand, StoredProcedureResult>
    {
        protected StoredProcedureCommandBuilder()
        {
            CommandRequest = new StoredProcedureRequest();
        }

        public StoredProcedureCommand Name(string storedProcedureName)
        {
            CommandRequest.StoredProcedureName = storedProcedureName;

            return this as StoredProcedureCommand;
        }
    }
}
