using System.Data;

namespace FluentCommander.Database.Utility
{
    public interface IAutoMapper
    {
        void MapDataTableToTable(string tableName, DataTable dataTable, ColumnMapping columnMapping);
    }
}
