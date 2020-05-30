using FluentCommander.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Commands
{
    public class PaginationCommand : IDatabaseCommand<PaginationResult>
    {
        private readonly IDatabaseCommander _databaseCommander;
        private readonly PaginationRequest _paginationRequest;

        public PaginationCommand(IDatabaseCommander databaseCommander)
        {
            _databaseCommander = databaseCommander;
            _paginationRequest = new PaginationRequest();
        }

        public PaginationCommand Select(string columns)
        {
            _paginationRequest.Columns = columns;

            return this;
        }

        public PaginationCommand From(string target)
        {
            _paginationRequest.TableName = target;

            return this;
        }

        public PaginationCommand Where(string conditions)
        {
            _paginationRequest.Conditions = conditions;

            return this;
        }

        public PaginationCommand OrderBy(string columns)
        {
            _paginationRequest.OrderBy = columns;

            return this;
        }

        public PaginationCommand PageNumber(int pageNumber)
        {
            _paginationRequest.PageNumber = pageNumber;

            return this;
        }

        public PaginationCommand PageSize(int pageSize)
        {
            _paginationRequest.PageSize = pageSize;

            return this;
        }

        public PaginationCommand Timeout(TimeSpan timeout)
        {
            _paginationRequest.Timeout = timeout;

            return this;
        }

        public PaginationResult Execute()
        {
            Validate();

            return _databaseCommander.Paginate(_paginationRequest);
        }

        public async Task<PaginationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            Validate();

            return await _databaseCommander.PaginateAsync(_paginationRequest, cancellationToken);
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
