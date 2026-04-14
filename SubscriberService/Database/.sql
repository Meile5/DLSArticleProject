CREATE TABLE Subscribers (
                             SubscriberId UNIQUEIDENTIFIER PRIMARY KEY,
                             Email NVARCHAR(255) NOT NULL,
                             SubscribedAt DATETIME2 NOT NULL,
                             IsActive BIT NOT NULL
);