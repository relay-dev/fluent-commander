using System.Data;

namespace FluentCommander.Utility
{
    public interface IAutoMapper
    {
        void MapDataTableToTable(string tableName, DataTable dataTable, ColumnMapping columnMapping);
    }
}
