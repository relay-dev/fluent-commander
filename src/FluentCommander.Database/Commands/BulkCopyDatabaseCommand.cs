using FluentCommander.Database.Core;
using FluentCommander.Database.Utility;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.Commands
{
    public class BulkCopyDatabaseCommand : IDatabaseCommand<BulkCopyResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly IAutoMapper _autoMapper;
        private readonly BulkCopyRequest _bulkCopyRequest;
        private readonly BulkCopyMappingOptions _options;
        private bool _isAutoMap;

        public BulkCopyDatabaseCommand(IDatabaseCommander databaseCommander, IAutoMapper autoMapper)
        {
            _databaseCommander = databaseCommander;
            _autoMapper = autoMapper;
            _bulkCopyRequest = new BulkCopyRequest();
            _options = new BulkCopyMappingOptions(this);
        }

        public BulkCopyDatabaseCommand From(DataTable dataTable)
        {
            _bulkCopyRequest.DataTable = dataTable;

            return this;
        }

        public BulkCopyDatabaseCommand Into(string tableName)
        {
            _bulkCopyRequest.TableName = tableName;

            return this;
        }

        public BulkCopyDatabaseCommand MappingOptions(Func<BulkCopyMappingOptions, BulkCopyMappingOptions> opt)
        {
            opt.Invoke(_options);

            return this;
        }

        public BulkCopyDatabaseCommand Timeout(int timeoutInSeconds)
        {
            _bulkCopyRequest.TimeoutInSeconds = timeoutInSeconds;

            return this;
        }

        public BulkCopyResult Execute()
        {
            Validate();
            
            if (_isAutoMap)
            {
                _autoMapper.MapDataTableToTable(_bulkCopyRequest.TableName, _bulkCopyRequest.DataTable, _bulkCopyRequest.ColumnMapping);
            }

            return _databaseCommander.BulkCopy(_bulkCopyRequest);
        }

        public async Task<BulkCopyResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            Validate();

            if (_isAutoMap)
            {
                _autoMapper.MapDataTableToTable(_bulkCopyRequest.TableName, _bulkCopyRequest.DataTable, _bulkCopyRequest.ColumnMapping);
            }

            return await _databaseCommander.BulkCopyAsync(_bulkCopyRequest, cancellationToken);
        }

        private void Validate()
        {
            if (_bulkCopyRequest.DataTable == null)
            {
                throw new InvalidOperationException("From(dataTable) must be called before calling the Execute() method");
            }

            if (string.IsNullOrEmpty(_bulkCopyRequest.TableName))
            {
                throw new InvalidOperationException("Into(tableName) must be called before calling the Execute() method");
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
                _command._isAutoMap = true;

                return this;
            }

            public BulkCopyMappingOptions UsePartialMap(ColumnMapping columnMapping)
            {
                _command._bulkCopyRequest.ColumnMapping = columnMapping;
                _command._isAutoMap = true;

                return this;
            }

            public BulkCopyMappingOptions UseMap(ColumnMapping columnMapping)
            {
                _command._bulkCopyRequest.ColumnMapping = columnMapping;
                _command._isAutoMap = false;

                return this;
            }
        }
    }
}
