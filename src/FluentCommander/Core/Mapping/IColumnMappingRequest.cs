namespace FluentCommander.Core.Mapping
{
    public interface IColumnMappingRequest
    {
        ColumnMapping ColumnMapping { get; set; }
        MappingType MappingType { get; set; }
    }
}
