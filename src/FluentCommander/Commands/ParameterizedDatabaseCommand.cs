using FluentCommander.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Commands
{
    public abstract class ParameterizedDatabaseCommand<TResult> : IDatabaseCommand<TResult>
    {
        protected readonly List<DatabaseCommandParameter> DatabaseParameters;
        protected TimeSpan TimeoutTimeSpan;

        protected ParameterizedDatabaseCommand()
        {
            DatabaseParameters = new List<DatabaseCommandParameter>();
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

            DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public ParameterizedDatabaseCommand<TResult> AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input,
                DatabaseType = databaseType.ToString()
            };

            databaseParameter.Value ??= DBNull.Value;

            DatabaseParameters.Add(databaseParameter);

            return this;
        }

        public ParameterizedDatabaseCommand<TResult> Timeout(TimeSpan timeout)
        {
            TimeoutTimeSpan = timeout;

            return this;
        }

        public abstract TResult Execute();
        public abstract Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
