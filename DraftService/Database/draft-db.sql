-- Create database
CREATE DATABASE DraftServiceDb;
GO

USE DraftServiceDb;
GO

-- Draft status lookup
CREATE TABLE DraftStatus (
                             Id INT PRIMARY KEY,
                             Name NVARCHAR(50) NOT NULL
);

INSERT INTO DraftStatus (Id, Name) VALUES
                                       (1, 'Draft'),
                                       (2, 'PendingReview'),
                                       (3, 'Approved'),
                                       (4, 'Rejected');

-- Drafts table
CREATE TABLE Drafts (
                        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                        Title NVARCHAR(255) NOT NULL,
                        Content NVARCHAR(MAX) NOT NULL,
                        AuthorId NVARCHAR(100) NOT NULL,

                        StatusId INT NOT NULL,

                        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                        UpdatedAt DATETIME2 NULL,

                        CONSTRAINT FK_Drafts_Status
                            FOREIGN KEY (StatusId)
                                REFERENCES DraftStatus(Id)
);

-- Optional index for faster queries by author
CREATE INDEX IX_Drafts_AuthorId
    ON Drafts (AuthorId);

-- Optional index for status queries
CREATE INDEX IX_Drafts_StatusId
    ON Drafts (StatusId);