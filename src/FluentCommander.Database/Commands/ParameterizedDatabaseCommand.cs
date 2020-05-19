using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.Commands
{
    public abstract class ParameterizedDatabaseCommand<TResult> : IDatabaseCommand<TResult>
    {
        protected readonly List<DatabaseCommandParameter> Parameters;

        protected ParameterizedDatabaseCommand()
        {
            Parameters = new List<DatabaseCommandParameter>();
        }

        public ParameterizedDatabaseCommand<TResult> AddInputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input
            };

            databaseParameter.Value ??= DBNull.Value;

            Parameters.Add(databaseParameter);

            return this;
        }

        public ParameterizedDatabaseCommand<TResult> AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, DbType dbType)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input,
                DbType = dbType
            };

            databaseParameter.Value ??= DBNull.Value;

            Parameters.Add(databaseParameter);

            return this;
        }

        public ParameterizedDatabaseCommand<TResult> AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput
            };

            databaseParameter.Value ??= DBNull.Value;

            Parameters.Add(databaseParameter);

            return this;
        }

        public ParameterizedDatabaseCommand<TResult> AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, DbType dbType, int size)
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

            Parameters.Add(databaseParameter);

            return this;
        }

        public ParameterizedDatabaseCommand<TResult> AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, DbType dbType)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput,
                DbType = dbType
            };

            databaseParameter.Value ??= DBNull.Value;

            Parameters.Add(databaseParameter);

            return this;
        }

        public ParameterizedDatabaseCommand<TResult> AddOutputParameter(string parameterName)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output
            };

            Parameters.Add(databaseParameter);

            return this;
        }

        public ParameterizedDatabaseCommand<TResult> AddOutputParameter(string parameterName, DbType dbType)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                DbType = dbType
            };

            Parameters.Add(databaseParameter);

            return this;
        }

        public ParameterizedDatabaseCommand<TResult> AddOutputParameter(string parameterName, DbType dbType, int size)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                DbType = dbType,
                Size = size
            };

            Parameters.Add(databaseParameter);

            return this;
        }

        public ParameterizedDatabaseCommand<TResult> WithReturnParameter()
        {
            if (Parameters.Any(p => p.Direction == ParameterDirection.ReturnValue))
            {
                throw new InvalidOperationException("WithReturnParameter() can only be called once");
            }

            var databaseParameter = new DatabaseCommandParameter
            {
                Name = "ReturnParameter",
                Direction = ParameterDirection.ReturnValue
            };

            Parameters.Add(databaseParameter);

            return this;
        }

        public abstract TResult Execute();
        public abstract Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
