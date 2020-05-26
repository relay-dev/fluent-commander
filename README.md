<img src="https://imgur.com/KLdrWTh.png" alt="Relay" height="150" width="150">

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
public async Task BulkCopyUsingAutoMapping()
{
    DataTable dataTable = GetDataToInsert();

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .MappingOptions(opt => opt.UseAutoMap())
        .ExecuteAsync(new CancellationToken());

    int rowCountCopied = result.RowCountCopied;
}
```

#### Partial Mapping

This variation automatically maps between the source and destination, but also allows you to specify mappings where you know the column names do not match. This works well when you want to use the auto-mapping feature, but you need to specify some additional details:

```c#
public async Task BulkCopyUsingPartialMap()
{
    DataTable dataTable = GetDataToInsert();

    // Specify the mapping
    var columnMapping = new ColumnMapping
    {
        ColumnMaps = new List<ColumnMap>
        {
            new ColumnMap("SampleString", "SampleVarChar")
        }
    };

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .MappingOptions(opt => opt.UsePartialMap(columnMapping))
        .ExecuteAsync(new CancellationToken());

    int rowCountCopied = result.RowCountCopied;
}
```

#### Manual Mapping

This variation relies on you to specify mappings where you know the column names do not match. This works well when you have a significant mismatch between the column names of the source and the destination:

```c#
public async Task BulkCopyUsingMap()
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
        .MappingOptions(opt => opt.UseMap(columnMapping))
        .Timeout(TimeSpan.FromSeconds(30))
        .ExecuteAsync(new CancellationToken());

    int rowCountCopied = result.RowCountCopied;
}
```

### Stored Procedures

This demonstrates how to build a stored procedure command using various combinations of input, output and return parameters. To see the bodies of these Stored Procedures, navigate to the Resources folder and review the setup-*.sql files.

#### Input Parameters

Stored Procedures can be called with various input parameter types. This stored procedure has output, which is found on the result object:

```c#
public async Task ExecuteStoredProcedureWithAllInputTypesAndTableResult()
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
public async Task ExecuteStoredProcedureWithOutput()
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
public async Task ExecuteStoredProcedureWithInputOutputParameter()
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

Stored Procedures with Return parameters can retrieve them from result.ReturnParameters:

```c#
public async Task ExecuteStoredProcedureWithReturnParameter()
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
public async Task ExecutePaginationAllSettingsAreUsed()
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

#### Assume Defualts

Several defaults are specified so the only input required is the target:

```c#
public async Task ExecutePaginationUsingMinimalInput()
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

There are several other commands available on the API that could often be considered less attractive when compared to an ORM. These miscellaneous commands are available if you should find that you need them

#### Parameterized SQL Query

Input parameters require a database type parameter, which can often be inferred by looking at the type of the parameter value:

```c#
public async Task ExecuteSqlWithInput()
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

#### Parameterized SQL Non-Query

SQL insert and delete statements with parameters can be parameterized for SQL Server to cache the execution plan and to avoid SQL injection:

```c#
public async Task ExecuteParameterizedInsertDeleteSql()
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

#### Parameterized SQL Scalar Query

SQL queries with parameters can be parameterized for SQL Server to cache the execution plan and to avoid SQL injection. This method demonstrates how to query the database with inline SQL using input parameters:

```c#
public async Task ExecuteScalarWithInput()
{
    bool result = await _databaseCommander.BuildCommand()
        .ForScalar<bool>("SELECT [SampleBit] FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
        .AddInputParameter("SampleTableID", 1)
        .AddInputParameter("SampleVarChar", "Row 1")
        .ExecuteAsync(new CancellationToken());
}
```

There are several other variations of these samples can be found [here](https://github.com/relay-dev/fluent-commander/tree/master/samples/Samples/Commands)