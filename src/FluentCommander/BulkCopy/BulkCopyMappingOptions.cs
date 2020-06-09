using FluentCommander.Core.Mapping;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyMappingOptions
    {
        private readonly IHaveColumnMapping _request;
        internal bool IsAutoMap { get; private set; }

        public BulkCopyMappingOptions(IHaveColumnMapping request)
        {
            _request = request;
        }

        public BulkCopyMappingOptions UseAutoMap()
        {
            IsAutoMap = true;

            return this;
        }

        public BulkCopyMappingOptions UsePartialMap(ColumnMapping columnMapping)
        {
            _request.ColumnMapping = columnMapping;
            IsAutoMap = true;

            return this;
        }

        public BulkCopyMappingOptions UseMap(ColumnMapping columnMapping)
        {
            _request.ColumnMapping = columnMapping;
            IsAutoMap = false;

            return this;
        }
    }
}
