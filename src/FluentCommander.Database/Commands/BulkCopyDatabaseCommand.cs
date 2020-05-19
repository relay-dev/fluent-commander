using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentCommander.Database.Utility;

namespace FluentCommander.Database.Commands
{
    public class BulkCopyDatabaseCommand : IDatabaseCommand<BulkCopyCommandResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly IAutoMapper _autoMapper;
        private string _tableName;
        private DataTable _dataTable;
        private ColumnMapping _columnMapping;
        private bool _isSuppressAutoMapping;

        public BulkCopyDatabaseCommand(IDatabaseCommander databaseCommander, IAutoMapper autoMapper)
        {
            _databaseCommander = databaseCommander;
            _autoMapper = autoMapper;
        }

        public BulkCopyDatabaseCommand From(DataTable dataTable)
        {
            _dataTable = dataTable;

            return this;
        }

        public BulkCopyDatabaseCommand To(string tableName)
        {
            _tableName = tableName;

            return this;
        }

        public BulkCopyDatabaseCommand AddMap(ColumnMapping columnMapping)
        {
            _columnMapping = columnMapping;
            _isSuppressAutoMapping = true;

            return this;
        }

        public BulkCopyDatabaseCommand AddPartialMap(ColumnMapping columnMapping)
        {
            _columnMapping = columnMapping;

            return this;
        }

        public BulkCopyCommandResult Execute()
        {
            if (_dataTable == null)
            {
                throw new Exception("From(dataTable) must be called before calling the Execute() method");
            }

            if (!_isSuppressAutoMapping)
            {
                _autoMapper.MapDataTableToTable(_tableName, _dataTable, _columnMapping);
            }

            _databaseCommander.BulkCopy(_tableName, _dataTable, _columnMapping);

            return new BulkCopyCommandResult(_dataTable.Rows.Count);
        }

        public async Task<BulkCopyCommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_dataTable == null)
            {
                throw new Exception("DataTable was not set. Please call From(dataTable) before calling the Execute() method");
            }

            if (!_isSuppressAutoMapping)
            {
                _autoMapper.MapDataTableToTable(_tableName, _dataTable, _columnMapping);
            }

            await _databaseCommander.BulkCopyAsync(_tableName, _dataTable, _columnMapping, cancellationToken);

            return new BulkCopyCommandResult(_dataTable.Rows.Count);
        }
    }
}
