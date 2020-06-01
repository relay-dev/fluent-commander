using FluentCommander.Core.Utility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyCommand : BulkCopyCommandBuilder
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly IAutoMapper _autoMapper;

        public BulkCopyCommand(IDatabaseCommander databaseCommander, IAutoMapper autoMapper)
        {
            _databaseCommander = databaseCommander;
            _autoMapper = autoMapper;
        }

        public override BulkCopyResult Execute()
        {
            Validate();
            
            if (IsAutoMap)
            {
                _autoMapper.MapDataTableToTable(CommandRequest.TableName, CommandRequest.DataTable, CommandRequest.ColumnMapping);
            }

            return _databaseCommander.BulkCopy(CommandRequest);
        }

        public override async Task<BulkCopyResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            Validate();

            if (IsAutoMap)
            {
                _autoMapper.MapDataTableToTable(CommandRequest.TableName, CommandRequest.DataTable, CommandRequest.ColumnMapping);
            }

            return await _databaseCommander.BulkCopyAsync(CommandRequest, cancellationToken);
        }

        private void Validate()
        {
            if (CommandRequest.DataTable == null)
            {
                throw new InvalidOperationException("From(dataTable) must be called before calling the Execute() method");
            }

            if (string.IsNullOrEmpty(CommandRequest.TableName))
            {
                throw new InvalidOperationException("Into(tableName) must be called before calling the Execute() method");
            }
        }
    }
}
