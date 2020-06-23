namespace FluentCommander.Core.Ordering
{
    public class ColumnOrder
    {
        /// <summary>
        /// The name of the column to order by.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// The direction to order the column by.
        /// </summary>
        public OrderDirection Direction { get; set; }

        public ColumnOrder() { }

        public ColumnOrder(string columnName, OrderDirection direction)
        {
            ColumnName = columnName;
            Direction = direction;
        }
    }
}
