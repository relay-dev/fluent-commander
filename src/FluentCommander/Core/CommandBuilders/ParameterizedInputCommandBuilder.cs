using System;
using System.Collections.Generic;
using System.Data;

namespace FluentCommander.Core.CommandBuilders
{
    public abstract class ParameterizedInputCommandBuilder<TRequest, TBuilder, TResult> : CommandBuilderBase<TRequest, TBuilder, TResult> where TBuilder : class
    {
        protected readonly List<DatabaseCommandParameter> Parameters;

        protected ParameterizedInputCommandBuilder()
        {
            Parameters = new List<DatabaseCommandParameter>();
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

            Parameters.Add(parameter);

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

            Parameters.Add(parameter);

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

            Parameters.Add(parameter);

            return this as TBuilder;
        }

        
    }
}
