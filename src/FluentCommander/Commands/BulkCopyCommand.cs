using FluentCommander.Core;
using FluentCommander.Utility;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Commands
{
    public class BulkCopyCommand : IDatabaseCommand<BulkCopyResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly IAutoMapper _autoMapper;
        private readonly BulkCopyRequest _bulkCopyRequest;
        private readonly BulkCopyMappingOptions _options;
        private bool _isAutoMap;

        public BulkCopyCommand(IDatabaseCommander databaseCommander, IAutoMapper autoMapper)
        {
            _databaseCommander = databaseCommander;
            _autoMapper = autoMapper;
            _bulkCopyRequest = new BulkCopyRequest();
            _options = new BulkCopyMappingOptions(this);
        }

        public BulkCopyCommand From(DataTable dataTable)
        {
            _bulkCopyRequest.DataTable = dataTable;

            return this;
        }

        public BulkCopyCommand Into(string tableName)
        {
            _bulkCopyRequest.TableName = tableName;

            return this;
        }

        public BulkCopyCommand MappingOptions(Func<BulkCopyMappingOptions, BulkCopyMappingOptions> opt)
        {
            opt.Invoke(_options);

            return this;
        }

        public BulkCopyCommand Timeout(TimeSpan timeout)
        {
            _bulkCopyRequest.Timeout = timeout;

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
            private readonly BulkCopyCommand _command;

            public BulkCopyMappingOptions(BulkCopyCommand command)
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
