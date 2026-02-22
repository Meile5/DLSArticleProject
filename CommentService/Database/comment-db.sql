
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'comments-db-real')
    BEGIN
        CREATE DATABASE [comments-db-real];
    END
GO

USE [comments-db-real];
GO

CREATE TABLE Comments(
                         CommentId nvarchar(50) NOT NULL PRIMARY KEY,
                         ArticleId nvarchar(50) NOT NULL,
                         Text nvarchar(200) NOT NULL

);

CREATE TABLE CommentUser(
                            Id nvarchar(50) NOT NULL PRIMARY KEY,
                            CommentId nvarchar(50) NOT NULL,
                            UserId nvarchar(50) NOT NULL,
                            FOREIGN KEY (CommentId) REFERENCES Comments(CommentId)
);