using FluentCommander.Core.Builders;

namespace FluentCommander.Pagination
{
    public abstract class PaginationCommandBuilder : CommandBuilder<PaginationCommandBuilder, PaginationResult>
    {
        protected readonly PaginationRequest CommandRequest;

        protected PaginationCommandBuilder(PaginationRequest commandRequest)
            : base(commandRequest)
        {
            CommandRequest = commandRequest;
        }

        public PaginationCommandBuilder Select(string columns)
        {
            CommandRequest.Columns = columns;

            return this;
        }

        public PaginationCommandBuilder From(string target)
        {
            CommandRequest.TableName = target;

            return this;
        }

        public PaginationCommandBuilder Where(string conditions)
        {
            CommandRequest.Conditions = conditions;

            return this;
        }

        public PaginationCommandBuilder OrderBy(string columns)
        {
            CommandRequest.OrderBy = columns;

            return this;
        }

        public PaginationCommandBuilder PageNumber(int pageNumber)
        {
            CommandRequest.PageNumber = pageNumber;

            return this;
        }

        public PaginationCommandBuilder PageSize(int pageSize)
        {
            CommandRequest.PageSize = pageSize;

            return this;
        }
    }
}
