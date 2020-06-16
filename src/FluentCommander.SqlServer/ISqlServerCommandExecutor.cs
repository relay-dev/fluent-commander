using Microsoft.Data.SqlClient;
using System.Data;

namespace FluentCommander.SqlServer
{
    public interface ISqlServerCommandExecutor
    {
        DataTable Execute(SqlCommand command);
    }
}
