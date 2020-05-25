using FluentCommander.Database.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCommander.Database.Commands
{
    public class PaginationDatabaseCommand : IDatabaseCommand<PaginationResult>
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

        public PaginationDatabaseCommand Timeout(int timeoutInSeconds)
        {
            _paginationRequest.TimeoutInSeconds = timeoutInSeconds;

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
