using System.Collections.Generic;

namespace FluentCommander.Core
{
    public interface IConnectionStringProvider
    {
        string Get(string connectionStringName);
        List<string> ConnectionStringNames { get; }
    }
}
