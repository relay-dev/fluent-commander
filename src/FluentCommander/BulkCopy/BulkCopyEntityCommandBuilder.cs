using FluentCommander.Core.Mapping;
using System;

namespace FluentCommander.BulkCopy
{
    public abstract class BulkCopyCommandBuilder<TEntity> : BulkCopyCommandBuilder<BulkCopyCommandBuilder<TEntity>, BulkCopyResult>
    {
        protected BulkCopyCommandBuilder(BulkCopyRequest commandRequest)
            : base(commandRequest) { }

        public BulkCopyCommandBuilder<TEntity> Mapping(Func<MappingOptionsBuilder<TEntity>, MappingOptionsBuilder<TEntity>> mapping)
        {
            mapping.Invoke(new MappingOptionsBuilder<TEntity>(CommandRequest));

            return this;
        }
    }
}
