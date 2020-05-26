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

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

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

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

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

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

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

            _storedProcedureRequest.DatabaseParameters.Add(databaseParameter);

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

        public StoredProcedureDatabaseCommand Timeout(TimeSpan timeout)
        {
            _storedProcedureRequest.Timeout = timeout;

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