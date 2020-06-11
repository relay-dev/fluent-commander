using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace FluentCommander.SqlServer
{
    public interface ISqlServerConnectionProvider
    {
        SqlConnection GetConnection();
        Task<SqlConnection> GetConnectionAsync(CancellationToken cancellationToken);
    }
}
