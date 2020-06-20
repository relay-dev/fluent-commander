namespace FluentCommander.Core.Ordering
{
    public class ColumnOrderingBuilder
    {
        private readonly IHaveColumnOrdering _request;

        public ColumnOrderingBuilder(IHaveColumnOrdering request)
        {
            _request = request;
            _request.ColumnOrdering = new ColumnOrdering();
        }

        public ColumnOrderingBuilder OrderBy(string columnName)
        {
            _request.ColumnOrdering.ColumnOrders.Add(new ColumnOrder(columnName, OrderDirection.Ascending));

            return this;
        }

        public ColumnOrderingBuilder OrderByDescending(string columnName)
        {
            _request.ColumnOrdering.ColumnOrders.Add(new ColumnOrder(columnName, OrderDirection.Descending));

            return this;
        }
    }
}
