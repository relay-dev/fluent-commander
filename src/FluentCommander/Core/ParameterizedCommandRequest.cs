using System.Collections.Generic;

namespace FluentCommander.Core
{
    public class ParameterizedCommandRequest : DatabaseCommandRequest
    {
        public ParameterizedCommandRequest()
        {
            DatabaseParameters = new List<DatabaseCommandParameter>();
        }

        public ParameterizedCommandRequest(List<DatabaseCommandParameter> databaseParameters)
        {
            DatabaseParameters = databaseParameters;
        }

        /// <summary>
        /// Database parameters required to execute the command
        /// </summary>
        public List<DatabaseCommandParameter> DatabaseParameters { get; set; }
    }
}
