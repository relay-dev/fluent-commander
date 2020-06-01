using System.Collections.Generic;
using FluentCommander.Core;

namespace FluentCommander.SqlQuery
{
    public class SqlRequest : ParameterizedCommandRequest
    {
        public SqlRequest() { }

        public SqlRequest(string sql)
        {
            Sql = sql;
        }

        public SqlRequest(string sql, List<DatabaseCommandParameter> databaseParameters)
            : base(databaseParameters)
        {
            Sql = sql;
        }

        /// <summary>
        /// The SQL to be executed
        /// </summary>
        public string Sql { get; set; }
    }
}
