﻿namespace FluentCommander.Core.Mapping
{
    public interface IHaveColumnMapping
    {
        ColumnMapping ColumnMapping { get; set; }
        MappingType MappingType { get; set; }
    }
}
