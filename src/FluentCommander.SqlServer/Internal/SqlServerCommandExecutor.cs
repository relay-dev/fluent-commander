using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FluentCommander.UnitTests")]
namespace FluentCommander.SqlServer.Internal
{
    internal class SqlServerCommandExecutor : ISqlServerCommandExecutor
    {
        public DataTable Execute(SqlCommand command)
        {
            DataTable dataTable = new DataTable();

            new SqlDataAdapter(command).Fill(dataTable);

            return dataTable;
        }
    }
}
