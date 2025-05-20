
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
	Id int identity(1, 1) not null,
	Title nvarchar(250) not null,
	ReleaseYear smallint not null,
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
	Id int identity(1, 1) not null,
	BookId int not null,
	GenreId smallint not null,
	constraint PK_GenreByBook primary key clustered (Id),
	constraint FK_GenreByBook_BookId foreign key (BookId) references Book(Id),
	constraint FK_GenreByBook_GenreId foreign key (GenreId) references Genre(Id),
	constraint UQ_GenreByBook_BookId_GenreId unique (BookId, GenreId)
)
go
	
create table AuthorByBook(
	Id int identity(1, 1) not null,
	BookId int not null,
	AuthorId int not null,
	constraint PK_AuthorByBook primary key clustered (Id),
	constraint FK_AuthorByBook_BookId foreign key (BookId) references Book(Id),
	constraint FK_AuthorByBook_AuthorId foreign key (AuthorId) references Author(Id),
	constraint UQ_AuthorByBook_BookId_AuthorId unique (BookId,AuthorId)
)
go

alter table Book add constraint CK_Book_Year check(ReleaseYear >= 1450 and ReleaseYear <= year(getdate()))
go

alter table Book add constraint CK_Book_ISBN check(ReleaseYear >= 1970 and ISBN is not null or ReleaseYear < 1970 and (ISBN is null or ISBN not like '%[^0-9]%'))
go

create or alter procedure GetBooks @authorId int, @orderingColumn varchar(20), @orderingDirection varchar(4)
as
begin
	set nocount on;

	declare @authorExists bit = iif(@authorId is null, 1, 0)

	select @authorExists = 1 from Author where Id = @authorId

	if @authorExists = 0
	begin
		;throw 51000, 'Invalid author identifier', 1;
	end

	if @orderingColumn is not null and @orderingColumn not in ('Title', 'ReleaseYear', 'Genre')
		or @orderingDirection is not null and @orderingDirection not in ('asc', 'desc')
	begin
		;throw 51001, 'Invalid ordering parameters', 1;
	end

	declare @query varchar(max)

	set @query = 'with Authors as (
		select AuthorByBook.BookId,
			string_agg(AuthorList.FullName, '', '') as Names
		from [dbo].AuthorByBook
		inner join [dbo].AuthorList on AuthorList.Id = AuthorByBook.AuthorId
		group by AuthorByBook.BookId
	), OrderedGenres as (
		select GenreByBook.BookId,
			string_agg(Genre.[Name], '', '') within group (order by Genre.[Name]) as Names
		from [dbo].GenreByBook
		inner join [dbo].Genre on Genre.Id = GenreByBook.GenreId
		group by GenreByBook.BookId
	)
	select Id,
		Title,
		ReleaseYear,
		ISBN,
		Author as AuthorNames,
		Illustrator as IllustratorName,
		Genre as GenreNames
	from (select Book.Id,
			Title,
			ReleaseYear,
			ISBN,
			Authors.Names as Author,
			IllustratorList.FullName as Illustrator,
			OrderedGenres.Names as Genre
		from [dbo].Book
		inner join (select AuthorByBook.BookId
			from [dbo].AuthorByBook 
			where ' + cast(@authorId as varchar(9)) + ' is null or AuthorByBook.AuthorId = ' + cast(@authorId as varchar(9)) + ') as authorMatch on authorMatch.BookId = BookId
		inner join Authors on Authors.BookId = Book.Id
		inner join [dbo].IllustratorList on IllustratorList.Id = Book.IllustratorId
		inner join OrderedGenres on OrderedGenres.BookId = Book.Id) as Temp'
	+ iif(@orderingColumn is not null and @orderingDirection is not null, 'order by ' + @orderingColumn + ' ' + @orderingDirection, '')

	declare @result table(Id int not null, Title nvarchar(250) not null, ReleaseYear smallint not null, ISBN char(13) null, AuthorNames nvarchar(max), IllustratorName nvarchar(101) not null, GenreNames nvarchar(max))
	
	insert into @result
	exec(@query)

	select * from @result
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

insert into Book (Title, ReleaseYear, ISBN, IllustratorId)
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



--alter table GenreByBook drop FK_GenreByBook_BookId
--alter table AuthorByBook drop FK_AuthorByBook_BookId
--go
--alter table Book drop PK_Book
--alter table Book drop FK_Book_Illustrator
--alter table Book drop UQ_Book_ISBN
--alter table Book drop CK_Book_ReleaseYear
--alter table Book drop CK_Book_ISBN
--go
--alter table Book drop column Id
--alter table Book add Id int identity(1, 1) not null
--alter table Book drop column Title
--alter table Book add Title nvarchar(250) not null
--alter table Book drop column ReleaseYear
--alter table Book add ReleaseYear smallint not null
--alter table Book drop column IllustratorId
--alter table Book add IllustratorId int not null
--alter table Book drop column ISBN
--alter table Book add ISBN char(13) null
--go
--alter table Book add constraint PK_Book primary key clustered (Id)
--alter table Book add constraint FK_Book_Illustrator foreign key (IllustratorId) references Illustrator(Id)
--alter table Book add constraint UQ_Book_ISBN unique (ISBN)
--go
--alter table AuthorByBook add constraint FK_AuthorByBook_BookId foreign key (BookId) references Book(Id)
--alter table GenreByBook add constraint FK_GenreByBook_BookId foreign key (BookId) references Book(Id)
--go
--alter table Book add constraint CK_Book_ReleaseYear check(ReleaseYear >= 1450 and ReleaseYear <= year(getdate()))
--alter table Book add constraint CK_Book_ISBN check(ReleaseYear >= 1970 and ISBN is not null or ReleaseYear < 1970 and (ISBN is null or ISBN not like '%[^0-9]%'))
--go
