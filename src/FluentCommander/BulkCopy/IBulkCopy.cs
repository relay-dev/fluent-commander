using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.BulkCopy
{
    public interface IBulkCopy
    {
        BulkCopyResult BulkCopy(BulkCopyRequest request);
        Task<BulkCopyResult> BulkCopyAsync(BulkCopyRequest request, CancellationToken cancellationToken);
    }
}
