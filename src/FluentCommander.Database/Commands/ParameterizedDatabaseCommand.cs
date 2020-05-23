using System;
using System.Collections.Generic;
using System.Data;
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

        public ParameterizedDatabaseCommand<TResult> AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, DbType dbType, int size)
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

            Parameters.Add(databaseParameter);

            return this;
        }

        public abstract TResult Execute();
        public abstract Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
