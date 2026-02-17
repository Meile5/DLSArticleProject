CREATE TABLE Comments(
                        CommentId nvarchar(50) NOT NULL PRIMARY KEY,
                        Text nvarchar(200) NOT NULL,

);

CREATE TABLE CommentUser(
                       Id nvarchar(50) NOT NULL PRIMARY KEY,
                       CommentId nvarchar(50) NOT NULL,
                       UserId nvarchar(50) NOT NULL,
                       FOREIGN KEY (CommentId) REFERENCES Comments(CommentId) 
);


-- Foreign key in CommentUser table should point to CommentId of Comments table
-- How to achieve this? In SQL...