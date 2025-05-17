
use [Library]
go

create table Individual(
	Id int identity(1, 1) not null,
	FirstName nvarchar(50) not null,
	LastName nvarchar(50) not null,
	FullName_CPT as FirstName + ' ' + LastName,
	constraint PK_Individual primary key clustered (Id),
	constraint UQ_Individual_FullName_CPT unique (FullName_CPT)
)
go

create table Author(
	Id int not null,
	constraint PK_Author primary key clustered (Id),
	constraint FK_Author_Individual foreign key (Id) references Individual(Id)
)
go

create table Illustrator(
	Id int not null,
	constraint PK_Illustrator primary key clustered (Id),
	constraint FK_Illustrator_Individual foreign key (Id) references Individual(Id)
)
go

create view AuthorList as
	select Individual.Id,
		FirstName,
		LastName,
		FullName_CPT as FullName
	from Individual
	inner join Author on Author.Id = Individual.Id
go

create view IllustratorList as
	select Individual.Id,
		FirstName,
		LastName,
		FullName_CPT as FullName
	from Individual
	inner join Illustrator on Illustrator.Id = Individual.Id
go

create table Book(
	Id int not null,
	Title nvarchar(250) not null,
	[Year] smallint not null, -- release year
	IllustratorId int not null,
	ISBN char(13) null,
	constraint PK_Book primary key clustered (Id),
	constraint FK_Book_Illustrator foreign key (IllustratorId) references Illustrator(Id),
	constraint UQ_Book_ISBN unique (ISBN)
)
go

create table Genre(
	Id int identity(1, 1) not null,
	[Name] nvarchar(50) not null,
	constraint PK_Genre primary key clustered (Id),
	constraint UQ_Genre_Name unique ([Name])
)
go
	
create table GenreByBook(
	BookId int not null,
	GenreId smallint not null,
	constraint PK_GenreByBook primary key clustered (BookId, GenreId),
	constraint FK_GenreByBook_BookId foreign key (BookId) references Book(Id),
	constraint FK_GenreByBook_GenreId foreign key (GenreId) references Genre(Id)
)
go
	
create table AuthorByBook(
	BookId int not null,
	AuthorId int not null,
	constraint PK_AuthorByBook primary key clustered (BookId, AuthorId),
	constraint FK_AuthorByBook_BookId foreign key (BookId) references Book(Id),
	constraint FK_AuthorByBook_AuthorId foreign key (AuthorId) references Author(Id)
)
go

alter table Book add constraint CK_Book_Year check([Year] >= 1450)
go

alter table Book add constraint CK_Book_ISBN check(([Year] >= 1970 and ISBN is not null or [Year] < 1970 and ISBN is null) 
	and ISBN not like '%[^0-9]%')
go

create or alter procedure GetBooks @authorId int, @orderingColumn varchar(20), @orderingDirection varchar(4)
as
begin
	select Book.Id,
		Title,
		[Year],
		ISBN,
		Authors.Names as AuthorNames,
		IllustratorList.FullName as IllustratorName,
		Genres.Names as GenreNames
	from [dbo].Book
	inner join (select AuthorByBook.BookId
		from [dbo].AuthorByBook 
		where @authorId is not null or AuthorByBook.AuthorId = @authorId) as authorMatch on authorMatch.BookId = BookId
	inner join (select AuthorByBook.BookId,
			string_agg(AuthorList.FullName, ', ') as Names
		from [dbo].AuthorByBook
		inner join [dbo].AuthorList on AuthorList.FullName = AuthorByBook.AuthorId
		group by AuthorByBook.BookId) as Authors on Authors.BookId = Book.Id
	inner join [dbo].IllustratorList on IllustratorList.Id = Book.IllustratorId
	inner join (select GenreByBook.BookId,
			string_agg(Genre.[Name], ', ') as Names
		from [dbo].GenreByBook
		inner join [dbo].Genre on Genre.Id = GenreByBook.GenreId
		group by GenreByBook.BookId) as Genres on Genres.BookId = Book.Id
	--order by 
end
go



insert into Individual (FirstName, LastName)
values('Stephen', 'King'),
	('Marguerite', 'Duras'),
	('Isaac', 'Asimov'),
	('Norman', 'Rockwell'),
	('Gustave', 'Doré'),
	('Beya', 'Rebaï')

insert into Author (Id)
values(1), (2), (3)

insert into Illustrator (Id)
values(4), (5), (6)
go

insert into Genre ([Name])
values('Action'), ('Comedy'), ('Drama'), ('Horror'), ('Science Fiction')
go

insert into Book (Title, [Year], ISBN, IllustratorId)
values('Les Robots', 1950, null, 5),
	('Christine', 1983, '9782226019431', 6)
go

insert into AuthorByBook(BookId, AuthorId)
values(1, 3),
	(2, 1)
go

insert into GenreByBook(BookId, GenreId)
values(1, 5),
	(2, 4)
go
