using System;
using System.Data;

namespace FluentCommander.Core.CommandBuilders
{
    public abstract class ParameterizedCommandBuilder<TCommand, TBuilder, TResult> : ParameterizedInputCommandBuilder<TCommand, TBuilder, TResult> where TBuilder : class
    {
        public TBuilder AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput
            };

            parameter.Value ??= DBNull.Value;

            Parameters.Add(parameter);

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

            Parameters.Add(parameter);

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

            Parameters.Add(parameter);

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

            Parameters.Add(parameter);

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

            Parameters.Add(parameter);

            return this as TBuilder;
        }

        public TBuilder WithReturnParameter()
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = "ReturnParameter",
                Direction = ParameterDirection.ReturnValue
            };

            Parameters.Add(parameter);

            return this as TBuilder;
        }
    }
}
