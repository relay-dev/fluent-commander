using FluentCommander.Commands;
using FluentCommander.EntityFramework.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.EntityFramework
{
    public class StoredProcedureEntityDatabaseCommand<TEntity> : StoredProcedureDatabaseCommand
    {
        private Action<PropertyMapBuilder<TEntity>> _mappingBuilder;

        public StoredProcedureEntityDatabaseCommand(IDatabaseCommander databaseCommander)
            : base(databaseCommander){ }

        public new StoredProcedureEntityDatabaseCommand<TEntity> Name(string storedProcedureName)
        {
            StoredProcedureRequest.StoredProcedureName = storedProcedureName;

            return this;
        }

        public new StoredProcedureEntityDatabaseCommand<TEntity> AddInputParameter<TParameter>(string parameterName, TParameter parameterValue)
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

        public new StoredProcedureEntityDatabaseCommand<TEntity> AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType)
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

        public new StoredProcedureEntityDatabaseCommand<TEntity> AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType, int size)
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

        public new StoredProcedureEntityDatabaseCommand<TEntity> AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue)
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

        public new StoredProcedureEntityDatabaseCommand<TEntity> AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType)
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

        public new StoredProcedureEntityDatabaseCommand<TEntity> AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType, int size)
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

        public new StoredProcedureEntityDatabaseCommand<TEntity> AddOutputParameter(string parameterName, object databaseType)
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

        public new StoredProcedureEntityDatabaseCommand<TEntity> AddOutputParameter(string parameterName, object databaseType, int size)
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

        public new StoredProcedureEntityDatabaseCommand<TEntity> WithReturnParameter()
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = "ReturnParameter",
                Direction = ParameterDirection.ReturnValue
            };

            StoredProcedureRequest.DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public new StoredProcedureEntityDatabaseCommand<TEntity> Timeout(TimeSpan timeout)
        {
            StoredProcedureRequest.Timeout = timeout;

            return this;
        }

        public StoredProcedureEntityDatabaseCommand<TEntity> Project(Action<PropertyMapBuilder<TEntity>> mappingBuilder)
        {
            _mappingBuilder = mappingBuilder;

            return this;
        }

        // TODO: 
        public StoredProcedureEntityResult<TEntity> ExecuteAndProject()
        {
            StoredProcedureResult storedProcedureResult = base.Execute();

            var options = new PropertyMapBuilder<TEntity>();

            _mappingBuilder(options);

            List<TEntity> result = ReflectionUtility.DataTableToList<TEntity>(storedProcedureResult.DataTable, options);

            return new StoredProcedureEntityResult<TEntity>(storedProcedureResult.Parameters, storedProcedureResult.DataTable, result);
        }

        public async Task<StoredProcedureEntityResult<TEntity>> ExecuteAndProjectAsync(CancellationToken cancellationToken)
        {
            StoredProcedureResult storedProcedureResult = await base.ExecuteAsync(cancellationToken);

            var options = new PropertyMapBuilder<TEntity>();

            _mappingBuilder(options);

            List<TEntity> result = ReflectionUtility.DataTableToList<TEntity>(storedProcedureResult.DataTable, options);

            return new StoredProcedureEntityResult<TEntity>(storedProcedureResult.Parameters, storedProcedureResult.DataTable, result);
        }
    }
}
