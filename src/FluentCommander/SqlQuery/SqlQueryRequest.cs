using FluentCommander.Core;
using FluentCommander.Core.Behaviors;
using System.Collections.Generic;

namespace FluentCommander.SqlQuery
{
    public class SqlQueryRequest : SqlRequest, IHaveReadBehaviors
    {
        public SqlQueryRequest() { }

        public SqlQueryRequest(string sql)
            : base(sql) { }

        public SqlQueryRequest(string sql, List<DatabaseCommandParameter> parameters)
            : base(sql, parameters) { }

        /// <summary>
        /// The behaviors to command should follow
        /// </summary>
        public ReadBehaviors ReadBehaviors { get; set; }
    }
}
