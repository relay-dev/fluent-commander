using FluentCommander.Core;
using System.Collections.Generic;

namespace FluentCommander.SqlNonQuery
{
    public class SqlNonQueryRequest : SqlRequest
    {
        public SqlNonQueryRequest() { }

        public SqlNonQueryRequest(string sql)
            : base(sql) { }

        public SqlNonQueryRequest(string sql, List<DatabaseCommandParameter> parameters)
            : base(sql, parameters) { }
    }
}
