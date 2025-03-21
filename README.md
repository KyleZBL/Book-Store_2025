

<connectionStrings>
	<add name="connString" connectionString="Server=LAPTOP-VJHVLSIP;Database=Book_Store;Integrated Security=True;TrustServerCertificate=True"/>
</connection Strings>

-SQLCMD command to Connect to Microsoft SQL Server studio in Visual Studio 2022 terminal--Replace server name

sqlcmd -S LAPTOP-VJHVLSIP -d Book_Store -E


If encountering SSL issues, add the -TrustServerCertificate parameter to bypass them:
add too connection string--"TrustServerCertificate=True"
sqlcmd -S LAPTOP-VJHVLSIP -d Book_Store -E -TrustServerCertificate


---Tables---

-terminal command to list all tables in database
sqlcmd -S LAPTOP-VJHVLSIP -d Book_Store -E
1> SELECT name FROM sys.tables;
2> GO

//IDETITY(1,1) auto-increments

CREATE TABLE account (
AccountID INT IDENTITY(1,1) PRIMARY KEY,
username varchar(30) not null,
password varchar(30) not null,
isAuthor int not null,
);


CREATE TABLE Author(
AuthorID INT IDENTITY(1,1) PRIMARY KEY,
FirstName VARCHAR(20),
LastName VARCHAR(20)
);


CREATE TABLE Books (
BookID INT IDENTITY(1,1) PRIMARY KEY,
ISBN VARCHAR(20) UNIQUE, 
Title VARCHAR(100) NOT NULL, 
AuthorID INT NOT NULL,
Genre NVARCHAR(50),           
Price DECIMAL(10, 2),         
Stock INT 
pages INT NOT NULL,
publishingdate DATE NOT NULL;
);




