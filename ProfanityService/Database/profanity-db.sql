create database if not exists profanity-db-real;
use profanity-db-real;

CREATE TABLE Words(
                         WordId nvarchar(50) NOT NULL PRIMARY KEY,
                         Word nvarchar(50) NOT NULL,

);




-- The correct sql to run is:
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'profanity-db-real')
BEGIN
    CREATE DATABASE [profanity-db-real];
END
GO

USE [profanity-db-real];
GO

CREATE TABLE Words (
                       WordId nvarchar(50) NOT NULL PRIMARY KEY,
                       Word   nvarchar(50) NOT NULL
);