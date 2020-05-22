using FluentCommander.Database.Utility;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.Commands
{
    public class BulkCopyDatabaseCommand : IDatabaseCommand<BulkCopyCommandResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly IAutoMapper _autoMapper;
        private readonly BulkCopyMappingOptions _options;
        private string _tableName;
        private DataTable _dataTable;
        private ColumnMapping _columnMapping;
        private bool _isSuppressAutoMapping;

        public BulkCopyDatabaseCommand(IDatabaseCommander databaseCommander, IAutoMapper autoMapper)
        {
            _databaseCommander = databaseCommander;
            _autoMapper = autoMapper;
            _options = new BulkCopyMappingOptions(this);
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

        public BulkCopyDatabaseCommand MappingOptions(Func<BulkCopyMappingOptions, BulkCopyMappingOptions> opt)
        {
            opt.Invoke(_options);

            return this;
        }
        
        public BulkCopyCommandResult Execute()
        {
            Validate();
            
            if (!_isSuppressAutoMapping)
            {
                _autoMapper.MapDataTableToTable(_tableName, _dataTable, _columnMapping);
            }

            _databaseCommander.BulkCopy(_tableName, _dataTable, _columnMapping);

            return new BulkCopyCommandResult(_dataTable.Rows.Count);
        }

        public async Task<BulkCopyCommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            Validate();

            if (!_isSuppressAutoMapping)
            {
                _autoMapper.MapDataTableToTable(_tableName, _dataTable, _columnMapping);
            }

            await _databaseCommander.BulkCopyAsync(_tableName, _dataTable, _columnMapping, cancellationToken);

            return new BulkCopyCommandResult(_dataTable.Rows.Count);
        }

        private void Validate()
        {
            if (_dataTable == null)
            {
                throw new InvalidOperationException("From(dataTable) must be called before calling the Execute() method");
            }

            if (string.IsNullOrEmpty(_tableName))
            {
                throw new InvalidOperationException("To(tableName) must be called before calling the Execute() method");
            }
        }

        public class BulkCopyMappingOptions
        {
            private readonly BulkCopyDatabaseCommand _command;

            public BulkCopyMappingOptions(BulkCopyDatabaseCommand command)
            {
                _command = command;
            }

            public BulkCopyMappingOptions UseAutoMap()
            {
                _command._isSuppressAutoMapping = false;

                return this;
            }

            public BulkCopyMappingOptions UsePartialMap(ColumnMapping columnMapping)
            {
                _command._columnMapping = columnMapping;

                return this;
            }

            public BulkCopyMappingOptions UseMap(ColumnMapping columnMapping)
            {
                _command._columnMapping = columnMapping;
                _command._isSuppressAutoMapping = true;

                return this;
            }
        }
    }
}
