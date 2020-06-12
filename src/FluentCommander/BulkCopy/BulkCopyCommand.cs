using FluentCommander.Core;
using FluentCommander.Core.Mapping;
using FluentCommander.Core.Utility;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyCommand : BulkCopyCommandBuilder
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly IRequestValidator<BulkCopyRequest> _requestValidator;
        private readonly IAutoMapper _autoMapper;

        public BulkCopyCommand(
            IDatabaseCommander databaseCommander,
            IRequestValidator<BulkCopyRequest> requestValidator,
            IAutoMapper autoMapper)
            : base(new BulkCopyRequest())
        {
            _databaseCommander = databaseCommander;
            _requestValidator = requestValidator;
            _autoMapper = autoMapper;
        }

        public override BulkCopyResult Execute()
        {
            _requestValidator.Validate(CommandRequest);
            
            if (CommandRequest.MappingType == MappingType.AutoMap || CommandRequest.MappingType == MappingType.PartialMap)
            {
                _autoMapper.MapDataTableToTable(CommandRequest.DestinationTableName, CommandRequest.DataTable, CommandRequest.ColumnMapping);
            }

            return _databaseCommander.BulkCopy(CommandRequest);
        }

        public override async Task<BulkCopyResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            _requestValidator.Validate(CommandRequest);

            if (CommandRequest.MappingType == MappingType.AutoMap || CommandRequest.MappingType == MappingType.PartialMap)
            {
                _autoMapper.MapDataTableToTable(CommandRequest.DestinationTableName, CommandRequest.DataTable, CommandRequest.ColumnMapping);
            }

            return await _databaseCommander.BulkCopyAsync(CommandRequest, cancellationToken);
        }
    }
}
