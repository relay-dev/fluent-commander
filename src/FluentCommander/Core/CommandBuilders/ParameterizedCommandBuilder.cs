using System;
using System.Data;

namespace FluentCommander.Core.CommandBuilders
{
    public abstract class ParameterizedCommandBuilder<TRequest, TBuilder, TResult> : ParameterizedInputCommandBuilder<TRequest, TBuilder, TResult> where TBuilder : class where TRequest : DatabaseCommandRequest
    {
        private readonly ParameterizedCommandRequest _commandRequest;

        protected ParameterizedCommandBuilder(ParameterizedCommandRequest commandRequest)
            : base(commandRequest)
        {
            _commandRequest = commandRequest;
        }

        public TBuilder AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput
            };

            parameter.Value ??= DBNull.Value;

            _commandRequest.Parameters.Add(parameter);

            return this as TBuilder;
        }

        public TBuilder AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput,
                DatabaseType = databaseType.ToString()
            };

            parameter.Value ??= DBNull.Value;

            _commandRequest.Parameters.Add(parameter);

            return this as TBuilder;
        }

        public TBuilder AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType, int size)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput,
                DatabaseType = databaseType.ToString(),
                Size = size
            };

            parameter.Value ??= DBNull.Value;

            _commandRequest.Parameters.Add(parameter);

            return this as TBuilder;
        }

        public TBuilder AddOutputParameter(string parameterName, object databaseType)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                DatabaseType = databaseType.ToString()
            };

            _commandRequest.Parameters.Add(parameter);

            return this as TBuilder;
        }

        public TBuilder AddOutputParameter(string parameterName, object databaseType, int size)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                DatabaseType = databaseType.ToString(),
                Size = size
            };

            _commandRequest.Parameters.Add(parameter);

            return this as TBuilder;
        }

        public TBuilder WithReturnParameter()
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = "ReturnParameter",
                Direction = ParameterDirection.ReturnValue
            };

            _commandRequest.Parameters.Add(parameter);

            return this as TBuilder;
        }
    }
}
