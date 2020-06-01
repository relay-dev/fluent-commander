using System;
using System.Data;

namespace FluentCommander.Core.CommandBuilders
{
    public abstract class ParameterizedInputCommandBuilder<TRequest, TBuilder, TResult> : CommandBuilder<TRequest, TBuilder, TResult> where TBuilder : class where TRequest : DatabaseCommandRequest
    {
        private readonly ParameterizedCommandRequest _commandRequest;

        protected ParameterizedInputCommandBuilder(ParameterizedCommandRequest commandRequest)
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
