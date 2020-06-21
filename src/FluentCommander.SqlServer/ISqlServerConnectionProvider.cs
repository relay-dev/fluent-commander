using FluentCommander.Core.Options;
using Microsoft.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.SqlServer
{
    public interface ISqlServerConnectionProvider
    {
        SqlConnection GetConnection(CommandOptions options);
        Task<SqlConnection> GetConnectionAsync(CancellationToken cancellationToken);
    }
}
