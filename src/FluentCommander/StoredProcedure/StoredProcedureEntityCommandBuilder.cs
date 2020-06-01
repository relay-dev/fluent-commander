﻿using FluentCommander.Core.CommandBuilders;

namespace FluentCommander.StoredProcedure
{
    public abstract class StoredProcedureCommandBuilder<TEntity> : ParameterizedCommandBuilder<StoredProcedureRequest, StoredProcedureCommand<TEntity>, StoredProcedureResult<TEntity>>
    {
        protected StoredProcedureCommandBuilder()
        {
            CommandRequest = new StoredProcedureRequest();
        }

        public StoredProcedureCommand<TEntity> Name(string storedProcedureName)
        {
            CommandRequest.StoredProcedureName = storedProcedureName;

            return this as StoredProcedureCommand<TEntity>;
        }
    }
}
