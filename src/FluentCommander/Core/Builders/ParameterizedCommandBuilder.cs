using System;
using System.Data;

namespace FluentCommander.Core.Builders
{
    public abstract class ParameterizedCommandBuilder<TBuilder, TResult> : CommandBuilder<TBuilder, TResult> where TBuilder : class
    {
        private readonly ParameterizedCommandRequest _commandRequest;

        protected ParameterizedCommandBuilder(ParameterizedCommandRequest commandRequest)
            : base(commandRequest)
        {
            _commandRequest = commandRequest;
        }

        public TBuilder AddInputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input
            };

            parameter.Value ??= DBNull.Value;

            _commandRequest.Parameters.Add(parameter);

            return this as TBuilder;
        }

        public TBuilder AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input,
                DatabaseType = databaseType.ToString()
            };

            parameter.Value ??= DBNull.Value;

            _commandRequest.Parameters.Add(parameter);

            return this as TBuilder;
        }

        public TBuilder AddInputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType, int size)
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

            _commandRequest.Parameters.Add(parameter);

            return this as TBuilder;
        }
    }
}
