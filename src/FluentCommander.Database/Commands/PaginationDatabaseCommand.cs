using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.Commands
{
    public class PaginationDatabaseCommand : IDatabaseCommand<PaginationCommandResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly PaginationRequest _paginationRequest;

        public PaginationDatabaseCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
            _paginationRequest = new PaginationRequest();
        }

        public PaginationDatabaseCommand Select(string columns)
        {
            _paginationRequest.Columns = columns;

            return this;
        }

        public PaginationDatabaseCommand From(string target)
        {
            _paginationRequest.TableName = target;

            return this;
        }

        public PaginationDatabaseCommand Where(string conditions)
        {
            _paginationRequest.Conditions = conditions;

            return this;
        }

        public PaginationDatabaseCommand OrderBy(string columns)
        {
            _paginationRequest.OrderBy = columns;

            return this;
        }

        public PaginationDatabaseCommand PageNumber(int pageNumber)
        {
            _paginationRequest.PageNumber = pageNumber;

            return this;
        }

        public PaginationDatabaseCommand PageSize(int pageSize)
        {
            _paginationRequest.PageSize = pageSize;

            return this;
        }

        public PaginationCommandResult Execute()
        {
            Validate();

            PaginationResult paginationResult = _databaseCommander.Paginate(_paginationRequest);

            return new PaginationCommandResult(paginationResult);
        }

        public async Task<PaginationCommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            Validate();

            PaginationResult paginationResult = await _databaseCommander.PaginateAsync(_paginationRequest, cancellationToken);

            return new PaginationCommandResult(paginationResult);
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(_paginationRequest.TableName))
            {
                throw new InvalidOperationException("From(target) must be called before calling the Execute() method");
            }
        }
    }
}
