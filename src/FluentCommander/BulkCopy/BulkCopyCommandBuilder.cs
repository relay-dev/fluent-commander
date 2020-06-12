using FluentCommander.Core.CommandBuilders;
using FluentCommander.Core.Mapping;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.BulkCopy
{
    public abstract class BulkCopyCommandBuilder<TBuilder, TResult> : CommandBuilder<TBuilder, TResult> where TBuilder : class
    {
        protected readonly BulkCopyRequest CommandRequest;

        protected BulkCopyCommandBuilder(BulkCopyRequest commandRequest)
            : base(commandRequest)
        {
            CommandRequest = commandRequest;
        }

        public TBuilder BatchSize(int batchSize)
        {
            CommandRequest.BatchSize = batchSize;

            return this as TBuilder;
        }

        public TBuilder Events(Func<BulkCopyCommandEventsBuilder, BulkCopyCommandEventsBuilder> events)
        {
            events.Invoke(new BulkCopyCommandEventsBuilder(CommandRequest));

            return this as TBuilder;
        }

        public TBuilder From(IDataReader dataReader)
        {
            CommandRequest.DataReader = dataReader;
            CommandRequest.EnableStreaming = true;

            return this as TBuilder;
        }

        public TBuilder From(DataRow[] dataRows)
        {
            CommandRequest.DataRows = dataRows;

            return this as TBuilder;
        }

        public TBuilder From(DataTable dataTable)
        {
            CommandRequest.DataTable = dataTable;

            return this as TBuilder;
        }

        public TBuilder From(DataTable dataTable, DataRowState dataRowState)
        {
            CommandRequest.DataTable = dataTable;
            CommandRequest.DataRowState = dataRowState;

            return this as TBuilder;
        }

        public TBuilder From(DbDataReader dbDataReader)
        {
            CommandRequest.DbDataReader = dbDataReader;
            CommandRequest.EnableStreaming = true;

            return this as TBuilder;
        }

        public TBuilder Into(string tableName)
        {
            CommandRequest.DestinationTableName = tableName;

            return this as TBuilder;
        }

        public TBuilder Mapping(Func<MappingOptionsBuilder, MappingOptionsBuilder> mapping)
        {
            mapping.Invoke(new MappingOptionsBuilder(CommandRequest));

            return this as TBuilder;
        }

        public TBuilder Options(Func<BulkCopyCommandOptionsBuilder, BulkCopyCommandOptionsBuilder> options)
        {
            options.Invoke(new BulkCopyCommandOptionsBuilder(CommandRequest));

            return this as TBuilder;
        }

        public abstract override TResult Execute();
        public abstract override Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
    }

    public abstract class BulkCopyCommandBuilder : BulkCopyCommandBuilder<BulkCopyCommandBuilder, BulkCopyResult>
    {
        protected BulkCopyCommandBuilder(BulkCopyRequest commandRequest)
            : base(commandRequest) { }
    }
}
