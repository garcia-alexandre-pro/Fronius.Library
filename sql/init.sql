
create table Individual(
	Id int identity(1, 1) not null,
	FirstName nvarchar(50) not null,
	LastName nvarchar(50) not null,
	FullName_CPT as FirstName + ' ' + LastName,
	constraint PK_Individual primary key clustered (Id),
	constraint UQ_Individual_FullName unique (FullName)
)

create table Author(
	Id int not null,
	constraint PK_Author primary key clustered (Id),
	constraint FK_Author_Individual foreign key (Id) references Individual(Id)
)

create table Illustrator(
	Id int not null,
	constraint PK_Illustrator primary key clustered (Id),
	constraint FK_Illustrator_Individual foreign key (Id) references Individual(Id)
)

create view AuthorList as
	select Id,
		FirstName,
		LastName,
		FullName_CPT
	from Individual
	inner join Author on Author.Id = Individual.Id

create view IllustratorList as
	select Id,
		FirstName,
		LastName,
		FullName_CPT
	from Individual
	inner join Illustrator on Illustrator.Id = Individual.Id
	
create table Book(
	Id int not null,
	Title nvarchar(250) not null,
	[Year] smallint not null,
	IllustratorId int not null,
	ISBN char(17) not null,
	constraint PK_Book primary key clustered (Id),
	constraint FK_Book_Illustrator foreign key (IllustratorId) references Illustrator(Id),
	constraint UQ_Book_ISBN unique (ISBN)
)
	
create table Genre(
	Id int identity(1, 1) not null,
	[Name] nvarchar(250) not null,
	constraint PK_Genre primary key clustered (Id),
	constraint UQ_Genre_Name unique ([Name])
)
	
create table GenreByBook(
	BookId int not null,
	GenreId int not null,
	constraint PK_GenreByBook primary key clustered (BookId, GenreId),
	constraint FK_GenreByBook_BookId foreign key (BookId) references Book(Id),
	constraint FK_GenreByBook_GenreId foreign key (GenreId) references Genre(Id)
)
	
create table AuthorByBook(
	BookId int not null,
	AuthorId int not null,
	constraint PK_AuthorByBook primary key clustered (BookId, AuthorId),
	constraint FK_AuthorByBook_BookId foreign key (BookId) references Book(Id),
	constraint FK_AuthorByBook_AuthorId foreign key (AuthorId) references Author(Id)
)
