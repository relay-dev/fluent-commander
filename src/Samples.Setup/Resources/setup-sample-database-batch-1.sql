/*
 *  ###########################################################
 *    Create the database if it does not exist
 *  ###########################################################
 */
IF (NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = 'DatabaseCommander' OR name = 'DatabaseCommander')))
BEGIN
    CREATE DATABASE [DatabaseCommander]
END