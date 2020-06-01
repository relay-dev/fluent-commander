using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentCommander.Core;

namespace FluentCommander.StoredProcedure
{
    public class StoredProcedureResult : DataTableResult
    {
        public StoredProcedureResult(List<DatabaseCommandParameter> parameters, DataTable dataTable)
            : base(dataTable)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// The parameters used when calling the stored procedure
        /// </summary>
        public List<DatabaseCommandParameter> Parameters { get; }

        /// <summary>
        /// The output parameters from the stored procedure
        /// </summary>
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
        
        /// <summary>
        /// If the stored procedure has a return parameter, it can be retrieved here
        /// </summary>
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

        /// <summary>
        /// Casts an output parameter by name to the specified type
        /// </summary>
        /// <typeparam name="TResult">The value of the output parameter</typeparam>
        /// <param name="parameterName">The name of the parameter</param>
        /// <returns>The value of the output parameter</returns>
        public TResult GetOutputParameter<TResult>(string parameterName)
        {
            if (!OutputParameters.ContainsKey(parameterName))
            {
                throw new Exception($"No Output parameter named {parameterName} was found");
            }

            return (TResult)OutputParameters[parameterName].Value;
        }

        /// <summary>
        /// Casts an return parameter by name to the specified type
        /// </summary>
        /// <typeparam name="TResult">The value of the return parameter</typeparam>
        /// <returns>The value of the return parameter</returns>
        public TResult GetReturnParameter<TResult>()
        {
            return (TResult)ReturnParameter.Value;
        }
    }
}
