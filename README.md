<img src="https://i.imgur.com/o7FT6yK.png" alt="Fluent Commander" height="100" width="100">

# Fluent Commander

[![Build status](https://ci.appveyor.com/api/projects/status/rbnas7sa2tnl5adl/branch/master?svg=true)](https://ci.appveyor.com/project/sfergusonATX/fluent-commander/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/relay-dev/fluent-commander/badge.svg?branch=master&service=github)](https://coveralls.io/github/relay-dev/fluent-commander?branch=master)
[![NuGet Release](https://img.shields.io/nuget/v/FluentCommander.svg)](https://www.nuget.org/packages/FluentCommander/)
[![MyGet](https://img.shields.io/myget/relay-dev/v/FluentCommander?color=red&label=myget)](https://www.myget.org/feed/relay-dev/package/nuget/FluentCommander)
[![License](https://img.shields.io/github/license/relay-dev/fluent-commander.svg)](https://github.com/relay-dev/fluent-commander/blob/master/LICENSE)

## Introduction

Fluent Commander is a lightweight library for executing asynchronous database commands with a fluent API. It is intended to subsidize ORM data access frameworks, which often lack clean APIs for tasks such as executing a Bulk Copy operation or calling a parameterized Stored Procedure.

Fluent Commander is built using .NET Standard and currently has SQL Server and Oracle implementations as separate NuGet packages.

## Getting Started

<a name="installation"></a>

### Installation

Follow the instructions below to install this NuGet package into your project:

#### .NET Core CLI

```sh
dotnet add package FluentCommander.SqlServer
```

#### Package Manager Console

```sh
Install-Package FluentCommander.SqlServer
```

### Latest releases

Latest releases and package version history can be found on [NuGet](https://www.nuget.org/packages/FluentCommander.SqlServer/).

## Build and Test

Follow the instructions below to build and test this project:

### Build

```sh
dotnet build
```

### Test

```sh
dotnet test
```

## Usage

### Bulk Copy

The Bulk Copy function is supported if you want to insert a batch of records at once from a DataTable. When Bulk Copying, SQL Server requires a mapping between source (the DataTable you want to persist) and the destination (the database on the server).

#### AutoMapping

This variation automatically maps between the source and destination. The details of this implementation can be found in the FluentCommander.Utility.AutoMapper class. This works well in circumstances where you control the source and can easily ensure the DataTable column names match the column names on the database table:

```c#
private async Task BulkCopyUsingAutoMapping(CancellationToken cancellationToken)
{
    DataTable dataTable = GetDataToInsert();

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .Mapping(mapping => mapping.UseAutoMap())
        .ExecuteAsync(cancellationToken);

    int rowCountCopied = result.RowCountCopied;
}
```

#### Partial Mapping

This variation automatically maps between the source and destination, but also allows you to specify mappings where you know the column names do not match. This works well when you want to use the auto-mapping feature, but you need to specify some additional details:

```c#
private async Task BulkCopyUsingPartialMap(CancellationToken cancellationToken)
{
    DataTable dataTable = GetDataToInsert();

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .Mapping(mapping => mapping.UsePartialMap(new ColumnMapping
        {
            ColumnMaps = new List<ColumnMap>
            {
                new ColumnMap
                {
                    Source = "SampleString",
                    Destination = "SampleVarChar"
                }
            }
        }))
        .ExecuteAsync(cancellationToken);

    int rowCountCopied = result.RowCountCopied;
}
```

#### Manual Mapping

This variation relies on you to specify mappings where you know the column names do not match. This works well when you have a significant mismatch between the column names of the source and the destination:

```c#
private async Task BulkCopyUsingMap(CancellationToken cancellationToken)
{
    DataTable dataTable = GetDataToInsert();

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .Mapping(mapping => mapping.UseMap(new ColumnMapping
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
        }))
        .ExecuteAsync(cancellationToken);

    int rowCountCopied = result.RowCountCopied;
}
```

#### Strongly-typed Mapping

When you have an entity type that reflects the shape of the table you are targeting, you can use it to drive your mappings:

```c#
private async Task BulkCopyUsingStronglyTypedMap(CancellationToken cancellationToken)
{
    DataTable dataTable = GetDataToInsert();

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy<SampleEntity>()
        .From(dataTable)
        .Into("[dbo].[SampleTable]")
        .Mapping(mapping => mapping.UseMap(entity =>
        {
            entity.Property(e => e.SampleInt).MapFrom("Column1");
            entity.Property(e => e.SampleSmallInt).MapFrom("Column2");
            entity.Property(e => e.SampleTinyInt).MapFrom("Column3");
            entity.Property(e => e.SampleBit).MapFrom("Column4");
            entity.Property(e => e.SampleDecimal).MapFrom("Column5");
            entity.Property(e => e.SampleFloat).MapFrom("Column6");
            entity.Property(e => e.SampleVarChar).MapFrom("Column7");
        }))
        .ExecuteAsync(cancellationToken);

    int rowCountCopied = result.RowCountCopied;

    Console.WriteLine("Row count copied: {0}", rowCountCopied);
}
```

#### Events

The OnRowsCopied event can be subscribed to:

```c#
private async Task BulkCopyUsingEvents(CancellationToken cancellationToken)
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
        .ExecuteAsync(cancellationToken);

    int rowCountCopied = result.RowCountCopied;

    Console.WriteLine("Row count copied: {0}", rowCountCopied);
}
```

#### All APIs

```c#
private async Task BulkCopyUsingAllApis(CancellationToken cancellationToken)
{
    DataTable dataTable = GetDataToInsert();

    BulkCopyResult result = await _databaseCommander.BuildCommand()
        .ForBulkCopy<SampleEntity>()
        .From(dataTable, DataRowState.Added)
        .Into("[dbo].[SampleTable]")
        .BatchSize(100)
        .Options(options => options.KeepNulls().CheckConstraints().TableLock(false).OpenConnectionWithoutRetry())
        .Mapping(mapping => mapping.UsePartialMap(entity =>
        {
            entity
                .Property(e => e.SampleVarChar)
                .MapFrom("SampleString");
        }))
        .Events(events => events.NotifyAfter(10).OnRowsCopied((sender, e) =>
        {
            var event = (SqlRowsCopiedEventArgs)e;

            Console.WriteLine($"Total rows copied: {event.RowsCopied}");
        }))
        .OrderHints(hints => hints.Build(entity =>
        {
            entity.Property(e => e.SampleInt).OrderByDescending();
        }))
        .Timeout(TimeSpan.FromSeconds(30))
        .ExecuteAsync(cancellationToken);

    int rowCountCopied = result.RowCountCopied;

    Console.WriteLine("Row count copied: {0}", rowCountCopied);
}
```

### Stored Procedures

This demonstrates how to build a stored procedure command using various combinations of Input, Output and Return parameters. To see the bodies of these Stored Procedures, navigate to the Resources folder and review the setup-*.sql files.

#### Input Parameters

Stored Procedures can be called with various input parameter types. This stored procedure has output, which is found on the result object:

```c#
private async Task ExecuteStoredProcedureWithAllInputTypesAndTableResult(CancellationToken cancellationToken)
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
        .ExecuteAsync(cancellationToken);

    int count = result.Count;
    bool hasData = result.HasData;
    DataTable dataTable = result.DataTable;
}
```

#### Output Parameters

Stored Procedures with output parameters need to call AddOutputParameter(), and retrieve the output from result.OutputParameters:

```c#
private async Task ExecuteStoredProcedureWithOutput(CancellationToken cancellationToken)
{
    string outputParameterName = "SampleOutputInt";
    
    StoredProcedureResult result = await _databaseCommander.BuildCommand()
        .ForStoredProcedure("[dbo].[usp_BigIntInput_IntOutput_NoResult]")
        .AddInputParameter("SampleTableID", 1)
        .AddOutputParameter(outputParameterName, DbType.Int32)
        .ExecuteAsync(cancellationToken);

    int outputParameter = result.GetOutputParameter<int>(outputParameterName);
}
```

#### InputOutput Parameters

Stored Procedures with InputOutput parameters need to call AddInputOutputParameter(), and retrieve the output from result.OutputParameters:

```c#
private async Task ExecuteStoredProcedureWithInputOutputParameter(CancellationToken cancellationToken)
{
    string inputOutputParameterName = "SampleInputOutputInt";

    StoredProcedureResult result = await _databaseCommander.BuildCommand()
        .ForStoredProcedure("[dbo].[usp_BigIntInput_IntInputOutput_TableResult]")
        .AddInputParameter("SampleTableID", 1)
        .AddInputOutputParameter(inputOutputParameterName, 1)
        .ExecuteAsync(cancellationToken);

    int inputOutputParameter = result.GetOutputParameter<int>(inputOutputParameterName);
}
```

#### Return Parameter

If a Stored Procedures has a Return parameter, the command should call .WithReturnParameter() and the result has the following method that can retrieve the return parameter: result.GetReturnParameter<T>():

```c#
private async Task ExecuteStoredProcedureWithReturnParameter(CancellationToken cancellationToken)
{
    StoredProcedureResult result = await _databaseCommander.BuildCommand()
        .ForStoredProcedure("[dbo].[usp_NoInput_NoOutput_ReturnInt]")
        .AddInputParameter("SampleTableID", 1)
        .WithReturnParameter()
        .ExecuteAsync(cancellationToken);

    int returnParameter = result.GetReturnParameter<int>();
}
```

#### Behaviors

SqlDataReader behaviors are exposed:

```c#
public async Task ExecuteStoredProcedureWithBehaviors(CancellationToken cancellationToken)
{
    StoredProcedureResult result = await _databaseCommander.BuildCommand()
        .ForStoredProcedure("[dbo].[usp_VarCharInput_NoOutput_TableResult]")
        .AddInputParameter("SampleVarChar", "Row 1", SqlDbType.VarChar, 1000)
        .Behaviors(behavior => behavior.SingleResult().KeyInfo())
        .ExecuteAsync(cancellationToken);

    DataTable dataTable = result.DataTable;
}
```

### Pagination

There are some cases where running pagination queries returned as a DataTable is convenient. This demonstrates how to build command for a SQL pagination query.

#### Assume Defaults

Several defaults are specified so the only input required is the target:

```c#
private async Task ExecutePaginationUsingMinimalInput(CancellationToken cancellationToken)
{
    PaginationResult result = await _databaseCommander.BuildCommand()
        .ForPagination()
        .From("[dbo].[SampleTable]")
        .ExecuteAsync(cancellationToken);

    int count = result.Count;
    int totalCount = result.TotalCount;
    bool hasData = result.HasData;
    DataTable dataTable = result.DataTable;
}
```

#### All Options

In this sample, all options are used:

```c#
private async Task ExecutePaginationAllSettingsAreUsed(CancellationToken cancellationToken)
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
        .ExecuteAsync(cancellationToken);

    int count = result.Count;
    int totalCount = result.TotalCount;
    bool hasData = result.HasData;
    DataTable dataTable = result.DataTable;
}
```


### Database Commander Factory

If your application needs to connect to multiple different databases, you can create instances of IDatabaseCommanders with specific database connection strings. Specify the connection strings in the appsettings.json file, inject an instance of IDatabaseCommanderFactory, and reference the connection string name when calling IDatabaseCommanderFactory.Create().

```c#
using FluentCommander;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Samples
{
    public class DatabaseCommanderFactorySample
    {
        private readonly IDatabaseCommanderFactory _databaseCommanderFactory;

        public DatabaseCommanderFactorySample(IDatabaseCommanderFactory databaseCommanderFactory)
        {
            _databaseCommanderFactory = databaseCommanderFactory;
        }
        
        public async Task DatabaseCommanderFactoryWorksWithAlternateConnectionStrings(CancellationToken cancellationToken)
        {
            // Creates an instance of an IDatabaseCommander connected to a data source using the connection string named AlternateConnectionString
            IDatabaseCommander databaseCommander = _databaseCommanderFactory.Create("AlternateConnectionString");

            // Verify the connection by running the GetServerName() command
            string serverName = await databaseCommander.GetServerNameAsync(cancellationToken);

            Console.WriteLine("Connected to: {0}", serverName);
        }
    }
}
```

### Other

There are several other commands available on the API that are often unneeded if you're using an ORM. These miscellaneous commands are available if you should find that you need them

#### SQL Query (Parameterized)

Input parameters require a database type parameter, which can often be inferred by looking at the type of the parameter value. Databases will cache the execution plan and prevent against SQL injection when you parameterize your queries like this:

```c#
private async Task ExecuteSqlWithInput(CancellationToken cancellationToken)
{
    SqlQueryResult result = await _databaseCommander.BuildCommand()
        .ForSqlQuery("SELECT * FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
        .AddInputParameter("SampleTableID", 1)
        .AddInputParameter("SampleVarChar", "Row 1")
        .Timeout(TimeSpan.FromSeconds(30))
        .ExecuteAsync(cancellationToken);

    int count = result.Count;
    bool hasData = result.HasData;
    DataTable dataTable = result.DataTable;
}
```

#### SQL Non-Query (Parameterized)

SQL Insert and Delete statements can also be parameterized:

```c#
private async Task ExecuteParameterizedInsertDeleteSql(CancellationToken cancellationToken)
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
        .ExecuteAsync(cancellationToken);

    SqlNonQueryResult deleteResult = await _databaseCommander.BuildCommand()
        .ForSqlNonQuery("DELETE FROM [dbo].[SampleTable] WHERE [SampleVarChar] = @SampleVarChar")
        .AddInputParameter("SampleVarChar", sampleVarChar)
        .ExecuteAsync(cancellationToken);

    int rowCountAffectedFromInsert = insertResult.RowCountAffected;
    int rowCountAffectedFromDelete = deleteResult.RowCountAffected;
}
```

#### SQL Scalar Query (Parameterized)

SQL Scalar queries can also be parameterized:

```c#
private async Task ExecuteScalarWithInput(CancellationToken cancellationToken)
{
    bool result = await _databaseCommander.BuildCommand()
        .ForScalar<bool>("SELECT [SampleBit] FROM [dbo].[SampleTable] WHERE [SampleTableID] = @SampleTableID AND [SampleVarChar] = @SampleVarChar")
        .AddInputParameter("SampleTableID", 1)
        .AddInputParameter("SampleVarChar", "Row 1")
        .ExecuteAsync(cancellationToken);
}
```

### More

There are several other variations not documented here. You can find a Console Application with these samples [here](https://github.com/relay-dev/fluent-commander/tree/master/samples/FluentCommander.Samples/Commands).

## Contribute

When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.
