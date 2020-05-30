using System;
using System.Data;

namespace FluentCommander.Commands.Builders
{
    public abstract class ParameterizedCommandBuilder<TCommand, TResult> : ParameterizedInputCommand<TCommand, TResult> where TCommand : class
    {
        public TCommand AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput
            };

            parameter.Value ??= DBNull.Value;

            Parameters.Add(parameter);

            return this as TCommand;
        }

        public TCommand AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType)
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

            return this as TCommand;
        }

        public TCommand AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue, object databaseType, int size)
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

            return this as TCommand;
        }

        public TCommand AddOutputParameter(string parameterName, object databaseType)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                DatabaseType = databaseType.ToString()
            };

            Parameters.Add(parameter);

            return this as TCommand;
        }

        public TCommand AddOutputParameter(string parameterName, object databaseType, int size)
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                DatabaseType = databaseType.ToString(),
                Size = size
            };

            Parameters.Add(parameter);

            return this as TCommand;
        }

        public TCommand WithReturnParameter()
        {
            var parameter = new DatabaseCommandParameter
            {
                Name = "ReturnParameter",
                Direction = ParameterDirection.ReturnValue
            };

            Parameters.Add(parameter);

            return this as TCommand;
        }
    }
}
