<img src="https://i.imgur.com/o7FT6yK.png" alt="Relay" height="150" width="150">

# Fluent Commander

[![Build status](https://ci.appveyor.com/api/projects/status/rbnas7sa2tnl5adl/branch/master?svg=true)](https://ci.appveyor.com/project/sfergusonATX/fluent-commander/branch/master)
[![NuGet Release](https://img.shields.io/nuget/v/FluentCommander.svg)](https://www.nuget.org/packages/FluentCommander/)
[![License](https://img.shields.io/github/license/relay-dev/fluent-commander.svg)](https://github.com/relay-dev/fluent-commander/blob/master/LICENSE)

A lightweight database command abstraction featuring a fluent API

> Fluent Commander is intended to subsidize transactional/ORM data access frameworks. These frameworks typically do not have clean, structured API's when it comes to tasks such as calling a Stored Procedure and retrieving results, or executing a Bulk Copy operation.

> Fluent Commander is built using .NET Standard and currently has SQL Server and Oracle implementations, as seperate NuGet packages.

<br />

## Production Deployment

The NuGet package is available on nuget.org:

https://www.nuget.org/packages/FluentCommander

https://www.nuget.org/packages/FluentCommander.SqlServer

https://www.nuget.org/packages/FluentCommander.Oracle

<br />

## Installation

Here's how you can install the SQL Server NuGet Package:

> #### *.NET Core CLI*
> 
> ```
> dotnet add package FluentCommander.SqlServer
> ```
>
> #### *Package Manager Console*
> 
> ```
> Install-Package FluentCommander.SqlServer
> ```

<br />

Here's how you can install the Oracle NuGet Package:

> #### *.NET Core CLI*
> 
> ```
> dotnet add package FluentCommander.Oracle
> ```
>
> #### *Package Manager Console*
> 
> ```
> Install-Package FluentCommander.Oracle
> ```

<br />

## Samples

### Bulk Copy

The Bulk Copy function is supported if you want to insert a batch of records at once from a DataTable. When Bulk Copying, SQL Server requires a mapping between source (the DataTable you want to persist) and the destination (the database on the server).

#### AutoMapping

This variation automatically maps between the source and destination. The details of this implementation can be found in the FluentCommander.Utility.AutoMapper class. This works well in circumstances where you control the source and can easily ensure the DataTable column names match the column names on the database table:

```c#
private async Task BulkCopyUsingAutoMapping()
{
    DataTable dataTable = GetDataToInsert();

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .Mapping(mapping => mapping.UseAutoMap())
        .ExecuteAsync(new CancellationToken());

    int rowCountCopied = result.RowCountCopied;
}
```

#### Partial Mapping

This variation automatically maps between the source and destination, but also allows you to specify mappings where you know the column names do not match. This works well when you want to use the auto-mapping feature, but you need to specify some additional details:

```c#
private async Task BulkCopyUsingPartialMap()
{
    DataTable dataTable = GetDataToInsert();

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .Mapping(mapping => mapping.UsePartialMap(new ColumnMapping(new List<ColumnMap>
        {
            new ColumnMap
            {
                Source = "SampleString",
                Destination = "SampleVarChar"
            }
        })))
        .ExecuteAsync(new CancellationToken());

    int rowCountCopied = result.RowCountCopied;
}
```

#### Manual Mapping

This variation relies on you to specify mappings where you know the column names do not match. This works well when you have a significant mismatch between the column names of the source and the destination:

```c#
private async Task BulkCopyUsingMap()
{
    DataTable dataTable = GetDataToInsert();

    // Specify the mapping
    var columnMapping = new ColumnMapping
    {
        ColumnMaps = new List<ColumnMap>
        {
            new ColumnMap("Column1", "SampleInt"),
            new ColumnMap("Column2", "SampleSmallInt"),
            new ColumnMap("Column3", "SampleTinyInt"),
            new ColumnMap("Column4", "SampleBit"),
            new ColumnMap("Column5", "SampleDecimal"),
            new ColumnMap("Column6", "SampleFloat"),
            new ColumnMap("Column7", "SampleVarChar"),
        }
    };

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .Mapping(mapping => mapping.UseMap(columnMapping))
        .Timeout(TimeSpan.FromSeconds(30))
        .ExecuteAsync(new CancellationToken());

    int rowCountCopied = result.RowCountCopied;
}
```

#### Strongly-typed Mapping

When you have an entity type that reflects the shape of the table, you can use it to drive your mappings:

```c#
private async Task BulkCopyUsingStronglyTypedMap()
{
    DataTable dataTable = GetDataToInsert();

    // Alter the DataTable to simulate a source where all column names are different than the destination
    dataTable.Columns["SampleInt"].ColumnName = "Column1";
    dataTable.Columns["SampleSmallInt"].ColumnName = "Column2";
    dataTable.Columns["SampleTinyInt"].ColumnName = "Column3";
    dataTable.Columns["SampleBit"].ColumnName = "Column4";
    dataTable.Columns["SampleDecimal"].ColumnName = "Column5";
    dataTable.Columns["SampleFloat"].ColumnName = "Column6";
    dataTable.Columns["SampleVarChar"].ColumnName = "Column7";

    // Bulk Copy
    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .Mapping<SampleEntity>(mapping => mapping.UseMap(entity =>
        {
            entity.Property(e => e.SampleInt).MapFrom("Column1");
            entity.Property(e => e.SampleSmallInt).MapFrom("Column2");
            entity.Property(e => e.SampleTinyInt).MapFrom("Column3");
            entity.Property(e => e.SampleBit).MapFrom("Column4");
            entity.Property(e => e.SampleDecimal).MapFrom("Column5");
            entity.Property(e => e.SampleFloat).MapFrom("Column6");
            entity.Property(e => e.SampleVarChar).MapFrom("Column7");
        }))
        .ExecuteAsync(new CancellationToken());

    int rowCountCopied = result.RowCountCopied;

    Console.WriteLine("Row count copied: {0}", rowCountCopied);
}
```

#### Bulk Copy Options

All BulkCopy options are exposed via the Options API:

```c#
private async Task BulkCopyUsingOptions()
{
    DataTable dataTable = GetDataToInsert();

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .Mapping(mapping => mapping.UseAutoMap())
        .Options(options => options.KeepNulls().CheckConstraints().TableLock(false))
        .ExecuteAsync(new CancellationToken());

    int rowCountCopied = result.RowCountCopied;

    Console.WriteLine("Row count copied: {0}", rowCountCopied);
}
```

#### Bulk Copy Events

The OnRowsCopied event can be subscribed to:

```c#
private async Task BulkCopyUsingEvents()
{
    DataTable dataTable = GetDataToInsert();

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .Mapping(mapping => mapping.UseAutoMap())
        .Events(events => events.NotifyAfter(10).OnRowsCopied((sender, e) =>
        {
            var sqlRowsCopiedEventArgs = (SqlRowsCopiedEventArgs)e;

            Console.WriteLine($"Total rows copied: {sqlRowsCopiedEventArgs.RowsCopied}");
        }))
        .ExecuteAsync(new CancellationToken());

    int rowCountCopied = result.RowCountCopied;

    Console.WriteLine("Row count copied: {0}", rowCountCopied);
}
```

#### All Options

```c#
private async Task BulkCopyUsingAllApis()
{
    DataTable dataTable = GetDataToInsert();

    // Alter the DataTable to simulate a source where the SampleVarChar field is named something different
    dataTable.Columns["SampleVarChar"].ColumnName = "SampleString";

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable, DataRowState.Added)
        .Into("[dbo].[SampleTable]")
        .Options(options => options.KeepNulls().CheckConstraints().TableLock(false))
        .BatchSize(100)
        .Timeout(TimeSpan.FromSeconds(30))
        .Mapping(mapping => mapping.UsePartialMap(new ColumnMapping(new List<ColumnMap>
        {
            new ColumnMap
            {
                Source = "SampleString",
                Destination = "SampleVarChar"
            }
        })))
        .Events(events => events.NotifyAfter(10).OnRowsCopied((sender, e) =>
        {
            var sqlRowsCopiedEventArgs = (SqlRowsCopiedEventArgs)e;

            Console.WriteLine($"Total rows copied: {sqlRowsCopiedEventArgs.RowsCopied}");
        }))
        .ExecuteAsync(new CancellationToken());

    int rowCountCopied = result.RowCountCopied;

    Console.WriteLine("Row count copied: {0}", rowCountCopied);
}
```

### Stored Procedures

This demonstrates how to build a stored procedure command using various combinations of Input, Output and Return parameters. To see the bodies of these Stored Procedures, navigate to the Resources folder and review the setup-*.sql files.

#### Input Parameters

Stored Procedures can be called with various input parameter types. This stored procedure has output, which is found on the result object:

```c#
private async Task ExecuteStoredProcedureWithAllInputTypesAndTableResult()
{
    StoredProcedureResult result = await _databaseCommander.BuildCommand()
        .ForStoredProcedure("[dbo].[usp_AllInputTypes_NoOutput_TableResult]")
        .AddInputParameter("SampleTableID", 1)
        .AddInputParameter("SampleInt", 0)
        .AddInputParameter("SampleSmallInt", 0)
        .AddInputParameter("SampleTinyInt", 0)
        .AddInputParameter("SampleBit", 0)
        .AddInputParameter("SampleDecimal", 0)
        .AddInputParameter("SampleFloat", 0)
        .AddInputParameter("SampleDateTime", DateTime.Now)
        .AddInputParameter("SampleUniqueIdentifier", Guid.NewGuid())
        .AddInputParameter("SampleVarChar", "Row 1")
        .ExecuteAsync(new CancellationToken());

    int count = result.Count;
    bool hasData = result.HasData;
    DataTable dataTable = result.DataTable;
}
```

#### Output Parameters

Stored Procedures with output parameters need to call AddOutputParameter(), and retrieve the output from result.OutputParameters:

```c#
private async Task ExecuteStoredProcedureWithOutput()
{
    string outputParameterName = "SampleOutputInt";
    
    StoredProcedureResult result = await _databaseCommander.BuildCommand()
        .ForStoredProcedure("[dbo].[usp_BigIntInput_IntOutput_NoResult]")
        .AddInputParameter("SampleTableID", 1)
        .AddOutputParameter(outputParameterName, DbType.Int32)
        .ExecuteAsync(new CancellationToken());

    int outputParameter = result.GetOutputParameter<int>(outputParameterName);
}
```

#### InputOutput Parameters

Stored Procedures with InputOutput parameters need to call AddInputOutputParameter(), and retrieve the output from result.OutputParameters:

```c#
private async Task ExecuteStoredProcedureWithInputOutputParameter()
{
    string inputOutputParameterName = "SampleInputOutputInt";

    StoredProcedureResult result = await _databaseCommander.BuildCommand()
        .ForStoredProcedure("[dbo].[usp_BigIntInput_IntInputOutput_TableResult]")
        .AddInputParameter("SampleTableID", 1)
        .AddInputOutputParameter(inputOutputParameterName, 1)
        .ExecuteAsync(new CancellationToken());

    int inputOutputParameter = result.GetOutputParameter<int>(inputOutputParameterName);
}
```

#### Return Parameter

If a Stored Procedures has a Return parameter, the command should call .WithReturnParameter() and the result has the following method that can retrieve the return parameter: result.GetReturnParameter<T>():

```c#
private async Task ExecuteStoredProcedureWithReturnParameter()
{
    StoredProcedureResult result = await _databaseCommander.BuildCommand()
        .ForStoredProcedure("[dbo].[usp_NoInput_NoOutput_ReturnInt]")
        .AddInputParameter("SampleTableID", 1)
        .WithReturnParameter()
        .ExecuteAsync(new CancellationToken());

    int returnParameter = result.GetReturnParameter<int>();
}
```

### Pagination

There are some cases where running pagination queries returned as a DataTable is convenient. This demonstrates how to build command for a SQL pagination query.

#### All Options

In this sample, all options are used:

```c#
private async Task ExecutePaginationAllSettingsAreUsed()
{
    PaginationResult result = await _databaseCommander.BuildCommand()
        .ForPagination()
        .Select("[SampleTableID]")
        .From("[dbo].[SampleTable]")
        .Where("[SampleTableID] < 100")
        .OrderBy("1")
        .PageSize(25)
        .PageNumber(2)
        .Timeout(TimeSpan.FromSeconds(30))
        .ExecuteAsync(new CancellationToken());

    int count = result.Count;
    int totalCount = result.TotalCount;
    bool hasData = result.HasData;
    DataTable dataTable = result.DataTable;
}
```

#### Assume Defaults

Several defaults are specified so the only input required is the target:

```c#
private async Task ExecutePaginationUsingMinimalInput()
{
    PaginationResult result = await _databaseCommander.BuildCommand()
        .ForPagination()
        .From("[dbo].[SampleTable]")
        .ExecuteAsync(new CancellationToken());

    int count = result.Count;
    int totalCount = result.TotalCount;
    bool hasData = result.HasData;
    DataTable dataTable = result.DataTable;
}
```

### Other

There are several other commands available on the API that are often unneeded if you're using an ORM. These miscellaneous commands are available if you should find that you need them

#### SQL Query (Parameterized)

Input parameters require a database type parameter, which can often be inferred by looking at the type of the parameter value. Databases will cache the execution plan and prevent against SQL injection when you paramaterize your queries like this:

```c#
private async Task ExecuteSqlWithInput()
{
    SqlQueryResult result = await _databaseCommander.BuildCommand()
        .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
        .AddInputParameter("SampleTableID", 1)
        .AddInputParameter("SampleVarChar", "Row 1")
        .Timeout(TimeSpan.FromSeconds(30))
        .ExecuteAsync(new CancellationToken());

    int count = result.Count;
    bool hasData = result.HasData;
    DataTable dataTable = result.DataTable;
}
```

#### SQL Non-Query (Parameterized)

SQL Insert and Delete statements can also be parameterized:

```c#
private async Task ExecuteParameterizedInsertDeleteSql()
{
    string sampleVarChar = "Temporary Row";
    string createdBy = "FluentCommander";
    DateTime createdDate = DateTime.UtcNow;

    string insertSql =
@"INSERT INTO [dbo].[SampleTable]
    ([SampleInt]
    ,[SampleSmallInt]
    ,[SampleTinyInt]
    ,[SampleBit]
    ,[SampleDecimal]
    ,[SampleFloat]
    ,[SampleVarChar]
    ,[CreatedBy]
    ,[CreatedDate])
VALUES
    (1
    ,1
    ,1
    ,1
    ,1
    ,1
    ,@SampleVarChar
    ,@CreatedBy
    ,@CreatedDate)";

    SqlNonQueryResult insertResult = await _databaseCommander.BuildCommand()
        .ForSqlNonQuery(insertSql)
        .AddInputParameter("SampleTableID", 1)
        .AddInputParameter("SampleVarChar", sampleVarChar)
        .AddInputParameter("CreatedBy", createdBy)
        .AddInputParameter("CreatedDate", createdDate)
        .ExecuteAsync(new CancellationToken());

    SqlNonQueryResult deleteResult = await _databaseCommander.BuildCommand()
        .ForSqlNonQuery("DELETE FROM [dbo].[SampleTable] WHERE [SampleVarChar] = @SampleVarChar")
        .AddInputParameter("SampleVarChar", sampleVarChar)
        .ExecuteAsync(new CancellationToken());

    int rowCountAffectedFromInsert = insertResult.RowCountAffected;
    int rowCountAffectedFromDelete = deleteResult.RowCountAffected;
}
```

#### SQL Scalar Query (Parameterized)

SQL Scalar queries can also be parameterized:

```c#
private async Task ExecuteScalarWithInput()
{
    bool result = await _databaseCommander.BuildCommand()
        .ForScalar<bool>("SELECT [SampleBit] FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
        .AddInputParameter("SampleTableID", 1)
        .AddInputParameter("SampleVarChar", "Row 1")
        .ExecuteAsync(new CancellationToken());
}
```

There are several other variations of these samples can be found [here](https://github.com/relay-dev/fluent-commander/tree/master/samples/Samples/Commands)