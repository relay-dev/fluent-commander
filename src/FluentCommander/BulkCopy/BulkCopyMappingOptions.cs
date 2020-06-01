namespace FluentCommander.BulkCopy
{
    public abstract partial class BulkCopyCommandBuilder
    {
        public class BulkCopyMappingOptions
        {
            private readonly BulkCopyCommandBuilder _builder;

            public BulkCopyMappingOptions(BulkCopyCommandBuilder builder)
            {
                _builder = builder;
            }

            public BulkCopyMappingOptions UseAutoMap()
            {
                _builder.IsAutoMap = true;

                return this;
            }

            public BulkCopyMappingOptions UsePartialMap(ColumnMapping columnMapping)
            {
                _builder.CommandRequest.ColumnMapping = columnMapping;
                _builder.IsAutoMap = true;

                return this;
            }

            public BulkCopyMappingOptions UseMap(ColumnMapping columnMapping)
            {
                _builder.CommandRequest.ColumnMapping = columnMapping;
                _builder.IsAutoMap = false;

                return this;
            }
        }
    }
}
