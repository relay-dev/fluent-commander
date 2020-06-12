namespace FluentCommander.Core.Mapping
{
    public class MappingOptions
    {
        public MappingOptions() { }

        public MappingOptions(MappingType mappingType)
        {
            MappingType = mappingType;
        }

        public MappingType MappingType { get; set; }

        public bool IsAutoMap => MappingType == MappingType.PartialMap || MappingType == MappingType.AutoMap;
    }
}
