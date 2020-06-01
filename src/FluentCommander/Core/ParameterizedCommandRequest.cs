using System.Collections.Generic;

namespace FluentCommander.Core
{
    public class ParameterizedCommandRequest : DatabaseCommandRequest
    {
        public ParameterizedCommandRequest()
        {
            Parameters = new List<DatabaseCommandParameter>();
        }

        public ParameterizedCommandRequest(List<DatabaseCommandParameter> parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Database parameters required to execute the command
        /// </summary>
        public List<DatabaseCommandParameter> Parameters { get; set; }
    }
}
