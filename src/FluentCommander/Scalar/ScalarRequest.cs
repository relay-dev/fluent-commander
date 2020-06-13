using FluentCommander.Core;
using System.Collections.Generic;

namespace FluentCommander.Scalar
{
    public class ScalarRequest : SqlRequest
    {
        public ScalarRequest() { }

        public ScalarRequest(string sql)
            : base(sql) { }

        public ScalarRequest(string sql, List<DatabaseCommandParameter> parameters)
            : base(sql, parameters) { }
    }
}
