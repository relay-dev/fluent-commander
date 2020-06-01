using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Pagination
{
    public class PaginationCommand : PaginationCommandBuilder
    {
        private readonly IDatabaseCommander _databaseCommander;

        public PaginationCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
        }

        public override PaginationResult Execute()
        {
            Validate();

            return _databaseCommander.Paginate(CommandRequest);
        }

        public override async Task<PaginationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            Validate();

            return await _databaseCommander.PaginateAsync(CommandRequest, cancellationToken);
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(CommandRequest.TableName))
            {
                throw new InvalidOperationException("From(target) must be called before calling the Execute() method");
            }
        }
    }
}
