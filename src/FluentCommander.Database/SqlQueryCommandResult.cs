using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FluentCommander.Database
{
    public class SqlQueryCommandResult
    {
        private readonly List<DatabaseCommandParameter> _parameters;
        public DataTable DataTable { get; }
        public bool HasData => DataTable != null && DataTable.Rows.Count > 0;

        public SqlQueryCommandResult(List<DatabaseCommandParameter> parameters, DataTable dataTable)
        {
            _parameters = parameters;
            DataTable = dataTable;
        }

        public Dictionary<string, DatabaseCommandParameter> OutputParameters
        {
            get
            {
                if (_parameters == null)
                {
                    return new Dictionary<string, DatabaseCommandParameter>();
                }

                return _parameters.Where(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput).ToDictionary(kvp => kvp.Name, kvp => kvp);
            }
        }

        public DatabaseCommandParameter ReturnParameter
        {
            get
            {
                DatabaseCommandParameter returnParameter =
                    _parameters.SingleOrDefault(p => p.Direction == ParameterDirection.ReturnValue);

                if (returnParameter == null)
                {
                    throw new InvalidOperationException("No return parameter was found");
                }

                return returnParameter;
            }
        }

        public TResult GetOutputParameter<TResult>(string parameterName)
        {
            if (!OutputParameters.ContainsKey(parameterName))
            {
                throw new Exception($"No Output parameter named {parameterName} was found");
            }

            return (TResult)OutputParameters[parameterName].Value;
        }

        public TResult GetReturnParameter<TResult>()
        {
            return (TResult)ReturnParameter.Value;
        }
    }
}
