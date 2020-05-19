namespace FluentCommander.Database
{
    /// <summary>
    /// Allows you to set all input parameters needed to paginate a view
    /// </summary>
    public class PaginationRequest
    {
        // Set the defaults
        public PaginationRequest()
        {
            PageNumber = 1;
            PageSize = 100;
            Columns = "*";
            Conditions = string.Empty;
            OrderBy = "1";
        }

        /// <summary>
        /// The name of the table or view to paginate
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// The index of the page number to return
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The number of records to return in the result
        /// </summary>
        public int PageSize { get; set; }

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
        /// Formats the Conditions to a where clause
        /// </summary>
        /// <returns>The where clause</returns>
        public string GetWhereClause()
        {
            return string.IsNullOrEmpty(Conditions) ? string.Empty : $"AND {Conditions}";
        }
    }
}
