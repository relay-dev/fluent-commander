namespace FluentCommander.BulkCopy
{
    public abstract partial class BulkCopyCommandBuilder
    {
        public class BulkCopyMappingOptions
        {
            private readonly BulkCopyCommandBuilder _builder;
            private readonly BulkCopyRequest _commandRequest;

            public BulkCopyMappingOptions(BulkCopyCommandBuilder builder, BulkCopyRequest commandRequest)
            {
                _builder = builder;
                _commandRequest = commandRequest;
            }

            public BulkCopyMappingOptions UseAutoMap()
            {
                _builder.IsAutoMap = true;

                return this;
            }

            public BulkCopyMappingOptions UsePartialMap(ColumnMapping columnMapping)
            {
                _commandRequest.ColumnMapping = columnMapping;
                _builder.IsAutoMap = true;

                return this;
            }

            public BulkCopyMappingOptions UseMap(ColumnMapping columnMapping)
            {
                _commandRequest.ColumnMapping = columnMapping;
                _builder.IsAutoMap = false;

                return this;
            }
        }
    }
}
