using FluentCommander.Core;

namespace FluentCommander.Pagination
{
    /// <summary>
    /// Allows you to set all input parameters needed to paginate a view
    /// </summary>
    public class PaginationRequest : DatabaseCommandRequest
    {
        /// <summary>
        /// The name of the table or view to paginate
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Optional: Allows you to limit the columns returned from the view. When not set, the default behavior is to return all columns in the view
        /// </summary>
        public string Columns { get; set; }

        /// <summary>
        /// Optional: Allows you to filter the result returned from the view. When not set, the default behavior is to not apply any filter to the view
        /// </summary>
        public string Conditions { get; set; }

        /// <summary>
        /// Optional: Allows you to order the result returned from the view. When not set, the default behavior is to order by the first column in the view
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// The index of the page number to return
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The number of records to return in the result
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Formats the Conditions to a where clause
        /// </summary>
        /// <returns>The where clause</returns>
        public string GetWhereClause()
        {
            return string.IsNullOrEmpty(Conditions) ? string.Empty : $"AND {Conditions}";
        }

        /// <summary>
        /// Interprets this instance and sets defaults where they have not been set by the client
        /// </summary>
        /// <returns>This instance</returns>
        public PaginationRequest SetDefaults()
        {
            if (PageNumber == 0)
            {
                PageNumber = 1;
            }

            if (PageSize == 0)
            {
                PageSize = 100;
            }

            if (string.IsNullOrEmpty(Columns))
            {
                Columns = "*";
            }

            if (Conditions == null)
            {
                Conditions = string.Empty;
            }

            if (string.IsNullOrEmpty(OrderBy))
            {
                OrderBy = "1";
            }

            return this;
        }
    }
}
