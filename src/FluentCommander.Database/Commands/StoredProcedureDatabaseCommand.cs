using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.Commands
{
    public class StoredProcedureDatabaseCommand : IDatabaseCommand<StoredProcedureCommandResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly List<DatabaseCommandParameter> _parameters;
        private string _storedProcedureName;

        public StoredProcedureDatabaseCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
            _parameters = new List<DatabaseCommandParameter>();
        }

        public StoredProcedureDatabaseCommand Name(string storedProcedureName)
        {
            _storedProcedureName = storedProcedureName;

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

            _parameters.Add(databaseParameter);

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

            _parameters.Add(databaseParameter);

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

            _parameters.Add(databaseParameter);

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

            _parameters.Add(databaseParameter);

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

            _parameters.Add(databaseParameter);

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

            _parameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand AddOutputParameter(string parameterName)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output
            };

            _parameters.Add(databaseParameter);

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

            _parameters.Add(databaseParameter);

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

            _parameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureDatabaseCommand WithReturnParameter()
        {
            if (_parameters.Any(p => p.Direction == ParameterDirection.ReturnValue))
            {
                throw new InvalidOperationException("WithReturnParameter() can only be called once");
            }

            var databaseParameter = new DatabaseCommandParameter
            {
                Name = "ReturnParameter",
                Direction = ParameterDirection.ReturnValue
            };

            _parameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureCommandResult Execute()
        {
            StoredProcedureResult result = _databaseCommander.ExecuteStoredProcedure(_storedProcedureName, _parameters);

            return new StoredProcedureCommandResult(result.Parameters, result.DataTable);
        }

        public async Task<StoredProcedureCommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            StoredProcedureResult result = await _databaseCommander.ExecuteStoredProcedureAsync(_storedProcedureName, cancellationToken, _parameters);

            return new StoredProcedureCommandResult(result);
        }
    }
}