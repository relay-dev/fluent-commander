using System.Data;

namespace FluentCommander.Core.Utility
{
    public interface IAutoMapper
    {
        void MapDataTableToTable(string tableName, DataTable dataTable, ColumnMapping columnMapping);
    }
}
