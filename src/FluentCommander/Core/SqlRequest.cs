using System.Collections.Generic;

namespace FluentCommander.Core
{
    public class SqlRequest : ParameterizedCommandRequest
    {
        public SqlRequest() { }

        public SqlRequest(string sql)
        {
            Sql = sql;
        }

        public SqlRequest(string sql, List<DatabaseCommandParameter> parameters)
            : base(parameters)
        {
            Sql = sql;
        }

        /// <summary>
        /// The SQL to be executed
        /// </summary>
        public string Sql { get; set; }
    }
}
