using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FluentCommander.Database
{
    public class StoredProcedureResult
    {
        public StoredProcedureResult(List<DatabaseCommandParameter> parameters, DataTable dataTable)
        {
            Parameters = parameters;
            DataTable = dataTable;
        }

        /// <summary>
        /// The count of all records returned
        /// </summary>
        public int Count => DataTable.Rows.Count;

        /// <summary>
        /// The records returned from the stored procedure
        /// </summary>
        public DataTable DataTable { get; }

        /// <summary>
        /// Indicates whether or not the DataTable has data in it
        /// </summary>
        public bool HasData => DataTable != null && DataTable.Rows.Count > 0;

        /// <summary>
        /// The records returned for this iteration of the pager
        /// </summary>
        public List<DatabaseCommandParameter> Parameters { get; }

        public Dictionary<string, DatabaseCommandParameter> OutputParameters
        {
            get
            {
                if (Parameters == null)
                {
                    return new Dictionary<string, DatabaseCommandParameter>();
                }

                return Parameters.Where(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput).ToDictionary(kvp => kvp.Name, kvp => kvp);
            }
        }

        public DatabaseCommandParameter ReturnParameter
        {
            get
            {
                DatabaseCommandParameter returnParameter =
                    Parameters.SingleOrDefault(p => p.Direction == ParameterDirection.ReturnValue);

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
