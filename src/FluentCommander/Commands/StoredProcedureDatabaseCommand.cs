using FluentCommander.Core;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Commands
{
    public class StoredProcedureDatabaseCommand : IDatabaseCommand<StoredProcedureResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly StoredProcedureRequest _storedProcedureRequest;

        public StoredProcedureDatabaseCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
            _storedProcedureRequest = new StoredProcedureRequest();
        }

        public StoredProcedureDatabaseCommand Name(string storedProcedureName)
        {
            _storedProcedureRequest.StoredProcedureName = storedProcedureName;

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

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, DbType dbType)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input,
                DbType = dbType
            };

            databaseParameter.Value ??= DBNull.Value;

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, DbType dbType, int size)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input,
                DbType = dbType,
                Size = size
            };

            databaseParameter.Value ??= DBNull.Value;

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

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

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, DbType dbType)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput,
                DbType = dbType
            };

            databaseParameter.Value ??= DBNull.Value;

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, DbType dbType, int size)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput,
                DbType = dbType,
                Size = size
            };

            databaseParameter.Value ??= DBNull.Value;

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddOutputParameter(string parameterName)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output
            };

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddOutputParameter(string parameterName, DbType dbType)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                DbType = dbType
            };

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddOutputParameter(string parameterName, DbType dbType, int size)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                DbType = dbType,
                Size = size
            };

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand WithReturnParameter()
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = "ReturnParameter",
                Direction = ParameterDirection.ReturnValue
            };

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand Timeout(int timeoutInSeconds)
        {
            _storedProcedureRequest.TimeoutInSeconds = timeoutInSeconds;

            return this;
        }

        public StoredProcedureResult Execute()
        {
            return _databaseCommander.ExecuteStoredProcedure(_storedProcedureRequest);
        }

        public async Task<StoredProcedureResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await _databaseCommander.ExecuteStoredProcedureAsync(_storedProcedureRequest, cancellationToken);
        }
    }
}