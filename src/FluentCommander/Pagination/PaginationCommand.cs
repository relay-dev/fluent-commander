using FluentCommander.Core;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Pagination
{
    public class PaginationCommand : PaginationCommandBuilder
    {
        private readonly IDatabaseRequestHandler _databaseRequestHandler;
        private readonly IRequestValidator<PaginationRequest> _requestValidator;

        public PaginationCommand(
            IDatabaseRequestHandler databaseRequestHandler,
            IRequestValidator<PaginationRequest> requestValidator)
            : base(new PaginationRequest())
        {
            _databaseRequestHandler = databaseRequestHandler;
            _requestValidator = requestValidator;
        }

        public override PaginationResult Execute()
        {
            _requestValidator.Validate(CommandRequest);

            return _databaseRequestHandler.Paginate(CommandRequest);
        }

        public override async Task<PaginationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            _requestValidator.Validate(CommandRequest);

            return await _databaseRequestHandler.PaginateAsync(CommandRequest, cancellationToken);
        }
    }
}
