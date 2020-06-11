using FluentCommander.Core;

namespace FluentCommander.BulkCopy
{
    public interface IBulkCopyCommand : IDatabaseCommand<BulkCopyRequest, BulkCopyResult> { }
}
