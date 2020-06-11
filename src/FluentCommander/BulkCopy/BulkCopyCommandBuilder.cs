using FluentCommander.Core.CommandBuilders;
using FluentCommander.Core.Mapping;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.BulkCopy
{
    public abstract class BulkCopyCommandBuilder : CommandBuilder<BulkCopyCommandBuilder, BulkCopyResult>
    {
        protected readonly BulkCopyRequest CommandRequest;

        protected BulkCopyCommandBuilder(BulkCopyRequest commandRequest)
            : base(commandRequest)
        {
            CommandRequest = commandRequest;
        }

        public BulkCopyCommandBuilder BatchSize(int batchSize)
        {
            CommandRequest.BatchSize = batchSize;

            return this;
        }

        public BulkCopyCommandBuilder Events(Func<BulkCopyCommandEventsBuilder, BulkCopyCommandEventsBuilder> events)
        {
            events.Invoke(new BulkCopyCommandEventsBuilder(CommandRequest));

            return this;
        }

        public BulkCopyCommandBuilder From(IDataReader dataReader)
        {
            CommandRequest.DataReader = dataReader;
            CommandRequest.EnableStreaming = true;

            return this;
        }

        public BulkCopyCommandBuilder From(DataRow[] dataRows)
        {
            CommandRequest.DataRows = dataRows;

            return this;
        }

        public BulkCopyCommandBuilder From(DataTable dataTable)
        {
            CommandRequest.DataTable = dataTable;

            return this;
        }

        public BulkCopyCommandBuilder From(DataTable dataTable, DataRowState dataRowState)
        {
            CommandRequest.DataTable = dataTable;
            CommandRequest.DataRowState = dataRowState;

            return this;
        }

        public BulkCopyCommandBuilder From(DbDataReader dbDataReader)
        {
            CommandRequest.DbDataReader = dbDataReader;
            CommandRequest.EnableStreaming = true;

            return this;
        }

        public BulkCopyCommandBuilder Into(string tableName)
        {
            CommandRequest.DestinationTableName = tableName;

            return this;
        }

        public BulkCopyCommandBuilder Mapping(Func<MappingOptionsBuilder, MappingOptionsBuilder> mapping)
        {
            mapping.Invoke(new MappingOptionsBuilder(CommandRequest));

            return this;
        }

        public BulkCopyCommandBuilder Mapping<TEntity>(Func<MappingOptionsBuilder<TEntity>, MappingOptionsBuilder<TEntity>> mapping)
        {
            mapping.Invoke(new MappingOptionsBuilder<TEntity>(CommandRequest));

            return this;
        }

        public BulkCopyCommandBuilder Options(Func<BulkCopyCommandOptionsBuilder, BulkCopyCommandOptionsBuilder> options)
        {
            options.Invoke(new BulkCopyCommandOptionsBuilder(CommandRequest));

            return this;
        }

        public abstract override BulkCopyResult Execute();
        public abstract override Task<BulkCopyResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
