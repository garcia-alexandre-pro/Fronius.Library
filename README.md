# Fronius.Library
Fronius technical test

## Technical Test

The test consists of creating a .Net REST API that allows:

- Creating books.
- Listing books.

## What is a book?

A book is characterized by:

- A title.
- A publication year.
- A list of authors.
- An illustrator.
- A list of genres (possible values: `Action`, `Comedy`, `Drama`, `Horror`, and `Science Fiction`).
- An ISBN number.

## What is an author?

An author is characterized by:

- A last name.
- A first name.

## What is an illustrator?

An illustrator is characterized by:

- A last name.
- A first name.

## Creating a book

Before persisting the book, it must be validated.

Book validation rules:

- A book cannot have a publication year in the future.
- A book cannot have been written before 1450.
- The title is mandatory.
- The illustrator is mandatory.
- The book's ISBN consists of 13 digits unless the book dates before 1970 (the creation date of the ISBN).
- A book cannot have the same combination of title, publication year, and author as another book already created.
- Two books cannot have the same ISBN.

## Listing books

The API must allow:

- Listing books by a given author.
- Sorting books by title, release year, or genre.
- Displaying all properties of a book. The list of authors should appear in the form `{FirstName} {LastName}` (e.g., "Just TheWhite").

## Storage

The data must be persisted in SQL Server via Entity Framework.

The authors' table must be initialized with the following authors:

- Stephen King.
- Marguerite Duras.
- Isaac Asimov.

The illustrators' table must be initialized with the following illustrators:

- Norman Rockwell.
- Gustave Doré.
- Beya Rebaï.

## Unit Tests

The book validation rules must be unit tested (with xUnit).

## Notes

- No authentication is required.
- No user interface is required.

## Use of Libraries

Like any developer, we don't like to reinvent the wheel and appreciate using various libraries as needed.

However, this test allows us to evaluate how you approach and solve a problem. Therefore, we prefer that you limit the use of libraries in the application (you can indicate the libraries you would have liked to use if you wish). This rule does not apply to unit tests, for which you can use the libraries of your choice, in addition to xUnit.

## Evaluation Criteria

You will be evaluated on the maintainability of your code (readability, extensibility, modularization, homogeneity, etc.), as well as your ability to meet the specifications, even if it means making decisions and justifying them if some points seem unclear to you.