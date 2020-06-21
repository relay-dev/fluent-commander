using System.Collections.Generic;

namespace FluentCommander.Core.Ordering
{
    public class ColumnOrdering
    {
        public List<ColumnOrder> ColumnOrders;

        public ColumnOrdering()
        {
            ColumnOrders = new List<ColumnOrder>();
        }

        public ColumnOrdering(List<ColumnOrder> columnOrders)
        {
            ColumnOrders = columnOrders;
        }
    }
}
