using FluentCommander.Core;

namespace FluentCommander
{
    public class StoredProcedureRequest : ParameterizedCommandRequest
    {
        public StoredProcedureRequest() { }

        public StoredProcedureRequest(string storedProcedureName)
        {
            StoredProcedureName = storedProcedureName;
        }

        /// <summary>
        /// The name of the stored procedure to be executed
        /// </summary>
        public string StoredProcedureName { get; set; }
    }
}
