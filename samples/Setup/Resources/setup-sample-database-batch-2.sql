USE [DatabaseCommander]
GO

/*
 *  ###########################################################
 *    Create a table for the console application to use
 *  ###########################################################
 */
DROP TABLE IF EXISTS [dbo].[SampleTable]
GO

CREATE TABLE [dbo].[SampleTable](
    [SampleTableID] [bigint] IDENTITY(1,1) NOT NULL,
    [SampleInt] [int] NOT NULL DEFAULT 0,
    [SampleSmallInt] [smallint] NOT NULL DEFAULT 0,
    [SampleTinyInt] [tinyint] NOT NULL DEFAULT 0,
    [SampleBit] [bit] NOT NULL DEFAULT 0,
    [SampleDecimal] [decimal] NOT NULL DEFAULT 0,
    [SampleFloat] [float] NOT NULL DEFAULT 0,
    [SampleDateTime] [datetime] NOT NULL DEFAULT GETUTCDATE(),
    [SampleUniqueIdentifier] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
    [SampleVarChar] [varchar](max) NULL,
    [CreatedBy] [varchar](100) NOT NULL DEFAULT SYSTEM_USER,
    [CreatedDate] [datetime] NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedBy] [varchar](100) NULL,
    [ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
    [SampleTableID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO


/*
 *  ###########################################################
 *    Give the table some test data
 *  ###########################################################
 */
INSERT INTO [dbo].[SampleTable]
           ([SampleVarChar])
     VALUES
           ('Row 1')
GO


INSERT INTO [dbo].[SampleTable]
           ([SampleVarChar])
     VALUES
           ('Row 2')
GO


INSERT INTO [dbo].[SampleTable]
           ([SampleInt]
           ,[SampleSmallInt]
           ,[SampleTinyInt]
           ,[SampleBit]
           ,[SampleDecimal]
           ,[SampleFloat]
           ,[SampleVarChar])
     VALUES
           (1
           ,1
           ,1
           ,1
           ,1
           ,1
           ,'Row 3')
GO