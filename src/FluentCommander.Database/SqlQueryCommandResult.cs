using System.Data;

namespace FluentCommander.Database
{
    public class SqlQueryCommandResult
    {
        public DataTable DataTable { get; }
        public bool HasData => DataTable != null && DataTable.Rows.Count > 0;

        public SqlQueryCommandResult(DataTable dataTable)
        {
            DataTable = dataTable;
        }

        //public Dictionary<string, DatabaseCommandParameter> OutputParameters
        //{
        //    get
        //    {
        //        if (Parameters == null)
        //        {
        //            return new Dictionary<string, DatabaseCommandParameter>();
        //        }

        //        return Parameters.Where(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput).ToDictionary(kvp => kvp.Name, kvp => kvp);
        //    }
        //}

        //public DatabaseCommandParameter ReturnParameter
        //{
        //    get
        //    {
        //        DatabaseCommandParameter returnParameter =
        //            Parameters.SingleOrDefault(p => p.Direction == ParameterDirection.ReturnValue);

        //        if (returnParameter == null)
        //        {
        //            throw new InvalidOperationException("No return parameter was found");
        //        }

        //        return returnParameter;
        //    }
        //}

        //public TResult GetOutputParameter<TResult>(string parameterName)
        //{
        //    if (!OutputParameters.ContainsKey(parameterName))
        //    {
        //        throw new Exception($"No Output parameter named {parameterName} was found");
        //    }

        //    return (TResult)OutputParameters[parameterName].Value;
        //}

        //public TResult GetReturnParameter<TResult>()
        //{
        //    return (TResult)ReturnParameter.Value;
        //}
    }
}
