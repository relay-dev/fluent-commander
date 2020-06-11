using FluentCommander.Core;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Pagination
{
    public class PaginationCommand : PaginationCommandBuilder
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly IRequestValidator<PaginationRequest> _requestValidator;

        public PaginationCommand(
            IDatabaseCommander databaseCommander,
            IRequestValidator<PaginationRequest> requestValidator)
            : base(new PaginationRequest())
        {
            _databaseCommander = databaseCommander;
            _requestValidator = requestValidator;
        }

        public override PaginationResult Execute()
        {
            _requestValidator.Validate(CommandRequest);

            return _databaseCommander.Paginate(CommandRequest);
        }

        public override async Task<PaginationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            _requestValidator.Validate(CommandRequest);

            return await _databaseCommander.PaginateAsync(CommandRequest, cancellationToken);
        }
    }
}
