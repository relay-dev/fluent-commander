USE [DatabaseCommander]
GO

/*
 *  ###########################################################
 *    Create stored procedures for permutations of samples
 *  ###########################################################
 */
DROP PROCEDURE IF EXISTS [dbo].[usp_NoInput_NoOutput_NoResult]
GO

CREATE PROCEDURE [dbo].[usp_NoInput_NoOutput_NoResult]
AS
BEGIN
    DECLARE @SampleVariable INT = 100
END
GO

DROP PROCEDURE IF EXISTS [dbo].[usp_NoInput_NoOutput_TableResult]
GO

CREATE PROCEDURE [dbo].[usp_NoInput_NoOutput_TableResult]
AS
BEGIN
    SELECT *
    FROM [DatabaseCommander].[dbo].[SampleTable]
END
GO

DROP PROCEDURE IF EXISTS [dbo].[usp_AllInputTypes_NoOutput_TableResult]
GO

CREATE PROCEDURE [dbo].[usp_AllInputTypes_NoOutput_TableResult]
      @SampleTableID [bigint]
    , @SampleInt [int]
    , @SampleSmallInt [smallint]
    , @SampleTinyInt [tinyint]
    , @SampleBit [bit]
    , @SampleDecimal [decimal]
    , @SampleFloat [float]
    , @SampleDateTime [datetime]
    , @SampleUniqueIdentifier [uniqueidentifier]
    , @SampleVarChar [varchar](1000)
AS
BEGIN
    SELECT *
    FROM [DatabaseCommander].[dbo].[SampleTable]
    WHERE 1 = 1
        AND SampleTableID = @SampleTableID
        AND SampleInt = @SampleInt
        AND SampleSmallInt = @SampleSmallInt
        AND SampleTinyInt = @SampleTinyInt
        AND SampleBit = @SampleBit
        AND SampleDecimal = @SampleDecimal
        AND SampleFloat = @SampleFloat
        -- For the sake of a sample, take the DateTime and Guid as input to demonstrate how to pass all parameter types, but don't apply the filter since these fields will not be constant
        --AND SampleDateTime = @SampleDateTime
        --AND SampleUniqueIdentifier = @SampleUniqueIdentifier
        AND SampleVarChar = @SampleVarChar
END
GO

DROP PROCEDURE IF EXISTS [dbo].[usp_BigIntInput_IntOutput_NoResult]
GO

CREATE PROCEDURE [dbo].[usp_BigIntInput_IntOutput_NoResult]
      @SampleTableID [bigint]
    , @SampleOutputInt [int] OUTPUT
AS
BEGIN
    SET @SampleOutputInt = 100
END
GO

DROP PROCEDURE IF EXISTS [dbo].[usp_BigIntInput_MultipleOutput_TableResult]
GO

CREATE PROCEDURE [dbo].[usp_BigIntInput_MultipleOutput_TableResult]
      @SampleTableID [bigint]
    , @SampleOutputInt [int] OUTPUT
    , @SampleOutputVarChar [varchar] OUTPUT
AS
BEGIN
    SET @SampleOutputInt = 100
    SET @SampleOutputVarChar = 'Output'

    SELECT *
    FROM [DatabaseCommander].[dbo].[SampleTable]
    WHERE SampleTableID = @SampleTableID
END
GO

DROP PROCEDURE IF EXISTS [dbo].[usp_BigIntInput_IntInputOutput_TableResult]
GO

CREATE PROCEDURE [dbo].[usp_BigIntInput_IntInputOutput_TableResult]
      @SampleTableID [bigint]
    , @SampleInputOutputInt [int] OUTPUT
AS
BEGIN
    SET @SampleInputOutputInt = 100

    SELECT *
    FROM [DatabaseCommander].[dbo].[SampleTable]
    WHERE SampleTableID = @SampleTableID
END
GO

DROP PROCEDURE IF EXISTS [dbo].[usp_BigIntInput_VarCharOutput_TableResult]
GO

CREATE PROCEDURE [dbo].[usp_BigIntInput_VarCharOutput_TableResult]
      @SampleTableID [bigint]
    , @SampleInputOutputVarChar [varchar](50) OUTPUT
AS
BEGIN
    SET @SampleInputOutputVarChar = 'Hello world'

    SELECT *
    FROM [DatabaseCommander].[dbo].[SampleTable]
    WHERE SampleTableID = @SampleTableID
END
GO

DROP PROCEDURE IF EXISTS [dbo].[usp_NoInput_NoOutput_ReturnInt]
GO

CREATE PROCEDURE [dbo].[usp_NoInput_NoOutput_ReturnInt]
      @SampleTableID [bigint]
AS
BEGIN
    DECLARE @SampleReturnInt AS INT = 100

    RETURN @SampleReturnInt
END
GO

DROP PROCEDURE IF EXISTS [dbo].[usp_Input_Output_ReturnBigInt]
GO

CREATE PROCEDURE [dbo].[usp_Input_Output_ReturnBigInt]
      @SampleTableID [bigint]
    , @SampleOutputBigInt [bigint] OUTPUT
AS
BEGIN
    IF EXISTS(SELECT * FROM [DatabaseCommander].[dbo].[SampleTable] WHERE SampleTableID > @SampleTableID AND SampleTableID < @SampleOutputBigInt)
    BEGIN                
        SELECT @SampleOutputBigInt = COUNT(1)
        FROM [DatabaseCommander].[dbo].[SampleTable]
        RETURN 1
    END
    ELSE
    BEGIN
        RETURN -1
    END
END
GO

DROP PROCEDURE IF EXISTS [dbo].[usp_OptionalInput_NoOutput_ReturnInt]
GO

CREATE PROCEDURE [dbo].[usp_OptionalInput_NoOutput_ReturnInt]
      @SampleTableID [bigint]
    , @SampleInputInt [int] = 100
AS
BEGIN
    RETURN @SampleInputInt
END
GO

/*
 *  ###########################################################
 *    Create functions for permutations of samples
 *  ###########################################################
 */
DROP FUNCTION IF EXISTS [dbo].[fun_NoInput_NoReturn]
GO

CREATE FUNCTION [dbo].[fun_NoInput_NoReturn]()
RETURNS INT
AS
BEGIN
    RETURN 1
END
GO

DROP FUNCTION IF EXISTS [dbo].[fun_VarCharInput_BitReturn]
GO

CREATE FUNCTION [dbo].[fun_VarCharInput_BitReturn]
(
    @SampleInputVarChar VARCHAR(100)
)
RETURNS BIT
AS 
BEGIN
    DECLARE @SampleBit BIT = 1

    RETURN @SampleBit
END
GO
