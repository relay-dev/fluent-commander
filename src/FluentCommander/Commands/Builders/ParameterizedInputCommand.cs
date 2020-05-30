using System;
using System.Collections.Generic;
using System.Data;
using FluentCommander.Commands.Builders;

namespace FluentCommander.Commands
{
    public abstract class ParameterizedInputCommand<TCommand, TResult> : CommandBuilder<TCommand, TResult> where TCommand : class
    {
        protected readonly List<DatabaseCommandParameter> Parameters;

        protected ParameterizedInputCommand()
        {
            Parameters = new List<DatabaseCommandParameter>();
        }

        public TCommand AddInputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input
            };

            parameter.Value ??= DBNull.Value;

            Parameters.Add(parameter);

            return this as TCommand;
        }

        public TCommand AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input,
                DatabaseType = databaseType.ToString()
            };

            parameter.Value ??= DBNull.Value;

            Parameters.Add(parameter);

            return this as TCommand;
        }

        public TCommand AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType, int size)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input,
                DatabaseType = databaseType.ToString(),
                Size = size
            };

            parameter.Value ??= DBNull.Value;

            Parameters.Add(parameter);

            return this as TCommand;
        }

        
    }
}
