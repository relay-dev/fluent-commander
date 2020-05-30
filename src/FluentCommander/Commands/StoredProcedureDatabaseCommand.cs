﻿using FluentCommander.Core;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Commands
{
    public class StoredProcedureDatabaseCommand : IDatabaseCommand<StoredProcedureResult>
    {
        protected readonly IDatabaseCommander DatabaseCommander;
        protected readonly StoredProcedureRequest StoredProcedureRequest;

        public StoredProcedureDatabaseCommand(IDatabaseCommander databaseCommander)
        {
            DatabaseCommander = databaseCommander;
            StoredProcedureRequest = new StoredProcedureRequest();
        }

        public StoredProcedureDatabaseCommand Name(string storedProcedureName)
        {
            StoredProcedureRequest.StoredProcedureName = storedProcedureName;

            return this;
        }

        public StoredProcedureDatabaseCommand AddInputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input
            };

            databaseParameter.Value ??= DBNull.Value;

            StoredProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input,
                DatabaseType = databaseType.ToString()
            };

            databaseParameter.Value ??= DBNull.Value;

            StoredProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType, int size)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input,
                DatabaseType = databaseType.ToString(),
                Size = size
            };

            databaseParameter.Value ??= DBNull.Value;

            StoredProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput
            };

            databaseParameter.Value ??= DBNull.Value;

            StoredProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput,
                DatabaseType = databaseType.ToString()
            };

            databaseParameter.Value ??= DBNull.Value;

            StoredProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType, int size)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput,
                DatabaseType = databaseType.ToString(),
                Size = size
            };

            databaseParameter.Value ??= DBNull.Value;

            StoredProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddOutputParameter(string parameterName, object databaseType)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                DatabaseType = databaseType.ToString()
            };

            StoredProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddOutputParameter(string parameterName, object databaseType, int size)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                DatabaseType = databaseType.ToString(),
                Size = size
            };

            StoredProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand WithReturnParameter()
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = "ReturnParameter",
                Direction = ParameterDirection.ReturnValue
            };

            StoredProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand Timeout(TimeSpan timeout)
        {
            StoredProcedureRequest.Timeout = timeout;

            return this;
        }

        public virtual StoredProcedureResult Execute()
        {
            return DatabaseCommander.ExecuteStoredProcedure(StoredProcedureRequest);
        }

        public virtual async Task<StoredProcedureResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await DatabaseCommander.ExecuteStoredProcedureAsync(StoredProcedureRequest, cancellationToken);
        }
    }
}