CREATE TABLE Article(
                        ArticleId nvarchar(50) NOT NULL PRIMARY KEY,
                        Title nvarchar(200) NOT NULL,
                        Contents nvarchar(800) NOT NULL,
                        PublishingDate datetime NOT NULL

);

CREATE TABLE Author(
                       AuthorId nvarchar(50) NOT NULL PRIMARY KEY,
                       AuthorName nvarchar(50) NOT NULL,
                       AuthorLastName nvarchar(50) NOT NULL,

);
